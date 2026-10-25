using UnityEngine;
using UnityEngine.UI;

namespace Ricimi
{
	[AddComponentMenu("UI/Effects/Gradient")]
	public class Gradient : BaseMeshEffect
	{
		public struct Matrix2x3
		{
			public float m00;

			public float m01;

			public float m02;

			public float m10;

			public float m11;

			public float m12;

			public Matrix2x3(float m00, float m01, float m02, float m10, float m11, float m12)
			{
				this.m00 = 0f;
				this.m01 = 0f;
				this.m02 = 0f;
				this.m10 = 0f;
				this.m11 = 0f;
				this.m12 = 0f;
			}

			public static Vector2 operator *(Matrix2x3 m, Vector2 v)
			{
				return default;
			}
		}

		public Color Color1;

		public Color Color2;

		[Range(-180f, 180f)]
		public float Angle;

		public override void ModifyMesh(VertexHelper vh)
		{
		}

		private Matrix2x3 LocalPositionMatrix(Rect rect, Vector2 dir)
		{
			return default;
		}

		private Vector2 RotationDir(float angle)
		{
			return default;
		}
	}
}
