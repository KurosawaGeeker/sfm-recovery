using System;
using System.Collections.Generic;
using Common.CommonUI.CommonSettingItem;
using ExposureUnnoticed2.Master.Hair;
using ExposureUnnoticed2.ObjectUI.BarberMenu;
using ExposureUnnoticed2.ObjectUI.CommonUI.CommonTab;
using ExposureUnnoticed2.ObjectUI.OptionMenu;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu
{
	public class CustomizeMenuView : MonoBehaviour
	{
		[SerializeField]
		private CommonTabView customizeCommonTabView;

		[SerializeField]
		private GameObject bodyView;

		[SerializeField]
		private GameObject faceView;

		[SerializeField]
		private GameObject hairView;

		[SerializeField]
		private GameObject paintView;

		[SerializeField]
		private GameObject futanariMask;

		[SerializeField]
		private Transform paintItemParent;

		[SerializeField]
		private GameObject paintItemPrefab;

		[SerializeField]
		private CustomizeLoadoutView customizeLoadoutView;

		[SerializeField]
		private TextMeshProUGUI facialCustomReleaseText;

		[SerializeField]
		private CustomizeBarItemView height;

		[SerializeField]
		private CustomizeBarItemView head;

		[SerializeField]
		private CustomizeBarItemView neck;

		[SerializeField]
		private CustomizeBarItemView neckThick;

		[SerializeField]
		private CustomizeBarItemView boobs;

		[SerializeField]
		private CustomizeBarItemView superBoobs;

		[SerializeField]
		private CustomizeBarItemView hyperBoobs;

		[SerializeField]
		private CustomizeBarItemView tit;

		[SerializeField]
		private CustomizeBarItemView butt;

		[SerializeField]
		private CustomizeBarItemView waist;

		[SerializeField]
		private CustomizeBarItemView legInterval;

		[SerializeField]
		private CustomizeBarItemView lowerBody;

		[SerializeField]
		private CustomizeBarItemView lowerBodyLength;

		[SerializeField]
		private CustomizeBarItemView lowerLeg;

		[SerializeField]
		private CustomizeBarItemView lowerLegLength;

		[SerializeField]
		private CustomizeBarItemView upperBody;

		[SerializeField]
		private CustomizeBarItemView upperBodyLength;

		[SerializeField]
		private CustomizeBarItemView upperBody2;

		[SerializeField]
		private CustomizeBarItemView upperBodyLength2;

		[SerializeField]
		private CustomizeBarItemView arm;

		[SerializeField]
		private CustomizeBarItemView shoulderWidth;

		[SerializeField]
		private CustomizeBarItemView imnou1;

		[SerializeField]
		private CustomizeBarItemView inmou2;

		[SerializeField]
		private CustomizeColorItemView CustomizeColorItemView;

		[SerializeField]
		private CustomizeBarItemView inmouColorR;

		[SerializeField]
		private CustomizeBarItemView inmouColorG;

		[SerializeField]
		private CustomizeBarItemView inmouColorB;

		[SerializeField]
		private CustomizeBarItemView skinColor;

		[SerializeField]
		private CustomizeCheckItemView isSkinColorFree;

		[SerializeField]
		private CustomizeColorItemView skinColorFree;

		[SerializeField]
		private GameObject skinColorMask;

		[SerializeField]
		private GameObject skinColorFreeMask;

		[SerializeField]
		private CustomizeCheckItemView wakige;

		[SerializeField]
		private GameObject futanariOptionParent;

		[SerializeField]
		private CustomizeBarItemView futanariSaoSize;

		[SerializeField]
		private CustomizeBarItemView futanariTamaSize;

		[SerializeField]
		private CustomizeBarItemView futanariStraight;

		[SerializeField]
		private CustomizeBarItemView futanariKitouSize;

		[SerializeField]
		private CustomizeBarItemView futanariWidth;

		[SerializeField]
		private CustomizeCheckItemView futanariTamaVisible;

		[SerializeField]
		private CustomizeBarItemView loliFace;

		[SerializeField]
		private CustomizeBarItemView eyeOpenness;

		[SerializeField]
		private CustomizeBarItemView eyeSize;

		[SerializeField]
		private CustomizeBarItemView hitomiSize;

		[SerializeField]
		private CustomizeBarItemView tareme;

		[SerializeField]
		private CustomizeBarItemView eyesDist;

		[SerializeField]
		private CustomizeBarItemView eyeHeight;

		[SerializeField]
		private CustomizeCheckItemView eyeDark;

		[SerializeField]
		private CustomizeBarItemView matsugeThick;

		[SerializeField]
		private CustomizeBarItemView matsugeLength;

		[SerializeField]
		private CustomizeBarItemView matsugeColorR;

		[SerializeField]
		private CustomizeBarItemView matsugeColorG;

		[SerializeField]
		private CustomizeBarItemView matsugeColorB;

		[SerializeField]
		private CustomizeColorItemView matsugeColor;

		[SerializeField]
		private CustomizeBarItemView mayuThick;

		[SerializeField]
		private CustomizeBarItemView mayuColorR;

		[SerializeField]
		private CustomizeBarItemView mayuColorG;

		[SerializeField]
		private CustomizeBarItemView mayuColorB;

		[SerializeField]
		private CustomizeColorItemView mayuColor;

		[SerializeField]
		private CustomizeBarItemView mayuDog;

		[SerializeField]
		private CustomizeBarItemView cheekFat;

		[SerializeField]
		private CustomizeBarItemView earElf;

		[SerializeField]
		private CustomizeBarItemView chinLength;

		[SerializeField]
		private CustomizeBarItemView eyePattern;

		[SerializeField]
		private CustomizeCheckItemView oddEye;

		[SerializeField]
		private CustomizeBarItemView eyeLeftPattern;

		[SerializeField]
		private GameObject oddEyeMask;

		[SerializeField]
		private CustomizeCheckItemView yaeba;

		[SerializeField]
		private SettingSelectItemView facialTest;

		[SerializeField]
		private Transform hairItemParent;

		[SerializeField]
		private SettingSelectItemView hairType;

		[SerializeField]
		private CustomizeBarItemView hairTextureId;

		[SerializeField]
		private CustomizeBarItemView hairR;

		[SerializeField]
		private CustomizeBarItemView hairG;

		[SerializeField]
		private CustomizeBarItemView hairB;

		[SerializeField]
		private CustomizeColorItemView hairColor;

		[SerializeField]
		private CustomizeBarItemView hairShadeColor;

		private Action onClose;

		private Animator animator;

		private List<MHairRecord> releaseHairRecords = new List<MHairRecord>();

		private MHairRecord currentHair;

		private MHairRecord.HairCustomizeData currentHairCustomizeData;

		private string[] facialTestStrings;

		public static CustomizeMenuView Instance { get; private set; }

		public int FacialTest { get; private set; }

		[field: SerializeField]
		public CustomizeFacialPanelView customizeFacialPanelView { get; private set; }

		public void Initialize(Action onClose, ClosetMenuView parent)
		{
Instance = this;
this.onClose = onClose;
customizeLoadoutView.Initialize();
animator = GetComponent<global::UnityEngine.Animator>();
currentHair = global::ExposureUnnoticed2.Master.Hair.MHair.Get(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CurrentHair);
OnClickCategory(0);
ResetFacialTestStrings();
RefreshItemView();
customizeFacialPanelView.Initialize();
var releaseCaptionParent = facialCustomReleaseText.transform.parent.gameObject;
int releaseNeedRp = global::ExposureUnnoticed2.Master.Rank.MRank.GetByRank(global::ExposureUnnoticed2.Master.RankRelease.MRankRelease.Get((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)115).ReleaseRank).NeedRp;
releaseCaptionParent.SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ExposeExp < releaseNeedRp);
facialCustomReleaseText.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_release_at_rank", global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.GetRankNameAutoHide(global::ExposureUnnoticed2.Master.RankRelease.MRankRelease.Get((global::ExposureUnnoticed2.Master.RankRelease.RankReleaseType)115).ReleaseRank));
var names = new global::System.Collections.Generic.List<string>();
int count = 0;
int selected = 0;
foreach (var record in global::ExposureUnnoticed2.Master.Hair.MHair.GetAll())
{
    if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.HairTypeList.Contains(record.Key))
    {
        names.Add(global::ExposureUnnoticed2.Master.Hair.MHair.Get(record.Key).NameKey);
        releaseHairRecords.Add(global::ExposureUnnoticed2.Master.Hair.MHair.Get(record.Key));
        if (record.Key == global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CurrentHair) selected = count;
        count++;
    }
}
hairType.Initialize(index =>
{
    currentHair = releaseHairRecords[index];
    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.ChangeHair(releaseHairRecords[index]);
    OnChangeHairType();
    global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() => global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent), 1);
}, global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_customize_hair"), names.ToArray(), selected);
customizeCommonTabView.Initialize(OnClickCategory, () => !parent.IsActivePanel());
futanariOptionParent.SetActive(!global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.FutanariDisable);
RefreshPaintView();
futanariMask.SetActive(!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveFutanari);
}

		private void RefreshPaintView()
		{
global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(paintItemParent);
global::System.Linq.Enumerable.ToArray(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.BodyPaintTypeDict.Keys);
foreach (var kvp in global::ExposureUnnoticed2.Master.BodyPaint.MBodyPaint.GetAll())
{
    if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringBodyPaint(kvp.Key)) continue;
    var item = global::UnityEngine.Object.Instantiate(paintItemPrefab, paintItemParent).GetComponent<global::ExposureUnnoticed2.ObjectUI.OptionMenu.CustomizeCheckItemView>();
    item.InitializeForPlayerCustomize(global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipBodyPaint(kvp.Key), val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.BodyPaintTypeDict[kvp.Key] = val; }, global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(global::ExposureUnnoticed2.Master.BodyPaint.MBodyPaint.Get(kvp.Key).NameKey));
}
}

		public void Show()
		{
// Complete original native body draft; native/CLR acceptance pending.
gameObject.SetActive(true); customizeCommonTabView.OnClickTab(customizeCommonTabView.CurrentSelect);
}

		private void RefreshItemView()
		{
// Complete original family draft; native/CLR acceptance pending.
height.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHeight, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHeight = val; }, false, false);
head.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHead, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHead = val; }, true, false);
neck.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNeck, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNeck = val; }, false, false);
neckThick.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNeckThick, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNeckThick = val; }, false, false);
boobs.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeBoobs, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeBoobs = val; }, false, false);
superBoobs.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSuperBoobs, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSuperBoobs = val; }, false, true);
hyperBoobs.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHyperBoobs, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHyperBoobs = val; }, false, true);
tit.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNipple, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNipple = val; }, false, true);
butt.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeButt, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeButt = val; }, false, true);
waist.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeWaist, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeWaist = val; }, false, false);
legInterval.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLegInterval, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLegInterval = val; }, false, false);
lowerBody.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeThighs, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeThighs = val; }, false, true);
lowerBodyLength.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeThighsLength, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeThighsLength = val; }, false, false);
lowerLeg.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLowerLeg, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLowerLeg = val; }, false, false);
lowerLegLength.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLowerLegLength, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLowerLegLength = val; }, false, false);
upperBody.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBody, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBody = val; }, false, false);
upperBody2.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBody2, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBody2 = val; }, false, false);
upperBodyLength.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBodyLength, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBodyLength = val; }, false, false);
upperBodyLength2.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBodyLength2, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBodyLength2 = val; }, false, false);
arm.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeArm, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeArm = val; }, false, false);
shoulderWidth.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeShoulder, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeShoulder = val; }, false, false);
skinColor.SetMax(global::ExposureUnnoticed2.Master.Common.MCustomizeColor.Instance.SkinPattern.Length - 1);
skinColor.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColor, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColor = val; }, false, false);
isSkinColorFree.InitializeForPlayerCustomize(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomIsSkinColorFree, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomIsSkinColorFree = val; skinColorMask.SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomIsSkinColorFree); skinColorFreeMask.SetActive(!global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomIsSkinColorFree); });
skinColorFree.Initialize(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorR, global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorG, global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorB, (r, g, b) => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorR = r; global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorG = g; global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorB = b; global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent); });
skinColorMask.SetActive(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomIsSkinColorFree);
skinColorFreeMask.SetActive(!global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomIsSkinColorFree);
imnou1.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou1, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou1 = val; }, false, false);
inmou2.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou2, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou2 = val; }, false, false);
inmouColorR.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorR, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorR = val; }, false, false);
inmouColorG.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorG, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorG = val; }, false, false);
inmouColorB.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorB, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorB = val; }, false, false);
CustomizeColorItemView.Initialize(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorR, global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorG, global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorB, (r, g, b) => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorR = r; global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorG = g; global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorB = b; global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent); });
futanariSaoSize.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariSaoSize, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariSaoSize = val; }, false, false);
futanariTamaSize.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariTamaSize, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariTamaSize = val; }, false, false);
futanariStraight.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariStraight, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariStraight = val; }, false, false);
futanariKitouSize.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariKitouSize, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariKitouSize = val; }, false, false);
futanariWidth.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariWidth, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariWidth = val; }, false, false);
futanariTamaVisible.InitializeForPlayerCustomize(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariTamaVisible, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariTamaVisible = val; });
loliFace.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLoliFace, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLoliFace = val; }, true, false);
eyeOpenness.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeOpenness, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeOpenness = val; }, false, false);
eyeSize.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeSize, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeSize = val; }, false, false);
hitomiSize.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHitomiSize, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHitomiSize = val; }, false, false);
tareme.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeTareme, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeTareme = val; }, false, false);
eyesDist.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeDist, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeDist = val; }, false, false);
eyeHeight.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeHeight, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeHeight = val; }, false, false);
eyeDark.InitializeForPlayerCustomize(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeDark, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeDark = val; });
oddEye.InitializeForPlayerCustomize(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeOddEye, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeOddEye = val; oddEyeMask.SetActive(!global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeOddEye); });
matsugeThick.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMatsugeThick, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMatsugeThick = val; }, false, false);
matsugeLength.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMatsugeLength, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMatsugeLength = val; }, false, false);
matsugeColorR.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorR, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorR = val; }, false, false);
matsugeColorG.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorG, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorG = val; }, false, false);
matsugeColorB.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorB, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorB = val; }, false, false);
matsugeColor.Initialize(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorR, global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorG, global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorB, (r, g, b) => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorR = r; global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorG = g; global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorB = b; global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent); });
mayuThick.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMayuThick, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMayuThick = val; }, false, false);
mayuColorR.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorR, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorR = val; }, false, false);
mayuColorG.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorG, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorG = val; }, false, false);
mayuColorB.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorB, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorB = val; }, false, false);
mayuColor.Initialize(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorR, global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorG, global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorB, (r, g, b) => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorR = r; global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorG = g; global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorB = b; global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent); });
mayuDog.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMayuDog, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMayuDog = val; }, false, false);
cheekFat.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeCheekFat, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeCheekFat = val; }, false, false);
earElf.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEarElf, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEarElf = val; }, false, false);
chinLength.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeChinLength, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeChinLength = val; }, false, false);
eyePattern.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyePattern, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyePattern = val; }, false, false);
eyeLeftPattern.InitializeForPlayerCustom(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLeftPattern, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLeftPattern = val; }, false, false);
yaeba.InitializeForPlayerCustomize(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeYaeba, val => { global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeYaeba = val; });
oddEyeMask.SetActive(!global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeOddEye);
OnChangeHairType();
}

		private void OnChangeHairType()
		{
// Complete original family draft; native/CLR acceptance pending.
currentHairCustomizeData = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.HairCustomizeDataDict[currentHair.Type];
hairTextureId.SetMax(currentHair.HairTextures.Length - 1);
hairTextureId.InitializeForPlayerCustom(currentHairCustomizeData.TextureId, val => { currentHairCustomizeData.TextureId = val; });
hairR.InitializeForPlayerCustom(currentHairCustomizeData.R, val => { currentHairCustomizeData.R = val; });
hairG.InitializeForPlayerCustom(currentHairCustomizeData.G, val => { currentHairCustomizeData.G = val; });
hairB.InitializeForPlayerCustom(currentHairCustomizeData.B, val => { currentHairCustomizeData.B = val; });
hairColor.Initialize(currentHairCustomizeData.R, currentHairCustomizeData.G, currentHairCustomizeData.B, (r, g, b) => { currentHairCustomizeData.R = r; currentHairCustomizeData.G = g; currentHairCustomizeData.B = b; global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent); });
hairShadeColor.InitializeForPlayerCustom(currentHairCustomizeData.ShadeColor, val => { currentHairCustomizeData.ShadeColor = val; });
}

		private void ResetFacialTestStrings()
		{
facialTestStrings = new[]
{
    CustomizeFacialItemView.FacialNames[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace0Id],
    CustomizeFacialItemView.FacialNames[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace1Id],
    CustomizeFacialItemView.FacialNames[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace2Id],
    CustomizeFacialItemView.FacialNames[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace3Id],
    CustomizeFacialItemView.FacialNames[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFaceIkuId],
    CustomizeFacialItemView.FacialNames[global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFaceIkuAfterId]
};
facialTest.Initialize(index =>
{
    global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerFaceController.OverrideFaceId = index switch
    {
        0 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace0Id,
        1 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace1Id,
        2 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace2Id,
        3 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFace3Id,
        4 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFaceIkuId,
        5 => global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerFaceIkuAfterId,
    };
}, "", facialTestStrings);
}

		public void OnClickCategory(int index)
		{
bodyView.SetActive(false);
faceView.SetActive(false);
hairView.SetActive(false);
paintView.SetActive(false);
global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerFaceController.OverrideFaceId = -1;
if (index != 4)
{
    customizeLoadoutView.Hide();
    currentHair = global::ExposureUnnoticed2.Master.Hair.MHair.Get(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CurrentHair);
    int selectIndex = 0;
    for (int i = 0; i < releaseHairRecords.Count; i++)
    {
        if (releaseHairRecords[i] == currentHair) { selectIndex = i; break; }
    }
    hairType.ForceSelect(selectIndex);
    RefreshItemView();
    RefreshPaintView();
    if (index == 5) { customizeFacialPanelView.Open(); return; }
}
customizeFacialPanelView.Hide();
switch (index)
{
    case 0: bodyView.SetActive(true); break;
    case 1: faceView.SetActive(true); facialTest.ForceSelect(0, true); ResetFacialTestStrings(); break;
    case 2: hairView.SetActive(true); OnChangeHairType(); break;
    case 3: paintView.SetActive(true); break;
    case 4: customizeLoadoutView.Open(); break;
    case 5: customizeFacialPanelView.Open(); break;
}
}

		public void OnClose()
		{
// Complete original native body draft; native/CLR acceptance pending.
Instance = null; customizeLoadoutView.Hide(); customizeFacialPanelView.Hide();
}

		public void OnClickDefaultBody()
		{
            // Original RVA 0x5384C0; no action on the left, reset only on confirmation.
            global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonPopupView.Open(
                global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_common_confirm"),
                global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_customize_reset_body"),
                new global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonButtonData
                {
                    Text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_common_no"),
                    OnClick = () => { }
                },
                new global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonButtonData
                {
                    Text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_common_yes"),
                    OnClick = () =>
                    {
                        // Original confirmation callback 0x539250: preserve reset, event, refresh order.
                        global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.SetDefaultCustomBody();
                        global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent);
                        RefreshItemView();
                    }
                }, false);
		}

		public void OnClickDefaultFace()
		{
            // Original RVA 0x5388C0; no action on the left, reset only on confirmation.
            global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonPopupView.Open(
                global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_common_confirm"),
                global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_customize_reset_face"),
                new global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonButtonData
                {
                    Text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_common_no"),
                    OnClick = () => { }
                },
                new global::ExposureUnnoticed2.ObjectUI.CommonPopup.CommonButtonData
                {
                    Text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_common_yes"),
                    OnClick = () =>
                    {
                        // Original confirmation callback 0x539300: preserve reset, event, refresh order.
                        global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.SetDefaultCustomFace();
                        global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent);
                        RefreshItemView();
                    }
                }, false);
		}

		public void OnClickAdjustHeight()
		{
            // Original RVA 0x538CC0 toggles only this option; original consumers apply adjustment.
            var custom = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData;
            custom.CustomizeAdjustHeight = !custom.CustomizeAdjustHeight;
            global::ExposureUnnoticed2.Scripts.Base.EventManager.Broadcast(global::ExposureUnnoticed2.Scripts.Base.Events.ChangeBodyEvent);
		}
	}
}
