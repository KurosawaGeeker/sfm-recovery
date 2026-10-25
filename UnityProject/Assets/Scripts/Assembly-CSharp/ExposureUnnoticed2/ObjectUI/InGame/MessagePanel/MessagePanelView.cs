using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.InGame.MessagePanel
{
	public class MessagePanelView : MonoBehaviour
	{
		public enum MessageType
		{
			Caution = 0,
			Log = 1,
			RankUp = 2,
			DroneMissionComplete = 3
		}

		private const int MaxItemNum = 5;

		private static MessagePanelView Instance;

		[SerializeField]
		private GameObject dummyMessagePrefab;

		[SerializeField]
		private GameObject messageItemPrefab;

		[SerializeField]
		private GameObject logItemPrefab;

		[SerializeField]
		private GameObject rankUpItemPrefab;

		[SerializeField]
		private GameObject droneMissionCompleteItemPrefab;

		[SerializeField]
		private VerticalLayoutGroup itemParent;

		private List<MessageItemView> itemList = new List<MessageItemView>();

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x50BF80
			global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(itemParent.transform);
			Instance = this;
		}

		private GameObject GetItemPrefab(MessageType type)
		{
			switch (type)
			{
			    case MessageType.Caution: return messageItemPrefab;
			    case MessageType.Log: return logItemPrefab;
			    case MessageType.RankUp: return rankUpItemPrefab;
			    case MessageType.DroneMissionComplete: return droneMissionCompleteItemPrefab;
			    default: return null;
			}
		}

		private void AddMessageInstance(string str, MessageType type)
		{
			var dummy = UnityEngine.Object.Instantiate(dummyMessagePrefab, itemParent.transform);
			var view = UnityEngine.Object.Instantiate(GetItemPrefab(type), transform).GetComponent<MessageItemView>();
			view.Initialize(str, dummy, item => itemList.Remove(item), type);
			itemList.Add(view);
			itemParent.CalculateLayoutInputVertical();
			itemParent.SetLayoutVertical();
			if (itemList.Count > MaxItemNum)
			{
			    var first = itemList[0];
			    itemList.Remove(first);
			    first.ForceClose();
			}
		}

		public static void AddMessage(string str, MessageType type = MessageType.Caution)
		{
if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.Message) return;
if (Instance is null) throw new System.NullReferenceException();
Instance.AddMessageInstance(str, type);
if ((int)type == 0)
{
    var sound = global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance;
    if (sound is null) throw new System.NullReferenceException();
    sound.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)69, -1f, false);
}
}
	}
}
