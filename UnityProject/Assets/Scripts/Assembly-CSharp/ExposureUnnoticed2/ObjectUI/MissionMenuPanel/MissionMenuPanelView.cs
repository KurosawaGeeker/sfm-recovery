using System;
using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.ObjectUI.CommonUI.CommonTab;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Scripts.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.MissionMenuPanel
{
	public class MissionMenuPanelView : BasePanelView
	{
		private const string HideName = "???";

		private StageType[] CvtStageTypeArray;

		[SerializeField]
		private Transform missionItemParent;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private CommonTabView tabView;

		[SerializeField]
		private TextMeshProUGUI[] tabNames;

		[SerializeField]
		private TextMeshProUGUI completeRate;

		[SerializeField]
		private TextMeshProUGUI ReleaseCondition;

		[SerializeField]
		private ScrollRect scrollRect;

		private new Action onClose;

		public static MissionMenuPanelView Instance { get; private set; }

		private void Initialize(Action onClose)
		{
			// Recovered original RVA 0x4DFEE0. The callback belongs to this panel,
			// not BasePanelView.onClose; OnClickClose invokes it before base Close.
			this.onClose = onClose;
			Instance = this;
			tabView.Initialize(OnChangeTab, null);
			int rank = PlayerController.Instance.Pca.PlayerState.CurrentRank.Rank;
			tabNames[0].text = Localizer.Trans(MStage.Get(StageType.Residence).StageNameKey);
			var stage = MStage.Get(StageType.Park);
			tabNames[1].text = stage.EnterRankLimit <= rank ? Localizer.Trans(stage.StageNameKey) : HideName;
			stage = MStage.Get(StageType.Convenience);
			tabNames[2].text = stage.EnterRankLimit <= rank ? Localizer.Trans(stage.StageNameKey) : HideName;
			stage = MStage.Get(StageType.StationFront);
			tabNames[3].text = stage.EnterRankLimit <= rank ? Localizer.Trans(stage.StageNameKey) : HideName;
			stage = MStage.Get(StageType.Mansion);
			tabNames[4].text = stage.EnterRankLimit <= rank ? Localizer.Trans(stage.StageNameKey) : HideName;
			stage = MStage.Get(StageType.ShoppingMall);
			tabNames[5].text = stage.EnterRankLimit <= rank ? Localizer.Trans(stage.StageNameKey) : HideName;
			stage = MStage.Get(StageType.FashionShop);
			tabNames[6].text = stage.EnterRankLimit <= rank ? Localizer.Trans(stage.StageNameKey) : HideName;
			int index = 0;
			for (int i = 0; i < CvtStageTypeArray.Length; i++)
			{
				if (GameState.GameStateData.CurrentStageType == CvtStageTypeArray[i])
				{
					index = i;
					break;
				}
			}
			tabView.OnClickTab(index);
			// Original cached callback at RVA 0x3B65E0 is empty, delayed 100 frames.
			Scheduler.DelayFlame(() => { }, 100);
		}

		private void OnDestroy()
		{
			// Original RVA 0x4E0590; no close callback is invoked here.
			Instance = null;
		}

		private void OnChangeTab(int index)
		{
			// Recovered original RVA 0x4E05A0. Progress counts prior completions,
			// after the original trial and content-option filters.
			ExternalMethodManager.DestroyChildren(missionItemParent);
			int rank = PlayerController.Instance.Pca.PlayerState.CurrentRank.Rank;
			var stage = MStage.Get(CvtStageTypeArray[index]);
			float completion = 0f;
			if (rank < stage.EnterRankLimit)
			{
				ReleaseCondition.gameObject.SetActive(true);
				ReleaseCondition.text = Localizer.Trans("ui_release_at_rank", GameStateFacade.GetRankNameAutoHide(stage.EnterRankLimit));
			}
			else
			{
				ReleaseCondition.gameObject.SetActive(false);
				float count = 0f;
				foreach (var pair in MMission.Get(CvtStageTypeArray[index]).GetAll())
				{
					var mission = pair.Value;
					if (SystemManager.IsTrial && !mission.Trial) continue;
					if (GameState.OptionData.FutanariDisable && mission.OriginalRecord.CategoryType == MissionCategoryType.Futanari) continue;
					if (GameState.OptionData.PeeDisable && mission.OriginalRecord.CategoryType == MissionCategoryType.Pee) continue;
					UnityEngine.Object.Instantiate(itemPrefab, missionItemParent).GetComponent<MissionMenuItemView>().Initialize(mission);
					count += 1f;
					if (GameStateFacade.GetMissionAchieveCount(mission.Id) > 0) completion += 1f;
				}
				completion /= count;
			}
			completeRate.text = Localizer.Trans("ui_mission_panel_achieve_rate", unchecked((int)(completion * 100f)).ToString());
			scrollRect.verticalNormalizedPosition = 1f;
			scrollRect.velocity = Vector2.zero;
		}

		private void LateUpdate()
		{
			// Original RVA 0x4E0DD0 has no extra active-panel gate.
			if (InputManager.IsDownFrame(InputManager.InputType.Cancel) || InputManager.IsDownFrame(InputManager.InputType.Mission))
				OnClickClose();
		}

		public void OnClickClose()
		{
			// Original RVA 0x4E0ED0: caller callback, close sound, then base Close.
			onClose?.Invoke();
			UiSoundPlayer.Instance.PlayOneShotAudioData((InGameSoundManager.SeType)50, -1f, false);
			Close();
		}

		public static void Open(Action onClose)
		{
            var view = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(
                global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MissionMenuPanel, false) as MissionMenuPanelView;
            if (view is null) throw new global::System.NullReferenceException();
            view.Initialize(onClose);
		}

		public MissionMenuPanelView()
		{
			// Original RVA 0x4E1070; exact 28-byte array at metadata offset 0x4F7C88.
			CvtStageTypeArray = new[]
			{
				StageType.Residence, StageType.Park, StageType.Convenience,
				StageType.StationFront, StageType.Mansion, StageType.ShoppingMall, StageType.FashionShop
			};
		}
	}
}
