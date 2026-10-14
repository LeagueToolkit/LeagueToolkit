namespace LeagueToolkit.Core.Mesh;

/// <summary>An authored material and absolute index range in a GMESH or TMESH.</summary>
public readonly record struct RenderMeshSubmesh(string Material, uint StartIndex, uint IndexCount, uint MinVertex, uint MaxVertex);
