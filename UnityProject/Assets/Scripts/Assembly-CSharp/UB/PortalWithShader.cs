using System.Collections;
using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class PortalWithShader : EffectBase
	{
		public Camera MainCamera;

		public Camera PortalCamera;

		public Transform Source;

		public Transform Destination;

		[Range(1f, 100f)]
		public int Iterations;

		public float CustomFloatParam2;

		public Shader Shader;

		private Material _material;

		public bool DisablePixelLights;

		public int TextureSize;

		public LayerMask ReflectLayers;

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

		public void OnWillRenderObject()
		{
		}

		private void RenderMe(Material[] materials)
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
	}
}
