using System;
using System.Collections;
using System.Reflection;
using ExposureUnnoticed2.Master.AdultGoods;
using ExposureUnnoticed2.ObjectUI.InventoryPanel;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace SfmMvp
{
    // Owned presentation only. Every successful transaction invokes the existing original persistent button.
    [DefaultExecutionOrder(60)]
    public sealed class EquipmentManualPanel : MonoBehaviour
    {
        public EquipmentBootstrapDraft Equipment;
        public OriginalMovementMvpBridge Bridge;
        public CoreGameplayPanel Shop;

        public bool Busy { get; private set; }
        public bool Ready => bound && InputsReady;
        public bool Initialized => bound;
        public bool InteractiveSmoke => interactiveSmoke && !standardQa;
        public string Failure { get; private set; }
        public string Error => Failure;
        public string Status { get; private set; } = "先在 F8 商店购买胶带。";
        public int OriginalButtonInvocations { get; private set; }
        public bool LastStateChanged { get; private set; }
        public bool LastBefore { get; private set; }
        public bool LastAfter { get; private set; }

        static EquipmentManualPanel active;
        const BindingFlags F = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
        const MAdultGoodsType Supported = (MAdultGoodsType)6;
        bool open, bound, ownsRows, standardQa, interactiveSmoke;
        object boundAccessor;
        Font font;
        MethodInfo rowMethod, invokeMethod, refreshMethod, cleanupMethod;
        FieldInfo canvasField, hostField, observedField;
        Rect Launcher => new Rect(Screen.width - 260, 72, 245, 32);
        Rect Area => new Rect(Screen.width - 440, 112, 425, Mathf.Min(330, Screen.height - 125));
        bool QaActive => standardQa || (Equipment && Equipment.RunQa);
        bool InputsReady => !QaActive && Equipment && Equipment.Data && Equipment.Data.Initialized &&
            Equipment.Player && Equipment.Player.Pca != null && Bridge && Bridge.Ready &&
            Equipment.GetComponent<EquipmentSlowBinding>() && Equipment.GetComponent<EquipmentSlowBinding>().Ready &&
            SlowAssistManager.Instance != null;

        // Root must OR this into CoreGameplayPanel.PointerOverPanel before accepting manual mouse input.
        public static bool PointerOverPanel
        {
            get
            {
                if (!active || active.QaActive) return false;
                var p = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
                return active.Launcher.Contains(p) || (active.open && active.Area.Contains(p));
            }
        }

        void Awake()
        {
            var args = Environment.GetCommandLineArgs();
            standardQa = Array.IndexOf(args, "-sfmEquipmentSmoke") >= 0 || Array.IndexOf(args, "-sfmOriginalActionSmoke") >= 0;
            interactiveSmoke = Array.IndexOf(args, "-sfmEquipmentInteractiveSmoke") >= 0;
            active = this;
        }
        void Update()
        {
            if (QaActive) return;
            if (Input.GetKeyDown(KeyCode.F10)) SetOpen(!open);
            if (open && Bridge && Bridge.legacyMotor && Bridge.legacyMotor.ControlsCaptured) ReleasePointer();
        }
        void ReleasePointer()
        {
            if (Bridge && Bridge.legacyMotor) Bridge.legacyMotor.ControlsCaptured = false;
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        public void SetOpen(bool value)
        {
            if (QaActive) return;
            open = value;
            if (!open) return;
            ReleasePointer();
            if (!bound && InputsReady && !Busy) Begin(Prepare());
        }

        void CacheOwnedApi()
        {
            var t = typeof(EquipmentBootstrapDraft);
            rowMethod = t.GetMethod("Type6Row", F, null, Type.EmptyTypes, null);
            invokeMethod = t.GetMethod("InvokeOriginalPersistentEquip", F, null, new[] { typeof(InventoryEquipItemView) }, null);
            refreshMethod = t.GetMethod("RefreshOriginalRows", F, null, Type.EmptyTypes, null);
            cleanupMethod = t.GetMethod("CleanupOriginalViewListeners", F, null, Type.EmptyTypes, null);
            canvasField = t.GetField("canvasHost", F);
            hostField = t.GetField("inactivePanelHost", F);
            observedField = t.GetField("observedViews", F);
            if (rowMethod == null || invokeMethod == null || refreshMethod == null || cleanupMethod == null ||
                canvasField == null || hostField == null || observedField == null)
                throw new MissingMemberException("The reviewed owned EquipmentBootstrapDraft API differs.");
        }
        void EnsureBound()
        {
            if (!InputsReady) throw new InvalidOperationException("Original actor/Slow equipment inputs are not ready.");
            if (bound)
            {
                if (!ReferenceEquals(boundAccessor, Equipment.Player.Pca))
                    throw new InvalidOperationException("The original actor changed; a fresh manual panel is required.");
                return;
            }
            CacheOwnedApi();
            if (canvasField.GetValue(Equipment) is GameObject existing && existing)
                throw new InvalidOperationException("The owned original row fixture already has another driver.");
            Equipment.BindOriginalActorAndSound();
            boundAccessor = Equipment.Player.Pca;
            bound = true;
        }
        IEnumerator Prepare()
        {
            EnsureBound();
            Status = Owned() ? "已拥有胶带，可以装备。" : "先在 F8 商店购买胶带。";
            yield break;
        }
        bool Owned() => Equipment && Equipment.Data && Equipment.Data.Initialized &&
            GameState.GameStateData != null && GameState.GameStateData.MAdultGoodsDict.ContainsKey(Supported);
        bool Equipped() => Owned() && GameState.GameStateData.MAdultGoodsDict[Supported];
        void HideOriginalCanvas()
        {
            var go = canvasField.GetValue(Equipment) as GameObject;
            if (!go || !go.GetComponent<Canvas>()) throw new InvalidOperationException("Original row Canvas is missing.");
            // Hide only rendering. Keep real original buttons active so their persistent UnityEvents remain valid.
            go.GetComponent<Canvas>().enabled = false;
        }
        void CleanupOwnedViewListeners()
        {
            cleanupMethod.Invoke(Equipment, null);
            ((IList)observedField.GetValue(Equipment)).Clear();
        }
        IEnumerator SettleRows()
        {
            // Existing frame dispatcher/Scheduler owns real delayed callbacks. Do not manually pump it here.
            for (int i = 0; i < 5; i++) yield return null;
            yield return new WaitForEndOfFrame();
        }
        IEnumerator ToggleRoutine()
        {
            EnsureBound();
            if (!Owned()) { Status = "请先在 F8 商店购买胶带。"; yield break; }
            if (!ownsRows)
            {
                Equipment.BuildOriginalRows();
                ownsRows = true;
            }
            else
            {
                // Original row OnDestroy is still a stub. Reuse the reviewed owned teardown before rebuilding.
                CleanupOwnedViewListeners();
                refreshMethod.Invoke(Equipment, null);
            }
            HideOriginalCanvas();
            yield return SettleRows();
            if (QaActive || !InputsReady) throw new InvalidOperationException("Manual input lost original runtime ownership.");
            LastBefore = Equipped();
            var row = (InventoryEquipItemView)rowMethod.Invoke(Equipment, null);
            invokeMethod.Invoke(Equipment, new object[] { row });
            OriginalButtonInvocations++;
            HideOriginalCanvas();
            yield return SettleRows();
            LastAfter = Equipped();
            LastStateChanged = LastAfter != LastBefore;
            Status = LastStateChanged ? (LastAfter ? "已装备胶带。" : "已卸下胶带。") : "当前状态暂时不能切换装备。";
        }
        void Begin(IEnumerator routine)
        {
            Busy = true;
            Failure = null;
            LastStateChanged = false;
            StartCoroutine(Drive(routine));
        }
        IEnumerator Drive(IEnumerator routine)
        {
            try
            {
                while (true)
                {
                    bool more;
                    object current = null;
                    try { more = routine.MoveNext(); if (more) current = routine.Current; }
                    catch (Exception e)
                    {
                        while (e is TargetInvocationException && e.InnerException != null) e = e.InnerException;
                        Failure = e.ToString();
                        Status = "装备操作未完成，详情已记录到日志。";
                        Debug.LogError("SFM_MANUAL_EQUIPMENT " + Failure);
                        yield break;
                    }
                    if (!more) break;
                    yield return current;
                }
            }
            finally { (routine as IDisposable)?.Dispose(); Busy = false; }
        }
        // GUI and root's separate interactive acceptance probe use the same entry. Never bypass QA exclusion.
        public bool ToggleType6()
        {
            if (QaActive || Busy || !InputsReady) return false;
            Begin(ToggleRoutine());
            return true;
        }
        public void OpenShop()
        {
            if (QaActive || Busy || !Shop || !Shop.Ready) return;
            // These two fields belong to the owned F8 presentation, not to Game rules or save state.
            typeof(CoreGameplayPanel).GetField("page", F).SetValue(Shop, 0);
            typeof(CoreGameplayPanel).GetField("category", F).SetValue(Shop, 3);
            SetOpen(false);
            Shop.SetOpen(true);
        }
        void OnGUI()
        {
            if (QaActive) return;
            if (!font) font = Font.CreateDynamicFontFromOSFont("Microsoft YaHei", 16);
            var previousFont = GUI.skin.font;
            GUI.skin.font = font;
            try
            {
                if (GUI.Button(Launcher, "装备胶带 [F10]")) SetOpen(!open);
                if (!open) return;
                GUILayout.BeginArea(Area, GUI.skin.box);
                GUILayout.BeginHorizontal(); GUILayout.Label("装备");
                if (GUILayout.Button("关闭", GUILayout.Width(55))) SetOpen(false);
                GUILayout.EndHorizontal();
                GUILayout.Label("当前支持：胶带（Type6）。其他类别仍在恢复。");
                if (!InputsReady) GUILayout.Label("正在等待角色与装备系统就绪。");
                else
                {
                    var record = MAdultGoods.Get(Supported);
                    GUILayout.Label("价格 " + record.Price + " RP　" + (Owned() ? "已拥有" : "尚未购买"));
                    GUILayout.Label("状态：" + (Equipped() ? "已装备" : "未装备"));
                    var wasEnabled = GUI.enabled;
                    GUI.enabled = wasEnabled && !Busy && Owned();
                    if (GUILayout.Button(Busy ? "处理中…" : Equipped() ? "卸下胶带" : "装备胶带")) ToggleType6();
                    GUI.enabled = wasEnabled && !Busy && Shop && Shop.Ready;
                    if (GUILayout.Button("前往商店购买 [F8]")) OpenShop();
                    GUI.enabled = wasEnabled;
                }
                GUILayout.Label(Status);
                GUILayout.EndArea();
            }
            finally { GUI.skin.font = previousFont; }
        }
        void OnDestroy()
        {
            if (ReferenceEquals(active, this)) active = null;
            if (ownsRows && Equipment && !QaActive)
            {
                try
                {
                    CleanupOwnedViewListeners();
                    foreach (var f in new[] { canvasField, hostField })
                    { var go = f.GetValue(Equipment) as GameObject; if (go) Destroy(go); }
                }
                catch (Exception e) { Debug.LogError("SFM_MANUAL_EQUIPMENT_CLEANUP " + e); }
            }
            if (font) Destroy(font);
        }
    }
}
