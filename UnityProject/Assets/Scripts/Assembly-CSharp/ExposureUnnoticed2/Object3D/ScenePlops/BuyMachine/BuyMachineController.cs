using ExposureUnnoticed2.Object3D.InteractArea;
using ExposureUnnoticed2.Object3D.RoutePoint;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.BuyMachine
{
	public class BuyMachineController : BaseInteractTokenPublisher
	{
		private AudioSource audioSource;

		private InteractAreaController interactAreaController;

		private void Start()
		{
			// Original RVA 0x60B110; complete original body, unchanged owner/signature.
			audioSource = GetComponent<AudioSource>();
			interactAreaController = GetComponentInChildren<InteractAreaController>();
			interactAreaController.Initialize(OnInteract, null);
			SetMaxTokenNum(1);
		}

		private void OnInteract()
		{
			// Original RVA 0x60B2A0; complete original body, unchanged owner/signature.
			if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.CanUseHand(true)) return;
			if (!TryGetToken())
			{
			    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(
			        global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("log_use_npc"));
			    return;
			}
			global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.LastInteractTokenPublisher = this;
			global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.SmoothMoveTo(interactAreaController.transform);
			global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.TransAction((global::ExposureUnnoticed2.Master.Action.ActionType)10011);
		}
	}
}
