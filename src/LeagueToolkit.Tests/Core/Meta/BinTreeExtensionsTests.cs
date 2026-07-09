using Xunit;
using LeagueToolkit.Core.Meta;
using LeagueToolkit.Core.Meta.Properties;
using LeagueToolkit.Hashing;
using System.Collections.Generic;

namespace LeagueToolkit.Tests.Core.Meta;

public class BinTreeExtensionsTests
{
    [Fact]
    public void TestPathingTraversals()
    {
        // 1. Setup simple properties
        var floatProp = new BinTreeF32(Fnv1a.HashLower("myFloat"), 3.14f);
        var intProp = new BinTreeI32(Fnv1a.HashLower("myInt"), 42);

        // 2. Setup Struct property
        var structProps = new List<BinTreeProperty> { floatProp, intProp };
        var structProp = new BinTreeStruct(
            Fnv1a.HashLower("myStruct"),
            Fnv1a.HashLower("MyStructClass"),
            structProps
        );

        // 3. Setup Container property
        var element1 = new BinTreeI32(0, 100);
        var element2 = new BinTreeI32(0, 200);
        var containerProp = new BinTreeContainer(
            Fnv1a.HashLower("myList"),
            BinPropertyType.I32,
            new[] { element1, element2 }
        );

        // 4. Setup Optional property
        var optionalInner = new BinTreeI32(Fnv1a.HashLower("optValue"), 999);
        var optionalProp = new BinTreeOptional(
            Fnv1a.HashLower("myOptional"),
            optionalInner
        );

        // 5. Setup Map property
        var key1 = new BinTreeString(0, "keyOne");
        var val1 = new BinTreeI32(0, 111);
        var key2 = new BinTreeString(0, "keyTwo");
        var val2 = new BinTreeI32(0, 222);
        var mapPairs = new List<KeyValuePair<BinTreeProperty, BinTreeProperty>>
        {
            KeyValuePair.Create<BinTreeProperty, BinTreeProperty>(key1, val1),
            KeyValuePair.Create<BinTreeProperty, BinTreeProperty>(key2, val2)
        };
        var mapProp = new BinTreeMap(
            Fnv1a.HashLower("myMap"),
            BinPropertyType.String,
            BinPropertyType.I32,
            mapPairs
        );

        // Assemble Object 1: "Characters/Ahri/Skins/Skin0"
        var objProperties = new List<BinTreeProperty>
        {
            structProp,
            containerProp,
            optionalProp,
            mapProp
        };
        var binObj = new BinTreeObject(
            "Characters/Ahri/Skins/Skin0",
            "SkinData",
            objProperties
        );

        // Assemble BinTree
        var binTree = new BinTree(new[] { binObj }, new string[0]);

        // --- TEST Traversals ---

        // Test Object Direct Traversal (should fail because object itself is not a property)
        Assert.False(binTree.TryGetProperty("Characters/Ahri/Skins/Skin0", out _));

        // Test basic Struct lookup
        Assert.True(binTree.TryGetProperty("Characters/Ahri/Skins/Skin0/myStruct/myFloat", out var res1));
        var floatVal = Assert.IsType<BinTreeF32>(res1);
        Assert.Equal(3.14f, floatVal.Value);

        // Test Struct lookup starting from the Object directly
        var resObj = binObj.GetProperty("myStruct/myInt");
        var intVal = Assert.IsType<BinTreeI32>(resObj);
        Assert.Equal(42, intVal.Value);

        // Test Container index lookup
        var resContainer = binTree.GetProperty("Characters/Ahri/Skins/Skin0/myList[1]");
        var containerVal = Assert.IsType<BinTreeI32>(resContainer);
        Assert.Equal(200, containerVal.Value);

        // Test Optional direct name match lookup
        var resOpt1 = binTree.GetProperty("Characters/Ahri/Skins/Skin0/myOptional/optValue");
        var optVal1 = Assert.IsType<BinTreeI32>(resOpt1);
        Assert.Equal(999, optVal1.Value);

        // Test Map lookup by string key
        var resMap1 = binTree.GetProperty("Characters/Ahri/Skins/Skin0/myMap/keyOne");
        var mapVal1 = Assert.IsType<BinTreeI32>(resMap1);
        Assert.Equal(111, mapVal1.Value);

        // Test Map lookup by hash key string (keyTwo)
        var resMap2 = binTree.GetProperty("Characters/Ahri/Skins/Skin0/myMap/keyTwo");
        var mapVal2 = Assert.IsType<BinTreeI32>(resMap2);
        Assert.Equal(222, mapVal2.Value);

        // Test hexadecimal hash fallback lookup
        string hexStructHash = "0x" + Fnv1a.HashLower("myStruct").ToString("X");
        string hexFloatHash = "0x" + Fnv1a.HashLower("myFloat").ToString("X");
        var resHex = binTree.GetProperty($"Characters/Ahri/Skins/Skin0/{hexStructHash}/{hexFloatHash}");
        var floatValHex = Assert.IsType<BinTreeF32>(resHex);
        Assert.Equal(3.14f, floatValHex.Value);

        // Test non-existing path
        Assert.Null(binTree.GetProperty("Characters/Ahri/Skins/Skin0/nonExisting"));
        Assert.Null(binTree.GetProperty("Characters/Ahri/Skins/Skin0/myStruct/nonExisting"));
        Assert.Null(binTree.GetProperty("Characters/Ahri/Skins/Skin0/myList[99]"));
    }

    [Fact]
    public void TestRealBinFileSerialization()
    {
        // 1. Create a tree with a struct and simple types
        var floatProp = new BinTreeF32(Fnv1a.HashLower("myFloat"), 9.99f);
        var structProps = new List<BinTreeProperty> { floatProp };
        var structProp = new BinTreeStruct(
            Fnv1a.HashLower("myStruct"),
            Fnv1a.HashLower("MyStructClass"),
            structProps
        );

        var objProperties = new List<BinTreeProperty> { structProp };
        var binObj = new BinTreeObject(
            "Characters/Ahri/Skins/Skin0",
            "SkinData",
            objProperties
        );
        var originalTree = new BinTree(new[] { binObj }, new string[0]);

        // 2. Serialize to binary stream (mimicking writing a real .bin file)
        using var ms = new System.IO.MemoryStream();
        originalTree.Write(ms);
        ms.Position = 0;

        // 3. Deserialize back (mimicking reading a real .bin file)
        var loadedTree = new BinTree(ms);

        // 4. Test pathing on the loaded tree
        Assert.True(loadedTree.TryGetProperty("Characters/Ahri/Skins/Skin0/myStruct/myFloat", out var res));
        var floatVal = Assert.IsType<BinTreeF32>(res);
        Assert.Equal(9.99f, floatVal.Value);
    }
}
