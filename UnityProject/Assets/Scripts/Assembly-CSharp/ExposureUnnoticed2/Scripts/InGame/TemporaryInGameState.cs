using System.Collections.Generic;
using ExposureUnnoticed2.Master.Stage;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class TemporaryInGameState
	{
		public static bool IsCollectDropItem;

		public bool IsStopGameStateApplier;

		public bool IsGameOver;

		public bool IsFirstLoadStage;

		public bool IsPlayingSomePerformance;

		public bool IsTimeStop;

		public StageType BeforeStageType;

		public bool IsDownRank;

		public int LostExp;

		public int LostRp;

		public HashSet<int> MissionCompleteThisGameSet;

		public Dictionary<int, int> MissionMaxRpBonus100ThisGameDict;

		public bool DroneMissionCompleteThisGame;

		public Dictionary<StageType, int> AchieveMissionCountByStageThisGameDict;

		public float CurrentEarnThisTimePerSecRp;

		public bool IsNoAchieveMissionMode;

		public float CurrentMotionSpeed;

		public Vector3? EntryStageInitPosition;
	// Recovered original member RVA 0x4A8510: .ctor
		public TemporaryInGameState()
        {
            IsFirstLoadStage = true;
            MissionCompleteThisGameSet = new HashSet<int>();
            MissionMaxRpBonus100ThisGameDict = new Dictionary<int, int>();
            AchieveMissionCountByStageThisGameDict = new Dictionary<StageType, int>();
        }
	}
}
