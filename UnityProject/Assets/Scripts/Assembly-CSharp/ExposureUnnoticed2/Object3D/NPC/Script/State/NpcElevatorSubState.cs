using ExposureUnnoticed2.Object3D.RoutePoint;
using ExposureUnnoticed2.Object3D.ScenePlops.Elevator;

namespace ExposureUnnoticed2.Object3D.NPC.Script.State
{
	public class NpcElevatorSubState
	{
		private enum State
		{
			NoActive = 0,
			WaitElevator = 1,
			EnterElevator = 2,
			PressButton = 3,
			WaitTargetFloor = 4,
			ExitElevator = 5
		}

		private NpcComponentAccessor nca;

		private State currentState;

		private ElevatorController elevator;

		private ElevatorRidePositionController waitPosition;

		private float waitPositionRemainTime;

		private float continuePressEntranceButtonRemainTime;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
			elevator = ExposureUnnoticed2.Object3D.ScenePlops.Elevator.ElevatorController.Instance;
		}

		public void OnEnter()
		{
		}

		public void OnUpdate()
		{
		}

		private void ChangeState(State next)
		{
		}

		private void UpdateWaitElevator()
		{
		}

		private void UpdateEnterElevator()
		{
		}

		private void UpdateWaitTargetFloor()
		{
		}

		private void UpdateExitElevator()
		{
		}

		private ExposureUnnoticed2.Object3D.RoutePoint.RoutePoint[] GetElevatorPoints()
		{
			return null;
		}
	}
}
