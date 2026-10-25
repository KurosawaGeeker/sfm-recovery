using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Key = UnityEngine.InputSystem.Key;

namespace SfmMvp
{
    // Owned standalone observation. Gameplay effects originate only in original SDK input/UI processing.
    public sealed class SkillLearningPlayerV1 : MonoBehaviour
    {
        const string Prefix = "SfmSkillLearningPlayerV1.";
        const string Flag = "--sfm-ui-options-player-v1";
        const string DevicePrefix = "SfmSkillLearningPlayerV1-";
        const string OutputRoot = @"C:\Users\Administrator\sfm-layerfs-dev\mission-menu-v1";
        const string Product = "SFMOriginalGameplayInputVisual-31014133f09b4dad99db9b29b82fe544";
        const string Profile = @"C:\Users\Administrator\AppData\LocalLow\SFMRecovery\SFMOriginalGameplayInputVisual-31014133f09b4dad99db9b29b82fe544";
        const string ProtectedProfile = @"C:\Users\Administrator\AppData\LocalLow\SFMRecovery\SFMOriginalGameplaySuccessDiag-32841af7e68d462cba45cbe47234e3de";
        const string Home = "Assets/SfmOriginalGameplayV3/Apartment.unity";
        const string G = "ExposureUnnoticed2.Scripts.InGame.";
        const string B = "ExposureUnnoticed2.Scripts.Base.";
        const string UI = "ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager";
        const string MenuType = "ExposureUnnoticed2.ObjectUI.InGameMenu.InGameMenuView";
        const string PanelType = "ExposureUnnoticed2.ObjectUI.SaveDataSelectPanel.SkillPanelView";
        const string PreviewType = "ExposureUnnoticed2.Object3D.UIPlayer.UiPlayerController";
        const BindingFlags Fields = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        static readonly Assembly Game = typeof(global::ExposureUnnoticed2.Scripts.InGame.InGameManager).Assembly;
                [Serializable] public sealed class Pin { public string path, sha256; public long bytes; }
        [Serializable] public sealed class Contract
        {
            public string schema, playerGameSHA, observerAssemblySHA, executableSHA, buildRoot, testProduct, testProfilePath, output;
            public bool launchReady; public Pin[] inputs, testSaves, protectedSaves;
        }
        [Serializable] public sealed class MoveSample
        {
            public int frame, phase; public float time; public Vector3 position; public Vector2 sdkMove;
            public float dt, stepXZ, planarSpeed; public bool hasPrevious;
        }
        [Serializable] public sealed class Tap
        {
            public string role, action, path; public int key, raw, pressFrame, releaseFrame;
            public bool pressSeen, releaseSeen, rawOne;
        }
        [Serializable] public sealed class Click
        {
            public string role, path, moveHit, pressHit, releaseHit; public Vector2 point;
            public int target, firstFrame, moveFrame, pressFrame, releaseFrame, moveModuleFrame, pressModuleFrame, releaseModuleFrame;
            public bool pressed, released, moveSeen, pressSeen, releaseSeen, repeatHidden;
            public float readySince;
        }
        [Serializable] public sealed class PreviewClip { public string name; public float weight; }
        [Serializable] public sealed class PreviewLayer
        {
            public int index, currentHash, nextHash; public string name; public float weight, currentTime, nextTime; public bool transition;
            public PreviewClip[] current, next;
        }
        [Serializable] public sealed class Snapshot
        {
            public string reason, scene, product, previewController, previewPath, previewClipNames, skillLevels;
            public int frame, phase, rank, level, maxLevel, price, type, category, order, releaseRank, menuState, selectedIndex, tabIndex, stack, poseId, previewAnimatorId, playerId;
            public double rp, totalUsed; public float time, scale, previewStateTime;
            public bool menuPresent, masked, learned, purchaseVisible, learnedVisible, previewContent, previewPanel, forceInactive, cancelUi, allowUi, fade, child, moveEnabled, keyboardPaired, mousePaired;
            public Vector3 playerPosition; public int playerAction; public PreviewLayer[] previewLayers;
        }
        [Serializable] public sealed class PoseState
        {
            public string reason, controller; public int frame, phase, action, animatorAction, justPose, ringShortcut, shortcutIndex, page, selectedRingItem, motionCount, cursorLock;
            public float time, cooldown, variantTarget, variantCurrent, animatorVariant; public bool ringOpen, cameraLocked, selectedReleased, justInteract, cursorVisible;
            public Vector3 position, decodedMove; public Vector2 sdkMove; public PreviewLayer[] layers;
        }
        [Serializable] public sealed class Capture
        {
            public string role,path,sha256;public int frame;public long bytes;public bool ready;
        }
        [Serializable] public sealed class UiVisibility
        { public string field,option,path;public bool desired,activeSelf,activeInHierarchy; }
        [Serializable] public sealed class UiAudio
        { public string parameter,option;public int percent;public bool readable;public float expected,actual; }
        [Serializable] public sealed class UiOptions
        {
            public string reason;public int frame;public bool matched;public int scalePercent,ownListenerCount;
            public Vector3 scale;public UiVisibility[] visibility;public UiAudio[] audio;
        }
        [Serializable] public sealed class Report
        {
            public string schema="SfmUiOptionsPlayerObservation/v1", status="preflight", failure, testProduct, testProfilePath, playerGameSHA, observerAssemblySHA, executableSHA, screenshot, screenshotSHA;
            public bool uiOptionsInitiallyMatched;public List<UiOptions> uiOptions=new List<UiOptions>();
            public bool ringOpened, ringSelectionObserved, shortcutBound, poseEntered, poseVariantChanged, poseCancelled, poseMovementAccepted, poseInputSeen, poseInteractSeen, poseMovePressSeen, poseMoveReleaseSeen;
            public int poseIndex, targetRingItem, targetModelIndex, targetRingPage, targetRingSlot, pageSteps, poseMovePressFrame, poseMoveReleaseFrame;
            public float ringSelectedSince=-1, variantBefore;public Vector3 poseMoveStart,poseMoveReleased,poseMoveFinal;
            public PoseState beforeRing,enteredPose,variantPose,cancelledPose;
            public List<PoseState> poseSamples=new List<PoseState>(),poseSnapshots=new List<PoseState>();
            public List<Capture> captures=new List<Capture>();
            public string visualLimit="Screenshot and animation telemetry require external visual review. Runtime passed remains false until a separate reviewed receipt; no save/reload acceptance.";
            public int phase, phaseFrame, checks, persistedSamples, scrollSteps, movePressFrame, moveReleaseFrame;
            public float phaseTime, startTime, screenshotTime; public Vector3 moveStart, moveReleased, moveFinal;
            public float stopTailDuration, stopTailMaxDisplacement; public int stopTailSamples; public bool stopTailAllSdkZero;
            public bool screenshotTimedOut, previewPoseObserved, actualPlayer, learned, maxLevelUiRejected, methodGuardRuntimeAccepted, returnedToMenu, returnedToHome, movementAccepted, movePressSeen, moveReleaseSeen, screenshotReady, sourceSavesUnchanged, sdkRestored, identityUnchanged, pairingRestored, autoSwitchRestored, reportComplete, flowAccepted, visualAccepted, passed, inputReleaseObserved;
            public string originalScheme; public int[] originalDeviceIds; public bool originalNeverAutoSwitch;
            public uint originalInputUserId; public bool originalSchemePresent;
            public Snapshot baseline, beforeBuy, afterBuy, lastSample;
            public List<string> readResolutions=new List<string>(), errors=new List<string>(), cleanupErrors=new List<string>();
            public List<Snapshot> snapshots=new List<Snapshot>(); public List<Tap> taps=new List<Tap>(); public List<Click> clicks=new List<Click>(); public List<Pin> finalTestFiles=new List<Pin>(); public List<MoveSample> movement=new List<MoveSample>();
        }
        static Contract contract; static Report report; static bool finished; static int lastFrame=-1;
        static Keyboard keyboard; static Mouse mouse; static Vector2 mousePoint, scroll, mouseDelta; static bool mouseLeft;
        static object manager, player, pca, state, gsd, ui; static Transform playerTransform;
        static Behaviour menu, panel, item; static int scrollFrame=-100; static Button purchaseButton;
        static PlayerInput routedInput; static bool pairingCaptured;
        static InputControlScheme? originalUserScheme;
        static bool Armed, outputOwned, backgroundCaptured, originalRunInBackground, finishing, completing;
        static bool pairingWasCaptured; static int keyboardId, mouseId;
        static InputSettings.BackgroundBehavior originalBackground;
        static DateTime deadline, finishingDeadline;
        static bool HasFlag => Environment.GetCommandLineArgs().Contains(Flag);
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Install()
        {
            if(!HasFlag)return;
            report=new Report();
            try
            {
                Begin();
                var host=new GameObject("SfmSkillLearningPlayerV1-OwnedObserver");
                DontDestroyOnLoad(host);host.AddComponent<SkillLearningPlayerV1>();
                Application.logMessageReceived+=Log;InputSystem.onAfterUpdate+=AfterInput;
            }
            catch(Exception e)
            {
                report.failure=e.ToString();report.status="preflight-refused";
                try{Flush();}finally{Application.Quit(6);}
            }
        }
        void Update(){if(!Armed||finished)return;if(finishing)DrainAndComplete();else Poll();}
        void LateUpdate()
        {
            if(!Armed||finished||finishing||report.phase<15||report.phase>27)return;
            try {var sample=ObservePose("late",false);Need(report.poseSamples.Count<8192,"Bounded real pose telemetry");report.poseSamples.Add(sample);report.poseInputSeen|=sample.justPose==report.targetRingItem;report.poseInteractSeen|=report.poseEntered&&sample.justInteract;}
            catch(Exception e){Finish(e);}
        }
        void OnApplicationQuit()
        {
            if(!Armed||finished||completing)return;
            report.flowAccepted=false;report.failure=(report.failure??"")+"; Unexpected Player quit";
            Complete(false);
        }
        static void Need(bool value,string reason) { if(report!=null) report.checks++; if(!value) throw new InvalidOperationException(reason); }
        static object Read(object value,string name)
        {
            if(value==null) return null;
            for(Type t=value is Type ? (Type)value : value.GetType();t!=null;t=t.BaseType)
            {
                var f=t.GetField(name,Fields) ?? t.GetField("<"+name+">k__BackingField",Fields);
                if(f==null) continue;
                string id=f.DeclaringType.FullName+"::"+f.Name+" : "+f.FieldType.FullName;
                if(report!=null&&!report.readResolutions.Contains(id)) report.readResolutions.Add(id);
                return f.GetValue(value is Type ? null : value);
            }
            throw new MissingFieldException((value is Type?(Type)value:value.GetType()).FullName,name);
        }
        static object Static(string type,string name)=>Read(Game.GetType(type,true),name);
        static int I(object v) { Need(v!=null,"Known integer/enum"); return Convert.ToInt32(v); }
        static double D(object v) { Need(v!=null,"Known numeric field"); return Convert.ToDouble(v); }
        static bool Z(object v) { Need(v is bool,"Known boolean field"); return (bool)v; }
        static int Id(object v)=>v is UnityEngine.Object ? ((UnityEngine.Object)v).GetInstanceID():0;
        static bool Alive(object v)=>v!=null&&(!(v is UnityEngine.Object)||(UnityEngine.Object)v);
        static string Arg(string name) { var a=Environment.GetCommandLineArgs();int k=Array.IndexOf(a,name);Need(k>=0&&k+1<a.Length,"Explicit "+name);return a[k+1]; }
        static string Hash(string p) { using(var h=SHA256.Create()) using(var f=File.OpenRead(p)) return BitConverter.ToString(h.ComputeHash(f)).Replace("-",""); }
        static void CheckPin(Pin p) { Need(p!=null&&File.Exists(p.path)&&new FileInfo(p.path).Length==p.bytes&&Hash(p.path)==p.sha256,"Exact pinned file "+(p==null?"null":p.path)); }
        static bool Inside(string path,string root)=>Path.GetFullPath(path).StartsWith(Path.GetFullPath(root).TrimEnd('\\','/')+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase);
        static void Save(){if(report!=null)report.persistedSamples=report.snapshots.Count;}
        static void Write(){if(report!=null&&contract!=null&&outputOwned&&Directory.Exists(contract.output))File.WriteAllText(Path.Combine(contract.output,"observation.json"),JsonUtility.ToJson(report,true)+"\n");}
        static void Flush(){try{CleanupStep("record-count",Save);}finally{CleanupStep("write-report",Write);}}
        static void CheckTree(string root)
        {
            Need(Directory.Exists(root),"Existing opaque profile");
            foreach(var d in new[]{root}.Concat(Directory.GetDirectories(root,"*",SearchOption.AllDirectories)))
                Need((File.GetAttributes(d)&FileAttributes.ReparsePoint)==0,"No profile directory reparse point");
            foreach(var f in Directory.GetFiles(root,"*",SearchOption.AllDirectories))
                Need((File.GetAttributes(f)&FileAttributes.ReparsePoint)==0,"No profile file reparse point");
        }
        static void Begin()
        {
            Need(!Application.isEditor&&!Armed,"Explicit standalone Player only");
            var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,Flag);
            Need(i>=0&&i+2<args.Length,"Explicit contract path and SHA");
            string contractPath=Path.GetFullPath(args[i+1]);Need(Hash(contractPath)==args[i+2],"Exact sealed runtime contract");
            contract=JsonUtility.FromJson<Contract>(File.ReadAllText(contractPath));
            Need(contract.schema=="SfmUiOptionsPlayerContract/v1"&&contract.launchReady,"Parent sealed actual build");
            Need(contract.testProduct==Product&&Path.GetFullPath(contract.testProfilePath)==Path.GetFullPath(Profile),"Continue exact existing isolated profile");
            Need(Application.companyName=="SFMRecovery"&&Application.productName==Product&&Path.GetFullPath(Application.persistentDataPath)==Path.GetFullPath(Profile),"Baked identity and actual persistentDataPath; arguments cannot redirect");
            Need(args.Contains("--sfm-manual-preview"),"Existing original bootstrap startup mode");
            foreach(string flag in args.Where(x=>x.StartsWith("--sfm",StringComparison.Ordinal)||x.StartsWith("-sfm",StringComparison.Ordinal)))
                Need(flag==Flag||flag=="--sfm-manual-preview","No competing diagnostic flag: "+flag);
            Need(Path.GetDirectoryName(Path.GetFullPath(contract.buildRoot))==OutputRoot,"One-build workspace direct child");
            string exe=System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            Need(Path.GetDirectoryName(Path.GetFullPath(exe))==Path.GetFullPath(contract.buildRoot)&&Inside(Application.dataPath,contract.buildRoot),"Actual process and data directory belong to sealed build");
            Need(Inside(Game.Location,Path.Combine(Application.dataPath,"Managed")),"Loaded Game inside actual Player Managed directory");
            report.playerGameSHA=Hash(Game.Location);report.observerAssemblySHA=Hash(typeof(SkillLearningPlayerV1).Assembly.Location);report.executableSHA=Hash(exe);
            Need(report.playerGameSHA==contract.playerGameSHA&&report.observerAssemblySHA==contract.observerAssemblySHA&&report.executableSHA==contract.executableSHA,"Actual Player/Game/observer hashes");
            Need(contract.inputs!=null&&contract.inputs.Length>0&&contract.inputs.Length<=64,"Bounded build provenance pins");foreach(var p in contract.inputs)CheckPin(p);
            Need(contract.testSaves!=null&&contract.testSaves.Length==3&&contract.protectedSaves!=null&&contract.protectedSaves.Length==3,"Exactly existing test and protected-source opaque inputs");
            CheckTree(Profile);CheckTree(ProtectedProfile);
            Need(contract.testSaves.Select(x=>Path.GetFullPath(x.path)).Distinct(StringComparer.OrdinalIgnoreCase).Count()==3,"Three distinct test files");
            Need(contract.protectedSaves.Select(x=>Path.GetFullPath(x.path)).Distinct(StringComparer.OrdinalIgnoreCase).Count()==3,"Three distinct source files");
            foreach(var p in contract.testSaves){Need(Inside(p.path,Profile),"Only existing test root input");CheckPin(p);}
            foreach(var p in contract.protectedSaves){Need(Inside(p.path,ProtectedProfile),"Only protected original source");CheckPin(p);}
            Need(Path.GetDirectoryName(Path.GetFullPath(contract.output))==OutputRoot&&!Directory.Exists(contract.output)&&!File.Exists(contract.output),"Fresh direct evidence child");
            Need(!string.Equals(Path.GetFullPath(contract.output),Path.GetFullPath(contract.buildRoot),StringComparison.OrdinalIgnoreCase),"Evidence separate from the sole build");
            Directory.CreateDirectory(contract.output);outputOwned=true;
            report.testProduct=Product;report.testProfilePath=Profile;report.actualPlayer=true;report.status="running";
            deadline=DateTime.UtcNow.AddSeconds(240);Armed=true;Flush();
        }
        static void SetupSDK()
        {
            if(!backgroundCaptured)
            {
                originalBackground=InputSystem.settings.backgroundBehavior;originalRunInBackground=Application.runInBackground;backgroundCaptured=true;
                InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;Application.runInBackground=true;
            }
            if(keyboard==null){keyboard=InputSystem.AddDevice<Keyboard>(DevicePrefix+"Keyboard");keyboardId=keyboard.deviceId;}
            if(mouse==null){mouse=InputSystem.AddDevice<Mouse>(DevicePrefix+"Mouse");mouseId=mouse.deviceId;}
            var pi=PI();
            if(pi&&!pairingCaptured)
            {
                Need(pi.user.valid&&pi.actions.controlSchemes.Any(x=>x.name=="Keyboard and Mouse"),"Original SDK user and supported scheme");
                // v2: a valid InputUser can have zero paired devices and/or no active control scheme.
                routedInput=pi;originalUserScheme=pi.user.controlScheme;report.originalInputUserId=pi.user.id;
                report.originalSchemePresent=originalUserScheme.HasValue;report.originalScheme=originalUserScheme.HasValue?originalUserScheme.Value.name:null;
                report.originalDeviceIds=pi.user.pairedDevices.Select(x=>x.deviceId).ToArray();report.originalNeverAutoSwitch=pi.neverAutoSwitchControlSchemes;
                pairingCaptured=true;pairingWasCaptured=true;
                pi.neverAutoSwitchControlSchemes=true;pi.SwitchCurrentControlScheme("Keyboard and Mouse",keyboard,mouse);
            }
            if(pairingCaptured)Need(routedInput&&routedInput.neverAutoSwitchControlSchemes&&routedInput.user.pairedDevices.Count==2&&routedInput.user.pairedDevices.Any(x=>x.deviceId==keyboard.deviceId)&&routedInput.user.pairedDevices.Any(x=>x.deviceId==mouse.deviceId),"Only owned SDK devices paired; physical manual-player input excluded");
        }
        static void Queue(Key[] keys)
        {
            if(keyboard!=null)InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
            if(mouse!=null)InputSystem.QueueStateEvent(mouse,new MouseState{position=mousePoint,scroll=scroll,delta=mouseDelta}.WithButton(MouseButton.Left,mouseLeft));
        }
        static PlayerInput PI()=>Static(B+"InputManager","playerInput") as PlayerInput;
        static int Raw(int id)
        {
            var dict=Static(B+"InputManager","inputDict") as IDictionary;Need(dict!=null,"Original input dictionary");
            foreach(DictionaryEntry e in dict)if(I(e.Key)==id)return I(e.Value);throw new InvalidOperationException("Missing raw input "+id);
        }
        static Key EffectiveKey(string action)
        {
            var a=PI().currentActionMap.FindAction(action,true);var paths=a.bindings.Where(x=>!x.isComposite&&!x.isPartOfComposite&&x.effectivePath!=null&&x.effectivePath.StartsWith("<Keyboard>/",StringComparison.Ordinal)).ToArray();
            Need(paths.Length==1,"One actual keyboard binding for "+action);var k=keyboard.TryGetChildControl<UnityEngine.InputSystem.Controls.KeyControl>(paths[0].effectivePath.Substring(11));Need(k!=null,"Actual SDK KeyControl");return k.keyCode;
        }
        static Key ForwardKey()
        {
            var a=PI().currentActionMap.FindAction("Move",true);var paths=a.bindings.Where(x=>x.isPartOfComposite&&x.name=="up"&&x.effectivePath!=null&&x.effectivePath.StartsWith("<Keyboard>/",StringComparison.Ordinal)).ToArray();
            Need(paths.Length==1,"One actual keyboard Move/up binding");return keyboard.TryGetChildControl<UnityEngine.InputSystem.Controls.KeyControl>(paths[0].effectivePath.Substring(11)).keyCode;
        }
        static bool TapKey(string role,string action,int raw,out Key[] keys)
        {
            keys=new Key[0];var t=report.taps.SingleOrDefault(x=>x.role==role);
            if(t==null){var k=EffectiveKey(action);var a=PI().currentActionMap.FindAction(action,true);t=new Tap{role=role,action=action,path=a.bindings.First(x=>x.effectivePath=="<Keyboard>/"+keyboard[k].name).effectivePath,key=(int)k,raw=raw,pressFrame=Time.frameCount};report.taps.Add(t);keys=new[]{k};return false;}
            Need(Time.frameCount-t.pressFrame<180,"Finite single key transaction "+role);
            if(t.releaseFrame==0&&Time.frameCount>t.pressFrame)t.releaseFrame=Time.frameCount;
            t.rawOne|=Raw(t.raw)==1;return t.releaseFrame>0&&t.pressSeen&&t.releaseSeen&&t.rawOne;
        }
        static string ObjectPath(Transform t)=>t==null?"":t.parent==null?t.name:ObjectPath(t.parent)+"/"+t.name;
        static void AfterInput()
        {
            if(!Armed||finished||!Application.isPlaying||InputState.currentUpdateType!=InputUpdateType.Dynamic)return;
            try
            {
                if(keyboard==null||mouse==null)return;
                if(finishing){report.inputReleaseObserved=!keyboard.anyKey.isPressed&&!mouse.leftButton.isPressed&&mouse.scroll.ReadValue()==Vector2.zero;return;}var pi=PI();if(!pi||pi.currentActionMap==null)return;
                foreach(var t in report.taps){var a=pi.currentActionMap.FindAction(t.action,false);if(a==null||!a.enabled)continue;t.pressSeen|=keyboard[(Key)t.key].isPressed&&a.ReadValue<float>()>0.5f;t.releaseSeen|=t.releaseFrame>0&&!keyboard[(Key)t.key].isPressed&&a.ReadValue<float>()==0f;t.rawOne|=Raw(t.raw)==1;}
                var move=pi.currentActionMap.FindAction("Move",false);if(move!=null&&report.movePressFrame>0){report.movePressSeen|=move.enabled&&move.ReadValue<Vector2>().sqrMagnitude>0.5f;report.moveReleaseSeen|=report.moveReleaseFrame>0&&move.ReadValue<Vector2>().sqrMagnitude==0f;}
                if(move!=null&&report.poseMovePressFrame>0){report.poseMovePressSeen|=move.enabled&&move.ReadValue<Vector2>().sqrMagnitude>0.5f;report.poseMoveReleaseSeen|=report.poseMoveReleaseFrame>0&&move.ReadValue<Vector2>().sqrMagnitude==0f;}
                var es=EventSystem.current;var im=es?es.currentInputModule as InputSystemUIInputModule:null;if(!im||im.point==null||im.leftClick==null)return;
                var point=im.point.action;var click=im.leftClick.action;if(point==null||click==null||!point.enabled||!click.enabled)return;
                var hit=im.GetLastRaycastResult(mouse.deviceId).gameObject;var hitButton=hit?hit.GetComponentInParent<Button>():null;int hitId=Id(hitButton);string hitPath=hit?ObjectPath(hit.transform):"";
                foreach(var c in report.clicks)
                {
                    if((point.ReadValue<Vector2>()-c.point).sqrMagnitude<0.01f&&(mouse.position.ReadValue()-c.point).sqrMagnitude<0.01f)c.moveSeen=true;
                    if(c.pressed&&mouse.leftButton.isPressed&&click.ReadValue<float>()>0.5f)c.pressSeen=true;
                    if(c.released&&!mouse.leftButton.isPressed&&click.ReadValue<float>()==0f)c.releaseSeen=true;
                    if(!c.pressed&&c.moveSeen&&Time.frameCount>c.moveFrame+1){c.moveModuleFrame=Time.frameCount;c.moveHit=hitPath;if(!c.repeatHidden&&hitId!=c.target)c.moveModuleFrame=0;}
                    if(c.pressed&&c.pressSeen&&c.pressModuleFrame==0&&Time.frameCount>c.pressFrame+1){c.pressModuleFrame=Time.frameCount;c.pressHit=hitPath;Need(c.repeatHidden?hitId!=c.target:hitId==c.target,"Retained SDK press raycast on intended original control");}
                    if(c.released&&c.releaseSeen&&c.releaseModuleFrame==0&&Time.frameCount>c.releaseFrame+1){c.releaseModuleFrame=Time.frameCount;c.releaseHit=hitPath;}
                }
            }catch(Exception e){Finish(e);}
        }
        static bool Callback(Button b,UnityEngine.Object target,string method,int? argument=null)
        {
            for(int i=0;i<b.onClick.GetPersistentEventCount();i++)
            {
                if(b.onClick.GetPersistentTarget(i)!=target||b.onClick.GetPersistentMethodName(i)!=method||b.onClick.GetPersistentListenerState(i)==UnityEngine.Events.UnityEventCallState.Off)continue;
                if(!argument.HasValue)return true;
                var calls=Read(Read(b.onClick,"m_PersistentCalls"),"m_Calls") as IList;Need(calls!=null&&i<calls.Count,"Persistent argument metadata");var row=calls[i];
                if(I(Read(row,"m_Mode"))==3&&I(Read(Read(row,"m_Arguments"),"m_IntArgument"))==argument.Value)return true;
            }return false;
        }
        static Button ButtonFor(Behaviour owner,UnityEngine.Object target,string method,int? argument=null)
        {
            Need(owner,"Original owner exists");var a=owner.GetComponentsInChildren<Button>(true).Where(x=>Callback(x,target,method,argument)).ToArray();Need(a.Length==1,"One original Button for "+method);return a[0];
        }
        static Vector2 Center(RectTransform rt)
        {
            var canvas=rt.GetComponentInParent<Canvas>();Need(canvas,"Actual Canvas");return RectTransformUtility.WorldToScreenPoint(canvas.renderMode==RenderMode.ScreenSpaceOverlay?null:canvas.worldCamera,rt.TransformPoint(rt.rect.center));
        }
        static bool ClickButton(string role,Button b,bool hiddenRepeat=false)
        {
            int frame=Time.frameCount;var c=report.clicks.SingleOrDefault(x=>x.role==role);
            if(c!=null){Need(frame-c.firstFrame<180,"Finite mouse transaction "+role);mousePoint=c.point;if(c.released){mouseLeft=false;return c.moveSeen&&c.pressSeen&&c.releaseSeen&&c.releaseModuleFrame>c.releaseFrame+1;}}
            Need(b,"Original serialized button remains referenced");
            if(!hiddenRepeat)Need(b.enabled&&b.gameObject.activeInHierarchy&&b.interactable&&b.IsInteractable(),"Original live interactive button");
            else Need(!b.gameObject.activeInHierarchy&&Z(Read(item,"isLearned")),"Only already-learned hidden purchase button");
            var point=hiddenRepeat?report.clicks.Single(x=>x.role=="learn").point:Center(b.transform as RectTransform);
            Need(point.x>=0&&point.y>=0&&point.x<Screen.width&&point.y<Screen.height,"Original button point within GameView");
            var es=EventSystem.current;Need(es&&es.currentInputModule is InputSystemUIInputModule,"Original SDK UI module");
            if(c==null){c=new Click{role=role,path=ObjectPath(b.transform),target=Id(b),point=point,firstFrame=frame,moveFrame=frame,readySince=Time.unscaledTime,repeatHidden=hiddenRepeat};report.clicks.Add(c);}
            Need(c.target==Id(b),"Same control through one mouse transaction");
            if(!c.pressed)
            {
                mouseLeft=false;if((point-c.point).sqrMagnitude>=0.01f){c.point=point;c.moveFrame=frame;c.moveSeen=false;c.moveModuleFrame=0;c.readySince=Time.unscaledTime;}mousePoint=c.point;
                var hits=new List<RaycastResult>();es.RaycastAll(new PointerEventData(es){position=c.point},hits);
                bool hit=hits.Count>0&&(hits[0].gameObject==b.gameObject||hits[0].gameObject.transform.IsChildOf(b.transform));
                if(hiddenRepeat){Need(hits.Count>0&&hits[0].gameObject.transform.IsChildOf(item.transform),"Repeated old point must remain inside the same harmless learned row");hit=!hit;}
                if(!hit||Time.unscaledTime-c.readySince<0.3f||frame-c.moveFrame<2||!c.moveSeen||c.moveModuleFrame==0)return false;
                c.pressed=true;c.pressFrame=frame;mouseLeft=true;return false;
            }
            mousePoint=c.point;if(c.pressSeen&&c.pressModuleFrame>c.pressFrame+1){c.released=true;c.releaseFrame=frame;mouseLeft=false;}return false;
        }
        static Behaviour One(string name)=>UnityEngine.Object.FindObjectsOfType<MonoBehaviour>(true).Where(x=>x&&x.gameObject.scene.isLoaded&&x.GetType().FullName==name).Cast<Behaviour>().SingleOrDefault();
        static object Top(){var a=Read(ui,"basePanelStack") as IList;return a==null||a.Count==0?null:a[a.Count-1];}
        static bool Bind()
        {
            manager=Static(G+"InGameManager","Instance");if(!Alive(manager))return false;
            player=Read(Read(manager,"InGameReferencer"),"Player");if(!Alive(player))return false;
            pca=Read(player,"pca");state=Read(pca,"PlayerState");gsd=Static(G+"GameState","GameStateData");ui=Static(UI,"Instance");playerTransform=Read(pca,"Transform") as Transform;
            if(pca==null||state==null||gsd==null||!Alive(ui)||!playerTransform||Read(state,"CurrentRank")==null)return false;
            menu=One(MenuType);panel=One(PanelType);return PI()&&PI().currentActionMap!=null&&Read(ui,"basePanelStack")!=null;
        }
        static int Level(int type)
        {
            var d=Read(gsd,"SkillLevel") as IDictionary;Need(d!=null,"Original live skill dictionary");foreach(DictionaryEntry e in d)if(I(e.Key)==type)return I(e.Value);return 0;
        }
        static string Levels()
        {
            var a=new List<string>();foreach(DictionaryEntry e in (IDictionary)Read(gsd,"SkillLevel"))a.Add(I(e.Key)+":"+I(e.Value));a.Sort(StringComparer.Ordinal);return string.Join(";",a);
        }
        static PreviewLayer[] PreviewLayers(Animator a)
        {
            return Enumerable.Range(0,a.layerCount).Select(i=>new PreviewLayer{
                index=i,name=a.GetLayerName(i),weight=a.GetLayerWeight(i),transition=a.IsInTransition(i),
                currentHash=a.GetCurrentAnimatorStateInfo(i).fullPathHash,nextHash=a.GetNextAnimatorStateInfo(i).fullPathHash,
                currentTime=a.GetCurrentAnimatorStateInfo(i).normalizedTime,nextTime=a.GetNextAnimatorStateInfo(i).normalizedTime,
                current=a.GetCurrentAnimatorClipInfo(i).Where(x=>x.weight>0).Select(x=>new PreviewClip{name=x.clip.name,weight=x.weight}).ToArray(),
                next=a.GetNextAnimatorClipInfo(i).Where(x=>x.weight>0).Select(x=>new PreviewClip{name=x.clip.name,weight=x.weight}).ToArray()}).ToArray();
        }
        static bool PoseObserved(Snapshot s)
        {
            return s.poseId==10005&&s.previewLayers!=null&&s.previewLayers.Any(l=>l.index==1&&l.name=="New Layer"&&l.weight>0&&
                l.current.Concat(l.transition?l.next:new PreviewClip[0]).Any(c=>c.weight>0&&(c.name=="EroAction_Dogeza"||c.name=="EroAction_DogezaUpHead")));
        }
        static Snapshot Observe(string reason)
        {
            var s=new Snapshot{reason=reason,frame=Time.frameCount,phase=report.phase,time=Time.unscaledTime,scale=Time.timeScale,scene=SceneManager.GetActiveScene().path,product=Application.productName,rp=D(Read(gsd,"CurrentRp")),totalUsed=D(Read(gsd,"StatistisTotalUseRp")),rank=I(Read(Read(state,"CurrentRank"),"Rank")),level=Level(10005),skillLevels=Levels(),playerId=Id(player),playerPosition=playerTransform.position,playerAction=I(Read(Read(state,"CurrentAction"),"type")),menuPresent=menu,menuState=menu?I(Read(menu,"currentState")):0,child=menu&&Z(Read(menu,"isOpenChild")),stack=((IList)Read(ui,"basePanelStack")).Count,forceInactive=Z(Read(ui,"IsForceDeActive")),cancelUi=Z(Read(ui,"IsOpenSomeCancelInputUi")),allowUi=Z(Read(ui,"isOpenSomeAllowInputUi")),fade=Z(Read(ui,"IsOpenFade"))};
            var pi=PI();var move=pi.currentActionMap.FindAction("Move",false);s.moveEnabled=move!=null&&move.enabled;s.keyboardPaired=pi.user.valid&&pi.user.pairedDevices.Any(d=>d.deviceId==keyboard.deviceId);s.mousePaired=pi.user.valid&&pi.user.pairedDevices.Any(d=>d.deviceId==mouse.deviceId);
            if(panel){s.selectedIndex=I(Read(panel,"currentSelectIndex"));s.tabIndex=I(Read(Read(panel,"tabView"),"CurrentSelect"));s.previewPanel=((GameObject)Read(panel,"uiPlayerPanelObject")).activeInHierarchy;}
            if(item){var r=Read(item,"skill");s.type=I(Read(r,"Type"));s.category=I(Read(r,"CategoryType"));s.order=I(Read(r,"ReleaseOrderTarget"));s.releaseRank=I(Read(r,"ReleaseRank"));s.maxLevel=I(Read(r,"MaxLevel"));s.price=I(Read(item,"necessaryRp"));s.masked=Z(Read(item,"isSomeMask"));s.learned=Z(Read(item,"isLearned"));s.purchaseVisible=((GameObject)Read(item,"reinforceButtonObject")).activeInHierarchy;s.learnedVisible=((GameObject)Read(item,"learnedText")).activeInHierarchy;}
            var preview=Static(PreviewType,"Instance");if(Alive(preview)){s.previewContent=((GameObject)Read(preview,"contentParent")).activeInHierarchy;var a=Read(preview,"uiPlayerAnimator") as Animator;if(a){s.previewAnimatorId=Id(a);s.poseId=a.GetInteger("PoseId");s.previewPath=ObjectPath(a.transform);s.previewController=a.runtimeAnimatorController?a.runtimeAnimatorController.name:"";s.previewLayers=PreviewLayers(a);s.previewStateTime=a.GetCurrentAnimatorStateInfo(0).normalizedTime;s.previewClipNames=string.Join(";",a.GetCurrentAnimatorClipInfo(0).Where(x=>x.weight>0).Select(x=>x.clip.name+":"+x.weight));}}
            return s;
        }
        static void RecordSnapshot(string reason){Need(report.snapshots.Count<100,"Bounded snapshots");report.snapshots.Add(Observe(reason));}
        static void SaveIsolation()
        {
            Need(Application.companyName=="SFMRecovery"&&Application.productName==contract.testProduct&&Path.GetFullPath(Application.persistentDataPath)==Path.GetFullPath(contract.testProfilePath),"Current SDK product/profile exactly isolated");
            var settings=Static("ES3Settings","_defaults");var cached=Static("ES3Internal.ES3IO","persistentDataPath") as string;
            Need(settings!=null&&I(Read(settings,"_location"))==0&&I(Read(settings,"directory"))==0&&!string.IsNullOrEmpty(cached)&&Path.GetFullPath(cached)==Path.GetFullPath(contract.testProfilePath),"ES3 File/Persistent cache matches isolated root");
        }
        static bool ScrollIntoView(Button b)
        {
            var rt=b.transform as RectTransform;var point=Center(rt);var sr=b.GetComponentInParent<ScrollRect>();Need(sr&&sr.viewport,"Original scroll view");
            var corners=new Vector3[4];sr.viewport.GetWorldCorners(corners);var canvas=sr.GetComponentInParent<Canvas>();var cam=canvas.renderMode==RenderMode.ScreenSpaceOverlay?null:canvas.worldCamera;
            Vector2 low=RectTransformUtility.WorldToScreenPoint(cam,corners[0]),high=RectTransformUtility.WorldToScreenPoint(cam,corners[2]);
            if(point.y>low.y+18f&&point.y<high.y-18f)return true;
            Need(report.scrollSteps<12,"Bounded real scroll transactions to Dogeza");mousePoint=(low+high)*0.5f;
            if(Time.frameCount-scrollFrame>=12){scroll=new Vector2(0,point.y<=low.y+18f?-120f:120f);scrollFrame=Time.frameCount;report.scrollSteps++;}return false;
        }
        static void Advance(int phase){report.phase=phase;report.phaseFrame=Time.frameCount;report.phaseTime=Time.unscaledTime;}
        static void StartShot()
        {
            if(!string.IsNullOrEmpty(report.screenshot))return;report.screenshot=Path.Combine(contract.output,"skill-pose-preview.png");ScreenCapture.CaptureScreenshot(report.screenshot,1);report.screenshotTime=Time.unscaledTime;
        }
        static bool ShotReady()
        {
            if(report.screenshot!=null&&File.Exists(report.screenshot)&&new FileInfo(report.screenshot).Length>100){report.screenshotSHA=Hash(report.screenshot);report.screenshotReady=true;}return report.screenshotReady;
        }
        static void Poll()
        {
            if(!Armed||finished)return;
            try
            {
                Need(DateTime.UtcNow<deadline,"Finite 240-second run");if(!Application.isPlaying)return;
                if(Time.frameCount<1||Time.frameCount==lastFrame)return;lastFrame=Time.frameCount;report.actualPlayer=true;SetupSDK();
                Need(report.errors.Count==0,"No actual Player errors");scroll=Vector2.zero;mouseDelta=Vector2.zero;Key[] keys=new Key[0];
                if(!Bind()){Need(Time.unscaledTime<30f,"Finite original startup");Queue(keys);return;}
                SaveIsolation();var s=Observe("sample");report.lastSample=s;Need(s.scene==Home,"Home-only learning flow");Need(report.phase==0||Time.unscaledTime-report.phaseTime<25f,"Finite 25-second phase "+report.phase);
                if(report.phase==13||report.phase==14||report.phase==25||report.phase==26)RecordMoveSample();
                foreach(var tap in report.taps)tap.rawOne|=Raw(tap.raw)==1;
                switch(report.phase)
                {
                    case 0:
                        Need(s.keyboardPaired&&s.mousePaired,"Reviewed temporary SDK routing is active");
                        if(s.stack!=0||s.fade||s.forceInactive||s.cancelUi||s.allowUi||!s.moveEnabled||Time.unscaledTime<2f)break;
                        var uiOptions=ObserveUiOptions("home-baseline");report.uiOptions.Add(uiOptions);
                        Need(uiOptions.matched,"Original Initialize applies actual option fields, scale and mixer values");report.uiOptionsInitiallyMatched=true;
                        report.baseline=s;RecordSnapshot("home-baseline");Advance(1);break;
                    case 1:
                        if(TapKey("open-menu","GameMenu",2,out keys))Advance(2);break;
                    case 2:
                        if(report.clicks.Any(x=>x.role=="open-skill")||(s.menuState==1&&!s.child&&ReferenceEquals(Top(),menu)&&!s.forceInactive&&Time.unscaledTime-report.phaseTime>0.5f))
                            if(ClickButton("open-skill",ButtonFor(menu,menu,"OnClickSkill")))Advance(3);break;
                    case 3:
                        if(panel&&ReferenceEquals(Top(),panel)&&!s.forceInactive&&Time.unscaledTime-report.phaseTime>0.5f)
                            if(ClickButton("pose-category",ButtonFor(panel,Read(panel,"tabView") as UnityEngine.Object,"OnClickTab",1)))Advance(4);break;
                    case 4:
                        if(!panel||s.tabIndex!=1)break;
                        var rows=Read(panel,"childViewList") as IList;Need(rows!=null,"Original category rows");var targets=rows.Cast<object>().Where(x=>I(Read(Read(x,"skill"),"Type"))==10005).ToArray();Need(targets.Length==1,"One actual ordinary Dogeza row");item=(Behaviour)targets[0];
                        var select=ButtonFor(item,item,"OnClick");if(ScrollIntoView(select)&&ClickButton("select-dogeza",select))Advance(5);break;
                    case 5:
                        s=Observe("selected");if(Time.unscaledTime-report.phaseTime<0.8f)break;
                        Need(s.type==10005&&s.category==1&&s.maxLevel==1&&s.order==-1&&s.releaseRank==0,"Pinned original ordinary skill metadata");
                        Need(s.previewContent&&s.previewPanel&&s.poseId==10005&&s.previewAnimatorId!=0&&PoseObserved(s),"Actual original Dogeza clip on positive-weight pose layer");report.previewPoseObserved=true;
                        Need(s.playerAction==report.baseline.playerAction,"Preview does not execute actual player action");report.beforeBuy=s;RecordSnapshot("before-learning");
                        if(s.level!=0||s.masked||s.price<=0||s.rp<s.price){report.status="partial-existing-level-or-unaffordable";report.failure="Actual level/mask/price/balance does not permit a fresh ordinary learning transaction; no reset or funding";StartShot();Advance(9);break;}
                        Need(s.price==20,"Current original Dogeza first-learning price20");purchaseButton=ButtonFor(item,item,"OnClickReinforce");Advance(6);break;
                    case 6:
                        if(ClickButton("learn",purchaseButton))Advance(7);break;
                    case 7:
                        s=Observe("learned");if(Time.unscaledTime-report.phaseTime<0.5f)break;
                        Need(s.level==1&&s.rp==report.beforeBuy.rp-report.beforeBuy.price&&s.totalUsed==report.beforeBuy.totalUsed+report.beforeBuy.price,"Real original charge, level and total-spend changes");
                        Need(s.learned&&s.learnedVisible&&!s.purchaseVisible&&s.poseId==10005,"Max-level UI and selected independent preview");
                        Need(OtherSkills(s.skillLevels)==OtherSkills(report.beforeBuy.skillLevels),"No unrelated skill levels changed");report.learned=true;report.afterBuy=s;RecordSnapshot("after-learning");StartShot();Advance(8);break;
                    case 8:
                        if(ClickButton("repeat-hidden-purchase",purchaseButton,true))
                        {s=Observe("max-level-ui-repeat");Need(s.level==report.afterBuy.level&&s.rp==report.afterBuy.rp&&s.totalUsed==report.afterBuy.totalUsed&&!s.purchaseVisible,"Repeat UI attempt cannot purchase again");report.maxLevelUiRejected=true;report.methodGuardRuntimeAccepted=false;RecordSnapshot("repeat-ui-refused");Advance(9);}break;
                    case 9:
                        if(!ShotReady()){if(Time.unscaledTime-report.phaseTime<10f)break;report.screenshotTimedOut=true;}
                        if(TapKey("cancel-skill","Cancel",10,out keys))Advance(10);break;
                    case 10:
                        if(!panel&&!s.child&&s.menuState==1&&ReferenceEquals(Top(),menu)&&!s.forceInactive)
                        {report.returnedToMenu=true;RecordSnapshot("returned-to-main-menu");Advance(11);}break;
                    case 11:
                        if(TapKey("cancel-main-menu","Cancel",10,out keys))Advance(12);break;
                    case 12:
                        if(s.stack==0&&s.menuState==0&&!s.child&&!s.forceInactive&&!s.cancelUi&&!s.allowUi&&!s.fade&&s.moveEnabled)
                        {report.returnedToHome=true;RecordSnapshot("home-input-restored");report.moveStart=playerTransform.position;report.movePressFrame=Time.frameCount;Advance(13);}break;
                    case 13:
                        if(Time.unscaledTime-report.phaseTime<0.5f)keys=new[]{ForwardKey()};
                        else {report.moveReleaseFrame=Time.frameCount;report.moveReleased=playerTransform.position;Advance(14);}break;
                    case 14:
                        if(Time.unscaledTime-report.phaseTime<0.6f)break;report.moveFinal=playerTransform.position;
                        report.movementAccepted=report.movePressSeen&&report.moveReleaseSeen&&XZ(report.moveStart,report.moveReleased)>0.05f&&XZ(report.moveReleased,report.moveFinal)<0.08f;
                        Need(report.movementAccepted,"Real short walking displacement and bounded release tail after menus");SummarizeStopTail();RecordSnapshot("final");
                        report.flowAccepted=report.previewPoseObserved&&report.learned&&report.maxLevelUiRejected&&report.returnedToMenu&&report.returnedToHome&&report.movementAccepted&&report.screenshotReady;
                        if(!report.learned){report.status="learning-not-observed-pose-flow-skipped";Finish(null);return;}
                        report.flowAccepted=false;PreparePose();report.beforeRing=ObservePose("before-ring",true);Advance(15);break;
                    case 15:
                        keys=new[]{EffectiveKey("SkillRingMenu")};
                        if(Z(Read(Ring(),"IsOpenRing"))){report.ringOpened=true;PoseRecord("ring-opened");Advance(16);}break;
                    case 16:
                        keys=new[]{EffectiveKey("SkillRingMenu")};
                        var ring=Ring();Need(Z(Read(ring,"IsOpenRing")),"Original ring remains held");
                        int page=I(Read(ring,"CurrentRingIndexSkill"));
                        // Finish the key release acknowledgement even when the press already changed the page.
                        if(page!=report.targetRingPage||report.taps.Any(t=>t.role=="ring-next-page-"+report.pageSteps))
                        {
                            Need(report.pageSteps<4,"Bounded real ring page input");Key[] pageKeys;
                            if(TapKey("ring-next-page-"+report.pageSteps,"UiRingRight",1011,out pageKeys)){report.pageSteps++;}
                            keys=keys.Concat(pageKeys).Distinct().ToArray();break;
                        }
                        var selected=ObservePose("ring-select",false);
                        if(selected.selectedRingItem==report.targetRingItem&&selected.selectedReleased)
                        {
                            if(report.ringSelectedSince<0)report.ringSelectedSince=Time.unscaledTime;
                            if(Time.unscaledTime-report.ringSelectedSince>.3f){report.ringSelectionObserved=true;PoseRecord("ring-target-selected");CapturePose("pose-ring-selected");Advance(17);}
                        }
                        else
                        {
                            report.ringSelectedSince=-1;
                            float radians=(90f-(report.targetRingSlot+.5f)*45f)*Mathf.Deg2Rad;
                            mouseDelta=new Vector2(Mathf.Cos(radians),Mathf.Sin(radians))*12f;
                        }
                        break;
                    case 17:
                        var binding=ObservePose("ring-released",false);
                        if(!binding.ringOpen&&!Z(Read(Ring(),"lockOpen"))&&binding.shortcutIndex==report.targetModelIndex&&binding.action==-1&&binding.cameraLocked==report.beforeRing.cameraLocked)
                        {report.shortcutBound=true;PoseRecord("original-shortcut-bound");Advance(18);}break;
                    case 18:
                        if(TapKey("pose-shortcut-enter","SkillRingMenu",23,out keys))Advance(19);break;
                    case 19:
                        var entered=ObservePose("pose-entered",true);
                        if(entered.action==50005&&entered.animatorAction==50005&&HasActorPoseClip(entered)&&Time.unscaledTime-report.phaseTime>.65f)
                        {Need(entered.motionCount>0,"Original selected pose supports another motion");report.poseEntered=true;report.enteredPose=entered;report.variantBefore=entered.variantTarget;report.poseSnapshots.Add(entered);CapturePose("pose-entered");Advance(20);}break;
                    case 20:
                        if(TapKey("pose-variant-interact","Interact",8,out keys))Advance(21);break;
                    case 21:
                        var variant=ObservePose("pose-variant",true);
                        float expectedVariant=(report.variantBefore+1f)%(report.enteredPose.motionCount+1);
                        if(variant.action==50005&&Mathf.Abs(variant.variantTarget-expectedVariant)<.001f&&Mathf.Abs(variant.animatorVariant-expectedVariant)<.05f&&HasActorPoseClip(variant)&&Time.unscaledTime-report.phaseTime>.75f)
                        {report.poseVariantChanged=true;report.variantPose=variant;report.poseSnapshots.Add(variant);CapturePose("pose-variant");Advance(22);}break;
                    case 22:
                        if(!CapturesReady())break;
                        if(TapKey("pose-shortcut-cancel","SkillRingMenu",23,out keys))Advance(23);break;
                    case 23:
                        var cancelled=ObservePose("pose-cancelled",true);
                        if(cancelled.action==-1&&cancelled.animatorAction==-1&&!cancelled.ringOpen&&cancelled.cameraLocked==report.beforeRing.cameraLocked&&Time.unscaledTime-report.phaseTime>.6f)
                        {report.poseCancelled=true;report.cancelledPose=cancelled;report.poseSnapshots.Add(cancelled);CapturePose("pose-cancelled");Advance(24);}break;
                    case 24:
                        if(!CapturesReady())break;
                        report.poseMoveStart=playerTransform.position;report.poseMovePressFrame=Time.frameCount;Advance(25);break;
                    case 25:
                        Need(s.playerAction==-1,"Pose remains cancelled during walking");
                        if(Time.unscaledTime-report.phaseTime<.5f)keys=new[]{ForwardKey()};
                        else{report.poseMoveReleaseFrame=Time.frameCount;report.poseMoveReleased=playerTransform.position;Advance(26);}break;
                    case 26:
                        if(Time.unscaledTime-report.phaseTime<.6f)break;
                        report.poseMoveFinal=playerTransform.position;SummarizeStopTail();
                        report.poseMovementAccepted=report.poseMovePressSeen&&report.poseMoveReleaseSeen&&XZ(report.poseMoveStart,report.poseMoveReleased)>.05f&&XZ(report.poseMoveReleased,report.poseMoveFinal)<.08f;
                        Need(report.poseMovementAccepted&&report.stopTailAllSdkZero&&report.stopTailSamples>=2&&report.stopTailMaxDisplacement<.001f,"Real post-pose walking and sampled stop tail");PoseRecord("post-pose-walk-stopped");CapturePose("post-pose-walk-stopped");Advance(27);break;
                    case 27:
                        if(!CapturesReady())break;
                        report.uiOptions.Add(ObserveUiOptions("post-pose-return"));
                        Need(report.uiOptionsInitiallyMatched,"Current options were applied through original initialization");
                        Need(report.poseInputSeen&&report.poseInteractSeen,"Original decoded pose and interact input observed");
                        report.flowAccepted=report.previewPoseObserved&&report.learned&&report.maxLevelUiRejected&&report.returnedToHome&&report.movementAccepted&&report.screenshotReady&&report.shortcutBound&&report.poseEntered&&report.poseVariantChanged&&report.poseCancelled&&report.poseMovementAccepted;
                        if(report.flowAccepted)report.status="original-learning-pose-variant-cancel-movement-pending-visual-review";
                        Finish(null);return;
                }
                Queue(keys);
            }catch(Exception e){Finish(e);}
        }
        static UiOptions ObserveUiOptions(string reason)
        {
            var applier=Read(Read(manager,"InGameReferencer"),"UiOptionApplyer");Need(Alive(applier),"Original UI option applier");
            var options=Static(G+"GameState","OptionData");
            string[][] pairs={new[]{"faceCamera","FaceCamera"},new[]{"faceCameraPanel","FaceCamera"},new[]{"bodyCamera","BodyCamera"},new[]{"bodyCameraPanel","BodyCamera"},new[]{"playerInfoPanel","PlayerInfo"},new[]{"heartRatePanel","PlayerInfo"},new[]{"ecstasyGauge","EcstasyGauge"},new[]{"compass","Compass"},new[]{"statusInfo","RightDownPanel"},new[]{"shortcutPanel","RightDownPanel"},new[]{"shortcutPanel2","RightDownPanel"},new[]{"interactPanel","InteractPanel"},new[]{"heartBeatEffect","HeartBeatEffect"},new[]{"manualPanel","Manual"}};
            var visibility=new List<UiVisibility>();foreach(var pair in pairs)
            {var obj=Read(applier,pair[0]) as GameObject;Need(obj,"Original option target exists "+pair[0]);visibility.Add(new UiVisibility{field=pair[0],option=pair[1],path=ObjectPath(obj.transform),desired=Z(Read(options,pair[1])),activeSelf=obj.activeSelf,activeInHierarchy=obj.activeInHierarchy});}
            var mixer=Read(applier,"audioMixer") as UnityEngine.Audio.AudioMixer;Need(mixer,"Original mixer");
            string[][] audioPairs={new[]{"Master","SoundMaster100"},new[]{"Environment","SoundSe100"},new[]{"SystemSe","SoundUi100"},new[]{"Bgm","SoundBgm100"}};
            var audio=new List<UiAudio>();foreach(var pair in audioPairs)
            {int percent=I(Read(options,pair[1]));float actual;bool readable=mixer.GetFloat(pair[0],out actual);audio.Add(new UiAudio{parameter=pair[0],option=pair[1],percent=percent,readable=readable,actual=actual,expected=Mathf.Clamp(Mathf.Log10(percent/100f)*20f+6f,-80f,20f)});}
            var target=Read(applier,"missionPanelSizeTarget") as Transform;Need(target,"Original mission scale target");int size=I(Read(options,"MissionPanelSize100"));
            var lookups=Static(B+"EventManager","s_EventLookups") as IDictionary;Need(lookups!=null,"Original listener lookup");int listeners=0;
            foreach(DictionaryEntry entry in lookups){var callback=entry.Key as Delegate;if(callback!=null&&ReferenceEquals(callback.Target,applier)&&callback.Method.Name=="OnChange")listeners++;}
            return new UiOptions{reason=reason,frame=Time.frameCount,visibility=visibility.ToArray(),audio=audio.ToArray(),scalePercent=size,scale=target.localScale,ownListenerCount=listeners,
                matched=visibility.All(v=>v.desired==v.activeSelf)&&audio.All(a=>a.readable&&Mathf.Abs(a.actual-a.expected)<.0001f)&&(target.localScale-Vector3.one*size*.01f).sqrMagnitude<.000001f&&listeners==1};
        }
        static object Ring()=>Static("ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView","Instance");
        static void PreparePose()
        {
            var list=Static("ExposureUnnoticed2.Master.Skill.MSkill","poseList") as IList;Need(list!=null,"Original initialized pose order");
            var hits=Enumerable.Range(0,list.Count).Where(i=>I(Read(list[i],"Type"))==10005).ToArray();Need(hits.Length==1,"One original learned Dogeza index");
            report.poseIndex=hits[0];report.targetRingItem=10000+hits[0];report.targetModelIndex=8+hits[0];report.targetRingPage=report.targetModelIndex/8;report.targetRingSlot=report.targetModelIndex%8;
            var models=Read(Ring(),"commonRingMenuModel") as IList;Need(models!=null&&report.targetModelIndex<models.Count,"Original ring model range");
            var model=models[report.targetModelIndex];Need(I(Read(model,"Index"))==report.targetModelIndex&&I(Read(model,"RingItemType"))==report.targetRingItem,"Actual original pose/ring index mapping");
        }
        static PoseState ObservePose(string reason,bool layers)
        {
            var action=Read(state,"CurrentAction");var animation=Read(pca,"PlayerAnimationManager");var animator=Read(animation,"animator") as Animator;Need(animator,"Actual actor Animator");
            var input=Read(Read(player,"inputResolver"),"inputData");var other=Read(animation,"anotherMotionIndex");var ring=Ring();
            var camera=Read(Read(manager,"InGameReferencer"),"BaseCameraController");
            var result=new PoseState{reason=reason,frame=Time.frameCount,phase=report.phase,time=Time.unscaledTime,action=I(Read(action,"type")),motionCount=I(Read(action,"AnotherMotionCount")),
                animatorAction=animator.GetInteger("ActionId"),animatorVariant=animator.GetFloat("AnotherMotionIndex"),variantTarget=(float)Read(other,"Target"),variantCurrent=(float)Read(other,"Current"),
                cooldown=(float)Read(state,"PoseActionCoolTime"),justPose=I(Read(input,"JustPoseSkill")),justInteract=Z(Read(input,"JustInteract")),decodedMove=(Vector3)Read(input,"Move"),
                ringShortcut=I(Static("ExposureUnnoticed2.Object3D.Player.Scripts.InputResolver","RingShortcutInput")),ringOpen=Z(Read(ring,"IsOpenRing")),
                shortcutIndex=I(Read(gsd,"RingShortcutModelIndexSkill")),page=I(Read(ring,"CurrentRingIndexSkill")),cameraLocked=Z(Read(camera,"IsLockCamera")),
                cursorLock=(int)Cursor.lockState,cursorVisible=Cursor.visible,position=playerTransform.position,sdkMove=PI().currentActionMap.FindAction("Move",true).ReadValue<Vector2>(),
                controller=animator.runtimeAnimatorController?animator.runtimeAnimatorController.name:"",layers=layers?PreviewLayers(animator):null};
            var rings=Read(ring,"openRingList") as IList;
            if(rings!=null)foreach(var view in rings)if(I(Read(view,"ringIndex"))==result.page)
            {var selected=Read(view,"currentSelectView");if(Alive(selected)){var model=Read(selected,"Model");if(model!=null){result.selectedRingItem=I(Read(model,"RingItemType"));result.selectedReleased=Z(Read(selected,"IsRelease"));}}}
            return result;
        }
        static bool HasActorPoseClip(PoseState s)=>s.layers!=null&&s.layers.Any(l=>l.index==1&&l.weight>0&&l.current.Concat(l.transition?l.next:new PreviewClip[0]).Any(c=>c.weight>0&&(c.name=="EroAction_Dogeza"||c.name=="EroAction_DogezaUpHead")));
        static void PoseRecord(string reason){Need(report.poseSnapshots.Count<30,"Bounded pose milestones");report.poseSnapshots.Add(ObservePose(reason,true));}
        static void CapturePose(string role)
        {
            if(report.captures.Any(c=>c.role==role))return;Need(report.captures.Count<8,"Bounded original frame captures");
            var capture=new Capture{role=role,path=Path.Combine(contract.output,role+".png"),frame=Time.frameCount};report.captures.Add(capture);ScreenCapture.CaptureScreenshot(capture.path,1);
        }
        static bool CapturesReady()
        {
            foreach(var capture in report.captures)if(!capture.ready&&File.Exists(capture.path)&&new FileInfo(capture.path).Length>100)
            {capture.bytes=new FileInfo(capture.path).Length;capture.sha256=Hash(capture.path);capture.ready=true;}
            return report.captures.Count>0&&report.captures.All(c=>c.ready);
        }
        static float XZ(Vector3 a,Vector3 b)=>new Vector2(a.x-b.x,a.z-b.z).magnitude;
        static string OtherSkills(string s)=>string.Join(";",s.Split(';').Where(x=>!x.StartsWith("10005:",StringComparison.Ordinal)));
        static void Log(string message,string stack,LogType type)
        {
            if(!Armed||(type!=LogType.Error&&type!=LogType.Exception&&type!=LogType.Assert))return;if(report.errors.Count<25)report.errors.Add(message+"\n"+stack);
        }
        static void CleanupStep(string name,Action action)
        {try{action();}catch(Exception e){report.flowAccepted=false;report.passed=false;report.cleanupErrors.Add(name+": "+e.Message);}}
        static void RemoveDevice(string kind)
        {
            int id=kind=="keyboard"?keyboardId:mouseId;var d=InputSystem.GetDeviceById(id);
            if(d!=null){Need(d.name.StartsWith(DevicePrefix,StringComparison.Ordinal),"Remove only owned device");InputSystem.RemoveDevice(d);}
        }
        static void RestorePairing()
        {
            if(!pairingCaptured)return;
            try
            {
                CleanupStep("original-pairing",()=>
                {
                    Need(routedInput,"Restore original SDK pairing while Play object still exists");
                    var devices=report.originalDeviceIds.Select(InputSystem.GetDeviceById).ToArray();Need(devices.All(x=>x!=null&&x.added),"Original SDK devices still exist");
                    var user=routedInput.user;Need(user.valid&&user.id==report.originalInputUserId,"Restore the same original InputUser");
                    // InputUser supports a default (absent) scheme. Do not auto-pair any extra devices.
                    {
                        user.UnpairDevices();
                        foreach(var device in devices)InputUser.PerformPairingWithDevice(device,user:user);
                        user.ActivateControlScheme(originalUserScheme??default(InputControlScheme));
                    }
                    var restored=routedInput.user.controlScheme;
                    bool sameScheme=restored.HasValue==report.originalSchemePresent&&(!restored.HasValue||restored.Value.Equals(originalUserScheme.Value));
                    report.pairingRestored=routedInput.user.valid&&routedInput.user.id==report.originalInputUserId&&sameScheme&&routedInput.user.pairedDevices.Select(x=>x.deviceId).OrderBy(x=>x).SequenceEqual(report.originalDeviceIds.OrderBy(x=>x));Need(report.pairingRestored,"Original nullable scheme and exact SDK pairing restored");
                });
            }
            finally
            {
                CleanupStep("original-auto-switch",()=>{Need(routedInput,"Original SDK object for auto-switch restore");routedInput.neverAutoSwitchControlSchemes=report.originalNeverAutoSwitch;report.autoSwitchRestored=routedInput.neverAutoSwitchControlSchemes==report.originalNeverAutoSwitch;Need(report.autoSwitchRestored,"Original auto-switch restored");});
                pairingCaptured=false;
            }
        }
        static void RecordMoveSample()
        {
            Need(report.movement.Count<4096,"Bounded movement tail telemetry");
            var action=PI().currentActionMap.FindAction("Move",true);
            var sample=new MoveSample{frame=Time.frameCount,phase=report.phase,time=Time.unscaledTime,position=playerTransform.position,sdkMove=action.ReadValue<Vector2>()};
            var previous=report.movement.LastOrDefault();
            if(previous!=null){sample.hasPrevious=true;sample.dt=sample.time-previous.time;sample.stepXZ=XZ(previous.position,sample.position);sample.planarSpeed=sample.dt>0?sample.stepXZ/sample.dt:0;}
            report.movement.Add(sample);
        }
        static void SummarizeStopTail()
        {
            var tail=report.movement.Where(x=>x.phase==report.phase&&x.time>=Time.unscaledTime-.2f).ToArray();
            report.stopTailSamples=tail.Length;if(tail.Length==0)return;
            report.stopTailDuration=tail.Last().time-tail[0].time;report.stopTailAllSdkZero=tail.All(x=>x.sdkMove==Vector2.zero);
            report.stopTailMaxDisplacement=tail.Max(x=>XZ(tail[0].position,x.position));
        }
        static void Cleanup()
        {
            try{RestorePairing();}
            finally{try{CleanupStep("keyboard",()=>RemoveDevice("keyboard"));}
            finally{try{CleanupStep("mouse",()=>RemoveDevice("mouse"));}
            finally{try{CleanupStep("background",()=>{if(backgroundCaptured)InputSystem.settings.backgroundBehavior=originalBackground;});}
            finally{try{CleanupStep("run-in-background",()=>{if(backgroundCaptured)Application.runInBackground=originalRunInBackground;});}
            finally{try{CleanupStep("identity",()=>{report.identityUnchanged=Application.companyName=="SFMRecovery"&&Application.productName==Product&&Path.GetFullPath(Application.persistentDataPath)==Path.GetFullPath(Profile);Need(report.identityUnchanged,"Baked identity unchanged");});}
            finally{CleanupStep("opaque-protection",()=>{foreach(var p in contract.protectedSaves)CheckPin(p);report.sourceSavesUnchanged=true;CheckTree(Profile);foreach(var p in Directory.GetFiles(Profile,"*",SearchOption.AllDirectories)){if(Path.GetExtension(p)==".sd")report.finalTestFiles.Add(new Pin{path=p,bytes=new FileInfo(p).Length,sha256=Hash(p)});}});}}}}}}
            report.sdkRestored=(!backgroundCaptured||(InputSystem.settings.backgroundBehavior==originalBackground&&Application.runInBackground==originalRunInBackground))&&InputSystem.GetDeviceById(keyboardId)==null&&InputSystem.GetDeviceById(mouseId)==null&&(!pairingWasCaptured||(report.pairingRestored&&report.autoSwitchRestored));
            if(report.cleanupErrors.Count>0||report.errors.Count>0||!report.sdkRestored||!report.identityUnchanged||!report.sourceSavesUnchanged)report.flowAccepted=false;
        }
        static void Finish(Exception error)
        {
            if(finished||finishing)return;
            finishing=true;finishingDeadline=DateTime.UtcNow.AddSeconds(2);
            if(error!=null){report.flowAccepted=false;report.status="partial-evidence";report.failure=error.ToString();}
            try{mouseLeft=false;scroll=Vector2.zero;if(keyboard!=null&&mouse!=null)Queue(new Key[0]);}
            catch(Exception e){report.cleanupErrors.Add("release-input: "+e.Message);report.flowAccepted=false;}
            finally{Flush();}
        }
        static void DrainAndComplete()
        {
            try
            {
                if(keyboard==null||mouse==null||report.inputReleaseObserved){Complete(true);return;}
                if(DateTime.UtcNow>=finishingDeadline){report.cleanupErrors.Add("Owned input release was not observed during Dynamic update before timeout");report.flowAccepted=false;Complete(true);return;}
                mouseLeft=false;scroll=Vector2.zero;Queue(new Key[0]);
            }
            catch(Exception e){report.cleanupErrors.Add("release-drain: "+e.Message);report.flowAccepted=false;Complete(true);}
        }
        static void Complete(bool quit)
        {
            if(completing||finished)return;completing=true;
            try{Cleanup();}
            catch(Exception e){report.flowAccepted=false;report.cleanupErrors.Add("cleanup-unhandled: "+e.Message);}
            finally
            {
                try{InputSystem.onAfterUpdate-=AfterInput;Application.logMessageReceived-=Log;}
                finally
                {
                    finished=true;Armed=false;report.reportComplete=true;report.visualAccepted=false;report.passed=false;
                    if(!report.flowAccepted&&(report.status=="player-input-completed-pending-visual-review"||report.status=="original-learning-pose-variant-cancel-movement-pending-visual-review"))report.status="partial-evidence";
                    try{Flush();}finally{if(quit)Application.Quit(report.flowAccepted&&report.screenshotReady&&report.errors.Count==0&&report.cleanupErrors.Count==0?0:6);}
                }
            }
        }
    }
}
