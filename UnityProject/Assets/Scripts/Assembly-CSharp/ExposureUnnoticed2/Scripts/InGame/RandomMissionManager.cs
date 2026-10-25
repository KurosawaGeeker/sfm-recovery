using System.Collections.Generic;
using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Scripts.DroneMission;
using ExposureUnnoticed2.Scripts.Mission;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class RandomMissionManager
	{
		public static RandomMissionManager Instance;

		private List<MissionBase> missionList;

		private int currentMissionOrderIndex;

		private StageType currentStateType;

		private int currentMissionNumId;

		public bool IsActiveMission { get; private set; }

		public int RpBonus { get; private set; }

		public MissionBase CurrentMission => missionList.Count <= currentMissionOrderIndex ? null : missionList[currentMissionOrderIndex];

		public RandomMissionBase MissionBase { get; private set; }

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x49A7D0
			Instance = this;
		}

		public void StartMission(StageType stageType, int missionNum, bool isRandomStage)
		{
		}

		private bool IsSkipFutanari(MissionBase mb)
		{
			return false;
		}

		private bool IsSkipPee(MissionBase mb)
		{
			return false;
		}

		public void OnUpdate()
		{
		}

		public void InterruptMission()
		{
// Complete original RVA 0x49B540; PE/typed identity is audited in this owned stage.
IsActiveMission = false;
}
	// Recovered original member RVA 0x49B550: .ctor
		public RandomMissionManager()
        {
            missionList = new List<MissionBase>();
            MissionBase = new RandomMissionBase();
        }
	}
}
