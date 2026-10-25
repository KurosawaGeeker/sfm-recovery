using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class PosterizesPE : EffectBase
	{
		public int Level;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
