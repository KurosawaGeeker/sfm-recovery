using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class DotsPE : EffectBase
	{
		public float BlockSize;

		public float DotRadius;

		public Color EmptyColor;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
