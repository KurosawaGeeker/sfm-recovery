namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcStaggerState
	{
		private NpcComponentAccessor nca;

		private float remainTime;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
		}

		public void OnEnter()
		{
			remainTime = 1f;
		}

		public void OnUpdate()
		{
			float previous = remainTime;
			float delta = UnityEngine.Time.deltaTime;
			remainTime = previous - delta;
			if (remainTime <= 0f) nca.NpcController.ChangeState(NpcStateModel.State.TurnAround);
		}
	}
}
