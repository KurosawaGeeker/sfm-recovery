using System;
using System.Collections.Generic;
using System.Text;
using Common.CommonUI.CommonGauge;
using Common.Scripts.UI;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.ObjectUI.InGame;
using ExposureUnnoticed2.ObjectUI.InGame.Compass;
using ExposureUnnoticed2.ObjectUI.InGame.HeatBeatPanel;
using ExposureUnnoticed2.ObjectUI.InGame.MessagePanel;
using ExposureUnnoticed2.ObjectUI.InGame.MissionPanel;
using ExposureUnnoticed2.ObjectUI.InGame.MotionSpeedPanel;
using ExposureUnnoticed2.ObjectUI.InGame.NpcDirectIconPanel;
using ExposureUnnoticed2.ObjectUI.InGame.RingMenu;
using ExposureUnnoticed2.ObjectUI.InGame.TempUnlockPanel;
using ExposureUnnoticed2.ObjectUI.InGame.VIbeStatePanel;
using ExposureUnnoticed2.ObjectUI.InteractPanel;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using Ricimi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.Object3D.IngameManager
{
	public class InGameUiManager : MonoBehaviour, ILateUpdater
	{
		public static InGameUiManager Instance;

		[SerializeField]
		private GaugeView staminaGauge;

		[SerializeField]
		private GaugeFillView ecstasyGauge;

		[SerializeField]
		private GaugeView mentalGauge;

		[SerializeField]
		private GaugeView moistureGauge;

		[SerializeField]
		private Animator mentalGaugeAnimator;

		[SerializeField]
		private Image moistureIcon;

		[SerializeField]
		private InteractPanelView interactPanelView;

		[SerializeField]
		private HeartRatePanelView heartRatePanelView;

		[SerializeField]
		private MessagePanelView messagePanelView;

		[SerializeField]
		private MissionPanelView missionPanelView;

		[SerializeField]
		private TempUnlockPanelView tempUnlockPanelView;

		[SerializeField]
		private CompassView compassView;

		[SerializeField]
		private RingMenuParentView RingMenuParentView;

		[SerializeField]
		private NpcDirectionPanelView npcDirectionPanelView;

		[SerializeField]
		private TextMeshProUGUI rpBonusText;

		[SerializeField]
		private TextMeshProUGUI bareyasusaText;

		[SerializeField]
		public Image BlindVignette;

		[SerializeField]
		public Transform middleCloneLayer;

		[SerializeField]
		public Transform frontLayer;

		[SerializeField]
		public Transform BackLayer;

		[SerializeField]
		private CanvasGroup middleCloneCanvasGroup;

		[SerializeField]
		private GameObject statusInfoObject;

		[SerializeField]
		private GameObject faceCameraObject;

		[SerializeField]
		private Image ecstasyHeartIcon;

		[SerializeField]
		private MotionSpeedPanelView motionSpeedPanelView;

		[SerializeField]
		private WalkSpeedPanelView walkSpeedPanelView;

		[SerializeField]
		private Image invisibleVignette;

		[SerializeField]
		private GameObject CurrentRpApart;

		[SerializeField]
		private GameObject CurrentRpOtherMap;

		[SerializeField]
		private Animator StaminaGaugeAnimator;

		[SerializeField]
		private VibeStatePanelView vibeStatePanelView;

		[SerializeField]
		private IngameUiManualView ingameUiManualView;

		[SerializeField]
		private GameObject sexManualParent;

		[SerializeField]
		private GameObject nomalManualParent;

		[SerializeField]
		private RawImage faceCameraImage;

		[SerializeField]
		private RawImage bodyCameraImage;

		[SerializeField]
		private Ricimi.Gradient rpBonusGradeImage;

		[SerializeField]
		private GameObject NoMissionAchieveIcon;

		[SerializeField]
		private GameObject shortcutSlotParent;

		[SerializeField]
		private GameObject shortcutSlotParent2;

		private PlayerController player;

		private UiPanelManager panelManager;

		private List<BasePanelView> basePanelStack;

		private bool isProcessedGameOver;

		private SmoothFloat middleCloneAlpha;

		private SmoothFloat invisibleVignetteAlpha;

		private StringBuilder sb;

		private static readonly int Red = Animator.StringToHash("Red");

		private static readonly int RedBlink = Animator.StringToHash("RedBlink");

		private Material selfCameraImageMaterial;

		private static readonly int Cutoff = Shader.PropertyToID("_Cutoff");

		private Color oldRpBonusGradientColor;

		public bool IsOpenFade { get; set; }

		public bool IsOpenSomeUi => isOpenSomeAllowInputUi || IsOpenSomeCancelInputUi;

		public bool IsOpenSomeCancelInputUi { get; set; }

		public bool isOpenSomeAllowInputUi { get; set; }

		public bool IsForceDeActive { get; private set; }

		public void InitializeBeforePlayer()
		{
			// Recovered from GameAssembly.dll RVA 0x454740
			messagePanelView.Initialize();
		}

		public void Initialize(PlayerController player)
		{
			// Recovered from GameAssembly.dll RVA 0x454870
			if (Instance != null) return;
			Instance = this;
			this.player = player;
			Material material = UnityEngine.Object.Instantiate(faceCameraImage.material);
			faceCameraImage.material = material;
			bodyCameraImage.material = material;
			selfCameraImageMaterial = material;
			middleCloneAlpha.Target = 1f;
			panelManager = new UiPanelManager();
			staminaGauge.SetLengthScale(200f);
			staminaGauge.SetMax(player.Pca.Stamina.Max);
			mentalGauge.SetLengthScale(2f);
			mentalGauge.SetMax(player.Pca.Mental.MaxMental);
			moistureGauge.SetLengthScale(400f);
			moistureGauge.SetMax(1f);
			interactPanelView.Initialize();
			heartRatePanelView.Initialize(player.Pca.PlayerHeartBeat);
			missionPanelView.Initialize();
			tempUnlockPanelView.Initialize();
			compassView.Initialize();
			npcDirectionPanelView.Initialize();
			RingMenuParentView.Initialize();
			motionSpeedPanelView.Initialize();
			walkSpeedPanelView.Initialize();
			vibeStatePanelView.Initialize();
			ingameUiManualView.Initialize();
			GetComponent<global::ExposureUnnoticed2.Scripts.Base.AspectRatioApplyer>().Initialize();
			EventManager.AddListener<InteractEvent>(OnInteract, false);
			EventManager.AddListener<CompleteTransitionStageEvent>(OnChangeStage, false);
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x455310
			if (Instance != this) return;
			Instance = null;
			EventManager.RemoveListener<InteractEvent>(OnInteract);
			EventManager.RemoveListener<CompleteTransitionStageEvent>(OnChangeStage);
			compassView.Dispose();
		}

		public void UpdateSelfCameraImageMaterial(float val)
		{
			// Recovered from GameAssembly.dll RVA 0x455680
			selfCameraImageMaterial.SetFloat(Cutoff, val);
		}

		public void OnLateUpdate()
		{
            global::ExposureUnnoticed2.ObjectUI.InGame.NpcDirectIconPanel.NpcDirectionArrowView.IsDoneCommonProcessThisFrame = false;
            if (InGameManager.Instance.TempInGameState.IsGameOver && !isProcessedGameOver)
            {
                isProcessedGameOver = true;
                middleCloneAlpha.Target = 0f;
            }
            middleCloneAlpha.UpdateTo(middleCloneAlpha.Target);
            var alphaGroup = middleCloneCanvasGroup;
            float middleAlpha = middleCloneAlpha.Current;
            alphaGroup.alpha = middleAlpha;
            staminaGauge.SetMax(player.Pca.Stamina.Max);
            staminaGauge.Set(player.Pca.Stamina.Current);
            ecstasyGauge.Set(GameState.GameStateData.PlayerEcstasy);
            mentalGauge.SetMax(player.Pca.Mental.MaxMental);
            mentalGauge.Set(GameState.GameStateData.PlayerMental);
            moistureGauge.Set(GameState.GameStateData.PlayerMoisture);
            moistureIcon.fillAmount = GameState.GameStateData.PlayerMoisture;
            interactPanelView.OnLateUpdate();
            heartRatePanelView.OnLateUpdate();
            tempUnlockPanelView.OnLateUpdate();
            missionPanelView.OnLateUpdate();
            motionSpeedPanelView.OnLateUpdate();
            walkSpeedPanelView.OnLateUpdate();
            vibeStatePanelView.OnLateUpdate();
            ingameUiManualView.OnUpdate();
            var noMissionIcon = NoMissionAchieveIcon;
            bool noMission = InGameManager.Instance.TempInGameState.IsNoAchieveMissionMode;
            noMissionIcon.SetActive(noMission);
            var bonusLabel = rpBonusText;
            float rp = player.Pca.PlayerState.RpBonus;
            bonusLabel.SetText("x{0:2}", rp);
            Color secondary = global::ExposureUnnoticed2.ObjectUI.Misc.RpBonusColorSelector.GetTextColorSecondary(player.Pca.PlayerState.RpBonus);
            Color primary = global::ExposureUnnoticed2.ObjectUI.Misc.RpBonusColorSelector.GetTextColor(player.Pca.PlayerState.RpBonus);
            rpBonusText.colorGradient = new VertexGradient(secondary, secondary, primary, primary);
            Color nextGradient = global::ExposureUnnoticed2.ObjectUI.Misc.RpBonusColorSelector.GetTextColorSecondary(player.Pca.PlayerState.RpBonus);
            GameObject gradeObject = rpBonusGradeImage.gameObject;
            float gradeRp = player.Pca.PlayerState.RpBonus;
            gradeObject.SetActive(gradeRp >= 1.5f);
            if (oldRpBonusGradientColor != nextGradient)
            {
                var capturedGrade = rpBonusGradeImage;
                oldRpBonusGradientColor = nextGradient;
                Color nextPrimary = global::ExposureUnnoticed2.ObjectUI.Misc.RpBonusColorSelector.GetTextColor(player.Pca.PlayerState.RpBonus);
                capturedGrade.Color1 = nextPrimary;
                rpBonusGradeImage.Color2 = nextGradient;
                rpBonusGradeImage.enabled = false;
                rpBonusGradeImage.enabled = true;
            }
            var visibilityLabel = bareyasusaText;
            float visibility = player.Pca.PlayerState.Bareyasusa;
            int percent = unchecked((int)(((double)visibility + 0.001) * 100.0));
            visibilityLabel.SetText("{0}%", percent);
            bool allowSystemMenu;
            if (!global::ExposureUnnoticed2.Object3D.NPC.Script.NpcManager.Instance.IsMaxStrangeSomeOne) allowSystemMenu = true;
            else
            {
                if (PlayerFacade.Instance is null) throw new global::System.NullReferenceException();
                allowSystemMenu = GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)21)
                    || GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)22)
                    || SexManager.Instance.IsSexMode;
            }
            if (allowSystemMenu && !InGameManager.Instance.TempInGameState.IsGameOver)
            {
                if ((!isOpenSomeAllowInputUi && !IsOpenSomeCancelInputUi)
                    || (SexManager.Instance.IsSexMode && !global::ExposureUnnoticed2.ObjectUI.SystemMenu.SystemMenuView.Instance))
                {
                    if (InputManager.IsDownFrame(InputManager.InputType.SystemMenu)
                        && Time.time - global::ExposureUnnoticed2.ObjectUI.InGameMenu.InGameMenuView.LastCloseTime >= 0.5f)
                        global::ExposureUnnoticed2.ObjectUI.SystemMenu.SystemMenuView.Open();
                }
            }
            if (!InGameManager.Instance.TempInGameState.IsGameOver && !isOpenSomeAllowInputUi && !IsOpenSomeCancelInputUi)
            {
                if (InputManager.IsDownFrame(InputManager.InputType.GameMenu) && !global::ExposureUnnoticed2.ObjectUI.InGameMenu.InGameMenuView.Instance)
                    global::ExposureUnnoticed2.ObjectUI.InGameMenu.InGameMenuView.Open();
                else if (InputManager.IsDownFrame(InputManager.InputType.Mission) && !global::ExposureUnnoticed2.ObjectUI.MissionMenuPanel.MissionMenuPanelView.Instance)
                {
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
                    global::ExposureUnnoticed2.ObjectUI.MissionMenuPanel.MissionMenuPanelView.Open(null);
                }
                else if (InputManager.IsDownFrame(InputManager.InputType.Inventory) && !global::ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryPanelView.Instance)
                {
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
                    global::ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryPanelView.Open(null, 0);
                }
                else if (InputManager.IsDownFrame(InputManager.InputType.AdultGoods) && !global::ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryPanelView.Instance)
                {
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
                    global::ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryPanelView.Open(null, 1);
                }
                else if (InputManager.IsDownFrame(InputManager.InputType.Costume) && !global::ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryPanelView.Instance)
                {
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
                    global::ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryPanelView.Open(null, 2);
                }
                else if (InputManager.IsDownFrame(InputManager.InputType.MySet) && !global::ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryPanelView.Instance)
                {
                    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)49);
                    global::ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryPanelView.Open(null, 3);
                }
            }
            invisibleVignetteAlpha.UpdateTo(GameState.GameStateData.IsActiveInvisible ? 1f : 0f);
            var vignette = invisibleVignette;
            Color vignetteColor = vignette.color;
            float vignetteAlpha = invisibleVignetteAlpha.Current;
            vignette.color = new Color(vignetteColor.r, vignetteColor.g, vignetteColor.b, vignetteAlpha);
		}

		public void OpenFadeInTransStage(Action onPureBlack, Action onStartFadeIn = null, float waitTimeAfterSceneLoad = 0f, Action onCompleteFade = null)
		{
		
            // Original complete 0x456C80 and original display76_0 callback0x458C40.
            InGameManager.Instance.TempInGameState.IsPlayingSomePerformance = true;
            IsOpenFade = true;
            var starter = new global::ExposureUnnoticed2.ObjectUI.FadeOutPanel.WaitSceneLoadAndTimeStarter(waitTimeAfterSceneLoad);
            global::ExposureUnnoticed2.ObjectUI.FadeOutPanel.FadePanelView.Open(starter, onPureBlack, onStartFadeIn, () =>
            {
                InGameManager.Instance.TempInGameState.IsPlayingSomePerformance = false;
                IsOpenFade = false;
                onCompleteFade?.Invoke();
            });
		}

		private void OnGameOver()
		{
			// Recovered from GameAssembly.dll RVA 0x456FB0
			middleCloneAlpha.Target = 0f;
		}

		public void SetSexModeUi()
		{
			// Recovered from GameAssembly.dll RVA 0x456FE0
			bool isSexMode = SexManager.Instance.IsSexMode;
			shortcutSlotParent.SetActive(!isSexMode);
			shortcutSlotParent2.SetActive(!isSexMode);
			nomalManualParent.SetActive(!isSexMode);
			sexManualParent.SetActive(isSexMode);
		}

		private void OnInteract(InteractEvent evt)
		{
            // Complete original4571B0. Keep sequential event/model rereads rather than one cached switch.
            int first = (int)evt.Model.Type;
            if (first == 0 || (unchecked((uint)first) < 20U && ((0x88410U >> (first & 31)) & 1U) != 0U) || first == 17) return;
            if ((int)evt.Model.Type == 1)
            {
                {
                    var data = GameState.GameStateData;
                    if ((int)data.PlayerCurrentHadObject != 0 && (int)data.PlayerCurrentHadObject != -1) return;
                }
                if (!PlayerFacade.Instance.CanUseHand(true)) return;
                global::ExposureUnnoticed2.ObjectUI.BuyPanel.BuyPanelView.Open((global::ExposureUnnoticed2.ObjectUI.BuyPanel.BuyPanelView.BuyPanelType)0, () => { });
            }
            if ((int)evt.Model.Type == 6)
            {
                {
                    var data = GameState.GameStateData;
                    if ((int)data.PlayerCurrentHadObject != 0 && (int)data.PlayerCurrentHadObject != -1) return;
                }
                if (!PlayerFacade.Instance.CanUseHand(true)) return;
                global::ExposureUnnoticed2.ObjectUI.BuyPanel.BuyPanelView.Open((global::ExposureUnnoticed2.ObjectUI.BuyPanel.BuyPanelView.BuyPanelType)1, () => { });
            }
            if ((int)evt.Model.Type == 5)
            {
                isOpenSomeAllowInputUi = true;
                global::ExposureUnnoticed2.ObjectUI.ConversationPanel.ConversationPanelView.Open(global::ExposureUnnoticed2.Master.Conversation.MConversation.Get((global::ExposureUnnoticed2.Master.Conversation.MConversationType)0), () => isOpenSomeAllowInputUi = false);
            }
            if ((int)evt.Model.Type == 7)
            {
                isOpenSomeAllowInputUi = true;
                global::ExposureUnnoticed2.ObjectUI.ConversationPanel.ConversationPanelView.Open(global::ExposureUnnoticed2.Master.Conversation.MConversation.Get((global::ExposureUnnoticed2.Master.Conversation.MConversationType)1), () => isOpenSomeAllowInputUi = false);
            }
            if ((int)evt.Model.Type == 8) global::ExposureUnnoticed2.ObjectUI.BarberMenu.BarberMenuView.Open();
            if ((int)evt.Model.Type == 11 && global::ExposureUnnoticed2.ObjectUI.BarberMenu.ClosetMenuView.Instance == null) global::ExposureUnnoticed2.ObjectUI.BarberMenu.ClosetMenuView.Open();
            if ((int)evt.Model.Type == 13) global::ExposureUnnoticed2.ObjectUI.ChooseHandcuffsPanel.SleepSelectPanelView.Open();
            if ((int)evt.Model.Type == 14) global::ExposureUnnoticed2.ObjectUI.PcMenuPanel.PcMenuPanelView.Open();
        }

		public BasePanelView Open(BasePanelView view, bool isFront = false)
		{
			// Recovered from GameAssembly.dll RVA 0x457A40
			GameObject obj = UnityEngine.Object.Instantiate(view.gameObject, isFront ? frontLayer : middleCloneLayer);
			BasePanelView panel = obj.GetComponent<BasePanelView>();
			// Original close callback RVA 0x458CD0.
			panel.InitializeFromManager(() =>
			{
			    PopBasePanel(panel);
			    if (basePanelStack.Count == 0)
			    {
			        IsOpenSomeCancelInputUi = false;
			        CursorController.SetVisible(false);
			    }
			    // The original enqueues an empty callback for one frame (shared RET RVA 0x3B65E0).
			    Scheduler.DelayFlame(() => { }, 1);
			});
			IsForceDeActive = true;
			// Original delayed open callback RVA 0x458EC0.
			Scheduler.DelayFlame(() =>
			{
			    PushBasePanel(panel);
			    IsOpenSomeCancelInputUi = true;
			    IsForceDeActive = false;
			    if (!InputManager.IsGamePad) CursorController.SetVisible(true);
			}, 2);
			return panel;
		}

		public void StaminaRedBlink()
		{
			// Recovered from GameAssembly.dll RVA 0x457D20
			StaminaGaugeAnimator.SetTrigger(RedBlink);
		}

		private void PushBasePanel(BasePanelView view)
		{
			// Recovered from GameAssembly.dll RVA 0x457DE0
			basePanelStack.Add(view);
		}

		private void PopBasePanel(BasePanelView view)
		{
			// Recovered from GameAssembly.dll RVA 0x457E80
			if (basePanelStack.Count == 0) return;
			if (basePanelStack[basePanelStack.Count - 1] != view)
			{
			    throw new Exception("BasePanelStackがズレてます ^1:" + basePanelStack[basePanelStack.Count - 1].gameObject?.name
			                        + " view:" + view.gameObject?.name);
			}
			// Original List.Remove removes the first matching reference, including duplicates.
			basePanelStack.Remove(view);
			if (basePanelStack.Count > 0) basePanelStack[basePanelStack.Count - 1].OnReturnActive();
		}

		public BasePanelView GetCurrentBasePanelView()
		{
			// Recovered from GameAssembly.dll RVA 0x458200
			return basePanelStack.Count == 0 ? null : basePanelStack[basePanelStack.Count - 1];
		}

		public void SetVisibleRightDownInfo(bool isVisible)
		{
			// Recovered from GameAssembly.dll RVA 0x4582A0
			statusInfoObject.SetActive(isVisible && GameState.OptionData.RightDownPanel);
			faceCameraObject.SetActive(isVisible && GameState.OptionData.FaceCamera);
		}

		private void OnChangeStage(CompleteTransitionStageEvent ect)
		{
			// Recovered from GameAssembly.dll RVA 0x458400
			bool isApartment = (int)GameState.GameStateData.CurrentStageType == 0;
			CurrentRpApart.SetActive(isApartment);
			CurrentRpOtherMap.SetActive(!isApartment);
		}
	// Recovered original member RVA 0x458520: .ctor
		public InGameUiManager()
        {
            basePanelStack = new List<BasePanelView>();
            middleCloneAlpha = new SmoothFloat(0.1f, 1f, 0f, false, false, -1f);
            invisibleVignetteAlpha = new SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
            sb = new StringBuilder();
            oldRpBonusGradientColor = new Color(0f, 0f, 0f, 1f);
        }
	// Recovered original member RVA 0x458820: .cctor
		// Recovered original .cctor RVA 0x458820: ordered field initializers preserve BeforeFieldInit.
	}
}
