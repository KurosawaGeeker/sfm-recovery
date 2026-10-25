using ExposureUnnoticed2.ObjectTitle;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;
using UnityEngine.Audio;

namespace ExposureUnnoticed2.Scripts.UI
{
	public class TitleSceneView : MonoBehaviour
	{
		private enum Phase
		{
			Intro = 0,
			Title = 1
		}

		private static bool IsPlayedIntro;

		[SerializeField]
		private GameObject teamNameJp;

		[SerializeField]
		private GameObject teamNameGlobal;

		[SerializeField]
		private GameObject continueButton;

		[SerializeField]
		private Transform popupTransform;

		[SerializeField]
		private AudioSource bgm;

		[SerializeField]
		private AspectRatioApplyer subCanvasAspectRatioApplyer;

		[SerializeField]
		private IntroAnimationController IntroAnimationController;

		[SerializeField]
		private InGameGraphicsOptionApplyer inGameGraphicsOptionApplyer;

		[SerializeField]
		private GameObject normalBackground;

		[SerializeField]
		private GameObject wideBackground;

		[SerializeField]
		private GameObject blackCensorParent;

		[SerializeField]
		private GameObject blackCensorParent2;

		[SerializeField]
		private GameObject[] logos;

		[SerializeField]
		private ButtonGroupManager buttonGroupManager;

		[SerializeField]
		private GameObject demoObject;

		[SerializeField]
		private AudioMixer audioMixer;

		private Phase currentPhase;

		private int currentSelectIndex;

		private bool isOpenChild;

		public int IsEndLoadCount;

		public static TitleSceneView Instance { get; private set; }

		[field: SerializeField]
		public GameObject mainCamera { get; private set; }

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x46E4E0
			IntroAnimationController.gameObject.SetActive(true);
			EventManager.AddListener<ChangeLanguageEvent>(OnChangeLanguageEvent);
			EventManager.AddListener<ChangeWindowModeEvent>(OnChangeAspect);
			Instance = this;
			Application.targetFrameRate = 60;
			SystemManager.CreateIfNoExist();
			SaveManager.Load(SaveRapper.SaveDataType.System, -1);
			bool canContinue = SaveManager.IsExistSaveData(SaveRapper.SaveDataType.HomeSave, -1);
			for (int slot = 0; !canContinue && slot < 20; ++slot)
			    canContinue = SaveManager.IsExistSaveData(SaveRapper.SaveDataType.ManualSlot, slot);
			if (!canContinue) canContinue = SaveManager.IsExistSaveData(SaveRapper.SaveDataType.AutoSave, 1);
			if (!canContinue) canContinue = SaveManager.IsExistSaveData(SaveRapper.SaveDataType.Interrupt, -1);
			continueButton.SetActive(canContinue);
			WindowModeOptionApplyer.Apply();
			Application.targetFrameRate = GameState.OptionData.MaxFrameRate;
			GetComponent<AspectRatioApplyer>().Initialize();
			subCanvasAspectRatioApplyer.Initialize();
			CursorController.SetVisible(true);
			teamNameJp.SetActive((int)GameState.OptionData.Language == 0);
			teamNameGlobal.SetActive((int)GameState.OptionData.Language != 0);
			inGameGraphicsOptionApplyer.Initialize();
			if (IsPlayedIntro) IntroAnimationController.Cancel();
			IsPlayedIntro = true;
			normalBackground.SetActive(GameState.OptionData.AspectRatioIndex == 0);
			wideBackground.SetActive(GameState.OptionData.AspectRatioIndex == 1);
			blackCensorParent.SetActive(GameState.OptionData.IsBlackCensor);
			blackCensorParent2.SetActive(GameState.OptionData.IsBlackCensor);
			RefreshLogo();
			demoObject.SetActive(SystemManager.IsTrial);
			if (SystemManager.IsEnc)
			    global::IKA9nt.Encrypter.EncrypterCore.LoadBundleFromStreamAsync("models", OnModelsBundleLoaded, "", 0u);
			else
			    IsEndLoadCount = 500;
		}

		private void ToPadModeIfNeed()
		{
			// Recovered from GameAssembly.dll RVA 0x46F200
			if (isOpenChild) return;
			buttonGroupManager.ForceSelect(InputManager.IsGamePad ? buttonGroupManager.Get(currentSelectIndex) : null);
		}

		private void OnChangeLanguageEvent(ChangeLanguageEvent changeLanguageEvent)
		{
			// Recovered from GameAssembly.dll RVA 0x46F2B0
			RefreshLogo();
		}

		private void RefreshLogo()
		{
			// Recovered from GameAssembly.dll RVA 0x46F2C0
			for (int index = 0; index < logos.Length; ++index)
			    logos[index].SetActive((int)GameState.OptionData.Language == index);
		}

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x46F3E0
			if (currentPhase == Phase.Intro)
			{
			    if (!IntroAnimationController.StartTitle) return;
			    currentPhase = Phase.Title;
			    audioMixer.SetFloat("Master", Mathf.Clamp(Mathf.Log10((float)GameState.OptionData.SoundMaster100 / 100f) * 20f + 6f, -80f, 20f));
			    audioMixer.SetFloat("Environment", Mathf.Clamp(Mathf.Log10((float)GameState.OptionData.SoundSe100 / 100f) * 20f + 6f, -80f, 20f));
			    audioMixer.SetFloat("SystemSe", Mathf.Clamp(Mathf.Log10((float)GameState.OptionData.SoundUi100 / 100f) * 20f + 6f, -80f, 20f));
			    audioMixer.SetFloat("Bgm", Mathf.Clamp(Mathf.Log10((float)GameState.OptionData.SoundBgm100 / 100f) * 20f + 6f, -80f, 20f));
			    bgm.Play();
			    ToPadModeIfNeed();
			    return;
			}
			if (currentPhase != Phase.Title) return;
			if (InputManager.JustChangeIsGamePad) ToPadModeIfNeed();
			if (!InputManager.IsGamePad || isOpenChild) return;
			int direction = 0;
			if (InputManager.IsDownFrame(InputManager.InputType.UIUp)
			    || InputManager.IsDownLongRepeat(InputManager.InputType.UIUp, 20, 5))
			    direction = -1;
			if (InputManager.IsDownFrame(InputManager.InputType.UIDown)
			    || InputManager.IsDownLongRepeat(InputManager.InputType.UIDown, 20, 5))
			    ++direction;
			if (direction != 0)
			{
			    global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48, -1f, false);
			    currentSelectIndex = (buttonGroupManager.ButtonCount + currentSelectIndex + direction) % buttonGroupManager.ButtonCount;
			    buttonGroupManager.ForceSelect(buttonGroupManager.Get(currentSelectIndex));
			}
			if (InputManager.IsDownFrame(InputManager.InputType.UIDecide))
			    buttonGroupManager.InvokeOnClick(currentSelectIndex);
		}

		private void OnChangeAspect(ChangeWindowModeEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x46F910
			normalBackground.SetActive(GameState.OptionData.AspectRatioIndex == 0);
			wideBackground.SetActive(GameState.OptionData.AspectRatioIndex == 1);
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x46FA40
			if (Instance != this) return;
			Instance = null;
			EventManager.RemoveListener<ChangeWindowModeEvent>(OnChangeAspect);
			EventManager.RemoveListener<ChangeLanguageEvent>(OnChangeLanguageEvent);
		}

		public void OnClickNewGame()
		{
			// Recovered from GameAssembly.dll RVA 0x46FC90
			isOpenChild = true;
			if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.AnsweredFirstSexOption && !global::ExposureUnnoticed2.Scripts.Base.SystemManager.IsTrial)
			{
			    // Original continuation RVA 0x472A50.
			    global::ExposureUnnoticed2.ObjectUI.SelectSexOptionPanel.SelectSexOptionPanelView.Open(popupTransform,
			        () => global::ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel.SelectDifficultyPanelView.Open(popupTransform, null, false, OnReturn), OnReturn);
			}
			else
			    global::ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel.SelectDifficultyPanelView.Open(popupTransform, null, false, OnReturn);
		}

		public void OnClickContinue()
		{
			// Recovered from GameAssembly.dll RVA 0x46FEF0
			isOpenChild = true;
			global::ExposureUnnoticed2.ObjectUI.SaveDataSelectPanel.SaveDataSelectPanelView.Open(() =>
			{
			    // Original continuation RVA 0x472AF0.
			    global::ExposureUnnoticed2.ObjectUI.BlurCanvas.BlurCanvasController.SetBlur(false);
			    isOpenChild = false;
			    ToPadModeIfNeed();
			}, popupTransform);
		}

		public void OnClickGraphics()
		{
			// Recovered from GameAssembly.dll RVA 0x46FF80
			isOpenChild = true;
			global::ExposureUnnoticed2.ObjectUI.OptionMenu.GraphicsOption.GraphicsOptionPanelView.Open(popupTransform, OnReturn);
		}

		public void OnClickLanguage()
		{
			// Recovered from GameAssembly.dll RVA 0x470010
			isOpenChild = true;
			global::ExposureUnnoticed2.ObjectUI.ChooseDildoPanelView.ChooseLanguageView.Open(popupTransform, OnReturn);
		}

		private void OnReturn()
		{
			// Recovered from GameAssembly.dll RVA 0x4700A0
			isOpenChild = false;
		}

		public void OnClickExitGame()
		{
		}
	// Recovered original member RVA 0x470260: <Awake>b__30_0
		private void OnModelsBundleLoaded()
        {
            // Original asset callback RVA 0x470D20, field offset 0x328.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "DildoFloor", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                GameObject[] prefabs = referencer.MultipleDroppedPrefabs;
                if (prefabs is null) throw new System.NullReferenceException();
                prefabs[0] = (global::UnityEngine.GameObject)loadAsset;
            }, "");
            // Original asset callback RVA 0x470DD0, field offset 0x328.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "DildoWall", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                GameObject[] prefabs = referencer.MultipleDroppedPrefabs;
                if (prefabs is null) throw new System.NullReferenceException();
                prefabs[1] = (global::UnityEngine.GameObject)loadAsset;
            }, "");
            // Original asset callback RVA 0x470E80, field offset 0x320.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "DroppedCoat", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                GameObject[] prefabs = referencer.DroppedPrefabs;
                if (prefabs is null) throw new System.NullReferenceException();
                prefabs[0] = (global::UnityEngine.GameObject)loadAsset;
            }, "");
            // Original asset callback RVA 0x470F30, field offset 0x320.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "DroppedHandcuffKey", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                GameObject[] prefabs = referencer.DroppedPrefabs;
                if (prefabs is null) throw new System.NullReferenceException();
                prefabs[5] = (global::UnityEngine.GameObject)loadAsset;
            }, "");
            // Original asset callback RVA 0x470FE0, field offset 0x320.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "DroppedVibeRemocon", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                GameObject[] prefabs = referencer.DroppedPrefabs;
                if (prefabs is null) throw new System.NullReferenceException();
                prefabs[6] = (global::UnityEngine.GameObject)loadAsset;
            }, "");
            // Original asset callback RVA 0x471090, field offset 0x348.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Player", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.Player = (global::UnityEngine.GameObject)loadAsset;
            }, "");
            // Original asset callback RVA 0x4711B0, field offset 0x280.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Sweat Texture [Less] -Selestia- 1", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.SweatBody = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x471300, field offset 0x288.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Selestia_body_Inmon", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.Inmon = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x471450, field offset 0x290.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Selestia_bodyRakugaki", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.Rakugaki = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x4715A0, field offset 0x298.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Selestia_bodypaint", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.ClothesPaint = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x4716F0, field offset 0x2a0.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Selestia_body_DarkTatto", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.TattooDark = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x471840, field offset 0x2a8.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Selestia_body_Flower1", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.TattooFlower1 = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x471990, field offset 0x2b0.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Selestia_body_Flower2", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.TattooFlower2 = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x471AE0, field offset 0x2b8.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Selestia_body_Lotus", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.TattooLotus = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x471C30, field offset 0x2c0.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Selestia_body_Satanism", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.TattooSatanism = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x471D80, field offset 0x2c8.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Selestia_body_Slave", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.TattooSlave = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x471ED0, field offset 0x2d0.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "Selestia_bodyRubberSuit", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.RubberSuit = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x472020, field offset 0x2d8.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "otinpov3_texturePaint", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.FutanariPaint = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x472170, field offset 0x2e8.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "otinpov3_texture", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.FutanariMaterial = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x4722C0, field offset 0x2f0.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "otinpov3_Rubber", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.FutanariRubberMaterial = (global::UnityEngine.Material)loadAsset;
            }, "");
            // Original asset callback RVA 0x472410, field offset 0x310.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "BodySuit_Non NP_Black", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.RubberSuitBlack = (global::UnityEngine.Texture)loadAsset;
            }, "");
            // Original asset callback RVA 0x472560, field offset 0x318.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "BodySuit_Non NP_Black1", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.RubberSuitWhite = (global::UnityEngine.Texture)loadAsset;
            }, "");
            // Original asset callback RVA 0x4726B0, field offset 0x30.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "MCustomizeColor", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.MCustomizeColor = (global::ExposureUnnoticed2.Master.Common.MCustomizeColor)loadAsset;
            }, "");
            // Original asset callback RVA 0x4727F0, field offset 0x68.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "MHair", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.MHair = (global::ExposureUnnoticed2.Master.Hair.MHair)loadAsset;
            }, "");
            // Original asset callback RVA 0x472930, field offset 0x350.
            global::IKA9nt.Encrypter.EncrypterCore.LoadAssetAsync("models", "SimpleManustia", loadAsset =>
            {
                ++IsEndLoadCount;
                var referencer = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
                if (referencer is null) throw new System.NullReferenceException();
                referencer.UiPlayer = (global::UnityEngine.GameObject)loadAsset;
            }, "");
        }
	}
}
