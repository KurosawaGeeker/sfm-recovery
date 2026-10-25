using System;
using ExposureUnnoticed2.Master.Hair;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu
{
	public class CustomizeHairItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		private Action<MHairRecord> onClick;

		private MHairRecord record;

		public HairType HairType => HairType.Short;

		public void Initialize(MHairRecord record, Action<MHairRecord> onClick)
		{
		}

		public void OnClick()
		{
		}
	}
}
