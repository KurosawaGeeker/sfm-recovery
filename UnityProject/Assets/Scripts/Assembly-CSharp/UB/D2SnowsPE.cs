using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class D2SnowsPE : EffectBase
	{
		public Color Color;

		[Range(1f, 50f)]
		public float ParticleMultiplier;

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

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
