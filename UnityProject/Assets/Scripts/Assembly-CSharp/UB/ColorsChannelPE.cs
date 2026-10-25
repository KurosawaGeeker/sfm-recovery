using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class ColorsChannelPE : EffectBase
	{
		[Range(0f, 1f)]
		public float Red;

		[Range(0f, 1f)]
		public float Green;

		[Range(0f, 1f)]
		public float Blue;

		public float Luminance;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
