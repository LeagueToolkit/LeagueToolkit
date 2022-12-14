using LeagueToolkit.Helpers.Extensions;
using LeagueToolkit.Helpers.Structures;
using System.IO;
using System.Numerics;

namespace LeagueToolkit.IO.MapGeometry
{
    /*
     * Process:
     * for CameraTransformCount:
     * - LOAD: TransformationMatrix
     * - LOAD: AABB
     * - LOAD: UnkVec
     * 
     * foreach |CameraTransform|
     *   // UnkMapParamVector comes from outside the map parsing function (server/bin file)
     *   // and also does operations on all of the mesh transforms and bucket grid (likely mapscale/mapoffset)
     * - Matrix44::MultiplyByVec(CameraTransform.TransformationMatrix, UnkMapParamVector)
     * 
     *   // Loop through all CameraTransform.AABB vertices
     * - for (i = 0; i < 8; i++)
     *   - selectedAABBVertex = Box::SelectVertex(CameraTransform.AABB, i)
     *   - selectedAABBVertexTransformed = Vector3f::MultiplyByMatrix(CameraTransform.UnkVec, CameraTransform.TransformationMatrix)
     *   - Box::Grow(CameraTransform.ExpandedAABB, selectedAABBVertexTransformed)
     * 
     * Apply CameraTransform:
     * - call unknown CallerRenderer method
     * - check if EnvironmentQuality >= 3
     * - foreach |CameraTransform|
     *   - continue if Frustum::IsBoxInside(MainCameraFrustum, CameraTransform.ExpandedAABB)
     * 
     *     // This creates a new Frustum which is a copy of the MainCamera Frustum
     *     // but with the Camera Matrix multiplied by the CameraTransform Matrix
     *     // MainCamera = Global::r3dRendererData->CameraData.CurrentRenderTargetData.CameraPtr
     *   - transformedMainCameraFrustum = Frustum::Build(MainCamera, CameraTransform.TransformationMatrix)
     *   - continue if Frustum::IsBoxInside(transformedMainCameraFrustum, CameraTransform.AABB)
     *     - unkVecNormalized = Vector3f::Normalize(CameraTransform.UnkVec)
     *     - unkNegativeDotProduct = -Vector3f::Dot(unkVecNormalized, CameraTransform.TransformationMatrix.Translation)
     *     
     *       // Copies camera settings from Global::r3dRendererData->CameraData.CurrentRenderTargetData.CameraPtr
     *       // into the renderer structure that called this function (thiscall in vtable)
     *     - UnkPlanarCameraData.Camera = Camera::Init(UnkCameraPtr)
     *     - Camera::SetType(UnkPlanarCameraData.Camera, CameraType.kTPS = 2)
     * 
     *     - cameraRotationOverride = Vector4f(unkVecNormalized, W = unkNegativeDotProduct)
     *     - Camera::SetRotationOverride(UnkPlanarCameraData.Camera, cameraRotationOverride)
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
     *     - UnkPlanarCameraData->Camera.RenderTargets = newUnkScreenBuffer
     *     - renderTargetData = Riot::MaybeRenderTargetData::Init(renderTargetDataBuffer, newUnkScreenBuffer.Width, newUnkScreenBuffer.Height)
     *     - Riot::CameraRenderTargetData::SetMaybeRenderTargetData(UnkPlanarCameraData.Camera.CurrentRenderTargetData, renderTargetData)
     * - END FOREACH LOOP (RENDER TARGET SET)
     */
    public class MapGeometryCameraTransformer
    {
        /// <summary>
        /// This matrix is used for creating an Expanded Bounding Box by multiplying all of the vertices of <see cref="BoundingBox"/> by <see cref="Transform"/>
        /// <br>It is also used to transform the Camera's World Matrix through multiplication</br>
        /// </summary>
        public Matrix4x4 Transform { get; set; }
        public Box BoundingBox { get; set; }
        /// <summary>
        /// Rotation Vector which gets normalized and transformed into a quaternion
        /// <code>
        /// quaternionXYZ = Vector3f::Normalize(<see cref="RotationVector"/>)
        /// quaternionW = -Vector3f::Dot(quaternionXYZ, <see cref="Transform"/>.Translation)
        /// </code>
        /// </summary>
        public Vector3 RotationVector { get; set; }

        public MapGeometryCameraTransformer() { }
        public MapGeometryCameraTransformer(BinaryReader br)
        {
            this.Transform = br.ReadMatrix4x4RowMajor();
            this.BoundingBox = new(br);
            this.RotationVector = br.ReadVector3();
        }

        public void Write(BinaryWriter bw)
        {
            bw.WriteMatrix4x4RowMajor(this.Transform);
            this.BoundingBox.Write(bw);
            bw.WriteVector3(this.RotationVector);
        }
    }
}
