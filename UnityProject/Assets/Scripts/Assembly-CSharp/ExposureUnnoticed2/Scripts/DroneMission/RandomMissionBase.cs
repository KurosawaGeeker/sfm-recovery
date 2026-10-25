using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.ObjectUI.InGame.MissionPanel;

namespace ExposureUnnoticed2.Scripts.DroneMission
{
	public class RandomMissionBase : IMissionItemPanelModel
	{
		public string MissionName => null;

		public MissionType MissionType => MissionType.Time;

		public int RewardRp => 0;

		public float Satisfy => 0f;

		public bool IsOverDestroyTime { get; private set; }

		public float Progress { get; protected set; }

		public bool IsPerSec => false;

		public float RpPerSec { get; }

		public bool IsComplete => false;

		public void OnMissionStart()
		{
		}

		public void Initialize()
		{
		}

		public void OnUpdate(float progress)
		{
		}

		public virtual void Reset()
		{
		}
	}
}
