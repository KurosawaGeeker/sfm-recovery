using ExposureUnnoticed2.Master.Hair;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame.SceneProcessManager
{
	public class BarberShopManager : MonoBehaviour
	{
		public static BarberShopManager Instance { get; private set; }

		public HairType TemporaryCurrentHair { get; set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}
	}
}
