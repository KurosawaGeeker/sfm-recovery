using UnityEngine;

namespace AkilliMum.Standard.D2WeatherEffects
{
	[ExecuteInEditMode]
	public class D2FogsSprite : EffectBase
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

		public float Size;

		public float HorizontalSpeed;

		public float VerticalSpeed;

		[Range(0f, 20f)]
		public float Density;

		public Material[] EffectedMaterials;

		private void Awake()
		{
		}

		private void Update()
		{
		}
	}
}
