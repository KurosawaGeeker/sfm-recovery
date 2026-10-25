using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class CrosshatchsPE : EffectBase
	{
		public Color Color1;

		public Color Color2;

		[Range(0f, 20f)]
		public int Size;

		[Range(0f, 20f)]
		public int Detail;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
