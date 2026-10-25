using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Master.Rank;
using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Object3D.IngameManager;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.ObjectUI.CommonUI.CommonTab;
using ExposureUnnoticed2.ObjectUI.InGameMenu;
using ExposureUnnoticed2.ObjectUI.MissionMenuPanel;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SfmMvp
{
    // Owned integration test. Normal Players never create this component.
    // Only SDK input and existing serialized UI events drive the original game.
    public sealed class MissionMenuSmokeV1 : MonoBehaviour
    {
        const string Flag = "--sfm-mission-menu-smoke";
        const string OutputRoot = @"C:\Users\Administrator\sfm-layerfs-dev\mission-menu-v1";
        const BindingFlags Fields = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic;
        [Serializable] public sealed class TabSample { public int index, stage, items, expectedItems, lockedItems; public bool locked; public string rate, condition; }
        [Serializable] public sealed class Report
        {
            public string status = "running", failure, scene, output, persistentDataPath;
            public string inputMode = "Unity InputSystem virtual keyboard for Mission/Tab/Cancel; existing serialized Button.onClick for mission and region tabs";
            public bool passed, physicalKeyboardTested, shortcutOpened, shortcutClosed, parentOpened, childOpened, parentRestored, allUiClosed, inputRestored, savesUnchanged, progressionUnchanged, lockedRegionExercised;
            public int checks, tutorialCancels, residenceItems, residenceMasterItems, rank;
            public double beforeRp, afterRp;
            public List<string> steps = new List<string>(), errors = new List<string>(), screenshots = new List<string>(), keys = new List<string>();
            public List<string> presentation = new List<string>();
            public List<TabSample> tabs = new List<TabSample>();
        }
        Report report = new Report();
        string output, initialProgress;
        Dictionary<string, string> initialSaves;
        Keyboard keyboard;
        Mouse mouse;
        PlayerInput input;
        InputDevice[] previousDevices;
        string previousScheme;
        InputSettings.BackgroundBehavior previousBackground;
        bool previousRunInBackground, finished;
        float started;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Install()
        {
            string[] args = Environment.GetCommandLineArgs();
            int index = Array.IndexOf(args, Flag);
            if (index < 0) return;
            if (index + 1 >= args.Length) throw new ArgumentException(Flag + " requires a fresh output directory");
            string path = Path.GetFullPath(args[index + 1]);
            if (!path.StartsWith(Path.GetFullPath(OutputRoot) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Smoke output must be a fresh child of " + OutputRoot);
            if (Directory.Exists(path)) throw new IOException("Fresh smoke directory required: " + path);
            Directory.CreateDirectory(path);
            var go = new GameObject("MissionMenuSmokeV1");
            DontDestroyOnLoad(go);
            var smoke = go.AddComponent<MissionMenuSmokeV1>();
            smoke.output = path;
            smoke.report.output = path;
            smoke.report.persistentDataPath = Application.persistentDataPath;
            smoke.initialSaves = SaveHashes();
            smoke.started = Time.realtimeSinceStartup;
            Application.logMessageReceived += smoke.Log;
        }
        static string Hash(string path) { using (var s = File.OpenRead(path)) using (var h = SHA256.Create()) return BitConverter.ToString(h.ComputeHash(s)).Replace("-", ""); }
        static Dictionary<string, string> SaveHashes()
        {
            // SaveRapper's SaveData/ and SaveDataBackup/ prefixes resolve here:
            // ES3Defaults uses File + PersistentDataPath; ES3Settings.FullPath
            // prefixes ES3IO.persistentDataPath == Application.persistentDataPath.
            // Hash opaque bytes only; never deserialize or print saved values.
            var result = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string sub in new[] { "SaveData", "SaveDataBackup" })
            {
                string root = Path.Combine(Application.persistentDataPath, sub);
                if (!Directory.Exists(root)) continue;
                foreach (string file in Directory.GetFiles(root, "*", SearchOption.AllDirectories))
                    result.Add(file.Substring(Application.persistentDataPath.Length), Hash(file));
            }
            return result;
        }
        static object Read(object instance, string name)
        {
            if (instance == null) return null;
            for (Type type = instance.GetType(); type != null; type = type.BaseType)
            {
                var field = type.GetField(name, Fields | BindingFlags.DeclaredOnly) ?? type.GetField("<" + name + ">k__BackingField", Fields | BindingFlags.DeclaredOnly);
                if (field != null) return field.GetValue(instance);
            }
            throw new MissingFieldException(instance.GetType().FullName, name);
        }
        void Need(bool value, string label) { report.checks++; if (!value) throw new InvalidOperationException(label); }
        void Mark(string label) { report.steps.Add(label); File.WriteAllText(Path.Combine(output, "result.json"), JsonUtility.ToJson(report, true)); }
        void Log(string message, string stack, LogType type) { if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert) report.errors.Add(message + "\n" + stack); }
        IEnumerator Start()
        {
            // Flatten nested iterators so every failed assertion reaches the receipt.
            var iterators = new Stack<IEnumerator>(); iterators.Push(Run());
            while (iterators.Count > 0 && !finished)
            {
                object next = null; bool advanced = false; Exception failure = null;
                try { advanced = iterators.Peek().MoveNext(); if (advanced) next = iterators.Peek().Current; }
                catch (Exception e) { failure = e; }
                if (failure != null) { Finish(failure); yield break; }
                if (!advanced) { (iterators.Pop() as IDisposable)?.Dispose(); continue; }
                if (next is IEnumerator nested) { iterators.Push(nested); continue; }
                yield return next;
            }
            if (!finished) Finish(null);
        }
        void Update() { if (!finished && Time.realtimeSinceStartup - started > 240f) Finish(new TimeoutException("240 second smoke watchdog")); }
        IEnumerator Wait(Func<bool> predicate, string label, float seconds = 20f)
        {
            float deadline = Time.realtimeSinceStartup + seconds;
            while (!predicate()) { Need(Time.realtimeSinceStartup < deadline, label); yield return null; }
        }
        static IEnumerator Frames(int count) { for (int i = 0; i < count; i++) yield return null; }
        IEnumerator Press(string actionName)
        {
            var action = input.currentActionMap.FindAction(actionName, true);
            Need(action.enabled, "Original action enabled: " + actionName);
            KeyControl key = action.controls.OfType<KeyControl>().FirstOrDefault(c => c.device == keyboard);
            Need(key != null, "Original action has a bound virtual keyboard key: " + actionName);
            report.keys.Add(actionName + "=" + key.keyCode);
            InputSystem.QueueStateEvent(keyboard, new KeyboardState(key.keyCode));
            InputSystem.QueueStateEvent(mouse, new MouseState());
            yield return null;
            InputSystem.QueueStateEvent(keyboard, new KeyboardState());
            yield return Frames(3);
        }
        void SetupInput()
        {
            input = PlayerInput.all.FirstOrDefault(p => p.currentActionMap != null && p.currentActionMap.FindAction("Mission", false) != null);
            Need(input != null, "Original PlayerInput with Mission action");
            previousDevices = input.devices.ToArray(); previousScheme = input.currentControlScheme;
            previousBackground = InputSystem.settings.backgroundBehavior; previousRunInBackground = Application.runInBackground;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
            Application.runInBackground = true;
            keyboard = InputSystem.AddDevice<Keyboard>("MissionMenuSmokeV1Keyboard");
            mouse = InputSystem.AddDevice<Mouse>("MissionMenuSmokeV1Mouse");
            input.SwitchCurrentControlScheme("Keyboard and Mouse", keyboard, mouse);
            Need(input.devices.Any(d => d == keyboard), "SDK input routing paired to owned keyboard");
        }
        static string Nonzero(IDictionary values)
        {
            var entries = new List<string>();
            foreach (DictionaryEntry entry in values)
            {
                double value = Convert.ToDouble(entry.Value, CultureInfo.InvariantCulture);
                if (value != 0) entries.Add(Convert.ToString(entry.Key, CultureInfo.InvariantCulture) + ":" + value.ToString("R", CultureInfo.InvariantCulture));
            }
            entries.Sort(StringComparer.Ordinal); return string.Join(",", entries);
        }
        static string Progress()
        {
            var data = GameState.GameStateData;
            return data.CurrentRp.ToString("R", CultureInfo.InvariantCulture) + "|" + data.ExposeExp.ToString(CultureInfo.InvariantCulture) + "|" + data.CurrentEarnThisTimeRp.ToString("R", CultureInfo.InvariantCulture) + "|" +
                Nonzero(data.MissionAchieveCountDict) + "|" + Nonzero(data.ContinueMissionAchieveCountDict) + "|" + Nonzero(data.MissionSatisfyDict) + "|" + Nonzero(data.MissionEarnRpDict) + "|" + Nonzero(data.MissionMaxRpBonus100Dict);
        }
        Button BoundButton(Component root, UnityEngine.Object target, string method)
        {
            var matches = root.GetComponentsInChildren<Button>(true).Where(b => Enumerable.Range(0, b.onClick.GetPersistentEventCount()).Any(i => b.onClick.GetPersistentTarget(i) == target && b.onClick.GetPersistentMethodName(i) == method)).ToArray();
            Need(matches.Length == 1, "Exactly one serialized " + method + " button");
            Need(matches[0].gameObject.activeInHierarchy && matches[0].enabled && matches[0].interactable, "Usable serialized " + method + " button");
            return matches[0];
        }
        IEnumerator SelectTab(int index)
        {
            var panel = MissionMenuPanelView.Instance;
            var tab = (CommonTabView)Read(panel, "tabView");
            var parent = (Transform)Read(tab, "itemParent");
            var buttons = parent.GetComponentsInChildren<Button>(false).Where(b => Enumerable.Range(0, b.onClick.GetPersistentEventCount()).Any(i => b.onClick.GetPersistentTarget(i) == tab && b.onClick.GetPersistentMethodName(i) == "OnClickTab")).ToArray();
            Need(buttons.Length == 7, "Seven existing region tab buttons");
            Need(buttons[index].enabled && buttons[index].interactable, "Region tab usable: " + index);
            buttons[index].onClick.Invoke();
            yield return Frames(4);
            Need(tab.CurrentSelect == index, "Serialized region button selected expected index");
        }
        TabSample CheckTab(int index)
        {
            var panel = MissionMenuPanelView.Instance;
            var stages = (StageType[])Read(panel, "CvtStageTypeArray");
            var sample = new TabSample { index = index, stage = (int)stages[index] };
            sample.locked = MStage.Get(stages[index]).EnterRankLimit > report.rank;
            var parent = (Transform)Read(panel, "missionItemParent");
            var rows = parent.GetComponentsInChildren<MissionMenuItemView>(false);
            sample.items = rows.Length;
            var records = MMission.Get(stages[index]).GetAll().Values;
            sample.expectedItems = sample.locked ? 0 : records.Count(m =>
                (!SystemManager.IsTrial || m.Trial) &&
                (!GameState.OptionData.FutanariDisable || m.OriginalRecord.CategoryType != MissionCategoryType.Futanari) &&
                (!GameState.OptionData.PeeDisable || m.OriginalRecord.CategoryType != MissionCategoryType.Pee));
            sample.rate = ((TMP_Text)Read(panel, "completeRate")).text;
            var condition = (TMP_Text)Read(panel, "ReleaseCondition"); sample.condition = condition.text;
            Need(sample.items == sample.expectedItems, "Region item count matches existing master data: " + index);
            Need(condition.gameObject.activeSelf == sample.locked, "Region lock notice agrees with rank");
            if (sample.locked) Need(!string.IsNullOrWhiteSpace(sample.condition) && !sample.condition.StartsWith("ui_"), "Localized lock notice");
            else Need(!string.IsNullOrWhiteSpace(sample.rate) && !sample.rate.StartsWith("ui_"), "Localized completion rate");
            foreach (var row in rows)
            {
                var name = (TMP_Text)Read(row, "missionNameText");
                var reward = (TMP_Text)Read(row, "rewardText");
                var achieved = (TMP_Text)Read(row, "achievementsText");
                var mask = (GameObject)Read(row, "rankConditionMask");
                Need(!string.IsNullOrWhiteSpace(reward.text) && !reward.text.Contains("ui_"), "Mission reward text is populated and localized");
                Need(int.TryParse(achieved.text, out _), "Mission completion count is numeric");
                if (name.gameObject.activeSelf) Need(!string.IsNullOrWhiteSpace(name.text), "Visible mission name is populated");
                if (mask.activeSelf) { sample.lockedItems++; Need(!string.IsNullOrWhiteSpace(((TMP_Text)Read(row, "rankConditionText")).text), "Locked mission has rank explanation"); }
            }
            if (stages[index] == StageType.Residence)
            {
                report.residenceItems = rows.Length; report.residenceMasterItems = records.Count;
                Need(records.Count == 31, "Residence master contains 31 original tasks");
            }
            report.tabs.Add(sample); return sample;
        }
        IEnumerator Capture(string name)
        {
            yield return new WaitForSecondsRealtime(0.6f);
            yield return new WaitForEndOfFrame();
            var texture = ScreenCapture.CaptureScreenshotAsTexture();
            Need(texture && texture.width > 100 && texture.height > 100, "Rendered screenshot dimensions");
            string path = Path.Combine(output, name + ".png");
            File.WriteAllBytes(path, texture.EncodeToPNG());
            var pixels = texture.GetPixels32();
            int lit = pixels.Count(p => p.r > 8 || p.g > 8 || p.b > 8);
            Destroy(texture); report.screenshots.Add(path);
            var panel = MissionMenuPanelView.Instance;
            float opacity = 1f;
            foreach (var group in panel.GetComponentsInParent<CanvasGroup>(true))
            {
                report.presentation.Add(name + ": group=" + group.name + " alpha=" + group.alpha + " ignoreParents=" + group.ignoreParentGroups);
                opacity *= group.alpha;
            }
            foreach (var canvas in panel.GetComponentsInParent<Canvas>(true))
                report.presentation.Add(name + ": canvas=" + canvas.name + " enabled=" + canvas.enabled + " mode=" + canvas.renderMode + " display=" + canvas.targetDisplay + " camera=" + (canvas.worldCamera ? canvas.worldCamera.name : "null"));
            var rect = panel.GetComponent<RectTransform>();
            report.presentation.Add(name + ": rect=" + rect.rect + " position=" + rect.position + " scale=" + rect.lossyScale + " litPixels=" + lit);
            Need(lit > pixels.Length / 200, "Screenshot contains rendered color, not a blank hidden window");
            Need(opacity > .9f, "Task panel is visibly opaque after its opening animation");
        }
        IEnumerator AwaitStartupUi()
        {
            float deadline = Time.realtimeSinceStartup + 120f;
            float stableSince = -1f, tutorialSeenAt = -1f;
            int observedTutorial = 0, cancelledTutorial = 0;
            while (true)
            {
                float now = Time.realtimeSinceStartup;
                Need(now < deadline, "Startup UI becomes continuously clear within 120 seconds");
                var top = InGameUiManager.Instance.GetCurrentBasePanelView();
                if (top && top.GetType().Name.Equals("TutorialPanelView", StringComparison.Ordinal))
                {
                    stableSince = -1f;
                    int identity = top.GetInstanceID();
                    if (identity != observedTutorial) { observedTutorial = identity; tutorialSeenAt = now; }
                    if (cancelledTutorial != identity && now - tutorialSeenAt >= 1.2f)
                    {
                        Need(report.tutorialCancels < 3, "At most three original startup tutorials may be dismissed");
                        cancelledTutorial = identity;
                        report.tutorialCancels++;
                        yield return Press("Cancel");
                    }
                }
                else
                {
                    observedTutorial = 0; tutorialSeenAt = -1f;
                    bool clear = !top && !InGameUiManager.Instance.IsOpenSomeUi && !InGameUiManager.Instance.IsOpenFade && Time.timeScale > 0f;
                    if (!clear) stableSince = -1f;
                    else if (stableSince < 0f) stableSince = now;
                    else if (now - stableSince >= 1.0f) yield break;
                }
                yield return null;
            }
        }
        IEnumerator Run()
        {
            yield return Wait(() => InGameManager.Instance && PlayerController.Instance && PlayerController.Instance.Pca != null && InGameUiManager.Instance && InputManager.Instance, "Original startup ready", 120f);
            SetupInput(); yield return AwaitStartupUi();
            report.scene = SceneManager.GetActiveScene().name;
            Need(report.scene == "Apartment", "Home scene loaded normally");
            report.rank = MRank.CalcCurrentRank(GameState.GameStateData.ExposeExp).Rank;
            report.beforeRp = GameState.GameStateData.CurrentRp; initialProgress = Progress(); Mark("startup-ready");

            yield return Press("Mission");
            yield return Wait(() => MissionMenuPanelView.Instance && MissionMenuPanelView.Instance.IsActivePanel(), "Mission shortcut opened panel");
            report.shortcutOpened = true; yield return Frames(20); yield return SelectTab(0); CheckTab(0);
            yield return Capture("01-residence-missions"); Mark("shortcut-open-and-residence-verified");
            yield return Press("Mission");
            yield return Wait(() => !MissionMenuPanelView.Instance && !InGameUiManager.Instance.IsOpenSomeUi, "Mission shortcut closed panel");
            report.shortcutClosed = true;

            yield return Press("GameMenu");
            yield return Wait(() => InGameMenuView.Instance && InGameMenuView.Instance.IsActivePanel(), "Tab opened parent menu");
            report.parentOpened = true; yield return Frames(20);
            var menu = InGameMenuView.Instance;
            BoundButton(menu, menu, "OnClickMission").onClick.Invoke();
            yield return Wait(() => MissionMenuPanelView.Instance && MissionMenuPanelView.Instance.IsActivePanel(), "Serialized mission button opened child");
            report.childOpened = true;
            Need((bool)Read(menu, "isOpenChild"), "Parent marks its child open");
            yield return Frames(20);
            bool capturedLock = false;
            for (int i = 1; i < 7; i++)
            {
                yield return SelectTab(i); var sample = CheckTab(i);
                if (sample.locked && !capturedLock) { yield return Capture("02-locked-region"); capturedLock = true; report.lockedRegionExercised = true; }
            }
            yield return SelectTab(0);
            if (!capturedLock) yield return Capture("02-parent-child-residence");
            Mark("all-region-tabs-verified");
            yield return Press("Cancel");
            yield return Wait(() => !MissionMenuPanelView.Instance && menu && menu.IsActivePanel(), "Close returns active parent menu");
            Need(!(bool)Read(menu, "isOpenChild"), "Parent child latch released"); report.parentRestored = true;
            yield return Press("Cancel");
            yield return Wait(() => !InGameMenuView.Instance && !InGameUiManager.Instance.IsOpenSomeUi, "Cancel returns gameplay UI");
            yield return Frames(20); report.allUiClosed = true;
            report.afterRp = GameState.GameStateData.CurrentRp;
            report.progressionUnchanged = initialProgress == Progress(); Need(report.progressionUnchanged, "Menus preserve RP, experience, rewards, completion counts and nonzero task progress");
            Need(report.errors.Count == 0, "No runtime errors");
            Mark("return-and-progression-verified");
        }
        void Finish(Exception error)
        {
            if (finished) return; finished = true;
            try
            {
                if (keyboard != null) InputSystem.QueueStateEvent(keyboard, new KeyboardState());
                if (input && previousDevices != null && previousDevices.Length > 0 && previousDevices.All(d => d.added) && !string.IsNullOrEmpty(previousScheme)) input.SwitchCurrentControlScheme(previousScheme, previousDevices);
                if (keyboard != null && keyboard.added) InputSystem.RemoveDevice(keyboard);
                if (mouse != null && mouse.added) InputSystem.RemoveDevice(mouse);
                if (previousDevices != null) { InputSystem.settings.backgroundBehavior = previousBackground; Application.runInBackground = previousRunInBackground; }
                report.inputRestored = (keyboard == null || !keyboard.added) && (mouse == null || !mouse.added) && (previousDevices == null || InputSystem.settings.backgroundBehavior == previousBackground);
                var after = SaveHashes();
                report.savesUnchanged = initialSaves.Count == after.Count && initialSaves.All(p => after.TryGetValue(p.Key, out string hash) && hash == p.Value);
                if (!report.savesUnchanged && error == null) error = new InvalidOperationException("Persistent save files changed during smoke");
                if (!report.inputRestored && error == null) error = new InvalidOperationException("SDK input restoration failed");
                if (report.errors.Count > 0 && error == null) error = new InvalidOperationException("Runtime logged errors");
            }
            catch (Exception cleanup) { error = error ?? cleanup; }
            report.failure = error?.ToString(); report.passed = error == null; report.status = report.passed ? "passed" : "failed";
            File.WriteAllText(Path.Combine(output, "result.json"), JsonUtility.ToJson(report, true));
            Application.logMessageReceived -= Log;
            Application.Quit(report.passed ? 0 : 6);
        }
    }
}
