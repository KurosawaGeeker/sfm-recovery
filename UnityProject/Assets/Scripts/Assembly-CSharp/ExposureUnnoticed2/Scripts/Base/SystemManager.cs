using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class SystemManager : MonoBehaviour
	{
		public static bool IsTrial = false;

		public static bool IsSex = true;

		public static bool IsEnc = true;

		public static bool IsFanza = false;

		private static SystemManager instance;

		private TimeScaleManager timeScaleManager;

		public static void CreateIfNoExist()
		{
			// Recovered from GameAssembly.dll RVA 0x4BB1F0
			if (instance != null) return;
			UnityEngine.Object.Instantiate((GameObject)UnityEngine.Resources.Load("SystemManager"));
		}

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x4BB380
			if (instance != null)
			{
			    UnityEngine.Object.Destroy(gameObject);
			    return;
			}
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			instance = this;
			timeScaleManager = new TimeScaleManager();
			timeScaleManager.Initialize();
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x4BB8A0
			if (instance == this) timeScaleManager.Dispose();
		}

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x4BBA90
			timeScaleManager.OnUpdate();
		}
	// Recovered original member RVA 0x4BBAB0: .cctor
		// Recovered original .cctor RVA 0x4BBAB0: ordered field initializers preserve BeforeFieldInit.
	}
}
