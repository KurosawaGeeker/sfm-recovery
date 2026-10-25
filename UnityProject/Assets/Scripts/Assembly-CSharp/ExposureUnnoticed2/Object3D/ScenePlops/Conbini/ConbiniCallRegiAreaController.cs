using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.NPC.Script;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Conbini
{
	public class ConbiniCallRegiAreaController : MonoBehaviour
	{
		private bool isInArea;

		private HashSet<NpcComponent> inAreaNpcSet = new HashSet<NpcComponent>();

		public bool IsInAreaPlayer => isInArea;

		public bool IsInAreaBringNpc()
		{
			return false;
		}

		public void OnChangeTime()
		{
			inAreaNpcSet.Clear();
		}

		private void OnTriggerEnter(Collider collider)
		{
		}

		private void OnTriggerExit(Collider collider)
		{
		}
	}
}
