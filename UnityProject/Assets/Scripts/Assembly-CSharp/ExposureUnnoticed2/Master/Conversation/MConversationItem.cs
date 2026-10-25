using UnityEngine;

namespace ExposureUnnoticed2.Master.Conversation
{
	[CreateAssetMenu(fileName = "MConversationItem", menuName = "ScriptableObjects/MConversationItem")]
	public class MConversationItem : ScriptableObject
	{
		public string TalkerNameKey;

		public string MessageKey;

		public ConversationParamKeyType[] ParamKeyTypes;

		public ConversationEventType eventType;

		public MConversationItem[] Next;

		public ConversationDialogButtonModel[] DialogItem;

		public ConversationConditionalBranchType BranchType;

		public ConversationItemType ItemType => ConversationItemType.Message;
	}
}
