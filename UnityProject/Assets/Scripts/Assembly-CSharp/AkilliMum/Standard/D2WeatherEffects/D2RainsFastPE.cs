using UnityEngine;

namespace AkilliMum.Standard.D2WeatherEffects
{
	[ExecuteInEditMode]
	public class D2RainsFastPE : EffectBase
	{
		public Transform CamTransform;

		private Vector3 _firstPosition;

		private Vector3 _difference;

		public float CameraSpeedMultiplier;

		public Color Color;

		public Texture2D Noise;

		public float Density;

		public float Speed;

		public float Exposure;

		public float Direction;

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
