using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class SaveManager
	{
		public const int AutoSaveMaxNum = 3;

		public const int ManualSaveMaxNum = 20;

		public static int CurrentSaveSlot;

		public static bool IsExistSaveData(SaveRapper.SaveDataType type, int slot = -1)
		{
			// Recovered from GameAssembly.dll RVA 0x49BF10
			return SaveRapper.IsExistSaveData(type, slot);
		}

		private static void SaveSystem(SaveRapper.SaveDataType type, int slot = -1)
		{
            // Recovered original RVA 0x49C0A0..0x49C589, full 1257-byte body.
            if (type == SaveRapper.SaveDataType.Interrupt)
                GameState.InterruptSaveTime = System.DateTime.Now;
            else if (type == SaveRapper.SaveDataType.AutoSave)
                GameState.AutoSaveTime[slot] = System.DateTime.Now;
            else if (type == SaveRapper.SaveDataType.HomeSave)
                GameState.HomeSaveTime = System.DateTime.Now;
            else if (type == SaveRapper.SaveDataType.ManualSlot)
            {
                GameState.ManualSaveTime[slot] = System.DateTime.Now;
                GameState.DifficultData[slot] = GameState.GameStateData.DifficultType;
                GameState.RankData[slot] = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentRank.Rank;
                GameState.TotalPlayTimeData[slot] = GameState.GameStateData.StatistisTotalPlayTime;
            }
            SaveRapper.PreProcess(true, -1, false);
            SaveRapper.Save("AutoSaveTime", GameState.AutoSaveTime);
            SaveRapper.Save("ManualSaveTime", GameState.ManualSaveTime);
            SaveRapper.Save("DifficultData", GameState.DifficultData);
            SaveRapper.Save("RankData", GameState.RankData);
            SaveRapper.Save("TotalPlayTimeData", GameState.TotalPlayTimeData);
            SaveRapper.Save("InterruptSaveTime", GameState.InterruptSaveTime);
            SaveRapper.Save("HomeSaveTime", GameState.HomeSaveTime);
            SaveRapper.Save("TutorialData", GameState.TutorialData);
            SaveRapper.Save("OptionData", GameState.OptionData);
        }

		public static void LoadSystem()
		{
			// Recovered from GameAssembly.dll RVA 0x49C590
			SaveRapper.PreProcess(true, -1, false);
			GameState.AutoSaveTime = SaveRapper.Load("AutoSaveTime", new System.DateTime[3]);
			GameState.ManualSaveTime = SaveRapper.Load("ManualSaveTime", new System.DateTime[20]);
			GameState.DifficultData = SaveRapper.Load("DifficultData", new global::ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel.DifficultType[20]);
			GameState.RankData = SaveRapper.Load("RankData", new int[20]);
			GameState.TotalPlayTimeData = SaveRapper.Load("TotalPlayTimeData", new uint[20]);
			GameState.InterruptSaveTime = SaveRapper.Load("InterruptSaveTime", default(System.DateTime));
			GameState.HomeSaveTime = SaveRapper.Load("HomeSaveTime", default(System.DateTime));
			GameState.TutorialData = SaveRapper.Load("TutorialData", new global::ExposureUnnoticed2.ObjectUI.TutorialPanel.TutorialData());
			GameState.OptionData = SaveRapper.Load("OptionData", new global::ExposureUnnoticed2.ObjectUI.OptionMenu.OptionData());
		}

		public static void Save(SaveRapper.SaveDataType type, int slot = -1, bool isBackup = false)
		{
			// Recovered from GameAssembly.dll complete RVA 0x49CDF0..0x49D37F.
			if (type == SaveRapper.SaveDataType.ManualSlot)
			    slot = CurrentSaveSlot;
			SaveSystem(type, slot);
			if (type == SaveRapper.SaveDataType.System)
			    return;
			SaveRapper.PreProcess(false, slot, isBackup);
			if (type == SaveRapper.SaveDataType.AutoSave)
			{
			    ES3.CopyFile(SaveRapper.GetSpecifiedSaveDataPath(SaveRapper.SaveDataType.AutoSave, 1),
			                 SaveRapper.GetSpecifiedSaveDataPath(SaveRapper.SaveDataType.AutoSave, 2));
			    ES3.CopyFile(SaveRapper.GetSpecifiedSaveDataPath(SaveRapper.SaveDataType.AutoSave, 0),
			                 SaveRapper.GetSpecifiedSaveDataPath(SaveRapper.SaveDataType.AutoSave, 1));
			}
			SaveRapper.Save("SaveDataSlotIndex", CurrentSaveSlot);
			GameState.GameStateData.PlayerEcstasy = UnityEngine.Mathf.Min(GameState.GameStateData.PlayerEcstasy, 0.99f);
			GameState.GameStateData.PlayerCurrentClothes = InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CurrentClothes.ClothesType;
			SaveRapper.Save("GameStateData", GameState.GameStateData);
			SaveRapper.Save("IsTrial", GameState.IsTrial);
			SaveRapper.Save("PlayerCustomData", GameState.PlayerCustomData);
			SaveRapper.AfterProcess();
		}

		public static void Load(SaveRapper.SaveDataType type, int slot = -1)
		{
			// Recovered from GameAssembly.dll RVA 0x49D380
			LoadSystem();
			if (type == SaveRapper.SaveDataType.System) return;
			SaveRapper.PreProcess(false, slot, false);
			GameState.PlayerCustomData = SaveRapper.Load("PlayerCustomData", new PlayerCustomData());
			if (GameState.PlayerCustomData.CustomizeHeight == -999)
			{
			    GameStateFacade.SetDefaultCustomFace();
			    GameStateFacade.SetDefaultCustomBody();
			}
			GameState.GameStateData = SaveRapper.Load("GameStateData", new GameStateData());
			GameState.SaveDataSlotIndex = SaveRapper.Load("SaveDataSlotIndex", SaveRapper.CurrentSlot);
			if (GameState.GameStateData.CosplayReinforceLockState == null)
			    GameState.GameStateData.CosplayReinforceLockState = new System.Collections.Generic.Dictionary<int, bool[]>();
			if (GameState.GameStateData.AdultGoodsReinforceLockState == null)
			    GameState.GameStateData.AdultGoodsReinforceLockState = new System.Collections.Generic.Dictionary<global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType, bool[]>();
			foreach (var item in global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.GetAll())
			    GameState.GameStateData.AdultGoodsReinforceLockState.TryAdd(item.Key, new bool[4]);
			foreach (var item in global::ExposureUnnoticed2.Master.Cosplay.MCosplay.GetAll())
			    foreach (var part in item.Value.GetAll())
			    {
			        GameState.GameStateData.CosplayPartsReinforceEffectDict.TryAdd(part.UniqueId, new int[3]);
			        GameState.GameStateData.CosplayReinforceLockState.TryAdd(part.UniqueId, new bool[4]);
			    }
			foreach (var item in global::ExposureUnnoticed2.Master.Hair.MHair.GetAll())
			{
			    var record = item.Value;
			    if (GameState.PlayerCustomData.HairCustomizeDataDict.ContainsKey(record.Type)) continue;
			    GameState.PlayerCustomData.HairCustomizeDataDict.Add(record.Type, new global::ExposureUnnoticed2.Master.Hair.MHairRecord.HairCustomizeData(record.Type));
			    GameState.PlayerCustomData.HairCustomizeDataDict[record.Type].TextureId = record.DefaultValue.TextureId;
			    GameState.PlayerCustomData.HairCustomizeDataDict[record.Type].R = record.DefaultValue.R;
			    GameState.PlayerCustomData.HairCustomizeDataDict[record.Type].G = record.DefaultValue.G;
			    GameState.PlayerCustomData.HairCustomizeDataDict[record.Type].B = record.DefaultValue.B;
			}
			foreach (var stage in global::ExposureUnnoticed2.Master.Mission.MMission.GetAll())
			    foreach (var mission in stage.Value.GetAll())
			        GameState.GameStateData.MissionMaxRpBonus100Dict.TryAdd(mission.Value.Id, 0);
			foreach (var item in global::ExposureUnnoticed2.Master.Skill.MSkill.GetAll())
			    GameState.GameStateData.SkillLevel.TryAdd(item.Key, 0);
			// Original comparison is <= 20, producing 21 entries; retain it.
			while (GameState.GameStateData.LoadoutDatas.Count <= 20)
			    GameState.GameStateData.LoadoutDatas.Add(new global::ExposureUnnoticed2.ObjectUI.InventoryPanel.LoadoutData());
			while (GameState.GameStateData.LoadoutCustomizeDatas.Count <= 20)
			{
			    var data = new global::ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu.LoadoutCustomizeData();
			    data.SaveCurrentCustomize();
			    GameState.GameStateData.LoadoutCustomizeDatas.Add(data);
			}
			foreach (var stage in global::ExposureUnnoticed2.Master.Stage.MStage.GetAll())
			    GameState.GameStateData.RandomMissionClearNumIdDict.TryAdd(stage.Key, -1);
			GameState.IsTrial = SystemManager.IsTrial;
			GameState.OptionData.MissionPanel = true;
			if (GameState.GameStateData.MAdultGoodsDict.ContainsKey((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)4)
			    && !GameStateFacade.IsBringItem((global::ExposureUnnoticed2.Master.Item.ItemType)10000))
			    GameStateFacade.ChangeItemNum((global::ExposureUnnoticed2.Master.Item.ItemType)10000, 1);
			if ((GameState.GameStateData.MAdultGoodsDict.ContainsKey((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)1)
			     || GameState.GameStateData.MAdultGoodsDict.ContainsKey((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)6)
			     || GameState.GameStateData.MAdultGoodsDict.ContainsKey((global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoodsType)7))
			    && !GameStateFacade.IsBringItem((global::ExposureUnnoticed2.Master.Item.ItemType)10001))
			    GameStateFacade.ChangeItemNum((global::ExposureUnnoticed2.Master.Item.ItemType)10001, 1);
			GameState.GameStateData.CurrentStageType = (global::ExposureUnnoticed2.Master.Stage.StageType)0;
			GameState.GameStateData.ActiveDroneMissionId = -1;
			GameState.GameStateData.ActiveDroneProgress = 0f;
			if (GameState.GameStateData.CosplayMySetNames == null)
			{
			    var names = new System.Collections.Generic.List<string>();
			    for (int i = 1; i <= 20; ++i) names.Add(Localizer.Trans("ui_loadout_name") + i);
			    GameState.GameStateData.CosplayMySetNames = names.ToArray();
			}
			if (GameState.GameStateData.CustomizeMySetNames == null)
			{
			    var names = new System.Collections.Generic.List<string>();
			    for (int i = 1; i <= 20; ++i) names.Add(Localizer.Trans("ui_loadout_name") + i);
			    GameState.GameStateData.CustomizeMySetNames = names.ToArray();
			}
		}

		public static int LoadSaveDataSlotIndex(int slot)
		{
			return 0;
		}

		public static bool LoadSaveDataIsTrial(int slot)
		{
			return false;
		}
	}
}
