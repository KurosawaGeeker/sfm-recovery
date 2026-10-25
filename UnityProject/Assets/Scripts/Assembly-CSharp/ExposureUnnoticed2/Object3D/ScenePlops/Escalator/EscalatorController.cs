using Common.Scripts.InGame;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Escalator
{
	public class EscalatorController : MonoBehaviour
	{
		[SerializeField]
		private OnSplineChaser onSplineChaser;

		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private float delayAudioTime;

		private Animator animator;

		private SmoothFloat elevatorOutsideVolumeScale;

		private float initVolume;

		[field: SerializeField]
		public EscalatorForceEffector ForceEffector { get; private set; }

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void OnTimeStopEvent(TimeStopEvent evt)
		{
		}
	}
}
