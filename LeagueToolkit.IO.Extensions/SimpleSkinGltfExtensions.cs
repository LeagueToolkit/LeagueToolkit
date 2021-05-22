using LeagueToolkit.Helpers.Cryptography;
using LeagueToolkit.IO.AnimationFile;
using LeagueToolkit.IO.SkeletonFile;
using ImageMagick;
using SharpGLTF.Animations;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using SharpGLTF.Transforms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using LeagueAnimation = LeagueToolkit.IO.AnimationFile.Animation;

namespace LeagueToolkit.IO.SimpleSkinFile
{
    using VERTEX = VertexBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>;
    using VERTEX_SKINNED = VertexBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>;

    public static class SimpleSkinGltfExtensions
    {
        public static ModelRoot ToGltf(this SimpleSkin skn, Dictionary<string, MagickImage> materialTextues = null)
        {
            SceneBuilder sceneBuilder = new SceneBuilder("model");
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

            sceneBuilder.AddRigidMesh(meshBuilder, new AffineTransform(new Vector3(-1, 1, 1), Quaternion.Identity, Vector3.Zero).Matrix);

            return sceneBuilder.ToGltf2();
        }
        public static ModelRoot ToGltf(this SimpleSkin skn, Skeleton skeleton, Dictionary<string, MagickImage> materialTextues = null, List<(string, LeagueAnimation)> leagueAnimations = null)
        {
            SceneBuilder sceneBuilder = new SceneBuilder();
            NodeBuilder rootNodeBuilder = new NodeBuilder("model");

            var meshBuilder = VERTEX_SKINNED.CreateCompatibleMesh();
            var bones = CreateSkeleton(rootNodeBuilder, skeleton);

            // Build mesh primitives
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
                    VERTEX_SKINNED a = vertices[submesh.Indices[i + 0]];
                    VERTEX_SKINNED b = vertices[submesh.Indices[i + 1]];
                    VERTEX_SKINNED c = vertices[submesh.Indices[i + 2]];

                    submeshPrimitive.AddTriangle(c, b, a);
                }
            }

            // Add mesh to scene
            sceneBuilder.AddSkinnedMesh(meshBuilder, bones.ToArray());

            // Create animations
            if (leagueAnimations is not null)
            {
                CreateAnimations(bones.Select(x => x.Node).ToList(), leagueAnimations);
            }

            // Flip the scene across the X axis
            sceneBuilder.ApplyBasisTransform(Matrix4x4.CreateScale(new Vector3(-1, 1, 1)));

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
        private static List<(NodeBuilder Node, Matrix4x4 InverseBindMatrix)> CreateSkeleton(NodeBuilder rootNode, Skeleton skeleton)
        {
            var bones = new List<(NodeBuilder, Matrix4x4)>();

            foreach (SkeletonJoint joint in skeleton.Joints)
            {
                // Root
                if (joint.ParentID == -1)
                {
                    NodeBuilder jointNode = rootNode.CreateNode(joint.Name);

                    jointNode.LocalTransform = joint.LocalTransform;

                    bones.Add((jointNode, joint.InverseBindTransform));
                }
                else
                {
                    SkeletonJoint parentJoint = skeleton.Joints.FirstOrDefault(x => x.ID == joint.ParentID);
                    NodeBuilder parentNode = bones.FirstOrDefault(x => x.Item1.Name == parentJoint.Name).Item1;
                    NodeBuilder jointNode = parentNode.CreateNode(joint.Name);

                    jointNode.LocalTransform = joint.LocalTransform;

                    bones.Add((jointNode, joint.InverseBindTransform));
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
                        if (track.Translations.Count == 0) track.Translations.Add(0.0f, new Vector3(0, 0, 0));
                        if (track.Translations.Count == 1) track.Translations.Add(1.0f, new Vector3(0, 0, 0));
                        CurveBuilder<Vector3> translationBuilder = joint.UseTranslation().UseTrackBuilder(animationName);
                        foreach (var translation in track.Translations)
                        {
                            translationBuilder.SetPoint(translation.Key, translation.Value);
                        }

                        if (track.Rotations.Count == 0) track.Rotations.Add(0.0f, Quaternion.Identity);
                        if (track.Rotations.Count == 1) track.Rotations.Add(1.0f, Quaternion.Identity);
                        CurveBuilder<Quaternion> rotationBuilder = joint.UseRotation().UseTrackBuilder(animationName);
                        foreach (var rotation in track.Rotations)
                        {
                            rotationBuilder.SetPoint(rotation.Key, rotation.Value);
                        }

                        if (track.Scales.Count == 0) track.Scales.Add(0.0f, new Vector3(1, 1, 1));
                        if (track.Scales.Count == 1) track.Scales.Add(1.0f, new Vector3(1, 1, 1));
                        CurveBuilder<Vector3> scaleBuilder = joint.UseScale().UseTrackBuilder(animationName);
                        foreach (var scale in track.Scales.ToList())
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

            Mesh mesh = root.LogicalMeshes[0];

            List<byte> influences = new();
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
                    byte[] influenceBones = new byte[] 
                    {
                        (byte)bonesVector.X,
                        (byte)bonesVector.Y,
                        (byte)bonesVector.Z,
                        (byte)bonesVector.W
                    };

                    for(byte b = 0; b < influenceBones.Length; b++)
                    {
                        if(!influences.Any(x => x == influenceBones[b]))
                        {
                            influences.Add(influenceBones[b]);
                        }
                    }

                    Vector4 weightsVector = vertexWeightsAccessor[i];
                    float[] weights = new float[]
                    {
                        weightsVector.X,
                        weightsVector.Y,
                        weightsVector.Z,
                        weightsVector.W
                    };

                    Vector3 vertexPosition = vertexPositionAccessor[i];

                    SimpleSkinVertex vertex = new SimpleSkinVertex(vertexPosition, influenceBones, weights, vertexNormalAccessor[i], vertexUvAccessor[i]);

                    vertices.Add(vertex);
                }

                submeshes.Add(new SimpleSkinSubmesh(primitive.Material.Name, indices.Select(x => (ushort)x).ToList(), vertices));
            }

            SimpleSkin simpleSkin = new SimpleSkin(submeshes);
            Skeleton skeleton = CreateLeagueSkeleton(root.LogicalSkins[0]);

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
                    Vector3 scale = jointNode.LocalTransform.Scale;

                    joints.Add(new SkeletonJoint((short)i, -1, jointNode.Name, jointNode.LocalTransform.Translation, scale, jointNode.LocalTransform.Rotation));
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
