namespace AkilliMum.Standard.Mirror
{
	public class OptionManager : ManagerBase
	{
		public struct OptionSettings
		{
			public bool IsDebug { get; set; }

			public int LODLevel { get; set; }

			public bool DisablePixelLights { get; set; }

			public bool Shadow { get; set; }
		}

		private bool _previousFog;

		private int _previousLODLevel;

		private int _previousMaxAdditionalLightsCount;

		private float _previousShadowDistance;

		private OptionSettings _settings;

		public OptionManager(OptionSettings settings)
        {
            _settings = settings;
        }

		private void SetSettings(OptionSettings settings)
		{
		}

		public void Start()
		{
		}

		public void End()
		{
		}

		public override void Destroy()
		{
		}
	}
}
