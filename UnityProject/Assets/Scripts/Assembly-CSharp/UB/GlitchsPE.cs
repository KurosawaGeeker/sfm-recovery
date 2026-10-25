using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class GlitchsPE : EffectBase
	{
		public float Noise;

		public float Speed;

		public float Size;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
