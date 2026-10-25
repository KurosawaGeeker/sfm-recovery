namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel
{
	public interface IOnlineShopItemRecord
	{
		string NameKey { get; }

		int Price { get; }

		int TypeId { get; }

		string DescriptionKey { get; }

		void SwitchVisible(int defaultType = -1);
	}
}
