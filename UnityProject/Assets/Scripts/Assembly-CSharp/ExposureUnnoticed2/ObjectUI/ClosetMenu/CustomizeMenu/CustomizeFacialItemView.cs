using Common.CommonUI.CommonSettingItem;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu
{
	public class CustomizeFacialItemView : MonoBehaviour
	{
		public static string[] FacialNames = new string[] { "ui_Closet_facial_type_1", "ui_Closet_facial_type_2", "ui_Closet_facial_type_3", "ui_Closet_facial_type_4", "ui_Closet_facial_type_5", "ui_Closet_facial_type_6", "ui_Closet_facial_type_7", "ui_Closet_facial_type_8", "ui_Closet_facial_type_9", "ui_Closet_facial_type_10", "ui_Closet_facial_type_11", "ui_Closet_facial_type_12" };

		[SerializeField]
		private SettingSelectItemView selectView;

		private int targetId;

		private ButtonView buttonView;

		public void Initialize(int targetId, ButtonGroupManager buttonGroupManager)
		{
buttonView = GetComponent<global::HadakaCoat.ObjectsUi.Common.Button.ButtonView>();
this.targetId = targetId;
selectView.Initialize(index =>
{
    Set(index);
    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerFaceController.OverrideFaceId = Get();
    buttonGroupManager.ForceSelect(buttonView);
}, "", FacialNames, Get());
}

		public void OnClick()
		{
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerFaceController.OverrideFaceId = Get();
}

		private void Set(int index)
		{
switch (targetId)
{
    case 0: global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace0Id = index; break;
    case 1: global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace1Id = index; break;
    case 2: global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace2Id = index; break;
    case 3: global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace3Id = index; break;
    case 4: global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFaceIkuId = index; break;
    case 5: global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFaceIkuAfterId = index; break;
}
}

		private int Get()
		{
return targetId switch
{
    0 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace0Id,
    1 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace1Id,
    2 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace2Id,
    3 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace3Id,
    4 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFaceIkuId,
    5 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFaceIkuAfterId,
};
}
	}
}
