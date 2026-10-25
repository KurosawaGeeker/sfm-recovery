using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class PulsesPE : EffectBase
	{
		public float Speed;

		public float Iteration;

		public float Min;

		public float Max;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
