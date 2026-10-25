using System;
using ExposureUnnoticed2.Master.Skill;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.SkillSwitchPanel
{
	public class SkillSwitchItemView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI skillName;

		[SerializeField]
		private Toggle toggle;

		[SerializeField]
		private CanvasGroup emitCanvasGroup;

		private MSkillRecord record;

		private Action<MSkillRecord> onClick;

		private Action onChange;

		private bool isNoSoundByInit;

		public void Initialize(MSkillRecord record, Action<MSkillRecord> onClick, Action onChange)
		{
		}

		public void Refresh()
		{
		}

		public void OnClick()
		{
		}

		public void ChangeToggleByPad()
		{
		}

		public void OnChangeToggle()
		{
		}

		public void ForceSetEmit(int alpha)
		{
		}
	}
}
