using System;
using ExposureUnnoticed2.Object3D.ScenePlops.ChairSitController;
using ExposureUnnoticed2.Object3D.ScenePlops.Toilet;
using ExposureUnnoticed2.Object3D.ToiletManager;

namespace ExposureUnnoticed2.Object3D.NPC.Script.State.Toilet
{
	public class NpcToiletKoshitsuState
	{
		private enum State
		{
			Wait = 0,
			Move = 1,
			OpenDoorFromOutside = 2,
			MoveInside = 3,
			CloseDoor = 4,
			SitDown = 5,
			Sit = 6,
			StandUp = 7,
			OpenDoorFromInSide = 8,
			MoveOut = 9
		}

		private State currentState;

		private NpcComponentAccessor nca;

		private ExposureUnnoticed2.Object3D.ToiletManager.ToiletManager toiletManager;

		private ToiletController toiletController;

		private ChairSitController chairSitController;

		private float motionEndRemainTime;

		private int phase;

		private Action onEnd;

		public void Initialize(NpcComponentAccessor nca, Action onEnd)
		{
			this.nca = nca;
			toiletManager = ExposureUnnoticed2.Object3D.ToiletManager.ToiletManager.Instance;
			this.onEnd = onEnd;
		}

		public void OnUpdate()
		{
		}

		private void UpdateWait()
		{
		}

		private void UpdateMove()
		{
		}

		private void UpdateOpenDoorFromOutside()
		{
		}

		private void UpdateMoveInside()
		{
		}

		private void UpdateCloseDoor()
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

		private void UpdateOpenFromInside()
		{
		}

		private void UpdateMoveOut()
		{
		}
	}
}
