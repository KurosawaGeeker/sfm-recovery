using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Mission
{
	[CreateAssetMenu(fileName = "MMissionOverrideRecord", menuName = "ScriptableObjects/MMissionOverrideRecord")]
	public class MMissionOverrideRecord : ScriptableObject
	{
		[SerializeField]
		private string MEMO;

		[SerializeField]
		public MMissionRecord OriginalRecord;

		[SerializeField]
		public int RewardRankPoint;

		[SerializeField]
		public int ReleaseRank;

		[SerializeField]
		public float AddSatisfyScale;

		[SerializeField]
		public float AddSatisfyScalePassiveContinue;

		[SerializeField]
		public float FloatParam;

		[SerializeField]
		public MissionAreaManager.Area AreaType;

		[SerializeField]
		public MissionAreaManager.Area AreaType2;

		[SerializeField]
		public int Level;

		[SerializeField]
		public bool Trial;

		[SerializeField]
		public float OverrideRpPerSecScale;

		public int Id { get; set; }

		public StageType StageType { get; set; }

		public float RpPerSecScale => OverrideRpPerSecScale > 0f ? OverrideRpPerSecScale : OriginalRecord.RpPerSecScale;

		public string GetMissionName()
		{
var original = OriginalRecord;
string key = original.NameKey;
if (original.CategoryType == MissionCategoryType.Area)
{
    string param;
    switch (AreaType)
    {
        case MissionAreaManager.Area.ResidenceCrossing: param = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_mission_area_crossing"); break;
        case MissionAreaManager.Area.MansionEntrance: param = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_mission_area_entrance"); break;
        case MissionAreaManager.Area.ConbiniRegiCounter: param = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_mission_area_regi"); break;
        case MissionAreaManager.Area.ParkCentral: param = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_mission_area_park_central"); break;
        case MissionAreaManager.Area.ParkMaze: param = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_mission_area_park_maze"); break;
        case MissionAreaManager.Area.Bridge: param = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_mission_area_bridge"); break;
        default: param = ""; break;
    }
    return global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(key, param);
}
return global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(key, unchecked((int)FloatParam).ToString());
}

		public void Initialize(int id, StageType stageType)
		{
		}
	}
}
