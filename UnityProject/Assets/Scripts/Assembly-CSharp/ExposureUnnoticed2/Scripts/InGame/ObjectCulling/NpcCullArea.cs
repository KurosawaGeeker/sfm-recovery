using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.NPC.Script;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame.ObjectCulling
{
	public class NpcCullArea : MonoBehaviour
	{
		private Dictionary<NpcComponent, bool> npcDict;

		private void Awake()
		{
		}

		private void OnTriggerEnter(Collider collider)
		{
		}

		private void OnTriggerExit(Collider collider)
		{
		}

		public bool IsInArea(NpcComponent nc)
		{
			return false;
		}
	}
}
