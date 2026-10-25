using ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Item
{
	[CreateAssetMenu(fileName = "MItemRecord", menuName = "ScriptableObjects/MItemRecord")]
	public class MItemRecord : ScriptableObject, IOnlineShopItemRecord
	{
		[SerializeField]
		public ItemType Type;

		[SerializeField]
		public bool IsReusable;

		[SerializeField]
		public bool IsValuables;

		[SerializeField]
		public bool IsDelayConsume;

		[SerializeField]
		public bool CanByOnlineShop;

		[SerializeField]
		public bool CanDrop;

		[field: SerializeField]
		public string NameKey { get; set; }

		[field: SerializeField]
		public string DescriptionKey { get; set; }

		[field: SerializeField]
		public int Price { get; private set; }

		public int TypeId { get; }

		public void SwitchVisible(int defaultType = -1)
		{
		}
	}
}
