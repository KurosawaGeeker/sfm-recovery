using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class HeatsPE : EffectBase
	{
		public float Threshold;

		public float Noise;

		public float Light;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
