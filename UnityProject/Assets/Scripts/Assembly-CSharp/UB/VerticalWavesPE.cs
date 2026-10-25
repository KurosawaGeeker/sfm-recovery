using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class VerticalWavesPE : EffectBase
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
