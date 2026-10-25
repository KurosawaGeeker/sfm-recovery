using System;
using ExposureUnnoticed2.Master.Conversation;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ConversationPanel
{
	public class ConversationPanelView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI talkerName;

		[SerializeField]
		private TextMeshProUGUI message;

		[SerializeField]
		private Transform dialogParent;

		[SerializeField]
		private GameObject dialogItemPrefab;

		private MConversationItem currentViewItem;

		private Action onClose;

		private Animator animator;

		private bool isAlreadyCloseProcess;

		private void Initialize(MConversationRecord conversation, Action onClose)
		{
		}

		public static void Open(MConversationRecord conversation, Action onClose)
		{
		}

		public void Close()
		{
		}

		public void OnAnimCloseEnd()
		{
		}

		private void UpdateView()
		{
		}

		private string[] TransParam(ConversationParamKeyType[] types)
		{
			return null;
		}

		private void GotoNextConversation()
		{
		}

		private void ApplyConditionBranchIfNeed()
		{
		}

		private void LateUpdate()
		{
		}
	}
}
