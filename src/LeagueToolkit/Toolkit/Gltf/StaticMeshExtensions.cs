using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Mesh;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;

namespace LeagueToolkit.Toolkit.Gltf;

public static class StaticMeshExtensions
{
    /// <summary>
    /// Creates a glTF asset
    /// </summary>
    /// <param name="staticMesh">The <see cref="StaticMesh"/> to create a glTF asset from</param>
    /// <returns>The created glTF asset</returns>
    public static ModelRoot ToGltf(this StaticMesh staticMesh)
    {
        Guard.IsNotNull(staticMesh, nameof(staticMesh));

        SceneBuilder scene = new();

        scene.AddRigidMesh(CreateMesh(staticMesh), new NodeBuilder("static_mesh"));

        return scene.ToGltf2();
    }

    private static IMeshBuilder<MaterialBuilder> CreateMesh(StaticMesh staticMesh)
    {
        MeshBuilder<VertexPosition, VertexTexture1, VertexEmpty> meshBuilder = new("static_mesh");

        var primitiveBuilder = meshBuilder.UsePrimitive(new("staticMaterial"));

        IVertexBuilder[] vertices = new IVertexBuilder[staticMesh.Vertices.Count];
        for (int i = 0; i < staticMesh.Vertices.Count; i++)
        {
            vertices[i] = new VertexBuilder<VertexPosition, VertexTexture1, VertexEmpty>();
        }

        foreach (StaticMeshFace face in staticMesh.Faces)
        {
            VertexBuilder<VertexPosition, VertexTexture1, VertexEmpty> v0 =
                new(new(staticMesh.Vertices[face.VertexId0]), new VertexTexture1(face.UV0));
            VertexBuilder<VertexPosition, VertexTexture1, VertexEmpty> v1 =
                new(new(staticMesh.Vertices[face.VertexId1]), new VertexTexture1(face.UV1));
            VertexBuilder<VertexPosition, VertexTexture1, VertexEmpty> v2 =
                new(new(staticMesh.Vertices[face.VertexId2]), new VertexTexture1(face.UV2));

            primitiveBuilder.AddTriangle(v0, v1, v2);
        }

        return meshBuilder;
    }
}
