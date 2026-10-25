using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class PlayerEnterAreaChecker : MonoBehaviour
	{
		public bool IsInArea { get; private set; }

		private void OnTriggerEnter(Collider collider)
		{
		}

		private void OnTriggerExit(Collider collider)
		{
		}
	}
}
