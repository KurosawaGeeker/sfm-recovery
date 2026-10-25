using ExposureUnnoticed2.Master.Hair;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerHairController
	{
		private PlayerClassAccessor pca;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x64E100
			this.pca = pca;
			if (global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CurrentHair == (global::ExposureUnnoticed2.Master.Hair.HairType)0) return;
			pca.PlayerAvatarObjectReferencer.Hairs[0].SetActive(false);
			ChangeHair(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CurrentHair);
		}

		public void ChangeHair(HairType next)
		{
			// Recovered from GameAssembly.dll RVA 0x64E260
			pca.PlayerAvatarObjectReferencer.Hairs[(int)global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CurrentHair].SetActive(false);
			if (next == (global::ExposureUnnoticed2.Master.Hair.HairType)(-1)) return;
			pca.PlayerAvatarObjectReferencer.Hairs[(int)next].SetActive(true);
			global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CurrentHair = next;
		}
	}
}
