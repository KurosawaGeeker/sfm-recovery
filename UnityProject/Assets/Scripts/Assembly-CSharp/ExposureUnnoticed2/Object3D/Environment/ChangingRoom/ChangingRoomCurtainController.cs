using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Environment.ChangingRoom
{
	public class ChangingRoomCurtainController : MonoBehaviour
	{
		[SerializeField]
		private Collider collider;

		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private bool isByNpc;

		private SmoothFloat closeBlend;

		private SkinnedMeshRenderer mesh;

		[field: SerializeField]
		public Transform npcStartPoint { get; private set; }

		[field: SerializeField]
		public Transform npcInsidePoint { get; private set; }

		public bool IsOpen { get; private set; }

		private void Awake()
		{
		}

		private void Update()
		{
		}

		public void SetOpen(bool isOpen, bool isPlaySound = true)
		{
		}
	}
}
