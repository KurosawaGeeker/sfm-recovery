using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Master.ReinforceEffect;
using ExposureUnnoticed2.ObjectUI.BuyPanel;
using ExposureUnnoticed2.ObjectUI.InventoryPanel;
using ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel;
using ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Accessory
{
	[CreateAssetMenu(fileName = "MAdultGoodsRecord", menuName = "ScriptableObjects/MAdultGoodsRecord")]
	public class MAdultGoodsRecord : ScriptableObject, IBuyPanelItem, IEquipableMaster, IOnlineShopItemRecord, IReinforcableRecord, IEffectiveEquipRecord
	{
		public MAdultGoodsType Type;

		public EquipMotionType MotionType;

		[SerializeField]
		public int EcstasyRisePerSecond;

		[SerializeField]
		public AdultGoodsEffectType AdultGoodsEffectType;

		[SerializeField]
		public float Discomfort;

		[SerializeField]
		public Constant.DiscomfortDirect DiscomfortDirect;

		[SerializeField]
		public bool SlideManko;

		[SerializeField]
		public bool SlideAnal;

		[field: SerializeField]
		public int Price { get; private set; }

		[field: SerializeField]
		public string DescriptionKey { get; set; }

		public int TypeId { get; }

		[field: SerializeField]
		public string NameKey { get; private set; }

		[field: SerializeField]
		public int DokidokiBonus { get; private set; }

		[field: SerializeField]
		public int StrangeBonus { get; private set; }

		[field: SerializeField]
		public int RpBonus { get; private set; }

		public int[] EffectIds => null;

		public bool[] GetReinforceSettings => null;

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
            // Recovered complete original native body.
            switch ((int)MotionType)
            {
                case 0: return (ActionType)10008;
                case 1: return (ActionType)10006;
                case 4: return (ActionType)10023;
                case 7: return (ActionType)(-1);
                default: throw new global::System.Runtime.CompilerServices.SwitchExpressionException(MotionType);
            }
        }

		public ActionType TransDetachActionType()
		{
            // Recovered complete original native body.
            switch ((int)MotionType)
            {
                case 0: return (ActionType)10008;
                case 1: return (ActionType)10007;
                case 4: return (ActionType)10023;
                case 7: return (ActionType)(-1);
                default: throw new global::System.Runtime.CompilerServices.SwitchExpressionException(MotionType);
            }
        }
	}
}
