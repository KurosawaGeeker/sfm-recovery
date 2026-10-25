using System;
using Common.CommonUI.CommonSettingItem;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.DroneMissionPanel
{
	public class RandomMissionPanelView : MonoBehaviour
	{
		[SerializeField]
		private SettingSelectItemView mapSelect;

		[SerializeField]
		private SettingSelectItemView missionNumSelect;

		[SerializeField]
		private TextMeshProUGUI rewardRp;

		[SerializeField]
		private ButtonView startButtonView;

		[SerializeField]
		private RandomMissionItemCheckView vibeItemCheck;

		[SerializeField]
		private RandomMissionItemCheckView vibeSwitchItemCheck;

		[SerializeField]
		private RandomMissionItemCheckView handcuffItemCheck;

		[SerializeField]
		private RandomMissionItemCheckView eyeMaskItemCheck;

		[SerializeField]
		private RandomMissionItemCheckView dildoItemCheck;

		[SerializeField]
		private RandomMissionItemCheckView coatItemCheck;

		[SerializeField]
		private RandomMissionItemCheckView futanariItemCheck;

		[SerializeField]
		private RandomMissionItemCheckView waterItemCheck;

		[SerializeField]
		private RandomMissionItemCheckView perspectiveSkillCheck;

		[SerializeField]
		private RandomMissionItemCheckView exhibitionismSkillCheck;

		[SerializeField]
		private RandomMissionItemCheckView sexSkillCheck;

		[SerializeField]
		private GameObject randomOrderMask;

		[SerializeField]
		private GameObject randomCompleteMask;

		[SerializeField]
		private GameObject clearObject;

		private int currentSelectMapIndex;

		private int currentMissionNum;

		private int currentMissionNumIndex;

		private bool isMaskedStart;

		private Action close;

		public void Initialize(Action close)
		{
		}

		private void OnChange()
		{
		}

		public void OnUpdateForPad()
		{
		}

		public void OnClickStart()
		{
		}
	}
}
