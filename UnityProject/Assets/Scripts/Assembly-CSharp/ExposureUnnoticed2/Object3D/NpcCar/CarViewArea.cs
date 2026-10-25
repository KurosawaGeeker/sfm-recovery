using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.NPC.Script;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NpcCar
{
	public class CarViewArea : MonoBehaviour
	{
		private Dictionary<NpcComponent, bool> npcDict;

		private Dictionary<Rigidbody, bool> carDict;

		private bool isPlayerInArea;

		public bool IsInArea => false;

		private void OnTriggerEnter(Collider collider)
		{
		}

		private void OnTriggerExit(Collider collider)
		{
		}
	}
}
