using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SplineMesh
{
	[Serializable]
	public class CubicBezierCurve
	{
		private const int STEP_COUNT = 30;

		private const float T_STEP = 1f / 30f;

		private readonly List<CurveSample> samples;

		public SplineNode n1;

		public SplineNode n2;

		public UnityEvent Changed;

		public float Length { get; private set; }

		public CubicBezierCurve(SplineNode n1, SplineNode n2)
		{
		}

		public void ConnectStart(SplineNode n1)
		{
		}

		public void ConnectEnd(SplineNode n2)
		{
		}

		public Vector3 GetInverseDirection()
		{
			return default;
		}

		private Vector3 GetLocation(float t)
		{
			return default;
		}

		private Vector3 GetTangent(float t)
		{
			return default;
		}

		private Vector3 GetUp(float t)
		{
			return default;
		}

		private Vector2 GetScale(float t)
		{
			return default;
		}

		private float GetRoll(float t)
		{
			return 0f;
		}

		private void ComputeSamples(object sender, EventArgs e)
		{
		}

		private CurveSample CreateSample(float distance, float time)
		{
			return default;
		}

		public CurveSample GetSample(float time)
		{
			return default;
		}

		public CurveSample GetSampleAtDistance(float d)
		{
			return default;
		}

		private static void AssertTimeInBounds(float time)
		{
		}

		public CurveSample GetProjectionSample(Vector3 pointToProject)
		{
			return default;
		}
	}
}
