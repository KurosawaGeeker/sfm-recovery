using System.Collections;
using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class MirrorWithShader : EffectBase
	{
		public string UniqueName;

		public bool UpdateOnEditor;

		public WorkType WorkType;

		public bool UseCameraClipPlane;

		public FollowVector UpVector;

		public bool DisablePixelLights;

		public int TextureSize;

		public float ClipPlaneOffset;

		public Camera Camera;

		public LayerMask ReflectLayers;

		public GameObject ReflectionCameraPrefab;

		private GameObject _reflectionCameraPrefabInstance;

		[Space(20f)]
		[Header("\"Shader to run on texture\" settings")]
		public Shader Shader;

		public int Lod;

		[Range(1f, 20f)]
		public int Iterations;

		public float CustomFloatParam2;

		private Material _material;

		private Hashtable _reflectionCameras;

		private RenderTexture _reflectionTexture1;

		private RenderTexture _reflectionTexture2;

		private RenderTexture _reflectionTexture3;

		private int _oldReflectionTextureSize;

		public static Vector3 ZeroV3;

		public static Vector3 OneV3;

		public static Quaternion QuaternionFromMatrix(Matrix4x4 m)
		{
			return default;
		}

		public static Vector4 PosToV4(Vector3 v)
		{
			return default;
		}

		public static Vector3 ToV3(Vector4 v)
		{
			return default;
		}

		private void Start()
		{
		}

		public void OnWillRenderObject()
		{
		}

		private void RenderMe(Material[] materials)
		{
		}

		private void SetMaterial(Material mat, string properyName)
		{
		}

		private void OnDisable()
		{
		}

		private void UpdateCameraModes(Camera src, Camera dest)
		{
		}

		private void CreateMirrorObjects(Camera currentCamera, out Camera reflectionCamera)
		{
			reflectionCamera = null;
		}

		private Vector4 CameraSpacePlane(Camera cam, Vector3 pos, Vector3 normal, float sideSign)
		{
			return default;
		}

		private void CalculateReflectionMatrix(ref Matrix4x4 reflectionMat, Vector4 plane)
		{
		}
	}
}
