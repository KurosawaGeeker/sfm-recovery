using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;

namespace SfmMvp
{
    // Owned QA V4 candidate. Same integration class/flag and timed v2 input contract; additive preview/routing telemetry with owned QA input checks. Not installed. No Game calls, Game event subscription or Game field writes.
    // The build must already have its own product identity; this does NOT redirect save paths.
    public sealed class HomePlayerInputVisualV1 : MonoBehaviour
    {
        public const string Prefix = "SFMOriginalGameplayInputVisual-";
        public const string Flag = "--sfm-home-input-visual";
        const string Root = @"C:\Users\Administrator\sfm-layerfs-dev\mission-menu-v1";
        const string PC = "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController";
        const string UI = "ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager";
        const string BC = "ExposureUnnoticed2.Object3D.Camera.BaseCameraController";
        [Serializable] public class Segment { public string name; public string[] keys; public int maxFrames; public float durationSeconds, mouseXPerSecond, mouseYPerSecond; public float[] screenshotSeconds; }
        [Serializable] public class Contract { public string schema, product, profile, output, playerGameSHA; public Segment[] segments; }
        [Serializable] public class Clip { public string name; public float weight; public string[] events; }
        [Serializable] public class Layer { public string name; public int layer, currentHash, nextHash; public float weight, currentTime, nextTime; public bool transition; public Clip[] current, next; }
        [Serializable] public class Cam
        {
            public string name, path, target, clearFlags, renderingPath, actualRenderingPath, cameraType, depthTextureMode;
            public int id, mask, targetId; public float depth, near, far; public Rect rect, pixelRect; public Vector3 position; public Quaternion rotation; public Color background;
            public bool allowHDR, allowMSAA, allowDynamicResolution, forceIntoRenderTexture, postProcessPresent, postProcessEnabled; public int antialiasingMode; public string antialiasingName;
            public Rt texture; public TransformRecord transform; public float fieldOfView, orthographicSize, aspect; public bool orthographic; public ViewportPoint headViewport, faceTargetViewport;
        }
        [Serializable] public class TransformRecord
        {
            public string role, name, path; public bool found; public int id, parentId;
            public Vector3 localPosition, worldPosition, localScale, lossyScale, rootRelativePosition, up, forward;
            public Quaternion localRotation, worldRotation, rootRelativeRotation;
        }
        [Serializable] public class ViewportPoint { public string anchor; public bool known, inFront, insideViewport; public Vector3 world, viewport; }
        [Serializable] public class FacePreview
        {
            public int id, cameraId; public bool enabled, isFaceCamera, playerMatchesRoot; public TransformRecord transform;
            public Vector3 offset, currentOffset; public float initRectHeight, offsetY, rectScale;
        }
        [Serializable] public class BodyPreview
        {
            public int id; public bool enabled; public TransformRecord transform;
            public Vector3 smallOffset, bigOffset, futomomoOffset, initPosition; public float a, b, c, d, e;
        }
        [Serializable] public class PreviewFraming
        {
            public bool avatarKnown, customDataKnown, heelOffsetKnown; public int avatarId;
            public int customizeHeight, customizeNeck, customizeThighsLength, customizeLowerLegLength, customizeUpperBodyLength; public float heelOffset;
            public TransformRecord[] anchors; public FacePreview[] faces; public BodyPreview[] bodies;
        }
        [Serializable] public class Rt { public string format, graphicsFormat, depthStencilFormat, dimension; public int width, height, depth, volumeDepth, antiAliasing; public bool sRGB, graphicsFormatHasAlpha, useDynamicScale; }
        [Serializable] public class Bone { public string humanoidName, path; public bool found; public int id, parentId; public Vector3 localPosition, worldPosition, rootRelativePosition; public Quaternion localRotation, worldRotation, rootRelativeRotation; }
        [Serializable] public class ClipIdentity { public int instanceId; public string name; public float length, frameRate; public bool humanMotion, legacy, looping; public string wrapMode; public ClipEvent[] events; }
        [Serializable] public class ClipEvent { public float time, floatParameter; public int intParameter; public string function, stringParameter; }
        [Serializable] public class ClipSet { public ClipIdentity[] clips; }
        [Serializable] public class AnimatorIdentity
        {
            public int animatorId, avatarId, controllerId; public string animatorName, path, avatarName, controllerName, clipMetadataSHA;
            public bool avatarPresent, avatarValid, avatarHuman, isHuman; public float humanScale; public string[] layerNames; public ClipIdentity[] controllerClips;
            public string limitation="Runtime instance IDs are not serialized pathIDs. The SHA describes sorted clip metadata without IDs, not clip curves, masks, controller bytes or unique resource identity; matching signatures may remain ambiguous.";
        }
        [Serializable] public class SegmentResult { public int sdkMoveFrames, decodedMoveFrames, sdkDashFrames, decodedDashFrames; public float sampledTravelXZ; public double longestZeroMoveSeconds; public bool movementRequested, movementAccepted; public string name; public double started, ended, elapsed; public float gameTimeStarted, gameTimeEnded; public int frames, samples, screenshots; public float requestedSeconds; }
        [Serializable] public class RenderEnvironment { public string pipeline, activeColorSpace, graphicsDevice, graphicsAPI; public int screenWidth, screenHeight; }
        [Serializable] public class Sample
        {
            public int frame, segmentFrame; public float time, dt, footTime, sourceRms, listenerRms, speed, cameraDistance;
            public double dsp, realtime, segmentElapsed, actualSampleSeconds, screenshotBeginRealtime, screenshotEndRealtime; public int sampleFrameGap, animatorIdentityIndex;
            public bool positionDeltaKnown, animatorEnabled, applyRootMotion; public Vector3 positionDelta, positionVelocity; public Quaternion rootRotation;
            public string animatorUpdateMode, animatorCullingMode; public float animatorSpeed, actualPlanarSpeed;
            public Bone[] bones; public string phase, requested, scene, screenshot, footAudioSourceWindow, mixedAudioWindow;
            public Vector2 sdkMove, sdkCamera; public Vector3 position, velocity, decodedMove, moveV, cameraPosition;
            public bool sdkDash, decodedDash, stateDash, isStrafe, autoRun, autoChase, sourcePlaying, sourceMute, grounded;
            public float sourceVolume, sourcePitch, listenerVolume; public string sourceAssignedClip;
            public Layer[] layers; public Cam[] cameras; public PreviewFraming previewFraming;
        }
        [Serializable] public class DeviceRecord { public int id; public string name, layout; public bool present, added, enabled, paired; }
        [Serializable] public class ActionRecord { public bool present, enabled; public string name, phase, activePath; public int activeDevice; public int[] controlDeviceIds; public Vector2 vector; public float scalar; }
        [Serializable] public class Routing
        {
            public int frame, segmentFrame, playerInputId, actionAssetId, keyboardCurrentId, mouseCurrentId; public double realtime;
            public string phase, requested, scheme, map, inputUpdateMode, inputUpdateType; public bool focused, playerInputEnabled, inputActive, mapEnabled, neverAutoSwitch;
            public int[] pairedIds; public string[] actualOwnedKeys; public Vector2 requestedMove, requestedMouseDelta, actualOwnedMouseDelta; public Vector3 decodedMove; public bool decodedDash;
            public DeviceRecord keyboard, mouse; public ActionRecord move, dash, camera, cancel;
        }
        DeviceRecord DeviceSnapshot(InputDevice d,int[] pairedIds)
        {
            return d==null?new DeviceRecord():new DeviceRecord{present=true,id=d.deviceId,name=d.name,layout=d.layout,added=d.added,enabled=d.enabled,paired=pairedIds.Contains(d.deviceId)};
        }
        ActionRecord ActionSnapshot(string name,bool vector)
        {
            var action=input&&input.currentActionMap!=null?input.currentActionMap.FindAction(name,false):null;
            if(action==null)return new ActionRecord{name=name};
            var controls=action.controls;Need(controls.Count<=64,"Bounded original action controls");
            return new ActionRecord{present=true,name=name,enabled=action.enabled,phase=action.phase.ToString(),activePath=action.activeControl?.path,activeDevice=action.activeControl?.device.deviceId??0,
                controlDeviceIds=controls.Select(c=>c.device.deviceId).Distinct().ToArray(),vector=vector?action.ReadValue<Vector2>():Vector2.zero,scalar=vector?0:action.ReadValue<float>()};
        }
        Routing RecordRouting(string phase,int segmentFrame)
        {
            Need(++report.routingFrames<=13000,"Bounded per-frame routing observations including startup");
            var ids=input?input.devices.Select(d=>d.deviceId).ToArray():new int[0];Need(ids.Length<=16,"Bounded paired SDK devices");
            var move=new Vector2((queuedKeys.Contains(Key.D)?1:0)-(queuedKeys.Contains(Key.A)?1:0),(queuedKeys.Contains(Key.W)?1:0)-(queuedKeys.Contains(Key.S)?1:0));
            var r=new Routing{frame=Time.frameCount,realtime=Time.realtimeSinceStartupAsDouble,segmentFrame=segmentFrame,phase=phase,requested=string.Join("+",queuedKeys.Select(k=>k.ToString())),requestedMove=move,requestedMouseDelta=queuedMouseDelta,actualOwnedMouseDelta=mouse!=null&&mouse.added?mouse.delta.ReadValue():Vector2.zero,
                focused=Application.isFocused,playerInputId=input?input.GetInstanceID():0,actionAssetId=input&&input.actions?input.actions.GetInstanceID():0,playerInputEnabled=input&&input.enabled,inputActive=input&&input.inputIsActive,
                scheme=input?input.currentControlScheme:null,map=input?input.currentActionMap?.name:null,mapEnabled=input&&input.currentActionMap!=null&&input.currentActionMap.enabled,neverAutoSwitch=input&&input.neverAutoSwitchControlSchemes,
                inputUpdateMode=InputSystem.settings.updateMode.ToString(),inputUpdateType=InputState.currentUpdateType.ToString(),pairedIds=ids,keyboard=DeviceSnapshot(keyboard,ids),mouse=DeviceSnapshot(mouse,ids),
                keyboardCurrentId=Keyboard.current?.deviceId??0,mouseCurrentId=Mouse.current?.deviceId??0,
                actualOwnedKeys=keyboard!=null&&keyboard.added?new[]{Key.W,Key.A,Key.S,Key.D,Key.LeftShift,Key.Escape}.Where(k=>keyboard[k].isPressed).Select(k=>k.ToString()).ToArray():new string[0],
                move=ActionSnapshot("Move",true),dash=ActionSnapshot("Dash",false),camera=ActionSnapshot("CameraMove",true),cancel=ActionSnapshot("Cancel",false),decodedMove=(Vector3)Read(decoded,"Move"),decodedDash=(bool)Read(decoded,"Dash")};
            routingTrace.WriteLine(JsonUtility.ToJson(r));return r;
        }
        void CheckMovementInput(SegmentResult result,Routing r,double now)
        {
            result.elapsed=now-result.started;Vector3 current=playerTransform.position;result.sampledTravelXZ+=new Vector2(current.x-segmentPreviousPosition.x,current.z-segmentPreviousPosition.z).magnitude;segmentPreviousPosition=current;
            if(r.move.vector.sqrMagnitude>0.000001f)result.sdkMoveFrames++;if(r.decodedMove.sqrMagnitude>0.000001f)result.decodedMoveFrames++;
            if(r.dash.scalar>.5f)result.sdkDashFrames++;if(r.decodedDash)result.decodedDashFrames++;
            if(r.requestedMove.sqrMagnitude<=0.000001f){zeroMoveSince=-1;return;}result.movementRequested=true;
            if(r.move.vector.sqrMagnitude<=0.000001f&&r.decodedMove.sqrMagnitude<=0.000001f)
            {
                if(zeroMoveSince<0)zeroMoveSince=now;double duration=now-zeroMoveSince;result.longestZeroMoveSeconds=Math.Max(result.longestZeroMoveSeconds,duration);
                Need(duration<.25,"Owned QA requested nonzero movement but original SDK and decoded Move stayed zero for >=0.25 seconds; inspect retained per-frame routing; root cause unknown");
            }
            else zeroMoveSince=-1;
        }
        [Serializable] public class Report
        {
            public string observationRevision = "owned-routing-v4";
            public string schema = "HomePlayerInputVisual/v4", status = "running", failure, product, profile, output, contractSHA;
            public int sampleCount, footTimestampChanges, audioWindows, rate; public bool completed, sdkRestored;
            public bool visualAccepted = false, physicalKeyboardTested = false, continuousAudioRecorded = false, completeGameplayAccepted = false;
            public string evidenceLimit = "SDK input compatibility only. Audio windows are discontinuous source/mixed channel-0 PCM snapshots, not system loopback. OneShot identity is not AudioSource.clip. Visual review and full gameplay remain separate.";
            public bool autoSwitchOriginal, autoSwitchTemporaryApplied, autoSwitchRestored, movementSequenceAccepted, sdkCleanupFailed; public List<string> cleanupAttempts=new List<string>(), cleanupErrors=new List<string>(); public int routingFrames, movementSegments; public SegmentResult inProgressSegment; public RenderEnvironment rendering; public List<AnimatorIdentity> animatorIdentities=new List<AnimatorIdentity>(); public List<SegmentResult> segments=new List<SegmentResult>();
            public List<string> errors = new List<string>(), bindings = new List<string>();
        }
        Contract contract; Report report = new Report(); StreamWriter trace, routingTrace; Keyboard keyboard; Mouse mouse; PlayerInput input;
        InputDevice[] oldDevices; string oldScheme; InputSettings.BackgroundBehavior oldBackground; bool oldRun, paired, done;
        bool autoSwitchCaptured, oldNeverAutoSwitch; Key[] queuedKeys=new Key[0]; Vector2 queuedMouseDelta; double zeroMoveSince=-1; Vector3 segmentPreviousPosition;
        double previousRealtime; Vector3 previousPosition; int previousFrame; bool hasPrevious;
        const double SampleInterval=1.0/30.0; // Input still queues every frame; bound retained observations to 30 Hz plus segment boundaries/screenshots.
        static readonly HumanBodyBones[] LegBones={HumanBodyBones.Hips,HumanBodyBones.LeftUpperLeg,HumanBodyBones.RightUpperLeg,HumanBodyBones.LeftLowerLeg,HumanBodyBones.RightLowerLeg,HumanBodyBones.LeftFoot,HumanBodyBones.RightFoot,HumanBodyBones.LeftToes,HumanBodyBones.RightToes};
        float begin, oldFoot = -1; int captureDue = -1; readonly float[] sourceBlock = new float[8192], mixBlock = new float[8192];
        object player, pca, state, ui, cameraController, sound, decoded; Transform playerTransform; CharacterController cc; Animator animator; AudioSource source;
        static void Need(bool v, string m) { if (!v) throw new InvalidOperationException(m); }
        static string Hash(string p) { using(var s=File.OpenRead(p)) using(var h=SHA256.Create()) return BitConverter.ToString(h.ComputeHash(s)).Replace("-", ""); }
        static object Read(object o, string n)
        {
            if(o == null) return null;
            for(Type t=o.GetType(); t!=null; t=t.BaseType)
            {
                const BindingFlags f=BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.DeclaredOnly;
                var field=t.GetField(n,f)??t.GetField("<"+n+">k__BackingField",f);
                if(field!=null) return field.GetValue(o);
            }
            throw new MissingFieldException(o.GetType().FullName,n);
        }
        static object Find(string name)
        {
            var type=AppDomain.CurrentDomain.GetAssemblies().Select(a=>a.GetType(name,false)).FirstOrDefault(t=>t!=null);
            Need(type!=null,"Required loaded type: "+name); return UnityEngine.Object.FindObjectOfType(type);
        }
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Install()
        {
            try { InstallChecked(); }
            catch(Exception e) { Debug.LogError("Home QA setup refused: "+e); Application.Quit(6); }
        }
        static void InstallChecked()
        {
            var args=Environment.GetCommandLineArgs(); int i=Array.IndexOf(args,Flag); if(i<0) return;
            Need(i+2<args.Length,"Contract path and SHA required");
            string path=Path.GetFullPath(args[i+1]); Need(Hash(path)==args[i+2],"Exact runtime contract hash");
            var c=JsonUtility.FromJson<Contract>(File.ReadAllText(path));
            Need(c.schema=="HomePlayerInputVisualContract/v2","Contract schema");
            Need(c.product.StartsWith(Prefix,StringComparison.Ordinal)&&c.product.Length==Prefix.Length+32&&Guid.TryParseExact(c.product.Substring(Prefix.Length),"N",out _),"Unique QA product");
            Need(Application.companyName=="SFMRecovery"&&Application.productName==c.product,"Baked QA identity required before scene startup");
            string profile=Path.GetFullPath(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),"AppData/LocalLow/SFMRecovery",c.product));
            Need(Path.GetFullPath(c.profile)==profile&&Path.GetFullPath(Application.persistentDataPath)==profile,"Exact isolated persistent-data root");
            Need(!Directory.Exists(Path.Combine(profile,"SaveData"))&&!Directory.Exists(Path.Combine(profile,"SaveDataBackup")),"Fresh natural profile only; no copied/replaced saves");
            Need(Hash(typeof(HomePlayerInputVisualV1).Assembly.Location)==c.playerGameSHA,"Actual built Game hash");
            string output=Path.GetFullPath(c.output);
            Need(Path.GetDirectoryName(output)==Root&&!Directory.Exists(output),"Fresh direct evidence child of the one-build workspace");
            Need(c.segments!=null&&c.segments.Length>=3&&c.segments.Length<=12&&c.segments.Sum(x=>x.maxFrames)<=12000&&c.segments.Sum(x=>x.durationSeconds)<=30,"Bounded original-input sequence");
            foreach(var s in c.segments)
            {
                Need(!string.IsNullOrEmpty(s.name)&&s.name.All(ch=>char.IsLetterOrDigit(ch)||ch=='-'),"Safe segment label");
                Need(s.maxFrames>=30&&s.maxFrames<=1500&&s.durationSeconds>=.1f&&s.durationSeconds<=3f&&!float.IsNaN(s.durationSeconds),"Real-time segment and finite frame guard");
                Need(Mathf.Abs(s.mouseXPerSecond)<=360&&Mathf.Abs(s.mouseYPerSecond)<=180,"Bounded SDK mouse rate");
                Need(s.screenshotSeconds!=null&&s.screenshotSeconds.Length<=3&&s.screenshotSeconds.All(t=>t>=0&&t<s.durationSeconds&&!float.IsNaN(t))&&s.screenshotSeconds.SequenceEqual(s.screenshotSeconds.OrderBy(t=>t).Distinct()),"Ordered passive screenshot times inside segment");
                Need((s.keys??new string[0]).All(k=>new[]{"W","A","S","D","LeftShift"}.Contains(k)),"Only ordinary movement and dash keys");
            }
            Need((c.segments.Last().keys??new string[0]).Length==0&&c.segments.Last().durationSeconds>=1.0f&&c.segments.Last().mouseXPerSecond==0&&c.segments.Last().mouseYPerSecond==0,"Observed neutral release tail");
            Directory.CreateDirectory(output); var go=new GameObject("HomePlayerInputVisualV1-owned"); DontDestroyOnLoad(go);
            var p=go.AddComponent<HomePlayerInputVisualV1>(); p.contract=c; p.begin=Time.realtimeSinceStartup;
            p.report.product=c.product; p.report.profile=profile; p.report.output=output; p.report.contractSHA=args[i+2]; p.report.rate=AudioSettings.outputSampleRate;
            p.trace=new StreamWriter(Path.Combine(output,"trace.jsonl")); p.routingTrace=new StreamWriter(Path.Combine(output,"input-routing.jsonl")); Application.logMessageReceived+=p.Log;
        }
        void Log(string m,string st,LogType t) { if(t==LogType.Error||t==LogType.Exception||t==LogType.Assert) report.errors.Add(m+"\n"+st); }
        void Update() { if(contract!=null&&!done&&Time.realtimeSinceStartup-begin>180) Finish(new TimeoutException("Owned 180-second watchdog; no gameplay acceptance")); }
        void OnApplicationQuit() { if(contract!=null&&!done)Finish(new OperationCanceledException("Player closed before sequence completion")); }
        IEnumerator Start()
        {
            var stack=new Stack<IEnumerator>(); stack.Push(Run());
            while(stack.Count>0&&!done)
            {
                object next=null; bool more=false; Exception error=null;
                try { more=stack.Peek().MoveNext(); if(more)next=stack.Peek().Current; } catch(Exception e) { error=e; }
                if(error!=null) { Finish(error); yield break; }
                if(!more) { (stack.Pop()as IDisposable)?.Dispose(); continue; }
                if(next is IEnumerator nested) { stack.Push(nested); continue; } yield return next;
            }
            if(!done) Finish(null);
        }
        bool BindObjects()
        {
            player=Find(PC); ui=Find(UI); cameraController=Find(BC); pca=Read(player,"pca");
            if(player==null||ui==null||cameraController==null||pca==null) return false;
            state=Read(pca,"PlayerState"); playerTransform=Read(pca,"Transform")as Transform; cc=Read(pca,"CharacterController")as CharacterController;
            animator=Read(pca,"Animator")as Animator; sound=Read(pca,"PlayerSoundManager"); source=Read(sound,"audioSource")as AudioSource;
            decoded=Read(Read(player,"inputResolver"),"inputData");
            return state!=null&&playerTransform&&cc&&animator&&source&&decoded!=null;
        }
        bool ClearUi() => !(bool)Read(ui,"IsOpenFade")&&!(bool)Read(ui,"isOpenSomeAllowInputUi")&&!(bool)Read(ui,"IsOpenSomeCancelInputUi")&&((IList)Read(ui,"basePanelStack")).Count==0&&Time.timeScale>0;
        void Setup()
        {
            input=PlayerInput.all.Single(p=>p.currentActionMap!=null&&p.currentActionMap.FindAction("Move",false)!=null);
            oldDevices=input.devices.ToArray(); oldScheme=input.currentControlScheme; oldBackground=InputSystem.settings.backgroundBehavior; oldRun=Application.runInBackground; paired=true;
            oldNeverAutoSwitch=input.neverAutoSwitchControlSchemes;autoSwitchCaptured=true;report.autoSwitchOriginal=oldNeverAutoSwitch;
            input.neverAutoSwitchControlSchemes=true;report.autoSwitchTemporaryApplied=input.neverAutoSwitchControlSchemes;
            Need(report.autoSwitchTemporaryApplied,"Owned QA disables SDK automatic device switching temporarily");
            keyboard=InputSystem.AddDevice<Keyboard>("HomeInputVisual-QA"); mouse=InputSystem.AddDevice<Mouse>("HomeInputVisual-QA");
            InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus; Application.runInBackground=true;
            RecordRouting("setup-before-switch",0);
            input.SwitchCurrentControlScheme("Keyboard and Mouse",keyboard,mouse);
            RecordRouting("setup-after-switch",0);
            foreach(string name in new[]{"Move","Dash","CameraMove","Cancel"})
            {
                var a=input.currentActionMap.FindAction(name,true); Need(a.enabled,"Original action enabled: "+name);
                foreach(var b in a.bindings) report.bindings.Add(name+"|"+b.name+"|"+b.path+"|"+b.effectivePath);
            }
            var move=input.currentActionMap.FindAction("Move",true); var dash=input.currentActionMap.FindAction("Dash",true);
            foreach(var key in new[]{"w","a","s","d"}) Need(move.bindings.Any(b=>b.path=="<Keyboard>/"+key&&b.effectivePath==b.path),"Original keyboard binding: "+key);
            Need(dash.bindings.Any(b=>b.path=="<Keyboard>/leftShift"&&b.effectivePath==b.path),"Original dash binding");
            Need(input.currentActionMap.FindAction("CameraMove",true).controls.Any(c=>c.device==mouse),"Original camera mouse action");
        }
        void Queue(Key[] keys,float x=0,float y=0) { queuedKeys=keys;queuedMouseDelta=new Vector2(x,y); InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys)); InputSystem.QueueStateEvent(mouse,new MouseState{delta=new Vector2(x,y)}); }
        IEnumerator Run()
        {
            while(!BindObjects()) yield return null; Setup(); int cancels=0, stable=0, wait=0;
            while(stable<60)
            {
                Need(++wait<=900,"Startup becomes clear within 900 actual frames"); Queue(new Key[0]);
                var panels=(IList)Read(ui,"basePanelStack"); var top=panels.Count==0?null:panels[panels.Count-1];
                if(top!=null&&top.GetType().Name=="TutorialPanelView"&&wait%90==0)
                {
                    Need(++cancels<=3,"At most three original tutorial cancels");
                    var k=input.currentActionMap.FindAction("Cancel",true).controls.OfType<KeyControl>().First(c=>c.device==keyboard);
                    Queue(new[]{k.keyCode});
                }
                yield return null; RecordRouting("startup",wait); stable=ClearUi()?stable+1:0;
            }
            // No position reset. Fresh-profile original lifecycle chooses the starting pose/location.
            report.rendering=new RenderEnvironment{pipeline=UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline?UnityEngine.Rendering.GraphicsSettings.currentRenderPipeline.name:"built-in",activeColorSpace=QualitySettings.activeColorSpace.ToString(),graphicsDevice=SystemInfo.graphicsDeviceName,graphicsAPI=SystemInfo.graphicsDeviceType.ToString(),screenWidth=Screen.width,screenHeight=Screen.height};
            int totalFrames=0;
            foreach(var segment in contract.segments)
            {
                var keys=(segment.keys??new string[0]).Select(k=>(Key)Enum.Parse(typeof(Key),k)).ToArray();
                var r=new SegmentResult{name=segment.name,started=Time.realtimeSinceStartupAsDouble,gameTimeStarted=Time.time,requestedSeconds=segment.durationSeconds};
                double nextSample=r.started, previousInput=r.started; int shot=0;zeroMoveSince=-1;segmentPreviousPosition=playerTransform.position;report.inProgressSegment=r;
                while(Time.realtimeSinceStartupAsDouble-r.started<segment.durationSeconds)
                {
                    Need(++totalFrames<=12000&&r.frames<segment.maxFrames,"Finite real-frame guard; never silently shorten a timed hold");
                    Need(ClearUi()&&SceneManager.GetActiveScene().name=="Apartment","Home and controls remain clear; no automatic portal/menu retry");
                    double before=Time.realtimeSinceStartupAsDouble;
                    float mouseStep=(float)Math.Min(.05,Math.Max(0,before-previousInput)); previousInput=before;
                    Queue(keys,segment.mouseXPerSecond*mouseStep,segment.mouseYPerSecond*mouseStep);
                    yield return null; yield return new WaitForEndOfFrame(); r.frames++;
                    double now=Time.realtimeSinceStartupAsDouble, elapsed=now-r.started;
                    var routing=RecordRouting(segment.name,r.frames);CheckMovementInput(r,routing,now);
                    bool shotDue=shot<segment.screenshotSeconds.Length&&elapsed>=segment.screenshotSeconds[shot];
                    if(now>=nextSample||r.frames==1||elapsed>=segment.durationSeconds||shotDue)
                    {
                        var sample=Observe(segment,r.frames-1,elapsed,now);
                        if(shotDue) { sample.screenshotBeginRealtime=Time.realtimeSinceStartupAsDouble; sample.screenshot=Capture(segment.name+"-"+(shot+1).ToString("D2")); sample.screenshotEndRealtime=Time.realtimeSinceStartupAsDouble; shot++; r.screenshots++; }
                        trace.WriteLine(JsonUtility.ToJson(sample)); report.sampleCount++; r.samples++; nextSample=Time.realtimeSinceStartupAsDouble+SampleInterval;
                        Need(report.sampleCount<=1500,"Bounded retained sample count"); Need(report.errors.Count==0,"No Game/engine errors");
                    }
                }
                // Release immediately after the timed hold. The following neutral segment observes the original game's stop response.
                Queue(new Key[0]); r.ended=Time.realtimeSinceStartupAsDouble; r.gameTimeEnded=Time.time; r.elapsed=r.ended-r.started;
                if(r.movementRequested){Need(r.sdkMoveFrames>0&&r.decodedMoveFrames>0&&r.sampledTravelXZ>.01f,"Owned QA movement segment requires original SDK+decoded input and >1cm actual horizontal travel; no gameplay acceptance");
                    if(keys.Contains(Key.LeftShift))Need(r.sdkDashFrames>0&&r.decodedDashFrames>0,"Requested dash reaches original SDK and decoded input");
                    r.movementAccepted=true;report.movementSegments++;}
                report.segments.Add(r);report.inProgressSegment=null; trace.Flush();routingTrace.Flush();
                Need(r.screenshots==segment.screenshotSeconds.Length,"All passive screenshot points observed; do not infer missing images");
            }
        }
        static string TransformPath(Transform t) { var parts=new List<string>(); while(t) {parts.Add(t.name+"["+t.GetSiblingIndex()+"]");t=t.parent;} parts.Reverse();return string.Join("/",parts); }
        int AnimatorRecord()
        {
            var avatar=animator.avatar;var controller=animator.runtimeAnimatorController;
            int aid=avatar?avatar.GetInstanceID():0,cid=controller?controller.GetInstanceID():0,id=animator.GetInstanceID();
            int found=report.animatorIdentities.FindIndex(x=>x.animatorId==id&&x.avatarId==aid&&x.controllerId==cid);
            if(found>=0)return found; Need(report.animatorIdentities.Count<8,"Bounded animator identities");
            var clips=controller?controller.animationClips:new AnimationClip[0]; Need(clips.Length<=512,"Bounded controller clip inventory; no silent truncation");
            var identities=clips.Select(c=>new ClipIdentity{instanceId=c.GetInstanceID(),name=c.name,length=c.length,frameRate=c.frameRate,humanMotion=c.humanMotion,legacy=c.legacy,looping=c.isLooping,wrapMode=c.wrapMode.ToString(),events=c.events.Select(e=>new ClipEvent{time=e.time,function=e.functionName,intParameter=e.intParameter,floatParameter=e.floatParameter,stringParameter=e.stringParameter}).ToArray()}).OrderBy(c=>c.name,StringComparer.Ordinal).ThenBy(c=>c.length).ThenBy(c=>c.frameRate).ToArray();
            // Exclude session-dependent IDs from metadata digest. This deliberately is not a hash of animation curves or masks.
            var metadata=identities.Select(c=>new ClipIdentity{name=c.name,length=c.length,frameRate=c.frameRate,humanMotion=c.humanMotion,legacy=c.legacy,looping=c.looping,wrapMode=c.wrapMode,events=c.events}).ToArray();
            string sha;using(var h=SHA256.Create())sha=BitConverter.ToString(h.ComputeHash(System.Text.Encoding.UTF8.GetBytes(JsonUtility.ToJson(new ClipSet{clips=metadata})))).Replace("-","");
            report.animatorIdentities.Add(new AnimatorIdentity{animatorId=id,animatorName=animator.name,path=TransformPath(animator.transform),avatarId=aid,avatarName=avatar?avatar.name:null,avatarPresent=avatar,avatarValid=avatar&&avatar.isValid,avatarHuman=avatar&&avatar.isHuman,isHuman=animator.isHuman,humanScale=animator.isHuman?animator.humanScale:0,controllerId=cid,controllerName=controller?controller.name:null,clipMetadataSHA=sha,controllerClips=identities,layerNames=Enumerable.Range(0,animator.layerCount).Select(i=>animator.GetLayerName(i)).ToArray()});return report.animatorIdentities.Count-1;
        }
        Bone[] Bones()
        {
            bool usable=animator.avatar&&animator.avatar.isValid&&animator.isHuman;
            return LegBones.Select(b=>{var t=usable?animator.GetBoneTransform(b):null;return t?new Bone{humanoidName=b.ToString(),found=true,id=t.GetInstanceID(),parentId=t.parent?t.parent.GetInstanceID():0,path=TransformPath(t),localPosition=t.localPosition,localRotation=t.localRotation,worldPosition=t.position,worldRotation=t.rotation,rootRelativePosition=playerTransform.InverseTransformPoint(t.position),rootRelativeRotation=Quaternion.Inverse(playerTransform.rotation)*t.rotation}:new Bone{humanoidName=b.ToString(),found=false};}).ToArray();
        }
        Cam CameraRecord(Camera c)
        {
            var t=c.targetTexture;
            var post=c.GetComponents<Component>().FirstOrDefault(x=>x&&x.GetType().FullName=="UnityEngine.Rendering.PostProcessing.PostProcessLayer");
            var aa=post?Read(post,"antialiasingMode"):null;
            var avatar=Read(pca,"PlayerAvatarObjectReferencer");
            return new Cam{transform=TransformRecordOf(c.transform,"Camera"),fieldOfView=c.fieldOfView,orthographicSize=c.orthographicSize,aspect=c.aspect,orthographic=c.orthographic,headViewport=ProjectAnchor(c,Read(avatar,"Head")as Transform,"Head"),faceTargetViewport=ProjectAnchor(c,Read(avatar,"FaceCameraTarget")as Transform,"FaceCameraTarget"),postProcessPresent=post,postProcessEnabled=post&&(post as Behaviour).enabled,antialiasingMode=aa==null?-1:Convert.ToInt32(aa),antialiasingName=aa?.ToString(),name=c.name,path=TransformPath(c.transform),id=c.GetInstanceID(),target=t?t.name:null,targetId=t?t.GetInstanceID():0,depth=c.depth,rect=c.rect,pixelRect=c.pixelRect,mask=c.cullingMask,position=c.transform.position,rotation=c.transform.rotation,clearFlags=c.clearFlags.ToString(),background=c.backgroundColor,renderingPath=c.renderingPath.ToString(),actualRenderingPath=c.actualRenderingPath.ToString(),cameraType=c.cameraType.ToString(),depthTextureMode=c.depthTextureMode.ToString(),near=c.nearClipPlane,far=c.farClipPlane,allowHDR=c.allowHDR,allowMSAA=c.allowMSAA,allowDynamicResolution=c.allowDynamicResolution,forceIntoRenderTexture=c.forceIntoRenderTexture,
                texture=t?new Rt{format=t.format.ToString(),graphicsFormat=t.graphicsFormat.ToString(),depthStencilFormat=t.descriptor.depthStencilFormat.ToString(),dimension=t.dimension.ToString(),width=t.width,height=t.height,depth=t.depth,volumeDepth=t.volumeDepth,antiAliasing=t.antiAliasing,sRGB=t.sRGB,graphicsFormatHasAlpha=UnityEngine.Experimental.Rendering.GraphicsFormatUtility.HasAlphaChannel(t.graphicsFormat),useDynamicScale=t.useDynamicScale}:null};
        }
        // All Game paths below use fields (including compiler-generated auto backing fields), never getters.
        static object ReadStatic(string typeName,string fieldName)
        {
            var type=AppDomain.CurrentDomain.GetAssemblies().Select(a=>a.GetType(typeName,false)).FirstOrDefault(t=>t!=null);
            Need(type!=null,"Required loaded type: "+typeName);
            var field=type.GetField(fieldName,BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.DeclaredOnly);
            if(field==null)throw new MissingFieldException(typeName,fieldName);return field.GetValue(null);
        }
        static Component[] PreviewComponents(string typeName)
        {
            var type=AppDomain.CurrentDomain.GetAssemblies().Select(a=>a.GetType(typeName,false)).FirstOrDefault(t=>t!=null);
            Need(type!=null,"Required loaded type: "+typeName);
            var components=UnityEngine.Object.FindObjectsOfType(type).Cast<Component>().ToArray();
            Need(components.Length<=8,"Bounded active preview controller inventory; never silently truncate");return components;
        }
        TransformRecord TransformRecordOf(Transform t,string role)
        {
            if(!t)return new TransformRecord{role=role,found=false};
            return new TransformRecord{role=role,name=t.name,path=TransformPath(t),found=true,id=t.GetInstanceID(),parentId=t.parent?t.parent.GetInstanceID():0,
                localPosition=t.localPosition,worldPosition=t.position,localScale=t.localScale,lossyScale=t.lossyScale,localRotation=t.localRotation,worldRotation=t.rotation,
                rootRelativePosition=playerTransform.InverseTransformPoint(t.position),rootRelativeRotation=Quaternion.Inverse(playerTransform.rotation)*t.rotation,up=t.up,forward=t.forward};
        }
        static ViewportPoint ProjectAnchor(Camera c,Transform t,string name)
        {
            if(!t)return new ViewportPoint{anchor=name,known=false};
            var point=c.WorldToViewportPoint(t.position);bool front=point.z>0;
            return new ViewportPoint{anchor=name,known=true,world=t.position,viewport=point,inFront=front,insideViewport=front&&point.x>=0&&point.x<=1&&point.y>=0&&point.y<=1};
        }
        PreviewFraming PreviewRecord()
        {
            var avatar=Read(pca,"PlayerAvatarObjectReferencer");var custom=ReadStatic("ExposureUnnoticed2.Scripts.InGame.GameState","PlayerCustomData");var heel=Read(pca,"PlayerToeHeelController");
            var r=new PreviewFraming{avatarKnown=avatar!=null,avatarId=avatar is UnityEngine.Object obj?obj.GetInstanceID():0,customDataKnown=custom!=null,heelOffsetKnown=heel!=null};
            r.anchors=new[]{"FaceCameraPosiiton","FaceCameraTarget","Head","Neck","Spine"}.Select(n=>TransformRecordOf(Read(avatar,n)as Transform,n)).ToArray();
            if(custom!=null){r.customizeHeight=(int)Read(custom,"CustomizeHeight");r.customizeNeck=(int)Read(custom,"CustomizeNeck");r.customizeThighsLength=(int)Read(custom,"CustomizeThighsLength");r.customizeLowerLegLength=(int)Read(custom,"CustomizeLowerLegLength");r.customizeUpperBodyLength=(int)Read(custom,"CustomizeUpperBodyLength");}
            if(heel!=null)r.heelOffset=(float)Read(heel,"HeelOffset");
            r.faces=PreviewComponents("ExposureUnnoticed2.Object3D.Camera.FaceCameraController").Select(c=>{
                var cam=Read(c,"camera")as Camera;return new FacePreview{id=c.GetInstanceID(),enabled=((Behaviour)c).enabled,isFaceCamera=(bool)Read(c,"isFaceCamera"),playerMatchesRoot=object.ReferenceEquals(Read(c,"player"),playerTransform),cameraId=cam?cam.GetInstanceID():0,
                    transform=TransformRecordOf(c.transform,"FaceCameraController"),offset=(Vector3)Read(c,"offset"),currentOffset=(Vector3)Read(c,"currentOffset"),initRectHeight=(float)Read(c,"initRectHeight"),offsetY=(float)Read(c,"offsetY"),rectScale=(float)Read(c,"rectScale")};}).ToArray();
            r.bodies=PreviewComponents("ExposureUnnoticed2.Object3D.Camera.BodyCameraController").Select(c=>new BodyPreview{id=c.GetInstanceID(),enabled=((Behaviour)c).enabled,transform=TransformRecordOf(c.transform,"BodyCameraController"),
                smallOffset=(Vector3)Read(c,"smallOffset"),bigOffset=(Vector3)Read(c,"bigOffset"),futomomoOffset=(Vector3)Read(c,"futomomoOffset"),initPosition=(Vector3)Read(c,"initPosition"),a=(float)Read(c,"a"),b=(float)Read(c,"b"),c=(float)Read(c,"c"),d=(float)Read(c,"d"),e=(float)Read(c,"e")}).ToArray();
            return r;
        }
        static Clip[] Clips(AnimatorClipInfo[] clips) => clips.Select(c=>new Clip{name=c.clip?c.clip.name:null,weight=c.weight,events=c.clip?c.clip.events.Select(e=>e.functionName).Distinct().ToArray():new string[0]}).ToArray();
        static float Rms(float[] a) { double s=0; foreach(float x in a)s+=x*x; return (float)Math.Sqrt(s/a.Length); }
        Sample Observe(Segment segment,int n,double elapsed,double now)
        {
            var baseCam=(Camera)Read(cameraController,"camera"); var tp=Read(cameraController,"thirdPersonCamera"); var target=(Transform)Read(tp,"cameraTarget");
            source.GetOutputData(sourceBlock,0); AudioListener.GetOutputData(mixBlock,0);
            float foot=(float)Read(sound,"lastFootSeTime");
            if(oldFoot>=0&&foot!=oldFoot) { report.footTimestampChanges++; captureDue=Time.frameCount+2; } oldFoot=foot;
            var s=new Sample{frame=Time.frameCount,realtime=now,segmentElapsed=elapsed,time=Time.time,dt=Time.deltaTime,dsp=AudioSettings.dspTime,phase=segment.name,segmentFrame=n,requested=string.Join("+",segment.keys??new string[0]),scene=SceneManager.GetActiveScene().name,
                sdkMove=input.currentActionMap["Move"].ReadValue<Vector2>(),sdkDash=input.currentActionMap["Dash"].ReadValue<float>()>.5f,sdkCamera=input.currentActionMap["CameraMove"].ReadValue<Vector2>(),
                decodedMove=(Vector3)Read(decoded,"Move"),decodedDash=(bool)Read(decoded,"Dash"),moveV=(Vector3)Read(state,"MoveV"),stateDash=(bool)Read(state,"IsDash"),isStrafe=(bool)Read(state,"IsStrafe"),autoRun=(bool)Read(state,"IsAutoRun"),autoChase=(bool)Read(state,"IsAutoChase"),
                position=playerTransform.position,velocity=cc.velocity,speed=new Vector2(cc.velocity.x,cc.velocity.z).magnitude,grounded=cc.isGrounded,cameraPosition=baseCam.transform.position,cameraDistance=Vector3.Distance(baseCam.transform.position,target.position),
                footTime=foot,sourceRms=Rms(sourceBlock),listenerRms=Rms(mixBlock),sourcePlaying=source.isPlaying,sourceMute=source.mute,sourceVolume=source.volume,sourcePitch=source.pitch,sourceAssignedClip=source.clip?source.clip.name:null,listenerVolume=AudioListener.volume};
            s.rootRotation=playerTransform.rotation;s.animatorIdentityIndex=AnimatorRecord();s.bones=Bones();s.animatorEnabled=animator.enabled;s.applyRootMotion=animator.applyRootMotion;s.animatorSpeed=animator.speed;s.animatorUpdateMode=animator.updateMode.ToString();s.animatorCullingMode=animator.cullingMode.ToString();
            s.positionDeltaKnown=hasPrevious&&now>previousRealtime;
            if(s.positionDeltaKnown){s.actualSampleSeconds=now-previousRealtime;s.sampleFrameGap=Time.frameCount-previousFrame;s.positionDelta=s.position-previousPosition;s.positionVelocity=s.positionDelta/(float)s.actualSampleSeconds;s.actualPlanarSpeed=new Vector2(s.positionVelocity.x,s.positionVelocity.z).magnitude;}
            previousPosition=s.position;previousRealtime=now;previousFrame=Time.frameCount;hasPrevious=true;
            s.layers=Enumerable.Range(0,animator.layerCount).Select(i=>{var a=animator.GetCurrentAnimatorStateInfo(i);var b=animator.GetNextAnimatorStateInfo(i);return new Layer{layer=i,name=animator.GetLayerName(i),weight=animator.GetLayerWeight(i),transition=animator.IsInTransition(i),currentHash=a.fullPathHash,currentTime=a.normalizedTime,nextHash=b.fullPathHash,nextTime=b.normalizedTime,current=Clips(animator.GetCurrentAnimatorClipInfo(i)),next=Clips(animator.GetNextAnimatorClipInfo(i))};}).ToArray();
            var cameras=Camera.allCameras;Need(cameras.Length<=16,"Bounded all-enabled-camera inventory; no silent omissions");s.cameras=cameras.Select(CameraRecord).ToArray();s.previewFraming=PreviewRecord();
            if(captureDue>=0&&Time.frameCount>=captureDue&&report.audioWindows<24)
            {
                captureDue=-1; string name="foot-window-"+(++report.audioWindows).ToString("D2");
                s.footAudioSourceWindow=name+"-source.wav"; s.mixedAudioWindow=name+"-mix.wav";
                Wave(Path.Combine(contract.output,s.footAudioSourceWindow),sourceBlock); Wave(Path.Combine(contract.output,s.mixedAudioWindow),mixBlock);
            }
            return s;
        }
        void Wave(string path,float[] samples)
        {
            using(var f=new BinaryWriter(File.Create(path)))
            {
                int bytes=samples.Length*2; f.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));f.Write(36+bytes);f.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));f.Write(16);f.Write((short)1);f.Write((short)1);f.Write(report.rate);f.Write(report.rate*2);f.Write((short)2);f.Write((short)16);f.Write(System.Text.Encoding.ASCII.GetBytes("data"));f.Write(bytes);
                foreach(float x in samples)f.Write((short)Mathf.RoundToInt(Mathf.Clamp(x,-1f,1f)*32767f));
            }
        }
        string Capture(string name)
        {
            var tex=ScreenCapture.CaptureScreenshotAsTexture(); try { Need(tex&&tex.width>100&&tex.height>100,"Rendered window screenshot"); string p=name+".png"; File.WriteAllBytes(Path.Combine(contract.output,p),tex.EncodeToPNG());return p; } finally { if(tex)Destroy(tex); }
        }
        void CleanupFailure(string step,Exception failure,ref Exception first)
        {
            report.sdkCleanupFailed=true;report.cleanupErrors.Add(step+": "+failure);first=first??failure;
        }
        void Finish(Exception error)
        {
            if(done)return; done=true;
            // Each resource is attempted independently. Keep the original failure, but retain every cleanup error.
            try { report.cleanupAttempts.Add("release-owned-input");if(keyboard!=null&&keyboard.added)Queue(new Key[0]); }
            catch(Exception e){CleanupFailure("release-owned-input",e,ref error);}
            try
            {
                report.cleanupAttempts.Add("restore-original-pairing");
                if(paired)
                {
                    Need(input&&oldDevices!=null&&oldDevices.Length>0&&oldDevices.All(d=>d.added)&&!string.IsNullOrEmpty(oldScheme),"Original SDK devices and scheme available for restoration");
                    input.SwitchCurrentControlScheme(oldScheme,oldDevices);
                    Need(input.currentControlScheme==oldScheme&&input.devices.Count==oldDevices.Length&&oldDevices.All(d=>input.devices.Contains(d)),"Original SDK device pairing restored exactly");
                }
            }
            catch(Exception e){CleanupFailure("restore-original-pairing",e,ref error);}
            try { report.cleanupAttempts.Add("remove-owned-keyboard");if(keyboard!=null&&keyboard.added)InputSystem.RemoveDevice(keyboard); }
            catch(Exception e){CleanupFailure("remove-owned-keyboard",e,ref error);}
            try { report.cleanupAttempts.Add("remove-owned-mouse");if(mouse!=null&&mouse.added)InputSystem.RemoveDevice(mouse); }
            catch(Exception e){CleanupFailure("remove-owned-mouse",e,ref error);}
            try { report.cleanupAttempts.Add("restore-background-behavior");if(paired)InputSystem.settings.backgroundBehavior=oldBackground; }
            catch(Exception e){CleanupFailure("restore-background-behavior",e,ref error);}
            try { report.cleanupAttempts.Add("restore-run-in-background");if(paired)Application.runInBackground=oldRun; }
            catch(Exception e){CleanupFailure("restore-run-in-background",e,ref error);}
            finally
            {
                try
                {
                    report.cleanupAttempts.Add("restore-auto-switch");
                    if(autoSwitchCaptured){Need(input,"Original SDK PlayerInput still exists for restoring auto-switch");input.neverAutoSwitchControlSchemes=oldNeverAutoSwitch;report.autoSwitchRestored=input.neverAutoSwitchControlSchemes==oldNeverAutoSwitch;Need(report.autoSwitchRestored,"Original SDK auto-switch setting restored");}
                }
                catch(Exception e){CleanupFailure("restore-auto-switch",e,ref error);}
            }
            try
            {
                report.sdkRestored=!report.sdkCleanupFailed&&(!paired||InputSystem.settings.backgroundBehavior==oldBackground&&Application.runInBackground==oldRun)&&!(keyboard?.added??false)&&!(mouse?.added??false)&&(!autoSwitchCaptured||report.autoSwitchRestored);
                Need(report.sdkRestored,"SDK cleanup incomplete; earlier failures remain reported even if later steps succeed");
            }
            catch(Exception e){report.sdkRestored=false;CleanupFailure("verify-sdk-cleanup",e,ref error);}
            try { trace?.Dispose(); } catch(Exception e){CleanupFailure("close-trace",e,ref error);report.sdkRestored=false;}
            try { routingTrace?.Dispose(); } catch(Exception e){CleanupFailure("close-routing-trace",e,ref error);report.sdkRestored=false;}
            report.completed=error==null;report.movementSequenceAccepted=error==null&&report.movementSegments>0&&report.segments.Where(s=>s.movementRequested).All(s=>s.movementAccepted);
            report.status=error==null?"sequence-complete-review-required":"failed";report.failure=error?.ToString();
            File.WriteAllText(Path.Combine(contract.output,"result.json"),JsonUtility.ToJson(report,true)); Application.logMessageReceived-=Log; Application.Quit(error==null?0:6);
        }
    }
}
