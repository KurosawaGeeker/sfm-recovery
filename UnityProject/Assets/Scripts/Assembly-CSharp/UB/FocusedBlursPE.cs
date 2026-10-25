using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class FocusedBlursPE : EffectBase
	{
		public Vector3 Focus;

		public float Step;

		[Range(0f, 0.5f)]
		public float Radius;

		public float NeighbourPixels;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
