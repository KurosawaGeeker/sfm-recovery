using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class BleachsBypassPE : EffectBase
	{
		public float Scale;

		public Color Color;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
