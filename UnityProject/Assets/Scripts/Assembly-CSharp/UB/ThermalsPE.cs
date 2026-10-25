using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class ThermalsPE : EffectBase
	{
		public float Threshold;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
