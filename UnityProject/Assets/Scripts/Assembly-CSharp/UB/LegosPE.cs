using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class LegosPE : EffectBase
	{
		public float BlockSize;

		public bool Shadow;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
