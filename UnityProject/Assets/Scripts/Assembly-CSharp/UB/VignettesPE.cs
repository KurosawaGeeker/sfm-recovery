using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class VignettesPE : EffectBase
	{
		public float Size;

		public float Blackness;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
