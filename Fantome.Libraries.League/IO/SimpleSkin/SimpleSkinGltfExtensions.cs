using Fantome.Libraries.League.Helpers;
using Fantome.Libraries.League.IO.SkeletonFile;
using LeagueFileTranslator.FileTranslators.SKL.IO;
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
    using VERTEX_SKINNED = VertexBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>;

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
                var vertices = new List<VERTEX>(submesh.Vertices.Count);
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

        public static ModelRoot ToGLTF(this SimpleSkin skn, Skeleton skeleton)
        {
            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene("default");
            var meshBuilder = VERTEX_SKINNED.CreateCompatibleMesh();

            List<Node> bones = CreateSkeleton(scene, skeleton);

            foreach (SimpleSkinSubmesh submesh in skn.Submeshes)
            {
                MaterialBuilder material = new MaterialBuilder(submesh.Name).WithUnlitShader();
                var submeshPrimitive = meshBuilder.UsePrimitive(material);

                // Build vertices
                var vertices = new List<VERTEX_SKINNED>(submesh.Vertices.Count);
                foreach (SimpleSkinVertex vertex in submesh.Vertices)
                {
                    VertexPositionNormal positionNormal = new VertexPositionNormal(vertex.Position, vertex.Normal);
                    VertexTexture1 uv = new VertexTexture1(vertex.UV);
                    VertexJoints4 joints = new VertexJoints4(new (int, float)[]
                    {
                        // + 1 because im lazy to map the node ids to joints
                        (skeleton.Influences[vertex.BoneIndices[0]] + 1, vertex.Weights[0]),
                        (skeleton.Influences[vertex.BoneIndices[1]] + 1, vertex.Weights[1]),
                        (skeleton.Influences[vertex.BoneIndices[2]] + 1, vertex.Weights[2]),
                        (skeleton.Influences[vertex.BoneIndices[3]] + 1, vertex.Weights[3])
                    });

                    vertices.Add(new VERTEX_SKINNED(positionNormal, uv, joints));
                }

                // Add vertices to primitive
                for (int i = 0; i < submesh.Indices.Count; i += 3)
                {
                    VERTEX_SKINNED v1 = vertices[submesh.Indices[i + 0]];
                    VERTEX_SKINNED v2 = vertices[submesh.Indices[i + 1]];
                    VERTEX_SKINNED v3 = vertices[submesh.Indices[i + 2]];

                    submeshPrimitive.AddTriangle(v1, v2, v3);
                }
            }

            

            Node mainNode = scene.CreateNode();
            Mesh mesh = root.CreateMesh(meshBuilder);
            
            //mainNode.WithMesh(mesh);
            mainNode.WithSkinnedMesh(mesh, Matrix4x4.Identity, bones.ToArray());

            return root;
        }

        private static List<Node> CreateSkeleton(Scene scene, Skeleton skeleton)
        {
            Node skeletonRoot = scene.CreateNode();
            List<Node> bones = new List<Node>();

            foreach(SkeletonJoint joint in skeleton.Joints)
            {
                // Root
                if(joint.ParentID == -1)
                {
                    Node jointNode = skeletonRoot.CreateNode(joint.Name).WithLocalTransform(joint.Local);

                    bones.Add(jointNode);
                }
                else
                {
                    SkeletonJoint parentJoint = skeleton.Joints.FirstOrDefault(x => x.ID == joint.ParentID);
                    Node parentNode = bones.FirstOrDefault(x => x.Name == parentJoint.Name);
                    Node jointNode = parentNode.CreateNode(joint.Name).WithLocalTransform(joint.Local);

                    bones.Add(jointNode);
                }
            }

            return bones;
        }
    }
}
