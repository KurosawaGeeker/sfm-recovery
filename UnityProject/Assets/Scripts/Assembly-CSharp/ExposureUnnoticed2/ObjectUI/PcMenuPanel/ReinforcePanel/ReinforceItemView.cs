using System;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.PcMenuPanel.ReinforcePanel
{
	public class ReinforceItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private GameObject equippedTextObject;

		[SerializeField]
		private CanvasGroup emitCanvasGroup;

		private Action<IReinforcableRecord> onClick;

		private IReinforcableRecord record;

		public void Initialize(IReinforcableRecord record, Action<IReinforcableRecord> onClick, bool isEquipped)
		{
		}

		public void OnClick()
		{
		}

		public void ForceSetEmit(int alpha)
		{
		}
	}
}
