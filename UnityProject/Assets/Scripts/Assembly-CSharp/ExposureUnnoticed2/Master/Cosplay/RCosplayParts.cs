using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Master.Costume;
using ExposureUnnoticed2.Master.ReinforceEffect;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using ExposureUnnoticed2.ObjectUI.InventoryPanel;
using ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel;
using ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Cosplay
{
	[CreateAssetMenu(fileName = "RCosplayParts", menuName = "ScriptableObjects/RCosplayParts")]
	public class RCosplayParts : ScriptableObject, IEquipableMaster, IOnlineShopItemRecord, IShoesRecord, IReinforcableRecord, IEffectiveEquipRecord
	{
		[SerializeField]
		public bool SlideManko;

		[SerializeField]
		public bool SlideAnal;

		[SerializeField]
		public float Discomfort;

		[SerializeField]
		public Constant.DiscomfortDirect DiscomfortDirect;

		[Header("カスタマイズオプション")]
		[SerializeField]
		public bool IsShowCustomizeButton;

		[SerializeField]
		public bool CanChangeColor;

		[SerializeField]
		public string ChangeMaterialNumText;

		[SerializeField]
		public int ChangeMaterialNum;

		[SerializeField]
		public string ChangeBoolOption1Text;

		[SerializeField]
		public string ChangeBoolOption2Text;

		[SerializeField]
		public bool CanChangeInvisible;

		[Header("カスタマイズオプションデフォルト設定")]
		[SerializeField]
		public int DefaultColorR;

		[SerializeField]
		public int DefaultColorG;

		[SerializeField]
		public int DefaultColorB;

		[SerializeField]
		public int DefaultShadeColor;

		[SerializeField]
		public int DefaultMaterialIndex;

		[SerializeField]
		public bool DefaultChangeBoolOption1;

		[SerializeField]
		public bool DefaultChangeBoolOption2;

		[field: SerializeField]
		public string NameKey { get; private set; }

		[field: SerializeField]
		public int Price { get; private set; }

		[field: SerializeField]
		public string DescriptionKey { get; set; }

		[field: SerializeField]
		public bool IsNotSell { get; set; }

		public int TypeId { get; }

		[field: SerializeField]
		public float HeelSize { get; private set; }

		[field: SerializeField]
		public bool IsStocking { get; private set; }

		[field: SerializeField]
		public bool IsEyeMaskEffect { get; private set; }

		[field: SerializeField]
		public bool IsCorset { get; private set; }

		[field: SerializeField]
		public bool IsFlattenTit { get; private set; }

		[field: SerializeField]
		public bool IsHideHair { get; private set; }

		[field: SerializeField]
		public bool IsHideInmou { get; private set; }

		[field: SerializeField]
		public bool IsHideFuta { get; private set; }

		[field: SerializeField]
		public bool IsNotHeelToe { get; private set; }

		[field: SerializeField]
		public float HeelToeShapeScale { get; private set; }

		[field: SerializeField]
		public EquipMotionType EquipMotionType { get; private set; }

		[field: SerializeField]
		public CostumeCategory CostumeCategory { get; private set; }

		[field: SerializeField]
		public CostumeEffectType EffectType { get; private set; }

		[field: SerializeField]
		public PlayerStateModel.HiddenBodyPartsByCostumeType HiddenPartsType { get; private set; }

		[field: SerializeField]
		public float LookHiddenPartMaxStrange { get; private set; }

		[field: SerializeField]
		public int MinBreastSize { get; private set; }

		[field: SerializeField]
		public ShrinkType[] ShrinkTypes { get; private set; }

		[field: SerializeField]
		public int DokidokiBonus { get; private set; }

		[field: SerializeField]
		public int StrangeBonus { get; private set; }

		[field: SerializeField]
		public int RpBonus { get; private set; }

		[field: SerializeField]
		public int ReleaseRank { get; private set; }

		[field: SerializeField]
		public bool HideCensorPussy { get; set; }

		[field: SerializeField]
		public bool HideCensorAnal { get; set; }

		[field: SerializeField]
		public bool OffHipDynamicBone { get; set; }

		[field: SerializeField]
		public bool IsHat { get; set; }

		public int[] EffectIds => null;

		public bool[] GetReinforceSettings => null;

		public int Id { get; set; }

		public CosplayType ParentCosplayType { get; set; }

		public int UniqueId => (int)ParentCosplayType * 10000 + Id;

		public void SwitchVisible(int defaultType = -1)
		{
			bool isActive = !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCosplayParts(UniqueId);
			global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.SetCosplayActive(ParentCosplayType, Id, isActive);
		}

		public void SetReinforceEffect(RReinforceEffect[] effects, bool[] settings)
		{
		}

		public void SetReinforceSettings(bool[] settings)
		{
		}

		public ActionType TransAttachActionType()
		{
			return ActionType.OldOnaniNormal;
		}

		public ActionType TransDetachActionType()
		{
			return ActionType.OldOnaniNormal;
		}
	}
}
