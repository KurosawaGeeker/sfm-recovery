using ExposureUnnoticed2.Master.ReinforceEffect;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel
{
	public class ReinforceEffectItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private TextMeshProUGUI amount;

		[SerializeField]
		private ReinforceEffectRankIconView iconView;

		[SerializeField]
		private GameObject lockIcon;

		public void Initialize(RReinforceEffect record, bool isLocked)
		{
		}
	}
}
