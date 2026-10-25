using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace ch.sycoforge.Decal.Demo
{
	[RequireComponent(typeof(NavMeshAgent))]
	[RequireComponent(typeof(LineRenderer))]
	public class PathAgent : MonoBehaviour
	{
		public float PathThickness;

		[Tooltip("Distance from the ground.")]
		public float NormalPathOffset;

		[Range(0.001f, 0.5f)]
		[Tooltip("Max radius between segments.")]
		public float Radius;

		[Tooltip("Discard segments when their angle is smaller than this value.")]
		public float AngleThreshold;

		public bool DrawGizmos;

		public EasyDecal TargetAimDecal;

		public GameObject TargetPointDecalPrefab;

		private List<Vector3> path;

		private NavMeshAgent agent;

		private LineRenderer lineRenderer;

		private Vector3 decalOffset;

		private const int MAXDISTANCE = 50;

		private void Start()
		{
		}

		private void Update()
		{
		}

		private void SetTarget(Ray mouseRay)
		{
		}

		private void CreatePath(Ray mouseRay)
		{
		}

		private void OnDrawGizmos()
		{
		}
	}
}
