using ExposureUnnoticed2.Master.Cosplay;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerCosplayManager
	{
		private PlayerClassAccessor pca;

		public bool IsSlideManko { get; private set; }

		public bool IsSlideAnal { get; private set; }

		public bool IsActiveEyeMask { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x6462C0
			this.pca = pca;
			foreach (var pair in global::ExposureUnnoticed2.Master.Cosplay.MCosplay.GetAll())
			{
			    var controllers = this.pca.PlayerAvatarObjectReferencer.CosplayControllers;
			    var controller = controllers[(int)pair.Value.Type];
			    foreach (var part in pair.Value.GetAll())
			    {
			        bool active = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCosplayParts(part.UniqueId);
			        if ((int)part.CostumeCategory == 0) this.pca.PlayerShoesController.ChangeCurrentShoes(part, active);
			        if (part.CanChangeInvisible && active) active = !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetCosplayPartsOptionData(part.UniqueId).invisible;
			        controller.SetActiveParts(part.Id, active);
			    }
			}
			CalcCostumeEffect();
			this.pca.PlayerBodyMaterialController.SetupBodyPaintFromGameState();
			global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeCostumeCosplayEvent);
		}

		public void SetupCostumeFromGameState()
		{
			// Recovered from GameAssembly.dll RVA 0x646320
			foreach (var pair in global::ExposureUnnoticed2.Master.Cosplay.MCosplay.GetAll())
			{
			    var controllers = this.pca.PlayerAvatarObjectReferencer.CosplayControllers;
			    var controller = controllers[(int)pair.Value.Type];
			    foreach (var part in pair.Value.GetAll())
			    {
			        bool active = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCosplayParts(part.UniqueId);
			        if ((int)part.CostumeCategory == 0) this.pca.PlayerShoesController.ChangeCurrentShoes(part, active);
			        if (part.CanChangeInvisible && active) active = !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetCosplayPartsOptionData(part.UniqueId).invisible;
			        controller.SetActiveParts(part.Id, active);
			    }
			}
			CalcCostumeEffect();
			this.pca.PlayerBodyMaterialController.SetupBodyPaintFromGameState();
			global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeCostumeCosplayEvent);
		}

		public void DeactivateAll()
		{
		}

		public bool IsSomeActiveCostume()
		{
			return false;
		}

		public void SetCosplayActive(CosplayType type, int id, bool isActive, bool isSkipCalcEffect = false)
		{
			var record = MCosplay.Get(type).Get(id);
			if ((int)record.CostumeCategory == 0) pca.PlayerShoesController.ChangeCurrentShoes(record, isActive);
			global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.SetActiveCosplayParts(record.UniqueId, isActive);
			if (record.CanChangeInvisible && isActive)
			    isActive = !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetCosplayPartsOptionData(record.UniqueId).invisible;
			pca.PlayerAvatarObjectReferencer.CosplayControllers[(int)type].SetActiveParts(id, isActive);
			if (!isSkipCalcEffect) CalcCostumeEffect();
			pca.PlayerBodyMaterialController.SetupBodyPaintFromGameState();
			global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeCostumeCosplayEvent);
			pca.PlayerMiscProcess.nextUpdateTimeHiddenBodyPartsByCostume = global::UnityEngine.Time.unscaledTime;
		}

		public void CalcCostumeEffect()
		{
			// Recovered from GameAssembly.dll RVA 0x646D50
			IsSlideManko = false;
			IsSlideAnal = false;
			IsActiveEyeMask = false;
			pca.PlayerState.CosplayBuffList.Clear();
			int rp = 0, strange = 0;
			bool hidePussy = false, hideAnal = false, hideInmou = false, hideFuta = false, offHip = false;
			pca.PlayerState.IsHatByCosplay = false;
			pca.PlayerState.CostumeBuffList.Clear();
			foreach (var pair in global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict)
			{
			    if (!pair.Value) continue;
			    var part = global::ExposureUnnoticed2.Master.Cosplay.MCosplay.GetParts(pair.Key);
			    rp = unchecked(rp + part.RpBonus);
			    int partStrange = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)24) ? 0 : part.StrangeBonus;
			    strange = unchecked(strange + partStrange);
			    if (part.SlideManko) IsSlideManko = true;
			    if (part.SlideAnal) IsSlideAnal = true;
			    if (part.IsEyeMaskEffect && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)24)) IsActiveEyeMask = true;
			    var buff = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			    buff.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(part.NameKey);
			    buff.RpBonus100 = part.RpBonus;
			    buff.Strange100 = partStrange;
			    buff.Dokidoki100 = part.DokidokiBonus;
			    buff.BuffSourceType = (global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)5;
			    buff.ItemIdParseInt = part.UniqueId;
			    pca.PlayerState.CosplayBuffList.Add(buff);
			    hidePussy = hidePussy || part.HideCensorPussy;
			    hideAnal = hideAnal || part.HideCensorAnal;
			    hideInmou = hideInmou || part.IsHideInmou;
			    hideFuta = hideFuta || part.IsHideFuta;
			    offHip = offHip || part.OffHipDynamicBone;
			    pca.PlayerState.IsHatByCosplay = part.IsHat || pca.PlayerState.IsHatByCosplay;
			}
			pca.PlayerState.IsHideFutaByCosplay = hideFuta;
			pca.PlayerState.CosplayRpBonus100 = rp;
			pca.PlayerState.AddBareyasusaByCosplay01 = strange / 100f;
			global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.VisibilityFogController.UpdateEyeMask();
			pca.PlayerAvatarObjectReferencer.CensorPussy.SetActive(!hidePussy);
			pca.PlayerAvatarObjectReferencer.CensorAnal.SetActive(!hideAnal);
			pca.PlayerAvatarObjectReferencer.InmouController.SetHide(hideInmou);
			pca.FutanariController.OnChangeCosplay();
			pca.PlayerState.IsOffHipDBFromCosplay = offHip;
			pca.PlayerReinforceEffectManager.Apply();
		}
	}
}
