using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class SimplesNoisePE : EffectBase
	{
		public int Noise;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
