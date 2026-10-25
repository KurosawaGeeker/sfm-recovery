using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class GameboysPE : EffectBase
	{
		public float BlockSize;

		public Color Color1;

		public Color Color2;

		public Color Color3;

		public Color Color4;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
