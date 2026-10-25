using System;
using ExposureUnnoticed2.Master.Clothes;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ClosetMenu
{
	public class ClosetMenuItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private GameObject equipTextObject;

		[SerializeField]
		private GameObject outsideMask;

		private Action<MClothesRecord> onClick;

		private Action onClickNaked;

		private MClothesRecord record;

		public void Initialize(MClothesRecord record, Action<MClothesRecord> onClick)
		{
// Complete original native body draft; native/CLR acceptance pending.

this.onClick = onClick;
this.record = record;
name.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(record.NameKey);
var data = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[(global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.DropItemType)((int)record.ClothesType == 0 ? 0 : (int)record.ClothesType == 1 ? 1 : -1)];
bool outside = data.IsDropped && (int)data.StageType != 0;
outsideMask.SetActive(outside);
if (outside) GetComponent<global::UnityEngine.UI.Button>().enabled = false;

}

		public void InitializeNaked(Action onClick)
		{
// Complete original native body draft; native/CLR acceptance pending.
onClickNaked = onClick; name.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("裸"); outsideMask.SetActive(false);
}

		public void OnClick()
		{
// Complete original native body draft; native/CLR acceptance pending.
onClickNaked?.Invoke(); onClick?.Invoke(record);
}

		public void Refresh(MClothesRecord current)
		{
// Complete original native body draft; native/CLR acceptance pending.
equipTextObject.SetActive(current == record || (record == null && (int)current.ClothesType == -1));
}
	}
}
