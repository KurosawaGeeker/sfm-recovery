using ExposureUnnoticed2.Master.AdultGoods;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerAdultGoodsController
	{
		private PlayerClassAccessor pca;

		private bool oldIsGaman;

		public bool IsSlideManko { get; private set; }

		public bool IsSlideAnal { get; private set; }

		public bool IsActiveEyeMask { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x627FC0
			this.pca = pca;
			foreach (var pair in global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.GetAll())
			{
			    var obj = pca.PlayerAvatarObjectReferencer.Accessorys[(int)pair.Key];
			    obj.SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipAdultGoods(pair.Value.Type));
			}
			CalcAdultGoodsEffect();
			global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeAdultGoodsEvent);
		}

		public void OnUpdate()
		{
		}

		public void ChangeAdultGoods(MAdultGoodsType type, bool isActive)
		{
            // Recovered complete original native body.
            pca.PlayerAvatarObjectReferencer.Accessorys[(int)type].SetActive(isActive);
            global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.UpdateAdultGoodsState(type, isActive);
            CalcAdultGoodsEffect();
            global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeAdultGoodsEvent);
        }

		private void CalcAdultGoodsEffect()
		{
			// Recovered from GameAssembly.dll RVA 0x6288B0
			IsSlideManko = false;
			IsSlideAnal = false;
			IsActiveEyeMask = false;
			pca.PlayerState.AdultGoodsBuffList.Clear();
			float vibratorRise = 0f;
			float pistonRise = 0f;
			foreach (var pair in global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.MAdultGoodsDict)
			{
			    if (!pair.Value) continue;
			    var record = global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.Get(pair.Key);
			    if ((int)record.AdultGoodsEffectType == 1) vibratorRise += record.EcstasyRisePerSecond;
			    else if ((int)record.AdultGoodsEffectType == 3) pistonRise += record.EcstasyRisePerSecond;
			    else if ((int)record.AdultGoodsEffectType == 2) IsActiveEyeMask = true;
			    var buff = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			    buff.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(record.NameKey);
			    buff.RpBonus100 = record.RpBonus;
			    buff.Strange100 = record.StrangeBonus;
			    buff.Dokidoki100 = record.DokidokiBonus;
			    buff.BuffSourceType = (global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)4;
			    buff.ItemIdParseInt = (int)record.Type;
			    if ((int)record.AdultGoodsEffectType == 1) buff.SpecialType = (global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.SpecialType)1;
			    if ((int)record.AdultGoodsEffectType == 3) buff.SpecialType = (global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.SpecialType)2;
			    pca.PlayerState.AdultGoodsBuffList.Add(buff);
			    if (record.SlideManko) IsSlideManko = true;
			    if (record.SlideAnal) IsSlideAnal = true;
			}
			int normalRp = 0, vibratorRp = 0, pistonRp = 0, strange = 0, dokidoki = 0;
			foreach (var buff in pca.PlayerState.AdultGoodsBuffList)
			{
			    if ((int)buff.SpecialType == 1) vibratorRp = unchecked(vibratorRp + buff.RpBonus100);
			    else if ((int)buff.SpecialType == 2) pistonRp = unchecked(pistonRp + buff.RpBonus100);
			    else normalRp = unchecked(normalRp + buff.RpBonus100);
			    strange = unchecked(strange + buff.Strange100);
			    dokidoki = unchecked(dokidoki + buff.Dokidoki100);
			}
			pca.PlayerState.AdultGoodsRpBonus100 = normalRp;
			float visibilityBonus = strange / 100f;
			pca.PlayerState.AddBareyasusaByAdultGoods = visibilityBonus <= 0.5f ? visibilityBonus : 0.5f;
			var capturedState = pca.PlayerState;
			float upper = global::ExposureUnnoticed2.Master.Common.MCommon.Instance.DokidokiAdultGoodsUpperLimit;
			float dokidokiBonus = dokidoki / 100f;
			if (dokidokiBonus < 0f) dokidokiBonus = 0f;
			else if (upper < dokidokiBonus) dokidokiBonus = upper;
			capturedState.DokidokiBonusByAdultGoods = dokidokiBonus;
			pca.PlayerState.EcstasyRiseByVibratorPerSecond = vibratorRise / 100f;
			pca.PlayerState.EcstasyRiseByPistonMachinePerSecond = pistonRise / 100f;
			pca.PlayerState.RpBonusByVibratorPerSecond = vibratorRp;
			pca.PlayerState.RpBonusByPistonMachinePerSecond = pistonRp;
			pca.PlayerReinforceEffectManager.Apply();
			global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.VisibilityFogController.UpdateEyeMask();
		}
	}
}
