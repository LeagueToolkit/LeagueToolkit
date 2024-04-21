using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.Environment;

public struct EnvironmentAssetBakedPaintChannelDef
{
    public int Index { get; set; }
    public string Texture { get; set; }

    public EnvironmentAssetBakedPaintChannelDef()
    {
        this.Index = 0;
        this.Texture = string.Empty;
    }

    public EnvironmentAssetBakedPaintChannelDef(int index, string texture)
    {
        this.Index = index;
        this.Texture = texture;
    }

    public static EnvironmentAssetBakedPaintChannelDef Read(BinaryReader br)
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
