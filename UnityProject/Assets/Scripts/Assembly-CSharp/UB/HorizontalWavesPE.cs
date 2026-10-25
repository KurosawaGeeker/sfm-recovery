using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class HorizontalWavesPE : EffectBase
	{
		public float Speed;

		public float SizeX;

		public float SizeY;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
