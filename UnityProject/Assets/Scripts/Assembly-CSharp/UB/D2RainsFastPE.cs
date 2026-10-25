using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class D2RainsFastPE : EffectBase
	{
		public Color Color;

		public Texture2D Noise;

		public float Density;

		public float Speed;

		public float Exposure;

		public float Direction;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
