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
    using VERTEX_SKINNED_COLOR = VertexBuilder<VertexPositionNormal, VertexColor1Texture1, VertexJoints4>;
    using VERTEX_SKINNED_TANGENT = VertexBuilder<VertexPositionNormalTangent, VertexColor1Texture1, VertexJoints4>;

    public static class SimpleSkinGltfExtensions
    {
        public static ModelRoot ToGltf(
            this SkinnedMesh skinnedMesh,
            Skeleton skeleton,
            Dictionary<string, SixLabors.ImageSharp.Image> materialTextues = null,
            List<(string, LeagueAnimation)> leagueAnimations = null
        )
        {
            SceneBuilder sceneBuilder = new();
            NodeBuilder rootNodeBuilder = new("model");

            var meshBuilder = CreateVertexSkinnedMesh(skinnedMesh, skeleton, materialTextues);
            var bones = CreateSkeleton(rootNodeBuilder, skeleton);

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

        private static MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4> CreateVertexSkinnedMesh(
            SkinnedMesh skinnedMesh,
            Skeleton skeleton,
            Dictionary<string, SixLabors.ImageSharp.Image> materialTextues
        )
        {
            MeshBuilder<VertexPositionNormal, VertexTexture1, VertexJoints4> meshBuilder =
                VERTEX_SKINNED.CreateCompatibleMesh();

            foreach (SkinnedMeshRange range in skinnedMesh.Ranges)
            {
                MaterialBuilder material = new MaterialBuilder(range.Material).WithUnlitShader();
                var primitiveBuilder = meshBuilder.UsePrimitive(material);

                // Assign submesh Image
                if (materialTextues is not null && materialTextues.ContainsKey(range.Material))
                {
                    var submeshImage = materialTextues[range.Material];
                    AssignMaterialTexture(material, submeshImage);
                }

                bool hasPrimaryColor = skinnedMesh.VerticesView.TryGetAccessor(
                    ElementName.PrimaryColor,
                    out var primaryColorAccessor
                );
                bool hasTangents = skinnedMesh.VerticesView.TryGetAccessor(
                    ElementName.Tangent,
                    out var tangentAccessor
                );

                VERTEX_SKINNED[] vertices = new VERTEX_SKINNED[range.VertexCount];
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
                    VERTEX_SKINNED vertex = new();
                    vertex = hasTangents switch
                    {
                        true => vertex.WithGeometry(positions[i], normals[i], tangents[i]),
                        false => vertex.WithGeometry(positions[i], normals[i]),
                    };
                    vertex = hasPrimaryColor switch
                    {
                        true
                            => vertex.WithMaterial(
                                new Vector4(
                                    primaryColors[i].r / 255,
                                    primaryColors[i].g / 255,
                                    primaryColors[i].b / 255,
                                    primaryColors[i].a / 255
                                ),
                                diffuseUvs[i]
                            ),
                        false => vertex.WithMaterial(diffuseUvs[i])
                    };

                    SetVertexSkinning(i, skeleton, vertex, boneWeights, boneIndices);

                    vertices[i] = vertex;
                }

                // Add vertices to primitive
                ReadOnlySpan<ushort> indices = skinnedMesh.IndicesView.Span.Slice(range.StartIndex, range.IndexCount);
                ushort minIndex = indices.Min();
                for (int i = 0; i < indices.Length; i += 3)
                {
                    IVertexBuilder v1 = vertices[indices[i + 0] - minIndex];
                    IVertexBuilder v2 = vertices[indices[i + 1] - minIndex];
                    IVertexBuilder v3 = vertices[indices[i + 2] - minIndex];

                    primitiveBuilder.AddTriangle(v1, v2, v3);
                }
            }

            return meshBuilder;
        }

        private static void SetVertexSkinning(
            int index,
            Skeleton skeleton,
            IVertexBuilder vertex,
            VertexElementArray<Vector4> boneWeights,
            VertexElementArray<(byte x, byte y, byte z, byte w)> boneIndices
        )
        {
            var joints = boneIndices[index];
            Vector4 jointWeights = boneWeights[index];
            VertexJoints4 vertexSkinning =
                new(
                    new (int, float)[]
                    {
                        (skeleton.Influences[joints.x], jointWeights.X),
                        (skeleton.Influences[joints.y], jointWeights.Y),
                        (skeleton.Influences[joints.z], jointWeights.Z),
                        (skeleton.Influences[joints.w], jointWeights.W)
                    }
                );

            vertex.SetSkinning(vertexSkinning);
        }

        private static void AssignMaterialTexture(MaterialBuilder materialBuilder, SixLabors.ImageSharp.Image texture)
        {
            MemoryStream textureStream = new MemoryStream();
            texture.Save(textureStream, new SixLabors.ImageSharp.Formats.Png.PngEncoder());

            materialBuilder
                .UseChannel(KnownChannel.BaseColor)
                .UseTexture()
                .WithPrimaryImage(new SharpGLTF.Memory.MemoryImage(textureStream.GetBuffer()));
        }

        private static List<(NodeBuilder Node, Matrix4x4 InverseBindMatrix)> CreateSkeleton(
            NodeBuilder rootNode,
            Skeleton skeleton
        )
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
                        if (track.Translations.Count == 0)
                            track.Translations.Add(0.0f, new Vector3(0, 0, 0));
                        if (track.Translations.Count == 1)
                            track.Translations.Add(1.0f, new Vector3(0, 0, 0));
                        CurveBuilder<Vector3> translationBuilder = joint
                            .UseTranslation()
                            .UseTrackBuilder(animationName);
                        foreach (var translation in track.Translations)
                        {
                            translationBuilder.SetPoint(translation.Key, translation.Value);
                        }

                        if (track.Rotations.Count == 0)
                            track.Rotations.Add(0.0f, Quaternion.Identity);
                        if (track.Rotations.Count == 1)
                            track.Rotations.Add(1.0f, Quaternion.Identity);
                        CurveBuilder<Quaternion> rotationBuilder = joint.UseRotation().UseTrackBuilder(animationName);
                        foreach (var rotation in track.Rotations)
                        {
                            rotationBuilder.SetPoint(rotation.Key, rotation.Value);
                        }

                        if (track.Scales.Count == 0)
                            track.Scales.Add(0.0f, new Vector3(1, 1, 1));
                        if (track.Scales.Count == 1)
                            track.Scales.Add(1.0f, new Vector3(1, 1, 1));
                        CurveBuilder<Vector3> scaleBuilder = joint.UseScale().UseTrackBuilder(animationName);
                        foreach (var scale in track.Scales.ToList())
                        {
                            scaleBuilder.SetPoint(scale.Key, scale.Value);
                        }
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

            return new Skeleton(joints, influences);
        }
    }
}
