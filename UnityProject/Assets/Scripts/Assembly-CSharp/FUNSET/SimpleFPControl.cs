using UnityEngine;

namespace FUNSET
{
	public class SimpleFPControl : MonoBehaviour
	{
		[Header("* Need EventSystem *")]
		public float speed;

		public float Camsensitivity;

		[SerializeField]
		private float CamRotation;

		[Header("Set Camera")]
		[SerializeField]
		private Transform FPCcam;

		private CharacterController FPcharController;

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
