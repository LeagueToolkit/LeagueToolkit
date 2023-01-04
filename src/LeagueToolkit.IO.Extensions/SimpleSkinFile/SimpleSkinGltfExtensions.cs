using CommunityToolkit.Diagnostics;
using CommunityToolkit.HighPerformance.Buffers;
using LeagueToolkit.Core.Memory;
using LeagueToolkit.Core.Mesh;
using LeagueToolkit.Helpers.Cryptography;
using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.IO.AnimationFile;
using LeagueToolkit.IO.SkeletonFile;
using SharpGLTF.Animations;
using SharpGLTF.Geometry;
using SharpGLTF.Geometry.VertexTypes;
using SharpGLTF.Materials;
using SharpGLTF.Scenes;
using SharpGLTF.Schema2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using LeagueAnimation = LeagueToolkit.IO.AnimationFile.Animation;

namespace LeagueToolkit.IO.SimpleSkinFile
{
    using VERTEX_SKINNED = VertexBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4>;
    using VERTEX_SKINNED_COLOR = VertexBuilder<VertexPositionNormal, VertexColor1Texture1, VertexJoints4>;
    using VERTEX_SKINNED_TANGENT = VertexBuilder<VertexPositionNormalTangent, VertexColor1Texture1, VertexJoints4>;

    public static class SimpleSkinGltfExtensions
    {
        public static ModelRoot ToGltf(
            this SkinnedMesh skinnedMesh,
            Skeleton skeleton,
            IReadOnlyDictionary<string, ReadOnlyMemory<byte>> materialTextues,
            IReadOnlyList<(string name, LeagueAnimation animation)> animations
        )
        {
            Guard.IsNotNull(skeleton, nameof(skeleton));
            Guard.IsNotNull(materialTextues, nameof(materialTextues));
            Guard.IsNotNull(animations, nameof(animations));

            SceneBuilder sceneBuilder = new();
            NodeBuilder rootNodeBuilder = new("model");

            var meshBuilder = CreateVertexSkinnedMesh(skinnedMesh, skeleton, materialTextues);
            var bones = CreateSkeleton(rootNodeBuilder, skeleton);

            // Add mesh to scene
            sceneBuilder.AddSkinnedMesh(meshBuilder, bones.ToArray());

            // Create animations
            CreateAnimations(bones.Select(x => x.Node).ToList(), animations);

            // Flip the scene across the X axis
            sceneBuilder.ApplyBasisTransform(Matrix4x4.CreateScale(new Vector3(-1, 1, 1)));

            return sceneBuilder.ToGltf2();
        }

        private static MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4> CreateVertexSkinnedMesh(
            SkinnedMesh skinnedMesh,
            Skeleton skeleton,
            IReadOnlyDictionary<string, ReadOnlyMemory<byte>> materialTextues
        )
        {
            Guard.IsNotNull(skinnedMesh, nameof(skinnedMesh));
            Guard.IsNotNull(skeleton, nameof(skeleton));
            Guard.IsNotNull(materialTextues, nameof(materialTextues));

            MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4> meshBuilder =
                VERTEX_SKINNED.CreateCompatibleMesh();

            IVertexBuilder[] vertices = CreateVertices(skinnedMesh, skeleton);

            foreach (SkinnedMeshRange range in skinnedMesh.Ranges)
            {
                MaterialBuilder material = new MaterialBuilder(range.Material).WithUnlitShader();
                var primitiveBuilder = meshBuilder.UsePrimitive(material);

                // Assign texture to material
                if (materialTextues.TryGetValue(range.Material, out ReadOnlyMemory<byte> textureMemory))
                    AssignMaterialTexture(material, textureMemory);

                // Add vertices to primitive
                ReadOnlySpan<ushort> indices = skinnedMesh.IndicesView.Span.Slice(range.StartIndex, range.IndexCount);
                for (int i = 0; i < indices.Length; i += 3)
                {
                    IVertexBuilder v1 = vertices[indices[i + 0]];
                    IVertexBuilder v2 = vertices[indices[i + 1]];
                    IVertexBuilder v3 = vertices[indices[i + 2]];

                    primitiveBuilder.AddTriangle(v1, v2, v3);
                }
            }

            return meshBuilder;
        }

        private static IVertexBuilder[] CreateVertices(SkinnedMesh skinnedMesh, Skeleton skeleton)
        {
            Guard.IsNotNull(skinnedMesh, nameof(skinnedMesh));
            Guard.IsNotNull(skeleton, nameof(skeleton));

            bool hasPrimaryColor = skinnedMesh.VerticesView.TryGetAccessor(
                ElementName.PrimaryColor,
                out var primaryColorAccessor
            );
            bool hasTangents = skinnedMesh.VerticesView.TryGetAccessor(ElementName.Tangent, out var tangentAccessor);

            IVertexBuilder[] vertices = new IVertexBuilder[skinnedMesh.VerticesView.VertexCount];
            VertexElementArray<Vector3> positions = skinnedMesh.VerticesView
                .GetAccessor(ElementName.Position)
                .AsVector3Array();
            VertexElementArray<Vector4> boneWeights = skinnedMesh.VerticesView
                .GetAccessor(ElementName.BlendWeight)
                .AsVector4Array();
            VertexElementArray<(byte x, byte y, byte z, byte w)> boneIndices = skinnedMesh.VerticesView
                .GetAccessor(ElementName.BlendIndex)
                .AsXyzwU8Array();
            VertexElementArray<Vector3> normals = skinnedMesh.VerticesView
                .GetAccessor(ElementName.Normal)
                .AsVector3Array();
            VertexElementArray<Vector2> diffuseUvs = skinnedMesh.VerticesView
                .GetAccessor(ElementName.DiffuseUV)
                .AsVector2Array();
            VertexElementArray<(byte b, byte g, byte r, byte a)> primaryColors = hasPrimaryColor
                ? primaryColorAccessor.AsBgraU8Array()
                : default;
            VertexElementArray<Vector4> tangents = hasTangents ? tangentAccessor.AsVector4Array() : default;

            for (int i = 0; i < vertices.Length; i++)
            {
                var joints = boneIndices[i];
                Vector4 jointWeights = boneWeights[i];

                IVertexBuilder vertex = (hasPrimaryColor, hasTangents) switch
                {
                    (false, false) => new VERTEX_SKINNED(),
                    (true, false) => new VERTEX_SKINNED_COLOR(),
                    (true, true) => new VERTEX_SKINNED_TANGENT(),
                    (false, true) => throw new InvalidOperationException("Mesh must have colors if it has tangents"),
                };

                vertex.SetGeometry(
                    hasTangents switch
                    {
                        true => new VertexPositionNormalTangent(positions[i], normals[i], tangents[i]),
                        false => new VertexPositionNormal(positions[i], normals[i]),
                    }
                );
                vertex.SetMaterial(
                    hasPrimaryColor switch
                    {
                        true
                            => new VertexColor1Texture1(
                                new Vector4(
                                    primaryColors[i].r / 255,
                                    primaryColors[i].g / 255,
                                    primaryColors[i].b / 255,
                                    primaryColors[i].a / 255
                                ),
                                diffuseUvs[i]
                            ),
                        false => new VertexTexture1(diffuseUvs[i])
                    }
                );
                vertex.SetSkinning(
                    new VertexJoints4(
                        new (int, float)[]
                        {
                            (skeleton.Influences[joints.x], jointWeights.X),
                            (skeleton.Influences[joints.y], jointWeights.Y),
                            (skeleton.Influences[joints.z], jointWeights.Z),
                            (skeleton.Influences[joints.w], jointWeights.W)
                        }
                    )
                );

                vertices[i] = vertex;
            }

            return vertices;
        }

        private static void AssignMaterialTexture(
            MaterialBuilder materialBuilder,
            ReadOnlyMemory<byte> textureMemory
        ) => materialBuilder.UseChannel(KnownChannel.BaseColor).UseTexture().WithPrimaryImage(textureMemory.ToArray());

        private static List<(NodeBuilder Node, Matrix4x4 InverseBindMatrix)> CreateSkeleton(
            NodeBuilder rootNode,
            Skeleton skeleton
        )
        {
            Guard.IsNotNull(rootNode, nameof(rootNode));
            Guard.IsNotNull(skeleton, nameof(skeleton));

            var joints = new List<(NodeBuilder, Matrix4x4)>();

            foreach (SkeletonJoint joint in skeleton.Joints)
            {
                // Root
                if (joint.ParentID == -1)
                {
                    NodeBuilder jointNode = rootNode.CreateNode(joint.Name);

                    jointNode.LocalTransform = joint.LocalTransform;

                    joints.Add((jointNode, joint.InverseBindTransform));
                }
                else
                {
                    SkeletonJoint parentJoint = skeleton.Joints.FirstOrDefault(x => x.ID == joint.ParentID);
                    NodeBuilder parentNode = joints.FirstOrDefault(x => x.Item1.Name == parentJoint.Name).Item1;
                    NodeBuilder jointNode = parentNode.CreateNode(joint.Name);

                    jointNode.LocalTransform = joint.LocalTransform;

                    joints.Add((jointNode, joint.InverseBindTransform));
                }
            }

            return joints;
        }

        private static void CreateAnimations(
            IReadOnlyList<NodeBuilder> joints,
            IReadOnlyList<(string name, LeagueAnimation animation)> animations
        )
        {
            Guard.IsNotNull(joints, nameof(joints));
            Guard.IsNotNull(animations, nameof(animations));

            foreach (var (name, animation) in animations)
            {
                foreach (AnimationTrack track in animation.Tracks)
                {
                    NodeBuilder joint = joints.FirstOrDefault(x => Cryptography.ElfHash(x.Name) == track.JointHash);

                    if (joint is null)
                        continue;

                    if (track.Translations.Count == 0)
                        track.Translations.Add(0.0f, new Vector3(0, 0, 0));
                    if (track.Translations.Count == 1)
                        track.Translations.Add(1.0f, new Vector3(0, 0, 0));
                    CurveBuilder<Vector3> translationBuilder = joint.UseTranslation().UseTrackBuilder(name);
                    foreach (var translation in track.Translations)
                    {
                        translationBuilder.SetPoint(translation.Key, translation.Value);
                    }

                    if (track.Rotations.Count == 0)
                        track.Rotations.Add(0.0f, Quaternion.Identity);
                    if (track.Rotations.Count == 1)
                        track.Rotations.Add(1.0f, Quaternion.Identity);
                    CurveBuilder<Quaternion> rotationBuilder = joint.UseRotation().UseTrackBuilder(name);
                    foreach (var rotation in track.Rotations)
                    {
                        rotationBuilder.SetPoint(rotation.Key, rotation.Value);
                    }

                    if (track.Scales.Count == 0)
                        track.Scales.Add(0.0f, new Vector3(1, 1, 1));
                    if (track.Scales.Count == 1)
                        track.Scales.Add(1.0f, new Vector3(1, 1, 1));
                    CurveBuilder<Vector3> scaleBuilder = joint.UseScale().UseTrackBuilder(name);
                    foreach (var scale in track.Scales.ToList())
                    {
                        scaleBuilder.SetPoint(scale.Key, scale.Value);
                    }
                }
            }
        }

        public static (SkinnedMesh, Skeleton) ToRiggedMesh(this ModelRoot root)
        {
            Guard.HasSizeEqualTo(root.LogicalMeshes, 1, nameof(root.LogicalMeshes));
            Guard.HasSizeEqualTo(root.LogicalSkins, 1, nameof(root.LogicalSkins));

            Mesh mesh = root.LogicalMeshes[0];
            List<(MeshPrimitive primitive, IList<uint> indices)> primitiveIndices = mesh.Primitives
                .Select(primitive => (primitive, primitive.GetIndices()))
                .ToList();

            // Create ranges and index buffer
            int indexOffset = 0;
            int indexCount = primitiveIndices.Sum(x => x.indices.Count);
            MemoryOwner<ushort> indexBufferOwner = MemoryOwner<ushort>.Allocate(indexCount);
            SkinnedMeshRange[] ranges = new SkinnedMeshRange[mesh.Primitives.Count];
            for (int primitiveId = 0; primitiveId < primitiveIndices.Count; primitiveId++)
            {
                var (primitive, indices) = primitiveIndices[primitiveId];

                uint minIndex = indices.Min();
                uint maxIndex = indices.Max();
                for (int i = 0; i < indices.Count; i++)
                {
                    indexBufferOwner.Span[indexOffset + i] = (ushort)(indices[i] - minIndex);
                }

                ranges[primitiveId] = new(
                    primitive.Material.Name,
                    (int)minIndex,
                    (int)(maxIndex - minIndex + 1),
                    indexOffset,
                    indices.Count
                );
                indexOffset += indices.Count;
            }

            // Create vertex buffer
            int vertexCount = ranges.Sum(range => range.VertexCount);
            VertexBufferDescription vertexBufferDescription = CreateRiggedMeshVertexBufferDescription(root);
            MemoryOwner<byte> vertexBufferOwner = MemoryOwner<byte>.Allocate(
                vertexCount * vertexBufferDescription.GetVertexSize()
            );
            VertexBufferWriter vertexBufferWriter = new(vertexBufferDescription.Elements, vertexBufferOwner.Memory);

            List<byte> influences = new();
            for (int primitiveId = 0; primitiveId < mesh.Primitives.Count; primitiveId++)
            {
                MeshPrimitive primitive = mesh.Primitives[primitiveId];
                SkinnedMeshRange range = ranges[primitiveId];

                bool hasPositions = primitive.VertexAccessors.TryGetValue("POSITION", out var positionAccessor);
                bool hasJoints = primitive.VertexAccessors.TryGetValue("JOINTS_0", out var jointsAccessor);
                bool hasWeights = primitive.VertexAccessors.TryGetValue("WEIGHTS_0", out var weightsAccessor);
                bool hasNormals = primitive.VertexAccessors.TryGetValue("NORMAL", out var normalAccessor);
                bool hasDiffuseUvs = primitive.VertexAccessors.TryGetValue("TEXCOORD_0", out var diffuseUvAccessor);
                bool hasColors = primitive.VertexAccessors.TryGetValue("COLOR_0", out var colorAccessor);
                bool hasTangents = primitive.VertexAccessors.TryGetValue("TANGENT_0", out var tangentAccessor);

                IList<Vector3> positions = hasPositions ? positionAccessor.AsVector3Array() : null;
                IList<Vector4> joints = hasJoints ? jointsAccessor.AsVector4Array() : null;
                IList<Vector4> weights = hasWeights ? weightsAccessor.AsVector4Array() : null;
                IList<Vector3> normals = hasNormals ? positionAccessor.AsVector3Array() : null;
                IList<Vector2> diffuseUvs = hasDiffuseUvs ? diffuseUvAccessor.AsVector2Array() : null;
                IList<Vector4> colors = hasJoints ? colorAccessor.AsColorArray() : null;
                IList<Vector4> tangents = hasJoints ? tangentAccessor.AsVector4Array() : null;

                for (int i = 0; i < positions.Count; i++)
                {
                    Vector4 vertexJoints = joints[i];
                    byte[] vertexJointsArray = new[]
                    {
                        (byte)vertexJoints.X,
                        (byte)vertexJoints.Y,
                        (byte)vertexJoints.Z,
                        (byte)vertexJoints.W
                    };
                    for (int joint = 0; joint < vertexJointsArray.Length; joint++)
                    {
                        if (!influences.Any(x => x == vertexJointsArray[joint]))
                        {
                            influences.Add(vertexJointsArray[joint]);
                        }
                    }

                    vertexBufferWriter.WriteVector3(i, ElementName.Position, positions[i]);
                    vertexBufferWriter.WriteXyzwU8(
                        i,
                        ElementName.BlendIndex,
                        (vertexJointsArray[0], vertexJointsArray[1], vertexJointsArray[2], vertexJointsArray[3])
                    );
                    vertexBufferWriter.WriteVector4(i, ElementName.BlendWeight, weights[i]);
                    vertexBufferWriter.WriteVector3(i, ElementName.Normal, normals[i]);
                    vertexBufferWriter.WriteVector2(i, ElementName.DiffuseUV, diffuseUvs[i]);

                    if (hasColors || hasTangents)
                        vertexBufferWriter.WriteColorBgraU8(i, ElementName.PrimaryColor, colors[i]);
                    if (hasTangents)
                        vertexBufferWriter.WriteVector4(i, ElementName.Tangent, tangents[i]);
                }
            }

            VertexBuffer vertexBuffer = VertexBuffer.Create(
                vertexBufferDescription.Usage,
                vertexBufferDescription.Elements,
                vertexBufferOwner
            );

            SkinnedMesh skinnedMesh = new(ranges, vertexBuffer, indexBufferOwner);
            Skeleton skeleton = CreateLeagueSkeleton(root.LogicalSkins[0]);

            return (skinnedMesh, skeleton);
        }

        private static VertexBufferDescription CreateRiggedMeshVertexBufferDescription(this ModelRoot root)
        {
            Guard.HasSizeEqualTo(root.LogicalMeshes, 1, nameof(root.LogicalMeshes));
            Guard.HasSizeEqualTo(root.LogicalSkins, 1, nameof(root.LogicalSkins));

            MeshPrimitive firstPrimitive = root.LogicalMeshes[0].Primitives[0];

            bool hasPositions = firstPrimitive.VertexAccessors.TryGetValue("POSITION", out _);
            bool hasJoints = firstPrimitive.VertexAccessors.TryGetValue("JOINTS_0", out _);
            bool hasWeights = firstPrimitive.VertexAccessors.TryGetValue("WEIGHTS_0", out _);
            bool hasNormals = firstPrimitive.VertexAccessors.TryGetValue("NORMAL", out _);
            bool hasDiffuseUvs = firstPrimitive.VertexAccessors.TryGetValue("TEXCOORD_0", out _);
            bool hasColors = firstPrimitive.VertexAccessors.TryGetValue("COLOR_0", out _);
            bool hasTangents = firstPrimitive.VertexAccessors.TryGetValue("TANGENT_0", out _);

            if (hasPositions is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh does not have positions");
            if (hasJoints is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh does not have joints");
            if (hasWeights is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh does not have weights");
            if (hasNormals is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh does not have normals");
            if (hasDiffuseUvs is false)
                ThrowHelper.ThrowInvalidOperationException($"Mesh does not have diffuse Uvs");

            return (hasColors, hasTangents) switch
            {
                (false, false) => SkinnedMeshVertex.BASIC,
                (true, false) => SkinnedMeshVertex.COLOR,
                (true, true) => SkinnedMeshVertex.TANGENT,
                (false, true) => throw new InvalidOperationException("Mesh must have colors if it has tangents")
            };
        }

        private static Skeleton CreateLeagueSkeleton(Skin skin)
        {
            Guard.IsNotNull(skin, nameof(skin));

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

                    joints.Add(
                        new SkeletonJoint(
                            (short)i,
                            -1,
                            jointNode.Name,
                            jointNode.LocalTransform.Translation,
                            scale,
                            jointNode.LocalTransform.Rotation
                        )
                    );
                }
                else
                {
                    short parentId = (short)nodes.IndexOf(jointNode.VisualParent);

                    joints.Add(
                        new SkeletonJoint(
                            (short)i,
                            parentId,
                            jointNode.Name,
                            jointNode.LocalTransform.Translation,
                            jointNode.LocalTransform.Scale,
                            jointNode.LocalTransform.Rotation
                        )
                    );
                }
            }

            List<short> influences = new(joints.Count);
            for (short i = 0; i < joints.Count; i++)
            {
                influences.Add(i);
            }

            return new(joints, influences);
        }
    }
}
