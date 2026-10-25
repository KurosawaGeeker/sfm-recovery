using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class RefractionsPE : EffectBase
	{
		public float ReflDistort;

		public Texture2D Overlay;

		public Texture2D BumpMap;

		public float TileX;

		public float TileY;

		[Range(0f, 1f)]
		public float Luminance;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
