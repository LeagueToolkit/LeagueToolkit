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
    /// <summary>
    /// Loads the bytecode for a shader.
    /// </summary>
    /// <param name="shaderObjectPath">The path to the shader object.</param>
    /// <param name="shaderType">The type of the shader.</param>
    /// <param name="platform">The platform of the shader.</param>
    /// <param name="defines">The defines to use for the shader.</param>
    /// <param name="wad">The WAD file containing the shader.</param>
    /// <returns>The bytecode for the shader.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the shader is not found.</exception>
    public static byte[] LoadBytecode(
        string shaderObjectPath,
        ShaderType shaderType,
        GraphicsPlatform platform,
        IEnumerable<ShaderMacroDefinition> defines,
        WadFile wad
    )
    {
        shaderObjectPath =
            $"{shaderObjectPath}.{GetShaderTypeExtension(shaderType)}.{GetGraphicsPlatformExtension(platform)}";

        using var shaderObjectStream = wad.LoadChunkDecompressed(shaderObjectPath).AsStream();
        var shaderObject = new ShaderToc(shaderObjectStream);

        var filteredDefinesFormatted = FilterDefines(defines, shaderObject.BaseDefines);
        var filteredDefinesHash = XxHash64Ext.Hash(filteredDefinesFormatted);

        var shaderIndex = shaderObject.ShaderHashes.IndexOf(filteredDefinesHash);
        if (shaderIndex == -1)
        {
            throw new InvalidOperationException($"Shader not found for defines: {filteredDefinesFormatted}");
        }

        var shaderId = shaderObject.ShaderIds[shaderIndex];
        var shaderBundleId = 100 * (shaderId / 100);
        var shaderIndexInBundle = shaderId % 100;
        var shaderBundlePath = $"{shaderObjectPath}_{shaderBundleId}";

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

    private static string FilterDefines(
        IEnumerable<ShaderMacroDefinition> defines,
        IEnumerable<ShaderMacroDefinition> baseDefines
    )
    {
        // Filter and override defines based on base defines
        var filteredDefines = new List<ShaderMacroDefinition>();
        foreach (var requestedDefine in defines)
        {
            var matchingBaseDefine = baseDefines.FirstOrDefault(x => x.Hash == requestedDefine.Hash);
            if (!string.IsNullOrEmpty(matchingBaseDefine.Name))
            {
                filteredDefines.Add(requestedDefine);
            }
        }

        return string.Join("", filteredDefines.OrderBy(x => x.Name).Select(x => x.ToString()));
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
