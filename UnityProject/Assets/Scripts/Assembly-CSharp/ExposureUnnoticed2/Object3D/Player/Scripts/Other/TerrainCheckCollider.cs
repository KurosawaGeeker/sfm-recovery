using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class TerrainCheckCollider : MonoBehaviour
	{
		private Collider collider;

		public List<GameObject> CollisionList { get; private set; } = new List<GameObject>(); // Original constructor RVA 0x407A50.

		private void Awake()
		{
		    // Original RVA 0x4073E0.
		    collider = GetComponent<Collider>();
		    EventManager.AddListener<StartTransitionStageEvent>(OnTransitionStageEvent, false);
		}

		public void OnDestroy()
		{
		    // Original RVA 0x407510.
		    EventManager.RemoveListener<StartTransitionStageEvent>(OnTransitionStageEvent);
		}

		private void OnTransitionStageEvent(StartTransitionStageEvent startTransitionStageEvent)
		{
		    // Original RVA 0x4075D0 and callback RVA 0x407B50; one scheduled frame, not a timer.
		    collider.enabled = false;
		    CollisionList.Clear();
		    Scheduler.DelayFlame(() => collider.enabled = true, 1);
		}

		private void OnTriggerEnter(Collider other)
		{
		    // Original RVA 0x4076E0. Preserve duplicate suppression and Level-only solids.
		    if (other.gameObject.layer != LayerAccessor.LevelLayer) return;
		    if (other.isTrigger) return;
		    if (!CollisionList.Contains(other.gameObject)) CollisionList.Add(other.gameObject);
		}

		private void OnTriggerExit(Collider collider)
		{
		    // Original RVA 0x4078E0; exits do not repeat the entry layer/trigger filters.
		    if (CollisionList.Contains(collider.gameObject)) CollisionList.Remove(collider.gameObject);
		}
	}
}
