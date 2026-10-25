using System.Collections.Generic;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel
{
	public class InventoryPanelLoadoutView : MonoBehaviour
	{
		public const int MaxLoadoutNum = 20;

		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private Transform descriptionParent;

		[SerializeField]
		private GameObject barPrefab;

		[SerializeField]
		private GameObject descriptionItemPrefab;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private CanvasGroup canvasGroup;

		public List<InventoryLoadoutItemView> ItemList;

		private InventoryPanelView parentView;

		public void Initialize(InventoryPanelView parentView)
		{
            // Complete original RVA 0x4EC9D0, both linked unwind records.
            this.parentView = parentView;
            canvasGroup = GetComponent<CanvasGroup>();
            global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(descriptionParent);
            global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(itemParent);
            ItemList.Clear();
            for (int i = 0; i < 20; ++i)
            {
                var row = UnityEngine.Object.Instantiate(itemPrefab, itemParent).GetComponent<InventoryLoadoutItemView>();
                row.Initialize(i, OnClickItem, OnClickEquip, OnClickRegister);
                ItemList.Add(row);
            }
            buttonGroupManager.ReSearchButton(false, false);
        }

		private void OnClickItem(int index)
		{
		}

		private void OnClickEquip(int index)
		{
		}

		private void OnClickRegister(int index)
		{
		}

		public void Show()
		{
            // Recovered complete original RVA 0x4EE880.
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
            parentView.currentSelectIndex = 0;
            parentView.ToPadModeIfNeed(false);
        }

		public void Hide()
		{
            // Recovered complete original RVA 0x4EE970.
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
		// Complete original constructor RVA 0x4EEA30.
		public InventoryPanelLoadoutView()
		{
            ItemList = new List<InventoryLoadoutItemView>();
		}

	}
}
