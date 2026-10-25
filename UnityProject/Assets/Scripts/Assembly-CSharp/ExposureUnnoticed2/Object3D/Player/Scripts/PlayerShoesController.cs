using ExposureUnnoticed2.Object3D.Player.Scripts.Other;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerShoesController
	{
		private PlayerClassAccessor pca;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.pca = pca;
		}

		public void ChangeCurrentShoes(IShoesRecord shoesRecord, bool isActive)
		{
			// Recovered from GameAssembly.dll RVA 0x65D230
			var state = pca.PlayerState;
			if (!object.ReferenceEquals(state.CurrentShoesRecord, shoesRecord) && !isActive) return;
			if (state.CurrentShoesRecord != null)
			{
			    if (state.CurrentShoesRecord is global::ExposureUnnoticed2.Master.Costume.RCostume costume)
			    {
			        global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCostumeDict[costume.Type] = false;
			        pca.PlayerAvatarObjectReferencer.Costumes[(int)costume.Type].SetActive(false);
			    }
			    else if (pca.PlayerState.CurrentShoesRecord is global::ExposureUnnoticed2.Master.Cosplay.RCosplayParts part)
			    {
			        global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.SetActiveCosplayParts(part.UniqueId, false);
			        pca.PlayerAvatarObjectReferencer.CosplayControllers[(int)part.ParentCosplayType].SetActiveParts(part.Id, false);
			    }
			}
			pca.PlayerState.CurrentShoesRecord = isActive ? shoesRecord : null;
			global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeShoesEvent);
		}
	}
}
