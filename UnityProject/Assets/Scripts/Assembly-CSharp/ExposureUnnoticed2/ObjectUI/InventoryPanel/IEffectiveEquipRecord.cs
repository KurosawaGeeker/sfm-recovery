namespace ExposureUnnoticed2.ObjectUI.InventoryPanel
{
	public interface IEffectiveEquipRecord
	{
		int DokidokiBonus { get; }

		int StrangeBonus { get; }

		int RpBonus { get; }

		int[] EffectIds { get; }
	}
}
