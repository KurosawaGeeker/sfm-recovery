using UnityEngine;

namespace ExposureUnnoticed2.Master.Conversation
{
	[CreateAssetMenu(fileName = "MConversationRecord", menuName = "ScriptableObjects/MConversationRecord")]
	public class MConversationRecord : ScriptableObject
	{
		public MConversationType Type;

		public MConversationItem Start;
	}
}
