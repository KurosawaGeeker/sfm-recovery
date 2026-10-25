using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class BlackHolesPE : EffectBase
	{
		public float HoleSize;

		public Vector2 Position;

		public float Size;

		public float Radius;

		public float Speed;

		public float Luminance;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
