using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ScenePlops.AutoDoor
{
	public class AutoDoorController : MonoBehaviour
	{
		private enum SoundState
		{
			None = 0,
			Open = 1,
			Close = 2
		}

		private HashSet<GameObject> inAreaObjectSet;

		private SmoothFloat openValue;

		private const float Speed = 0.01f;

		private SmoothFloat openValueAdd;

		private float closeStartRemainTime;

		private Animator animator;

		private AudioSource openAudioSource;

		private AudioSource closeAudioSource;

		private SmoothFloat openVolume;

		private SmoothFloat closeVolume;

		private float openInitVolume;

		private float closeInitVolume;

		private static readonly int OpenValue;

		private SoundState currentSoundState;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnTimeStopEvent(TimeStopEvent evt)
		{
		}

		private void Update()
		{
		}

		private void UpdateSound()
		{
		}

		private void OnTriggerEnter(Collider collider)
		{
		}

		private void OnTriggerExit(Collider collider)
		{
		}
	}
}
