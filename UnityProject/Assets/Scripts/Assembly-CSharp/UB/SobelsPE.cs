using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class SobelsPE : EffectBase
	{
		public Color Color;

		public float Detail;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
