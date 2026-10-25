using System.Collections.Generic;
using ExposureUnnoticed2.Master.Accessory;
using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Master.BodyPaint;
using ExposureUnnoticed2.Master.Clothes;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Master.Costume;
using ExposureUnnoticed2.Master.DroneMission;
using ExposureUnnoticed2.Master.Hair;
using ExposureUnnoticed2.Master.Item;
using ExposureUnnoticed2.Master.RankRelease;
using ExposureUnnoticed2.Master.ReinforceEffect;
using ExposureUnnoticed2.Master.Skill;
using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu;
using ExposureUnnoticed2.ObjectUI.InventoryPanel;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class GameStateFacade : MonoBehaviour
	{
		public static bool IsActivePassiveSkill(SkillType type)
		{
			// Recovered from GameAssembly.dll RVA 0x48DA50
			if (GetSkillLevel(type) < 1) return false;
			return GameState.GameStateData.PlayerPassiveSkillActiveDict[type];
		}

		public static void ChangeActivePassiveSkill(SkillType type, bool isActive)
		{
            // Original RVA 0x48DAE0.
            GameState.GameStateData.PlayerPassiveSkillActiveDict.TryAdd(type, false);
            GameState.GameStateData.PlayerPassiveSkillActiveDict[type] = isActive;
            if (GameState.GameStateData.PlayerPassiveSkillActiveDict.ContainsKey(SkillType.FixFps)
                && GameState.GameStateData.PlayerPassiveSkillActiveDict[SkillType.FixFps]
                && type == SkillType.FixTps && isActive)
                GameState.GameStateData.PlayerPassiveSkillActiveDict[SkillType.FixFps] = false;
            if (GameState.GameStateData.PlayerPassiveSkillActiveDict.ContainsKey(SkillType.FixTps)
                && GameState.GameStateData.PlayerPassiveSkillActiveDict[SkillType.FixTps]
                && type == SkillType.FixFps && isActive)
                GameState.GameStateData.PlayerPassiveSkillActiveDict[SkillType.FixTps] = false;
		}

		public static float GetCameraMoveX()
		{
			// Draft from complete original 48DE10..48DF89. Preserve axis/base/scale capture and float association.
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.InputManager).TypeHandle);
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.InputManager).TypeHandle);
			bool isGamePad = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad;
			if (!isGamePad)
			{
			    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.InputManager).TypeHandle);
			    float axis = global::ExposureUnnoticed2.Scripts.Base.InputManager.GetVector2(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.CameraMove).x;
			    float capturedBase = global::ExposureUnnoticed2.Master.Common.MCommon.BaseCameraSensitive;
			    float optionScale = GetOptionMouseXScale();
			    float baseAndAxis = capturedBase * axis;
			    return optionScale * baseAndAxis;
			}
			else
			{
			    System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.InputManager).TypeHandle);
			    float axis = global::ExposureUnnoticed2.Scripts.Base.InputManager.GetVector2(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.CameraMove).x;
			    var capturedOption = GameState.OptionData;
			    if (capturedOption is null) throw new System.NullReferenceException();
			    bool inverse = capturedOption.PadInverseX;
			    float scaledAxis = axis * 0.15f;
			    var capturedSensitivityOption = GameState.OptionData;
			    float signedScaledAxis = (float)(inverse ? -1 : 1) * scaledAxis;
			    return signedScaledAxis * (float)capturedSensitivityOption.PadSensitive100;
			}
		}

		public static float GetCameraMoveY()
        {
            global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.InputManager).TypeHandle);
            global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.InputManager).TypeHandle);
            bool pad = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad;
            global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.InputManager).TypeHandle);
            float axis = global::ExposureUnnoticed2.Scripts.Base.InputManager.GetVector2(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.CameraMove).y;
            if (!pad)
            {
                float capturedBase = global::ExposureUnnoticed2.Master.Common.MCommon.BaseCameraSensitive;
                float scale = GetOptionMouseYScale();
                float baseAndAxis = axis * capturedBase;
                return scale * baseAndAxis;
            }
            var options = GameState.OptionData;
            if (options is null) throw new global::System.NullReferenceException();
            bool inverse = options.PadInverseY;
            float scaledAxis = axis * 0.15f;
            float scaled = (float)(inverse ? -1 : 1) * scaledAxis;
            var sensitivityOptions = GameState.OptionData;
            return scaled * (float)sensitivityOptions.PadSensitive100;
        }

		public static int GetSkillLevel(SkillType type)
		{
			// Recovered from GameAssembly.dll RVA 0x48E170
			return GameState.GameStateData.SkillLevel[type];
		}

		public static void RiseSkillLevel(SkillType type)
		{
            // Original RVA 0x48E1F0.
            var levels = GameState.GameStateData.SkillLevel;
            levels[type] = unchecked(levels[type] + 1);
            if (MSkill.Get(type).IsPassiveToggle && !GameState.GameStateData.PlayerPassiveSkillActiveDict.ContainsKey(type))
                ChangeActivePassiveSkill(type, true);
		}

		public static void SetReinforceEffect(MAdultGoodsType type, RReinforceEffect[] effects, bool[] settings)
		{
		}

		public static void SetReinforceEffect(CostumeType type, RReinforceEffect[] effects)
		{
		}

		public static void SetReinforceEffect(int uniqueId, RReinforceEffect[] effects, bool[] settings)
		{
		}

		public static bool IsEnoughRp(int necessaryRp)
		{
            // Recovered complete original transaction primitive.
            return (double)necessaryRp <= GameState.GameStateData.CurrentRp;
        }

		public static void ConsumeRp(int rp)
		{
            // Recovered complete original transaction primitive.
            GameState.GameStateData.CurrentRp -= (double)rp;
            GameStateData data = GameState.GameStateData;
            data.StatistisTotalUseRp = (double)rp + data.StatistisTotalUseRp;
        }

		public static void RiseEarnRp(float rp)
		{
            var data = GameState.GameStateData;
            if (data is null) throw new System.NullReferenceException();
            data.CurrentEarnThisTimeRp = (double)rp + data.CurrentEarnThisTimeRp;
            data = GameState.GameStateData;
            if (data is null) throw new System.NullReferenceException();
            data.StatistisTotalEarnRp = (double)rp + data.StatistisTotalEarnRp;
        }

		public static void RiseRp(float rp)
		{
            var data = GameState.GameStateData;
            if (data is null) throw new System.NullReferenceException();
            data.CurrentRp = (double)rp + data.CurrentRp;
            data = GameState.GameStateData;
            if (data is null) throw new System.NullReferenceException();
            data.CurrentEarnThisTimeRp = (double)rp + data.CurrentEarnThisTimeRp;
            data = GameState.GameStateData;
            if (data is null) throw new System.NullReferenceException();
            data.StatistisTotalEarnRp = (double)rp + data.StatistisTotalEarnRp;
            if (GameState.GameStateData is null) throw new System.NullReferenceException();
        }

		public static void RiseRpDirectly(float rp)
		{
            var data = GameState.GameStateData;
            if (data is null) throw new System.NullReferenceException();
            data.CurrentRp = (double)rp + data.CurrentRp;
            data = GameState.GameStateData;
            if (data is null) throw new System.NullReferenceException();
            data.StatistisTotalEarnRp = (double)rp + data.StatistisTotalEarnRp;
        }

		public static void AddClothes(MClothesRecord record)
		{
            // Recovered complete original transaction primitive.
            GameState.GameStateData.ClothesDict.Add(record.ClothesType, true);
        }

		public static void AddClothes(ClothesType type)
		{
            // Recovered complete original transaction primitive.
            GameState.GameStateData.ClothesDict.Add(type, true);
        }

		public static void AddAdultGoods(MAdultGoodsType type)
		{
            // Recovered complete original transaction primitive.
            GameState.GameStateData.MAdultGoodsDict.Add(type, false);
        }

		public static bool IsBringBodyPaint(BodyPaintType type)
		{
            return GameState.PlayerCustomData.BodyPaintTypeDict.ContainsKey(type);
		}

		public static bool IsEquipBodyPaint(BodyPaintType type)
		{
			// Recovered from GameAssembly.dll RVA 0x48EF80
			if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.BodyPaintTypeDict.ContainsKey(type)) return false;
			return global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.BodyPaintTypeDict[type];
		}

		public static void SetActiveBodyPaint(BodyPaintType type, bool isActive)
		{
            // Recovered complete original native body; presentation callees remain original calls.
            GameState.PlayerCustomData.BodyPaintTypeDict[type] = isActive;
        }

		public static bool IsBringClothes(MClothesRecord record)
		{
            return GameState.GameStateData.ClothesDict.ContainsKey(record.ClothesType);
		}

		public static bool IsBringAdultGoods(MAdultGoodsRecord record)
		{
			return false;
		}

		public static bool IsBringAdultGoods(MAdultGoodsType type)
		{
            // Recovered complete original native body.
            return GameState.GameStateData.MAdultGoodsDict.ContainsKey(type);
        }

		public static bool IsEquipAdultGoods(MAdultGoodsType type)
		{
			// Recovered from GameAssembly.dll RVA 0x48F2E0
			if (!GameState.GameStateData.MAdultGoodsDict.ContainsKey(type)) return false;
			return GameState.GameStateData.MAdultGoodsDict[type];
		}

		public static void UpdateAdultGoodsState(MAdultGoodsType type, bool isActive)
		{
            // Recovered complete original native body.
            GameState.GameStateData.MAdultGoodsDict[type] = isActive;
        }

		public static bool IsEquipCostume(RCostume record)
		{
            // Recovered complete original native body.
            if (!GameState.GameStateData.PlayerCostumeDict.ContainsKey(record.Type)) return false;
            return GameState.GameStateData.PlayerCostumeDict[record.Type];
        }

		public static void UpdateCostumeState(RCostume record, bool isActive)
		{
            // Recovered complete original native body.
            GameState.GameStateData.PlayerCostumeDict[record.Type] = isActive;
        }

		public static bool IsBringCostume(CostumeType type)
		{
			return false;
		}

		public static void AddCostume(CostumeType type)
		{
		}

		public static Dictionary<SkillType, int> GetDefaultSkillDict()
		{
			// Recovered from GameAssembly.dll RVA 0x48F730
			var result = new Dictionary<SkillType, int>();
			foreach (SkillType type in System.Enum.GetValues(typeof(SkillType))) result.Add(type, 0);
			return result;
		}

		public static Dictionary<DroppedItemManager.DropItemType, DroppedItemManager.DropData> GetDefaultDropDataDict()
		{
			// Recovered from GameAssembly.dll RVA 0x48FB20
			var result = new Dictionary<DroppedItemManager.DropItemType, DroppedItemManager.DropData>();
			foreach (DroppedItemManager.DropItemType type in System.Enum.GetValues(typeof(DroppedItemManager.DropItemType)))
			{
			    result.Add(type, new DroppedItemManager.DropData(type));
			    if ((int)type > 9999)
			        for (int i = 1; i < 10; ++i)
			        {
			            var additional = (DroppedItemManager.DropItemType)((int)type + i);
			            result.Add(additional, new DroppedItemManager.DropData(additional));
			        }
			}
			return result;
		}

		public static Dictionary<HairType, MHairRecord.HairCustomizeData> GetDefaultHairCustomDataDict()
		{
			// Recovered from GameAssembly.dll RVA 0x48FF20
			var result = new Dictionary<HairType, MHairRecord.HairCustomizeData>();
			foreach (var item in MHair.GetAll())
			{
			    MHairRecord record = item.Value;
			    result.Add(record.Type, new MHairRecord.HairCustomizeData(record.Type));
			    result[record.Type].TextureId = record.DefaultValue.TextureId;
			    result[record.Type].R = record.DefaultValue.R;
			    result[record.Type].G = record.DefaultValue.G;
			    result[record.Type].B = record.DefaultValue.B;
			    if ((int)record.Type == 0)
			    {
			        result[record.Type].TextureId = 4;
			        result[record.Type].R = 159;
			        result[record.Type].G = 131;
			        result[record.Type].B = 113;
			    }
			}
			return result;
		}

		public static Dictionary<ClothesType, bool> GetDefaultClothesDict()
		{
			// Recovered from GameAssembly.dll RVA 0x490360
			return new Dictionary<ClothesType, bool> { { (ClothesType)0, true } };
		}

		public static Dictionary<CostumeType, bool> GetDefaultCostumeDict()
		{
			// Recovered from GameAssembly.dll RVA 0x490410
			return new Dictionary<CostumeType, bool>();
		}

		public static Dictionary<int, bool> CreateDefaultCosplayPartsDict()
		{
			// Recovered from GameAssembly.dll RVA 0x490470
			return new Dictionary<int, bool> { { 20011, true } };
		}

		public static CosplayPartsOptionData GetCosplayPartsOptionData(int uniqueId)
		{
			// Recovered from GameAssembly.dll RVA 0x490520
			if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CosplayPartsOptionDataDict.ContainsKey(uniqueId))
			{
			    var option = new global::ExposureUnnoticed2.Master.Cosplay.CosplayPartsOptionData(uniqueId);
			    var part = global::ExposureUnnoticed2.Master.Cosplay.MCosplay.Get((global::ExposureUnnoticed2.Master.Cosplay.CosplayType)(uniqueId / 10000)).Get(uniqueId % 10000);
			    option.r = part.DefaultColorR;
			    option.g = part.DefaultColorG;
			    option.b = part.DefaultColorB;
			    option.shadeColor = part.DefaultShadeColor;
			    option.materialIndex = part.DefaultMaterialIndex;
			    option.boolOption1 = part.DefaultChangeBoolOption1;
			    option.boolOption2 = part.DefaultChangeBoolOption2;
			    option.invisible = false;
			    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CosplayPartsOptionDataDict.Add(uniqueId, option);
			}
			return global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CosplayPartsOptionDataDict[uniqueId];
		}

		public static bool IsEquipCosplayParts(int uniqueId)
		{
			// Recovered from GameAssembly.dll RVA 0x490740
			if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict.ContainsKey(uniqueId)) return false;
			return global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict[uniqueId];
		}

		public static bool IsBringCosplayParts(int uniqueId)
		{
			return GameState.GameStateData.PlayerCosplayPartsDict.ContainsKey(uniqueId);
		}

		public static void AddCosplayParts(int uniqueId)
		{
		}

		public static void SetActiveCosplayParts(int uniqueId, bool isActive)
		{
			// Recovered from GameAssembly.dll RVA 0x490940
			global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict[uniqueId] = isActive;
		}

		public static int GetDroneMissionAchieveCount(RDroneMission record)
		{
            if (!GameState.GameStateData.DroneMissionAchieveCountDict.ContainsKey(record.Id))
                GameState.GameStateData.DroneMissionAchieveCountDict.Add(record.Id, 0);
            return GameState.GameStateData.DroneMissionAchieveCountDict[record.Id];
        }

		public static int GetDroneMissionAchieveCount(RDroneMission record, StageType stageType)
		{
			return 0;
		}

		public static int GetDroneMissionMaxRpBonus(RDroneMission record, StageType stageType)
		{
			return 0;
		}

		public static void AddOneDroneMissionAchieveCount(RDroneMission record)
		{
		}

		public static void ChangeItemNum(ItemType type, int changeAmount)
		{
			// Recovered from GameAssembly.dll RVA 0x491360
			if (!GameState.GameStateData.ItemDict.ContainsKey(type)) GameState.GameStateData.ItemDict.Add(type, 0);
			GameState.GameStateData.ItemDict[type] += changeAmount;
			MItemRecord record = MItem.Get(type);
			if (record.IsValuables && changeAmount > 0) GameState.GameStateData.ItemDict[type] = 1;
			// Native branch tests the managed instance pointer, with no Unity Object null operator.
			InventoryPanelView.Instance?.RefreshItemCategory();
		}

		public static bool IsBringItem(ItemType type)
		{
			// Recovered from GameAssembly.dll RVA 0x4915A0
			return GameState.GameStateData.ItemDict.ContainsKey(type) && GameState.GameStateData.ItemDict[type] > 0;
		}

		public static List<ClothesType> GetPossessionClothes()
		{
// Complete original native body draft; native/CLR acceptance pending.
var result = new List<ClothesType>(); foreach (var pair in GameState.GameStateData.ClothesDict) if (pair.Value) result.Add(pair.Key); return result;
}

		public static bool IsEquipped(IEquipableMaster record)
		{
            // Recovered complete original native body.
            if (record is MAdultGoodsRecord accessory) return IsEquipAdultGoods(accessory.Type);
            if (record is RCostume costume) return IsEquipCostume(costume);
            if (record is RCosplayParts cosplay) return IsEquipCosplayParts(cosplay.UniqueId);
            return false;
        }

		public static float GetOptionMouseXScale()
		{
			// Draft from complete original 4919C0..491A26. NEG is unchecked Int32; false branch rereads the field.
			var capturedOption = GameState.OptionData;
			int signedSensitivity = unchecked(-capturedOption.MouseSensitive100);
			if (!capturedOption.MouseInverseX) signedSensitivity = capturedOption.MouseSensitive100;
			return (float)signedSensitivity * 0.01f;
		}

		public static float GetOptionMouseYScale()
        {
            var options = GameState.OptionData;
            int sensitivity = unchecked(-options.MouseSensitive100);
            if (!options.MouseInverseY) sensitivity = options.MouseSensitive100;
            return (float)sensitivity * 0.01f;
        }

		public static float GetOptionWheelScale()
		{
            var options = GameState.OptionData;
            int sensitivity = unchecked(-options.WheelSensitive100);
            if (!options.WheelInverse) sensitivity = options.WheelSensitive100;
            return (float)sensitivity * 0.1f;
		}

		public static int GetMissionAchieveCount(int UniqueMissionId)
		{
            if (!GameState.GameStateData.MissionAchieveCountDict.ContainsKey(UniqueMissionId)) return 0;
            return GameState.GameStateData.MissionAchieveCountDict[UniqueMissionId];
        }

		public static void AchieveMission(int UniqueMissionId)
		{
            if (!GameState.GameStateData.MissionAchieveCountDict.ContainsKey(UniqueMissionId))
                GameState.GameStateData.MissionAchieveCountDict.Add(UniqueMissionId, 0);
            var dictionary = GameState.GameStateData.MissionAchieveCountDict;
            dictionary[UniqueMissionId] = unchecked(dictionary[UniqueMissionId] + 1);
        }

		public static void AchieveContinueMission(int UniqueMissionId)
		{
            // The original 0x491D70 also targets MissionAchieveCountDict (native +0xF0).
            if (!GameState.GameStateData.MissionAchieveCountDict.ContainsKey(UniqueMissionId))
                GameState.GameStateData.MissionAchieveCountDict.Add(UniqueMissionId, 0);
            var dictionary = GameState.GameStateData.MissionAchieveCountDict;
            dictionary[UniqueMissionId] = unchecked(dictionary[UniqueMissionId] + 1);
        }

		public static float GetMissionSatisfy(int UniqueMissionId)
		{
            if (!GameState.GameStateData.MissionSatisfyDict.ContainsKey(UniqueMissionId))
                GameState.GameStateData.MissionSatisfyDict.Add(UniqueMissionId, 0f);
            return GameState.GameStateData.MissionSatisfyDict[UniqueMissionId];
        }

		public static float GetMissionEarnRp(int UniqueMissionId)
		{
            if (!GameState.GameStateData.MissionEarnRpDict.ContainsKey(UniqueMissionId))
                GameState.GameStateData.MissionEarnRpDict.Add(UniqueMissionId, 0f);
            return GameState.GameStateData.MissionEarnRpDict[UniqueMissionId];
        }

		public static bool IsAttachSomeHandcuffs()
		{
            // Recovered complete original native body.
            return GameState.GameStateData.PlayerIsAttachHandcuffs || GameState.GameStateData.PlayerIsAttachMapHandcuffs;
        }

		public static bool IsRelease(RankReleaseType type)
		{
            if ((int)type == -1) return true;
            var release = global::ExposureUnnoticed2.Master.RankRelease.MRankRelease.Get(type);
            var rank = global::ExposureUnnoticed2.Master.Rank.MRank.GetByRank(release.ReleaseRank);
            return rank.NeedRp <= global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ExposeExp;
        }

		public static void SetDefaultCustomBody()
		{
			// Recovered from GameAssembly.dll RVA 0x492250
			GameState.PlayerCustomData.CustomizeHeight = 160;
			GameState.PlayerCustomData.CustomizeHead = 50;
			GameState.PlayerCustomData.CustomizeNeck = 50;
			GameState.PlayerCustomData.CustomizeNeckThick = 50;
			GameState.PlayerCustomData.CustomizeBoobs = 50;
			GameState.PlayerCustomData.CustomizeSuperBoobs = 0;
			GameState.PlayerCustomData.CustomizeHyperBoobs = 0;
			GameState.PlayerCustomData.CustomizeNipple = 50;
			GameState.PlayerCustomData.CustomizeButt = 50;
			GameState.PlayerCustomData.CustomizeWaist = 0;
			GameState.PlayerCustomData.CustomizeLegInterval = 50;
			GameState.PlayerCustomData.CustomizeThighs = 50;
			GameState.PlayerCustomData.CustomizeThighsLength = 50;
			GameState.PlayerCustomData.CustomizeLowerLeg = 50;
			GameState.PlayerCustomData.CustomizeLowerLegLength = 50;
			GameState.PlayerCustomData.CustomizeUpperBody = 50;
			GameState.PlayerCustomData.CustomizeUpperBody2 = 50;
			GameState.PlayerCustomData.CustomizeUpperBodyLength = 50;
			GameState.PlayerCustomData.CustomizeUpperBodyLength2 = 50;
			GameState.PlayerCustomData.CustomizeArm = 50;
			GameState.PlayerCustomData.CustomizeShoulder = 50;
			GameState.PlayerCustomData.CustomizeInmou1 = 0;
			GameState.PlayerCustomData.CustomizeInmou2 = 50;
			GameState.PlayerCustomData.CustomizeInmouColorR = 0;
			GameState.PlayerCustomData.CustomizeInmouColorG = 0;
			GameState.PlayerCustomData.CustomizeInmouColorB = 0;
			GameState.PlayerCustomData.CustomizeSkinColor = 0;
			GameState.PlayerCustomData.CustomizeSkinColorR = 255;
			GameState.PlayerCustomData.CustomizeSkinColorG = 255;
			GameState.PlayerCustomData.CustomizeSkinColorB = 255;
			GameState.PlayerCustomData.CustomIsSkinColorFree = false;
			GameState.PlayerCustomData.CustomizeFutanariSaoSize = 0;
			GameState.PlayerCustomData.CustomizeFutanariTamaSize = 50;
			GameState.PlayerCustomData.CustomizeFutanariStraight = 0;
			GameState.PlayerCustomData.CustomizeFutanariKitouSize = 0;
			GameState.PlayerCustomData.CustomizeFutanariWidth = 0;
			GameState.PlayerCustomData.CustomizeFutanariTamaVisible = true;
		}

		public static void SetDefaultCustomFace()
		{
			// Recovered from GameAssembly.dll RVA 0x492740
			GameState.PlayerCustomData.CustomizeLoliFace = 0;
			GameState.PlayerCustomData.CustomizeEyeOpenness = 0;
			GameState.PlayerCustomData.CustomizeEyeSize = 30;
			GameState.PlayerCustomData.CustomizeHitomiSize = 50;
			GameState.PlayerCustomData.CustomizeTareme = 50;
			GameState.PlayerCustomData.CustomizeEyeDist = 50;
			GameState.PlayerCustomData.CustomizeEyeHeight = 100;
			GameState.PlayerCustomData.CustomizeEyeDark = false;
			GameState.PlayerCustomData.CustomizeOddEye = false;
			GameState.PlayerCustomData.CustomizeMatsugeThick = 50;
			GameState.PlayerCustomData.CustomizeMatsugeLength = 50;
			GameState.PlayerCustomData.CustomizeMayuThick = 50;
			GameState.PlayerCustomData.CustomizeMayuDog = 0;
			GameState.PlayerCustomData.CustomizeCheekFat = 0;
			GameState.PlayerCustomData.CustomizeEarElf = 0;
			GameState.PlayerCustomData.CustomizeChinLength = 50;
			GameState.PlayerCustomData.CustomizeEyeLeftPattern = 23;
			GameState.PlayerCustomData.CustomizeEyePattern = 23;
			GameState.PlayerCustomData.CustomizeEyeBlowColorR = 183;
			GameState.PlayerCustomData.CustomizeEyeBlowColorG = 163;
			GameState.PlayerCustomData.CustomizeEyeBlowColorB = 163;
			GameState.PlayerCustomData.CustomizeEyeLashColorR = 183;
			GameState.PlayerCustomData.CustomizeEyeLashColorG = 163;
			GameState.PlayerCustomData.CustomizeEyeLashColorB = 163;
			GameState.PlayerCustomData.CustomizeYaeba = false;
		}

		public static Dictionary<MAdultGoodsType, int[]> CreateDefaultReinforceEffectAdultGoods()
		{
			// Recovered from GameAssembly.dll RVA 0x492AD0
			var result = new Dictionary<MAdultGoodsType, int[]>();
			foreach (MAdultGoodsType type in System.Enum.GetValues(typeof(MAdultGoodsType))) result.Add(type, new int[3]);
			return result;
		}

		public static Dictionary<int, int[]> CreateDefaultReinforceEffectCosplayParts()
		{
			// Recovered from GameAssembly.dll RVA 0x492EE0
			var result = new Dictionary<int, int[]>();
			foreach (var item in MCosplay.GetAll())
			    foreach (RCosplayParts part in item.Value.GetAll()) result.Add(part.UniqueId, new int[3]);
			return result;
		}

		public static Dictionary<CostumeType, int[]> CreateDefaultReinforceEffectCostume()
		{
			// Recovered from GameAssembly.dll RVA 0x4931D0
			var result = new Dictionary<CostumeType, int[]>();
			foreach (CostumeType type in System.Enum.GetValues(typeof(CostumeType))) result.Add(type, new int[3]);
			return result;
		}

		public static List<LoadoutData> CreateDefaultLoadoutData()
		{
			// Recovered from GameAssembly.dll RVA 0x4935E0
			var result = new List<LoadoutData>();
			for (int i = 0; i < 20; ++i) result.Add(new LoadoutData());
			return result;
		}

		public static List<LoadoutCustomizeData> CreateDefaultLoadoutCustomizeData()
		{
			// Recovered from GameAssembly.dll RVA 0x4936F0
			var result = new List<LoadoutCustomizeData>();
			for (int i = 0; i < 20; ++i)
			{
			    var data = new LoadoutCustomizeData();
			    data.SaveCurrentCustomize();
			    result.Add(data);
			}
			return result;
		}

		public static List<HairType> TransHairType(List<int> before)
		{
			// Recovered from GameAssembly.dll RVA 0x493800
			var result = new List<HairType>();
			foreach (int value in before) result.Add((HairType)value);
			return result;
		}

		public static List<int> TransInt(List<HairType> before)
		{
			return null;
		}

		public static string GetRankNameAutoHide(int rank)
		{
			// Recovered from GameAssembly.dll RVA 0x493D20. Reveal only the next rank's name.
			int currentRank = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentRank.Rank;
			if (unchecked(currentRank + 1) < rank) return "???";
			return global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(
				global::ExposureUnnoticed2.Master.Rank.MRank.GetByRank(rank).NameKey);
		}
	}
}
