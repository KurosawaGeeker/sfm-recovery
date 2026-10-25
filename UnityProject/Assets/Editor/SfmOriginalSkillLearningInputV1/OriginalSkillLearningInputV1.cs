#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Key = UnityEngine.InputSystem.Key;

namespace SfmOriginalSkillLearningInputV1
{
    // Owned Editor observation only. All gameplay effects must originate in original UI processing.
    [InitializeOnLoad]
    public static class OriginalSkillLearningInputV1
    {
        const string Prefix = "SfmOriginalSkillLearningInputV1.";
        const string Flag = "-sfmSkillLearningV1Contract";
        const string DevicePrefix = "SfmSkillLearningV1-";
        const string Project = @"E:\ACTgame\SFM-LayerFS-Work\UnityProject";
        const string Home = "Assets/SfmOriginalGameplayV3/Apartment.unity";
        const string G = "ExposureUnnoticed2.Scripts.InGame.";
        const string B = "ExposureUnnoticed2.Scripts.Base.";
        const string UI = "ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager";
        const string MenuType = "ExposureUnnoticed2.ObjectUI.InGameMenu.InGameMenuView";
        const string PanelType = "ExposureUnnoticed2.ObjectUI.SaveDataSelectPanel.SkillPanelView";
        const string PreviewType = "ExposureUnnoticed2.Object3D.UIPlayer.UiPlayerController";
        const BindingFlags Fields = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance | BindingFlags.DeclaredOnly;
        static readonly Assembly Game = typeof(global::ExposureUnnoticed2.Scripts.InGame.InGameManager).Assembly;
        static readonly string Domain = Guid.NewGuid().ToString("N");
        [Serializable] public sealed class Pin { public string path, sha256; public long bytes; }
        [Serializable] public sealed class Contract
        {
            public string schema, editorGameSHA, editorAssemblySHA, testProduct, testProfilePath, output;
            public bool launchReady; public Pin[] inputs, testSaves, protectedSaves;
        }
        [Serializable] public sealed class ScenePin { public string path; public bool loaded, active; }
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
        [Serializable] public sealed class Report
        {
            public string schema="SfmSkillLearningInputObservation/v1", status="preflight", failure, beginDomain, playDomain, testProduct, testProfilePath, editorGameSHA, editorAssemblySHA, screenshot, screenshotSHA;
            public int phase, phaseFrame, checks, persistedSamples, scrollSteps, movePressFrame, moveReleaseFrame;
            public float phaseTime, startTime, screenshotTime; public Vector3 moveStart, moveReleased, moveFinal;
            public bool screenshotTimedOut; public bool previewPoseObserved; public bool actualPlayMode, domainReloadObserved, learned, maxLevelUiRejected, methodGuardRuntimeAccepted, returnedToMenu, returnedToHome, movementAccepted, movePressSeen, moveReleaseSeen, screenshotReady, sourceSavesUnchanged, sdkRestored, productRestored, scenesRestored, pairingRestored, autoSwitchRestored, reportComplete, passed;
            public string originalScheme; public int[] originalDeviceIds; public bool originalNeverAutoSwitch;
            public uint originalInputUserId; public bool originalSchemePresent, originalSceneSetupEmpty, sceneSetupExactlyRestored, emptySceneFallbackCreated;
            public string sceneRestorationMode; public int finalEditorSceneCount;
            public Snapshot baseline, beforeBuy, afterBuy, lastSample; public ScenePin[] originalScenes;
            public List<string> readResolutions=new List<string>(), errors=new List<string>(), cleanupErrors=new List<string>();
            public List<Snapshot> snapshots=new List<Snapshot>(); public List<Tap> taps=new List<Tap>(); public List<Click> clicks=new List<Click>(); public List<Pin> finalTestFiles=new List<Pin>();
        }
        static Contract contract; static Report report; static bool finished; static int lastFrame=-1;
        static Keyboard keyboard; static Mouse mouse; static Vector2 mousePoint, scroll; static bool mouseLeft;
        static object manager, player, pca, state, gsd, ui; static Transform playerTransform;
        static Behaviour menu, panel, item; static int scrollFrame=-100; static Button purchaseButton;
        static PlayerInput routedInput; static bool pairingCaptured;
        static InputControlScheme? originalUserScheme;
        static bool Armed => SessionState.GetBool(Prefix+"armed",false);
        static bool HasFlag => Environment.GetCommandLineArgs().Contains(Flag);
        static OriginalSkillLearningInputV1()
        {
            if (!HasFlag) return;
            EditorApplication.update += Poll; EditorApplication.playModeStateChanged += Changed;
            Application.logMessageReceived += Log; InputSystem.onAfterUpdate += AfterInput;
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
        static void Load()
        {
            if(report==null) report=JsonUtility.FromJson<Report>(SessionState.GetString(Prefix+"report","{}"));
            if(contract==null) contract=JsonUtility.FromJson<Contract>(SessionState.GetString(Prefix+"contractJson","{}"));
        }
        static void Save() { if(report==null)return; report.persistedSamples=report.snapshots.Count; SessionState.SetString(Prefix+"report",JsonUtility.ToJson(report)); }
        static void Write() { if(report!=null&&contract!=null&&SessionState.GetBool(Prefix+"outputOwned",false)&&Directory.Exists(contract.output)) File.WriteAllText(Path.Combine(contract.output,"observation.json"),JsonUtility.ToJson(report,true)+"\n"); }
        static void Flush()
        {
            try{CleanupStep("persist-session",Save);}finally{CleanupStep("write-report",Write);}
        }
        public static void Begin()
        {
            report=new Report{beginDomain=Domain};
            try
            {
                Need(HasFlag&&!Armed,"Only explicit skill observer flag; not already armed");
                SessionState.SetBool(Prefix+"outputOwned",false);
                Need(Path.GetFullPath(Path.Combine(Application.dataPath,".."))==Path.GetFullPath(Project),"Only the sole native project");
                Need(!EditorApplication.isPlayingOrWillChangePlaymode&&!EditorSettings.enterPlayModeOptionsEnabled,"Fresh default domain reload");
                string contractPath=Path.GetFullPath(Arg(Flag));Need(Hash(contractPath)==Arg("-sfmSkillLearningV1ContractSHA"),"Exact launch contract");
                contract=JsonUtility.FromJson<Contract>(File.ReadAllText(contractPath));
                Need(contract.schema=="SfmSkillLearningInputContract/v1"&&contract.launchReady,"Sealed current import contract");
                Need(Environment.GetCommandLineArgs().Contains("--sfm-manual-preview"),"Existing reviewed bootstrap identity flag");
                Need(!Directory.Exists(contract.output)&&Inside(contract.output,Path.Combine(Project,"../runs")),"Fresh bounded output under native runs");Directory.CreateDirectory(contract.output);SessionState.SetBool(Prefix+"outputOwned",true);
                foreach(var p in contract.inputs)CheckPin(p);
                report.editorGameSHA=Hash(Game.Location);report.editorAssemblySHA=Hash(typeof(OriginalSkillLearningInputV1).Assembly.Location);
                Need(report.editorGameSHA==contract.editorGameSHA&&report.editorAssemblySHA==contract.editorAssemblySHA,"Actual imported Game and observer identities");
                Need(Application.companyName=="SFMRecovery"&&PlayerSettings.productName=="SFMOriginalGameplayRuntimeV2","Original project identity");
                const string productPrefix="SFMOriginalGameplayInputVisual-";
                Need(contract.testProduct!=null&&contract.testProduct.StartsWith(productPrefix,StringComparison.Ordinal)&&Guid.TryParseExact(contract.testProduct.Substring(productPrefix.Length),"N",out _),"Existing bootstrap GUID identity");
                Need(Path.GetFullPath(contract.testProfilePath)==Path.GetFullPath(Path.Combine(@"C:\Users\Administrator\AppData\LocalLow\SFMRecovery",contract.testProduct)),"Exact lexical isolated root");
                Need(Directory.Exists(contract.testProfilePath)&&contract.testSaves.Length==3&&contract.protectedSaves.Length==3,"Parent prepared opaque copy and protected source");
                foreach(var p in contract.testSaves){Need(Inside(p.path,contract.testProfilePath),"Only isolated test input");CheckPin(p);}
                foreach(var p in contract.protectedSaves){Need(!Inside(p.path,contract.testProfilePath),"Protected source is outside test root");CheckPin(p);}
                foreach(string flag in Environment.GetCommandLineArgs().Where(x=>x.StartsWith("-sfm",StringComparison.Ordinal)))Need(flag==Flag||flag=="-sfmSkillLearningV1ContractSHA","No competing observer flag: "+flag);
                foreach(string other in new[]{"SfmOriginalHomeStreetSuccessDiagnosticV1","SfmOriginalBodyCustomizeInputV3","SfmOriginalNpcDetectionInputV23"})Need(!SessionState.GetBool(other+".armed",false),"No competing armed observer");
                var scenes=EditorSceneManager.GetSceneManagerSetup();
                for(int i=0;i<SceneManager.sceneCount;i++)Need(!SceneManager.GetSceneAt(i).isDirty,"Do not replace dirty Editor scenes");
                report.originalScenes=scenes.Select(x=>new ScenePin{path=x.path,loaded=x.isLoaded,active=x.isActive}).ToArray();
                report.originalSceneSetupEmpty=report.originalScenes.Length==0;
                SessionState.SetString(Prefix+"contract",contractPath);SessionState.SetString(Prefix+"contractJson",JsonUtility.ToJson(contract));SessionState.SetString(Prefix+"originalProduct",PlayerSettings.productName);
                SessionState.SetBool(Prefix+"productChanged",true);SessionState.SetBool(Prefix+"finished",false);
                SessionState.SetString(Prefix+"deadline",DateTime.UtcNow.AddSeconds(240).Ticks.ToString());
                report.testProduct=contract.testProduct;report.testProfilePath=contract.testProfilePath;Save();SessionState.SetBool(Prefix+"armed",true);
                PlayerSettings.productName=contract.testProduct;AssetDatabase.SaveAssets();
                SessionState.SetBool(Prefix+"scenesChanged",true);EditorSceneManager.OpenScene(Home,OpenSceneMode.Single);EditorApplication.isPlaying=true;
            }
            catch(Exception e) { Finish(e); }
        }
        static void SetupSDK()
        {
            if(!SessionState.GetBool(Prefix+"routing",false))
            {
                var s=InputSystem.settings;SessionState.SetInt(Prefix+"oldRouting",(int)s.editorInputBehaviorInPlayMode);SessionState.SetInt(Prefix+"oldBackground",(int)s.backgroundBehavior);SessionState.SetBool(Prefix+"routing",true);
                s.editorInputBehaviorInPlayMode=InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;s.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;
            }
            if(keyboard==null){keyboard=InputSystem.AddDevice<Keyboard>(DevicePrefix+"Keyboard");SessionState.SetInt(Prefix+"keyboard",keyboard.deviceId);}
            if(mouse==null){mouse=InputSystem.AddDevice<Mouse>(DevicePrefix+"Mouse");SessionState.SetInt(Prefix+"mouse",mouse.deviceId);}
            var pi=PI();
            if(pi&&!pairingCaptured)
            {
                Need(pi.user.valid&&pi.actions.controlSchemes.Any(x=>x.name=="Keyboard and Mouse"),"Original SDK user and supported scheme");
                // v2: a valid InputUser can have zero paired devices and/or no active control scheme.
                routedInput=pi;originalUserScheme=pi.user.controlScheme;report.originalInputUserId=pi.user.id;
                report.originalSchemePresent=originalUserScheme.HasValue;report.originalScheme=originalUserScheme.HasValue?originalUserScheme.Value.name:null;
                report.originalDeviceIds=pi.user.pairedDevices.Select(x=>x.deviceId).ToArray();report.originalNeverAutoSwitch=pi.neverAutoSwitchControlSchemes;
                pairingCaptured=true;SessionState.SetBool(Prefix+"pairingCaptured",true);
                pi.neverAutoSwitchControlSchemes=true;pi.SwitchCurrentControlScheme("Keyboard and Mouse",keyboard,mouse);
            }
            if(pairingCaptured)Need(routedInput&&routedInput.neverAutoSwitchControlSchemes&&routedInput.user.pairedDevices.Count==2&&routedInput.user.pairedDevices.Any(x=>x.deviceId==keyboard.deviceId)&&routedInput.user.pairedDevices.Any(x=>x.deviceId==mouse.deviceId),"Only owned SDK devices paired; physical manual-player input excluded");
        }
        static void Queue(Key[] keys)
        {
            if(keyboard!=null)InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));
            if(mouse!=null)InputSystem.QueueStateEvent(mouse,new MouseState{position=mousePoint,scroll=scroll}.WithButton(MouseButton.Left,mouseLeft));
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
            if(!Armed||finished||!EditorApplication.isPlaying||InputState.currentUpdateType!=InputUpdateType.Dynamic)return;
            try
            {
                Load();if(keyboard==null||mouse==null)return;var pi=PI();if(!pi||pi.currentActionMap==null)return;
                foreach(var t in report.taps){var a=pi.currentActionMap.FindAction(t.action,false);if(a==null||!a.enabled)continue;t.pressSeen|=keyboard[(Key)t.key].isPressed&&a.ReadValue<float>()>0.5f;t.releaseSeen|=t.releaseFrame>0&&!keyboard[(Key)t.key].isPressed&&a.ReadValue<float>()==0f;t.rawOne|=Raw(t.raw)==1;}
                var move=pi.currentActionMap.FindAction("Move",false);if(move!=null&&report.movePressFrame>0){report.movePressSeen|=move.enabled&&move.ReadValue<Vector2>().sqrMagnitude>0.5f;report.moveReleaseSeen|=report.moveReleaseFrame>0&&move.ReadValue<Vector2>().sqrMagnitude==0f;}
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
                l.current.Concat(l.next).Any(c=>c.weight>0&&(c.name=="EroAction_Dogeza"||c.name=="EroAction_DogezaUpHead")));
        }
        static Snapshot Observe(string reason)
        {
            var s=new Snapshot{reason=reason,frame=Time.frameCount,phase=report.phase,time=Time.unscaledTime,scale=Time.timeScale,scene=SceneManager.GetActiveScene().path,product=Application.productName,rp=D(Read(gsd,"CurrentRp")),totalUsed=D(Read(gsd,"StatistisTotalUseRp")),rank=I(Read(Read(state,"CurrentRank"),"Rank")),level=Level(10005),skillLevels=Levels(),playerId=Id(player),playerPosition=playerTransform.position,playerAction=I(Read(Read(state,"CurrentAction"),"type")),menuPresent=menu,menuState=menu?I(Read(menu,"currentState")):0,child=menu&&Z(Read(menu,"isOpenChild")),stack=((IList)Read(ui,"basePanelStack")).Count,forceInactive=Z(Read(ui,"IsForceDeActive")),cancelUi=Z(Read(ui,"IsOpenSomeCancelInputUi")),allowUi=Z(Read(ui,"isOpenSomeAllowInputUi")),fade=Z(Read(ui,"IsOpenFade"))};
            var pi=PI();var move=pi.currentActionMap.FindAction("Move",false);s.moveEnabled=move!=null&&move.enabled;s.keyboardPaired=pi.user.valid&&pi.user.pairedDevices.Any(d=>d.deviceId==keyboard.deviceId);s.mousePaired=pi.user.valid&&pi.user.pairedDevices.Any(d=>d.deviceId==mouse.deviceId);
            if(panel){s.selectedIndex=I(Read(panel,"currentSelectIndex"));s.tabIndex=I(Read(Read(panel,"tabView"),"CurrentSelect"));s.previewPanel=((GameObject)Read(panel,"uiPlayerPanelObject")).activeInHierarchy;}
            if(item){var r=Read(item,"skill");s.type=I(Read(r,"Type"));s.category=I(Read(r,"CategoryType"));s.order=I(Read(r,"ReleaseOrderTarget"));s.releaseRank=I(Read(r,"ReleaseRank"));s.maxLevel=I(Read(r,"MaxLevel"));s.price=I(Read(item,"necessaryRp"));s.masked=Z(Read(item,"isSomeMask"));s.learned=Z(Read(item,"isLearned"));s.purchaseVisible=((GameObject)Read(item,"reinforceButtonObject")).activeInHierarchy;s.learnedVisible=((GameObject)Read(item,"learnedText")).activeInHierarchy;}
            var preview=Static(PreviewType,"Instance");if(Alive(preview)){s.previewContent=((GameObject)Read(preview,"contentParent")).activeInHierarchy;var a=Read(preview,"uiPlayerAnimator") as Animator;if(a){s.previewAnimatorId=Id(a);s.poseId=a.GetInteger("PoseId");s.previewPath=ObjectPath(a.transform);s.previewController=a.runtimeAnimatorController?AssetDatabase.GetAssetPath(a.runtimeAnimatorController):"";s.previewLayers=PreviewLayers(a);s.previewStateTime=a.GetCurrentAnimatorStateInfo(0).normalizedTime;s.previewClipNames=string.Join(";",a.GetCurrentAnimatorClipInfo(0).Where(x=>x.weight>0).Select(x=>x.clip.name+":"+x.weight));}}
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
                Load();Need(DateTime.UtcNow.Ticks<long.Parse(SessionState.GetString(Prefix+"deadline","0")),"Finite 240-second run");if(!EditorApplication.isPlaying)return;
                if(Time.frameCount<1||Time.frameCount==lastFrame)return;lastFrame=Time.frameCount;report.actualPlayMode=true;report.playDomain=Domain;report.domainReloadObserved=Domain!=report.beginDomain;SetupSDK();
                Need(report.errors.Count==0,"No actual PlayMode errors");scroll=Vector2.zero;Key[] keys=new Key[0];
                if(!Bind()){Need(Time.unscaledTime<30f,"Finite original startup");Queue(keys);return;}
                SaveIsolation();var s=Observe("sample");report.lastSample=s;Need(s.scene==Home,"Home-only learning flow");Need(report.phase==0||Time.unscaledTime-report.phaseTime<25f,"Finite 25-second phase "+report.phase);
                foreach(var tap in report.taps)tap.rawOne|=Raw(tap.raw)==1;
                switch(report.phase)
                {
                    case 0:
                        Need(s.keyboardPaired&&s.mousePaired,"Reviewed temporary SDK routing is active");
                        if(s.stack!=0||s.fade||s.forceInactive||s.cancelUi||s.allowUi||!s.moveEnabled||Time.unscaledTime<2f)break;
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
                        Need(report.movementAccepted,"Real short walking displacement and release-to-stop after menus");RecordSnapshot("final");
                        report.passed=report.previewPoseObserved&&report.learned&&report.maxLevelUiRejected&&report.returnedToMenu&&report.returnedToHome&&report.movementAccepted&&report.screenshotReady;
                        if(report.passed)report.status="original-learning-and-return-observed-guard-not-executed";else if(report.screenshotTimedOut){report.status="learning-preview-state-and-return-observed-visual-capture-unavailable";report.failure="Real screenshot was requested but no file was produced; visual acceptance remains incomplete.";}Finish(null);return;
                }
                Queue(keys);
            }catch(Exception e){Finish(e);}
        }
        static float XZ(Vector3 a,Vector3 b)=>new Vector2(a.x-b.x,a.z-b.z).magnitude;
        static string OtherSkills(string s)=>string.Join(";",s.Split(';').Where(x=>!x.StartsWith("10005:",StringComparison.Ordinal)));
        static void Log(string message,string stack,LogType type)
        {
            if(!Armed||(type!=LogType.Error&&type!=LogType.Exception&&type!=LogType.Assert))return;Load();if(report.errors.Count<25)report.errors.Add(message+"\n"+stack);
        }
        static void CleanupStep(string name,Action action)
        {try{action();}catch(Exception e){report.passed=false;report.cleanupErrors.Add(name+": "+e.Message);}}
        static void RemoveDevice(string kind)
        {
            int id=SessionState.GetInt(Prefix+kind,0);var d=InputSystem.GetDeviceById(id);
            if(d!=null){Need(d.name.StartsWith(DevicePrefix,StringComparison.Ordinal),"Remove only owned device");InputSystem.RemoveDevice(d);}SessionState.EraseInt(Prefix+kind);
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
        static void Cleanup()
        {
            // Independent try/finally steps: one failure must not skip restoring identity or input routing.
            try {CleanupStep("keyboard",()=>RemoveDevice("keyboard"));}
            finally {try {CleanupStep("mouse",()=>RemoveDevice("mouse"));}
            finally {try {CleanupStep("input-routing",()=>{if(SessionState.GetBool(Prefix+"routing",false)){var s=InputSystem.settings;s.editorInputBehaviorInPlayMode=(InputSettings.EditorInputBehaviorInPlayMode)SessionState.GetInt(Prefix+"oldRouting",-1);s.backgroundBehavior=(InputSettings.BackgroundBehavior)SessionState.GetInt(Prefix+"oldBackground",-1);report.sdkRestored=(int)s.editorInputBehaviorInPlayMode==SessionState.GetInt(Prefix+"oldRouting",-1)&&(int)s.backgroundBehavior==SessionState.GetInt(Prefix+"oldBackground",-1);SessionState.SetBool(Prefix+"routing",false);}else report.sdkRestored=true;});}
            finally {try {CleanupStep("product",()=>{if(SessionState.GetBool(Prefix+"productChanged",false)){string original=SessionState.GetString(Prefix+"originalProduct","");Need(original=="SFMOriginalGameplayRuntimeV2","Exact recorded original product");PlayerSettings.productName=original;AssetDatabase.SaveAssets();report.productRestored=PlayerSettings.productName==original;SessionState.SetBool(Prefix+"productChanged",false);}else report.productRestored=true;});}
            finally {try {CleanupStep("scenes",()=>
            {
                if(SessionState.GetBool(Prefix+"scenesChanged",false))
                {
                    if(report.originalSceneSetupEmpty)
                    {
                        // Unity rejects RestoreSceneManagerSetup(empty); leave an explicit unsaved empty scene.
                        var blank=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
                        Need(blank.IsValid()&&blank.isLoaded&&blank.path==string.Empty&&blank.rootCount==0&&SceneManager.GetActiveScene()==blank&&SceneManager.sceneCount==1,"One actual empty Editor scene before exit");
                        report.emptySceneFallbackCreated=true;report.sceneSetupExactlyRestored=false;report.sceneRestorationMode="explicit-empty-scene-fallback";
                    }
                    else
                    {
                        EditorSceneManager.RestoreSceneManagerSetup(report.originalScenes.Select(x=>new SceneSetup{path=x.path,isLoaded=x.loaded,isActive=x.active}).ToArray());
                        var restored=EditorSceneManager.GetSceneManagerSetup();
                        report.sceneSetupExactlyRestored=restored.Length==report.originalScenes.Length&&restored.Select((x,i)=>x.path==report.originalScenes[i].path&&x.isLoaded==report.originalScenes[i].loaded&&x.isActive==report.originalScenes[i].active).All(x=>x);
                        Need(report.sceneSetupExactlyRestored,"Original nonempty Editor scene setup restored");report.sceneRestorationMode="exact-original-setup";
                    }
                    SessionState.SetBool(Prefix+"scenesChanged",false);
                }
                else report.sceneRestorationMode="scene-setup-not-changed";
                report.finalEditorSceneCount=SceneManager.sceneCount;report.scenesRestored=true;
            });}
            finally {CleanupStep("opaque-protection",()=>{if(contract==null)return;foreach(var p in contract.protectedSaves)CheckPin(p);report.sourceSavesUnchanged=true;foreach(var p in Directory.GetFiles(contract.testProfilePath,"*",SearchOption.AllDirectories)){if(Path.GetExtension(p)==".sd")report.finalTestFiles.Add(new Pin{path=p,bytes=new FileInfo(p).Length,sha256=Hash(p)});}});}}}}}
            if(report.cleanupErrors.Count>0||report.errors.Count>0||!report.sdkRestored||!report.productRestored||!report.scenesRestored||!report.sourceSavesUnchanged||SessionState.GetBool(Prefix+"pairingCaptured",false)&&(!report.pairingRestored||!report.autoSwitchRestored))report.passed=false;
        }
        static void Changed(PlayModeStateChange mode)
        {
            if(!Armed)return;Load();
            if(mode==PlayModeStateChange.ExitingPlayMode){try{CleanupStep("release-on-exit",()=>{mouseLeft=false;scroll=Vector2.zero;Queue(new Key[0]);});}finally{try{RestorePairing();}finally{Flush();}}}
            if(mode==PlayModeStateChange.EnteredEditMode)
            {
                report.reportComplete=report.persistedSamples==report.snapshots.Count&&SessionState.GetBool(Prefix+"finished",false);
                if(!report.reportComplete){report.passed=false;report.failure=(report.failure??"")+"; Unexpected exit or incomplete report";}
                try{Cleanup();}finally{SessionState.SetBool(Prefix+"armed",false);try{Flush();}finally{EditorApplication.delayCall+=()=>EditorApplication.Exit(report.passed?0:6);}}
            }
        }
        static void Finish(Exception error)
        {
            if(finished)return;finished=true;SessionState.SetBool(Prefix+"finished",true);
            if(error!=null){report.passed=false;report.status="partial-evidence";report.failure=error.ToString();}
            try{mouseLeft=false;scroll=Vector2.zero;Queue(new Key[0]);}
            catch(Exception e){report.cleanupErrors.Add("release-input: "+e.Message);report.passed=false;}
            finally
            {
                try{Flush();}
                finally
                {
                    if(EditorApplication.isPlayingOrWillChangePlaymode)EditorApplication.isPlaying=false;
                    else {try{RestorePairing();}finally{try{Cleanup();}finally{SessionState.SetBool(Prefix+"armed",false);try{Flush();}finally{EditorApplication.delayCall+=()=>EditorApplication.Exit(report.passed?0:6);}}}}
                }
            }
        }
    }
}
#endif
