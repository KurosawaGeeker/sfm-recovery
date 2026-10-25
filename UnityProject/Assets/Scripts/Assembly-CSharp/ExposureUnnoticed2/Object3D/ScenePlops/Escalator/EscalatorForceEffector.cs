using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Escalator
{
	public class EscalatorForceEffector : MonoBehaviour
	{
		[SerializeField]
		private float aa;

		[SerializeField]
		private float npcScale;

		private int ignoreLayer;

		private escalator_control ec;

		private Dictionary<GameObject, bool> isAlreadyForcedDict;

		private List<GameObject> keyList;

		private void Awake()
		{
		}

		public bool IsPushed(GameObject go)
		{
			return false;
		}

		private void FixedUpdate()
		{
		}

		private void OnTriggerStay(Collider collider)
		{
		}
	}
}
