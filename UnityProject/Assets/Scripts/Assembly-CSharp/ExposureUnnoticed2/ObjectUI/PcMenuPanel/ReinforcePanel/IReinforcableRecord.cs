using ExposureUnnoticed2.Master.ReinforceEffect;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel
{
	public interface IReinforcableRecord
	{
		string NameKey { get; }

		int[] EffectIds { get; }

		bool[] GetReinforceSettings { get; }

		void SetReinforceEffect(RReinforceEffect[] effects, bool[] settings);

		void SetReinforceSettings(bool[] settings);
	}
}
