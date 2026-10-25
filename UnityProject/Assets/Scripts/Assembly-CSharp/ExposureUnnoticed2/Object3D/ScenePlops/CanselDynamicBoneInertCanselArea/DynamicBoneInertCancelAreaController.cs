using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.CanselDynamicBoneInertCanselArea
{
	public class DynamicBoneInertCancelAreaController : MonoBehaviour
	{
		private PlayerEnterAreaChecker areaChecker;

		private bool old;

		public static DynamicBoneInertCancelAreaController Instance { get; private set; }

		public bool IsInArea => false;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}
	}
}
