namespace ExposureUnnoticed2.Object3D.NPC.Script.State
{
	public class NpcChairSubState
	{
		public enum ChairState
		{
			SitDown = 0,
			Action = 1,
			StandUp = 2
		}

		public enum ChairActionType
		{
			None = 0,
			Sleep = 1,
			Book = 2,
			Smartphone = 3,
			End = 4
		}

		private NpcComponentAccessor nca;

		public ChairState currentChairState;

		private float ActionRemainTime;

		public bool IsCurrentChairSleep
		{
			get
			{
				if (currentChairState != ChairState.Action) return false;
				return nca.StateModel.CurrentChairActionType == ChairActionType.Sleep;
			}
		}

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
		}

		public void OnEnter()
		{
		}

		public void OnUpdate()
		{
		}

		private void UpdateSitDown()
		{
		}

		private void UpdateStandUp()
		{
		}

		private void UpdateAction()
		{
		}
	}
}
