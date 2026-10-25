using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu
{
	public class CustomizeFacialPanelView : MonoBehaviour
	{
		[SerializeField]
		private CustomizeFacialItemView face0;

		[SerializeField]
		private CustomizeFacialItemView face1;

		[SerializeField]
		private CustomizeFacialItemView face2;

		[SerializeField]
		private CustomizeFacialItemView face3;

		[SerializeField]
		private CustomizeFacialItemView faceIku;

		[SerializeField]
		private CustomizeFacialItemView faceIkuAfter;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private ButtonView firstButton;

		public void Initialize()
		{
buttonGroupManager.ReSearchButton();
face0.Initialize(0, buttonGroupManager);
face1.Initialize(1, buttonGroupManager);
face2.Initialize(2, buttonGroupManager);
face3.Initialize(3, buttonGroupManager);
faceIku.Initialize(4, buttonGroupManager);
faceIkuAfter.Initialize(5, buttonGroupManager);
}

		public void Open()
		{
gameObject.SetActive(true); buttonGroupManager.ForceSelect(firstButton); face0.OnClick();
}

		public void Hide()
		{
// Complete original native body draft; native/CLR acceptance pending.
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerFaceController.OverrideFaceId = -1; gameObject.SetActive(false);
}
	}
}
