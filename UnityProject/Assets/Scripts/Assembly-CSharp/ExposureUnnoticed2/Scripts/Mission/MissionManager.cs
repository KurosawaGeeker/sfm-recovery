using System;
using ExposureUnnoticed2.Master.Mission;
using UnityEngine;
using System.Collections.Generic;
using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Master.Stage;

namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionManager
	{
		private const string TimeMissionNameSpace = "ExposureUnnoticed2.Scripts.Mission.";

		private const string ConditionMissionNameSpace = "ExposureUnnoticed2.Scripts.Mission.";

		public static MissionManager Instance;

		public List<MissionBase> MissionList;

		public Dictionary<StageType, List<MissionBase>> MissionDict;

		public void Initialize()
		{
Instance = this;
foreach (var area in MMission.GetAll())
{
    MissionDict.Add(area.Key, new List<MissionBase>());
    foreach (var recordPair in area.Value.GetAll())
    {
        var record = recordPair.Value;
        string prefix;
        var type = record.OriginalRecord.MissionType;
        switch (type)
        {
            case MissionType.Time:
            case MissionType.Condition: prefix = "ExposureUnnoticed2.Scripts.Mission."; break;
            case MissionType.Continue: prefix = ""; break;
            default: throw new System.Runtime.CompilerServices.SwitchExpressionException(type);
        }
        object created = Activator.CreateInstance(Type.GetType(prefix + record.OriginalRecord.MissionClassName), false);
        if (created is null)
            throw new UnityException("ミッションが存在しません:" + record.OriginalRecord.MissionClassName);
        var mission = (MissionBase)created;
        mission.Initialize(record);
        MissionList.Add(mission);
        MissionDict[area.Key].Add(mission);
    }
}
}

		public bool IsSuccessMission(MMissionOverrideRecord record)
        {
            foreach (var mission in MissionList)
                if (mission.RMission == record) return mission.Satisfy >= 1f;
            return false;
        }

		public void Dispose()
		{
if (Instance == this)
{
    Instance = null;
    foreach (var mission in MissionList) mission.Dispose();
}
}

		public void OnLateUpdate()
		{
foreach (var mission in MissionList) mission.Update();
}

		public void ResetMission()
		{
foreach (var mission in MissionList) mission.Reset();
}
	// Recovered original member RVA 0x47E270: .ctor
		public MissionManager()
        {
            MissionList = new List<MissionBase>();
            MissionDict = new Dictionary<StageType, List<MissionBase>>();
        }
	}
}
