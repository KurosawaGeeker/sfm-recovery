using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class NightVisionsPE : EffectBase
	{
		public float Noise;

		public float Power;

		public float Flicker;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
