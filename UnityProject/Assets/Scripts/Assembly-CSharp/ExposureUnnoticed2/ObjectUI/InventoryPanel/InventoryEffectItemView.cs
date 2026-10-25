using ExposureUnnoticed2.Master.ReinforceEffect;
using ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel
{
	public class InventoryEffectItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private TextMeshProUGUI amount;

		[SerializeField]
		private ReinforceEffectRankIconView iconView;

		public void Initialize(string name, string amount)
		{
		}

		public void Initialize(RReinforceEffect record)
		{
		}
	}
}
