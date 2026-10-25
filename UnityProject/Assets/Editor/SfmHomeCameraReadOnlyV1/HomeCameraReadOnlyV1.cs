using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SfmMvp.Editor
{
    // Owned observation only. No static reference to Game, reflection invocation, field write, or input injection.
    [InitializeOnLoad]
    public static class HomeCameraReadOnlyV1
    {
        const string Key="SFM.HomeCameraReadOnlyV1.";
        const string Product="SFMOriginalGameplayRuntimeV2";
        const string Prefix="SFMOriginalGameplayV23Diag-";
        const string Profiles=@"C:\Users\Administrator\AppData\LocalLow\SFMRecovery";
        [Serializable] public class Pin { public string path,sha256;public long bytes; }
        [Serializable] public class Contract { public string schema,homeScene,product,testProduct,testProfilePath,gameSHA,editorSHA;public Pin[] inputs,protectedSaves,testSaves; }
        [Serializable] public class Pose { public int id;public string path;public Vector3 position,forward,localPosition,localScale,lossyScale;public Quaternion rotation; }
        [Serializable] public class CameraView
        {
            public Pose pose;public int mask,targetWidth,targetHeight,targetDisplay;public float fov,near,far,depth,aspect;
            public Rect rect,pixelRect;public bool enabled,targetIsNull,urpPresent,urpRenderTypeKnown,urpStackKnown;
            public string target,cameraType,clearFlags,urpAssembly,urpRenderType;
            public List<Pose> urpCameraStack=new List<Pose>();
        }
        [Serializable] public class Value { public string field,value;public bool known; }
        [Serializable] public class Cast { public bool queryValid,hit;public string reason,colliderPath;public int colliderId,layer,mask;public bool trigger;public Vector3 origin,direction,point,normal;public float radius,maxDistance,distance,resultDistance; }
        [Serializable] public class RendererView { public string path,kind;public int id,layer;public bool enabled,active,maskIncludes,containsCamera;public float cameraDistance;public Vector3 center,size;public string shadowCastingMode; }
        [Serializable] public class Sample
        {
            public int frame,readyRelativeFrame;public string scene,samplingPhase="EditorApplication.update; not a before/after LateUpdate hook";
            public Pose player,pcaTransform,target,tp,fpChild,inventory;public CameraView main;
            public List<CameraView> allCameras=new List<CameraView>();public List<Value> state=new List<Value>(),baseFields=new List<Value>(),tpFields=new List<Value>(),fpFields=new List<Value>();
            public bool basePrivateCameraMatches,baseTPMatchesReferencer,baseFPMatchesAvatar;public float mainToTargetDistance,mainToPlayerDistance;
            public int nearRendererTotal;public bool nearRendererListTruncated;
            public Cast originalTPCast;public List<RendererView> nearRenderers=new List<RendererView>();
        }
        [Serializable] public class Report
        {
            public string schema="HomeCameraReadOnly/v1",contractSHA,error="",actualProfile,screenshotPath,screenshotSHA;
            public bool complete,passed,playModeEntered,productRestored,protectedSavesUnchanged,boundInputsUnchanged,screenshotRequested,screenshotExists;
            public int firstReadyFrame=-1,lastObservedFrame=-1,actualReadyFrames,engineErrors;
            public List<string> engineErrorMessages=new List<string>();public List<Sample> samples=new List<Sample>();
            public bool fullGameplayAccepted=false,standalonePlayerObserved=false;
        }
        static Contract contract;static Report report;static string output;static bool ending;
        static HomeCameraReadOnlyV1(){EditorApplication.update+=Tick;EditorApplication.playModeStateChanged+=Mode;Application.logMessageReceived+=Log;}
        static void Need(bool ok,string why){if(!ok)throw new InvalidOperationException(why);}
        static string Arg(string key){var a=Environment.GetCommandLineArgs();int i=Array.IndexOf(a,key);return i>=0&&i+1<a.Length?a[i+1]:null;}
        static string Hash(string p){using(var s=File.OpenRead(p))using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(s)).Replace("-","");}
        static void Verify(IEnumerable<Pin> pins){foreach(var p in pins){Need(File.Exists(p.path)&&new FileInfo(p.path).Length==p.bytes&&Hash(p.path)==p.sha256,"Changed pinned file: "+p.path);}}
        static void Write(){File.WriteAllText(Path.Combine(output,"observation.json"),JsonUtility.ToJson(report,true));}
        static void Load()
        {
            if(contract!=null)return;
            string path=SessionState.GetString(Key+"contract","");output=SessionState.GetString(Key+"output","");
            Need(Hash(path)==SessionState.GetString(Key+"sha",""),"Fresh contract SHA after reload");
            contract=JsonUtility.FromJson<Contract>(File.ReadAllText(path));
            report=JsonUtility.FromJson<Report>(File.ReadAllText(Path.Combine(output,"observation.json")));
        }
        public static void Begin()
        {
            try
            {
                Need(!EditorApplication.isPlayingOrWillChangePlaymode,"Idle Editor required");
                string path=Path.GetFullPath(Arg("-sfmHomeCameraContract"));string sha=Arg("-sfmHomeCameraContractSHA");
                Need(Hash(path)==sha,"Exact fresh camera contract");
                contract=JsonUtility.FromJson<Contract>(File.ReadAllText(path));
                Need(contract.schema=="HomeCameraReadOnly/v1"&&contract.product==Product,"Camera observation contract");
                Need(Arg("-sfmNpcDetectionV23Contract")==path,"Explicit isolated identity flag must bind same camera contract");
                Need(contract.testProduct.StartsWith(Prefix,StringComparison.Ordinal)&&contract.testProduct.Length==Prefix.Length+32&&Guid.TryParseExact(contract.testProduct.Substring(Prefix.Length),"N",out _),"Unique restricted isolated product");
                Need(Path.GetFullPath(contract.testProfilePath)==Path.GetFullPath(Path.Combine(Profiles,contract.testProduct)),"Restricted isolated profile path");
                Need(PlayerSettings.companyName=="SFMRecovery"&&PlayerSettings.productName==Product,"Original starting identity");
                Need(EditorSettings.enterPlayModeOptionsEnabled==false,"Domain reload must remain enabled for clean lifecycle");
                Verify(contract.inputs);Verify(contract.protectedSaves);Verify(contract.testSaves);
                Need(Hash(typeof(HomeCameraReadOnlyV1).Assembly.Location)==contract.editorSHA,"Exact imported observer assembly");
                var game=AppDomain.CurrentDomain.GetAssemblies().Single(x=>x.GetName().Name=="Assembly-CSharp");
                Need(Hash(game.Location)==contract.gameSHA,"Exact current Game assembly");
                output=Path.GetFullPath(Arg("-sfmHomeCameraOutput"));
                string runs=Path.GetFullPath(@"E:\ACTgame\SFM-LayerFS-Work\runs")+Path.DirectorySeparatorChar;
                Need(output.StartsWith(runs,StringComparison.OrdinalIgnoreCase)&&!Directory.Exists(output),"Fresh output under controlled runs root");
                Directory.CreateDirectory(output);report=new Report{contractSHA=sha,screenshotPath=Path.Combine(output,"game-view.png")};Write();
                SessionState.SetString(Key+"contract",path);SessionState.SetString(Key+"sha",sha);SessionState.SetString(Key+"output",output);
                SessionState.SetString(Key+"startedUTC",DateTime.UtcNow.ToString("O"));SessionState.SetString(Key+"originalProduct",Product);
                SessionState.SetBool(Key+"armed",true);SessionState.SetBool(Key+"productChanged",true);
                PlayerSettings.productName=contract.testProduct;AssetDatabase.SaveAssets();
                EditorSceneManager.OpenScene(contract.homeScene,OpenSceneMode.Single);
                EditorApplication.isPlaying=true;
            }
            catch(Exception e){if(report!=null)Fail(e);else{Debug.LogError(e);EditorApplication.Exit(4);}}
        }
        static void Log(string condition,string stack,LogType type)
        {
            if(!SessionState.GetBool(Key+"armed",false)||report==null)return;
            if(type==LogType.Error||type==LogType.Exception||type==LogType.Assert){report.engineErrors++;if(report.engineErrorMessages.Count<8)report.engineErrorMessages.Add(condition);}
        }
        static void Mode(PlayModeStateChange change)
        {
            if(!SessionState.GetBool(Key+"armed",false))return;
            try{Load();if(change==PlayModeStateChange.EnteredPlayMode){report.playModeEntered=true;report.actualProfile=Application.persistentDataPath;Need(Path.GetFullPath(report.actualProfile)==Path.GetFullPath(contract.testProfilePath),"Actual isolated persistent-data identity");Write();}
                else if(change==PlayModeStateChange.ExitingPlayMode)Write();
                else if(change==PlayModeStateChange.EnteredEditMode)RestoreAndExit();}
            catch(Exception e){Fail(e);}
        }
        static void Tick()
        {
            if(!SessionState.GetBool(Key+"armed",false)||ending)return;
            try
            {
                Load();
                Need((DateTime.UtcNow-DateTime.Parse(SessionState.GetString(Key+"startedUTC",""),null,System.Globalization.DateTimeStyles.RoundtripKind)).TotalSeconds<180,"180-second lifecycle timeout; not gameplay acceptance");
                if(!EditorApplication.isPlaying)return;
                Need(Path.GetFullPath(Application.persistentDataPath)==Path.GetFullPath(contract.testProfilePath),"Profile remains isolated");
                if(report.lastObservedFrame==Time.frameCount)return;
                report.lastObservedFrame=Time.frameCount;
                var player=Find("ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController");
                var b=Find("ExposureUnnoticed2.Object3D.Camera.BaseCameraController");
                object pca=Read(player,"pca"),state=Read(pca,"PlayerState");
                var camera=Read(b,"camera")as Camera;var tp=Read(b,"thirdPersonCamera")as Component;
                if(!player||!b||pca==null||state==null||!camera||!tp||!(Read(tp,"cameraTarget")is Transform))return;
                if(report.firstReadyFrame<0)report.firstReadyFrame=Time.frameCount;
                report.actualReadyFrames=Time.frameCount-report.firstReadyFrame;
                if(report.actualReadyFrames%25==0||report.actualReadyFrames>=180){report.samples.Add(Observe(player,b,pca,state,camera,tp));Write();}
                if(report.actualReadyFrames>=150&&!report.screenshotRequested){ScreenCapture.CaptureScreenshot(report.screenshotPath);report.screenshotRequested=true;Write();}
                if(report.actualReadyFrames>=180){report.complete=true;ending=true;Write();EditorApplication.isPlaying=false;}
            }
            catch(Exception e){Fail(e);}
        }
        static void Fail(Exception e)
        {
            if(ending&&report!=null&&report.error.Length>0)return;
            ending=true;
            if(report!=null){report.error=e.ToString();Write();}
            if(EditorApplication.isPlaying)EditorApplication.isPlaying=false;else RestoreAndExit();
        }
        static void RestoreAndExit()
        {
            ending=true;
            try
            {
                Load();Need(!EditorApplication.isPlaying,"Restore only after Play Mode exits");
                if(SessionState.GetBool(Key+"productChanged",false))
                {
                    Need(SessionState.GetString(Key+"originalProduct","")==Product,"Recorded original product");
                    Need(PlayerSettings.productName==contract.testProduct||PlayerSettings.productName==Product,"No unrelated product overwrite");
                    PlayerSettings.productName=Product;AssetDatabase.SaveAssets();SessionState.SetBool(Key+"productChanged",false);
                }
                report.productRestored=PlayerSettings.productName==Product;
                Verify(contract.protectedSaves);report.protectedSavesUnchanged=true;
                Verify(contract.inputs);report.boundInputsUnchanged=true;
                report.screenshotExists=File.Exists(report.screenshotPath);
                if(report.screenshotExists)report.screenshotSHA=Hash(report.screenshotPath);
                report.passed=report.complete&&report.playModeEntered&&report.actualReadyFrames>=180&&report.error.Length==0&&report.engineErrors==0&&report.productRestored&&report.protectedSavesUnchanged&&report.boundInputsUnchanged;
            }
            catch(Exception e){if(report!=null){report.error+=(report.error.Length>0?"\n":"")+e;report.passed=false;}}
            SessionState.SetBool(Key+"armed",false);if(report!=null)Write();EditorApplication.Exit(report!=null&&report.passed?0:6);
        }
        // Deliberately fields only: Game getters/methods are never invoked.
        static FieldInfo RawField(Type type,string name)
        {
            for(Type t=type;t!=null;t=t.BaseType)
            {
                var f=t.GetField(name,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.DeclaredOnly)
                    ??t.GetField("<"+name+">k__BackingField",BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.DeclaredOnly);
                if(f!=null)return f;
            }
            return null;
        }
        static object Read(object o,string name)
        {
            if(o==null)return null; // The object/field value may genuinely be uninitialized.
            var f=RawField(o.GetType(),name);
            if(f==null)throw new MissingFieldException(o.GetType().FullName,name+" [required raw field or automatic backing field; no Game getter invoked]");
            return f.GetValue(o);
        }
        static object ReadOptionalSDK(object o,string name)
        {
            if(o==null)return null;var f=RawField(o.GetType(),name);return f==null?null:f.GetValue(o);
        }
        static Component Find(string fullName)
        {
            var t=AppDomain.CurrentDomain.GetAssemblies().Select(a=>a.GetType(fullName,false)).FirstOrDefault(x=>x!=null);
            if(t==null)return null;var found=UnityEngine.Object.FindObjectsOfType(t);return found.Length==1?found[0]as Component:null;
        }
        static string PathOf(Transform t){if(!t)return "";var names=new List<string>();for(;t;t=t.parent)names.Add(t.name);names.Reverse();return string.Join("/",names);}
        static Pose P(Transform t){return !t?null:new Pose{id=t.GetInstanceID(),path=PathOf(t),position=t.position,rotation=t.rotation,forward=t.forward,localPosition=t.localPosition,localScale=t.localScale,lossyScale=t.lossyScale};}
        static CameraView C(Camera c)
        {
            if(!c)return null;
            var rt=c.targetTexture;
            var v=new CameraView{pose=P(c.transform),mask=c.cullingMask,fov=c.fieldOfView,near=c.nearClipPlane,far=c.farClipPlane,depth=c.depth,
                aspect=c.aspect,rect=c.rect,pixelRect=c.pixelRect,targetDisplay=c.targetDisplay,cameraType=c.cameraType.ToString(),clearFlags=c.clearFlags.ToString(),
                enabled=c.enabled,targetIsNull=!rt,target=rt?rt.name:"",targetWidth=rt?rt.width:0,targetHeight=rt?rt.height:0};
            var urp=c.GetComponents<Component>().FirstOrDefault(x=>x&&x.GetType().FullName=="UnityEngine.Rendering.Universal.UniversalAdditionalCameraData");
            v.urpPresent=urp;
            if(urp)
            {
                v.urpAssembly=urp.GetType().Assembly.GetName().Name;
                // Raw SDK serialization fields; no cameraStack getter that can emit overlay warnings or alter data.
                object renderType=ReadOptionalSDK(urp,"m_CameraType"),stack=ReadOptionalSDK(urp,"m_Cameras");
                v.urpRenderTypeKnown=renderType!=null;v.urpRenderType=Scalar(renderType);
                var cameras=stack as IEnumerable<Camera>;v.urpStackKnown=cameras!=null;
                if(cameras!=null)v.urpCameraStack=cameras.Select(x=>P(x?x.transform:null)).ToList();
            }
            return v;
        }
        static string Scalar(object v){if(v==null)return "";if(v is float||v is int||v is bool||v is Vector2||v is Vector3||v.GetType().IsEnum)return Convert.ToString(v,System.Globalization.CultureInfo.InvariantCulture);var c=v as Component;return c?PathOf(c.transform):v.GetType().FullName;}
        static List<Value> Values(object o,params string[] names){return names.Select(n=>{object v=Read(o,n);return new Value{field=n,known=v!=null,value=Scalar(v)};}).ToList();}
        static Sample Observe(Component player,Component b,object pca,object state,Camera camera,Component tp)
        {
            var fp=Read(b,"firstPersonCamera")as Component;var child=Read(fp,"child")as Transform;
            var target=Read(tp,"cameraTarget")as Transform;var inv=Read(b,"inventoryCameraController")as Component;var pcaTf=Read(pca,"Transform")as Transform;
            var manager=Find("ExposureUnnoticed2.Scripts.InGame.InGameManager");var refs=Read(manager,"InGameReferencer");var avatar=Read(pca,"PlayerAvatarObjectReferencer");
            var s=new Sample{frame=Time.frameCount,readyRelativeFrame=report.actualReadyFrames,scene=SceneManager.GetActiveScene().path,
                player=P(player.transform),pcaTransform=P(pcaTf),target=P(target),tp=P(tp.transform),fpChild=P(child),inventory=P(inv?inv.transform:null),main=C(camera),
                basePrivateCameraMatches=ReferenceEquals(Read(b,"Camera"),camera),baseTPMatchesReferencer=ReferenceEquals(tp,Read(refs,"TpCameraController")),baseFPMatchesAvatar=ReferenceEquals(fp,Read(avatar,"FPCameraController")),
                mainToTargetDistance=Vector3.Distance(camera.transform.position,target.position),mainToPlayerDistance=Vector3.Distance(camera.transform.position,player.transform.position)};
            s.state=Values(state,"IsStrafe","IsDash","CurrentAction","IsFixedPositionRotation","IsFixedWarpPositionRotation");
            s.state.AddRange(Values(Read(refs,"GameOverController"),"currentState"));
            s.baseFields=Values(b,"isStrafeCache","IsActiveInventoryCamera","IsLockCamera","tpsCameraMask","fpsCameraMask");
            s.tpFields=Values(tp,"cameraMaxDistance","currentVerticalValue","Yaw","Pitch","ignoreLayerMask","skipFrameCameraCount");
            s.fpFields=Values(fp,"targetOffset","neckOffset","initLocalPosition","Yaw","Pitch");
            var cameras=UnityEngine.Object.FindObjectsOfType<Camera>().Where(c=>c.enabled&&c.gameObject.activeInHierarchy).ToArray();
            Need(cameras.Length<=32,"Bounded full enabled-camera inventory, never silently truncated");
            s.allCameras=cameras.Select(C).ToList();s.originalTPCast=ProbeCast(pcaTf,tp,target);
            Vector3 cp=camera.transform.position;
            var near=UnityEngine.Object.FindObjectsOfType<Renderer>().Where(r=>r.enabled&&r.gameObject.activeInHierarchy)
                .Select(r=>new RendererView{path=PathOf(r.transform),id=r.GetInstanceID(),kind=r.GetType().Name,layer=r.gameObject.layer,
                    enabled=r.enabled,active=r.gameObject.activeInHierarchy,maskIncludes=(camera.cullingMask&(1<<r.gameObject.layer))!=0,
                    containsCamera=r.bounds.Contains(cp),cameraDistance=Vector3.Distance(cp,r.bounds.ClosestPoint(cp)),center=r.bounds.center,size=r.bounds.size,shadowCastingMode=r.shadowCastingMode.ToString()})
                .Where(r=>r.cameraDistance<=.5f).OrderBy(r=>r.cameraDistance).ThenBy(r=>r.id).ToList();
            s.nearRendererTotal=near.Count;s.nearRendererListTruncated=near.Count>64;s.nearRenderers=near.Take(64).ToList();
            return s;
        }
        static Cast ProbeCast(Transform playerRoot,Component tp,Transform target)
        {
            object max=Read(tp,"cameraMaxDistance"),v=Read(tp,"currentVerticalValue"),yaw=Read(tp,"Yaw"),pitch=Read(tp,"Pitch"),mask=Read(tp,"ignoreLayerMask");
            var c=new Cast{radius=.05f};
            if(!playerRoot||!(max is float)||!(v is float)||!(yaw is float)||!(pitch is float)||!(mask is int)){c.reason="Missing exact raw TP fields/Pca.Transform";return c;}
            c.origin=target.position;c.origin.y=Mathf.Max((target.position.y-playerRoot.position.y)*(float)v,.3f)+playerRoot.position.y;
            c.direction=Quaternion.Euler((float)pitch,(float)yaw,0)*Vector3.forward*-1;c.maxDistance=(float)max;c.mask=~(int)mask;c.queryValid=true;
            RaycastHit h;c.hit=Physics.SphereCast(c.origin,c.radius,c.direction,out h,c.maxDistance,c.mask,QueryTriggerInteraction.Ignore);
            c.resultDistance=c.maxDistance;if(c.hit){c.distance=h.distance;c.resultDistance=Mathf.Min(c.maxDistance,h.distance);c.point=h.point;c.normal=h.normal;c.colliderId=h.collider.GetInstanceID();c.colliderPath=PathOf(h.collider.transform);c.layer=h.collider.gameObject.layer;c.trigger=h.collider.isTrigger;}
            return c;
        }
    }
}
