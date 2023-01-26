using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Animation;
using LeagueToolkit.Core.Animation.Builders;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.Hashing;
using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.IO.AnimationFile;
using LeagueToolkit.IO.Extensions.Utils;
using LeagueToolkit.IO.MapGeometryFile;
using SharpGLTF.Animations;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Memory;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using SixLabors.ImageSharp.PixelFormats;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Security.Cryptography;
using LeagueAnimation = LeagueToolkit.IO.AnimationFile.Animation;
using GltfImage = SharpGLTF.Schema2.Image;

namespace LeagueToolkit.IO.SimpleSkinFile
{
    using VERTEX = VertexBuilder<VertexPositionNormal, VertexTexture1, VertexEmpty>;
    using VERTEX_COLOR = VertexBuilder<VertexPositionNormal, VertexColor1Texture1, VertexEmpty>;
    using VERTEX_SKINNED = VertexBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>;
    using VERTEX_SKINNED_COLOR = VertexBuilder<VertexPositionNormal, VertexColor1Texture1, VertexJoints4>;
    using VERTEX_SKINNED_TANGENT = VertexBuilder<VertexPositionNormalTangent, VertexColor1Texture1, VertexJoints4>;
    using VERTEX_TANGENT = VertexBuilder<VertexPositionNormalTangent, VertexColor1Texture1, VertexEmpty>;

    public static class SimpleSkinGltfExtensions
    {
        /// <summary>
        /// Coverts the <see cref="SkinnedMesh"/> into a glTF asset with the specified textures
        /// </summary>
        /// <param name="skinnedMesh">The <see cref="SkinnedMesh"/> to covert</param>
        /// <param name="materialTextures">The texture data for the specified materials</param>
        /// <returns>The created glTF asset</returns>
        public static ModelRoot ToGltf(
            this SkinnedMesh skinnedMesh,
            IReadOnlyDictionary<string, Stream> materialTextures
        )
        {
            Guard.IsNotNull(skinnedMesh, nameof(skinnedMesh));
            Guard.IsNotNull(materialTextures, nameof(materialTextures));

            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene("SkinnedMesh");
            Node modelNode = scene.CreateNode("model");

            Dictionary<string, Material> materials = CreateGltfMaterials(root, skinnedMesh, materialTextures);
            Mesh gltfMesh = CreateGltfMesh(root, skinnedMesh, false, materials);

            modelNode.WithMesh(gltfMesh);

            root.DefaultScene = scene;
            return root;
        }

        /// <summary>
        /// Coverts the <see cref="SkinnedMesh"/> into a glTF asset with the specified skeleton, textures and animations
        /// </summary>
        /// <param name="skinnedMesh">The <see cref="SkinnedMesh"/> to covert</param>
        /// <param name="rig">The <see cref="RigResource"/> of the <see cref="SkinnedMesh"/></param>
        /// <param name="materialTextures">The texture data for the specified materials</param>
        /// <param name="animations">The animations</param>
        /// <returns>The created glTF asset</returns>
        public static ModelRoot ToGltf(
            this SkinnedMesh skinnedMesh,
            RigResource rig,
            IReadOnlyDictionary<string, Stream> materialTextures,
            IReadOnlyList<(string name, LeagueAnimation animation)> animations
        )
        {
            Guard.IsNotNull(skinnedMesh, nameof(skinnedMesh));
            Guard.IsNotNull(rig, nameof(rig));
            Guard.IsNotNull(materialTextures, nameof(materialTextures));
            Guard.IsNotNull(animations, nameof(animations));

            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene("SkinnedMesh");
            Node modelNode = scene.CreateNode("model");

            Dictionary<string, Material> materials = CreateGltfMaterials(root, skinnedMesh, materialTextures);
            Mesh gltfMesh = CreateGltfMesh(root, skinnedMesh, true, materials);
            var (influenceNodes, jointNodes) = CreateGltfSkeleton(modelNode, rig);

            // Add mesh to scene
            modelNode.WithSkinnedMesh(gltfMesh, influenceNodes.ToArray());

            // Create animations
            CreateAnimations(jointNodes, animations);

            root.DefaultScene = scene;
            return root;
        }

        /// <summary>
        /// Coverts the <see cref="ModelRoot"/> into a rigged mesh
        /// </summary>
        /// <param name="root">The <see cref="ModelRoot"/> to convert</param>
        /// <returns>The created <see cref="SkinnedMesh"/> and <see cref="Skeleton"/></returns>
        public static (SkinnedMesh, RigResource) ToRiggedMesh(this ModelRoot root)
        {
            Guard.HasSizeEqualTo(root.LogicalMeshes, 1, nameof(root.LogicalMeshes));
            Guard.HasSizeEqualTo(root.LogicalSkins, 1, nameof(root.LogicalSkins));

            Mesh mesh = root.LogicalMeshes[0];
            Skin skin = root.LogicalSkins[0];

            // Create rig
            var (rig, influenceBridgeLookup) = CreateRig(skin.VisualParents.FirstOrDefault(), skin);

            SkinnedMeshRange[] ranges = CreateSkinnedMeshRanges(mesh.Primitives);
            MemoryOwner<ushort> indexBufferOwner = CreateSkinnedMeshIndexBuffer(mesh.Primitives);
            VertexBuffer vertexBuffer = CreateSkinnedMeshVertexBuffer(mesh, ranges, influenceBridgeLookup);

            SkinnedMesh skinnedMesh = new(ranges, vertexBuffer, indexBufferOwner);

            return (skinnedMesh, rig);
        }

        #region Skinned Mesh creation
        private static SkinnedMeshRange[] CreateSkinnedMeshRanges(IReadOnlyList<MeshPrimitive> primitives)
        {
            int indexOffset = 0;
            int baseIndex = 0;
            SkinnedMeshRange[] ranges = new SkinnedMeshRange[primitives.Count];
            for (int i = 0; i < primitives.Count; i++)
            {
                MeshPrimitive primitive = primitives[i];
                int primitiveVertexCount = primitive.GetVertexAccessor("POSITION").Count;

                ranges[i] = new(
                    primitive.Material.Name,
                    baseIndex,
                    primitiveVertexCount,
                    indexOffset,
                    primitive.IndexAccessor.Count
                );

                indexOffset += primitive.IndexAccessor.Count;
                baseIndex += primitiveVertexCount;
            }

            return ranges;
        }

        private static MemoryOwner<ushort> CreateSkinnedMeshIndexBuffer(IReadOnlyList<MeshPrimitive> primitives)
        {
            int indexOffset = 0;
            int baseIndex = 0;
            int indexCount = primitives.Sum(x => x.IndexAccessor.Count);
            MemoryOwner<ushort> indexBufferOwner = MemoryOwner<ushort>.Allocate(indexCount);
            for (int primitiveId = 0; primitiveId < primitives.Count; primitiveId++)
            {
                MeshPrimitive primitive = primitives[primitiveId];
                IReadOnlyList<uint> primitiveIndices = primitive.IndexAccessor.AsIndicesArray();

                for (int i = 0; i < primitive.IndexAccessor.Count; i++)
                {
                    indexBufferOwner.Span[indexOffset + i] = (ushort)(primitiveIndices[i] + baseIndex);
                }

                indexOffset += primitiveIndices.Count;
                baseIndex += primitive.GetVertexAccessor("POSITION").Count;
            }

            return indexBufferOwner;
        }

        private static VertexBuffer CreateSkinnedMeshVertexBuffer(
            Mesh gltfMesh,
            SkinnedMeshRange[] ranges,
            byte[] influenceBridgeLookup
        )
        {
            Guard.IsNotNull(gltfMesh, nameof(gltfMesh));
            Guard.IsNotNull(ranges, nameof(ranges));
            Guard.IsNotNull(influenceBridgeLookup, nameof(influenceBridgeLookup));

            int vertexCount = ranges.Sum(range => range.VertexCount);
            VertexBufferDescription vertexBufferDescription = CreateVertexBufferDescription(gltfMesh);
            MemoryOwner<byte> vertexBufferOwner = VertexBuffer.AllocateForElements(
                vertexBufferDescription.Elements,
                vertexCount
            );
            VertexBufferWriter vertexBufferWriter = new(vertexBufferDescription.Elements, vertexBufferOwner.Memory);

            for (int primitiveId = 0; primitiveId < gltfMesh.Primitives.Count; primitiveId++)
            {
                MeshPrimitive primitive = gltfMesh.Primitives[primitiveId];
                SkinnedMeshRange range = ranges[primitiveId];

                bool hasColors = primitive.VertexAccessors.TryGetValue("COLOR_0", out Accessor colorAccessor);
                bool hasTangents = primitive.VertexAccessors.TryGetValue("TANGENT", out Accessor tangentAccessor);

                IList<Vector3> positions = primitive.VertexAccessors["POSITION"].AsVector3Array();
                IList<Vector4> joints = primitive.VertexAccessors["JOINTS_0"].AsVector4Array();
                IList<Vector4> weights = primitive.VertexAccessors["WEIGHTS_0"].AsVector4Array();
                IList<Vector3> normals = primitive.VertexAccessors["NORMAL"].AsVector3Array();
                IList<Vector2> diffuseUvs = primitive.VertexAccessors["TEXCOORD_0"].AsVector2Array();
                IList<Vector4> colors = hasColors ? colorAccessor.AsColorArray() : null;
                IList<Vector4> tangents = hasTangents ? tangentAccessor.AsVector4Array() : null;

                for (int i = 0; i < positions.Count; i++)
                {
                    int vertexId = i + range.StartVertex;
                    Vector4 vertexJoints = joints[i];
                    Vector4 vertexWeights = weights[i];

                    vertexBufferWriter.WriteVector3(vertexId, ElementName.Position, positions[i]);
                    vertexBufferWriter.WriteXyzwU8(
                        vertexId,
                        ElementName.BlendIndex,
                        (
                            vertexWeights.X > 0f ? influenceBridgeLookup[(short)vertexJoints.X] : (byte)0,
                            vertexWeights.Y > 0f ? influenceBridgeLookup[(short)vertexJoints.Y] : (byte)0,
                            vertexWeights.Z > 0f ? influenceBridgeLookup[(short)vertexJoints.Z] : (byte)0,
                            vertexWeights.W > 0f ? influenceBridgeLookup[(short)vertexJoints.W] : (byte)0
                        )
                    );
                    vertexBufferWriter.WriteVector4(vertexId, ElementName.BlendWeight, vertexWeights);
                    vertexBufferWriter.WriteVector3(vertexId, ElementName.Normal, normals[i]);
                    vertexBufferWriter.WriteVector2(vertexId, ElementName.DiffuseUV, diffuseUvs[i]);

                    if (hasColors || hasTangents)
                        vertexBufferWriter.WriteColorBgraU8(vertexId, ElementName.PrimaryColor, colors[i]);
                    if (hasTangents)
                        vertexBufferWriter.WriteVector4(vertexId, ElementName.Tangent, tangents[i]);
                }
            }

            return VertexBuffer.Create(
                vertexBufferDescription.Usage,
                vertexBufferDescription.Elements,
                vertexBufferOwner
            );
        }

        private static VertexBufferDescription CreateVertexBufferDescription(Mesh gltfMesh)
        {
            Guard.IsNotNull(gltfMesh, nameof(gltfMesh));
            Guard.HasSizeGreaterThan(gltfMesh.Primitives, 0, nameof(gltfMesh.Primitives));

            MeshPrimitive primitive = gltfMesh.Primitives[0];

            bool hasPositions = primitive.VertexAccessors.TryGetValue("POSITION", out _);
            bool hasJoints = primitive.VertexAccessors.TryGetValue("JOINTS_0", out _);
            bool hasWeights = primitive.VertexAccessors.TryGetValue("WEIGHTS_0", out _);
            bool hasNormals = primitive.VertexAccessors.TryGetValue("NORMAL", out _);
            bool hasDiffuseUvs = primitive.VertexAccessors.TryGetValue("TEXCOORD_0", out _);
            bool hasColors = primitive.VertexAccessors.TryGetValue("COLOR_0", out _);
            bool hasTangents = primitive.VertexAccessors.TryGetValue("TANGENT", out _);

            if (hasPositions is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh does not have positions");
            if (hasJoints is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh does not have joints");
            if (hasWeights is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh does not have weights");
            if (hasNormals is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh does not have normals");
            if (hasDiffuseUvs is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh does not have diffuse uvs");

            return (hasColors, hasTangents) switch
            {
                (false, false) => SkinnedMeshVertex.BASIC,
                (true, false) => SkinnedMeshVertex.COLOR,
                (true, true) => SkinnedMeshVertex.TANGENT,
                (false, true) => throw new InvalidOperationException("Mesh must have vertex colors if it has tangents")
            };
        }
        #endregion

        #region glTF Mesh creation
        private static Mesh CreateGltfMesh(
            ModelRoot root,
            SkinnedMesh skinnedMesh,
            bool isSkinned,
            IReadOnlyDictionary<string, Material> materials
        )
        {
            Guard.IsNotNull(root, nameof(root));
            Guard.IsNotNull(skinnedMesh, nameof(skinnedMesh));
            Guard.IsNotNull(materials, nameof(materials));

            Mesh gltfMesh = root.CreateMesh();

            MemoryAccessor[] meshVertexMemoryAccessors = GltfUtils
                .CreateVertexMemoryAccessors(skinnedMesh.VerticesView)
                .Where(x => isSkinned || (x.Attribute.Name is not ("JOINTS_0" or "WEIGHTS_)")))
                .ToArray();

            MemoryAccessor.SanitizeVertexAttributes(meshVertexMemoryAccessors);
            GltfUtils.SanitizeVertexMemoryAccessors(meshVertexMemoryAccessors);

            int baseVertex = 0;
            foreach (SkinnedMeshRange range in skinnedMesh.Ranges)
            {
                MemoryAccessor indicesMemoryAccessor = GltfUtils.CreateIndicesMemoryAccessor(
                    skinnedMesh.IndicesView.Slice(range.StartIndex, range.IndexCount),
                    baseVertex
                );
                MemoryAccessor[] vertexMemoryAccessors = GltfUtils.SliceVertexMemoryAccessors(
                    baseVertex,
                    range.VertexCount,
                    meshVertexMemoryAccessors
                );

                MemoryAccessor.VerifyVertexIndices(indicesMemoryAccessor, (uint)range.VertexCount);

                gltfMesh
                    .CreatePrimitive()
                    .WithMaterial(materials[range.Material])
                    .WithIndicesAccessor(PrimitiveType.TRIANGLES, indicesMemoryAccessor)
                    .WithVertexAccessors(vertexMemoryAccessors);

                baseVertex += range.VertexCount;
            }

            return gltfMesh;
        }
        #endregion

        #region glTF Material creation
        private static Dictionary<string, Material> CreateGltfMaterials(
            ModelRoot root,
            SkinnedMesh skinnedMesh,
            IReadOnlyDictionary<string, Stream> materialTextures
        )
        {
            Guard.IsNotNull(root, nameof(root));
            Guard.IsNotNull(skinnedMesh, nameof(skinnedMesh));
            Guard.IsNotNull(materialTextures, nameof(materialTextures));

            Dictionary<string, Material> materials = new();
            foreach (SkinnedMeshRange range in skinnedMesh.Ranges)
            {
                Material material = root.CreateMaterial(range.Material).WithUnlit().WithDoubleSide(true);

                if (materialTextures.TryGetValue(range.Material, out Stream textureStream))
                    InitializeMaterialBaseColorChannel(root, material, textureStream);

                materials.Add(material.Name, material);
            }

            return materials;
        }

        private static void InitializeMaterialBaseColorChannel(ModelRoot root, Material material, Stream textureStream)
        {
            Guard.IsNotNull(root, nameof(root));
            Guard.IsNotNull(material, nameof(material));
            Guard.IsNotNull(textureStream, nameof(textureStream));

            MemoryStream textureMemoryStream = new();
            textureStream.CopyTo(textureMemoryStream);

            GltfImage image = root.UseImage(new(textureMemoryStream.ToArray()));
            material.WithChannelTexture("BaseColor", 0, image);
        }
        #endregion

        private static (
            List<(Node Node, Matrix4x4 InverseBindMatrix)> Influences,
            List<Node> JointNodes
        ) CreateGltfSkeleton(Node skeletonNode, RigResource rig)
        {
            Guard.IsNotNull(skeletonNode, nameof(skeletonNode));
            Guard.IsNotNull(rig, nameof(rig));

            Matrix4x4 flipX = Matrix4x4.CreateScale(-1f, 1f, 1f);

            // We create all rig joints as nodes but only bind those which act as influences
            // In blender, this makes the un-bound joints act as locators instead of armature joints
            List<(Node, Matrix4x4)> influenceJointNodes = new();
            List<Node> jointNodes = new();

            foreach (Joint joint in rig.Joints)
            {
                bool isInfluence = rig.Influences.Any(x => x == joint.Id);

                // Root
                if (joint.ParentId is -1)
                {
                    Node jointNode = skeletonNode
                        .CreateNode(joint.Name)
                        .WithLocalTranslation(joint.LocalTranslation)
                        .WithLocalScale(joint.LocalScale)
                        .WithLocalRotation(joint.LocalRotation);

                    jointNodes.Add(jointNode);
                    if (isInfluence)
                        influenceJointNodes.Add((jointNode, joint.InverseBindTransform));
                }
                else
                {
                    Joint parentJoint = rig.Joints.FirstOrDefault(x => x.Id == joint.ParentId);
                    Node parentNode = jointNodes.FirstOrDefault(x => x.Name == parentJoint.Name);
                    Node jointNode = parentNode
                        .CreateNode(joint.Name)
                        .WithLocalTranslation(joint.LocalTranslation)
                        .WithLocalScale(joint.LocalScale)
                        .WithLocalRotation(joint.LocalRotation);

                    jointNodes.Add(jointNode);
                    if (isInfluence)
                        influenceJointNodes.Add((jointNode, joint.InverseBindTransform));
                }
            }

            return (influenceJointNodes, jointNodes);
        }

        private static void CreateAnimations(
            IReadOnlyList<Node> joints,
            IReadOnlyList<(string name, LeagueAnimation animation)> animations
        )
        {
            Guard.IsNotNull(joints, nameof(joints));
            Guard.IsNotNull(animations, nameof(animations));

            foreach (var (name, animation) in animations)
            {
                foreach (AnimationTrack track in animation.Tracks)
                {
                    Node joint = joints.FirstOrDefault(x => Elf.HashLower(x.Name) == track.JointHash);

                    if (joint is null)
                        continue;

                    if (track.Translations.Count == 0)
                        track.Translations.Add(0.0f, new Vector3(0, 0, 0));
                    if (track.Translations.Count == 1)
                        track.Translations.Add(1.0f, new Vector3(0, 0, 0));
                    joint.WithTranslationAnimation(name, track.Translations);

                    if (track.Rotations.Count == 0)
                        track.Rotations.Add(0.0f, Quaternion.Identity);
                    if (track.Rotations.Count == 1)
                        track.Rotations.Add(1.0f, Quaternion.Identity);
                    joint.WithRotationAnimation(name, track.Rotations);

                    if (track.Scales.Count == 0)
                        track.Scales.Add(0.0f, new Vector3(1, 1, 1));
                    if (track.Scales.Count == 1)
                        track.Scales.Add(1.0f, new Vector3(1, 1, 1));
                    joint.WithScaleAnimation(name, track.Scales);
                }
            }
        }

        #region glTF -> Rig Resource
        private static (RigResource Rig, byte[] InfluenceBridgeLookup) CreateRig(Node skeletonNode, Skin skin)
        {
            Guard.IsNotNull(skeletonNode, nameof(skeletonNode));
            Guard.IsNotNull(skin, nameof(skin));

            RigResourceBuilder rigBuilder = new();

            // Build rig joints
            List<Node> jointNodes = TraverseJointNodes(skeletonNode).ToList();
            List<JointBuilder> joints = new(jointNodes.Count);
            foreach (Node jointNode in jointNodes)
                CreateRigJointFromGltfNode(rigBuilder, joints, jointNode, skeletonNode);

            // Build rig
            RigResource rig = rigBuilder.Build();

            // We need to map the vertex joint ids to the influences in the built rig
            byte[] influenceBridgeLookup = new byte[skin.JointsCount];
            for (int i = 0; i < skin.JointsCount; i++)
            {
                // Get the influence node
                Node jointNode = skin.GetJoint(i).Joint;

                // Find the rig joint and throw if it doesn't exist
                Joint influenceJoint = rig.Joints.FirstOrDefault(x => x.Name == jointNode.Name);
                if (influenceJoint is null)
                    ThrowHelper.ThrowInvalidOperationException($"Failed to find joint for node: {jointNode.Name}");

                // Find the id of the influence mapping in the rig which matches the joint
                int influenceId = rig.Influences.IndexOf(influenceJoint.Id);
                if (influenceId is -1)
                    ThrowHelper.ThrowInvalidOperationException(
                        $"Failed to find influence id for joint: {influenceJoint.Name}"
                    );

                influenceBridgeLookup[i] = (byte)influenceId;
            }

            return (rig, influenceBridgeLookup);
        }

        private static JointBuilder CreateRigJointFromGltfNode(
            RigResourceBuilder rigBuilder,
            List<JointBuilder> joints,
            Node jointNode,
            Node skeletonNode
        )
        {
            // This is to prevent duplicate joints since we're creating them recursively
            if (joints.Find(x => x.Name == jointNode.Name) is JointBuilder existingJoint)
                return existingJoint;

            Matrix4x4.Invert(jointNode.WorldMatrix, out Matrix4x4 inverseBindTransform);

            if (jointNode.VisualParent is null || jointNode.VisualParent == skeletonNode)
            {
                JointBuilder joint = rigBuilder
                    .CreateJoint(jointNode.Name)
                    .WithInfluence(jointNode.IsSkinJoint)
                    .WithLocalTransform(jointNode.LocalMatrix)
                    .WithInverseBindTransform(inverseBindTransform);

                joints.Add(joint);

                return joint;
            }
            else
            {
                // Find joint parent and create create it recursively if it doesn't exist yet
                JointBuilder parent = joints.Find(x => x.Name == jointNode.VisualParent.Name);
                parent ??= CreateRigJointFromGltfNode(rigBuilder, joints, jointNode.VisualParent, skeletonNode);

                JointBuilder joint = parent
                    .CreateJoint(jointNode.Name)
                    .WithInfluence(jointNode.IsSkinJoint)
                    .WithLocalTransform(jointNode.LocalMatrix)
                    .WithInverseBindTransform(inverseBindTransform);

                joints.Add(joint);

                return joint;
            }
        }

        private static IEnumerable<Node> TraverseJointNodes(Node node)
        {
            IEnumerable<Node> jointNodes = node.VisualChildren.Where(
                node => node.Skin is null && node.Mesh is null && node.Camera is null
            );
            foreach (Node joint in jointNodes)
            {
                yield return joint;

                foreach (Node jointChild in TraverseJointNodes(joint))
                    yield return jointChild;
            }
        }
        #endregion
    }
}
