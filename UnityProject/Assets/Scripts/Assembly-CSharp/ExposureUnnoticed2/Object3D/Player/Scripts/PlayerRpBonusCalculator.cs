using ExposureUnnoticed2.Master.Stage;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerRpBonusCalculator
	{
		private PlayerClassAccessor pca;

		private float pathCalcTime;

		private int lastClothesRpBonus;

		private StageType currentStageType;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x658B10
			this.pca = pca;
			string prefix = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_skill_prefix");
			// Original native allocation/add position 0, buff key 1.
			var skill0 = global::ExposureUnnoticed2.Master.Skill.MSkill.Get((global::ExposureUnnoticed2.Master.Skill.SkillType)5);
			var buffs0 = pca.PlayerState.OtherBuffDict;
			var model0 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model0.SourceName = prefix + global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(skill0.NameKey);
			model0.RpBonus100 = skill0.RpBonus;
			buffs0.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)1, model0);
			// Original native allocation/add position 1, buff key 2.
			var skill1 = global::ExposureUnnoticed2.Master.Skill.MSkill.Get((global::ExposureUnnoticed2.Master.Skill.SkillType)10);
			var buffs1 = pca.PlayerState.OtherBuffDict;
			var model1 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model1.SourceName = prefix + global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(skill1.NameKey);
			model1.RpBonus100 = skill1.RpBonus;
			buffs1.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)2, model1);
			// Original native allocation/add position 2, buff key 67.
			var skill2 = global::ExposureUnnoticed2.Master.Skill.MSkill.Get((global::ExposureUnnoticed2.Master.Skill.SkillType)20);
			var buffs2 = pca.PlayerState.OtherBuffDict;
			var model2 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model2.SourceName = prefix + global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(skill2.NameKey);
			model2.RpBonus100 = skill2.RpBonus;
			buffs2.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)67, model2);
			// Original native allocation/add position 3, buff key 66.
			var skill3 = global::ExposureUnnoticed2.Master.Skill.MSkill.Get((global::ExposureUnnoticed2.Master.Skill.SkillType)19);
			var buffs3 = pca.PlayerState.OtherBuffDict;
			var model3 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model3.SourceName = prefix + global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(skill3.NameKey);
			model3.RpBonus100 = skill3.RpBonus;
			buffs3.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)66, model3);
			// Original native allocation/add position 4, buff key 32.
			var skill4 = global::ExposureUnnoticed2.Master.Skill.MSkill.Get((global::ExposureUnnoticed2.Master.Skill.SkillType)13);
			var buffs4 = pca.PlayerState.OtherBuffDict;
			var model4 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model4.SourceName = prefix + global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(skill4.NameKey);
			model4.RpBonus100 = skill4.RpBonus;
			buffs4.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)32, model4);
			// Original native allocation/add position 5, buff key 33.
			var skill5 = global::ExposureUnnoticed2.Master.Skill.MSkill.Get((global::ExposureUnnoticed2.Master.Skill.SkillType)7);
			var buffs5 = pca.PlayerState.OtherBuffDict;
			var model5 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model5.SourceName = prefix + global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(skill5.NameKey);
			model5.RpBonus100 = skill5.RpBonus;
			buffs5.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)33, model5);
			// Original native allocation/add position 6, buff key 55.
			var skill6 = global::ExposureUnnoticed2.Master.Skill.MSkill.Get((global::ExposureUnnoticed2.Master.Skill.SkillType)9);
			var buffs6 = pca.PlayerState.OtherBuffDict;
			var model6 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model6.SourceName = prefix + global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(skill6.NameKey);
			model6.RpBonus100 = skill6.RpBonus;
			buffs6.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)55, model6);
			// Original native allocation/add position 7, buff key 48.
			var skill7 = global::ExposureUnnoticed2.Master.Skill.MSkill.Get((global::ExposureUnnoticed2.Master.Skill.SkillType)18);
			var buffs7 = pca.PlayerState.OtherBuffDict;
			var model7 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model7.SourceName = prefix + global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(skill7.NameKey);
			model7.RpBonus100 = skill7.RpBonus;
			buffs7.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)48, model7);
			// Original native allocation/add position 8, buff key 10.
			var buffs8 = pca.PlayerState.OtherBuffDict;
			var model8 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model8.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_expose");
			buffs8.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)10, model8);
			// Original native allocation/add position 9, buff key 9.
			var buffs9 = pca.PlayerState.OtherBuffDict;
			var model9 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model9.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_near_npc");
			buffs9.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)9, model9);
			// Original native allocation/add position 10, buff key 11.
			var buffs10 = pca.PlayerState.OtherBuffDict;
			var model10 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model10.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_light");
			model10.RpBonus100 = global::ExposureUnnoticed2.Master.Common.MCommon.Instance.LightRpBonus;
			buffs10.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)11, model10);
			// Original native allocation/add position 11, buff key 12.
			var buffs11 = pca.PlayerState.OtherBuffDict;
			var model11 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model11.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_away_clothes");
			buffs11.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)12, model11);
			// Original native allocation/add position 12, buff key 8.
			var buffs12 = pca.PlayerState.OtherBuffDict;
			var model12 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model12.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_daytime");
			model12.RpBonus100 = global::ExposureUnnoticed2.Master.Common.MCommon.Instance.DaytimeRpBonus;
			buffs12.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)8, model12);
			// Original native allocation/add position 13, buff key 37.
			var buffs13 = pca.PlayerState.OtherBuffDict;
			var model13 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model13.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_peeing");
			model13.RpBonus100 = 10;
			buffs13.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)37, model13);
			// Original native allocation/add position 14, buff key 26.
			var buffs14 = pca.PlayerState.OtherBuffDict;
			var model14 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model14.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_fear3");
			model14.RpBonus100 = 20;
			buffs14.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)26, model14);
			// Original native allocation/add position 15, buff key 27.
			var buffs15 = pca.PlayerState.OtherBuffDict;
			var model15 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model15.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_fear4");
			model15.RpBonus100 = 10;
			buffs15.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)27, model15);
			// Original native allocation/add position 16, buff key 28.
			var buffs16 = pca.PlayerState.OtherBuffDict;
			var model16 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model16.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_fear5");
			model16.RpBonus100 = 0;
			buffs16.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)28, model16);
			// Original native allocation/add position 17, buff key 38.
			var buffs17 = pca.PlayerState.OtherBuffDict;
			var model17 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model17.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_washer", "1");
			model17.RpBonus100 = 10;
			buffs17.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)38, model17);
			// Original native allocation/add position 18, buff key 39.
			var buffs18 = pca.PlayerState.OtherBuffDict;
			var model18 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model18.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_washer", "2");
			model18.RpBonus100 = 0;
			buffs18.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)39, model18);
			// Original native allocation/add position 19, buff key 40.
			var buffs19 = pca.PlayerState.OtherBuffDict;
			var model19 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model19.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_washer", "3");
			model19.RpBonus100 = 15;
			buffs19.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)40, model19);
			// Original native allocation/add position 20, buff key 41.
			var buffs20 = pca.PlayerState.OtherBuffDict;
			var model20 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model20.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_washer", "4");
			model20.RpBonus100 = 0;
			buffs20.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)41, model20);
			// Original native allocation/add position 21, buff key 42.
			var buffs21 = pca.PlayerState.OtherBuffDict;
			var model21 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model21.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_washer", "5");
			model21.RpBonus100 = 20;
			buffs21.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)42, model21);
			// Original native allocation/add position 22, buff key 44.
			var buffs22 = pca.PlayerState.OtherBuffDict;
			var model22 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model22.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_invisible");
			model22.RpBonusLastMult100 = 20;
			buffs22.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)44, model22);
			// Original native allocation/add position 23, buff key 45.
			var buffs23 = pca.PlayerState.OtherBuffDict;
			var model23 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model23.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_timer_handcuff", "1");
			buffs23.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)45, model23);
			// Original native allocation/add position 24, buff key 46.
			var buffs24 = pca.PlayerState.OtherBuffDict;
			var model24 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model24.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_timer_handcuff", "3");
			buffs24.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)46, model24);
			// Original native allocation/add position 25, buff key 47.
			var buffs25 = pca.PlayerState.OtherBuffDict;
			var model25 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model25.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_timer_handcuff", "5");
			buffs25.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)47, model25);
			// Original native allocation/add position 26, buff key 49.
			var buffs26 = pca.PlayerState.OtherBuffDict;
			var model26 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model26.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_random_mission", "5");
			buffs26.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)49, model26);
			// Original native allocation/add position 27, buff key 50.
			var buffs27 = pca.PlayerState.OtherBuffDict;
			var model27 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model27.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_bareyasusa");
			buffs27.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)50, model27);
			// Original native allocation/add position 28, buff key 51.
			var buffs28 = pca.PlayerState.OtherBuffDict;
			var model28 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model28.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_bad_visible");
			model28.RpBonus100 = 40;
			buffs28.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)51, model28);
			// Original native allocation/add position 29, buff key 52.
			var buffs29 = pca.PlayerState.OtherBuffDict;
			var model29 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model29.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_handcuff");
			model29.RpBonus100 = 10;
			buffs29.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)52, model29);
			// Original native allocation/add position 30, buff key 53.
			var buffs30 = pca.PlayerState.OtherBuffDict;
			var model30 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model30.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_handcuff_at_map");
			model30.RpBonus100 = 20;
			buffs30.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)53, model30);
			// Original native allocation/add position 31, buff key 54.
			var buffs31 = pca.PlayerState.OtherBuffDict;
			var model31 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model31.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_body_paint");
			model31.RpBonusLastMult100 = 70;
			buffs31.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)54, model31);
			// Original native allocation/add position 32, buff key 56.
			var buffs32 = pca.PlayerState.OtherBuffDict;
			var model32 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model32.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_in_npc_sight");
			model32.RpBonus100 = 10;
			buffs32.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)56, model32);
			// Original native allocation/add position 33, buff key 57.
			var buffs33 = pca.PlayerState.OtherBuffDict;
			var model33 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			var stage33 = global::ExposureUnnoticed2.Master.Stage.MStage.Get((global::ExposureUnnoticed2.Master.Stage.StageType)10);
			model33.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_continuous_bonus", global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(stage33.StageNameKey));
			buffs33.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)57, model33);
			// Original native allocation/add position 34, buff key 58.
			var buffs34 = pca.PlayerState.OtherBuffDict;
			var model34 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			var stage34 = global::ExposureUnnoticed2.Master.Stage.MStage.Get((global::ExposureUnnoticed2.Master.Stage.StageType)14);
			model34.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_continuous_bonus", global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(stage34.StageNameKey));
			buffs34.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)58, model34);
			// Original native allocation/add position 35, buff key 59.
			var buffs35 = pca.PlayerState.OtherBuffDict;
			var model35 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			var stage35 = global::ExposureUnnoticed2.Master.Stage.MStage.Get((global::ExposureUnnoticed2.Master.Stage.StageType)6);
			model35.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_continuous_bonus", global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(stage35.StageNameKey));
			buffs35.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)59, model35);
			// Original native allocation/add position 36, buff key 60.
			var buffs36 = pca.PlayerState.OtherBuffDict;
			var model36 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			var stage36 = global::ExposureUnnoticed2.Master.Stage.MStage.Get((global::ExposureUnnoticed2.Master.Stage.StageType)12);
			model36.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_continuous_bonus", global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(stage36.StageNameKey));
			buffs36.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)60, model36);
			// Original native allocation/add position 37, buff key 61.
			var buffs37 = pca.PlayerState.OtherBuffDict;
			var model37 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			var stage37 = global::ExposureUnnoticed2.Master.Stage.MStage.Get((global::ExposureUnnoticed2.Master.Stage.StageType)15);
			model37.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_continuous_bonus", global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(stage37.StageNameKey));
			buffs37.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)61, model37);
			// Original native allocation/add position 38, buff key 62.
			var buffs38 = pca.PlayerState.OtherBuffDict;
			var model38 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			var stage38 = global::ExposureUnnoticed2.Master.Stage.MStage.Get((global::ExposureUnnoticed2.Master.Stage.StageType)11);
			model38.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_continuous_bonus", global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(stage38.StageNameKey));
			buffs38.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)62, model38);
			// Original native allocation/add position 39, buff key 63.
			var buffs39 = pca.PlayerState.OtherBuffDict;
			var model39 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			var stage39 = global::ExposureUnnoticed2.Master.Stage.MStage.Get((global::ExposureUnnoticed2.Master.Stage.StageType)7);
			model39.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_continuous_bonus", global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(stage39.StageNameKey));
			buffs39.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)63, model39);
			// Original native allocation/add position 40, buff key 65.
			var buffs40 = pca.PlayerState.OtherBuffDict;
			var model40 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model40.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_difficulty", global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("difficulty_casual"));
			model40.RpBonusLastMult100 = 80;
			buffs40.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)65, model40);
			// Original native allocation/add position 41, buff key 64.
			var buffs41 = pca.PlayerState.OtherBuffDict;
			var model41 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model41.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_difficulty", global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("difficulty_easy"));
			model41.RpBonusLastMult100 = 80;
			buffs41.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)64, model41);
			// Original native allocation/add position 42, buff key 68.
			var buffs42 = pca.PlayerState.OtherBuffDict;
			var model42 = new global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffModel();
			model42.SourceName = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("rp_buff_ecstasyMotion");
			model42.RpBonus100 = 10;
			buffs42.Add((global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)68, model42);
		}

		public void OnUpdate()
        {
            // Complete original GameAssembly.dll 0x65AE60..0x65CD48, including original switch data.
            unchecked
            {
                int sum = 100 + pca.PlayerState.CosplayRpBonus100 + pca.PlayerState.ReinforceEffectData.RpBonus100 + pca.PlayerState.AdultGoodsRpBonus100;
                var buffs = pca.PlayerState.OtherBuffDict;
                foreach (var pair in buffs)
                    if ((int)pair.Key != 12 && (int)pair.Key != 36) pair.Value.IsActive = false;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)8].IsActive = true;
                if (pca.PlayerState.IsPeeing) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)37].IsActive = true;
                int nearCount = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.NpcManager.CalcInRangeNpcNum(1.5f);
                int near = (nearCount < 5 ? nearCount : 5) * global::ExposureUnnoticed2.Master.Common.MCommon.Instance.NearNpcPerOneRpBonus;
                if (near > 0)
                {
                    buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)9].IsActive = true;
                    buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)9].RpBonus100 = near;
                }
                int exposed = (int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.GetExposedBodyPartsByClothesAndCostume();
                int exposeBonus = 0;
                if ((exposed & 2) != 0) exposeBonus += global::ExposureUnnoticed2.Master.Common.MCommon.Instance.ExposeFrontRpBonus;
                if ((exposed & 8) != 0) exposeBonus += global::ExposureUnnoticed2.Master.Common.MCommon.Instance.ExposeUpperRpBonus;
                if ((exposed & 4) != 0) exposeBonus += global::ExposureUnnoticed2.Master.Common.MCommon.Instance.ExposeHipRpBonus;
                else if ((exposed & 32) != 0) exposeBonus += global::ExposureUnnoticed2.Master.Common.MCommon.Instance.ExposeAllRpBonus;
                if (exposeBonus > 0)
                {
                    buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)10].IsActive = true;
                    buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)10].RpBonus100 = exposeBonus;
                }
                float light = 0f;
                foreach (var lamp in pca.PlayerState.TouchedLightList) light += lamp.CalcBareyasusa(pca.GameObject);
                if (light >= 0.5f && !global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)11].IsActive = true;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)44].IsActive = true;
                if (global::UnityEngine.Time.time > pathCalcTime)
                {
                    pathCalcTime = global::UnityEngine.Time.time + 0.5f;
                    float distance = float.MaxValue;
                    var clothes = pca.DroppedItemManager.ClothesTypes;
                    for (int i = 0; i < clothes.Length; ++i)
                    {
                        var drop = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[clothes[i]];
                        if (drop.IsDropped && drop.StageType == global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType)
                        {
                            float candidate = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.CalcPathLength(drop.Position, pca.Transform.position);
                            if (!(candidate > distance)) distance = candidate;
                        }
                    }
                    if (float.MaxValue > distance) distance = 60f > distance ? distance : 60f;
                    lastClothesRpBonus = ((int)distance / 10) * global::ExposureUnnoticed2.Master.Common.MCommon.Instance.ClothesDistanceRpBonus;
                }
                if (lastClothesRpBonus > 0 && (int)pca.PlayerState.HiddenBodyPartsByCostume == 0)
                {
                    buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)12].IsActive = true;
                    buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)12].RpBonus100 = lastClothesRpBonus;
                }
                else buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)12].IsActive = false;
                int fear = 28 - (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFear;
                buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)(fear > 23 ? fear : 23)].IsActive = true;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel((global::ExposureUnnoticed2.Master.Skill.SkillType)5) > 0 && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerPassiveSkillActiveDict[(global::ExposureUnnoticed2.Master.Skill.SkillType)5]) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)1].IsActive = true;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel((global::ExposureUnnoticed2.Master.Skill.SkillType)10) > 0 && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerPassiveSkillActiveDict[(global::ExposureUnnoticed2.Master.Skill.SkillType)10]) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)2].IsActive = true;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel((global::ExposureUnnoticed2.Master.Skill.SkillType)20) > 0 && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerPassiveSkillActiveDict[(global::ExposureUnnoticed2.Master.Skill.SkillType)20]) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)67].IsActive = true;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel((global::ExposureUnnoticed2.Master.Skill.SkillType)19) > 0 && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerPassiveSkillActiveDict[(global::ExposureUnnoticed2.Master.Skill.SkillType)19]) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)66].IsActive = true;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel((global::ExposureUnnoticed2.Master.Skill.SkillType)13) > 0 && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerPassiveSkillActiveDict[(global::ExposureUnnoticed2.Master.Skill.SkillType)13]) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)32].IsActive = true;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel((global::ExposureUnnoticed2.Master.Skill.SkillType)7) > 0 && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerPassiveSkillActiveDict[(global::ExposureUnnoticed2.Master.Skill.SkillType)7]) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)33].IsActive = true;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel((global::ExposureUnnoticed2.Master.Skill.SkillType)9) > 0 && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerPassiveSkillActiveDict[(global::ExposureUnnoticed2.Master.Skill.SkillType)9]) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)55].IsActive = true;
                if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetSkillLevel((global::ExposureUnnoticed2.Master.Skill.SkillType)18) > 0 && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerPassiveSkillActiveDict[(global::ExposureUnnoticed2.Master.Skill.SkillType)18]) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)48].IsActive = true;
                if (global::ExposureUnnoticed2.Scripts.InGame.SceneProcessManager.LaundryManager.Instance)
                {
                    var source = global::ExposureUnnoticed2.Scripts.InGame.SceneProcessManager.LaundryManager.Instance.GetRpBuffSourceType();
                    if ((int)source != -1) buffs[source].IsActive = true;
                }
                if ((global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachHandcuffs || global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachMapHandcuffs) && (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AttachedHandcuffType == 5 && global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerHandcuffTimerRemainTime > 0f)
                {
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerHandcuffTimerSetTime == 1)
                    {
                        buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)45].IsActive = true;
                        buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)45].RpBonus100 = 10;
                        if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachMapHandcuffs) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)45].RpBonus100 *= 2;
                    }
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerHandcuffTimerSetTime == 3)
                    {
                        buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)46].IsActive = true;
                        buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)46].RpBonus100 = 15;
                        if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachMapHandcuffs) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)46].RpBonus100 *= 2;
                    }
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerHandcuffTimerSetTime == 5)
                    {
                        buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)47].IsActive = true;
                        buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)47].RpBonus100 = 20;
                        if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachMapHandcuffs) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)47].RpBonus100 *= 2;
                    }
                }
                if (global::ExposureUnnoticed2.Scripts.InGame.RandomMissionManager.Instance.IsActiveMission)
                {
                    buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)49].IsActive = true;
                    buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)49].RpBonus100 = global::ExposureUnnoticed2.Scripts.InGame.RandomMissionManager.Instance.RpBonus;
                }
                int bareBonus = ((int)(pca.PlayerState.BaseBareyasusa * 100f) - 100) / 5;
                if (bareBonus > 0)
                {
                    buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)50].IsActive = true;
                    buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)50].RpBonus100 = bareBonus;
                }
                if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AttachedHandcuffType == 3)
                {
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachHandcuffs) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)52].IsActive = true;
                    else if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachMapHandcuffs) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)53].IsActive = true;
                }
                if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.VisibilityFogController.IsBadVisibility) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)51].IsActive = true;
                if (pca.PlayerState.IsEcstasyMotion) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)68].IsActive = true;
                if (pca.BodyPaintController.IsPainted()) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)54].IsActive = true;
                if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible && global::ExposureUnnoticed2.Object3D.NPC.Script.NpcManager.Instance.IsSomeOnePlayerFound) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)56].IsActive = true;
                if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType == 1) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)64].IsActive = true;
                if ((int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType == 0) buffs[(global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)65].IsActive = true;
                foreach (var pair in global::ExposureUnnoticed2.Master.Stage.MStage.GetAll())
                {
                    if (pair.Key != global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType) continue;
                    var stage = pair.Value.StageType;
                    global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.AchieveMissionCountByStageThisGameDict.TryAdd(stage, 0);
                    int count = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.AchieveMissionCountByStageThisGameDict[stage];
                    if (count < 5) continue;
                    int bonus = (count / 5) * 5;
                    if (bonus > 20) bonus = 20;
                    global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType source;
                    switch ((int)stage)
                    {
                        case 6: source = (global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)59; break;
                        case 7: source = (global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)63; break;
                        case 10: source = (global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)57; break;
                        case 11: source = (global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)62; break;
                        case 12: source = (global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)60; break;
                        case 14: source = (global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)58; break;
                        case 15: source = (global::ExposureUnnoticed2.Object3D.Player.Scripts.Other.BuffSourceType)61; break;
                        default: throw new global::System.Runtime.CompilerServices.SwitchExpressionException(stage);
                    }
                    var model = buffs[source];
                    model.IsActive = true;
                    model.RpBonus100 = bonus;
                }
                foreach (var pair in buffs) if (pair.Value.IsActive) sum += pair.Value.RpBonus100;
                float total = (float)pca.PlayerState.RpBonusByVibratorPerSecond * pca.PlayerState.VibratorRpBonusScale + (float)sum;
                total += (float)pca.PlayerState.RpBonusByPistonMachinePerSecond * pca.PlayerState.PistonRpBonusScale;
                foreach (var pair in buffs) if (pair.Value.IsActive && pair.Value.RpBonusLastMult100 != 0) total *= (float)pair.Value.RpBonusLastMult100 * 0.01f;
                float ratio = total / 100f;
                pca.PlayerState.RpBonus = 0f > ratio ? 0f : ratio;
                pca.PlayerState.RpBonusInt = 0f > total ? 0f : total;
                float oldMaximum = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.StatistisMaxRpBoost;
                float current = pca.PlayerState.RpBonus;
                global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.StatistisMaxRpBoost = oldMaximum > current ? oldMaximum : current;
            }
        }
	// Recovered original member RVA 0x65CD50: .ctor
		public PlayerRpBonusCalculator()
        {
            currentStageType = (global::ExposureUnnoticed2.Master.Stage.StageType)2;
        }
	}
}
