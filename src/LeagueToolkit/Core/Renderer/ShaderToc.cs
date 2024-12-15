using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueToolkit.Hashing;
using LeagueToolkit.Utils.Exceptions;
using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.Renderer;

public class ShaderToc
{
    public IReadOnlyList<ShaderMacroDefinition> BaseDefines => this._baseDefines;
    private readonly List<ShaderMacroDefinition> _baseDefines = [];

    public IReadOnlyList<ulong> ShaderHashes => this._shaderHashes;
    private readonly ulong[] _shaderHashes = [];

    public IReadOnlyList<uint> ShaderIds => this._shaderIds;
    private readonly uint[] _shaderIds = [];

    public ShaderToc(
        IEnumerable<ShaderMacroDefinition> baseDefines,
        IEnumerable<ulong> shaderHashes,
        IEnumerable<uint> shaderIds
    )
    {
        this._baseDefines = [.. baseDefines];
        this._shaderHashes = [.. shaderHashes];
        this._shaderIds = [.. shaderIds];
    }

    public ShaderToc(Stream stream)
    {
        using BinaryReader reader = new(stream);

        var tocMagic = reader.ReadSizedString();
        if (tocMagic != "TOC3.0")
        {
            throw new InvalidFileSignatureException();
        }

        var shaderCount = reader.ReadUInt32();
        var baseDefinesCount = reader.ReadUInt32();
        var _bundledShaderCount = reader.ReadUInt32(); // count of bundled shaders, unused
        var _shaderType = reader.ReadUInt32(); // 0=vs, 1=ps

        var baseDefinesSectionMagic = reader.ReadSizedString();
        if (baseDefinesSectionMagic != "baseDefines")
        {
            throw new InvalidDataException("Invalid baseDefines section magic");
        }
        for (int i = 0; i < baseDefinesCount; i++)
        {
            this._baseDefines.Add(ShaderMacroDefinition.Read(reader));
        }

        var shadersSectionMagic = reader.ReadSizedString();
        if (shadersSectionMagic != "shaders")
        {
            throw new InvalidDataException("Invalid shaders section magic");
        }

        this._shaderHashes = new ulong[shaderCount];
        this._shaderIds = new uint[shaderCount];
        for (int i = 0; i < shaderCount; i++)
        {
            this._shaderHashes[i] = reader.ReadUInt64();
        }
        for (int i = 0; i < shaderCount; i++)
        {
            this._shaderIds[i] = reader.ReadUInt32();
        }
    }
}

[DebuggerDisplay("{Name} = {Value}")]
public readonly struct ShaderMacroDefinition(string name, string value) : IEquatable<ShaderMacroDefinition>
{
    public string Name { get; init; } = name;
    public string Value { get; init; } = value;
    public uint Hash { get; init; } =
        Fnv1a.HashLower(
            string.IsNullOrEmpty(value) switch
            {
                true => name,
                false => $"{name}={value}"
            }
        );

    internal static ShaderMacroDefinition Read(BinaryReader reader)
    {
        var name = reader.ReadSizedString();
        var value = reader.ReadSizedString();

        return new ShaderMacroDefinition(name, value);
    }

    public override readonly string ToString() => $"{this.Name}={this.Value}";

    public readonly bool Equals(ShaderMacroDefinition other) => this.Hash == other.Hash;

    public override bool Equals(object obj) => obj is ShaderMacroDefinition definition && Equals(definition);

    public static bool operator ==(ShaderMacroDefinition left, ShaderMacroDefinition right) => left.Equals(right);

    public static bool operator !=(ShaderMacroDefinition left, ShaderMacroDefinition right) => !(left == right);

    public override int GetHashCode() => this.Hash.GetHashCode();
}
