using System;
using ExposureUnnoticed2.Master.FastTravel;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace Common.CommonUI.CommonButton
{
	public class FastTravelPanelItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private ButtonView buttonView;

		[SerializeField]
		private TextMeshProUGUI maskText;

		private Action<RFastTravel> onClick;

		private RFastTravel record;

		public void Initialize(RFastTravel record, Action<RFastTravel> onClick)
		{
		}

		public void OnClick()
		{
		}
	}
}
