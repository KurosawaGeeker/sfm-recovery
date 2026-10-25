using UnityEngine;

namespace FUNSET
{
	public class OutRangeCuller : MonoBehaviour
	{
		[Header("Set the culling target")]
		[SerializeField]
		[Tooltip("culling target")]
		private GameObject[] _CullActivateSet;

		[Tooltip("Player or camera position")]
		[SerializeField]
		[Header("Player or camera position")]
		private string PlayerCameraName;

		[Header("Player or camera position")]
		[SerializeField]
		[Tooltip("Player or camera position")]
		private GameObject PlayerCamera;

		[SerializeField]
		[Header("Outrange judgment distance")]
		[Tooltip("Outrange judgment distance")]
		private float Distance;

		[Header("Culling judgment frequency (seconds)")]
		[Tooltip("Culling judgment frequency (seconds)")]
		[SerializeField]
		private float Checkinterval;

		private float CheckTime;

		private void Start()
		{
		}

		public void Update()
		{
		}

		public void SetTarget(GameObject a)
		{
		}

		public void SetCheckInterval(float a)
		{
		}

		public void SetDistance(float a)
		{
		}

		private void CheckDistance(int a)
		{
		}
	}
}
