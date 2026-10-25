using System.Collections.Generic;
using ExposureUnnoticed2.Master.DroneMission;
using ExposureUnnoticed2.Scripts.DroneMission;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class DroneManager
	{
		public const int NoActiveMissionId = -1;

		private const string DroneMissionNameSpace = "ExposureUnnoticed2.Scripts.DroneMission.";

		public static DroneManager Instance;

		private readonly Dictionary<int, DroneMissionBase> missionDict;

		public bool IsActiveMission => GameState.GameStateData.ActiveDroneMissionId != -1;

		public void Initialize()
		{
            // Complete original0x487AC0; actual original master, Type lookup, constructors and Base.Initialize.
            Instance = this;
            foreach (var entry in MDroneMission.GetAll())
            {
                var record = entry.Value;
                object created = System.Activator.CreateInstance(System.Type.GetType("ExposureUnnoticed2.Scripts.DroneMission." + record.MissionClassName), false);
                if (created is null) throw new UnityEngine.UnityException("ミッションが存在しません:" + record.MissionClassName);
                var mission = (DroneMissionBase)created;
                mission.Initialize(record);
                missionDict.Add(mission.Record.Id, mission);
            }
        }

		public DroneMissionBase GetActiveMission()
		{
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ActiveDroneMissionId == -1) return null;
            int id = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ActiveDroneMissionId;
            return missionDict[id];
		}

		public DroneMissionBase GetMissionBase(RDroneMission record)
		{
			return null;
		}

		public void StartMission(int missionId)
		{
		}

		public void OnUpdate()
		{
		}

		public bool IsActiveVibeMission()
		{
			return false;
		}

		public void InterruptMission(bool isNoLog = false)
		{
            GameState.GameStateData.ActiveDroneMissionId = -1;
		}
	// Recovered original member RVA 0x4886A0: .ctor
		public DroneManager()
        {
            missionDict = new Dictionary<int, DroneMissionBase>();
        }
	}
}
