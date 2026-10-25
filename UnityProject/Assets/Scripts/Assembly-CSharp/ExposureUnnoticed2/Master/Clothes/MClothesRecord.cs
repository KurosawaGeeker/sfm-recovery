using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.ObjectUI.BuyPanel;
using ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Clothes
{
	[CreateAssetMenu(fileName = "MClothesRecord", menuName = "ScriptableObjects/MClothesRecord")]
	public class MClothesRecord : ScriptableObject, IBuyPanelItem, IOnlineShopItemRecord
	{
		public ClothesType ClothesType;

		public PlayerStateModel.ClothesState ClothesStateAAfterPick;

		public MClothesAction[] Actions;

		[field: SerializeField]
		public int Price { get; private set; }

		[field: SerializeField]
		public string DescriptionKey { get; set; }

		[field: SerializeField]
		public string NameKey { get; private set; }

		public int TypeId => 0;

		public MClothesAction GetAction(PlayerStateModel.ClothesState state)
		{
            var actions = Actions;
            for (int i = 0; i < actions.Length; i++)
            {
                var action = actions[i];
                if (action.State == state) return action;
            }
            return null;
        }

		public void SwitchVisible(int defaultType = -1)
		{
		}
	}
}
