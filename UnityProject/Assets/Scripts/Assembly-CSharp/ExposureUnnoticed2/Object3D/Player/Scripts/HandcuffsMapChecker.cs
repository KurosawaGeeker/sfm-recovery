using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class HandcuffsMapChecker : MonoBehaviour
	{
		private Dictionary<Collider, bool> dict;

		private List<Collider> keyList;

		private int ignoreLayer;

		public bool IsInArea => false;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnTransitionStageEvent(StartTransitionStageEvent startTransitionStageEvent)
		{
		}

		private void Update()
		{
		}

		private void OnTriggerEnter(Collider collider)
		{
		}

		private void OnTriggerExit(Collider collider)
		{
		}
	}
}
