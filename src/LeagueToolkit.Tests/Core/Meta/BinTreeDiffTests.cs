using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Meta.Properties;

namespace LeagueToolkit.Tests.Core.Meta;

public class BinTreeDiffTests
{
    [Fact]
    public void Should_Return_Empty_Diff_For_Equal_Trees()
    {
        BinTree oldTree = CreateTree(new BinTreeString(0x10, "same"));
        BinTree newTree = CreateTree(new BinTreeString(0x10, "same"));

        BinTreeDiff diff = oldTree.Diff(newTree);

        Assert.True(diff.IsEmpty);
    }

    [Fact]
    public void Should_Report_Added_And_Removed_Objects_In_Hash_Order()
    {
        BinTree oldTree = new(new[] { new BinTreeObject(0x20, 1, Array.Empty<BinTreeProperty>()) }, Array.Empty<string>());
        BinTree newTree = new(new[] { new BinTreeObject(0x10, 1, Array.Empty<BinTreeProperty>()) }, Array.Empty<string>());

        BinTreeDiff diff = oldTree.Diff(newTree);

        Assert.Collection(
            diff.Objects,
            change =>
            {
                Assert.Equal(BinTreeDiffKind.Added, change.Kind);
                Assert.Equal(0x10u, change.PathHash);
            },
            change =>
            {
                Assert.Equal(BinTreeDiffKind.Removed, change.Kind);
                Assert.Equal(0x20u, change.PathHash);
            }
        );
    }

    [Fact]
    public void Should_Report_Changed_Object_Class()
    {
        BinTree oldTree = new(new[] { new BinTreeObject(1, 0x10, Array.Empty<BinTreeProperty>()) }, Array.Empty<string>());
        BinTree newTree = new(new[] { new BinTreeObject(1, 0x20, Array.Empty<BinTreeProperty>()) }, Array.Empty<string>());

        ModifiedBinTreeObjectDiff change = Assert.IsType<ModifiedBinTreeObjectDiff>(
            Assert.Single(oldTree.Diff(newTree).Objects)
        );

        Assert.Equal(BinTreeDiffKind.Modified, change.Kind);
        Assert.Empty(change.Properties);
        Assert.Equal(0x10u, change.OldObject.ClassHash);
        Assert.Equal(0x20u, change.NewObject.ClassHash);
    }

    [Fact]
    public void Should_Report_Added_Removed_And_Modified_Properties()
    {
        BinTree oldTree = CreateTree(
            new BinTreeString(0x10, "old"),
            new BinTreeU32(0x30, 1)
        );
        BinTree newTree = CreateTree(
            new BinTreeString(0x10, "new"),
            new BinTreeBool(0x20, true)
        );

        ModifiedBinTreeObjectDiff objectChange = Assert.IsType<ModifiedBinTreeObjectDiff>(
            Assert.Single(oldTree.Diff(newTree).Objects)
        );

        Assert.Collection(
            objectChange.Properties,
            change => Assert.Equal(BinTreeDiffKind.Modified, change.Kind),
            change => Assert.Equal(BinTreeDiffKind.Added, change.Kind),
            change => Assert.Equal(BinTreeDiffKind.Removed, change.Kind)
        );
    }

    [Fact]
    public void Should_Descend_Into_Struct_Properties()
    {
        BinTree oldTree = CreateTree(new BinTreeStruct(0x10, 0x100, new[] { new BinTreeString(0x20, "old") }));
        BinTree newTree = CreateTree(new BinTreeStruct(0x10, 0x100, new[] { new BinTreeString(0x20, "new") }));

        ModifiedBinTreeObjectDiff objectChange = Assert.IsType<ModifiedBinTreeObjectDiff>(
            Assert.Single(oldTree.Diff(newTree).Objects)
        );
        BinTreePropertyDiff change = Assert.Single(objectChange.Properties);

        Assert.Equal(new uint[] { 0x10, 0x20 }, change.Path);
        Assert.Equal(BinTreeDiffKind.Modified, change.Kind);
    }

    [Fact]
    public void Should_Report_Container_ElementType_Changes()
    {
        BinTree oldTree = CreateTree(new BinTreeContainer(0x10, BinPropertyType.U8, Array.Empty<BinTreeU8>()));
        BinTree newTree = CreateTree(new BinTreeContainer(0x10, BinPropertyType.U32, Array.Empty<BinTreeU32>()));

        ModifiedBinTreeObjectDiff objectChange = Assert.IsType<ModifiedBinTreeObjectDiff>(
            Assert.Single(oldTree.Diff(newTree).Objects)
        );
        ModifiedBinTreePropertyDiff propertyChange = Assert.IsType<ModifiedBinTreePropertyDiff>(
            Assert.Single(objectChange.Properties)
        );

        Assert.Equal(BinPropertyType.U8, ((BinTreeContainer)propertyChange.OldProperty).ElementType);
        Assert.Equal(BinPropertyType.U32, ((BinTreeContainer)propertyChange.NewProperty).ElementType);
    }

    private static BinTree CreateTree(params BinTreeProperty[] properties) =>
        new(new[] { new BinTreeObject(1, 2, properties) }, Array.Empty<string>());
}
