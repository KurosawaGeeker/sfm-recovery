using ExposureUnnoticed2.Object3D.ScenePlops.Toilet;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Sound
{
	public class SoundVolumeAreaController : MonoBehaviour
	{
		[SerializeField]
		private InSidePlayerChecker targetArea;

		[SerializeField]
		private AudioSource[] targetAudioSources;

		[SerializeField]
		private float changeTime;

		private SmoothFloat volume;

		private void Start()
		{
		}

		private void Update()
		{
		}
	}
}
