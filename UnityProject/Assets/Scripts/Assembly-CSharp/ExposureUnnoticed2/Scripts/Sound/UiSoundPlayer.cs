using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Sound
{
	public class UiSoundPlayer : MonoBehaviour
	{
		public static UiSoundPlayer Instance;

		private AudioSource audioSource;

		private AudioSource audioSourceBgm;

		private bool isFadeStop;

		private float initVolume;

		private SmoothFloat volumeScale;

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x477570
			if (Instance != null)
			{
			    UnityEngine.Object.Destroy(gameObject);
			    return;
			}
			Instance = this;
			AudioSource[] sources = GetComponents<AudioSource>();
			audioSource = sources[0];
			audioSourceBgm = sources[1];
		}

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x477840
			if (!isFadeStop) return;
			volumeScale.UpdateTo(0f);
			audioSourceBgm.volume = audioSource.volume = volumeScale.Current * initVolume;
			if (volumeScale.Current != 0f) return;
			audioSource.Stop();
			audioSourceBgm.Stop();
			audioSourceBgm.volume = audioSource.volume = initVolume;
			isFadeStop = false;
		}

		public void PlayOneShot(InGameSoundManager.SeType seType)
		{
			// Recovered from GameAssembly.dll RVA 0x4779E0
			if (seType != InGameSoundManager.SeType.NoType) audioSource.PlayOneShot(InGameSoundManager.Instance.GetSound(seType), 1f);
		}

		public void PlayOneShotAudioData(InGameSoundManager.SeType seType, float time = -1f, bool isBgm = false)
		{
			// Recovered from GameAssembly.dll RVA 0x477A70
			if (seType == InGameSoundManager.SeType.NoType) return;
			AudioSource source = isBgm ? audioSourceBgm : audioSource;
			InGameSoundManager.Instance.PlayOneShotAudioData(source, seType, -1f);
			if (!(time < 0f)) Scheduler.DelayUnscaledTime(() => source.Stop(), time);
		}

		public void FadeStop(float fadeTime = 0.5f)
		{
			// Recovered from GameAssembly.dll RVA 0x477BF0
			volumeScale.SetSecond(fadeTime);
			isFadeStop = true;
			volumeScale.Current = 1f;
		}
	// Recovered original member RVA 0x477C30: .ctor
		public UiSoundPlayer()
        {
            initVolume = 1f;
            volumeScale = new SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
        }
	}
}
