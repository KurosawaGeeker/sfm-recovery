using System;
using ExposureUnnoticed2.Object3D.InteractArea;
using ExposureUnnoticed2.Scripts.Base;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Elevator
{
	public class ElevatorBoxController : MonoBehaviour
	{
		[SerializeField]
		private InteractAreaController interactAreaController;

		[SerializeField]
		public AudioSource audioSource;

		[SerializeField]
		public AudioSource audioSourceMove;

		[SerializeField]
		private TextMeshProUGUI currentFloor;

		[SerializeField]
		private TextMeshProUGUI currentArrow;

		private Animator animator;

		private static readonly int IsDoorOpen;

		private Action onEndOpening;

		private Action onEndClosing;

		private ElevatorController controller;

		private ElevatorController.ArrowState displayArrow;

		private int currentDisplayFloor;

		public void Initialize(ElevatorController controller, int maxFloor, Action onEndOpening, Action onEndClosing, Action<bool> OnPressOpenClose, Action<int> OnPressFloor)
		{
		}

		public void Dispose()
		{
		}

		private void Update()
		{
		}

		public void OpenDoor()
		{
		}

		public void CloseDoor()
		{
		}

		private void OnEndDoorOpening()
		{
		}

		private void OnEndDoorClosing()
		{
		}

		private void OnTimeStopEvent(TimeStopEvent evt)
		{
		}
	}
}
