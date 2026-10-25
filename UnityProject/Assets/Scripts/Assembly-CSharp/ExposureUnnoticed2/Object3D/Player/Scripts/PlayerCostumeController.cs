using ExposureUnnoticed2.Master.Costume;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerCostumeController
	{
		private PlayerClassAccessor pca;

		public bool IsSlideManko { get; private set; }

		public bool IsSlideAnal { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.pca = pca;
		}

		public void SetupCostumeFromGameState()
		{
		}

		public void DeactivateAll()
		{
		}

		public void OnChangeCostume(RCostume record, bool isActive)
		{
		}

		private void SetActiveCostume(RCostume record, bool isActive)
		{
		}

		private void CalcCostumeEffect()
		{
		}
	}
}
