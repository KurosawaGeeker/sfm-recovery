using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class MissionAreaManager : MonoBehaviour
	{
		public enum Area
		{
			None = 0,
			ResidenceCrossing = 1,
			MansionEntrance = 2,
			MansionTopFloor = 3,
			ConbiniRegiCounter = 4,
			ParkCentral = 5,
			ParkMaze = 6,
			Bridge = 7
		}

		private MissionAreaController[] areas;

		public static MissionAreaManager Instance { get; private set; }

		public Area CurrentArea { get; private set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		public static string TransString(Area area)
		{
			return null;
		}
	}
}
