using ExposureUnnoticed2.Master.Mission;

namespace ExposureUnnoticed2.ObjectUI.InGame.MissionPanel
{
	public interface IMissionItemPanelModel
	{
		string MissionName { get; }

		MissionType MissionType { get; }

		int RewardRp { get; }

		float Satisfy { get; }

		bool IsPerSec { get; }

		float RpPerSec { get; }
	}
}
