using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class MosaicsPE : EffectBase
	{
		public float BlockSize;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
