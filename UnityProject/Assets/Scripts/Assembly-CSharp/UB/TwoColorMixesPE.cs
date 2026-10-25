using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class TwoColorMixesPE : EffectBase
	{
		public float Luminance;

		public Color Color1;

		public Color Color2;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
