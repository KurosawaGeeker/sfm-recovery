using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class TV70sPE : EffectBase
	{
		public float Corner;

		public float Scan;

		public float ShiftAmount;

		public Texture2D Noise;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
