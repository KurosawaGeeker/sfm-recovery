using Common.Scripts.UI;
using ExposureUnnoticed2.Master.Achievement;
using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Master.BodyPaint;
using ExposureUnnoticed2.Master.Clothes;
using ExposureUnnoticed2.Master.Common;
using ExposureUnnoticed2.Master.Conversation;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Master.Costume;
using ExposureUnnoticed2.Master.DroneMission;
using ExposureUnnoticed2.Master.FastTravel;
using ExposureUnnoticed2.Master.Hair;
using ExposureUnnoticed2.Master.Item;
using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Master.Pants;
using ExposureUnnoticed2.Master.Rank;
using ExposureUnnoticed2.Master.RankRelease;
using ExposureUnnoticed2.Master.ReinforceEffect;
using ExposureUnnoticed2.Master.Sex;
using ExposureUnnoticed2.Master.Skill;
using ExposureUnnoticed2.Master.Skill.SkillMaxAccessoryNum;
using ExposureUnnoticed2.Master.Skill.SkillMental;
using ExposureUnnoticed2.Master.Skill.SkillSlow;
using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.ObjectUI.BarberMenu;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;

namespace ExposureUnnoticed2.Object3D.IngameManager
{
	public class AssetReferencer : MonoBehaviour
	{
		[Header("マスタ")]
		[SerializeField]
		public MStage MStage;

		[SerializeField]
		public MCommon MCommon;

		[SerializeField]
		public MCustomizeColor MCustomizeColor;

		[SerializeField]
		public MClothes MClothes;

		[SerializeField]
		public MSkill MSkill;

		[SerializeField]
		public MSex MSex;

		[SerializeField]
		public MAction MAction;

		[SerializeField]
		public MConversation MConversation;

		[SerializeField]
		public MAdultGoods mAdultGoods;

		[SerializeField]
		public MHair MHair;

		[SerializeField]
		public MItem MItem;

		[SerializeField]
		public MMission MMission;

		[SerializeField]
		public MRank MRank;

		[SerializeField]
		public MRankRelease MRankRelease;

		[SerializeField]
		public MCostume MCostume;

		[SerializeField]
		public MDroneMission MDroneMission;

		[SerializeField]
		public MSkillSlow MSkillSlow;

		[SerializeField]
		public MSkillStamina MSkillStamina;

		[SerializeField]
		public MSkillMental MSkillMental;

		[SerializeField]
		public MCosplay MCosplay;

		[SerializeField]
		public MReinforceEffect MReinforceEffect;

		[SerializeField]
		public MFastTravel MFastTravel;

		[SerializeField]
		public MSkillMaxAccessoryNum MSkillMaxAccessoryNum;

		[SerializeField]
		public MPants MPants;

		[SerializeField]
		public MBra MBra;

		[SerializeField]
		public MAchievement MAchievement;

		[SerializeField]
		public MBodyPaint MBodyPaint;

		[Header("UIPrefab")]
		[SerializeField]
		public GameObject FadePanel;

		[SerializeField]
		public GameObject StageNamePanelView;

		[SerializeField]
		public GameObject InGameSystemMenuView;

		[SerializeField]
		public GameObject NpcUiView;

		[SerializeField]
		public GameObject MissionFailedPanel;

		[SerializeField]
		public BasePanelView SaveDataSelectPanel;

		[SerializeField]
		public BasePanelView SkillPanel;

		[SerializeField]
		public BasePanelView BuyPanel;

		[SerializeField]
		public BasePanelView InteractMenuPanel;

		[SerializeField]
		public BasePanelView TryClothesPanel;

		[SerializeField]
		public GameObject ConversationPanel;

		[SerializeField]
		public BasePanelView InventoryPanel;

		[SerializeField]
		public BarberMenuView BarberMenu;

		[SerializeField]
		public BasePanelView CommonPopup;

		[SerializeField]
		public BasePanelView ClosetMenu;

		[SerializeField]
		public BasePanelView SleepBlackPanel;

		[SerializeField]
		public BasePanelView MissionMenuPanel;

		[SerializeField]
		public BasePanelView ChooseHandcuffsPanel;

		[SerializeField]
		public BasePanelView RankConfirmPanel;

		[SerializeField]
		public BasePanelView SleepSelectPanel;

		[SerializeField]
		public BasePanelView PcMenuPanel;

		[SerializeField]
		public BasePanelView ManualSavePanel;

		[SerializeField]
		public BasePanelView SystemMenu;

		[SerializeField]
		public BasePanelView OptionMenu;

		[SerializeField]
		public BasePanelView GraphicsPanel;

		[SerializeField]
		public BasePanelView InGameMenu;

		[SerializeField]
		public BasePanelView DroneMissionPanel;

		[SerializeField]
		public BasePanelView VibeRemoconReinforcePanel;

		[SerializeField]
		public BasePanelView DroneReinforcePanel;

		[SerializeField]
		public BasePanelView BuffPanel;

		[SerializeField]
		public BasePanelView SelectSexualityTypePanel;

		[SerializeField]
		public BasePanelView SkillSwitchPanel;

		[SerializeField]
		public BasePanelView OnlineShopPanel;

		[SerializeField]
		public BasePanelView ReinforcePanel;

		[SerializeField]
		public BasePanelView FastTravelPanel;

		[SerializeField]
		public BasePanelView SelectDifficultyPanel;

		[SerializeField]
		public BasePanelView SelectSexOptionPanel;

		[SerializeField]
		public BasePanelView ChooseDildoPanel;

		[SerializeField]
		public BasePanelView KillTimeWaitPanel;

		[SerializeField]
		public BasePanelView WaitPanel;

		[SerializeField]
		public BasePanelView ResultPanel;

		[SerializeField]
		public BasePanelView AchievementPanel;

		[SerializeField]
		public BasePanelView ChooseHandcuffsTimerPanel;

		[SerializeField]
		public BasePanelView ApplyGraphicsCountDownPopup;

		[SerializeField]
		public BasePanelView TutorialPanel;

		[SerializeField]
		public BasePanelView ColorSettingPanel;

		[SerializeField]
		public BasePanelView ChooseLanguagePanel;

		[SerializeField]
		public BasePanelView SexMenuPanel;

		[SerializeField]
		public BasePanelView NameEditPanel;

		[Header("Material")]
		[SerializeField]
		public Material SweatBody;

		[SerializeField]
		public Material Inmon;

		[SerializeField]
		public Material Rakugaki;

		[SerializeField]
		public Material ClothesPaint;

		[SerializeField]
		public Material TattooDark;

		[SerializeField]
		public Material TattooFlower1;

		[SerializeField]
		public Material TattooFlower2;

		[SerializeField]
		public Material TattooLotus;

		[SerializeField]
		public Material TattooSatanism;

		[SerializeField]
		public Material TattooSlave;

		[SerializeField]
		public Material RubberSuit;

		[SerializeField]
		public Material FutanariPaint;

		[SerializeField]
		public Material NightSpermDecal;

		[SerializeField]
		public Material FutanariMaterial;

		[SerializeField]
		public Material FutanariRubberMaterial;

		[SerializeField]
		public Material Fresnel;

		[SerializeField]
		public Material CensorPussy;

		[SerializeField]
		public Material CensorChinpo;

		[Header("Texture")]
		[SerializeField]
		public Texture RubberSuitBlack;

		[SerializeField]
		public Texture RubberSuitWhite;

		[Header("3DObjectPrefab")]
		[SerializeField]
		public GameObject[] DroppedPrefabs;

		[SerializeField]
		public GameObject[] MultipleDroppedPrefabs;

		[SerializeField]
		public GameObject[] HoldInHandClothesPrefabs;

		[SerializeField]
		public GameObject SpermBullet;

		[SerializeField]
		public GameObject SpermDecal;

		[SerializeField]
		public GameObject Player;

		[SerializeField]
		public GameObject UiPlayer;

		[SerializeField]
		[Header("Font")]
		public TMP_FontAsset fontKr;

		[SerializeField]
		public TMP_FontAsset fontSc;

		[SerializeField]
		public TMP_FontAsset fontTc;

		[SerializeField]
		public TMP_FontAsset defaultFont;

		[SerializeField]
		public Material[] fontMaterials;

		[SerializeField]
		[Header("その他")]
		public AudioMixer AudioMixer;

		[SerializeField]
		public AudioMixerGroup AudioMixerEnvironment;

		public static AssetReferencer Instance;

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x454510
			if (Instance != null)
			{
			    UnityEngine.Object.Destroy(gameObject);
			    return;
			}
			Instance = this;
		}
	}
}
