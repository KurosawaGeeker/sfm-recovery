using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Sound
{
	public class CommonSoundTimeStopController : MonoBehaviour
	{
		private AudioSource audioSource;

		private float initVolume;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnTimeStopEvent(TimeStopEvent evt)
		{
		}
	}
}
