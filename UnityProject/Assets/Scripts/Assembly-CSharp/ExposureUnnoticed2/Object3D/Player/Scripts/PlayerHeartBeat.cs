namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerHeartBeat
	{
		public const float MaxBeat = 152f;

		public const float MinBeat = 95f;

		private PlayerStateModel playerStateModel;

		public float HeartRate { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x64F7B0
			playerStateModel = pca.PlayerState;
		}

		public void OnUpdate()
		{
			// SOURCE DRAFT v2: original64F820 and inlined64469D..6447DC, including unordered float branches.
			var data = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			float dokidoki = (int)data.CurrentStageType == 0 ? 0f : playerStateModel.Dokidoki;
			float target = dokidoki * 57f + 95f;
			// Original COMISS/JBE chooses0.1 on unordered comparison as well as target<=current.
			float rate = target > HeartRate ? 0.2f : 0.1f;
			float speed = System.Math.Abs(target - HeartRate) * rate;
			// Original MINSS operand2 is10000: ties/unordered choose that operand.
			if (!(speed < 10000f)) speed = 10000f;
			float oldHeartRate = HeartRate;
			float delta = UnityEngine.Time.deltaTime;
			float step = delta * speed;
			// SETB after COMISS is true for unordered too; do not use ordered '<'.
			if (!(step >= System.Math.Abs(target - oldHeartRate)))
			{
			    float difference = target - oldHeartRate;
			    float sign = difference >= 0f ? 1f : -1f;
			    target = sign * step + oldHeartRate;
			}
			HeartRate = target;
		}
	// Recovered original member RVA 0x64F990: .ctor
		public PlayerHeartBeat()
        {
            HeartRate = 95f;
        }
	}
}
