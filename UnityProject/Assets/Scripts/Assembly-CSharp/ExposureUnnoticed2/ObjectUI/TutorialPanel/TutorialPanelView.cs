using Common.Scripts.UI;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.TutorialPanel
{
	public class TutorialPanelView : BasePanelView
	{
		public enum TutorialOpenKind
		{
			SystemMenu = 0,
			FirstPlay = 1,
			FirstOutside = 2,
			FirstReinforce = 3,
			FirstWater = 4,
			FirstDildo = 5,
			FirstHandcuff = 6,
			FirstVibe = 7,
			FirstSecondMap = 8,
			FirstPistonMachine = 9,
			FirstBodyPaint = 10,
			FirstInvisible = 11,
			FirstTimeStop = 12,
			FirstHideCostume = 13,
			EndTrial = 14
		}

		[SerializeField]
		private GameObject[] contentParents;

		[SerializeField]
		private GameObject[] unreadIcon;

		[SerializeField]
		private GameObject[] categoryButton;

		[SerializeField]
		private TextMeshProUGUI currentName;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private GameObject blackBackground;

		[SerializeField]
		private GameObject trialButton;

		private TutorialData data;

		private float stopRemainTime;

		private int currentSelectIndex;

		private void Initialize(TutorialOpenKind kind)
		{
			// Recovered from GameAssembly.dll RVA 0x4BD130
			global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.Instance.SwitchMode(global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.TimeScaleMode.OpenTutorial, true);
			// Original close callback RVA 0x4BF070.
			onClose = () =>
			{
			    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)50, -1f, false);
			    global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.Instance.SwitchMode(global::ExposureUnnoticed2.Scripts.Base.TimeScaleManager.TimeScaleMode.OpenTutorial, false);
			};
			data = global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData;
			blackBackground.SetActive(kind != TutorialOpenKind.SystemMenu);
			stopRemainTime = kind != TutorialOpenKind.SystemMenu ? 1f : 0f;
			categoryButton[0].SetActive(data.IsOpenFirstPlay);
			categoryButton[1].SetActive(data.IsOpenFirstPlay);
			categoryButton[2].SetActive(data.IsOpenFirstPlay);
			categoryButton[3].SetActive(data.IsOpenFirstOutside);
			categoryButton[4].SetActive(data.IsOpenFirstOutside);
			categoryButton[5].SetActive(data.IsOpenFirstOutside);
			categoryButton[6].SetActive(data.IsOpenWater);
			categoryButton[7].SetActive(data.IsOpenReinforce);
			categoryButton[8].SetActive(data.IsOpenDildo);
			categoryButton[9].SetActive(data.IsOpenVibe);
			categoryButton[10].SetActive(data.IsOpenHandcuff);
			categoryButton[11].SetActive(data.IsOpenSecondMap);
			categoryButton[12].SetActive(data.IsOpenBodyPaint);
			categoryButton[13].SetActive(data.IsOpenPistonMachine);
			categoryButton[14].SetActive(data.IsOpenHideCostume);
			categoryButton[15].SetActive(data.IsOpenTimeStop);
			categoryButton[16].SetActive(data.IsOpenInvisible);
			categoryButton[17].SetActive(data.IsOpenFirstPlay);
			categoryButton[18].SetActive(data.IsOpenFirstPlay);
			categoryButton[19].SetActive(data.IsOpenEndTryal);
			categoryButton[20].SetActive(data.IsOpenFirstOutside);
			if (!global::ExposureUnnoticed2.Scripts.Base.SystemManager.IsTrial) trialButton.SetActive(false);
			int category = kind switch
			{
			    TutorialOpenKind.FirstOutside => 3,
			    TutorialOpenKind.FirstReinforce => 8,
			    TutorialOpenKind.FirstWater => 6,
			    TutorialOpenKind.FirstDildo => 9,
			    TutorialOpenKind.FirstHandcuff => 11,
			    TutorialOpenKind.FirstVibe => 10,
			    TutorialOpenKind.FirstSecondMap => 7,
			    TutorialOpenKind.FirstPistonMachine => 13,
			    TutorialOpenKind.FirstBodyPaint => 12,
			    TutorialOpenKind.FirstInvisible => 16,
			    TutorialOpenKind.FirstTimeStop => 15,
			    TutorialOpenKind.FirstHideCostume => 14,
			    TutorialOpenKind.EndTrial => 19,
			    _ => 0
			};
			ToPadModeIfNeed();
			OnClickCategory(category);
			buttonGroupManager.ReSearchButton(true, false);
			buttonGroupManager.ForceSelect(categoryButton[category].GetComponent<ButtonView>());
		}

		private void ToPadModeIfNeed()
		{
			// Recovered from GameAssembly.dll RVA 0x4BE000
			if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad) buttonGroupManager.ForceSelect(buttonGroupManager.Get(currentSelectIndex));
		}

		private void RefreshUnreadIcon()
		{
			// Recovered from GameAssembly.dll RVA 0x4BE0D0
			for (int i = 0; i < unreadIcon.Length; ++i) unreadIcon[i].SetActive(!data.AlreadyViewCategorySet.Contains(i));
		}

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x4BE200
			if (IsActivePanel() && global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad)
			{
			    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.JustChangeIsGamePad) ToPadModeIfNeed();
			    int direction = 0;
			    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIUp)
			        || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIUp, 20, 5)) direction = -1;
			    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDown)
			        || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDown, 20, 5)) ++direction;
			    if (direction != 0)
			    {
			        currentSelectIndex = (buttonGroupManager.ButtonCount + currentSelectIndex + direction) % buttonGroupManager.ButtonCount;
			        buttonGroupManager.InvokeOnClick(currentSelectIndex);
			    }
			}
			stopRemainTime -= Time.unscaledDeltaTime;
			if (stopRemainTime <= 0f) CommonCheckClose();
		}

		public void OnClickCategory(int categoryId)
		{
			// Recovered from GameAssembly.dll RVA 0x4BE430
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48, -1f, false);
			data.AlreadyViewCategorySet.Add(categoryId);
			RefreshUnreadIcon();
			for (int i = 0; i < contentParents.Length; ++i) contentParents[i].SetActive(i == categoryId);
			currentName.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(categoryId switch
			{
			    0 => "ui_tutorial_how_to_play",
			    1 => "m_stage_apart",
			    2 => "ui_tutorial_manual",
			    3 => "ui_tutorial_rule",
			    4 => "ui_discomfort",
			    5 => "rp_buff_bareyasusa",
			    6 => "ui_tutorial_pee",
			    7 => "ui_tutorial_fast_travel",
			    8 => "ui_tutorial_reinforce",
			    9 => "m_item_dildo",
			    10 => "ui_tutorial_vibe",
			    11 => "ui_tutorial_handcuff",
			    12 => "ui_tutorial_bodypaint",
			    13 => "ui_tutorial_piston",
			    14 => "ui_tutorial_hide_costume",
			    15 => "m_skill_time_stop",
			    16 => "ui_tutorial_invisible",
			    17 => "ui_tutorial_manual_pad",
			    18 => "ui_tutorial_manual_oneHanded",
			    19 => "ui_tutoriarl_Trial",
			    20 => "ui_tutorial_title_Stamina",
			    _ => throw new global::System.Runtime.CompilerServices.SwitchExpressionException(categoryId)
			});
		}

		public static void Open(TutorialOpenKind kind)
		{
			// Recovered from GameAssembly.dll RVA 0x4BE9D0
			if (global::ExposureUnnoticed2.Scripts.InGame.DebugController.IsForceSkipTutorial && kind != TutorialOpenKind.SystemMenu) return;
			if ((int)kind == 1 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenFirstPlay) return;
			if ((int)kind == 2 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenFirstOutside) return;
			if ((int)kind == 3 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenReinforce) return;
			if ((int)kind == 4 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenWater) return;
			if ((int)kind == 5 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenDildo) return;
			if ((int)kind == 6 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenHandcuff) return;
			if ((int)kind == 7 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenVibe) return;
			if ((int)kind == 8 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenSecondMap) return;
			if ((int)kind == 9 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenPistonMachine) return;
			if ((int)kind == 10 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenBodyPaint) return;
			if ((int)kind == 11 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenInvisible) return;
			if ((int)kind == 12 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenTimeStop) return;
			if ((int)kind == 13 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenHideCostume) return;
			if ((int)kind == 14 && global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenEndTryal) return;
			if ((int)kind == 1) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenFirstPlay = true;
			if ((int)kind == 2) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenFirstOutside = true;
			if ((int)kind == 3) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenReinforce = true;
			if ((int)kind == 4) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenWater = true;
			if ((int)kind == 5) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenDildo = true;
			if ((int)kind == 6) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenHandcuff = true;
			if ((int)kind == 7) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenVibe = true;
			if ((int)kind == 8) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenSecondMap = true;
			if ((int)kind == 9) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenPistonMachine = true;
			if ((int)kind == 10) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenBodyPaint = true;
			if ((int)kind == 11) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenInvisible = true;
			if ((int)kind == 12) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenTimeStop = true;
			if ((int)kind == 13) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenHideCostume = true;
			if ((int)kind == 14) global::ExposureUnnoticed2.Scripts.InGame.GameState.TutorialData.IsOpenEndTryal = true;
			// Original captured local OpenProcess RVA 0x4BF110.
			void OpenProcess()
			{
			    if (kind != TutorialOpenKind.SystemMenu)
			        global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49, -1f, false);
			    (global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.TutorialPanel, true) as TutorialPanelView).Initialize(kind);
			}
			if (kind != TutorialOpenKind.SystemMenu) global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayUnscaledTime(OpenProcess, 0.5f);
			else OpenProcess();
		}
	}
}
