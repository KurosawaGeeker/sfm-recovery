using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class D2FogsPE : EffectBase
	{
		public Color Color;

		public float Size;

		public float HorizontalSpeed;

		public float VerticalSpeed;

		[Range(0f, 5f)]
		public float Density;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
