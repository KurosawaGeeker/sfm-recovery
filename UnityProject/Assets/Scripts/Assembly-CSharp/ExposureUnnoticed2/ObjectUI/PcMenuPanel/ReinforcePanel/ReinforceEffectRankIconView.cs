using ExposureUnnoticed2.Master.ReinforceEffect;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel
{
	public class ReinforceEffectRankIconView : MonoBehaviour
	{
		[SerializeField]
		private GameObject[] upIcons;

		[SerializeField]
		private GameObject[] downIcons;

		[SerializeField]
		private GameObject[] leftIcons;

		[SerializeField]
		private GameObject[] rightIcons;

		public void AllHide()
		{
		}

		private void SetView(int rank, bool isNegative, bool isNeutral)
		{
		}

		public void SetView(RReinforceEffect record)
		{
		}
	}
}
