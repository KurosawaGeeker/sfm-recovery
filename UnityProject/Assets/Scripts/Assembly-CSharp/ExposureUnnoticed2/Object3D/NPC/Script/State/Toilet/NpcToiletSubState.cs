namespace ExposureUnnoticed2.Object3D.NPC.Script.State.Toilet
{
	public class NpcToiletSubState
	{
		private enum ToiletBaseState
		{
			Pee = 0,
			Koshitsu = 1,
			HandWash = 2
		}

		private NpcComponentAccessor nca;

		private bool isPee;

		private ToiletBaseState currentState;

		private NpcToiletPeeSubState peeState = new NpcToiletPeeSubState();

		private NpcToiletHandWashState handWashState = new NpcToiletHandWashState();

		private NpcToiletKoshitsuState koshitsuState = new NpcToiletKoshitsuState();

		public bool IsPeeing
        {
            get
            {
                if (currentState != ToiletBaseState.Pee) return false;
                return peeState.currentState == NpcToiletPeeSubState.State.Peeing;
            }
        }

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
			peeState.Initialize(nca, () => currentState = ToiletBaseState.HandWash);
			handWashState.Initialize(nca);
			koshitsuState.Initialize(nca, () => currentState = ToiletBaseState.HandWash);
		}

		public void OnEnter()
		{
		}

		public void OnUpdate()
		{
		}
	}
}
