using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Sound
{
	public class LoopSoundPlayer : MonoBehaviour
	{
		private const float FadeTime = 0.3f;

		[SerializeField]
		private InGameSoundManager.SeType sound;

		[SerializeField]
		private float fadeTransBgmTimeRate;

		private AudioSource[] bgmSources;

		private int currentAudioSourceIndex;

		private SmoothFloat bgmVolumeScale;

		private float initVolume;

		private bool isPlay;

		public float OtherVolumeScale { get; set; }

		public void Initailize()
		{
		}

		private void OnDestroy()
		{
		}

		public void Play()
		{
		}

		public void Stop(float fadeTime)
		{
		}

		private void Update()
		{
		}

		private void ChangeAudioSource()
		{
		}

		private AudioSource GetCurrentAudioSource()
		{
			return null;
		}

		private void OnTimeStopEvent(TimeStopEvent evt)
		{
		}
	}
}
