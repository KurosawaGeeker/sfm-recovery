using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class UnderWatersPE : EffectBase
	{
		public float VerticalStrength;

		public float HorizontalStrength;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
