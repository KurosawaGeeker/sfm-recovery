using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ExposureUnnoticed2.Master.Accessory;
using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.ObjectUI.InventoryPanel;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Scripts.Sound;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;
using UnityEngine.UI;

namespace SfmMvp
{
    // Owned fixture ONLY. Root must install/guard the original equipment9+click6 first.
    // No original method is replaced. No owned transaction delegate is constructed.
    // Compile validation does not establish actual Unity success.
    public sealed class EquipmentBootstrapDraft : MonoBehaviour
    {
        public CoreGameplayData Data;
        public PlayerController Player;
        public Transform OriginalManustia;
        public GameObject OriginalInventoryPanelPrefab;
        public AudioClip EquipAdClip, SelectClip, DecideClip, BuyClip;
        public bool RunQa;
        public bool LeaveUiVisible;
        public bool Passed { get; private set; }
        public string Failure { get; private set; }
        public int Checks { get; private set; }
        public readonly List<string> Evidence = new List<string>();
        const BindingFlags F = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        InventoryPanelView panel;
        Transform content;
        GameObject inactivePanelHost, canvasHost;
        PlayerClassAccessor pca;
        int adultGoodsEvents;
        double rpAfterPurchase;
        readonly List<InventoryEquipItemView> observedViews = new List<InventoryEquipItemView>();
        static readonly string[] Paths = {
            "Armature/Hips/Anal/RotateTarget/Parent/AnalPlugHeart",
            "AdultGoodsParent/baibu.002",
            "Armature/Hips/Spine/Chest/Neck/Head/EyeMask Variant",
            "Armature/Hips/Spine/Chest/Shoulder_L/UpperArm_L/LowerArm_L/Hand_L/HandCuffParent/Handcuffs (2)",
            "Armature/Hips/Spine/Chest/Shoulder_L/UpperArm_L/LowerArm_L/Hand_L/HandCuffParent/Handcuffs (2)",
            "Armature/Hips/Spine/Chest/Shoulder_L/UpperArm_L/LowerArm_L/Hand_L/HandCuffParent/Handcuffs (2)",
            "AdultGoodsParent/Tape", "AdultGoodsParent/ローター",
            "Armature/Hips/Anal/RotateTarget/otinpo/ScalePoint/otinpo.root/DirectBone/RotatePoint/otinpo00/otinpo01/OnahoMachine/onahole_machine",
            "Armature/Hips/Anal/RotateTarget/Parent/piston_machine_dildo",
            "Armature/Hips/Manko/RotateTarget/piston_machine_dildo (1)"
        };
        static FieldInfo Field(Type t, string name) {
            for (; t != null; t = t.BaseType) {
                var f = t.GetField(name, F | BindingFlags.DeclaredOnly) ?? t.GetField("<"+name+">k__BackingField", F | BindingFlags.DeclaredOnly);
                if (f != null) return f;
            }
            throw new MissingFieldException(name);
        }
        static object Get(object o, string n) { return Field(o.GetType(), n).GetValue(o); }
        static void Set(object o, string n, object v) { Field(o.GetType(), n).SetValue(o, v); }
        static MethodInfo Method(Type t, string n) { return t.GetMethod(n, F) ?? throw new MissingMethodException(t.FullName, n); }
        static void Call(object o, string n) { Method(o.GetType(), n).Invoke(o, null); }
        void Need(bool value, string evidence) {
            Checks++; if (!value) throw new InvalidOperationException(evidence);
            Evidence.Add(evidence);
        }
        void OriginalBody(Type type, string name, int minimumBytes = 8) {
            var body = Method(type, name).GetMethodBody();
            Need(body != null && body.GetILAsByteArray().Length >= minimumBytes, "nonstub original body: "+type.Name+"."+name);
        }
        void OnAdultGoodsChanged(ChangeAdultGoodsEvent evt) { adultGoodsEvents++; }

        public void BindOriginalActorAndSound() {
            Need(Data && Data.Initialized && Player && Player.Pca != null && OriginalManustia, "existing original Core data/PCA/visible actor ready");
            pca = Player.Pca;
            Need(ReferenceEquals(PlayerController.Instance, Player), "real singleton is selected visible actor");
            var actualAvatars=Player.GetComponentsInChildren<PlayerAvatarObjectReferencer>(true);
            Need(actualAvatars.Length==1&&ReferenceEquals(actualAvatars[0],pca.PlayerAvatarObjectReferencer)&&
                (actualAvatars[0].transform==OriginalManustia||actualAvatars[0].transform.IsChildOf(OriginalManustia)||OriginalManustia.IsChildOf(actualAvatars[0].transform)),
                "PCA selected the unique existing visible actor avatar component");
            Need(Player.PlayerAvatarObjectReferencer==null||ReferenceEquals(Player.PlayerAvatarObjectReferencer,actualAvatars[0]),
                "neutral controller serialized avatar input is empty or already the actual actor");
            Player.PlayerAvatarObjectReferencer=actualAvatars[0];
            Need(ReferenceEquals(pca.PlayerAvatarObjectReferencer, Player.PlayerAvatarObjectReferencer), "original PCA uses actual avatar referencer");
            var slots = Paths.Select(path => {
                var tr = OriginalManustia.Find(path);
                if (!tr) throw new InvalidOperationException("Missing original attachment: "+path);
                return tr.gameObject;
            }).ToArray();
            for (int i=0;i<slots.Length;i++) for (int j=0;j<slots.Length;j++)
                Need(ReferenceEquals(slots[i],slots[j]) == (Paths[i] == Paths[j]), "original attachment identity "+i+":"+j);
            pca.PlayerAvatarObjectReferencer.Accessorys = slots;
            Need(slots.Length == 11 && slots.Distinct().Count() == 9 && slots[6].name == "Tape", "original 11-slot/9-object attachment layout");
            Need(GameState.GameStateData.MAdultGoodsDict != null && pca.PlayerState.AdultGoodsBuffList != null,
                "original constructor dictionaries and buff list exist");
            Need(MAdultGoods.GetAll().Count == 10 && (int)MAdultGoods.Get((MAdultGoodsType)6).MotionType == 7,
                "original ten-record table and Type6 MotionType7");
            var d=GameState.GameStateData;
            Need(MAdultGoods.GetAll().Keys.All(k=>d.AdultGoodsReinforceEffectDict.ContainsKey(k)) &&
                 MCosplay.GetAll().Values.SelectMany(x=>x.GetAll()).All(x=>d.CosplayPartsReinforceEffectDict.ContainsKey(x.UniqueId)),
                 "original reinforce dictionaries cover every real master record");
            Need(d.AdultGoodsReinforceEffectDict.Values.Concat(d.CosplayPartsReinforceEffectDict.Values).All(x=>x!=null&&x.Length==3&&x.All(y=>y==0)),
                 "fresh original constructor three-zero reinforcement arrays retained; nonzero profiles need original reinforce master");
            Need(InGameManager.Instance != null && InGameManager.Instance.VisibilityFogController != null,
                "real initialized InGameManager fog graph");
            Need(UiSoundPlayer.Instance && InGameSoundManager.Instance && EquipAdClip && SelectClip && DecideClip && BuyClip,
                "real original audio graph and four original clips");
            Need(Get(UiSoundPlayer.Instance,"audioSource") is AudioSource && Get(UiSoundPlayer.Instance,"audioSourceBgm") is AudioSource,
                "original UiSoundPlayer.Awake selected two real sources");
            var sound = InGameSoundManager.Instance;
            Set(sound,"EquipAd",new InGameSoundManager.AudioData { Clip=EquipAdClip, Volume=.3f });
            Set(sound,"Select",new InGameSoundManager.AudioData { Clip=SelectClip, Volume=1f });
            Set(sound,"Decide",new InGameSoundManager.AudioData { Clip=DecideClip, Volume=.2f });
            Set(sound,"Buy",new InGameSoundManager.AudioData { Clip=BuyClip, Volume=.6f });
            OriginalBody(typeof(InventoryPanelView),"RefreshAdultGoodsCategory",100);
            OriginalBody(typeof(InventoryEquipItemView),"Initialize",100);
            OriginalBody(typeof(InventoryEquipItemView),"OnClickEquip",10);
            OriginalBody(typeof(MAdultGoods),"UseItemProcess",50);
            OriginalBody(typeof(PlayerFacade),"ChangeAccessoryIfAble",40);
            OriginalBody(typeof(PlayerAnimationReceiver),"JustEquipAccessory",40);
            OriginalBody(typeof(PlayerAdultGoodsController),"ChangeAdultGoods",30);
            OriginalBody(typeof(PlayerAdultGoodsController),"CalcAdultGoodsEffect",100);
            OriginalBody(typeof(PlayerReinforceEffectManager),"Apply",100);
            // A body-length refusal is only a preflight. Root's exact body/MM/TM guard is authoritative.
            pca.PlayerReinforceEffectManager.Initialize(pca);
            pca.PlayerAnimationReceiver.Initialize(pca);
            pca.PlayerAdultGoodsController.Initialize(pca);
            Need(ReferenceEquals(Get(pca.PlayerAdultGoodsController,"pca"),pca), "original AdultGoods.Initialize bound real PCA");
        }

        public void BuildOriginalRows() {
            Need(OriginalInventoryPanelPrefab, "original serialized InventoryPanel prefab assigned");
            inactivePanelHost = new GameObject("Owned inactive original panel fixture");
            inactivePanelHost.SetActive(false);
            var original = Instantiate(OriginalInventoryPanelPrefab,inactivePanelHost.transform,false);
            panel = original.GetComponent<InventoryPanelView>();
            Need(panel, "original Panel component retained");
            content = (Transform)Get(panel,"adultGoodsItemParent");
            var group = (ButtonGroupManager)Get(panel,"adultGoodsButtonGroupManager");
            var equipPrefab = (GameObject)Get(panel,"equipmentItemPrefab");
            Need(content && group && group.transform == content && equipPrefab.GetComponent<InventoryEquipItemView>(),
                "original parent/group/prefab fields and same-Content identity");
            canvasHost = new GameObject("Owned equipment QA canvas",typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
            canvasHost.GetComponent<Canvas>().renderMode=RenderMode.ScreenSpaceOverlay;
            // All other original panel sections remain inactive. This does not open the original menu.
            content.SetParent(canvasHost.transform,false);
            content.gameObject.SetActive(true);
            Set(panel,"adultGoodsViewList",new List<InventoryEquipItemView>());
            RefreshOriginalRows();
        }
        void RefreshOriginalRows() {
            Call(panel,"RefreshAdultGoodsCategory");
            var rows=(List<InventoryEquipItemView>)Get(panel,"adultGoodsViewList");
            Need(rows.Count == MAdultGoods.GetAll().Count(x=>GameStateFacade.IsBringAdultGoods(x.Key)), "original Refresh enumerates all owned real records");
            foreach(var row in rows) {
                observedViews.Add(row);
                var callback=(Delegate)Get(row,"onClickEquip");
                Need(callback != null && ReferenceEquals(callback.Target,panel) && callback.Method.Name=="<RefreshAdultGoodsCategory>b__40_1",
                    "row owns original compiler-generated equipment callback");
                Need(Get(row,"selfButtonView") != null && Get(row,"name") is TMPro.TextMeshProUGUI && Get(row,"buttonView") is ButtonView,
                    "original Initialize acquired root interface and serialized TMP/child button");
            }
        }
        InventoryEquipItemView Type6Row() {
            var rows=(List<InventoryEquipItemView>)Get(panel,"adultGoodsViewList");
            foreach(var row in rows) if(Get(row,"master") is MAdultGoodsRecord r && (int)r.Type==6) return row;
            throw new InvalidOperationException("Original Refresh did not produce Type6 row");
        }
        void InvokeOriginalPersistentEquip(InventoryEquipItemView row) {
            Button selected=null;
            foreach(var button in row.GetComponentsInChildren<Button>(true)) {
                for(int i=0;i<button.onClick.GetPersistentEventCount();i++)
                    if(button.onClick.GetPersistentTarget(i)==row && button.onClick.GetPersistentMethodName(i)=="OnClickEquip") {
                        Need(selected==null,"exactly one original persistent equip listener"); selected=button;
                    }
            }
            Need(selected && selected.gameObject.activeInHierarchy,"real prefab equipment button is active");
            // Original serialized Unity event invokes original View.OnClickEquip and the original stored callback.
            selected.onClick.Invoke();
            foreach(var replacement in (List<InventoryEquipItemView>)Get(panel,"adultGoodsViewList"))
                if(!observedViews.Contains(replacement)) observedViews.Add(replacement);
        }
        void ValidateState(bool active, Dictionary<MAdultGoodsType,bool> untouched, MAdultGoodsRecord record) {
            var d=GameState.GameStateData;var state=pca.PlayerState;
            Need(d.CurrentRp==rpAfterPurchase,"equipment toggles preserve post-purchase RP");
            Need(d.MAdultGoodsDict[(MAdultGoodsType)6]==active,"original Type6 ownership active flag="+active);
            Need(pca.PlayerAvatarObjectReferencer.Accessorys[6].activeSelf==active,"real original Tape.activeSelf="+active);
            Need(!active || pca.PlayerAvatarObjectReferencer.Accessorys[6].activeInHierarchy,"equipped Tape is in the active visible hierarchy");
            Need(ReferenceEquals(state.EquippingAdultGoods,record),"original pending record retained");
            Need(untouched.All(x=>d.MAdultGoodsDict.TryGetValue(x.Key,out var now)&&now==x.Value),"all other original adult ownership flags preserved");
            Need(state.AdultGoodsBuffList.Count(x=>x.ItemIdParseInt==6)==(active?1:0),"original CalcEffect rebuilt Type6 buff membership");
            if(active) {
                var buff=state.AdultGoodsBuffList.Single(x=>x.ItemIdParseInt==6);
                Need(buff.RpBonus100==record.RpBonus&&buff.Strange100==record.StrangeBonus&&buff.Dokidoki100==record.DokidokiBonus&&(int)buff.SpecialType==1,
                    "real Type6 original record bonuses applied to buff");
            }
            float expected=0;string activeRecords="";foreach(var pair in d.MAdultGoodsDict)if(pair.Value){var r=MAdultGoods.Get(pair.Key);activeRecords+=(int)pair.Key+":"+(int)r.AdultGoodsEffectType+":"+r.EcstasyRisePerSecond+";";if((int)r.AdultGoodsEffectType==1)expected+=r.EcstasyRisePerSecond;}
            int actualBits=BitConverter.ToInt32(BitConverter.GetBytes(state.EcstasyRiseByVibratorPerSecond),0);
            int expectedBits=BitConverter.ToInt32(BitConverter.GetBytes(expected/100f),0);
            Need(actualBits==expectedBits,"original aggregate vibrator stored-f32 rate: actual="+state.EcstasyRiseByVibratorPerSecond.ToString("R")+"/"+actualBits.ToString("X8")+",expected="+(expected/100f).ToString("R")+"/"+expectedBits.ToString("X8")+",active="+activeRecords);
            // QA transient sentinel must be removed by actual original Apply; no Game method is hooked.
            Need(state.ReinforceEffectData.Equals(default(SumReinforceEffectData)),"original Apply reset all fifteen reinforce fields from unchanged original zero profile");
        }
        IEnumerator SettleOriginalUi() {
            // Existing original root frame pump owns Scheduler. Never pump Scheduler a second time here.
            for(int i=0;i<5;i++)yield return null;
            yield return new WaitForEndOfFrame();
            var rows=(List<InventoryEquipItemView>)Get(panel,"adultGoodsViewList");
            Need(content.childCount==rows.Count,"original deferred DestroyChildren left exactly current rows");
            foreach(var row in rows) {
                var callback=(Delegate)Get(row,"onClickEquip");
                Need(callback!=null && ReferenceEquals(callback.Target,panel) && callback.Method.Name=="<RefreshAdultGoodsCategory>b__40_1",
                    "refreshed row still uses original callback");
            }
        }
        public IEnumerator Qa() {
            Need(!Passed,"fresh QA run required");
            BindOriginalActorAndSound();
            var record=MAdultGoods.Get((MAdultGoodsType)6);
            Need(record && record.Price==50 && (int)record.MotionType==7 && (int)record.AdultGoodsEffectType==1 && record.RpBonus==5 && record.EcstasyRisePerSecond==1,
                "unmodified real Type6 record price50/motion7/effect1/RP5/rate1");
            Need((int)record.TransAttachActionType()==-1&&(int)record.TransDetachActionType()==-1,"original Type6 motion accessors choose immediate original JustEquip");
            Need(!Data.Owned(record),"fresh Type6 must be purchased through original shop");
            Need(GameState.GameStateData.CurrentRp>=record.Price,"explicit existing fixture budget covers original price");
            var before=GameState.GameStateData.CurrentRp;
            Data.Purchase(record);
            Need(Data.Owned(record)&&GameState.GameStateData.CurrentRp==before-record.Price,"original shop bought Type6 for exact original price");
            Need(!GameState.GameStateData.MAdultGoodsDict[(MAdultGoodsType)6],"purchase creates inactive original ownership");
            rpAfterPurchase=GameState.GameStateData.CurrentRp;
            var slowBinding=GetComponent<EquipmentSlowBinding>();
            Need(slowBinding&&slowBinding.Ready,"original Slow initialization ready: "+(slowBinding?slowBinding.Failure:"component missing"));
            Need(SlowAssistManager.Instance!=null,"original Slow singleton exists at equipment gate");
            Need((int)SlowAssistManager.Instance.currentState!=1,"original Slow state permits equipment: "+(int)SlowAssistManager.Instance.currentState);
            Need(PlayerFacade.Instance!=null&&SexManager.Instance!=null,"original PlayerFacade and SexManager initialized");
            var gateState=pca.PlayerState;var gateData=GameState.GameStateData;
            Need(PlayerFacade.Instance.CanUseHand(false),"original CanUseHand gate: action="+(int)gateState.CurrentAction.Type+",state="+(int)gateState.CurrentState+",crouch="+gateState.IsCrouch+",ecstasy="+gateState.IsEcstasyMotion+",handcuffs="+gateData.PlayerIsAttachHandcuffs+",mapHandcuffs="+gateData.PlayerIsAttachMapHandcuffs+",sex="+SexManager.Instance.IsSexMode);
            var untouched=GameState.GameStateData.MAdultGoodsDict.Where(x=>(int)x.Key!=6).ToDictionary(x=>x.Key,x=>x.Value);
            EventManager.AddListener<ChangeAdultGoodsEvent>(OnAdultGoodsChanged);
            try {
                BuildOriginalRows();yield return SettleOriginalUi();
                var first=Type6Row();
                var sentinel=pca.PlayerState.ReinforceEffectData;sentinel.RpBonus100=123456789;pca.PlayerState.ReinforceEffectData=sentinel;
                InvokeOriginalPersistentEquip(first);
                ValidateState(true,untouched,record);
                yield return SettleOriginalUi();
                Need(!first,"old first view destroyed by original Refresh");
                var second=Type6Row();
                Need((bool)Get((ButtonView)Get(second,"buttonView"),"IsEmitting"),"original equipped-row Initialize emitted child button");
                sentinel=pca.PlayerState.ReinforceEffectData;sentinel.RpBonus100=123456789;pca.PlayerState.ReinforceEffectData=sentinel;
                InvokeOriginalPersistentEquip(second);
                ValidateState(false,untouched,record);
                yield return SettleOriginalUi();
                Need(!second&&adultGoodsEvents==2,"two original transactions broadcast and destroy their old views");
                PlayerFacade.Instance.ChangeAccessoryIfAble(null);
                Need(pca.PlayerState.EquippingAdultGoods==null && pca.PlayerState.EquippingCostume==null && PlayerFacade.Instance.CanUseHand(false),
                    "original null-record path clears pending equipment before action regression");
                Passed=true;
            } finally {
                EventManager.RemoveListener<ChangeAdultGoodsEvent>(OnAdultGoodsChanged);
                CleanupOriginalViewListeners();
            }
        }
        void CleanupOriginalViewListeners() {
            // Original view OnDestroy is still a stub. Fixture teardown is explicit and not claimed as recovered UI lifecycle.
            var method=Method(typeof(InventoryEquipItemView),"OnChangeSomeEquipEvent");
            foreach(var view in observedViews.Distinct()) if(!ReferenceEquals(view,null)) {
                var listener=(Action<ChangeSomeEquipEvent>)Delegate.CreateDelegate(typeof(Action<ChangeSomeEquipEvent>),view,method);
                EventManager.RemoveListener<ChangeSomeEquipEvent>(listener);
            }
        }
        IEnumerator Start() {
            if(!RunQa)yield break;
            for(int i=0;i<300 && !(Data&&Data.Initialized&&Player&&Player.Pca!=null);i++)yield return null;
            var routine=Qa();
            // Do not flatten nested Unity enumerators or manually run original delayed callbacks.
            while(true) {
                bool more;
                try { more=routine.MoveNext(); }
                catch(Exception e) { Failure=e.ToString();Debug.LogError("Owned equipment QA failed: "+Failure);yield break; }
                if(!more)break;yield return routine.Current;
            }
            if(!LeaveUiVisible) { if(canvasHost)Destroy(canvasHost);if(inactivePanelHost)Destroy(inactivePanelHost); }
            Debug.Log("Owned equipment QA complete checks="+Checks+" passed="+Passed);
        }
    }
}
