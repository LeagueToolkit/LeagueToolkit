using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Animation;
using LeagueToolkit.Core.Animation.Builders;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.Hashing;
using LeagueToolkit.IO.Extensions.Utils;
using LeagueToolkit.Utils.Extensions;
using SharpGLTF.Memory;
using SharpGLTF.Schema2;
using GltfImage = SharpGLTF.Schema2.Image;

namespace LeagueToolkit.IO.SimpleSkinFile
{
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
            IEnumerable<(string Material, Stream Texture)> textures
        )
        {
            Guard.IsNotNull(skinnedMesh, nameof(skinnedMesh));
            Guard.IsNotNull(textures, nameof(textures));

            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene("SkinnedMesh");
            Node meshNode = scene.CreateNode("skinned_mesh");

            Dictionary<string, Material> materials = CreateGltfMaterials(root, skinnedMesh, textures);
            Mesh gltfMesh = CreateGltfMesh(root, skinnedMesh, null, materials);

            meshNode.WithMesh(gltfMesh);

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
            IEnumerable<(string Material, Stream Texture)> textures,
            IEnumerable<(string Name, IAnimationAsset Animation)> animations
        )
        {
            Guard.IsNotNull(skinnedMesh, nameof(skinnedMesh));
            Guard.IsNotNull(rig, nameof(rig));
            Guard.IsNotNull(textures, nameof(textures));
            Guard.IsNotNull(animations, nameof(animations));

            ModelRoot root = ModelRoot.CreateModel();
            Scene scene = root.UseScene("SkinnedMesh");
            Node meshNode = scene.CreateNode("skinned_mesh");

            Dictionary<string, Material> materials = CreateGltfMaterials(root, skinnedMesh, textures);
            var (jointNodes, influenceLookup) = CreateGltfSkeleton(rig, meshNode);

            Mesh gltfMesh = CreateGltfMesh(root, skinnedMesh, influenceLookup, materials);

            // Add mesh to scene
            meshNode.WithSkinnedMesh(gltfMesh, jointNodes.ToArray());

            // Create animations
            CreateAnimations(jointNodes.Select(x => x.Item1), animations);

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
            bool[] isJointInfluenceLookup = CreateIsJointInfluenceLookup(mesh, skin);
            var (rig, influenceLookup) = CreateRig(skin, isJointInfluenceLookup);

            SkinnedMeshRange[] ranges = CreateSkinnedMeshRanges(mesh.Primitives);
            IndexBuffer indexBuffer = CreateSkinnedMeshIndexBuffer(mesh.Primitives);
            VertexBuffer vertexBuffer = CreateSkinnedMeshVertexBuffer(mesh, ranges, influenceLookup);

            SkinnedMesh skinnedMesh = new(ranges, vertexBuffer, indexBuffer);

            return (skinnedMesh, rig);
        }

        #region Skinned Mesh creation
        private static bool[] CreateIsJointInfluenceLookup(Mesh gltfMesh, Skin skin)
        {
            // We want to collect info about which joints are actually influences
            bool[] isJointInfluenceLookup = new bool[skin.JointsCount];

            foreach (MeshPrimitive primitive in gltfMesh.Primitives)
            {
                IList<Vector4> jointsArray = primitive.VertexAccessors["JOINTS_0"].AsVector4Array();
                IList<Vector4> weightsArray = primitive.VertexAccessors["WEIGHTS_0"].AsVector4Array();

                for (int i = 0; i < jointsArray.Count; i++)
                {
                    Vector4 joints = jointsArray[i];
                    Vector4 weights = weightsArray[i];

                    if (weights.X > 0.0f)
                        isJointInfluenceLookup[(byte)joints.X] = true;
                    if (weights.Y > 0.0f)
                        isJointInfluenceLookup[(byte)joints.Y] = true;
                    if (weights.Z > 0.0f)
                        isJointInfluenceLookup[(byte)joints.Z] = true;
                    if (weights.W > 0.0f)
                        isJointInfluenceLookup[(byte)joints.W] = true;
                }
            }

            return isJointInfluenceLookup;
        }

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

        private static IndexBuffer CreateSkinnedMeshIndexBuffer(IReadOnlyList<MeshPrimitive> gltfMeshPrimitives)
        {
            int indexOffset = 0;
            int baseIndex = 0;
            int indexSize = IndexBuffer.GetFormatSize(IndexFormat.U16);
            int indexCount = gltfMeshPrimitives.Sum(x => x.IndexAccessor.Count);

            MemoryOwner<byte> indexBufferOwner = MemoryOwner<byte>.Allocate(indexCount * indexSize);

            // TODO: Index Buffer writer
            for (int primitiveId = 0; primitiveId < gltfMeshPrimitives.Count; primitiveId++)
            {
                MeshPrimitive gltfPrimitive = gltfMeshPrimitives[primitiveId];
                IReadOnlyList<uint> primitiveIndices = gltfPrimitive.IndexAccessor.AsIndicesArray();

                for (int i = 0; i < gltfPrimitive.IndexAccessor.Count; i++)
                {
                    ushort index = (ushort)(primitiveIndices[i] + baseIndex);
                    MemoryMarshal.Write(indexBufferOwner.Span[((indexOffset + i) * indexSize)..], ref index);
                }

                indexOffset += primitiveIndices.Count;
                baseIndex += gltfPrimitive.GetVertexAccessor("POSITION").Count;
            }

            return IndexBuffer.Create(IndexFormat.U16, indexBufferOwner);
        }

        private static VertexBuffer CreateSkinnedMeshVertexBuffer(
            Mesh gltfMesh,
            SkinnedMeshRange[] ranges,
            byte[] influenceLookup
        )
        {
            Guard.IsNotNull(gltfMesh, nameof(gltfMesh));
            Guard.IsNotNull(ranges, nameof(ranges));
            Guard.IsNotNull(influenceLookup, nameof(influenceLookup));

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
                            vertexWeights.X > 0f ? influenceLookup[(short)vertexJoints.X] : (byte)0,
                            vertexWeights.Y > 0f ? influenceLookup[(short)vertexJoints.Y] : (byte)0,
                            vertexWeights.Z > 0f ? influenceLookup[(short)vertexJoints.Z] : (byte)0,
                            vertexWeights.W > 0f ? influenceLookup[(short)vertexJoints.W] : (byte)0
                        )
                    );
                    vertexBufferWriter.WriteVector4(vertexId, ElementName.BlendWeight, vertexWeights);
                    vertexBufferWriter.WriteVector3(vertexId, ElementName.Normal, normals[i]);
                    vertexBufferWriter.WriteVector2(vertexId, ElementName.Texcoord0, diffuseUvs[i]);

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
            byte[] influenceLookup,
            IReadOnlyDictionary<string, Material> materials
        )
        {
            Guard.IsNotNull(root, nameof(root));
            Guard.IsNotNull(skinnedMesh, nameof(skinnedMesh));
            Guard.IsNotNull(materials, nameof(materials));

            Mesh gltfMesh = root.CreateMesh();

            MemoryAccessor[] meshVertexMemoryAccessors =
            [
                .. GltfUtils
                    .CreateVertexMemoryAccessors(skinnedMesh.VerticesView)
                    .Where(x => influenceLookup is not null || (x.Attribute.Name is not ("JOINTS_0" or "WEIGHTS_0")))
            ];

            MemoryAccessor.SanitizeVertexAttributes(meshVertexMemoryAccessors);

            if (influenceLookup is not null)
                SanitizeVertexSkinningAttributes(meshVertexMemoryAccessors, influenceLookup);

            int baseVertex = 0;
            foreach (SkinnedMeshRange range in skinnedMesh.Ranges)
            {
                MemoryAccessor indicesMemoryAccessor = GltfUtils.CreateIndicesMemoryAccessor(
                    skinnedMesh.Indices.Slice(range.StartIndex, range.IndexCount),
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

        private static void SanitizeVertexSkinningAttributes(
            IEnumerable<MemoryAccessor> memoryAccessors,
            IReadOnlyList<byte> influenceLookup
        )
        {
            MemoryAccessor jointsAccessor = memoryAccessors.FirstOrDefault(x => x.Attribute.Name is "JOINTS_0");
            if (jointsAccessor is null)
                ThrowHelper.ThrowInvalidOperationException("Failed to find JOINTS_0 memory accessor");

            MemoryAccessor weightsAccessor = memoryAccessors.FirstOrDefault(x => x.Attribute.Name is "WEIGHTS_0");
            if (weightsAccessor is null)
                ThrowHelper.ThrowInvalidOperationException("Failed to find WEIGHTS_0 memory accessor");

            Vector4Array jointsArray = jointsAccessor.AsVector4Array();
            Vector4Array weightsArray = weightsAccessor.AsVector4Array();

            for (int i = 0; i < jointsArray.Count; i++)
            {
                Vector4 joints = jointsArray[i];
                Vector4 weights = weightsArray[i];

                jointsArray[i] = new(
                    weights.X > 0.0f ? influenceLookup[(byte)joints.X] : joints.X,
                    weights.Y > 0.0f ? influenceLookup[(byte)joints.Y] : joints.Y,
                    weights.Z > 0.0f ? influenceLookup[(byte)joints.Z] : joints.Z,
                    weights.W > 0.0f ? influenceLookup[(byte)joints.W] : joints.W
                );
            }
        }
        #endregion

        #region glTF Material creation
        private static Dictionary<string, Material> CreateGltfMaterials(
            ModelRoot root,
            SkinnedMesh skinnedMesh,
            IEnumerable<(string Material, Stream Texture)> textures
        )
        {
            Guard.IsNotNull(root, nameof(root));
            Guard.IsNotNull(skinnedMesh, nameof(skinnedMesh));
            Guard.IsNotNull(textures, nameof(textures));

            Dictionary<string, Stream> textureLookup =
                new(textures.Select(x => new KeyValuePair<string, Stream>(x.Material, x.Texture)));

            Dictionary<string, Material> materials = new();
            foreach (SkinnedMeshRange primitive in skinnedMesh.Ranges)
            {
                Material material = root.CreateMaterial(primitive.Material).WithUnlit().WithDoubleSide(true);

                if (textureLookup.TryGetValue(primitive.Material, out Stream textureStream))
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

            byte[] textureBuffer = new byte[textureStream.Length];
            textureStream.Read(textureBuffer);

            GltfImage image = root.UseImage(new(textureBuffer));
            material.WithChannelTexture("BaseColor", 0, image);
        }
        #endregion

        #region glTF Skeleton creation
        private static (List<(Node, Matrix4x4)>, byte[]) CreateGltfSkeleton(RigResource rig, Node skeletonRoot)
        {
            Guard.IsNotNull(rig, nameof(rig));
            Guard.IsNotNull(skeletonRoot, nameof(skeletonRoot));

            List<(Node, Matrix4x4)> jointNodes = new();
            foreach (Joint joint in rig.Joints)
                CreateGltfSkeletonJoint(joint, skeletonRoot, rig, jointNodes);

            byte[] influenceLookup = new byte[rig.Influences.Count];
            for (int i = 0; i < rig.Joints.Count; i++)
            {
                Joint joint = rig.Joints[i];

                // Check if the joint is an influence
                int influenceId = rig.Influences.IndexOf(joint.Id);
                if (influenceId is -1)
                    continue;

                // Get the joint node index
                int nodeId = jointNodes.FindIndex(((Node Node, Matrix4x4 _) x) => x.Node.Name == joint.Name);
                if (nodeId > byte.MaxValue)
                    ThrowHelper.ThrowInvalidOperationException("Cannot have more than 256 influences");

                influenceLookup[influenceId] = (byte)nodeId;
            }

            return (jointNodes, influenceLookup);
        }

        private static Node CreateGltfSkeletonJoint(
            Joint joint,
            Node skeletonRoot,
            RigResource rig,
            List<(Node JointNode, Matrix4x4 InverseBindTransform)> jointNodes
        )
        {
            if (skeletonRoot.FindNode(x => x.Name == joint.Name) is Node existingJoint)
                return existingJoint;

            // Root
            if (joint.ParentId is -1)
            {
                Node jointNode = skeletonRoot
                    .CreateNode(joint.Name)
                    .WithLocalTranslation(joint.LocalTranslation)
                    .WithLocalScale(joint.LocalScale)
                    .WithLocalRotation(joint.LocalRotation);

                jointNodes.Add((jointNode, joint.InverseBindTransform));

                return jointNode;
            }
            else
            {
                Joint parentJoint = rig.Joints.FirstOrDefault(x => x.Id == joint.ParentId);
                Node parentNode = jointNodes.FirstOrDefault(x => x.JointNode.Name == parentJoint.Name).JointNode;
                parentNode ??= CreateGltfSkeletonJoint(parentJoint, skeletonRoot, rig, jointNodes);

                Node jointNode = parentNode
                    .CreateNode(joint.Name)
                    .WithLocalTranslation(joint.LocalTranslation)
                    .WithLocalScale(joint.LocalScale)
                    .WithLocalRotation(joint.LocalRotation);

                jointNodes.Add((jointNode, joint.InverseBindTransform));

                return jointNode;
            }
        }
        #endregion

        #region glTF Animation creation
        private static void CreateAnimations(
            IEnumerable<Node> joints,
            IEnumerable<(string Name, IAnimationAsset Animation)> animations
        )
        {
            Guard.IsNotNull(joints, nameof(joints));
            Guard.IsNotNull(animations, nameof(animations));

            IReadOnlyList<Node> jointNodes = joints.ToArray();
            foreach (var (name, animation) in animations)
            {
                CreateGltfAnimation(name, animation, jointNodes);
            }
        }

        private static void CreateGltfAnimation(
            string animationName,
            IAnimationAsset animation,
            IReadOnlyList<Node> jointNodes
        )
        {
            Dictionary<uint, (Quaternion Rotation, Vector3 Translation, Vector3 Scale)> pose = new();

            int frameCount = (int)(animation.Fps * animation.Duration);
            Dictionary<uint, (float, Quaternion)[]> jointRotations = new(frameCount);
            Dictionary<uint, (float, Vector3)[]> jointTranslations = new(frameCount);
            Dictionary<uint, (float, Vector3)[]> jointScales = new(frameCount);

            // Re-sample the animation in linear time
            float frameDuration = 1 / animation.Fps;
            for (int frameId = 0; frameId < frameCount; frameId++)
            {
                float frameTime = frameId * frameDuration;

                // Evaluate the pose for the current frame
                animation.Evaluate(frameTime, pose);

                foreach (var (jointHash, transform) in pose)
                {
                    if (!jointRotations.ContainsKey(jointHash))
                        jointRotations.Add(jointHash, new (float, Quaternion)[frameCount]);
                    jointRotations[jointHash][frameId] = (frameTime, transform.Rotation);

                    if (!jointTranslations.ContainsKey(jointHash))
                        jointTranslations.Add(jointHash, new (float, Vector3)[frameCount]);
                    jointTranslations[jointHash][frameId] = (frameTime, transform.Translation);

                    if (!jointScales.ContainsKey(jointHash))
                        jointScales.Add(jointHash, new (float, Vector3)[frameCount]);
                    jointScales[jointHash][frameId] = (frameTime, transform.Scale);
                }
            }

            // Create samplers for joints
            foreach (Node jointNode in jointNodes)
            {
                uint jointHash = Elf.HashLower(jointNode.Name);

                if (jointRotations.TryGetValue(jointHash, out var rotationFrames))
                    jointNode.WithRotationAnimation(animationName, rotationFrames);

                if (jointTranslations.TryGetValue(jointHash, out var translationFrames))
                    jointNode.WithTranslationAnimation(animationName, translationFrames);

                if (jointScales.TryGetValue(jointHash, out var scaleFrames))
                    jointNode.WithScaleAnimation(animationName, scaleFrames);
            }
        }
        #endregion

        #region glTF -> Rig Resource
        private static (RigResource Rig, byte[] InfluenceLookup) CreateRig(
            Skin skin,
            IReadOnlyList<bool> isJointInfluenceLookup
        )
        {
            Guard.IsNotNull(skin, nameof(skin));
            Guard.IsNotNull(isJointInfluenceLookup, nameof(isJointInfluenceLookup));

            Node rootNode = GltfUtils.FindRootNode(skin.VisualParents.FirstOrDefault());

            RigResourceBuilder rigBuilder = new();

            // Build rig joints
            (Node Joint, Matrix4x4)[] jointNodes = Enumerable
                .Range(0, skin.JointsCount)
                .Select(skin.GetJoint)
                .ToArray();
            List<JointBuilder> joints = new(jointNodes.Length);

            for (int i = 0; i < jointNodes.Length; i++)
            {
                JointBuilder joint = CreateRigJointFromGltfNode(
                    rigBuilder,
                    joints,
                    jointNodes[i].Joint,
                    jointNodes,
                    rootNode
                );

                joint.WithInfluence(isJointInfluenceLookup[i]);
            }

            // Build rig
            RigResource rig = rigBuilder.Build();

            // We need to map the vertex joint ids to the influences in the built rig
            byte[] influenceLookup = new byte[skin.JointsCount];
            for (int i = 0; i < rig.Influences.Count; i++)
            {
                short influenceJointId = rig.Influences[i];
                Joint influenceJoint = rig.Joints[influenceJointId];

                int reverseLookupId = Array.FindIndex(jointNodes, x => x.Joint.Name == influenceJoint.Name);
                if (reverseLookupId is -1)
                    ThrowHelper.ThrowInvalidOperationException($"Failed to find node for joint: {influenceJoint.Name}");

                influenceLookup[reverseLookupId] = (byte)i;
            }

            return (rig, influenceLookup);
        }

        private static JointBuilder CreateRigJointFromGltfNode(
            RigResourceBuilder rigBuilder,
            List<JointBuilder> joints,
            Node jointNode,
            IReadOnlyList<(Node Joint, Matrix4x4 InverseBindTransform)> jointNodes,
            Node skeletonNode
        )
        {
            // This is to prevent duplicate joints since we're creating them recursively
            if (joints.Find(x => x.Name == jointNode.Name) is JointBuilder existingJoint)
                return existingJoint;

            Matrix4x4 inverseBindTransform = jointNodes.First(x => x.Joint == jointNode).InverseBindTransform;

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
                parent ??= CreateRigJointFromGltfNode(
                    rigBuilder,
                    joints,
                    jointNode.VisualParent,
                    jointNodes,
                    skeletonNode
                );

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
            IEnumerable<Node> jointNodes = node.VisualChildren.Where(node =>
                node.Skin is null && node.Mesh is null && node.Camera is null
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
