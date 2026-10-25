using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Sound
{
	public class Bgm2DAreaController : MonoBehaviour
	{
		private const float FadeTime = 0.3f;

		[SerializeField]
		private float constantDist;

		[SerializeField]
		private float fadeDist;

		[SerializeField]
		private float fadeTransBgmTimeRate;

		private AudioSource[] bgmSources;

		private int currentAudioSourceIndex;

		private float initVolume;

		private SmoothFloat bgmControlVolumeScale;

		private SmoothFloat timeStopScale;

		private void Start()
		{
		}

		public void OnDestroy()
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

		private void OnTransitionStageEvent(StartTransitionStageEvent startTransitionStageEvent)
		{
		}

		private void OnBgmControlEvent(BgmControlEvent bgmControlEvent)
		{
		}

		private void OnDrawGizmos()
		{
		}
	}
}
