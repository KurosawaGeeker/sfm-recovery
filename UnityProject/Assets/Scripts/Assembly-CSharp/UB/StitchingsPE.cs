using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class StitchingsPE : EffectBase
	{
		public bool Invert;

		public float Size;

		public Color Color;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
