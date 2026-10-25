using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class EdgesBlendPE : EffectBase
	{
		[Range(1f, 10f)]
		public int Strength;

		[Range(1f, 100f)]
		public int Smooth;

		public Color Color;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
