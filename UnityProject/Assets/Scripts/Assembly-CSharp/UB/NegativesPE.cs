using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class NegativesPE : EffectBase
	{
		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
