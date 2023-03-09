using LeagueToolkit.Utils.Extensions;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.Core.Primitives
{
    /*
     * Process:
     * for PlanarReflectorCount:
     * - LOAD: Transform
     * - LOAD: Plane
     * - LOAD: Normal
     *
     * foreach |PlanarReflector|
     *   // UnkMapParamVector comes from outside the map parsing function (server/bin file)
     *   // and also does operations on all of the mesh transforms and bucket grid (likely mapscale/mapoffset)
     * - Matrix44::MultiplyByVec(PlanarReflector.Transform, UnkMapParamVector)
     *
     *   // Loop through all PlanarReflector.Plane vertices
     * - for (i = 0; i < 8; i++)
     *   - selectedVertex = Box::SelectVertex(PlanarReflector.Plane, i)
     *   - selectedVertexTransformed = Vector3f::MultiplyByMatrix(selectedVertex, PlanarReflector.Transform)
     *   - Box::Grow(PlanarReflector.ReflectionPlane, selectedVertexTransformed)
     *
     * Apply PlanarReflector:
     * - call planarCameraData->ReleaseRenderResources_MAYBE()
     * - check if EnvironmentQuality >= 3
     * - foreach |PlanarReflector|
     *   - continue if Frustum::IsBoxInside(SceneCameraFrustum, PlanarReflector.ReflectionPlane)
     *
     *     // This creates a new Frustum which is a copy of the SceneCamera Frustum
     *     // but with the Camera Matrix multiplied by the PlanarReflector Matrix
     *     // SceneCamera = Global::r3dRendererData->CameraData.CurrentRenderTargetData.CameraPtr
     *   - transformedSceneCameraFrustum = Frustum::Build(SceneCamera, PlanarReflector.Transform)
     *   - continue if Frustum::IsBoxInside(transformedSceneCameraFrustum, PlanarReflector.Plane)
     *     - normalizedNormal = Vector3f::Normalize(PlanarReflector.Normal)
     *     - inverseTheta = -Vector3f::Dot(normalizedNormal, PlanarReflector.Transform.Translation)
     *
     *       // Copies camera settings from Global::r3dRendererData->CameraData.CurrentRenderTargetData.CameraPtr
     *       // into the renderer structure that called this function (thiscall in vtable)
     *     - planarCameraData.Camera = Camera::Init(SceneCamera)
     *     - Camera::SetType(planarCameraData.Camera, CameraType.kTPS = 2)
     *
     *     - cameraRotationOverride = Vector4f(normalizedNormal, W = inverseTheta)
     *     - Camera::SetRotationOverride(planarCameraData.Camera, cameraRotationOverride)
     *
     *     - newUnkScreenBufferWidth = Global::r3dRendererData->CameraData.CurrentRenderTargetData.Data.Rect.x2
     *                               - Global::r3dRendererData->CameraData.CurrentRenderTargetData.Data.Rect.x1
     *     - newUnkScreenBufferHeight = Global::r3dRendererData->CameraData.CurrentRenderTargetData.Data.Rect.y2
     *                               - Global::r3dRendererData->CameraData.CurrentRenderTargetData.Data.Rect.y1
     *     - newUnkScreenBuffer = r3dScreenBufferManager::AcquireScreenBuffer(
     *         Global::r3dScreenBufferManager, somethingResourceRelated,
     *         newUnkScreenBufferWidth, newUnkScreenBufferHeight,
     *         newUnkScreenBufferDesc { 0x00000000, 0x0101, 0x00, 0x00000000}
     *     );
     *     - planarCameraData->Camera.RenderTargets = newUnkScreenBuffer
     *     - renderTargetData = Riot::MaybeRenderTargetData::Init(renderTargetDataBuffer, newUnkScreenBuffer.Width, newUnkScreenBuffer.Height)
     *     - Riot::CameraRenderTargetData::SetMaybeRenderTargetData(planarCameraData.Camera.CurrentRenderTargetData, renderTargetData)
     * - END FOREACH LOOP (RENDER TARGET SET)
     */

    /// <summary>
    /// Describes a <see href="http://www.bluevoid.com/opengl/sig00/advanced00/notes/node164.html">planar reflector</see> plane
    /// </summary>
    /// <remarks>
    /// Only 1 <see cref="PlanarReflector"/> can be active in a scene at the same time
    /// (This is determined by frustum culling)
    /// </remarks>
    public readonly struct PlanarReflector
    {
        /// <summary>Gets the plane's transform</summary>
        public Matrix4x4 Transform { get; }

        /// <summary>Gets the reflection plane</summary>
        public Box Plane { get; }

        /// <summary>Gets the reflection plane normal</summary>
        public Vector3 Normal { get; }

        /// <summary>Creates a new <see cref="PlanarReflector"/> object</summary>
        public PlanarReflector()
        {
            this.Transform = Matrix4x4.Identity;
            this.Plane = new();
            this.Normal = Vector3.Zero;
        }

        /// <summary>Creates a new <see cref="PlanarReflector"/> object with the specified parameters</summary>
        public PlanarReflector(Matrix4x4 transform, Box plane, Vector3 normal)
        {
            this.Transform = transform;
            this.Plane = plane;
            this.Normal = normal;
        }

        internal static PlanarReflector ReadFromMapGeometry(BinaryReader br)
        {
            Matrix4x4 transform = br.ReadMatrix4x4RowMajor();
            Box plane = br.ReadBox();
            Vector3 normal = br.ReadVector3();

            return new(transform, plane, normal);
        }

        internal void WriteToMapGeometry(BinaryWriter bw)
        {
            bw.WriteMatrix4x4RowMajor(this.Transform);
            bw.WriteBox(this.Plane);
            bw.WriteVector3(this.Normal);
        }
    }
}
