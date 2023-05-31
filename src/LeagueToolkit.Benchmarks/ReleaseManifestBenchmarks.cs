using BenchmarkDotNet.Attributes;
using LeagueToolkit.IO.ReleaseManifestFile;

namespace LeagueToolkit.Benchmarks;

[MemoryDiagnoser]
public class ReleaseManifestBenchmarks
{
    private static readonly string[] ManifestUrls =
    {
        "https://lol.secure.dyn.riotcdn.net/channels/public/releases/3379CAB8EBF11345.manifest",
        "https://lol.secure.dyn.riotcdn.net/channels/public/releases/6ADDAB3D6812B9A9.manifest",
        "https://bacon.secure.dyn.riotcdn.net/channels/public/releases/BA2874BED61C60E1.manifest",
        "https://bacon.secure.dyn.riotcdn.net/channels/public/releases/CB37BF993B8F1F7C.manifest",
        "https://ks-foundation.secure.dyn.riotcdn.net/channels/public/releases/E14FCA78EA784741.manifest",
        "https://valorant.secure.dyn.riotcdn.net/channels/public/releases/B1158BEBA8E7626F.manifest"
    };

    [GlobalSetup]
    public void Setup()
    {
        using var httpClient = new HttpClient();
        foreach (string path in ManifestUrls)
        {
            using Task<Stream> response = httpClient.GetStreamAsync(path);
            string fileName = Path.GetFileName(path);
            using FileStream fs = File.Create(fileName);
            response.Result.CopyTo(fs);
        }
    }

    [Benchmark]
    public void LoadManifests()
    {
        foreach (string path in ManifestUrls)
        {
            var _ = new ReleaseManifest(Path.GetFileName(path));
        }
    }

    [Benchmark]
    public void LoadAndWriteManifests()
    {
        foreach (string path in ManifestUrls)
        {
            var manifest = new ReleaseManifest(Path.GetFileName(path));
            manifest.Write("temp");
        }
    }
}
