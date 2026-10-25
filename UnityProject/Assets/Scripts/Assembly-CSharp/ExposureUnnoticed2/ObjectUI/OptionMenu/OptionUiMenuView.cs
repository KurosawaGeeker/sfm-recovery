using System;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu
{
	public class OptionUiMenuView : MonoBehaviour, IOptionMiddlePanel
	{
		[SerializeField]
		private OptionItemCheckView faceCamera;

		[SerializeField]
		private OptionItemCheckView bodyCamera;

		[SerializeField]
		private OptionItemCheckView message;

		[SerializeField]
		private OptionItemCheckView ringMenuMessage;

		[SerializeField]
		private OptionItemCheckView playerInfo;

		[SerializeField]
		private OptionItemCheckView ecstasyGauge;

		[SerializeField]
		private OptionItemCheckView compass;

		[SerializeField]
		private OptionItemCheckView missionPanel;

		[SerializeField]
		private OptionItemBarView missionPanelSize;

		[SerializeField]
		private OptionItemCheckView rightDownPanel;

		[SerializeField]
		private OptionItemCheckView interactPanel;

		[SerializeField]
		private OptionItemCheckView heartBeatEffect;

		[SerializeField]
		private OptionItemCheckView manual;

		[SerializeField]
		private OptionItemBarView heartBeatEffectAlpha;

		private Animator animator;

		private Action onClose;

		public void Initialize(Action onClose, Action<OptionItemType> onClickItem)
		{
		}

		private void Update()
		{
		}

		public void Close()
		{
		}

		public void OnAnimCloseEnd()
		{
		}
	}
}
