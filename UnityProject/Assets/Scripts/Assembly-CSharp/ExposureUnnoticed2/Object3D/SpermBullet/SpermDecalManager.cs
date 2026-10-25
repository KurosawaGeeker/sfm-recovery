using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.SpermBullet
{
	public class SpermDecalManager
	{
		private const int MaxDecalNum = 30;

		private List<SpermDecalController> decalList;

		private float bukkakeMissionRemainTime;

		public static SpermDecalManager Instance { get; private set; }

		public bool IsBukkakeNpcMission { get; private set; }

		public void Initialize()
		{
		}

		public void Dispose()
		{
            // Original RVA 0x5F0E50: CLR identity guard, static reset, ordered unregister.
            if (Instance != this) return;
            Instance = null;
            EventManager.RemoveListener<ChangeTimeEvent>(OnChangeTime);
            EventManager.RemoveListener<StartTransitionStageEvent>(OnTransitionStageEvent);
		}

		public void OnUpdate()
		{
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}

		private void OnTransitionStageEvent(StartTransitionStageEvent startTransitionStageEvent)
		{
		}

		private void ResetDecalList()
		{
		}

		public void Register(SpermDecalController decal)
		{
		}

		public void SetBukkakeNpc()
		{
		}
	// Recovered original member RVA 0x5F14E0: .ctor
		public SpermDecalManager()
        {
            decalList = new List<SpermDecalController>();
        }
	}
}
