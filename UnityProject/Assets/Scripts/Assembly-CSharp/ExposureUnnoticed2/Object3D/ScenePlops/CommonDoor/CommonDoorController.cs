using ExposureUnnoticed2.Object3D.InteractArea;
using ExposureUnnoticed2.Scripts.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.CommonDoor
{
	public class CommonDoorController : MonoBehaviour
	{
		[SerializeField]
		private InteractAreaController interactAreaController;

		[SerializeField]
		private Animator targetAnimator;

		[SerializeField]
		private Collider deActiveDoorCollider;

		[SerializeField]
		private float deActiveDoorColliderDelayTime;

		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private InGameSoundManager.SeType openSe;

		[SerializeField]
		private InGameSoundManager.SeType closeSe;

		[SerializeField]
		private float openSeDelay;

		[SerializeField]
		private float closeSeDelay;

		private static readonly int IsDoorOpen;

		private bool isPlayingAnimation;

		private bool currentIsActiveInteract;

		public bool IsLocked { get; private set; }

		public bool IsCurrentOpen { get; private set; }

		private void Awake()
		{
		}

		private void OnInteract()
		{
		}

		private bool CanInteract()
		{
			return false;
		}

		public void SetLocked(bool isLocked)
		{
		}

		public void SetActiveInteract(bool isActive)
		{
		}

		public void OpenClose()
		{
		}

		public void OnEndDoorAnim()
		{
		}
	}
}
