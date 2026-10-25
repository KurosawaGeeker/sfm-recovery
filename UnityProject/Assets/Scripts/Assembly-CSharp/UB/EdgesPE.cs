using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class EdgesPE : EffectBase
	{
		public float Threshold;

		public Color Color;

		public Color BackgroundColor;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
