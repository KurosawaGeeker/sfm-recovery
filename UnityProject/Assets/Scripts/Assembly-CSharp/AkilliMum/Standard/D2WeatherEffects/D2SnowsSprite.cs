using UnityEngine;

namespace AkilliMum.Standard.D2WeatherEffects
{
	[ExecuteInEditMode]
	public class D2SnowsSprite : EffectBase
	{
		public Transform CamTransform;

		private Vector3 _firstPosition;

		private Vector3 _difference;

		public float CameraSpeedMultiplier;

		public Color Color;

		public bool TopFade;

		public bool RightFade;

		public bool BottomFade;

		public bool LeftFade;

		[Range(0f, 1f)]
		public float FadeMultiplier;

		[Range(1f, 50f)]
		public float ParticleMultiplier;

		public float Speed;

		public float Direction;

		public float Luminance;

		[Range(0.01f, 10f)]
		public float Zoom;

		public Material[] EffectedMaterials;

		private void Awake()
		{
		}

		private void Update()
		{
		}
	}
}
