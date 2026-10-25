using System;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.NpcCar
{
	public class NpcCarController : MonoBehaviour
	{
		private enum State
		{
			Move = 0,
			Brake = 1,
			Stop = 2,
			Start = 3
		}

		[SerializeField]
		private AudioSource idlingAudioSource;

		[SerializeField]
		private AudioSource startAudioSource;

		[SerializeField]
		private AudioSource stopAudioSource;

		[SerializeField]
		private AudioSource passAudioSource;

		[SerializeField]
		private CarViewArea viewArea;

		[SerializeField]
		private GameObject lightParent;

		private float MaxSpeed;

		private State currentState;

		private SmoothFloat speed;

		private SmoothFloat startVolume;

		private SmoothFloat passVolume;

		private SmoothFloat stopVolume;

		private SmoothFloat soundCancelAreaScale;

		private float initStopVolume;

		private float initIdlingVolume;

		private bool isAlreadyPlayPass;

		private Vector3 spawnPoint;

		private float destroyDistance;

		private NpcController npc;

		private Action<NpcCarController> onDestroy;

		private bool skipDestroy;

		public void Initialize(float destroyDistance, float speedKmPerHour, NpcController npc, Action<NpcCarController> onDestroy)
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

		private void ChangeState(State next)
		{
		}

		private void MoveUpdate()
		{
		}

		private void BrakeUpdate()
		{
		}

		private void StopUpdate()
		{
		}

		private void StartUpdate()
		{
		}

		public void SetSoundCancelScale(float volume)
		{
		}
	}
}
