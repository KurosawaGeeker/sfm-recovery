using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class VintagesPE : EffectBase
	{
		public float Luminance;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
