using ExposureUnnoticed2.Object3D.InteractArea;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.PinponDoor
{
	public class PinponDoorController : MonoBehaviour
	{
		private enum State
		{
			NoActive = 0,
			Wait = 1,
			OpenDoor = 2,
			CloseDoor = 3
		}

		[SerializeField]
		private bool isMirror;

		[SerializeField]
		private InteractAreaController interactAreaController;

		[SerializeField]
		private Transform npcParent;

		private Animator animator;

		private State currentState;

		private float ignoreByBaretaRemainTime;

		private float waitRemainTime;

		private float startOpenRemainTime;

		private float startCloseRemainTime;

		private float startInteractTime;

		private bool isActive;

		private bool isInArea;

		private bool isJustInteract;

		private bool isDuringPinponMission;

		private bool isMaxStrangeness;

		private bool isNoOne;

		private float animSpeed;

		private float randomScale;

		private float noOneResetActiveRemainTime;

		private NpcController npc;

		private static readonly int IsOpen;

		public bool IsActiveInArea => false;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnTimeStopEvent(TimeStopEvent evt)
		{
		}

		private void SetNoOnePinpon(ChangeTimeEvent evt)
		{
		}

		public void SpawnNpc()
		{
		}

		private void Update()
		{
		}

		private void NoActiveUpdate()
		{
		}

		private void WaitUpdate()
		{
		}

		private void OpenUpdate()
		{
		}

		private void CloseUpdate()
		{
		}

		private void OnTriggerEnter(Collider collider)
		{
		}

		private void OnTriggerExit(Collider collider)
		{
		}
	}
}
