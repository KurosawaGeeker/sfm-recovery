using System;
using ExposureUnnoticed2.Master.Sex;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.SexMenu
{
	public class SexMenuPanelItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		private Action<RSex> onClick;

		private RSex rSex;

		public void Initialize(RSex rSex, Action<RSex> onClick)
		{
		}

		public void OnClick()
		{
		}
	}
}
