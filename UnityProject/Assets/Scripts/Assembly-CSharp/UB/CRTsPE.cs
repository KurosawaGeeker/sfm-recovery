using UnityEngine;

namespace UB
{
	[ExecuteInEditMode]
	public class CRTsPE : EffectBase
	{
		public float NoiseX;

		public Vector3 Offset;

		public float RGBNoise;

		public float ScanLineTail;

		public float ScanLineSpeed;

		public Shader Shader;

		private Material _material;

		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
		}
	}
}
