using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class SimpleDisplacementsPE : EffectBase
	{
		public float Amount;

		public float Speed;

		public Texture2D Noise;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
