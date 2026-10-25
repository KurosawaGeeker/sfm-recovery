using ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel;
using UnityEngine;

namespace ExposureUnnoticed2.Master.BodyPaint
{
	[CreateAssetMenu(fileName = "RBodyPaint", menuName = "ScriptableObjects/RBodyPaint")]
	public class RBodyPaint : ScriptableObject, IOnlineShopItemRecord
	{
		[field: SerializeField]
		public BodyPaintType Type { get; private set; }

		[field: SerializeField]
		public string NameKey { get; private set; }

		[field: SerializeField]
		public int Price { get; private set; }

		[field: SerializeField]
		public string DescriptionKey { get; set; }

		public int TypeId { get; }

		public void SwitchVisible(int defaultType = -1)
		{
		}
	}
}
