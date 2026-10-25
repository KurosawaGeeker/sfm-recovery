using ExposureUnnoticed2.Master.Clothes;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerHoldInHand
	{
		private PlayerClassAccessor pca;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x6500A0
			this.pca = pca;
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentHoldClothes != (global::ExposureUnnoticed2.Master.Clothes.ClothesType)(-1))
			    Hold(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentHoldClothes, true);
		}

		public void Dispose()
		{
		}

		public void Hold(ClothesType type, bool inInit = false)
		{
			// Recovered from GameAssembly.dll RVA 0x650170
			if ((int)type == -1)
			{
			    global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(pca.PlayerAvatarObjectReferencer.HoldIdParent);
			    var dropped = pca.DroppedItemManager;
			    var position = pca.Transform.position;
			    var rotation = pca.Transform.rotation;
			    int previous = (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentHoldClothes;
			    var dropType = (global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)(previous == 0 ? 0 : previous == 1 ? 1 : -1);
			    dropped.Drop(position, rotation, dropType);
			    if (inInit) return;
			    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.SetHoldClothes(type);
			    return;
			}
			if (!inInit)
			{
			    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.SetHoldClothes(type);
			    var dropped = pca.DroppedItemManager;
			    int picked = (int)pca.PlayerState.LastPickClothesType;
			    dropped.Collect((global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)(picked == 0 ? 0 : picked == 1 ? 1 : -1));
			}
			pca.PlayerAnimationManager.SetHoldInHand(true);
			var prefab = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.HoldInHandClothesPrefabs[(int)type];
			UnityEngine.Object.Instantiate(prefab, pca.PlayerAvatarObjectReferencer.HoldIdParent);
		}

		public void TakeFromWasher()
		{
		}

		public void IntoWasher()
		{
		}
	}
}
