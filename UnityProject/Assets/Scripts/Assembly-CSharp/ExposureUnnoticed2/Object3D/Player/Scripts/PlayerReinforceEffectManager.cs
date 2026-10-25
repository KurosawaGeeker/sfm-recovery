namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerReinforceEffectManager
	{
		private PlayerClassAccessor pca;

		private float nextUpdateTime;

		private SumReinforceEffectData cacheNewData;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.pca = pca;
		}

		public void OnUpdate()
		{
		}

		public void Apply()
		{
			// Recovered from GameAssembly.dll RVA 0x658230
			cacheNewData = default;
			if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)24))
			{
			    foreach (var pair in global::ExposureUnnoticed2.Master.AdultGoods.MAdultGoods.GetAll())
			        if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipAdultGoods(pair.Key)) CalcData(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AdultGoodsReinforceEffectDict[pair.Key]);
			    foreach (var pair in global::ExposureUnnoticed2.Master.Cosplay.MCosplay.GetAll())
			        foreach (var part in pair.Value.GetAll())
			            if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCosplayParts(part.UniqueId)) CalcData(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CosplayPartsReinforceEffectDict[part.UniqueId]);
			    cacheNewData.RpBonus100 = System.Math.Min(cacheNewData.RpBonus100, 30);
			    cacheNewData.ReduceFootStepSound100 = System.Math.Max(cacheNewData.ReduceFootStepSound100, -100);
			    cacheNewData.StaminaRegenerate100 = System.Math.Max(cacheNewData.StaminaRegenerate100, -100);
			    cacheNewData.UnlockSpeed100 = System.Math.Max(cacheNewData.UnlockSpeed100, -80);
			    cacheNewData.DashSpeed100 = System.Math.Max(cacheNewData.DashSpeed100, -99);
			}
			pca.PlayerState.ReinforceEffectData = cacheNewData;
		}

		private void CalcData(int[] effectData)
		{
			// Recovered from GameAssembly.dll RVA 0x658820
			foreach (int id in effectData)
			{
			    if (id == 0) continue;
			    var record = global::ExposureUnnoticed2.Master.ReinforceEffect.MReinforceEffect.Get(id);
			    if (record == null) continue;
			    switch ((int)record.ReinforceEffectType)
			    {
			        case 0: cacheNewData.RpBonus100 = unchecked(cacheNewData.RpBonus100 + record.EffectAmount100); break;
			        case 1: cacheNewData.Bareyasusa100 = unchecked(cacheNewData.Bareyasusa100 + record.EffectAmount100); break;
			        case 2: cacheNewData.Dokidoki100 = unchecked(cacheNewData.Dokidoki100 + record.EffectAmount100); break;
			        case 3: cacheNewData.Stamina100 = unchecked(cacheNewData.Stamina100 + record.EffectAmount100); break;
			        case 4: cacheNewData.EcstasyGauge100 = unchecked(cacheNewData.EcstasyGauge100 + record.EffectAmount100); break;
			        case 5: cacheNewData.MissionGauge100 = unchecked(cacheNewData.MissionGauge100 + record.EffectAmount100); break;
			        case 6: cacheNewData.SlowGauge100 = unchecked(cacheNewData.SlowGauge100 + record.EffectAmount100); break;
			        case 7: cacheNewData.SkillCoolTime100 = unchecked(cacheNewData.SkillCoolTime100 + record.EffectAmount100); break;
			        case 8: cacheNewData.GameOverPenaltyRp100 = unchecked(cacheNewData.GameOverPenaltyRp100 + record.EffectAmount100); break;
			        case 9: cacheNewData.GameOverPenaltyRank100 = unchecked(cacheNewData.GameOverPenaltyRank100 + record.EffectAmount100); break;
			        case 10: cacheNewData.MentalDecreaseScale100 = unchecked(cacheNewData.MentalDecreaseScale100 + record.EffectAmount100); break;
			        case 11: cacheNewData.DashSpeed100 = unchecked(cacheNewData.DashSpeed100 + record.EffectAmount100); break;
			        case 12: cacheNewData.StaminaRegenerate100 = unchecked(cacheNewData.StaminaRegenerate100 + record.EffectAmount100); break;
			        case 13: cacheNewData.ReduceFootStepSound100 = unchecked(cacheNewData.ReduceFootStepSound100 + record.EffectAmount100); break;
			        case 14: cacheNewData.UnlockSpeed100 = unchecked(cacheNewData.UnlockSpeed100 + record.EffectAmount100); break;
			        default: throw new System.ArgumentOutOfRangeException();
			    }
			}
		}
	}
}
