using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Sound
{
	public class ShopEnvironmentBgmController : MonoBehaviour
	{
		private SmoothFloat killTimeScale;

		private SmoothFloat timeStopScale;

		private SmoothFloat elevatorOutsideVolumeScale;

		private AudioSource audioSource;

		private float initVolume;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void OnBgmControlEvent(BgmControlEvent bgmControlEvent)
		{
		}
	}
}
