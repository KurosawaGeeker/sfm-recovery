using ExposureUnnoticed2.Scripts.Sound;
using ExposureUnnoticed2.ObjectUI.SaveDataSelectPanel;
using SeType = ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType;
using Common.Scripts.UI;
using HadakaCoat.ObjectsUi.Common.Button;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGameMenu
{
	public class InGameMenuView : BasePanelView
	{
		private enum State
		{
			Hidden = 0,
			Show = 1,
			SystemShow = 2
		}

		public static InGameMenuView Instance;

		public static float LastCloseTime;

		[SerializeField]
		private CanvasGroup baseCanvasGroup;

		[SerializeField]
		private GameObject newRankInfo;

		[SerializeField]
		private GameObject newAchievementInfo;

		[SerializeField]
		private TextMeshProUGUI killTimeText;

		[SerializeField]
		private TextMeshProUGUI achevementText;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		private static readonly int IsShow = Animator.StringToHash("IsShow");

		private static readonly int IsSystemShow = Animator.StringToHash("IsSystemShow");

		private State currentState;

		private bool isOpenChild;

		private bool oldIsOpenChild;

		private bool isBlockFastTravelByPortalDist;

		private static readonly int Normal = Animator.StringToHash("Normal");

		private int currentSelectIndex;

		public void Update()
		{
            // Complete original 0x4F8620. Hidden menus are first opened by the separate original UI LateUpdate producer.
            if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsGameOver)
            {
                if (currentState == State.Show) MenuClose();
                return;
            }
            if (currentState == State.Hidden) return;
            OnUpdate();
            if (global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenSomeUi && currentState == State.Hidden) return;
            if (isOpenChild) { oldIsOpenChild = isOpenChild; return; }
            if (oldIsOpenChild) global::UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(null);
            if (currentState != State.Hidden)
            {
                if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsJustToGamePad) ToPadModeIfNeed();
                if (IsActivePanelPadMode())
                {
                    int move = 0;
                    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1001) || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1001, 20, 5)) move = -1;
                    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1002) || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1002, 20, 5)) move++;
                    if (move != 0)
                    {
                        global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48);
                        currentSelectIndex = unchecked(buttonGroupManager.ButtonCount + currentSelectIndex + move) % buttonGroupManager.ButtonCount;
                        buttonGroupManager.ForceSelect(currentSelectIndex);
                    }
                    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1000)) buttonGroupManager.InvokeOnClick(currentSelectIndex);
                    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)10))
                    {
                        global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)52);
                        MenuClose();
                    }
                }
                else if ((global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)2) || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)10)) && IsActivePanel())
                {
                    switch (currentState)
                    {
                        case State.Hidden: Open(); break;
                        case State.Show:
                            global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)52);
                            MenuClose(); break;
                        case State.SystemShow: SystemBack(); break;
                    }
                }
            }
            oldIsOpenChild = isOpenChild;
            // Original GetNearPortal is already complete. Preserve the final discarded distance and original null behavior.
            var nearest = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.CurrentStageController.PortalManager.GetNearPortal();
            global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DistSqrt(nearest.gameObject, global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject);
        }

		public static void Open()
		{
            var panel = global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.Open(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.InGameMenu, false) as InGameMenuView;
            panel.Initialize();
        }

		private void ToPadModeIfNeed(bool isForce = false)
		{
            if (!IsActivePanel() && !isForce) return;
            if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsGamePad) buttonGroupManager.ForceSelect(currentSelectIndex);
            else buttonGroupManager.ForceSelect((ButtonGroupManager.IButtonView)null);
        }

		public override void OnReturnActive()
		{
            ToPadModeIfNeed(false);
        }

		private void Initialize()
		{
            baseCanvasGroup.alpha = 0f;
            Instance = this;
            animator = GetComponent<Animator>();
            animator.SetBool(IsShow, true);
            currentState = State.Show;
            global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)51);
            newRankInfo.SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ConfirmedReleasedInfoRank < global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ReleasedInfoRank);
            bool available = false;
            foreach (var pair in global::ExposureUnnoticed2.Master.Achievement.MAchievement.GetAll())
            {
                if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.AchievementReceiveSet.Contains(pair.Value.Id) && pair.Value.CalcIsAchieved())
                {
                    available = !global::ExposureUnnoticed2.Scripts.Base.SystemManager.IsTrial;
                    break;
                }
            }
            newAchievementInfo.SetActive(available);
            killTimeText.color = global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsRelease((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)6) ? Color.white : new Color(.5f, .5f, .5f, 1f);
            ToPadModeIfNeed(true);
            achevementText.color = global::ExposureUnnoticed2.Scripts.Base.SystemManager.IsTrial ? new Color(.5f, .5f, .5f, 1f) : Color.white;
        }

		public void MenuClose()
		{
            if (currentState == State.Hidden) return;
            currentState = State.Hidden;
            animator.SetBool(IsShow, false);
            animator.SetBool(IsSystemShow, false);
            LastCloseTime = Time.time;
            Instance = null;
            Close();
        }

		private void SystemBack()
		{
            currentState = State.Show;
            animator.SetBool(IsSystemShow, false);
        }

		public void OnClickEmpty()
		{
            if (!isOpenChild) MenuClose();
        }

		public void OnClickInventory()
		{
            global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
            isOpenChild = true;
            global::ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryPanelView.Open(() => isOpenChild = false);
        }

		public void OnClickSkill()
		{
            // Original RVA 0x4F9AA0.
            UiSoundPlayer.Instance.PlayOneShotAudioData((SeType)49);
            isOpenChild = true;
            SkillPanelView.Open(() => isOpenChild = false);
		}

		public void OnClickMap()
		{
		}

		public void OnClickMission()
		{
			// Recovered from GameAssembly.dll RVA 0x4F9B70; same child-panel ownership as inventory.
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
			isOpenChild = true;
			global::ExposureUnnoticed2.ObjectUI.MissionMenuPanel.MissionMenuPanelView.Open(() => isOpenChild = false);
		}

		public void OnClickKillTime()
		{
		}

		public void OnClickFastTravel()
		{
		}

		public void OnClickActiveBuff()
		{
		}

		public void OnClickRank()
		{
		}

		public void OnClickAchievement()
		{
		}
	}
}
