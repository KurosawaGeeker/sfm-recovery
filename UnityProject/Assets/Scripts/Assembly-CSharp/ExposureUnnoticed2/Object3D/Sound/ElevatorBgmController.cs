using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Sound
{
	public class ElevatorBgmController : MonoBehaviour
	{
		private AudioSource audioSource;

		private float initVolume;

		private SmoothFloat elevatorOutsideVolumeScale;

		private SmoothFloat killTimeScale;

		private SmoothFloat timeStopScale;

		private void Start()
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
