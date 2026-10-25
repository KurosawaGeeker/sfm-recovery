using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class EmbossPE : EffectBase
	{
		public float Strength;

		public float Angle;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
