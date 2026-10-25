using UnityEngine;

namespace AkilliMum.Standard.D2WeatherEffects
{
	[ExecuteInEditMode]
	public class D2FogsPE : EffectBase
	{
		public Transform CamTransform;

		private Vector3 _firstPosition;

		private Vector3 _difference;

		public float CameraSpeedMultiplier;

		public Color Color;

		public float Size;

		public float HorizontalSpeed;

		public float VerticalSpeed;

		[Range(0f, 5f)]
		public float Density;

		public Shader Shader;

		private Material _material;

		private void Awake()
		{
		}

		private void Update()
		{
		}

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
