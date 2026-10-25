using System.Collections.Generic;
using ExposureUnnoticed2.ObjectUI.InventoryPanel;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu
{
	public class CustomizeLoadoutView : MonoBehaviour
	{
		public const int MaxLoadoutNum = 20;

		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private List<InventoryLoadoutItemView> itemList = new List<InventoryLoadoutItemView>();

		private LoadoutCustomizeData currentCustomize;

		private int currentIndex = -1;

		public void Initialize()
		{
var snapshot = new LoadoutCustomizeData();
snapshot.SaveCurrentCustomize();
currentCustomize = snapshot;
global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(itemParent);
itemList.Clear();
for (int i = 0; i < 20; i++)
{
    var item = global::UnityEngine.Object.Instantiate(itemPrefab, itemParent).GetComponent<global::ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryLoadoutItemView>();
    item.Initialize(i, OnClickItem, OnClickEquip, OnClickRegister);
    itemList.Add(item);
}
buttonGroupManager.ReSearchButton();
}

		private void OnClickEquip(int index)
		{
		}

		private void OnClickRegister(int index)
		{
		}

		public void Open()
		{
currentCustomize.SaveCurrentCustomize(); gameObject.SetActive(true);
}

		public void Hide()
		{
// Complete original native body draft; native/CLR acceptance pending.
if (!gameObject.activeInHierarchy) return; currentIndex = -1; buttonGroupManager.ForceSelect((global::HadakaCoat.ObjectsUi.Common.Button.ButtonGroupManager.IButtonView)null); gameObject.SetActive(false); currentCustomize.ApplyToGameState();
}

		private void OnClickItem(int index)
		{
		}
	}
}
