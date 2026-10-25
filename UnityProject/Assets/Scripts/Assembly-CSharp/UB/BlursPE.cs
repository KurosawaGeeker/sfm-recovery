using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class BlursPE : EffectBase
	{
		[Range(1f, 100f)]
		public int Iterations;

		public float NeighbourPixels;

		public Shader Shader;

		private Material _material;

		private RenderTexture _reflectionTexture1;

		private RenderTexture _reflectionTexture2;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}

		private void CreateRenderTextures(RenderTexture source)
		{
		}
	}
}
