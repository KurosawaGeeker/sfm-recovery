using System;
using UnityEngine;

namespace SplineMesh
{
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Spline))]
	public class SplineSmoother : MonoBehaviour
	{
		private Spline spline;

		[Range(0f, 1f)]
		public float curvature;

		private Spline Spline => null;

		private void OnValidate()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void Spline_NodeListChanged(object sender, ListChangedEventArgs<SplineNode> args)
		{
		}

		private void OnNodeChanged(object sender, EventArgs e)
		{
		}

		private void SmoothNode(SplineNode node)
		{
		}

		private void SmoothAll()
		{
		}
	}
}
