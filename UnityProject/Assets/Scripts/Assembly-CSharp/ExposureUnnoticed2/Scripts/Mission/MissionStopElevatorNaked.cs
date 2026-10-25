using ExposureUnnoticed2.Master.Mission;

namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionStopElevatorNaked : MissionBase
	{
		private float additionalDisplayTime;

		private int achievedCount;

		private bool[] isArrivedFloor;

		public static MissionStopElevatorNaked Instance { get; private set; }

		public int IsJustOpenElevatorEntranceFloor { get; set; }

		public override void Initialize(MMissionOverrideRecord record)
		{
base.Initialize(record);
if (Instance == null)
{
    Instance = this;
    isArrivedFloor = new bool[unchecked((int)record.FloatParam)];
}
}

		public override void Dispose()
		{
if (Instance == this) Instance = null;
}

		public override bool IsInCondition()
		{
			return false;
		}

		protected override float GetAddSatisfy()
		{
			return 0f;
		}

		public override void Reset()
		{
base.Reset();
additionalDisplayTime = 0f;
achievedCount = 0;
IsJustOpenElevatorEntranceFloor = -1;
for (int i = 0; i < isArrivedFloor.Length; i++) isArrivedFloor[i] = false;
}
	}
}
