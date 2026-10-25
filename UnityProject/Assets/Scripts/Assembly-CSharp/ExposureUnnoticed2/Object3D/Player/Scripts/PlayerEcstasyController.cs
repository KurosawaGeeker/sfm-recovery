namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerEcstasyController
	{
		private PlayerClassAccessor pca;

		private float lastAddEcstasyTime;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.pca = pca;
		}

		public void OnUpdate()
		{
		}

		private bool IsStopEcstasyByHandcuffEcstasyCoolTime()
		{
			return false;
		}
	}
}
