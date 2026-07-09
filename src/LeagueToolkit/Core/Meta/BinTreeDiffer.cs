using LeagueToolkit.Core.Meta.Properties;

namespace LeagueToolkit.Core.Meta;

internal static class BinTreeDiffer
{
    public static BinTreeDiff Diff(BinTree oldTree, BinTree newTree)
    {
        List<BinTreeObjectDiff> objectDiffs = new();
        foreach (uint pathHash in oldTree.Objects.Keys.Union(newTree.Objects.Keys).Order())
        {
            bool hasOldObject = oldTree.Objects.TryGetValue(pathHash, out BinTreeObject oldObject);
            bool hasNewObject = newTree.Objects.TryGetValue(pathHash, out BinTreeObject newObject);

            if (!hasOldObject)
            {
                objectDiffs.Add(new AddedBinTreeObjectDiff(newObject));
                continue;
            }

            if (!hasNewObject)
            {
                objectDiffs.Add(new RemovedBinTreeObjectDiff(oldObject));
                continue;
            }

            List<BinTreePropertyDiff> propertyDiffs = new();
            DiffProperties(oldObject.Properties, newObject.Properties, Array.Empty<uint>(), propertyDiffs);

            if (oldObject.ClassHash != newObject.ClassHash || propertyDiffs.Count is not 0)
                objectDiffs.Add(new ModifiedBinTreeObjectDiff(oldObject, newObject, propertyDiffs));
        }

        return new BinTreeDiff(objectDiffs);
    }

    private static void DiffProperties(
        IReadOnlyDictionary<uint, BinTreeProperty> oldProperties,
        IReadOnlyDictionary<uint, BinTreeProperty> newProperties,
        IReadOnlyList<uint> parentPath,
        ICollection<BinTreePropertyDiff> differences
    )
    {
        foreach (uint nameHash in oldProperties.Keys.Union(newProperties.Keys).Order())
        {
            bool hasOldProperty = oldProperties.TryGetValue(nameHash, out BinTreeProperty oldProperty);
            bool hasNewProperty = newProperties.TryGetValue(nameHash, out BinTreeProperty newProperty);
            uint[] path = parentPath.Append(nameHash).ToArray();

            if (!hasOldProperty)
            {
                differences.Add(new AddedBinTreePropertyDiff(path, newProperty));
                continue;
            }

            if (!hasNewProperty)
            {
                differences.Add(new RemovedBinTreePropertyDiff(path, oldProperty));
                continue;
            }

            if (oldProperty.Equals(newProperty))
                continue;

            if (oldProperty.Type == newProperty.Type
                && oldProperty is BinTreeStruct oldStruct
                && newProperty is BinTreeStruct newStruct
                && oldStruct.ClassHash == newStruct.ClassHash)
            {
                DiffProperties(oldStruct.Properties, newStruct.Properties, path, differences);
                continue;
            }

            differences.Add(new ModifiedBinTreePropertyDiff(path, oldProperty, newProperty));
        }
    }
}
