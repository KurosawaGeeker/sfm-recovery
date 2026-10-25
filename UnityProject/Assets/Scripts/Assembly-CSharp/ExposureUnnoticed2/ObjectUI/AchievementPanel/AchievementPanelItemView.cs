using System;
using ExposureUnnoticed2.Master.Achievement;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.AchievementPanel
{
	public class AchievementPanelItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private TextMeshProUGUI condition;

		[SerializeField]
		private TextMeshProUGUI reward;

		[SerializeField]
		private ButtonView receiveButton;

		[SerializeField]
		private GameObject checkMarkObject;

		private Action<RAchievement> onClickReceive;

		private RAchievement record;

		private bool isAchieved;

		private bool isMasked;

		public void Initialize(RAchievement record, Action<RAchievement> onClickReceive)
		{
		}

		public void OnClickReceive()
		{
		}
	}
}
