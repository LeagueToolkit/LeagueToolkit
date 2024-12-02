using CommunityToolkit.Diagnostics;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.Core.Primitives;
using SharpGLTF.Schema2;
using System.Numerics;

namespace LeagueToolkit.Toolkit.Gltf;

public static class ModelRootExtensions
{
    /// <summary>
    /// Converts the glTF asset into a StaticMesh
    /// </summary>
    /// <param name="root">The glTF asset to convert</param>
    /// <returns>The created StaticMesh</returns>
    public static StaticMesh ToStaticMesh(this ModelRoot root)
    {
        Guard.IsNotNull(root, nameof(root));
        Guard.HasSizeEqualTo(root.LogicalMeshes, 1, nameof(root.LogicalMeshes));

        var gltfMesh = root.LogicalMeshes[0];
        List<Vector3> vertices = [];
        List<Color> vertexColors = [];
        List<StaticMeshFace> faces = [];
        bool hasVertexColors = false;

        foreach (MeshPrimitive primitive in gltfMesh.Primitives)
        {
            // Get vertex data
            var primitivePositions = primitive.VertexAccessors["POSITION"].AsVector3Array();
            var primitiveColors = primitive.VertexAccessors.TryGetValue("COLOR_0", out var colorAccessor)
                ? colorAccessor.AsVector4Array()
                : null;

            if (primitiveColors != null)
            {
                hasVertexColors = true;
            }

            var baseVertex = vertices.Count;

            // Add vertices and colors
            for (int i = 0; i < primitivePositions.Count; i++)
            {
                vertices.Add(primitivePositions[i]);
                if (primitiveColors != null)
                {
                    var color = primitiveColors[i];
                    vertexColors.Add(new Color((byte)(color.X * 255), (byte)(color.Y * 255),
                        (byte)(color.Z * 255), (byte)(color.W * 255)));
                }
            }

            // Get indices and UVs
            IList<uint> indices = primitive.IndexAccessor.AsIndicesArray();
            IList<Vector2> texCoords = primitive.VertexAccessors.TryGetValue("TEXCOORD_0", out var uvAccessor)
                ? uvAccessor.AsVector2Array()
                : null;

            // Create faces
            for (int i = 0; i < indices.Count; i += 3)
            {
                ushort v0 = (ushort)(indices[i] + baseVertex);
                ushort v1 = (ushort)(indices[i + 1] + baseVertex);
                ushort v2 = (ushort)(indices[i + 2] + baseVertex);

                Vector2 uv0 = texCoords?[(int)indices[i]] ?? Vector2.Zero;
                Vector2 uv1 = texCoords?[(int)indices[i + 1]] ?? Vector2.Zero;
                Vector2 uv2 = texCoords?[(int)indices[i + 2]] ?? Vector2.Zero;

                faces.Add(new StaticMeshFace(
                    primitive.Material?.Name ?? "default",
                    (v0, v1, v2),
                    (uv0, uv1, uv2)
                ));
            }
        }

        return hasVertexColors
            ? new StaticMesh(gltfMesh.Name, faces, vertices, vertexColors)
            : new StaticMesh(gltfMesh.Name, faces, vertices);
    }
}