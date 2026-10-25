using System;
using ExposureUnnoticed2.Master.Conversation;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ConversationPanel
{
	public class DialogItemView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI text;

		private Action<ConversationDialogButtonModel> onClick;

		private ConversationDialogButtonModel model;

		public void Initialize(ConversationDialogButtonModel model, Action<ConversationDialogButtonModel> onClick)
		{
		}

		public void OnClick()
		{
		}
	}
}
