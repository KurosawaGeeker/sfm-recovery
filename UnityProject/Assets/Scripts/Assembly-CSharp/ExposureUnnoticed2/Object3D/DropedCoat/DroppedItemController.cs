using ExposureUnnoticed2.Object3D.InteractArea;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.DropedCoat
{
	public class DroppedItemController : MonoBehaviour
	{
		private DroppedItemManager.DropItemType type;

		public InteractAreaController Interact { get; private set; }

		public void Initialize(DroppedItemManager.DropItemType type)
		{
			// Recovered from GameAssembly.dll RVA 0x459880
			this.type = type;
			Interact = GetComponentInChildren<InteractAreaController>();
			Interact.DropItemType = type;
			string text = (int)type switch
			{
			    0 => "dropitem_override_text_coat",
			    1 => "dropitem_override_text_hoodie",
			    3 => "dropitem_override_text_pants",
			    4 => "dropitem_override_text_bra",
			    5 => "dropitem_override_text_handcuff_key",
			    6 => "dropitem_override_text_vibe_remocon",
			    -1 => null,
			    2 => null,
			    _ => ""
			};
			if ((int)Interact.Type == 3)
			{
			    Interact.Initialize(global::ExposureUnnoticed2.ObjectUI.InteractMenuPanel.InteractMenuPanelView.Open, null);
			    if (global::ExposureUnnoticed2.Scripts.InGame.FashionShopManager.Instance != null) return;
			    if (global::ExposureUnnoticed2.Object3D.AdultShopScene.AdultShopManager.Instance != null) return;
			    return;
			}
			if ((int)Interact.Type == 4)
			{
			    // Original captured callback RVA0x45A100.
			    Interact.Initialize(() => global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransPickClothes(DroppedItemManager.ToClothesType(type)), text);
			}
			else if ((int)Interact.Type == 12)
			{
			    // Original captured callback RVA0x45A1A0.
			    Interact.Initialize(() =>
			    {
			        if ((int)type == 3) global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)10015);
			        else if ((int)type == 4) global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)10017);
			    }, text);
			}
			else if ((int)Interact.Type == 15)
			{
			    // Original cached callback RVA0x459F30.
			    Interact.Initialize(() =>
			    {
			        global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.LastPickUpItemType = (DroppedItemManager.DropItemType)5;
			        global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.ForceSkipHandcuffCheckTransAction = true;
			        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)10025);
			    }, text);
			}
			else if ((int)Interact.Type == 16)
			{
			    // Original cached callback RVA0x45A040.
			    Interact.Initialize(() =>
			    {
			        global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.LastPickUpItemType = (DroppedItemManager.DropItemType)6;
			        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)10025);
			    }, text);
			}
		}

		private void AddFashionShopMenuAction()
		{
		}

		private void AddAdultShopMenuAction()
		{
		}

		private void AddDroppedClothesMenuAction()
		{
		}
	}
}
