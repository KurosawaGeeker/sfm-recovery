using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class BlursFastPE : EffectBase
	{
		[Range(1f, 100f)]
		public int Iterations;

		public float LoopNStepFor360Degree;

		public float JumpNPixelsFromOrigin;

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
