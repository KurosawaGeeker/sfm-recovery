using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerTitController
	{
		private PlayerClassAccessor pca;

		private SmoothFloat chikubiBlend;

		private bool isClosetDisplay;

		private bool isEquipBra;

		private bool isFlattenTit;

		public int CurrentChikubiBlend => 0;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x662430
			this.pca = pca;
			global::ExposureUnnoticed2.Master.Common.MCommon.Instance.IsControlOppaiByInspector = false;
			global::ExposureUnnoticed2.Scripts.Base.EventManager.AddListener<global::ExposureUnnoticed2.Scripts.Base.ChangeCostumeCosplayEvent>(OnChangeCostumeCosplayEvent, false);
		}

		public void OnDispose()
		{
			// Recovered from GameAssembly.dll RVA 0x662550
			global::ExposureUnnoticed2.Scripts.Base.EventManager.RemoveListener<global::ExposureUnnoticed2.Scripts.Base.ChangeCostumeCosplayEvent>(OnChangeCostumeCosplayEvent);
		}

		public void OnUpdate()
		{
		}

		private void ChikubiUpdate()
		{
            // Recovered complete original native body.
            int value = 100;
            if ((int)pca.PlayerState.CurrentClothes.ClothesType == 0 && (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCurrentClothesAState == 0) value = 0;
            chikubiBlend.Current = value;
            float weight5;
            int weight7;
            if (isFlattenTit) { weight5 = 0f; weight7 = 0; }
            else
            {
                weight5 = chikubiBlend.Current;
                weight7 = UnityEngine.Mathf.Clamp(unchecked(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNipple * 2 - 100), 0, 100);
            }
            pca.PlayerAvatarObjectReferencer.bodyMeshRenderer.SetBlendShapeWeight(5, weight5);
            pca.PlayerAvatarObjectReferencer.bodyMeshRenderer.SetBlendShapeWeight(7, weight7);
            var renderer = pca.PlayerAvatarObjectReferencer.bodyMeshRenderer;
            int weight8 = isFlattenTit ? 0 : UnityEngine.Mathf.Clamp(unchecked((50 - global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNipple) * 2), 0, 100);
            renderer.SetBlendShapeWeight(8, weight8);
        }

		public void OnChangeCustomizeBar()
		{
			// Recovered from GameAssembly.dll RVA 0x663260
			OnChangeCostumeCosplayEvent(null);
		}

		private void OnChangeCostumeCosplayEvent(ChangeCostumeCosplayEvent evt)
		{
            // Recovered complete original native body.
            isEquipBra = false;
            isFlattenTit = false;
            foreach (var pair in global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCostumeDict)
            {
                if (pair.Value && (int)global::ExposureUnnoticed2.Master.Costume.MCostume.Get(pair.Key).CostumeCategory == 1) isEquipBra = true;
            }
            foreach (var pair in global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict)
            {
                if (!pair.Value) continue;
                var record = global::ExposureUnnoticed2.Master.Cosplay.MCosplay.GetParts(pair.Key);
                if (record.IsFlattenTit) isFlattenTit = true;
                if ((int)record.CostumeCategory == 1) isEquipBra = true;
            }
            if (global::ExposureUnnoticed2.ObjectUI.BarberMenu.ClosetMenuView.Instance && !global::ExposureUnnoticed2.ObjectUI.BarberMenu.ClosetMenuView.Instance.IsDisplayCostume) isEquipBra = false;
            ChikubiUpdate();
        }
	// Recovered original member RVA 0x663810: .ctor
		public PlayerTitController()
        {
            chikubiBlend = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 100f, 0f, false, false, -1f);
        }
	}
}
