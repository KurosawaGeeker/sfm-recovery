using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.PeeDecal
{
	public class PeeDecalManager : MonoBehaviour
	{
		private const int PoolSize = 100;

		private int ringIndex;

		private PeeDecalController[] decalPool;

		[SerializeField]
		private GameObject decalPrefab;

		public static PeeDecalManager Instance { get; private set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		public void CreatePeeDecal(Vector3 position, Vector3 normalV)
		{
		}

		private PeeDecalController GetInstance()
		{
			return null;
		}

		private void OnCompleteTransitionStageEvent(CompleteTransitionStageEvent completeTransitionStageEvent)
		{
		}
	}
}
