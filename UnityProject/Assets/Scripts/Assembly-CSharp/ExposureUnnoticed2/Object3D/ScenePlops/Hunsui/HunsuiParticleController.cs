using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.Hunsui
{
	public class HunsuiParticleController : MonoBehaviour
	{
		[SerializeField]
		private GameObject parentObject;

		[SerializeField]
		private AudioSource audioSource;

		[SerializeField]
		private ParticleSystem[] particle;

		private bool oldIsDaytime;

		private void Start()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private void Change()
		{
		}

		private void OnTimeStopEvent(TimeStopEvent evt)
		{
		}
	}
}
