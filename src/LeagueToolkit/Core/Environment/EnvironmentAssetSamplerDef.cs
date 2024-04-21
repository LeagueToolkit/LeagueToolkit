﻿using LeagueToolkit.Utils.Extensions;

namespace LeagueToolkit.Core.Environment;

public struct EnvironmentAssetSamplerDef
{
    public int Index { get; set; }
    public string Name { get; set; }

    public EnvironmentAssetSamplerDef()
    {
        this.Index = 0;
        this.Name = string.Empty;
    }

    public EnvironmentAssetSamplerDef(int index, string name)
    {
        this.Index = index;
        this.Name = name;
    }

    public static EnvironmentAssetSamplerDef Read(BinaryReader br)
    {
        var index = br.ReadInt32();
        var name = br.ReadSizedString();

        return new(index, name);
    }

    public void Write(BinaryWriter bw)
    {
        bw.Write(this.Index);
        bw.WriteSizedString(this.Name);
    }
}
