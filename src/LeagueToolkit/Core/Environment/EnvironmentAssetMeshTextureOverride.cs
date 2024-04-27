using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.Environment;

public struct EnvironmentAssetMeshTextureOverride
{
    public int Index { get; set; }
    public string Texture { get; set; }

    public EnvironmentAssetMeshTextureOverride()
    {
        this.Index = 0;
        this.Texture = string.Empty;
    }

    public EnvironmentAssetMeshTextureOverride(int index, string texture)
    {
        this.Index = index;
        this.Texture = texture;
    }

    public static EnvironmentAssetMeshTextureOverride Read(BinaryReader br)
    {
        var index = br.ReadInt32();
        var name = br.ReadSizedString();

        return new(index, name);
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write(this.Index);
        bw.WriteSizedString(this.Texture);
    }
}
