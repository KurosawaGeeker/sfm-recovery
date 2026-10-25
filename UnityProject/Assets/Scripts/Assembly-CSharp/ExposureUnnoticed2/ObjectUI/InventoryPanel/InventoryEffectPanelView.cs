using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel
{
	public class InventoryEffectPanelView : MonoBehaviour
	{
		[SerializeField]
		private Transform effectItemParent;

		[SerializeField]
		private GameObject effectItemPrefab;

		[SerializeField]
		private GameObject effectBarPrefab;

		public void Refresh(IEffectiveEquipRecord currentSelect, bool isNoReinforce = false)
		{
		}
	}
}
