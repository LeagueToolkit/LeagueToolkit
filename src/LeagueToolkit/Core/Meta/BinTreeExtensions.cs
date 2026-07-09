using LeagueToolkit.Core.Meta.Properties;
using LeagueToolkit.Hashing;

namespace LeagueToolkit.Core.Meta;

/// <summary>
/// Provides extension methods for traversing and querying <see cref="BinTree"/> properties by path.
/// </summary>
public static class BinTreeExtensions
{
    /// <summary>
    /// Resolves a property path starting from the root of a <see cref="BinTree"/> without allocating string arrays.
    /// </summary>
    /// <param name="binTree">The property bin tree.</param>
    /// <param name="path">The path to the property (e.g., "Characters/Ahri/Skins/Skin0/mResourceResolver/vfx").</param>
    /// <returns>The resolved property, or <see langword="null"/> if not found.</returns>
    public static BinTreeProperty GetProperty(this BinTree binTree, string path)
    {
        return binTree.TryGetProperty(path, out var property) ? property : null;
    }

    /// <summary>
    /// Tries to resolve a property path starting from the root of a <see cref="BinTree"/> without allocating string arrays.
    /// </summary>
    /// <param name="binTree">The property bin tree.</param>
    /// <param name="path">The path to the property.</param>
    /// <param name="property">The resolved property if found; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the property was successfully resolved; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetProperty(this BinTree binTree, string path, out BinTreeProperty property)
    {
        property = null;
        if (string.IsNullOrEmpty(path))
            return false;

        ReadOnlySpan<char> pathSpan = path.AsSpan();

        // 1. Find the root object by checking incremental path prefixes.
        // League of Legends object names can contain slashes, so we search for a matching prefix.
        BinTreeObject rootObject = null;
        int remainingIndex = -1;
        int currentLength = 0;

        while (currentLength < pathSpan.Length)
        {
            // Find next separator
            int nextSep = pathSpan.Slice(currentLength).IndexOfAny('/', '\\');
            if (nextSep == -1)
            {
                currentLength = pathSpan.Length;
            }
            else
            {
                currentLength += nextSep;
            }

            ReadOnlySpan<char> prefixSpan = pathSpan.Slice(0, currentLength);
            uint hash = HashLower(prefixSpan);
            if (binTree.Objects.TryGetValue(hash, out rootObject))
            {
                remainingIndex = currentLength;
                if (remainingIndex < pathSpan.Length && (pathSpan[remainingIndex] == '/' || pathSpan[remainingIndex] == '\\'))
                {
                    remainingIndex++; // Skip the slash
                }
                break;
            }

            if (nextSep == -1)
                break;
            currentLength += 1; // Skip the separator for next loop
        }

        // If no prefix matched, check if the first segment is a raw hex/decimal hash representation
        if (rootObject == null)
        {
            int firstSep = pathSpan.IndexOfAny('/', '\\');
            ReadOnlySpan<char> firstSegment = firstSep == -1 ? pathSpan : pathSpan.Slice(0, firstSep);

            if (firstSegment.StartsWith("0x", StringComparison.OrdinalIgnoreCase) &&
                uint.TryParse(firstSegment.Slice(2), System.Globalization.NumberStyles.HexNumber, null, out uint hash))
            {
                if (binTree.Objects.TryGetValue(hash, out rootObject))
                    remainingIndex = firstSep == -1 ? pathSpan.Length : firstSep + 1;
            }
            else if (uint.TryParse(firstSegment, out uint rawHash))
            {
                if (binTree.Objects.TryGetValue(rawHash, out rootObject))
                    remainingIndex = firstSep == -1 ? pathSpan.Length : firstSep + 1;
            }
        }

        if (rootObject == null)
            return false;

        // If the path refers exactly to the object and doesn't specify any property
        if (remainingIndex >= pathSpan.Length)
            return false;

        // 2. Traverse down from the root object
        return TryGetPropertyFromObject(rootObject, pathSpan.Slice(remainingIndex), out property);
    }

    /// <summary>
    /// Resolves a property path starting from a specific <see cref="BinTreeObject"/>.
    /// </summary>
    /// <param name="obj">The object to start traversing from.</param>
    /// <param name="path">The path to the property (e.g., "mResourceResolver/vfx").</param>
    /// <returns>The resolved property, or <see langword="null"/> if not found.</returns>
    public static BinTreeProperty GetProperty(this BinTreeObject obj, string path)
    {
        return obj.TryGetProperty(path, out var property) ? property : null;
    }

    /// <summary>
    /// Tries to resolve a property path starting from a specific <see cref="BinTreeObject"/>.
    /// </summary>
    /// <param name="obj">The object to start traversing from.</param>
    /// <param name="path">The path to the property.</param>
    /// <param name="property">The resolved property if found; otherwise, <see langword="null"/>.</param>
    /// <returns><see langword="true"/> if the property was successfully resolved; otherwise, <see langword="false"/>.</returns>
    public static bool TryGetProperty(this BinTreeObject obj, string path, out BinTreeProperty property)
    {
        property = null;
        if (string.IsNullOrEmpty(path))
            return false;

        return TryGetPropertyFromObject(obj, path.AsSpan(), out property);
    }

    private static bool TryGetPropertyFromObject(BinTreeObject obj, ReadOnlySpan<char> remainingPath, out BinTreeProperty property)
    {
        property = null;
        if (remainingPath.IsEmpty)
            return false;

        int sepIndex = remainingPath.IndexOfAny('/', '\\');
        ReadOnlySpan<char> segment = sepIndex == -1 ? remainingPath : remainingPath.Slice(0, sepIndex);
        ReadOnlySpan<char> nextRemaining = sepIndex == -1 ? ReadOnlySpan<char>.Empty : remainingPath.Slice(sepIndex + 1);

        ParseSegmentIndex(segment, out var propName, out int index);

        uint propHash = HashLower(propName);
        if (!obj.Properties.TryGetValue(propHash, out var currentProperty))
        {
            // Support raw hash fallback
            if (propName.StartsWith("0x", StringComparison.OrdinalIgnoreCase) &&
                uint.TryParse(propName.Slice(2), System.Globalization.NumberStyles.HexNumber, null, out uint rawHash))
            {
                if (!obj.Properties.TryGetValue(rawHash, out currentProperty))
                    return false;
            }
            else if (uint.TryParse(propName, out uint rawHash2))
            {
                if (!obj.Properties.TryGetValue(rawHash2, out currentProperty))
                    return false;
            }
            else
            {
                return false;
            }
        }

        // Apply array index navigation if present
        if (index != -1)
        {
            if (currentProperty is BinTreeContainer container)
            {
                if (index < 0 || index >= container.Elements.Count)
                    return false;
                currentProperty = container.Elements[index];
            }
            else
            {
                return false;
            }
        }

        return TryGetPropertyFromProperty(currentProperty, nextRemaining, out property);
    }

    private static bool TryGetPropertyFromProperty(BinTreeProperty currentProperty, ReadOnlySpan<char> remainingPath, out BinTreeProperty property)
    {
        if (remainingPath.IsEmpty)
        {
            property = currentProperty;
            return true;
        }

        property = null;
        int sepIndex = remainingPath.IndexOfAny('/', '\\');
        ReadOnlySpan<char> segment = sepIndex == -1 ? remainingPath : remainingPath.Slice(0, sepIndex);
        ReadOnlySpan<char> nextRemaining = sepIndex == -1 ? ReadOnlySpan<char>.Empty : remainingPath.Slice(sepIndex + 1);

        ParseSegmentIndex(segment, out var propName, out int index);

        // 1. Struct or Embedded Property Traversal
        if (currentProperty is BinTreeStruct structProperty)
        {
            uint propHash = HashLower(propName);
            if (!structProperty.Properties.TryGetValue(propHash, out var nextProperty))
            {
                if (propName.StartsWith("0x", StringComparison.OrdinalIgnoreCase) &&
                    uint.TryParse(propName.Slice(2), System.Globalization.NumberStyles.HexNumber, null, out uint rawHash))
                {
                    if (!structProperty.Properties.TryGetValue(rawHash, out nextProperty))
                        return false;
                }
                else if (uint.TryParse(propName, out uint rawHash2))
                {
                    if (!structProperty.Properties.TryGetValue(rawHash2, out nextProperty))
                        return false;
                }
                else
                {
                    return false;
                }
            }

            if (index != -1)
            {
                if (nextProperty is BinTreeContainer container)
                {
                    if (index < 0 || index >= container.Elements.Count)
                        return false;
                    nextProperty = container.Elements[index];
                }
                else
                {
                    return false;
                }
            }

            return TryGetPropertyFromProperty(nextProperty, nextRemaining, out property);
        }

        // 2. Optional Property Traversal
        if (currentProperty is BinTreeOptional optionalProperty && optionalProperty.Value != null)
        {
            uint nextHash = HashLower(propName);
            if (optionalProperty.Value.NameHash == nextHash)
            {
                var nextProperty = optionalProperty.Value;
                if (index != -1)
                {
                    if (nextProperty is BinTreeContainer container)
                    {
                        if (index < 0 || index >= container.Elements.Count)
                            return false;
                        nextProperty = container.Elements[index];
                    }
                    else
                    {
                        return false;
                    }
                }
                return TryGetPropertyFromProperty(nextProperty, nextRemaining, out property);
            }
            // Transparently skip the optional wrapper if the child is a Struct and contains the property directly
            else if (optionalProperty.Value is BinTreeStruct optionalStruct)
            {
                if (optionalStruct.Properties.TryGetValue(nextHash, out var nextProperty))
                {
                    if (index != -1)
                    {
                        if (nextProperty is BinTreeContainer container)
                        {
                            if (index < 0 || index >= container.Elements.Count)
                                return false;
                            nextProperty = container.Elements[index];
                        }
                        else
                        {
                            return false;
                        }
                    }
                    return TryGetPropertyFromProperty(nextProperty, nextRemaining, out property);
                }
            }
        }

        // 3. Map Property Traversal
        if (currentProperty is BinTreeMap mapProperty)
        {
            BinTreeProperty matchedKey = null;
            foreach (var key in mapProperty.Keys)
            {
                if (KeyMatches(key, propName))
                {
                    matchedKey = key;
                    break;
                }
            }

            if (matchedKey != null)
            {
                var nextProperty = mapProperty[matchedKey];
                if (index != -1)
                {
                    if (nextProperty is BinTreeContainer container)
                    {
                        if (index < 0 || index >= container.Elements.Count)
                            return false;
                        nextProperty = container.Elements[index];
                    }
                    else
                    {
                        return false;
                    }
                }
                return TryGetPropertyFromProperty(nextProperty, nextRemaining, out property);
            }
        }

        return false;
    }

    private static void ParseSegmentIndex(ReadOnlySpan<char> segment, out ReadOnlySpan<char> name, out int index)
    {
        name = segment;
        index = -1;
        int bracketIndex = segment.IndexOf('[');
        if (bracketIndex != -1 && segment.Length > 0 && segment[segment.Length - 1] == ']')
        {
            name = segment.Slice(0, bracketIndex);
            if (int.TryParse(segment.Slice(bracketIndex + 1, segment.Length - bracketIndex - 2), out int parsedIndex))
            {
                index = parsedIndex;
            }
        }
    }

    private static bool KeyMatches(BinTreeProperty key, ReadOnlySpan<char> segment)
    {
        if (key is BinTreeString strKey && segment.Equals(strKey.Value.AsSpan(), StringComparison.Ordinal))
            return true;

        if (key is BinTreeHash hashKey)
        {
            if (hashKey.Value == HashLower(segment))
                return true;
            if (segment.StartsWith("0x", StringComparison.OrdinalIgnoreCase) &&
                uint.TryParse(segment.Slice(2), System.Globalization.NumberStyles.HexNumber, null, out uint rawHash) &&
                hashKey.Value == rawHash)
                return true;
            if (uint.TryParse(segment, out uint rawHash2))
            {
                if (hashKey.Value == rawHash2)
                    return true;
            }
        }

        if (key is BinTreeU32 u32Key && uint.TryParse(segment, out uint u32Val) && u32Key.Value == u32Val)
            return true;

        if (key is BinTreeI32 i32Key && int.TryParse(segment, out int i32Val) && i32Key.Value == i32Val)
            return true;

        return false;
    }

    private static uint HashLower(ReadOnlySpan<char> input)
    {
        uint hash = 2166136261;
        for (int i = 0; i < input.Length; i++)
        {
            char c = input[i];
            // Convert to lowercase invariant
            if (c >= 'A' && c <= 'Z')
                c = (char)(c + 32);
            hash ^= c;
            hash *= 16777619;
        }
        return hash;
    }
}
