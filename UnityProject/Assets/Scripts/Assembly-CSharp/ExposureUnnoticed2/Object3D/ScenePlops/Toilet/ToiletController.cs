using ExposureUnnoticed2.Object3D.InteractArea;
using ExposureUnnoticed2.Object3D.ScenePlops.ChairSitController;
using ExposureUnnoticed2.Object3D.ScenePlops.CommonDoor;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Toilet
{
	public class ToiletController : MonoBehaviour
	{
		[SerializeField]
		private InteractAreaController interactAreaController;

		[SerializeField]
		private Transform ridePoint;

		[SerializeField]
		private Collider futaCollider;

		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private GameObject playerBlocker;

		private Animator animator;

		private bool isCurrentFutaOpen;

		private static readonly int IsFutaOpen;

		private float reflashTime;

		[field: SerializeField]
		public CommonDoorController commonDoorController { get; private set; }

		[field: SerializeField]
		public ExposureUnnoticed2.Object3D.ScenePlops.ChairSitController.ChairSitController ChairSitController { get; private set; }

		[field: SerializeField]
		public InSidePlayerChecker InSidePlayerChecker { get; private set; }

		[field: SerializeField]
		public Transform npcInteractStartPoint { get; private set; }

		[field: SerializeField]
		public Transform NpcMoveInsidePoint { get; private set; }

		public bool IsLocked => false;

		public bool IsOpenDoor => false;

		private void Awake()
		{
		}

		public void PlayFlash()
		{
		}

		private void OnEndDoorAnim()
		{
		}

		public void ActiveBlockPlayer(bool isActive, bool isForce)
		{
		}

		public void OnChangeTime()
		{
		}

		public void OpenFutaIfNeed()
		{
		}
	}
}
