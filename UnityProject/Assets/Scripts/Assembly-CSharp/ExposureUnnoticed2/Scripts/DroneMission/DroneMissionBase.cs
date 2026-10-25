using ExposureUnnoticed2.Master.DroneMission;
using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.ObjectUI.InGame.MissionPanel;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.DroneMission
{
	public abstract class DroneMissionBase : IMissionItemPanelModel
	{
		public RDroneMission Record;

		public string MissionName => null;

		public MissionType MissionType => MissionType.Time;

		public int RewardRp => 0;

		public float Satisfy => 0f;

		public bool IsOverDestroyTime { get; private set; }

		public float Progress { get; protected set; }

		public bool IsPerSec => false;

		public float RpPerSec { get; }

		public bool IsComplete => false;

		public abstract bool CheckCondition();

		protected abstract void UpdateProgress();

		protected abstract void OnMissionStartChild();

		public void OnMissionStart()
		{
		}

		public void Initialize(RDroneMission record)
		{
            // Complete original0x481730; Record survives a later missing GameStateData failure.
            Record = record;
            Progress = GameState.GameStateData.ActiveDroneProgress;
        }

		public void OnUpdate()
		{
		}

		public virtual void Reset()
		{
		}

		protected bool CommonCheckCondition()
		{
			return false;
		}

		protected void WarpPlayerRandomPosition()
		{
		}

		protected void DropMissionItem(DroppedItemManager.DropItemType dropType, Vector3 position)
		{
		}

		protected void SetupVibe(Vector3 position, bool isDropRemocon)
		{
		}
	}
}
