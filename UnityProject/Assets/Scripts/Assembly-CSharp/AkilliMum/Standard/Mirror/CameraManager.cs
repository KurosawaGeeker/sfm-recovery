using UnityEngine;

namespace AkilliMum.Standard.Mirror
{
	public class CameraManager : ManagerBase
	{
		public struct CameraSettings
		{
			public bool IsDebug { get; set; }

			public WorkingType WorkingType { get; set; }

			public ClearBackGround ClearBackGround { get; set; }

			public Color ClearColor { get; set; }

			public Platform Platform { get; set; }

			public bool ForceMultiPass { get; set; }

			public bool HDR { get; set; }

			public float FOV { get; set; }

			public bool UseClipping { get; set; }

			public int ClipUV { get; set; }

			public int ClipEye { get; set; }

			public int ClippingPercentage { get; set; }

			public bool TurnOffOcclusion { get; set; }

			public MSAALevel MSAALevel { get; set; }

			public bool RenderPostProcessing { get; set; }

			public bool RequiresDepthTexture { get; set; }

			public bool RequiresOpaqueTexture { get; set; }

			public bool Cull { get; set; }

			public float CullDistance { get; set; }

			public LayerMask ReflectLayers { get; set; }

			public float ClipPlaneOffset { get; set; }

			public bool EnableDepth { get; set; }

			public bool EnableDepthBlur { get; set; }

			public Shader DepthBlurShader { get; set; }

			public Material DepthBlurMaterial { get; set; }

			public float DepthBlurCutoff { get; set; }

			public int DepthBlurIterations { get; set; }

			public float DepthBlurSurfacePower { get; set; }

			public int DepthBlurHorizontalMultiplier { get; set; }

			public int DepthBlurVerticalMultiplier { get; set; }

			public GameObject FakeCamera { get; set; }
		}

		public const string CameraNameStart = "Mirror camera for ";

		private Camera _mainCamera;

		private GameObject _mirrorCameraContainer;

		private Camera _mirrorCamera;

		private CameraSettings _settings;

		private Rect _viewPort;

		private Quaternion _viewRotation;

		private StereoTargetEyeMask _viewEyeMask;

		private Vector3 _viewTransformPosition;

		private Quaternion _viewTransformRotation;

		private Vector3 _viewTransformForward;

		private float _viewFOV;

		public CameraManager(CameraSettings settings)
        {
            _settings = settings;
        }

		public CameraSettings GetSettings()
		{
			return default;
		}

		public void SetReferenceCamera(Camera camera)
		{
		}

		public Camera GetMirrorCamera()
		{
			return null;
		}

		public Camera GetReferenceCamera()
		{
			return null;
		}

		public void CreateMirrorCamera()
		{
		}

		private void UpdateCameraModes()
		{
		}

		public void SetViewport(RendererManager rendererManager, StereoTargetEyeMask stereoTargetEyeMask)
		{
		}

		public void ResetViewport(StereoTargetEyeMask stereoTargetEyeMask)
		{
		}

		public void SetScissorRect(Rect r, StereoTargetEyeMask stereoTargetEyeMask)
		{
		}

		public void SetFake()
		{
		}

		public void ResetFake()
		{
		}

		public void SetFOV()
		{
		}

		public void ResetFOV()
		{
		}

		private void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
		{
		}

		private Vector4 CameraSpacePlane(Matrix4x4 worldToCameraMatrix, Vector3 pos, Vector3 normal, float sideSign)
		{
			return default;
		}

		private void MakeProjectionMatrixOblique(ref Matrix4x4 matrix, Vector4 clipPlane)
		{
		}

		private float sgn(float a)
		{
			return 0f;
		}

		public void Draw(RendererManager rendererManager, RenderTextureManager renderTextureManager, StereoTargetEyeMask stereoTargetEyeMask, bool invert, bool renderCam)
		{
		}

		public override void Destroy()
		{
		}
	}
}
