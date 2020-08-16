using Fantome.Libraries.League.Helpers;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Memory;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;

namespace Fantome.Libraries.League.IO.SimpleSkin
{
    using VERTEX = VertexBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>;

    public static class SimpleSkinGltfExtensions
    {
        public static ModelRoot ToGLTF(this SimpleSkin skn)
        {
            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene("default");
            var meshBuilder = VERTEX.CreateCompatibleMesh();

            foreach (SimpleSkinSubmesh submesh in skn.Submeshes)
            {
                MaterialBuilder material = new MaterialBuilder(submesh.Name).WithUnlitShader();
                var submeshPrimitive = meshBuilder.UsePrimitive(material);

                // Build vertices
                List<VERTEX> vertices = new List<VERTEX>(submesh.Vertices.Count);
                foreach (SimpleSkinVertex vertex in submesh.Vertices)
                {
                    VertexPositionNormal positionNormal = new VertexPositionNormal(vertex.Position, vertex.Normal);
                    VertexTexture1 uv = new VertexTexture1(vertex.UV);

                    vertices.Add(new VERTEX(positionNormal, uv));
                }

                // Add vertices to primitive
                for (int i = 0; i < submesh.Indices.Count; i += 3)
                {
                    VERTEX v1 = vertices[submesh.Indices[i + 0]];
                    VERTEX v2 = vertices[submesh.Indices[i + 1]];
                    VERTEX v3 = vertices[submesh.Indices[i + 2]];

                    submeshPrimitive.AddTriangle(v1, v2, v3);
                }
            }

            Node mainNode = scene.CreateNode();
            mainNode.WithMesh(root.CreateMesh(meshBuilder));

            return root;
        }
    }
}
