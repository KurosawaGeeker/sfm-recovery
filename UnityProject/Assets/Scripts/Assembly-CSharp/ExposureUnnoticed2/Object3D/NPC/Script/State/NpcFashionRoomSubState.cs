using ExposureUnnoticed2.Object3D.Environment.ChangingRoom;

namespace ExposureUnnoticed2.Object3D.NPC.Script.State
{
	public class NpcFashionRoomSubState
	{
		private NpcComponentAccessor nca;

		private int statePhase;

		private ChangingRoomCurtainController targetRoom;

		private float stateEndRemainTime;

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
	}
}
