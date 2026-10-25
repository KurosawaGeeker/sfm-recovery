using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class AlcoholsPE : EffectBase
	{
		public bool SeperateColorChannels;

		public float Size;

		public float MainMultiplier;

		public float RedMultiplier;

		public float GreenMultiplier;

		public float BlueMultiplier;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
