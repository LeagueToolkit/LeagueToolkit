using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.HighPerformance;
using LeagueToolkit.Core.Wad;
using LeagueToolkit.Hashing;
using LeagueToolkit.Utils.Exceptions;
using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.Renderer;

public class ShaderLoader
{
    public static byte[] ResolveShaderBytecode(
        string shaderObjectPath,
        ShaderType shaderType,
        GraphicsPlatform platform,
        IEnumerable<ShaderMacroDefinition> defines,
        WadFile wad
    )
    {
        shaderObjectPath =
            $"{shaderObjectPath}.{GetShaderTypeExtension(shaderType)}.{GetGraphicsPlatformExtension(platform)}";

        var shaderObject = new ShaderToc(wad.LoadChunkDecompressed(shaderObjectPath).AsStream());

        var filteredDefines = shaderObject.BaseDefines.Intersect(defines);
        var filteredDefinesFormatted = string.Join("", filteredDefines.Select(x => x.ToString()));
        var filteredDefinesHash = XxHash64Ext.Hash(filteredDefinesFormatted);

        var shaderIndex = shaderObject.ShaderHashes.IndexOf(filteredDefinesHash);
        if (shaderIndex == -1)
        {
            throw new InvalidOperationException($"Shader not found for defines: {filteredDefinesFormatted}");
        }

        var shaderId = shaderObject.ShaderIds[shaderIndex];
        var shaderBundleId = 100 * (shaderId / 100);
        var shaderIndexInBundle = shaderId % 100;
        var shaderBundlePath = $"{shaderObjectPath}_{shaderIndexInBundle}";

        using var shaderBundleStream = wad.LoadChunkDecompressed(shaderBundlePath).AsStream();
        using var shaderBundleReader = new BinaryReader(shaderBundleStream);
        for (int i = 0; i < shaderIndexInBundle; i++)
        {
            var shaderSize = shaderBundleReader.ReadUInt32();
            shaderBundleReader.BaseStream.Seek(shaderSize, SeekOrigin.Current);
        }

        var requestedShaderSize = shaderBundleReader.ReadInt32();
        return shaderBundleReader.ReadBytes(requestedShaderSize);
    }

    public static string GetShaderTypeExtension(ShaderType shaderType) =>
        shaderType switch
        {
            ShaderType.Vertex => "vs",
            ShaderType.Pixel => "ps",
            _ => throw new ArgumentOutOfRangeException(nameof(shaderType), shaderType, null)
        };

    public static string GetGraphicsPlatformExtension(GraphicsPlatform platform) =>
        platform switch
        {
            GraphicsPlatform.Dx9 => "dx9",
            GraphicsPlatform.Dx11 => "dx11",
            GraphicsPlatform.Glsl => "glsl",
            GraphicsPlatform.Metal => "metal",
            _ => throw new ArgumentOutOfRangeException(nameof(platform), platform, null)
        };
}
