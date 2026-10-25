using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class ScanningsPE : EffectBase
	{
		public Color Luminance;

		public Color Color;

		public float Speed;

		public float Threshold;

		public float ScanWidth;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
