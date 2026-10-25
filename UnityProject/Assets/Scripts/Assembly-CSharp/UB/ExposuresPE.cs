using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class ExposuresPE : EffectBase
	{
		public float Power;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
