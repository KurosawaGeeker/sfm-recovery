using UnityEngine;

namespace SplineMesh
{
	public struct CurveSample
	{
		public readonly Vector3 location;

		public readonly Vector3 tangent;

		public readonly Vector3 up;

		public readonly Vector2 scale;

		public readonly float roll;

		public readonly float distanceInCurve;

		public readonly float timeInCurve;

		public readonly CubicBezierCurve curve;

		private Quaternion rotation;

		public Quaternion Rotation => default;

		public CurveSample(Vector3 location, Vector3 tangent, Vector3 up, Vector2 scale, float roll, float distanceInCurve, float timeInCurve, CubicBezierCurve curve)
		{
			this.location = default;
			this.tangent = default;
			this.up = default;
			this.scale = default;
			this.roll = 0f;
			this.distanceInCurve = 0f;
			this.timeInCurve = 0f;
			this.curve = null;
			rotation = default;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return 0;
		}

		public static bool operator ==(CurveSample cs1, CurveSample cs2)
		{
			return false;
		}

		public static bool operator !=(CurveSample cs1, CurveSample cs2)
		{
			return false;
		}

		public static CurveSample Lerp(CurveSample a, CurveSample b, float t)
		{
			return default;
		}

		public MeshVertex GetBent(MeshVertex vert)
		{
			return null;
		}
	}
}
