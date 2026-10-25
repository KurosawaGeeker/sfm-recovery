using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class RadialVolumesPE : EffectBase
	{
		public float Size;

		public float Step;

		[Range(0f, 0.5f)]
		public float Smooth;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
