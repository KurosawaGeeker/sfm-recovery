namespace ExposureUnnoticed2.Object3D.NPC.Script.State
{
	public class NpcTraceSexPlayerState
	{
		private NpcComponentAccessor nca;

		private int initAreaMask;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
			initAreaMask = nca.NavAgent.areaMask;
			nca.NpcComponent.InteractArea.gameObject.SetActive(false);
			nca.NpcComponent.InteractArea.MenuAction.Add(0, new ExposureUnnoticed2.ObjectUI.InteractMenuPanel.InteractMenuItemModel
			{
			    NameKey = "ui_interactMenu_sex",
			    OnClick = () => { if (ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.CanUseHand(true)) ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.ActiveSex(); }
			});
			nca.NpcComponent.InteractArea.MenuAction.Add(1, new ExposureUnnoticed2.ObjectUI.InteractMenuPanel.InteractMenuItemModel
			{
			    NameKey = "ui_interactMenu_sexCancel",
			    OnClick = () => nca.NpcController.ChangeState((ExposureUnnoticed2.Object3D.NPC.Script.NpcStateModel.State)0)
			});
			nca.NpcComponent.InteractArea.Initialize(() => { if (ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.CanUseHand(false)) ExposureUnnoticed2.ObjectUI.InteractMenuPanel.InteractMenuPanelView.Open(); }, null);
		}

		public void OnEnter()
		{
		}

		public void OnExit()
		{
		}

		public void OnUpdate()
		{
		}
	}
}
