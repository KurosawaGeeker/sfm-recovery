using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Key = UnityEngine.InputSystem.Key;

namespace SfmMvp
{
    // Owned standalone observer: original Ring1 hold/release only; no Game calls or writes.
    public sealed class RingRuntimeBindingPlayerV1 : MonoBehaviour
    {
        const string Flag="--sfm-ring-runtime-binding-v1";
        const string DevicePrefix="SfmRingRuntimeBindingV1-";
        const string Product="SFMOriginalGameplayInputVisual-31014133f09b4dad99db9b29b82fe544";
        const string Profile=@"C:\Users\Administrator\AppData\LocalLow\SFMRecovery\SFMOriginalGameplayInputVisual-31014133f09b4dad99db9b29b82fe544";
        const string OutputRoot=@"C:\Users\Administrator\sfm-layerfs-dev\mission-menu-v1";
        const string Home="Assets/SfmOriginalGameplayV3/Apartment.unity";
        const string G="ExposureUnnoticed2.Scripts.InGame.";
        const string B="ExposureUnnoticed2.Scripts.Base.";
        const string P="ExposureUnnoticed2.Object3D.Player.Scripts.";
        const string R="ExposureUnnoticed2.ObjectUI.InGame.RingMenu.";
        const string UI="ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager";
        const BindingFlags Fields=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static|BindingFlags.DeclaredOnly;
        static readonly Assembly Game=typeof(global::ExposureUnnoticed2.Scripts.InGame.InGameManager).Assembly;
        static readonly string[] ReadFields={
            G+"InGameManager::Instance",G+"InGameManager::<InGameReferencer>k__BackingField",G+"InGameReferencer::<Player>k__BackingField",G+"InGameReferencer::<BaseCameraController>k__BackingField",
            G+"GameState::GameStateData",G+"GameState::OptionData",G+"GameStateData::CurrentRp",G+"GameStateData::ExposeExp",G+"GameStateData::SkillLevel",
            "ExposureUnnoticed2.ObjectUI.OptionMenu.OptionData::LongPressTimeScale10",
            G+"GameStateData::RingShortcutModelIndexCommon",G+"GameStateData::RingShortcutModelIndexSkill",
            P+"PlayerController::pca",P+"PlayerClassAccessor::<PlayerState>k__BackingField",P+"PlayerClassAccessor::<Transform>k__BackingField",
            P+"PlayerStateModel::CurrentAction",P+"PlayerStateModel::CurrentRank","ExposureUnnoticed2.Master.Action.MActionRecord::type",
            "ExposureUnnoticed2.Object3D.Camera.BaseCameraController::<IsLockCamera>k__BackingField",
            UI+"::Instance",UI+"::basePanelStack",UI+"::<IsForceDeActive>k__BackingField",UI+"::<IsOpenFade>k__BackingField",UI+"::<IsOpenSomeCancelInputUi>k__BackingField",UI+"::<isOpenSomeAllowInputUi>k__BackingField",
            B+"InputManager::playerInput",B+"InputManager::inputDict",B+"InputManager::oldInputDict",B+"InputManager::averageDeltaTime",B+"InputManager::SampleDeltaTime",
            R+"RingMenuParentView::<Instance>k__BackingField",R+"RingMenuParentView::<IsOpenRing>k__BackingField",R+"RingMenuParentView::IsClickToAction",
            R+"RingMenuParentView::openRingList",R+"RingMenuParentView::lockOpen",R+"RingMenuParentView::currentOpenType",R+"RingMenuParentView::currentOverrideType",R+"RingMenuParentView::<CurrentRingIndexCommon>k__BackingField",R+"RingMenuParentView::menuParent",
            R+"RingMenuView::parentView",R+"RingMenuView::ringIndex",R+"RingMenuView::itemView",R+"RingMenuView::currentSelectView",R+"RingMenuView::ringDirectRect",
            R+"RingItemView::<Model>k__BackingField",R+"RingItemView::<IsRelease>k__BackingField",R+"RingMenuItemModel::<Index>k__BackingField",R+"RingMenuItemModel::<RingItemType>k__BackingField",
            "ES3Settings::_defaults","ES3Settings::_location","ES3Settings::directory","ES3Internal.ES3IO::persistentDataPath"
        };
        [Serializable] public sealed class Pin {public string path,sha256;public long bytes;}
        [Serializable] public sealed class ProfilePin {public string root;public Pin[] files;}
        [Serializable] public sealed class Contract
        {public string schema,buildRoot,output,testProduct,testProfilePath,playerGameSHA,observerAssemblySHA,executableSHA;public bool launchReady;public Pin[] inputs,testSaves;public ProfilePin[] protectedProfiles;public int overallWallLimit;}
        [Serializable] public sealed class Snapshot
        {public string reason,scene,skills;public int frame,phase,action,shortcutCommon,shortcutSkill,ringPage,ringType,overrideType,viewCount,selectedModel=-1;public float time;public double rp;public int exp;public bool ringOpen,lockOpen,clickToAction,cameraLocked;public Vector3 position;}
        [Serializable] public sealed class Capture {public string role,path,sha256;public int frame;public long bytes;public bool ready;}
        [Serializable] public sealed class Pixel {public Vector2 uv;public Color color;}
        [Serializable] public sealed class MaterialState {public int id;public string name,shader,texture;public string[] keywords;}
        [Serializable] public sealed class Group {public string path;public int id;public float alpha;public bool active,enabled,ignoreParentGroups;}
        [Serializable] public sealed class ImageState
        {
            public string path,role,sprite,texture,textureFormat,graphicsFormat,textureRectIssue,readIssue,cpuSampling;
            public int imageId,spriteId,textureId,page,slot,textureWidth,textureHeight,spriteUvCount,canvasMaterialCount;
            public bool active,enabled,useSpriteMesh,textureReadable,canvasCull,canvasTransparentCull,textureRectKnown;
            public Rect imageRect,spriteRect,textureRect;public Vector2 spritePivot;public Vector3 imageScale;public Vector4 outerUV;
            public Vector2[] spriteUvs;public Color imageColor,canvasColor;public float canvasAlpha;
            public MaterialState assignedMaterial,renderingMaterial;public MaterialState[] canvasMaterials;public Group[] groups;public Pixel[] pixels;
        }
        [Serializable] public sealed class ViewState
        {public string path,parentPath;public int id,page,itemCount;public bool active,current;public List<ImageState> images=new List<ImageState>();}
        [Serializable] public sealed class InputSample
        {public string reason;public int frame,phase,raw,oldRaw,shortThreshold,releaseGate;public float longPressScale10,averageDelta,sampleDelta,thresholdFloat;public bool shortUpPredicate;}
        [Serializable] public sealed class Report
        {
            public string schema="RingRuntimeBindingPlayerObservation/v1",status="preflight",failure,testProduct,testProfilePath,playerGameSHA,observerAssemblySHA,executableSHA,ringBindingPath;
            public int phase,checks,phaseFrame,pressFrame,releaseFrame,heldDynamicUpdates,neutralDynamicUpdates,ringRawMax,totalViews,engineErrorCount;
            public float phaseTime;public bool actualPlayer,pressSeen,releaseSeen,ringOpened,eightCurrentSectorsObserved,ringClosed,shortcutChanged,liveEconomySkillsUnchanged;
            public bool flowAccepted,reportComplete,runtimeAccepted,visualAccepted,fullGameplayAccepted,sdkRestored,identityUnchanged,protectedProfilesUnchanged,testFilesUnchanged,testSaveChangeReviewRequired,inputReleaseObserved;
            public bool originalSchemePresent,originalNeverAutoSwitch,pairingRestored,autoSwitchRestored,viewsTruncated;
            public bool originalRawReleaseObserved,originalShortUpPredicateObserved,minimumHoldDrainAttempted,shortReleaseRisk;public int droppedInputSamples;
            public InputSample releaseGateSample;public List<InputSample> inputSamples=new List<InputSample>();
            public string originalScheme;public uint originalInputUserId;public int[] originalDeviceIds;
            public string limit="Input-driven binding capture only. No repair/visual or full gameplay acceptance. Natural shortcut/save changes are preserved, never restored by state writes.";
            public List<Snapshot> snapshots=new List<Snapshot>();public List<ViewState> views=new List<ViewState>();public List<Capture> captures=new List<Capture>();
            public List<string> readResolutions=new List<string>(),errors=new List<string>(),cleanupErrors=new List<string>();
            public List<Pin> testInventoryBefore=new List<Pin>(),finalTestFiles=new List<Pin>();
        }
        static Contract contract;static Report report;static bool Armed,outputOwned,finished,finishing,completing,backgroundCaptured,pairingCaptured,pairingWasCaptured;
        static bool originalRunInBackground;static InputSettings.BackgroundBehavior originalBackground;
        static InputControlScheme? originalUserScheme;static PlayerInput routedInput;static Keyboard keyboard;static Mouse mouse;static int keyboardId,mouseId,lastAfterFrame=-1;
        static Vector2 mousePoint;static DateTime deadline,finishingDeadline;static Key ringKey;
        static object manager,referencer,player,pca,state,gsd,ui,ring,baseCamera;static Transform playerTransform;
        static Snapshot before;static float stableSince=-1;static bool cleanupHoldPending;static int lastSampleRaw=int.MinValue,lastSamplePhase=-1;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Install()
        {
            if(!Environment.GetCommandLineArgs().Contains(Flag))return;
            report=new Report();try
            {
                Begin();var host=new GameObject("SfmRingRuntimeBindingV1-OwnedObserver");DontDestroyOnLoad(host);host.AddComponent<RingRuntimeBindingPlayerV1>();
                Application.logMessageReceived+=Log;InputSystem.onAfterUpdate+=AfterInput;
            }
            catch(Exception e){report.failure=e.ToString();report.status="preflight-refused";try{Write();}finally{Application.Quit(6);}}
        }
        void Update(){if(!Armed||finished)return;if(finishing)DrainAndComplete();else Poll();}
        void LateUpdate(){if(!Armed||finished||report.pressFrame==0)return;try{SampleInput("late",false);}catch(Exception e){Finish(e);}}
        void OnApplicationQuit(){if(!Armed||finished||completing)return;report.failure=(report.failure??"")+"; Unexpected Player quit";report.flowAccepted=false;Complete(false);}
        static void Need(bool value,string reason){if(report!=null)report.checks++;if(!value)throw new InvalidOperationException(reason);}
        static object Read(object value,string name)
        {
            if(value==null)return null;
            for(Type t=value is Type?(Type)value:value.GetType();t!=null;t=t.BaseType)
            {
                var f=t.GetField(name,Fields)??t.GetField("<"+name+">k__BackingField",Fields);if(f==null)continue;
                string id=f.DeclaringType.FullName+"::"+f.Name;Need(ReadFields.Contains(id),"Reviewed Game field only: "+id);
                if(!report.readResolutions.Contains(id))report.readResolutions.Add(id);return f.GetValue(value is Type?null:value);
            }
            throw new MissingFieldException((value is Type?(Type)value:value.GetType()).FullName,name);
        }
        static object Static(string type,string name)=>Read(Game.GetType(type,true),name);
        static int I(object v){Need(v!=null,"Known integer/enum");return Convert.ToInt32(v);}
        static bool Z(object v){Need(v is bool,"Known boolean");return (bool)v;}
        static bool Alive(object v)=>v!=null&&(!(v is UnityEngine.Object)||(UnityEngine.Object)v);
        static string ObjectPath(Transform t)=>t==null?"":t.parent==null?t.name:ObjectPath(t.parent)+"/"+t.name;
        static string Hash(string p){using(var h=SHA256.Create())using(var f=File.OpenRead(p))return BitConverter.ToString(h.ComputeHash(f)).Replace("-","");}
        static Pin PinFile(string p)=>new Pin{path=Path.GetFullPath(p),bytes=new FileInfo(p).Length,sha256=Hash(p)};
        static void CheckPin(Pin p){Need(p!=null&&File.Exists(p.path)&&new FileInfo(p.path).Length==p.bytes&&Hash(p.path)==p.sha256,"Exact opaque/provenance pin: "+(p==null?"null":p.path));}
        static bool Inside(string p,string root)=>Path.GetFullPath(p).StartsWith(Path.GetFullPath(root).TrimEnd('\\','/')+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase);
        static void CheckTree(string root)
        {
            Need(Directory.Exists(root),"Existing profile tree");
            foreach(var d in new[]{root}.Concat(Directory.GetDirectories(root,"*",SearchOption.AllDirectories)))Need((File.GetAttributes(d)&FileAttributes.ReparsePoint)==0,"No directory reparse point");
            foreach(var f in Directory.GetFiles(root,"*",SearchOption.AllDirectories))Need((File.GetAttributes(f)&FileAttributes.ReparsePoint)==0,"No file reparse point");
        }
        static Pin[] TreePins(string root){CheckTree(root);var files=Directory.GetFiles(root,"*",SearchOption.AllDirectories);Need(files.Length<=1024,"Bounded complete opaque inventory");return files.OrderBy(x=>x,StringComparer.OrdinalIgnoreCase).Select(PinFile).ToArray();}
        static bool SamePins(IEnumerable<Pin> a,IEnumerable<Pin> b){var aa=a.ToArray();var bb=b.ToArray();return aa.Length==bb.Length&&aa.All(x=>bb.Any(y=>string.Equals(Path.GetFullPath(x.path),Path.GetFullPath(y.path),StringComparison.OrdinalIgnoreCase)&&x.bytes==y.bytes&&x.sha256==y.sha256));}
        static void CheckProtected(){foreach(var p in contract.protectedProfiles)Need(SamePins(TreePins(p.root),p.files),"Complete protected profile inventory unchanged: "+p.root);}
        static void Begin()
        {
            Need(!Application.isEditor&&!Armed,"Standalone only");var args=Environment.GetCommandLineArgs();int at=Array.IndexOf(args,Flag);Need(at>=0&&at+2<args.Length,"Explicit sealed contract path/hash");
            string cp=Path.GetFullPath(args[at+1]);Need(Hash(cp)==args[at+2],"Exact runtime contract bytes");contract=JsonUtility.FromJson<Contract>(File.ReadAllText(cp));var c=contract;
            Need(c.schema=="RingRuntimeBindingPlayer/v1"&&c.launchReady&&c.overallWallLimit==120,"Reviewed bounded contract");
            Need(c.testProduct==Product&&Path.GetFullPath(c.testProfilePath)==Path.GetFullPath(Profile),"Continue existing profile31 only");
            Need(Application.companyName=="SFMRecovery"&&Application.productName==Product&&Path.GetFullPath(Application.persistentDataPath)==Path.GetFullPath(Profile),"Baked identity, no argument redirect");
            Need(args.Contains("--sfm-manual-preview"),"Existing original bootstrap");foreach(var f in args.Where(x=>x.StartsWith("--sfm",StringComparison.Ordinal)||x.StartsWith("-sfm",StringComparison.Ordinal)))Need(f==Flag||f=="--sfm-manual-preview","No competing diagnostic: "+f);
            Need(Path.GetDirectoryName(Path.GetFullPath(c.buildRoot))==OutputRoot,"One-slot build direct child");string exe=System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;
            Need(Path.GetDirectoryName(Path.GetFullPath(exe))==Path.GetFullPath(c.buildRoot)&&Inside(Application.dataPath,c.buildRoot)&&Inside(Game.Location,Path.Combine(Application.dataPath,"Managed")),"Actual process/Game inside sealed build");
            report.playerGameSHA=Hash(Game.Location);report.observerAssemblySHA=Hash(typeof(RingRuntimeBindingPlayerV1).Assembly.Location);report.executableSHA=Hash(exe);
            Need(report.playerGameSHA==c.playerGameSHA&&report.observerAssemblySHA==c.observerAssemblySHA&&report.executableSHA==c.executableSHA,"Actual executable/Game/observer pins");
            Need(c.inputs!=null&&c.inputs.Length>0&&c.inputs.Length<=64,"Bounded reviewed provenance");foreach(var p in c.inputs)CheckPin(p);
            var expected=new[]{"SaveData/0.sd","SaveData/s.sd","SaveDataBackup/0.sd"}.Select(x=>Path.GetFullPath(Path.Combine(Profile,x))).ToArray();
            Need(c.testSaves!=null&&c.testSaves.Length==3&&c.testSaves.Select(x=>Path.GetFullPath(x.path)).Distinct(StringComparer.OrdinalIgnoreCase).Count()==3&&c.testSaves.All(x=>expected.Contains(Path.GetFullPath(x.path))),"Exact existing opaque test files");foreach(var p in c.testSaves)CheckPin(p);
            report.testInventoryBefore=TreePins(Profile).ToList();Need(c.protectedProfiles!=null&&c.protectedProfiles.Length>0&&c.protectedProfiles.Length<=32,"Parent complete protected inventories");
            foreach(var p in c.protectedProfiles){Need(p.files!=null&&p.files.Length<=1024&&!Inside(p.root,Profile)&&!Inside(Profile,p.root)&&Path.GetFullPath(p.root)!=Path.GetFullPath(Profile),"Separate protected root");Need(p.files.All(x=>Inside(x.path,p.root)),"Protected file paths");}CheckProtected();
            Need(Path.GetDirectoryName(Path.GetFullPath(c.output))==OutputRoot&&!Directory.Exists(c.output)&&!File.Exists(c.output)&&Path.GetFullPath(c.output)!=Path.GetFullPath(c.buildRoot),"Fresh output direct child distinct from Player");Directory.CreateDirectory(c.output);outputOwned=true;
            report.testProduct=Product;report.testProfilePath=Profile;report.actualPlayer=true;report.status="running";deadline=DateTime.UtcNow.AddSeconds(c.overallWallLimit);Armed=true;Advance(0);Write();
        }
        static void Write(){if(outputOwned&&contract!=null)File.WriteAllText(Path.Combine(contract.output,"observation.json"),JsonUtility.ToJson(report,true)+"\n");}
        static PlayerInput PI()=>Static(B+"InputManager","playerInput")as PlayerInput;
        static bool Bind()
        {
            manager=Static(G+"InGameManager","Instance");if(!Alive(manager))return false;referencer=Read(manager,"InGameReferencer");if(referencer==null)return false;
            player=Read(referencer,"Player");if(!Alive(player))return false;pca=Read(player,"pca");state=Read(pca,"PlayerState");gsd=Static(G+"GameState","GameStateData");ui=Static(UI,"Instance");ring=Static(R+"RingMenuParentView","Instance");baseCamera=Read(referencer,"BaseCameraController");playerTransform=Read(pca,"Transform")as Transform;
            return pca!=null&&state!=null&&gsd!=null&&Alive(ui)&&Alive(ring)&&Alive(baseCamera)&&playerTransform&&Read(state,"CurrentRank")!=null&&PI()&&PI().currentActionMap!=null&&Read(ui,"basePanelStack")!=null;
        }
        static bool HomeReady()=>SceneManager.GetActiveScene().path==Home&&Time.timeScale>0&&((IList)Read(ui,"basePanelStack")).Count==0&&!Z(Read(ui,"IsForceDeActive"))&&!Z(Read(ui,"IsOpenFade"))&&!Z(Read(ui,"IsOpenSomeCancelInputUi"))&&!Z(Read(ui,"isOpenSomeAllowInputUi"));
        static void SaveIsolation(){var settings=Static("ES3Settings","_defaults");var cached=Static("ES3Internal.ES3IO","persistentDataPath")as string;Need(settings!=null&&I(Read(settings,"_location"))==0&&I(Read(settings,"directory"))==0&&cached!=null&&Path.GetFullPath(cached)==Path.GetFullPath(Profile),"Original save backend points only at existing isolated profile");}
        static void SetupSDK()
        {
            if(!backgroundCaptured){originalBackground=InputSystem.settings.backgroundBehavior;originalRunInBackground=Application.runInBackground;backgroundCaptured=true;InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;Application.runInBackground=true;mousePoint=Mouse.current==null?Vector2.zero:Mouse.current.position.ReadValue();}
            if(keyboard==null){keyboard=InputSystem.AddDevice<Keyboard>(DevicePrefix+"Keyboard");keyboardId=keyboard.deviceId;}if(mouse==null){mouse=InputSystem.AddDevice<Mouse>(DevicePrefix+"Mouse");mouseId=mouse.deviceId;}
            var pi=PI();if(pi&&!pairingCaptured){Need(pi.user.valid&&pi.actions.controlSchemes.Any(x=>x.name=="Keyboard and Mouse"),"Original SDK user/scheme");routedInput=pi;originalUserScheme=pi.user.controlScheme;report.originalInputUserId=pi.user.id;report.originalSchemePresent=originalUserScheme.HasValue;report.originalScheme=originalUserScheme.HasValue?originalUserScheme.Value.name:null;report.originalDeviceIds=pi.user.pairedDevices.Select(x=>x.deviceId).ToArray();report.originalNeverAutoSwitch=pi.neverAutoSwitchControlSchemes;pairingCaptured=true;pairingWasCaptured=true;pi.neverAutoSwitchControlSchemes=true;pi.SwitchCurrentControlScheme("Keyboard and Mouse",keyboard,mouse);}
            if(pairingCaptured)Need(pi==routedInput&&pi&&pi.neverAutoSwitchControlSchemes&&pi.user.valid&&pi.user.id==report.originalInputUserId&&pi.user.pairedDevices.Count==2&&pi.user.pairedDevices.Any(x=>x.deviceId==keyboardId)&&pi.user.pairedDevices.Any(x=>x.deviceId==mouseId),"Exclusive owned SDK pair on same original user");
        }
        static void Queue(bool hold){if(keyboard!=null)InputSystem.QueueStateEvent(keyboard,new KeyboardState(hold?new[]{ringKey}:new Key[0]));if(mouse!=null)InputSystem.QueueStateEvent(mouse,new MouseState{position=mousePoint,delta=Vector2.zero,scroll=Vector2.zero});}
        static int RawRing(string field="inputDict"){var d=Static(B+"InputManager",field)as IDictionary;Need(d!=null,"Original raw input map");foreach(DictionaryEntry e in d)if(I(e.Key)==14)return I(e.Value);throw new InvalidOperationException("Missing Ring1 raw14");}
        static InputSample SampleInput(string reason,bool force)
        {
            var option=Static(G+"GameState","OptionData");float scale=Convert.ToSingle(Read(option,"LongPressTimeScale10")),avg=Convert.ToSingle(Static(B+"InputManager","averageDeltaTime")),sample=Convert.ToSingle(Static(B+"InputManager","SampleDeltaTime"));
            Need(avg>0&&!float.IsNaN(avg)&&!float.IsInfinity(avg)&&scale>0,"Valid original long-press timing fields");float threshold=scale*.1f*20*sample/avg;threshold=threshold<1f?1f:threshold;
            int raw=RawRing(),old=RawRing("oldInputDict");var s=new InputSample{reason=reason,frame=Time.frameCount,phase=report.phase,raw=raw,oldRaw=old,longPressScale10=scale,averageDelta=avg,sampleDelta=sample,thresholdFloat=threshold,shortThreshold=(int)threshold,releaseGate=Math.Max(32,(int)Math.Ceiling(threshold)+12)};
            s.shortUpPredicate=old>0&&old<s.shortThreshold&&raw==-1;report.ringRawMax=Math.Max(report.ringRawMax,raw);report.originalRawReleaseObserved|=report.releaseFrame>0&&raw==-1;report.originalShortUpPredicateObserved|=s.shortUpPredicate;
            bool important=force||raw==-1||lastSamplePhase!=report.phase||Math.Sign(raw)!=Math.Sign(lastSampleRaw);if(important||Time.frameCount%10==0){if(report.inputSamples.Count<510||important){Need(report.inputSamples.Count<540,"Bounded input transition record");report.inputSamples.Add(s);}else report.droppedInputSamples++;}lastSampleRaw=raw;lastSamplePhase=report.phase;return s;
        }
        static bool HoldSufficient(){var s=SampleInput("hold-gate",false);return s.raw>=s.releaseGate&&report.heldDynamicUpdates>=30;}
        static Key RingKey()
        {
            var a=PI().currentActionMap.FindAction("CommonRingMenu",true);Need(a.enabled,"Original Ring1 enabled");var paths=a.bindings.Where(x=>!x.isComposite&&!x.isPartOfComposite&&x.effectivePath!=null&&x.effectivePath.StartsWith("<Keyboard>/",StringComparison.Ordinal)).ToArray();Need(paths.Length==1,"One effective Ring1 keyboard binding");var k=keyboard.TryGetChildControl<UnityEngine.InputSystem.Controls.KeyControl>(paths[0].effectivePath.Substring(11));Need(k!=null&&k.keyCode==Key.Digit1,"Reviewed original numeric1 Ring1 binding, no override");report.ringBindingPath=paths[0].effectivePath;return k.keyCode;
        }
        static void AfterInput()
        {
            if(!Armed||finished||!Application.isPlaying||InputState.currentUpdateType!=InputUpdateType.Dynamic||lastAfterFrame==Time.frameCount)return;lastAfterFrame=Time.frameCount;
            try{if(keyboard==null||mouse==null)return;bool neutral=!keyboard.anyKey.isPressed&&!mouse.leftButton.isPressed&&!mouse.rightButton.isPressed&&!mouse.middleButton.isPressed&&mouse.delta.ReadValue()==Vector2.zero&&mouse.scroll.ReadValue()==Vector2.zero;
                report.neutralDynamicUpdates=neutral?report.neutralDynamicUpdates+1:0;
                var a=PI().currentActionMap.FindAction("CommonRingMenu",true);if(report.pressFrame>0&&a.enabled){bool held=keyboard[ringKey].isPressed&&a.ReadValue<float>()>.5f;report.pressSeen|=held;if(held)report.heldDynamicUpdates++;report.releaseSeen|=report.releaseFrame>0&&neutral&&a.ReadValue<float>()==0f;SampleInput("after-input",false);}if(finishing)report.inputReleaseObserved=!cleanupHoldPending&&report.neutralDynamicUpdates>=2;}
            catch(Exception e){Finish(e);}
        }
        static string Skills(){var d=Read(gsd,"SkillLevel")as IDictionary;Need(d!=null,"Live skill dictionary");var rows=new List<string>();foreach(DictionaryEntry e in d)rows.Add(I(e.Key)+":"+I(e.Value));rows.Sort(StringComparer.Ordinal);return string.Join(";",rows);}
        static Snapshot SnapshotNow(string reason)
        {
            var views=Read(ring,"openRingList")as IList;var s=new Snapshot{reason=reason,scene=SceneManager.GetActiveScene().path,frame=Time.frameCount,phase=report.phase,time=Time.unscaledTime,rp=Convert.ToDouble(Read(gsd,"CurrentRp")),exp=I(Read(gsd,"ExposeExp")),skills=Skills(),action=I(Read(Read(state,"CurrentAction"),"type")),shortcutCommon=I(Read(gsd,"RingShortcutModelIndexCommon")),shortcutSkill=I(Read(gsd,"RingShortcutModelIndexSkill")),ringOpen=Z(Read(ring,"IsOpenRing")),lockOpen=Z(Read(ring,"lockOpen")),clickToAction=Z(Read(ring,"IsClickToAction")),cameraLocked=Z(Read(baseCamera,"IsLockCamera")),ringPage=I(Read(ring,"CurrentRingIndexCommon")),ringType=I(Read(ring,"currentOpenType")),overrideType=I(Read(ring,"currentOverrideType")),viewCount=views==null?0:views.Count,position=playerTransform.position};
            if(views!=null)foreach(var v in views)if(Alive(v)&&I(Read(v,"ringIndex"))==s.ringPage){var selected=Read(v,"currentSelectView");if(Alive(selected)){var model=Read(selected,"Model");if(model!=null)s.selectedModel=I(Read(model,"Index"));}}
            Need(report.snapshots.Count<16,"Bounded snapshots");report.snapshots.Add(s);return s;
        }
        static MaterialState MaterialInfo(Material m)=>m?new MaterialState{id=m.GetInstanceID(),name=m.name,shader=m.shader?m.shader.name:null,texture=m.mainTexture?m.mainTexture.name:null,keywords=m.shaderKeywords}:null;
        static ImageState ImageInfo(Image im,int page,int slot,string role)
        {
            var r=new ImageState{page=page,slot=slot,role=role};try
            {
                Need(im,"Actual Image exists on original object");r.path=ObjectPath(im.transform);r.imageId=im.GetInstanceID();r.active=im.gameObject.activeInHierarchy;r.enabled=im.enabled;r.imageRect=im.rectTransform.rect;r.imageScale=im.transform.lossyScale;r.imageColor=im.color;r.useSpriteMesh=im.useSpriteMesh;
                r.assignedMaterial=MaterialInfo(im.material);r.renderingMaterial=MaterialInfo(im.materialForRendering);var cr=im.canvasRenderer;r.canvasCull=cr.cull;r.canvasTransparentCull=cr.cullTransparentMesh;r.canvasAlpha=cr.GetAlpha();r.canvasColor=cr.GetColor();r.canvasMaterialCount=cr.materialCount;r.canvasMaterials=Enumerable.Range(0,Math.Min(4,cr.materialCount)).Select(i=>MaterialInfo(cr.GetMaterial(i))).ToArray();
                r.groups=im.GetComponentsInParent<CanvasGroup>(true).Take(16).Select(g=>new Group{path=ObjectPath(g.transform),id=g.GetInstanceID(),alpha=g.alpha,active=g.gameObject.activeInHierarchy,enabled=g.enabled,ignoreParentGroups=g.ignoreParentGroups}).ToArray();
                var sp=im.sprite;if(!sp){r.cpuSampling="unknown: null Image.sprite";return r;}r.sprite=sp.name;r.spriteId=sp.GetInstanceID();r.spriteRect=sp.rect;r.spritePivot=sp.pivot;r.outerUV=UnityEngine.Sprites.DataUtility.GetOuterUV(sp);var uv=sp.uv;r.spriteUvCount=uv.Length;r.spriteUvs=uv.Take(8).ToArray();try{r.textureRect=sp.textureRect;r.textureRectKnown=true;}catch(Exception e){r.textureRectIssue=e.GetType().Name+": "+e.Message;}
                var tx=sp.texture;if(!tx){r.cpuSampling="unknown: null Sprite.texture";return r;}r.texture=tx.name;r.textureId=tx.GetInstanceID();r.textureWidth=tx.width;r.textureHeight=tx.height;r.textureFormat=tx.format.ToString();r.graphicsFormat=tx.graphicsFormat.ToString();r.textureReadable=tx.isReadable;
                if(!tx.isReadable){r.cpuSampling="unknown: existing Texture2D is not CPU readable; no import or readback change";return r;}
                var samples=new List<Pixel>();foreach(float fy in new[]{.01f,.5f,.99f})foreach(float fx in new[]{.01f,.5f,.99f}){float x=Mathf.Lerp(r.outerUV.x,r.outerUV.z,fx),y=Mathf.Lerp(r.outerUV.y,r.outerUV.w,fy);samples.Add(new Pixel{uv=new Vector2(x,y),color=tx.GetPixelBilinear(x,y)});}r.pixels=samples.ToArray();r.cpuSampling="read existing CPU texture: nine outerUV-relative samples";
            }
            catch(Exception e){r.readIssue=e.GetType().Name+": "+e.Message;r.cpuSampling="unknown after read failure";}return r;
        }
        static bool HaveCurrentEight()
        {
            var list=Read(ring,"openRingList")as IList;if(list==null)return false;int page=I(Read(ring,"CurrentRingIndexCommon"));
            var current=list.Cast<object>().Where(v=>Alive(v)&&I(Read(v,"ringIndex"))==page).ToArray();if(current.Length!=1)return false;var items=Read(current[0],"itemView")as IList;return items!=null&&items.Count==8&&items.Cast<object>().All(v=>Alive(v)&&v is Component&&((Component)v).gameObject.activeInHierarchy);
        }
        static void ObserveBindings()
        {
            var list=Read(ring,"openRingList")as IList;Need(list!=null,"Original live openRingList");report.totalViews=list.Count;report.viewsTruncated=list.Count>8;int page=I(Read(ring,"CurrentRingIndexCommon"));var parent=Read(ring,"menuParent")as Transform;Need(parent,"Original dynamic menu parent");
            foreach(var obj in list.Cast<object>().OrderBy(v=>I(Read(v,"ringIndex"))==page?0:1).Take(8))
            {
                var view=obj as Component;Need(view&&view.GetType().FullName==R+"RingMenuView"&&ReferenceEquals(Read(view,"parentView"),ring)&&view.transform.IsChildOf(parent),"Actual current RingMenuView hierarchy and original parent reference");var items=Read(view,"itemView")as IList;Need(items!=null&&items.Count==8,"Exactly eight original itemView entries");
                var v=new ViewState{path=ObjectPath(view.transform),parentPath=ObjectPath(parent),id=view.GetInstanceID(),page=I(Read(view,"ringIndex")),itemCount=items.Count,active=view.gameObject.activeInHierarchy};v.current=v.page==page;
                for(int i=0;i<items.Count;i++){var item=items[i]as Component;Need(item&&item.GetType().FullName==R+"RingItemView"&&item.transform.IsChildOf(view.transform),"Original RingItemView belongs to exact live page");var image=item.GetComponent<Image>();Need(image,"Actual sector Image, never static label fallback");v.images.Add(ImageInfo(image,v.page,i,"sector"));}
                var arrow=Read(view,"ringDirectRect")as RectTransform;if(arrow&&arrow.IsChildOf(view.transform)){var image=arrow.GetComponent<Image>();if(image)v.images.Add(ImageInfo(image,v.page,-1,"direction"));}report.views.Add(v);
            }
            var actual=report.views.Where(x=>x.current).ToArray();report.eightCurrentSectorsObserved=actual.Length==1&&actual[0].images.Count(x=>x.role=="sector")==8;Need(report.eightCurrentSectorsObserved,"Eight real current dynamic sectors captured");
        }
        static void CaptureFrame(string role){Need(report.captures.Count<3&&!report.captures.Any(x=>x.role==role),"At most three distinct screenshots");var c=new Capture{role=role,path=Path.Combine(contract.output,role+".png"),frame=Time.frameCount};report.captures.Add(c);ScreenCapture.CaptureScreenshot(c.path,1);}
        static bool CapturesReady(){foreach(var c in report.captures)if(!c.ready&&File.Exists(c.path)&&new FileInfo(c.path).Length>100){c.bytes=new FileInfo(c.path).Length;c.sha256=Hash(c.path);c.ready=true;}return report.captures.Count==3&&report.captures.All(x=>x.ready);}
        static void Advance(int phase){report.phase=phase;report.phaseFrame=Time.frameCount;report.phaseTime=Time.unscaledTime;stableSince=-1;}
        static void Poll()
        {
            try
            {
                Need(DateTime.UtcNow<deadline&&Time.frameCount<60000,"Overall 120-second bounded observer");Need(report.engineErrorCount==0,"No swallowed engine error");
                if(!Bind()){Need(report.phase==0,"Original Home objects persist during observation");stableSince=-1;return;}
                if(report.phase==0){if(!HomeReady()){stableSince=-1;return;}SaveIsolation();SetupSDK();ringKey=RingKey();Queue(false);if(stableSince<0)stableSince=Time.unscaledTime;if(Time.unscaledTime-stableSince<1f||report.neutralDynamicUpdates<2)return;
                    before=SnapshotNow("before-ring");Need(before.action==-1&&!before.ringOpen&&!before.lockOpen&&!before.clickToAction&&before.overrideType==0,"Idle original Home, normal Common wheel, no action override");CaptureFrame("before-ring");Advance(1);Write();return;}
                Need(HomeReady(),"Remain at original Home with no other UI/fade");SetupSDK();Need(Time.unscaledTime-report.phaseTime<20f,"Each ring phase under20seconds");
                switch(report.phase)
                {
                    case 1:
                        if(report.pressFrame==0)report.pressFrame=Time.frameCount;Queue(true);
                        if(Z(Read(ring,"IsOpenRing"))&&HaveCurrentEight()&&report.pressSeen&&HoldSufficient()){report.ringOpened=true;if(stableSince<0)stableSince=Time.unscaledTime;if(Time.unscaledTime-stableSince>=.75f){var opened=SnapshotNow("ring-held");Need(opened.ringType==0&&!opened.clickToAction&&opened.action==-1,"Original Common wheel; no selected action invocation");ObserveBindings();CaptureFrame("ring-held-eight-sectors");Advance(2);Write();}}
                        break;
                    case 2:
                        Queue(true);if(Time.unscaledTime-report.phaseTime>=.5f&&report.captures.Any(x=>x.role=="ring-held-eight-sectors"&&File.Exists(x.path)&&new FileInfo(x.path).Length>100)&&HoldSufficient()){report.releaseGateSample=SampleInput("pre-release",true);report.releaseFrame=Time.frameCount;Queue(false);Advance(3);Write();}break;
                    case 3:
                        Queue(false);var list=Read(ring,"openRingList")as IList;
                        if(!Z(Read(ring,"IsOpenRing"))&&!Z(Read(ring,"lockOpen"))&&list!=null&&list.Count==0&&report.releaseSeen&&report.neutralDynamicUpdates>=2){var closed=SnapshotNow("ring-closed");Need(closed.action==-1&&!closed.clickToAction&&closed.cameraLocked==before.cameraLocked,"Normal close restored camera, did not execute pose/action");report.ringClosed=true;report.shortcutChanged=closed.shortcutCommon!=before.shortcutCommon;report.liveEconomySkillsUnchanged=closed.rp==before.rp&&closed.exp==before.exp&&closed.skills==before.skills&&closed.shortcutSkill==before.shortcutSkill;Need(report.liveEconomySkillsUnchanged,"No RP/exp/skills/skill-shortcut change");CaptureFrame("after-ring-closed");Advance(4);Write();}break;
                    case 4:
                        Queue(false);Need(!Z(Read(ring,"IsOpenRing"))&&I(Read(Read(state,"CurrentAction"),"type"))==-1,"No delayed action after release");if(Time.unscaledTime-report.phaseTime>=1f&&CapturesReady()){Need(report.originalRawReleaseObserved&&!report.originalShortUpPredicateObserved&&!report.shortReleaseRisk,"Actual raw release observed outside original short-up predicate");report.flowAccepted=true;report.status="binding-captured-pending-external-review";Finish(null);}break;
                }
            }
            catch(Exception e){Finish(e);}
        }
        static void Log(string message,string stack,LogType type){if(!Armed||finished||type!=LogType.Exception&&type!=LogType.Error&&type!=LogType.Assert)return;report.engineErrorCount++;string row=type+": "+message+"\n"+stack;if(report.errors.Count<16&&!report.errors.Contains(row))report.errors.Add(row);}
        static void CleanupStep(string name,Action f){try{f();}catch(Exception e){report.flowAccepted=false;report.cleanupErrors.Add(name+": "+e);}}
        static void RestorePairing()
        {
            if(!pairingCaptured)return;try{CleanupStep("original-pairing",()=>{Need(routedInput,"Original input survives cleanup");var devices=report.originalDeviceIds.Select(InputSystem.GetDeviceById).ToArray();Need(devices.All(x=>x!=null&&x.added),"Original SDK devices exist");var user=routedInput.user;Need(user.valid&&user.id==report.originalInputUserId,"Same original InputUser");user.UnpairDevices();foreach(var d in devices)InputUser.PerformPairingWithDevice(d,user:user);user.ActivateControlScheme(originalUserScheme??default(InputControlScheme));var restored=routedInput.user.controlScheme;report.pairingRestored=routedInput.user.valid&&routedInput.user.id==report.originalInputUserId&&restored.HasValue==report.originalSchemePresent&&(!restored.HasValue||restored.Value.Equals(originalUserScheme.Value))&&routedInput.user.pairedDevices.Select(x=>x.deviceId).OrderBy(x=>x).SequenceEqual(report.originalDeviceIds.OrderBy(x=>x));Need(report.pairingRestored,"Exact nullable scheme/device pairing restored");});}
            finally{CleanupStep("original-auto-switch",()=>{Need(routedInput,"Original SDK object");routedInput.neverAutoSwitchControlSchemes=report.originalNeverAutoSwitch;report.autoSwitchRestored=routedInput.neverAutoSwitchControlSchemes==report.originalNeverAutoSwitch;Need(report.autoSwitchRestored,"Auto-switch restored");});pairingCaptured=false;}
        }
        static void RemoveOwned(int id){var d=InputSystem.GetDeviceById(id);if(d==null)return;Need(d.name.StartsWith(DevicePrefix,StringComparison.Ordinal),"Only owned device removal");InputSystem.RemoveDevice(d);}
        static void Cleanup()
        {
            CleanupStep("restore-pairing",RestorePairing);CleanupStep("remove-keyboard",()=>RemoveOwned(keyboardId));CleanupStep("remove-mouse",()=>RemoveOwned(mouseId));
            CleanupStep("background",()=>{if(backgroundCaptured)InputSystem.settings.backgroundBehavior=originalBackground;});CleanupStep("run-in-background",()=>{if(backgroundCaptured)Application.runInBackground=originalRunInBackground;});
            CleanupStep("identity",()=>{report.identityUnchanged=Application.companyName=="SFMRecovery"&&Application.productName==Product&&Path.GetFullPath(Application.persistentDataPath)==Path.GetFullPath(Profile);Need(report.identityUnchanged,"Baked identity unchanged");});
            CleanupStep("protected-inventories",()=>{CheckProtected();report.protectedProfilesUnchanged=true;});
            CleanupStep("test-inventory",()=>{report.finalTestFiles=TreePins(Profile).ToList();report.testFilesUnchanged=SamePins(report.testInventoryBefore,report.finalTestFiles);if(!report.testFilesUnchanged){report.testSaveChangeReviewRequired=true;report.flowAccepted=false;report.cleanupErrors.Add("Natural test-profile change retained for save-cadence review; observer did not save or copy back");}});
            report.sdkRestored=(!backgroundCaptured||(InputSystem.settings.backgroundBehavior==originalBackground&&Application.runInBackground==originalRunInBackground))&&InputSystem.GetDeviceById(keyboardId)==null&&InputSystem.GetDeviceById(mouseId)==null&&(!pairingWasCaptured||(report.pairingRestored&&report.autoSwitchRestored));
            if(!report.sdkRestored||!report.identityUnchanged||!report.protectedProfilesUnchanged||report.engineErrorCount>0||report.cleanupErrors.Count>0)report.flowAccepted=false;
        }
        static void Finish(Exception error)
        {
            if(finished||finishing)return;finishing=true;report.neutralDynamicUpdates=0;if(error!=null){report.failure=error.ToString();report.flowAccepted=false;report.status="partial-evidence";}
            cleanupHoldPending=report.pressFrame>0&&report.releaseFrame==0&&keyboard!=null&&mouse!=null;report.minimumHoldDrainAttempted=cleanupHoldPending;finishingDeadline=DateTime.UtcNow.AddSeconds(cleanupHoldPending?3:2);
            CleanupStep("release-or-protect-existing-hold",()=>Queue(cleanupHoldPending));CleanupStep("partial-report",Write);
        }
        static void DrainAndComplete()
        {
            try
            {
                if(cleanupHoldPending)
                {
                    bool sufficient=false;try{sufficient=HoldSufficient();}catch(Exception e){if(report.cleanupErrors.Count<16)report.cleanupErrors.Add("minimum-hold-read: "+e.Message);}
                    if(sufficient||DateTime.UtcNow>=finishingDeadline){if(!sufficient){report.shortReleaseRisk=true;report.flowAccepted=false;report.cleanupErrors.Add("Original raw long-hold gate unavailable within3seconds; forced neutral cleanup may take original short-up branch. No state restoration claimed.");}else report.releaseGateSample=SampleInput("cleanup-pre-release",true);cleanupHoldPending=false;report.releaseFrame=Time.frameCount;report.neutralDynamicUpdates=0;finishingDeadline=DateTime.UtcNow.AddSeconds(2);Queue(false);}else Queue(true);return;
                }
                if(keyboard==null||mouse==null||report.inputReleaseObserved){Complete(true);return;}
                if(DateTime.UtcNow>=finishingDeadline){report.flowAccepted=false;report.cleanupErrors.Add("Neutral release not observed on two Dynamic updates before cleanup deadline");Complete(true);return;}Queue(false);
            }
            catch(Exception e){report.flowAccepted=false;report.cleanupErrors.Add(e.ToString());Complete(true);}
        }
        static void Complete(bool quit){if(completing||finished)return;completing=true;try{Cleanup();}catch(Exception e){report.flowAccepted=false;report.cleanupErrors.Add(e.ToString());}finally{InputSystem.onAfterUpdate-=AfterInput;Application.logMessageReceived-=Log;finished=true;Armed=false;report.reportComplete=true;report.runtimeAccepted=false;report.visualAccepted=false;report.fullGameplayAccepted=false;if(!report.flowAccepted)report.status="partial-evidence";try{Write();}finally{if(quit)Application.Quit(report.flowAccepted?0:6);}}}
    }
}
