using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Master.ReinforceEffect;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using ExposureUnnoticed2.ObjectUI.InventoryPanel;
using ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel;
using ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Costume
{
	[CreateAssetMenu(fileName = "RCostume", menuName = "ScriptableObjects/RCostume")]
	public class RCostume : ScriptableObject, IEquipableMaster, IOnlineShopItemRecord, IShoesRecord, IReinforcableRecord, IEffectiveEquipRecord
	{
		[SerializeField]
		public bool SlideManko;

		[SerializeField]
		public bool SlideAnal;

		[field: SerializeField]
		public CostumeType Type { get; private set; }

		[field: SerializeField]
		public int Price { get; private set; }

		[field: SerializeField]
		public string DescriptionKey { get; set; }

		[field: SerializeField]
		public bool IsNotHeelToe { get; private set; }

		[field: SerializeField]
		public float HeelToeShapeScale { get; private set; }

		public int TypeId { get; }

		[field: SerializeField]
		public string NameKey { get; private set; }

		[field: SerializeField]
		public EquipMotionType EquipMotionType { get; private set; }

		[field: SerializeField]
		public CostumeCategory CostumeCategory { get; private set; }

		[field: SerializeField]
		public CostumeEffectType EffectType { get; private set; }

		[field: SerializeField]
		public int RpBonus { get; private set; }

		public int[] EffectIds => null;

		public bool[] GetReinforceSettings { get; }

		[field: SerializeField]
		public int StrangeBonus { get; private set; }

		[field: SerializeField]
		public int DokidokiBonus { get; private set; }

		[field: SerializeField]
		[field: Header("各カテゴリ専用のやつら")]
		public float HeelSize { get; private set; }

		[field: SerializeField]
		public BraChikubiType BraChikubiType { get; private set; }

		public void SwitchVisible(int defaultType = -1)
		{
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
