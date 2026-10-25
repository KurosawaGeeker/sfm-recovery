using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class MissionAreaController : MonoBehaviour
	{
		[field: SerializeField]
		public MissionAreaManager.Area AreaType { get; private set; }

		public bool IsInArea { get; private set; }

		private void OnTriggerEnter(Collider collider)
		{
		}

		private void OnTriggerExit(Collider collider)
		{
		}
	}
}
