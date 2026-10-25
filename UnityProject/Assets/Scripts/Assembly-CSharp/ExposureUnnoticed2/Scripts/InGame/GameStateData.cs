using System.Collections.Generic;
using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Master.Clothes;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Master.Costume;
using ExposureUnnoticed2.Master.Hair;
using ExposureUnnoticed2.Master.Item;
using ExposureUnnoticed2.Master.Skill;
using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu;
using ExposureUnnoticed2.ObjectUI.InventoryPanel;
using ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class GameStateData
	{
		[ES3Serializable]
		public StageType CurrentStageType;

		[ES3Serializable]
		public Dictionary<DroppedItemManager.DropItemType, DroppedItemManager.DropData> DropDataDict;

		[ES3Serializable]
		public bool IsDaytime;

		[ES3Serializable]
		public List<DroppedItemManager.DropData> ReserveDropList;

		[ES3Serializable]
		public float SlowRemainTime;

		[ES3Serializable]
		public DifficultType DifficultType;

		[ES3Serializable]
		public List<int> ReleaseFastTravel;

		[ES3Serializable]
		public List<NpcSaveData> NpcSaveDataList;

		[ES3Serializable]
		public int ReleasedInfoRank;

		[ES3Serializable]
		public int ConfirmedReleasedInfoRank;

		[ES3Serializable]
		public int RingShortcutModelIndexCommon;

		[ES3Serializable]
		public int RingShortcutModelIndexSkill;

		[ES3Serializable]
		public int RingShortcutModelIndexSkill3;

		[ES3Serializable]
		public int RingShortcutModelIndexSkill4;

		[ES3Serializable]
		public int RingShortcutModelIndexSkill5;

		[ES3Serializable]
		public int RingShortcutModelIndexSkill6;

		[ES3Serializable]
		public int RingShortcutModelIndexSkill7;

		[ES3Serializable]
		public int RingShortcutModelIndexSkill8;

		[ES3Serializable]
		public HashSet<int> AchievementReceiveSet;

		[ES3Serializable]
		public int UsingWasherIndex;

		[ES3Serializable]
		public int MotionSpeedScaleIndex;

		[ES3Serializable]
		public int WalkSpeedScaleIndex;

		[ES3Serializable]
		public Dictionary<int, CosplayPartsOptionData> CosplayPartsOptionDataDict;

		[ES3Serializable]
		public int ConfirmedReinforceRank;

		[ES3Serializable]
		public string[] CosplayMySetNames;

		[ES3Serializable]
		public string[] CustomizeMySetNames;

		[ES3Serializable]
		public Dictionary<int, bool[]> CosplayReinforceLockState;

		[ES3Serializable]
		public Dictionary<MAdultGoodsType, bool[]> AdultGoodsReinforceLockState;

		[ES3Serializable]
		public bool IsDroneNeedCharge;

		[ES3Serializable]
		public int ActiveDroneMissionId;

		[ES3Serializable]
		public float ActiveDroneProgress;

		[ES3Serializable]
		public List<DroppedItemManager.DropItemType> DroneManagerDropList;

		[ES3Serializable]
		public Dictionary<int, int> DroneMissionAchieveCountDict;

		[ES3Serializable]
		public Dictionary<StageType, Dictionary<int, int>> DroneMissionAchieveCountPerStageDictDict;

		[ES3Serializable]
		public Dictionary<StageType, Dictionary<int, int>> DroneMissionMaxRpBonus100PerStageDictDict;

		[ES3Serializable]
		public Dictionary<StageType, int> RandomMissionClearNumIdDict;

		[ES3Serializable]
		public Dictionary<int, int> MissionAchieveCountDict;

		[ES3Serializable]
		public Dictionary<int, int> ContinueMissionAchieveCountDict;

		[ES3Serializable]
		public Dictionary<int, float> MissionSatisfyDict;

		[ES3Serializable]
		public Dictionary<int, float> ContinueMissionSatisfyDict;

		[ES3Serializable]
		public Dictionary<int, float> MissionEarnRpDict;

		[ES3Serializable]
		public Dictionary<int, int> MissionMaxRpBonus100Dict;

		[ES3Serializable]
		public float DroneMissionEarnRp;

		[ES3Serializable]
		public uint StatistisTotalPlayTime;

		[ES3Serializable]
		public uint StatistisTotalExposeTime;

		[ES3Serializable]
		public int StatistisGameOverCount;

		[ES3Serializable]
		public int StatistisEcstasyCount;

		[ES3Serializable]
		public int StatistisMaxEarnRp;

		[ES3Serializable]
		public double StatistisTotalEarnRp;

		[ES3Serializable]
		public double StatistisTotalLostRp;

		[ES3Serializable]
		public double StatistisTotalLostExp;

		[ES3Serializable]
		public double StatistisTotalUseRp;

		[ES3Serializable]
		public int StatistisMissionAchiveCount;

		[ES3Serializable]
		public float StatistisMaxRpBoost;

		[ES3Serializable]
		public float StatistisMaxNudeMoveDistance;

		[ES3Serializable]
		public int StatistisMaxEarnRpByOneDay;

		[ES3Serializable]
		public float PlayerEcstasy;

		[ES3Serializable]
		public float PlayerMental;

		[ES3Serializable]
		public float PlayerMoisture;

		[ES3Serializable]
		public float PlayerAddMoisture;

		[ES3Serializable]
		public float PlayerFear;

		[ES3Serializable]
		public float PlayerRemainBodyPaintTime;

		[ES3Serializable]
		public Vector3 PlayerPosition;

		[ES3Serializable]
		public Quaternion PlayerRotation;

		[ES3Serializable]
		public ClothesType PlayerCurrentClothes;

		[ES3Serializable]
		public PlayerStateModel.HadObjectType PlayerCurrentHadObject;

		[ES3Serializable]
		public ClothesType PlayerCurrentHoldClothes;

		[ES3Serializable]
		public bool PlayerIsAttachHandcuffs;

		[ES3Serializable]
		public bool PlayerIsAttachMapHandcuffs;

		[ES3Serializable]
		public MAdultGoodsType AttachedHandcuffType;

		[ES3Serializable]
		public float PlayerHandcuffTimerRemainTime;

		[ES3Serializable]
		public int PlayerHandcuffTimerSetTime;

		[ES3Serializable]
		public VibrationModeType PlayerVibrationModeType;

		[ES3Serializable]
		public int PlayerLife;

		[ES3Serializable]
		public int PlayerMaxLife;

		[ES3Serializable]
		public PlayerStateModel.ClothesState PlayerCurrentClothesAState;

		[ES3Serializable]
		public PlayerStateModel.ClothesState PlayerCurrentClothesBState;

		[ES3Serializable]
		public Dictionary<SkillType, bool> PlayerPassiveSkillActiveDict;

		[ES3Serializable]
		public PlayerStateModel.SexualityType CurrentSexualityType;

		[ES3Serializable]
		public bool IsActiveFutanari;

		[ES3Serializable]
		public bool IsActiveInvisible;

		[ES3Serializable]
		public bool IsActiveInvisibleReusable;

		[ES3Serializable]
		public bool IsActiveBodyPaintReusable;

		[ES3Serializable]
		public double CurrentRp;

		[ES3Serializable]
		public int ExposeExp;

		[ES3Serializable]
		public double CurrentEarnThisTimeRp;

		[ES3Serializable]
		public Dictionary<SkillType, int> SkillLevel;

		[ES3Serializable]
		public Dictionary<ClothesType, bool> ClothesDict;

		[ES3Serializable]
		public Dictionary<MAdultGoodsType, bool> MAdultGoodsDict;

		[ES3Serializable]
		public Dictionary<CostumeType, bool> PlayerCostumeDict;

		[ES3Serializable]
		public Dictionary<ItemType, int> ItemDict;

		[ES3Serializable]
		public Dictionary<int, bool> PlayerCosplayPartsDict;

		[ES3Serializable]
		public Dictionary<MAdultGoodsType, int[]> AdultGoodsReinforceEffectDict;

		[ES3Serializable]
		public Dictionary<int, int[]> CosplayPartsReinforceEffectDict;

		[ES3Serializable]
		public Dictionary<CostumeType, int[]> CostumeReinforceEffectDict;

		[ES3Serializable]
		public List<HairType> HairTypeList;

		[ES3Serializable]
		public List<LoadoutData> LoadoutDatas;

		[ES3Serializable]
		public List<LoadoutCustomizeData> LoadoutCustomizeDatas;

		[ES3Serializable]
		public int PlayerFace0Id;

		[ES3Serializable]
		public int PlayerFace1Id;

		[ES3Serializable]
		public int PlayerFace2Id;

		[ES3Serializable]
		public int PlayerFace3Id;

		[ES3Serializable]
		public int PlayerFaceIkuId;

		[ES3Serializable]
		public int PlayerFaceIkuAfterId;

		[ES3Serializable]
		public int VibeRemoconLevel;

		[ES3Serializable]
		public int PistonMachineModeType;

		[ES3Serializable]
		public int DroneLevel;
	// Recovered original member RVA 0x48BBD0: .ctor
		public GameStateData()
        {
            PlayerCurrentHadObject = (PlayerStateModel.HadObjectType)(-1);
            PlayerCurrentHoldClothes = (ClothesType)(-1);
            PlayerPassiveSkillActiveDict = new Dictionary<SkillType, bool>();
            ClothesDict = new Dictionary<ClothesType, bool>();
            MAdultGoodsDict = new Dictionary<MAdultGoodsType, bool>();
            PlayerCostumeDict = new Dictionary<CostumeType, bool>();
            ItemDict = new Dictionary<ItemType, int>();
            PlayerCosplayPartsDict = new Dictionary<int, bool>();
            AdultGoodsReinforceEffectDict = new Dictionary<MAdultGoodsType, int[]>();
            CosplayPartsReinforceEffectDict = new Dictionary<int, int[]>();
            CostumeReinforceEffectDict = new Dictionary<CostumeType, int[]>();
            DifficultType = (DifficultType)2;
            ActiveDroneMissionId = -1;
            DroneManagerDropList = new List<DroppedItemManager.DropItemType>();
            RingShortcutModelIndexCommon = -1;
            RingShortcutModelIndexSkill = -1;
            CurrentStageType = (StageType)0;
            RingShortcutModelIndexSkill3 = -1;
            RingShortcutModelIndexSkill4 = -1;
            RingShortcutModelIndexSkill5 = -1;
            RingShortcutModelIndexSkill6 = -1;
            RingShortcutModelIndexSkill7 = -1;
            RingShortcutModelIndexSkill8 = -1;
            UsingWasherIndex = -1;
            DropDataDict = GameStateFacade.GetDefaultDropDataDict();
            CosplayPartsOptionDataDict = new Dictionary<int, CosplayPartsOptionData>();
            AchievementReceiveSet = new HashSet<int>();
            ReserveDropList = new List<DroppedItemManager.DropData>();
            NpcSaveDataList = new List<NpcSaveData>();
            ReleaseFastTravel = new List<int>();
            MissionAchieveCountDict = new Dictionary<int, int>();
            MissionSatisfyDict = new Dictionary<int, float>();
            MissionMaxRpBonus100Dict = new Dictionary<int, int>();
            ContinueMissionAchieveCountDict = new Dictionary<int, int>();
            ContinueMissionSatisfyDict = new Dictionary<int, float>();
            MissionEarnRpDict = new Dictionary<int, float>();
            DroneMissionAchieveCountDict = new Dictionary<int, int>();
            DroneMissionAchieveCountPerStageDictDict = new Dictionary<StageType, Dictionary<int, int>>();
            DroneMissionMaxRpBonus100PerStageDictDict = new Dictionary<StageType, Dictionary<int, int>>();
            RandomMissionClearNumIdDict = new Dictionary<StageType, int>();
            PlayerVibrationModeType = (VibrationModeType)0;
            PlayerRotation = Quaternion.identity;
            ClothesDict = GameStateFacade.GetDefaultClothesDict();
            MAdultGoodsDict = new Dictionary<MAdultGoodsType, bool>();
            PlayerCostumeDict = GameStateFacade.GetDefaultCostumeDict();
            ItemDict = new Dictionary<ItemType, int>();
            SkillLevel = GameStateFacade.GetDefaultSkillDict();
            PlayerMental = 150f;
            PlayerCurrentClothes = (ClothesType)0;
            PlayerCurrentClothesAState = (PlayerStateModel.ClothesState)0;
            PlayerCurrentClothesBState = (PlayerStateModel.ClothesState)10;
            AttachedHandcuffType = (MAdultGoodsType)3;
            PlayerLife = 1;
            PlayerMaxLife = 1;
            PlayerPassiveSkillActiveDict = new Dictionary<SkillType, bool>();
            PlayerCosplayPartsDict = GameStateFacade.CreateDefaultCosplayPartsDict();
            CurrentSexualityType = (PlayerStateModel.SexualityType)0;
            AdultGoodsReinforceEffectDict = GameStateFacade.CreateDefaultReinforceEffectAdultGoods();
            CosplayPartsReinforceEffectDict = GameStateFacade.CreateDefaultReinforceEffectCosplayParts();
            CostumeReinforceEffectDict = GameStateFacade.CreateDefaultReinforceEffectCostume();
            LoadoutDatas = GameStateFacade.CreateDefaultLoadoutData();
            HairTypeList = GameStateFacade.TransHairType(new List<int>());
            if (HairTypeList.Count == 0 || !HairTypeList.Contains((HairType)0)) HairTypeList.Add((HairType)0);
            LoadoutCustomizeDatas = GameStateFacade.CreateDefaultLoadoutCustomizeData();
            PlayerFace0Id = 0;
            PlayerFace1Id = 1;
            PlayerFace2Id = 2;
            PlayerFace3Id = 3;
            PlayerFaceIkuId = 4;
            PlayerFaceIkuAfterId = 5;
            VibeRemoconLevel = 3;
            PistonMachineModeType = 0;
            DroneLevel = 5;
        }
	}
}
