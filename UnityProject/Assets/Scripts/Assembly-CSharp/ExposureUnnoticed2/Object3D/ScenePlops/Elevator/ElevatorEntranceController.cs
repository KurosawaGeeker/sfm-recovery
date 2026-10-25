using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Elevator
{
	public class ElevatorEntranceController : MonoBehaviour
	{
		[SerializeField]
		private Renderer numberOne;

		[SerializeField]
		private Renderer numberTen;

		[SerializeField]
		private Renderer arrow;

		private Animator animator;

		private static readonly int IsDoorOpen;

		private ElevatorController controller;

		private ElevatorController.ArrowState displayArrow;

		private int currentDisplayFloor;

		private Material arrowMaterial;

		private Material oneMaterial;

		private Material tenMaterial;

		private static readonly int EmissionColor;

		public void Initialize(ElevatorController controller)
		{
		}

		public void Dispose()
		{
		}

		public void OpenDoor()
		{
		}

		public void CloseDoor()
		{
		}

		private void Update()
		{
		}

		private void UpdateArrow()
		{
		}

		private void UpdateNumber()
		{
		}

		private void OnTimeStopEvent(TimeStopEvent evt)
		{
		}
	}
}
