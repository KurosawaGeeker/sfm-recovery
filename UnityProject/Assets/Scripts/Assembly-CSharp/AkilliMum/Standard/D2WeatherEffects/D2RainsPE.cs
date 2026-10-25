using UnityEngine;

namespace AkilliMum.Standard.D2WeatherEffects
{
	[ExecuteInEditMode]
	public class D2RainsPE : EffectBase
	{
		public Transform CamTransform;

		private Vector3 _firstPosition;

		private Vector3 _difference;

		public float CameraSpeedMultiplier;

		public Color Color;

		[Range(1f, 50f)]
		public float ParticleMultiplier;

		public float Tail;

		public float Speed;

		public float Direction;

		public float Luminance;

		[Range(0.01f, 10f)]
		public float Zoom;

		public bool DarkMode;

		[Range(0f, 0.1f)]
		public float LuminanceAdder;

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
