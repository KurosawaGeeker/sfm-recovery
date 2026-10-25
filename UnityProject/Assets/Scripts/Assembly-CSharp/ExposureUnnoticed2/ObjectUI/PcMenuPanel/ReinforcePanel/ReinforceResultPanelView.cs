using System;
using ExposureUnnoticed2.Master.ReinforceEffect;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel
{
	public class ReinforceResultPanelView : MonoBehaviour
	{
		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private TextMeshProUGUI title;

		private Action onClose;

		private float openTime;

		public void Initialize(IReinforcableRecord record, RReinforceEffect[] result, Action onClose)
		{
		}

		private void Update()
		{
		}

		public void OnClickOk()
		{
		}

		public void OnAnimCloseEnd()
		{
		}
	}
}
