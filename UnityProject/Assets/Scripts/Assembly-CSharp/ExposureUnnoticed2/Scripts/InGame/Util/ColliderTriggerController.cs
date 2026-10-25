using System;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame.Util
{
	public class ColliderTriggerController : MonoBehaviour, IColliderTriggerController
	{
		private Func<Collider, bool> condition;

		private Collider collider;

		private bool isCalcPenetrate;

		private Collider targetCollider;

		public bool IsInArea { get; private set; }

		public float PenetrateDist { get; private set; }

		public GameObject CenterObject => gameObject;

		public GameObject GameObject => null;

		public void Awake()
		{
		}

		public void Initialize(Func<Collider, bool> condition, bool isCalcPenetrate)
		{
		}

		private void Update()
		{
		}

		private void OnTriggerEnter(Collider targetCollider)
		{
		}

		private void OnTriggerExit(Collider collider)
		{
		}
	}
}
