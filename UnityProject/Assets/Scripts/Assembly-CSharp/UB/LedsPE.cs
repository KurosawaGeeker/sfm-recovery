using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class LedsPE : EffectBase
	{
		public float LedSize;

		public float Luminance;

		public Color ScanColor;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
