using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class CharcoalsPE : EffectBase
	{
		public float Strength;

		public Color LineColor;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
