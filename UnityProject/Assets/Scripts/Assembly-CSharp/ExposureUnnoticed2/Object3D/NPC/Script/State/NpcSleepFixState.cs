namespace ExposureUnnoticed2.Object3D.NPC.Script.State
{
	public class NpcSleepFixState
	{
		private NpcComponentAccessor nca;

		private float wakeUpTime;

		private float transSleepTime;

		private float openEyeTime;

		private bool isAwake;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
			wakeUpTime = CalcSleepLength();
		}

		public void OnEnter()
		{
		}

		private float CalcSleepLength()
		{
			float random = UnityEngine.Random.value;
			float time = UnityEngine.Time.time;
			return time + (random * 30f + 5f);
		}

		public bool OnUpdate(bool isReturnExit = false)
		{
			return false;
		}
	}
}
