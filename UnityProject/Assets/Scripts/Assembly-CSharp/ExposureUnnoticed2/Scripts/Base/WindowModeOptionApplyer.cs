using System.Collections.Generic;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class WindowModeOptionApplyer
	{
		private static bool initialized;

		public static List<ResolutionData> ResolutionDataList = new List<ResolutionData>();

		public static List<ResolutionData> WideResolutionDataList = new List<ResolutionData>();

		public static void InitializeIfNeed()
		{
			// Recovered from GameAssembly.dll RVA 0x4BC130
			if (initialized) return;
			initialized = true;
			ResolutionDataList.Add(new ResolutionData(1280, 720));
			ResolutionDataList.Add(new ResolutionData(1920, 1080));
			ResolutionDataList.Add(new ResolutionData(2560, 1440));
			ResolutionDataList.Add(new ResolutionData(3840, 2160));
			WideResolutionDataList.Add(new ResolutionData(1920, 800));
			WideResolutionDataList.Add(new ResolutionData(2560, 1080));
			WideResolutionDataList.Add(new ResolutionData(3440, 1440));
			WideResolutionDataList.Add(new ResolutionData(5120, 2160));
		}

		public static List<ResolutionData> GetTargetResolutionList()
		{
			// Recovered from GameAssembly.dll RVA 0x4BC440
			return global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.AspectRatioIndex == 0 ? ResolutionDataList : WideResolutionDataList;
		}

		public static void Apply()
		{
			// Recovered from GameAssembly.dll RVA 0x4BC500
			InitializeIfNeed();
			UnityEngine.Screen.fullScreen = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.WindowMode == 1;
			ResolutionData resolution = GetTargetResolutionList()[global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.Resolusion];
			UnityEngine.FullScreenMode mode = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.WindowMode == 1
			    ? UnityEngine.FullScreenMode.FullScreenWindow : UnityEngine.FullScreenMode.Windowed;
			UnityEngine.RefreshRate refreshRate = new UnityEngine.RefreshRate
			{
			    numerator = (uint)System.Math.Max(0, global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.MaxFrameRate),
			    denominator = 1u
			};
			UnityEngine.Screen.SetResolution(resolution.X, resolution.Y, mode, refreshRate);
		}
	// Recovered original member RVA 0x4BC7B0: .cctor
		// Recovered original .cctor RVA 0x4BC7B0: ordered field initializers preserve BeforeFieldInit.
	}
}
