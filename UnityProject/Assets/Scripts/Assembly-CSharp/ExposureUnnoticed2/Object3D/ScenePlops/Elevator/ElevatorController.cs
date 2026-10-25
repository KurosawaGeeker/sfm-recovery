using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Elevator
{
	public class ElevatorController : MonoBehaviour
	{
		public enum ElevatorState
		{
			StopDoorClose = 0,
			StopDoorOpen = 1,
			DoorOpening = 2,
			DoorClosing = 3,
			Moving = 4
		}

		public enum ArrowState
		{
			Stop = 0,
			Up = 1,
			Down = 2
		}

		[SerializeField]
		private ElevatorBoxController elevatorBox;

		[SerializeField]
		private ElevatorEntranceController[] elevatorEntrances;

		[SerializeField]
		private GameObject pushArea;

		[SerializeField]
		private Transform warpHeightPosition;

		[SerializeField]
		private ElevatorRidePositionController[] elevatorNpcWaitPosition;

		[SerializeField]
		private Transform firstDoorParts;

		private const int DeadFloor = -99999;

		private const float Speed = 0.8f;

		private const int MaxTokenNum = 5;

		private bool isUpDirect;

		private bool[] entranceButtonUpState;

		private bool[] entranceButtonDownState;

		private float transStateRemainTime;

		private ElevatorSoundController soundController;

		private bool isSoundedBell;

		private int publishedTokenNum;

		private List<ElevatorRidePositionController> unUsePositionList;

		private float openExtendRemainTime;

		private SmoothFloat speed;

		public static ElevatorController Instance { get; private set; }

		public ArrowState CurrentArrowState { get; private set; }

		public int CurrentFloor { get; private set; }

		public int MoveTargetFloor { get; private set; }

		public Bounds PushBounds { get; private set; }

		public ElevatorState CurrentElevatorState { get; private set; }

		private ElevatorEntranceController currentEntrance => null;

		private ElevatorEntranceController moveTargetEntrance => null;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnPressOpenClose(bool isOpen)
		{
		}

		private void OnPressFloor(int floor)
		{
		}

		public void PressEntranceByNpc(int floor, bool isUp)
		{
		}

		public void Initialize()
		{
		}

		public ElevatorRidePositionController TryGetWaitPosition()
		{
			return null;
		}

		public void ReturnWaitPosition(ElevatorRidePositionController tf)
		{
		}

		public bool IsInsidePlayer()
		{
			return PushBounds.Contains(global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.Transform.position);
		}

		private void Update()
		{
		}

		private void FixedUpdate()
		{
		}

		private void ChangeState(ElevatorState nextElevatorState)
		{
		}

		private void UpdateStopDoorClose()
		{
		}

		private void OnOpenDoor()
		{
		}

		private void UpdateStopDoorOpen()
		{
		}

		private void FixedUpdateMoving()
		{
		}

		public bool IsInArea(Vector3 position)
		{
			return PushBounds.Contains(position);
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}

		public bool IsDoorOpen()
		{
			return false;
		}

		public bool IsDoorOpenOverHalf()
		{
			return firstDoorParts.localPosition.x < -0.005159603f;
		}
	}
}
