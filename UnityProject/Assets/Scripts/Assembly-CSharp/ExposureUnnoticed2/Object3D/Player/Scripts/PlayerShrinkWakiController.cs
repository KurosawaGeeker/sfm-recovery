using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerShrinkWakiController
	{
		private PlayerClassAccessor pca;

		private SmoothFloat blend;

		private SmoothFloat blend2;

		private SmoothFloat blend3;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.pca = pca;
		}

		public void OnUpdate()
		{
		}
	}
}
