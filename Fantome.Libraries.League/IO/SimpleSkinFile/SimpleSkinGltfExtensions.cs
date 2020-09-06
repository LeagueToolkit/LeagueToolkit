using Fantome.Libraries.League.Helpers.Cryptography;
using Fantome.Libraries.League.Helpers.Extensions;
using Fantome.Libraries.League.IO.AnimationFile;
using Fantome.Libraries.League.IO.SkeletonFile;
using ImageMagick;
using SharpGLTF.Animations;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using GltfAnimation = SharpGLTF.Schema2.Animation;
using LeagueAnimation = Fantome.Libraries.League.IO.AnimationFile.Animation;

namespace Fantome.Libraries.League.IO.SimpleSkinFile
{
    using VERTEX = VertexBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>;
    using VERTEX_SKINNED = VertexBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>;

    public static class SimpleSkinGltfExtensions
    {
        public static ModelRoot ToGltf(this SimpleSkin skn, Dictionary<string, MagickImage> materialTextues = null)
        {
            SceneBuilder sceneBuilder = new SceneBuilder("model");
            NodeBuilder rootNodeBuilder = new NodeBuilder()
                .WithLocalScale(new Vector3(-1, 1, 1)); // X axis is flipped
            var meshBuilder = VERTEX.CreateCompatibleMesh();

            foreach (SimpleSkinSubmesh submesh in skn.Submeshes)
            {
                MaterialBuilder material = new MaterialBuilder(submesh.Name).WithUnlitShader();
                var submeshPrimitive = meshBuilder.UsePrimitive(material);

                // Assign submesh Image
                if(materialTextues is not null && materialTextues.ContainsKey(submesh.Name))
                {
                    MagickImage submeshImage = materialTextues[submesh.Name];
                    AssignMaterialTexture(material, submeshImage);
                }

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

            sceneBuilder.AddRigidMesh(meshBuilder, rootNodeBuilder);

            return sceneBuilder.ToGltf2();
        }
        public static ModelRoot ToGltf(this SimpleSkin skn, Skeleton skeleton, Dictionary<string, MagickImage> materialTextues = null, List<(string, LeagueAnimation)> leagueAnimations = null)
        {
            SceneBuilder sceneBuilder = new SceneBuilder("model");
            NodeBuilder rootNodeBuilder = new NodeBuilder()
                .WithLocalScale(new Vector3(-1, 1, 1)); // X axis is flipped
            var meshBuilder = VERTEX_SKINNED.CreateCompatibleMesh();

            List<NodeBuilder> bones = CreateSkeleton(rootNodeBuilder, skeleton);

            foreach (SimpleSkinSubmesh submesh in skn.Submeshes)
            {
                MaterialBuilder material = new MaterialBuilder(submesh.Name).WithUnlitShader();
                var submeshPrimitive = meshBuilder.UsePrimitive(material);

                // Assign submesh Image
                if (materialTextues is not null && materialTextues.ContainsKey(submesh.Name))
                {
                    MagickImage submeshImage = materialTextues[submesh.Name];
                    AssignMaterialTexture(material, submeshImage);
                }

                // Build vertices
                var vertices = new List<VERTEX_SKINNED>(submesh.Vertices.Count);
                foreach (SimpleSkinVertex vertex in submesh.Vertices)
                {
                    VertexPositionNormal positionNormal = new VertexPositionNormal(vertex.Position, vertex.Normal);
                    VertexTexture1 uv = new VertexTexture1(vertex.UV);
                    VertexJoints4 joints = new VertexJoints4(new (int, float)[]
                    {
                        (skeleton.Influences[vertex.BoneIndices[0]], vertex.Weights[0]),
                        (skeleton.Influences[vertex.BoneIndices[1]], vertex.Weights[1]),
                        (skeleton.Influences[vertex.BoneIndices[2]], vertex.Weights[2]),
                        (skeleton.Influences[vertex.BoneIndices[3]], vertex.Weights[3])
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

            sceneBuilder.AddSkinnedMesh(meshBuilder, Matrix4x4.Identity, bones.ToArray());

            if (leagueAnimations != null)
            {
                CreateAnimations(bones, leagueAnimations);
            }

            return sceneBuilder.ToGltf2();
        }

        private static void AssignMaterialTexture(MaterialBuilder materialBuilder, MagickImage texture)
        {
            MemoryStream textureStream = new MemoryStream();

            texture.Write(textureStream, MagickFormat.Png);

            materialBuilder
                .UseChannel(KnownChannel.BaseColor)
                .UseTexture()
                .WithPrimaryImage(new SharpGLTF.Memory.MemoryImage(textureStream.GetBuffer()));
        }
        private static List<NodeBuilder> CreateSkeleton(NodeBuilder rootNode, Skeleton skeleton)
        {
            NodeBuilder skeletonRoot = rootNode.CreateNode("skeleton");
            List<NodeBuilder> bones = new List<NodeBuilder>();

            foreach (SkeletonJoint joint in skeleton.Joints)
            {
                // Root
                if (joint.ParentID == -1)
                {
                    NodeBuilder jointNode = skeletonRoot.CreateNode(joint.Name);

                    jointNode.LocalTransform = joint.LocalTransform;

                    bones.Add(jointNode);
                }
                else
                {
                    SkeletonJoint parentJoint = skeleton.Joints.FirstOrDefault(x => x.ID == joint.ParentID);
                    NodeBuilder parentNode = bones.FirstOrDefault(x => x.Name == parentJoint.Name);
                    NodeBuilder jointNode = parentNode.CreateNode(joint.Name);

                    jointNode.LocalTransform = joint.LocalTransform;

                    bones.Add(jointNode);
                }
            }

            return bones;
        }
        private static void CreateAnimations(List<NodeBuilder> joints, List<(string, LeagueAnimation)> leagueAnimations)
        {
            // Check if all animations have names, if not then create them
            for (int i = 0; i < leagueAnimations.Count; i++)
            {
                if (string.IsNullOrEmpty(leagueAnimations[i].Item1))
                {
                    leagueAnimations[i] = ("Animation" + i, leagueAnimations[i].Item2);
                }
            }

            foreach ((string animationName, LeagueAnimation leagueAnimation) in leagueAnimations)
            {
                foreach (AnimationTrack track in leagueAnimation.Tracks)
                {
                    NodeBuilder joint = joints.FirstOrDefault(x => Cryptography.ElfHash(x.Name) == track.JointHash);

                    if (joint is not null)
                    {
                        CurveBuilder<Vector3> translationBuilder = joint.UseTranslation().UseTrackBuilder(animationName);
                        foreach (var translation in track.Translations)
                        {
                            translationBuilder.SetPoint(translation.Key, translation.Value);
                        }

                        CurveBuilder<Quaternion> rotationBuilder = joint.UseRotation().UseTrackBuilder(animationName);
                        foreach (var rotation in track.Rotations)
                        {
                            rotationBuilder.SetPoint(rotation.Key, rotation.Value);
                        }

                        CurveBuilder<Vector3> scaleBuilder = joint.UseScale().UseTrackBuilder(animationName);
                        foreach (var scale in track.Scales)
                        {
                            scaleBuilder.SetPoint(scale.Key, scale.Value);
                        }
                    }
                }
            }
        }
    
        public static (SimpleSkin, Skeleton) ToLeagueModel(this ModelRoot root)
        {
            if(root.LogicalMeshes.Count != 1)
            {
                throw new Exception("Invalid Mesh Count: " + root.LogicalMeshes.Count + " (must be 1)");
            }
            if(root.LogicalSkins.Count != 1)
            {
                throw new Exception("Invalid Skin count: " + root.LogicalSkins.Count + " (must be 1)");
            }

            Skeleton skeleton = CreateLeagueSkeleton(root.LogicalSkins[0]);
            Mesh mesh = root.LogicalMeshes[0];

            List<SimpleSkinSubmesh> submeshes = new(mesh.Primitives.Count);
            foreach(MeshPrimitive primitive in mesh.Primitives)
            {
                List<uint> indices = new(primitive.GetIndices());
                
                IList<Vector3> vertexPositionAccessor = GetVertexAccessor("POSITION", primitive.VertexAccessors).AsVector3Array();
                IList<Vector3> vertexNormalAccessor = GetVertexAccessor("NORMAL", primitive.VertexAccessors).AsVector3Array();
                IList<Vector2> vertexUvAccessor = GetVertexAccessor("TEXCOORD_0", primitive.VertexAccessors).AsVector2Array();
                IList<Vector4> vertexBonesAccessor = GetVertexAccessor("JOINTS_0", primitive.VertexAccessors).AsVector4Array();
                IList<Vector4> vertexWeightsAccessor = GetVertexAccessor("WEIGHTS_0", primitive.VertexAccessors).AsVector4Array();

                List<SimpleSkinVertex> vertices = new(vertexPositionAccessor.Count);
                for(int i = 0; i < vertexPositionAccessor.Count; i++)
                {
                    Vector4 bonesVector = vertexBonesAccessor[i];
                    byte[] bones = new byte[] 
                    {
                        (byte)bonesVector.X,
                        (byte)bonesVector.Y,
                        (byte)bonesVector.Z,
                        (byte)bonesVector.W
                    };

                    Vector4 weightsVector = vertexWeightsAccessor[i];
                    float[] weights = new float[]
                    {
                        weightsVector.X,
                        weightsVector.Y,
                        weightsVector.Z,
                        weightsVector.W
                    };

                    Vector3 position = vertexPositionAccessor[i];
                    position.X *= -1;

                    SimpleSkinVertex vertex = new SimpleSkinVertex(position, bones, weights, vertexNormalAccessor[i], vertexUvAccessor[i]);

                    vertices.Add(vertex);
                }

                submeshes.Add(new SimpleSkinSubmesh(primitive.Material.Name, indices.Select(x => (ushort)x).ToList(), vertices));
            }

            SimpleSkin simpleSkin = new SimpleSkin(submeshes);

            return (simpleSkin, skeleton);
        }

        private static Skeleton CreateLeagueSkeleton(Skin skin)
        {
            List<Node> nodes = new(skin.JointsCount);
            for (int i = 0; i < skin.JointsCount; i++)
            {
                nodes.Add(skin.GetJoint(i).Joint);
            }

            List<SkeletonJoint> joints = new(nodes.Count);
            for (int i = 0; i < nodes.Count; i++)
            {
                Node jointNode = nodes[i];

                // If parent is null or isn't a skin joint then the joint is a root bone
                if (jointNode.VisualParent is null || !jointNode.VisualParent.IsSkinJoint)
                {
                    joints.Add(new SkeletonJoint((short)i, -1, jointNode.Name, jointNode.LocalTransform.Translation, jointNode.LocalTransform.Scale, jointNode.LocalTransform.Rotation));
                }
                else
                {
                    short parentId = (short)nodes.IndexOf(jointNode.VisualParent);

                    joints.Add(new SkeletonJoint((short)i, parentId, jointNode.Name, jointNode.LocalTransform.Translation, jointNode.LocalTransform.Scale, jointNode.LocalTransform.Rotation));
                }
            }

            List<short> influences = new(joints.Count);
            for(short i = 0; i < joints.Count; i++)
            {
                influences.Add(i);
            }

            return new Skeleton(joints, influences);
        }
        private static Accessor GetVertexAccessor(string name, IReadOnlyDictionary<string, Accessor> vertexAccessors)
        {
            if(vertexAccessors.TryGetValue(name, out Accessor accessor))
            {
                return accessor;
            }
            else
            {
                throw new Exception("Mesh does not contain a vertex accessor: " + name);
            }
        }
    }
}
