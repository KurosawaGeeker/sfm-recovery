using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Conversation
{
	[CreateAssetMenu(fileName = "MConversation", menuName = "ScriptableObjects/MConversation")]
	public class MConversation : ScriptableObject
	{
		[SerializeField]
		private MConversationRecord[] conversations;

		private static Dictionary<MConversationType, MConversationRecord> dict;

		private static void CheckInitialize()
		{
		}

		public static MConversationRecord Get(MConversationType type)
		{
			return null;
		}
	}
}
