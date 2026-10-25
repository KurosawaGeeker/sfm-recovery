using System.Collections.Generic;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class TimeScaleManager
	{
		public enum TimeScaleMode
		{
			OpenPause = 0,
			OpenTutorial = 1
		}

		public static TimeScaleManager Instance;

		private List<bool> modeActivationStateList = new List<bool>();

		private float currentTimeScale;

		public float GetCurrentScale => UnityEngine.Time.timeScale;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x4BBB80
			Instance = this;
			modeActivationStateList.Add(false);
			modeActivationStateList.Add(false);
			currentTimeScale = 1f;
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x4BBCF0
			Instance = null;
		}

		public void OnUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x4BBD90
			if (modeActivationStateList[0] || modeActivationStateList[1])
			{
			    UnityEngine.Time.timeScale = 0f;
			    UnityEngine.Time.fixedDeltaTime = 0f;
			}
			else
			{
			    UnityEngine.Time.timeScale = currentTimeScale;
			    UnityEngine.Time.fixedDeltaTime = currentTimeScale * 0.02f;
			}
		}

		public void SwitchMode(TimeScaleMode mode, bool isActive)
		{
			// Recovered from GameAssembly.dll RVA 0x4BBF60
			modeActivationStateList[(int)mode] = isActive;
		}

		public void RequestTimeScale(float timeScale)
		{
			// Recovered from GameAssembly.dll RVA 0x4BBFF0
			if (!modeActivationStateList[0]) currentTimeScale = timeScale;
		}
	}
}
