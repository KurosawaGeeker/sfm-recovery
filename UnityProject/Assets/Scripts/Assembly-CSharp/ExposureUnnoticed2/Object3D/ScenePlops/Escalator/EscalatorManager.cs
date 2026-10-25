using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Escalator
{
	public class EscalatorManager : MonoBehaviour
	{
		[SerializeField]
		private EscalatorController[] escalators;

		public static EscalatorManager Instance { get; private set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		public bool IsPushed(GameObject go)
		{
			return false;
		}
	}
}
