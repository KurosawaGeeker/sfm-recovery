using UnityEngine;

namespace ExposureUnnoticed2.Master.Stage
{
	public class MStageRecord : ScriptableObject
	{
		public StageType StageType;

		public string SceneName;

		public string StageNameKey;

		public bool IsGoalTarget;

		public int NpcNum;

		public int NpcNumNight;

		public bool NpcNumRandom;

		public bool NpcNumRandomInitExist;

		public int FearLevelPenalty;

		public int StageRpBonus100;

		public int DefaultLightBareyasusaScale100;

		public float StartSightScaleReduceDist;

		public bool IsProhibitedWaitArea;

		public int EnterRankLimit;

		public bool IsUseDrone;

		public float DroneMissionRpBonus;

		public bool IsNoRandomStartDroneMission;

		public bool IsNightSpermDecal;

		[Header("固定NPC")]
		public int NpcTalkNumDaytime;

		public int NpcTalkNumNight;

		public int NpcFixSitNumDaytime;

		public int NpcFixSitNumNight;
	}
}
