using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class ObjectCullCollider : MonoBehaviour
	{
		public bool IsInArea { get; set; }

		private void OnTriggerEnter(Collider collider)
		{
		}

		private void OnTriggerExit(Collider collider)
		{
		}
	}
}
