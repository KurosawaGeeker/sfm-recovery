using System.Collections.Generic;
using ExposureUnnoticed2.ObjectUI.InGame.RingMenu;
using UnityEngine;

namespace ExposureUnnoticed2.Master.Action
{
	[CreateAssetMenu(fileName = "MAction", menuName = "ScriptableObjects/MAction")]
	public class MAction : ScriptableObject
	{
		public const int SexActionBias = 100000;

		[SerializeField]
		private MActionRecord[] actions;

		private static Dictionary<ActionType, MActionRecord> dict;

		private static void CheckInitialize()
		{
			// Recovered from GameAssembly.dll RVA 0x46CEE0
			if (dict != null) return;
			dict = new Dictionary<ActionType, MActionRecord>();
			foreach (MActionRecord record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MAction.actions)
			    dict.Add(record.Type, record);
		}

		public static MActionRecord Get(ActionType type)
		{
			// Recovered from GameAssembly.dll RVA 0x46D0B0
			CheckInitialize();
			return dict.TryGetValue(type, out MActionRecord record) ? record : null;
		}

		public static bool IsUseDildoAction(ActionType type)
		{
			return false;
		}

		public static bool IsUseDildoRiseEcstasyAction(ActionType type)
		{
			return false;
		}

		public static bool IsSitAction(ActionType type)
		{
			return false;
		}

		public static ActionType TransActionType(RingMenuParentView.RingItemType type)
		{
			return ActionType.OldOnaniNormal;
		}

		public static bool IsPoseSkill(ActionType type)
		{
			return false;
		}
	}
}
