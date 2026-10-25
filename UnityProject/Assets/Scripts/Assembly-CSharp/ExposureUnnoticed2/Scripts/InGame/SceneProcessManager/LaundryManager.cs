using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using ExposureUnnoticed2.Object3D.ScenePlops.ChairSitController;
using ExposureUnnoticed2.Object3D.ScenePlops.WasherMachine;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame.SceneProcessManager
{
	public class LaundryManager : MonoBehaviour
	{
		[SerializeField]
		private ChairSitController[] chairSitControllers;

		[SerializeField]
		private WasherMachineController[] washerMachineControllers;

		private List<WasherMachineController> nonNpcTargetWasherList;

		private WasherMachineController usingWasher;

		private int useMinutes;

		private int remainNpcToken;

		public static LaundryManager Instance { get; private set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void ChangeMaxToken()
		{
		}

		public void SetRpBonus(WasherMachineController washer, int minutes)
		{
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}

		public BuffSourceType GetRpBuffSourceType()
        {
            if (!usingWasher || (int)usingWasher.CurrentState != 1 || !usingWasher.IsUseByPlayer) return (BuffSourceType)(-1);
            switch (useMinutes)
            {
                case 1: return (BuffSourceType)38;
                case 2: return (BuffSourceType)39;
                case 3: return (BuffSourceType)40;
                case 4: return (BuffSourceType)41;
                case 5: return (BuffSourceType)42;
                default: throw new global::System.Runtime.CompilerServices.SwitchExpressionException(useMinutes);
            }
        }

		public bool IsInClothes()
		{
            return usingWasher != null && usingWasher.CurrentState != global::ExposureUnnoticed2.Object3D.ScenePlops.WasherMachine.WasherMachineController.State.Idle;
		}

		public bool TryGetNpcToken()
		{
			return false;
		}

		public WasherMachineController PublishEmptyWasher()
		{
			return null;
		}

		public ChairSitController PublishEmptyChair()
		{
			return null;
		}

		public void ReturnToken()
		{
			++remainNpcToken;
		}

		public void OnEndNpc(ChairSitController chair, WasherMachineController washer)
		{
		}
	}
}
