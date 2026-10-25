using ExposureUnnoticed2.Object3D.ScenePlops.ChairSitController;
using ExposureUnnoticed2.Object3D.ScenePlops.WasherMachine;
using ExposureUnnoticed2.Scripts.InGame.SceneProcessManager;

namespace ExposureUnnoticed2.Object3D.NPC.Script.State
{
	public class NpcLaundrySubState
	{
		private enum State
		{
			MoveToWasher = 0,
			IntoWasher = 1,
			MoveToChair = 2,
			SitDown = 3,
			Sit = 4,
			StandUp = 5,
			MoveToWasher2 = 6,
			TakeWasher = 7
		}

		private NpcComponentAccessor nca;

		private WasherMachineController washer;

		private ChairSitController chair;

		private LaundryManager manager;

		private State currentState;

		private bool isBook;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
			manager = ExposureUnnoticed2.Scripts.InGame.SceneProcessManager.LaundryManager.Instance;
		}

		public void OnEnter()
		{
		}

		public void OnUpdate()
		{
		}

		private void UpdateMoveToWasher()
		{
		}

		private void UpdateIntoWasher()
		{
		}

		private void UpdateMoveToChair()
		{
		}

		private void UpdateSitDown()
		{
		}

		private void UpdateSit()
		{
		}

		private void UpdateStandUp()
		{
		}

		private void UpdateMoveToWasher2()
		{
		}

		private void UpdateTakeWasher()
		{
		}
	}
}
