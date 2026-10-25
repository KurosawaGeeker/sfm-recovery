using UnityEngine;
using UnityEngine.Splines;

namespace Common.Scripts.InGame
{
	public class OnSplineChaser : MonoBehaviour
	{
		[SerializeField]
		private Transform moveTarget;

		[SerializeField]
		private SplineContainer splineContainer;

		[SerializeField]
		private float activeDist;

		private float lastCalcTime;

		public Transform ChaseTarget { get; set; }

		private void Update()
		{
		}
	}
}
