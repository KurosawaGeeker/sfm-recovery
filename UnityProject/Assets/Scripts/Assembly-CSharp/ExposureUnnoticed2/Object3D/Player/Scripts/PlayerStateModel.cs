using System;
using System.Collections.Generic;
using ExposureUnnoticed2.Master.Accessory;
using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Master.Clothes;
using ExposureUnnoticed2.Master.Costume;
using ExposureUnnoticed2.Master.Item;
using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Master.Rank;
using ExposureUnnoticed2.Object3D.Light;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using ExposureUnnoticed2.Object3D.RoutePoint;
using ExposureUnnoticed2.Object3D.ScenePlops.ChairSitController;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using HadakaCoat.Objects3D.Dildo;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerStateModel
	{
		public enum ClothesState
		{
			None = -1,
			CloseA = 0,
			OpenA1 = 1,
			OpenA2 = 2,
			OpenA3 = 3,
			DropClothes = 5,
			CloseB = 10,
			OpenB1 = 11
		}

		public enum CoatStateMove
		{
			None = -1,
			CloseBtoOpenB1 = 0,
			OpenB1toCloseB = 1
		}

		public enum CoatActionType
		{
			TypeA = 0,
			TypeB = 1
		}

		public enum PlayerState
		{
			Idle = 0,
			Bareta = 1,
			BaretaEscape = 2
		}

		public enum HadObjectType
		{
			None = -1,
			Basket = 0,
			HoldInHandClothes = 1
		}

		public enum SexualityType
		{
			Normal = 0,
			Flasher = 1,
			Raper = 2
		}

		[Flags]
		public enum HiddenBodyPartsByCostumeType
		{
			Boobs = 1,
			Hip = 2,
			Genitals = 4,
			SideOrBackUpperBody = 8,
			HipCrouch = 0x10
		}

		public MClothesRecord CurrentClothes;

		public bool IsArmAction;

		public bool IsDash;

		public bool IsStrafe;

		public bool IsCrouch;

		public bool IsPeeing;

		public bool IsOffHipDBFromCosplay;

		public NpcController NearNpc;

		public NpcController NearNpcForMission;

		public NpcController ChaseTargetNpc;

		public MAdultGoodsRecord EquippingAdultGoods;

		public RCostume EquippingCostume;

		public MActionRecord CurrentAction;

		public bool IsEcstasyMotion;

		public float EcstasyRemainAftermathTime;

		public float Dokidoki;

		public PlayerState CurrentState;

		public Transform SmoothTransformTarget;

		public Quaternion? SmoothRotateTarget;

		public float SmoothRemainTime;

		public float CompleteAttachHandcuffsTime;

		public float NearNpcDist;

		public bool IsNearNpcForMission;

		public bool IsNearNpcForMissionSecondary;

		public bool IsLookedHip;

		public bool IsLookedUpper;

		public bool IsLookedFlashing;

		public float StrangeBonus;

		public float Bareyasusa;

		public float BaseBareyasusa;

		public float BareyasusaFromStreetLight;

		public float AddBareyasusaByCosplay01;

		public float AddBareyasusaByAdultGoods;

		public float DokidokiBonusByAdultGoods;

		public SumReinforceEffectData ReinforceEffectData;

		public float RpBonus;

		public float RpBonusInt;

		public int AdultGoodsRpBonus100;

		public int CosplayRpBonus100;

		public float EcstasyRiseByVibratorPerSecond;

		public float EcstasyRiseByPistonMachinePerSecond;

		public int RpBonusByVibratorPerSecond;

		public int RpBonusByPistonMachinePerSecond;

		public float MiscRpBonus;

		public float VibratorRpBonusScale;

		public float PistonRpBonusScale;

		public IShoesRecord CurrentShoesRecord;

		public bool IsGamanBaibu;

		public float InvalidLocomotionTime;

		public MRankRecord CurrentRank;

		public float AddEcstasy;

		public List<StreetLightController> TouchedLightList;

		public float BareyasusaByHeel;

		public List<BuffModel> AdultGoodsBuffList;

		public List<BuffModel> CosplayBuffList;

		public List<BuffModel> CostumeBuffList;

		public Dictionary<BuffSourceType, BuffModel> OtherBuffDict;

		public HiddenBodyPartsByCostumeType HiddenBodyPartsByCostume;

		public float MaxLookHiddenPartMaxStrange;

		public Action OnEndCommonEquip;

		public float CommonEquipEndTime;

		public InteractModel LastInteractModel;

		public DildoController LastPickupDildoWall;

		public Vector3 MoveV;

		public BaseInteractTokenPublisher LastInteractTokenPublisher;

		public ChairSitController LastChairSitController;

		public bool IsAutoChase;

		public bool IsAutoRun;

		public bool IsHideFutaByCosplay;

		public float VibeBikunMotionRemainTime;

		public bool IsHatByCosplay;

		public bool IsFixedPositionRotation;

		public Vector3 FixedPosition;

		public Quaternion FixedRotation;

		public bool IsFixedWarpPositionRotation;

		public Vector3 FixedWarpPosition;

		public Quaternion FixedWarpRotation;

		public List<NpcController> timeStopStrangeMaxNpcList;

		public float TimeStopStrangeMaxNpcListClearRemainTime;

		public float PoseActionCoolTime;

		public float ForHandcuffEcstasyRemainCoolTime;

		public ClothesState CurrentClothesBStateCache;

		public HadObjectType LastHadObjectType;

		public HadObjectType LastDropType;

		public DroppedItemManager.DropItemType LastDropItemType;

		public ClothesType LastPickClothesType;

		public MAdultGoodsType LastAttachedHandcuffType;

		public DroppedItemManager.DropItemType LastPickUpItemType;

		public ItemType LastUseItemType;

		public float AddEcstasyByPiston;

		public bool TransPick;

		public bool TransPickClothes;

		public ActionType TransAction;

		public bool ForceSkipHandcuffCheckTransAction;

		public bool SkipDropClothes;

		public bool IsSit;

		public Transform DropClothesPoint;

		public Vector3 SumFixedPushVThisFrame;

		public Vector3 EscalatorPushVThisFrame;

		public bool IsDownMoveForwardOneHand;

		public Vector3 CacheForNpcPosition;

		public Vector3 CacheForNpcForward;

		public ExposedBodyParts CacheForNpcExposedBodyParts;

		public bool IsPlayingAction => (int)CurrentAction.Type != -1;

		public bool IsPlayingSomeNonMoveAction => (int)CurrentAction.Type != -1 && !CurrentAction.CanMove;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x65F250
			CurrentClothes = MClothes.Get(GameState.GameStateData.PlayerCurrentClothes);
			CurrentRank = global::ExposureUnnoticed2.Master.Rank.MRank.CalcCurrentRank((double)GameState.GameStateData.ExposeExp);
		}

		public void ResetExternalRequest()
		{
		}

		public void Save()
		{
		}

		public void Load()
		{
		}
	// Recovered original member RVA 0x65F3B0: .ctor
		public PlayerStateModel()
        {
            CurrentClothes = MClothes.Get((ClothesType)0);
            CurrentAction = MAction.Get((ActionType)(-1));
            CompleteAttachHandcuffsTime = float.MaxValue;
            TouchedLightList = new List<StreetLightController>();
            AdultGoodsBuffList = new List<BuffModel>();
            CosplayBuffList = new List<BuffModel>();
            CostumeBuffList = new List<BuffModel>();
            OtherBuffDict = new Dictionary<BuffSourceType, BuffModel>();
            timeStopStrangeMaxNpcList = new List<NpcController>();
            CurrentClothesBStateCache = (ClothesState)10;
            LastHadObjectType = (HadObjectType)(-1);
            LastDropType = (HadObjectType)(-1);
            LastDropItemType = (DroppedItemManager.DropItemType)(-1);
            LastPickClothesType = (ClothesType)(-1);
            TransAction = (ActionType)(-1);
        }
	}
}
