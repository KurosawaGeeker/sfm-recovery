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
using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Object3D.IngameManager;
using ExposureUnnoticed2.Object3D.Player.Scripts;
namespace SfmMvp.Editor
{
    // Observation only. Original MonoBehaviour messages own all Game calls.
    [InitializeOnLoad]
    public static class OriginalGameplayLifecycleProbeV8
    {
        const string Project="E:/ACTgame/SFM/ReverseEngineering/tools/unity-editor/core-gameplay-bootstrap-runtime-candidate-v2/Project";
        const string Key="SfmOriginalLifecycleV8.";
        static readonly string Domain=Guid.NewGuid().ToString("N");
        [Serializable]public sealed class Pin {public string path,sha256;public long bytes;}
        [Serializable]public sealed class Contract {public Pin[] inputs;public string editorGameSHA,scene,product;}
        [Serializable]public sealed class Sample {public int frame,managers,players,updaters,lateUpdaters,starters,resourceAssignments;public bool adapterReady,realPlayerBound,pcaPresent,dispatcherBound,stageControllerPresent;public double currentRp,earnedRp;public int missionProgressEntries;public bool baseCameraReady,thirdPersonReady,firstPersonReady,inventoryTargetReady,fovMatchesOriginalOption;public float cameraFov;public int cameraMask;public int inputBindingManagers,inputActionMaps;public bool inputBindingInstanceBound,inputActionAssetPresent,inputFilenameMatchesOriginal,inputOverridesFilePresent;}
        [Serializable]public sealed class Report
        {
            public bool passed,actualPlayMode,domainReloadObserved,manualGameTicks=false,GameHooks=false,ownedFunding=false,ownedMissionProgress=false,originalSavesAccessed=false,ownedPersistentDataOnly=true;
            public bool fullGameplayAccepted=false;public string failure,beginDomain,playDomain,editorGameSHA,persistentDataPath,contractSHA;
            public int checks;public List<string> transitionErrors=new List<string>();public List<string> errors=new List<string>();public List<Sample> samples=new List<Sample>();
        }
        static Report report;static bool finished;static int lastFrame=-1;
        static bool Armed=>SessionState.GetBool(Key+"armed",false);
        // Mono rejects some absolute Windows paths at 260 characters. Resolve only
        // files inside the exact Project relative to the verified process cwd.
        static string IoPath(string p){string full=Path.GetFullPath(p),root=Path.GetFullPath(Project)+Path.DirectorySeparatorChar;return full.StartsWith(root,StringComparison.OrdinalIgnoreCase)?full.Substring(root.Length):full;}
        [System.Runtime.InteropServices.DllImport("kernel32.dll",CharSet=System.Runtime.InteropServices.CharSet.Unicode,SetLastError=true)]static extern IntPtr CreateFileW(string path,uint access,uint share,IntPtr security,uint creation,uint flags,IntPtr template);
        [System.Runtime.InteropServices.DllImport("kernel32.dll",SetLastError=true)]static extern bool GetFileSizeEx(IntPtr handle,out long size);
        [System.Runtime.InteropServices.DllImport("kernel32.dll",SetLastError=true)]static extern bool ReadFile(IntPtr handle,byte[] buffer,uint count,out uint read,IntPtr overlapped);
        [System.Runtime.InteropServices.DllImport("kernel32.dll",SetLastError=true)]static extern bool CloseHandle(IntPtr handle);
        static byte[] ReadBoundFile(string p)
        {
            string full=Path.GetFullPath(p);if(full.Length<260)return File.ReadAllBytes(full);
            // Mono normalizes relative paths back to MAX_PATH-limited absolute paths.
            // Read the same exact existing file through the Windows extended-path API.
            IntPtr handle=CreateFileW(@"\\?\"+full,0x80000000,7,IntPtr.Zero,3,128,IntPtr.Zero);
            if(handle==new IntPtr(-1))throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error(),full);
            try{long size;if(!GetFileSizeEx(handle,out size))throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());if(size<0||size>int.MaxValue)throw new IOException("Bound input size");var bytes=new byte[(int)size];uint read;if(!ReadFile(handle,bytes,(uint)bytes.Length,out read,IntPtr.Zero)||read!=bytes.Length)throw new IOException("Exact bound input read failed: "+full);return bytes;}finally{CloseHandle(handle);}
        }
        static string Hash(string p){using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(ReadBoundFile(p))).Replace("-","");}
        static void Load(){if(report==null)report=JsonUtility.FromJson<Report>(SessionState.GetString(Key+"report","{}"));}
        static void Save(){SessionState.SetString(Key+"report",JsonUtility.ToJson(report));}
        static void Need(bool b,string label){report.checks++;if(!b)throw new InvalidOperationException(label);}
        static string Arg(string n){var a=Environment.GetCommandLineArgs();int k=Array.IndexOf(a,n);if(k<0||k+1>=a.Length)throw new InvalidOperationException(n);return a[k+1];}
        static object Field(object value,string name){return value==null?null:value.GetType().GetField(name,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance)?.GetValue(value);}
        static int Count(object value){return value is ICollection c?c.Count:-1;}
        static OriginalGameplayLifecycleProbeV8(){EditorApplication.update+=Poll;EditorApplication.playModeStateChanged+=Changed;Application.logMessageReceived+=Log;}
        public static void Begin()
        {
            if(Armed)throw new InvalidOperationException("Already armed");report=new Report{beginDomain=Domain};
            try
            {
                // Establish the owned report destination before any preflight can fail.
                string diagnosticOutput=Path.GetFullPath(Arg("-sfmOriginalV2Output"));
                Need(!Directory.Exists(diagnosticOutput)&&diagnosticOutput.StartsWith(Path.GetFullPath(Path.Combine(Project,"../runs"))+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase),"Fresh owned diagnostic output");
                Directory.CreateDirectory(diagnosticOutput);SessionState.SetString(Key+"output",diagnosticOutput);Save();
                Need(Path.GetFullPath(Path.Combine(Application.dataPath,".."))==Path.GetFullPath(Project),"Exact independent Project");Need(string.Equals(Path.GetFullPath(Environment.CurrentDirectory),Path.GetFullPath(Project),StringComparison.OrdinalIgnoreCase),"Exact Project cwd for relative IO");
                Need(!EditorApplication.isPlayingOrWillChangePlaymode&&!EditorSettings.enterPlayModeOptionsEnabled,"Fresh default domain reload");
                string cp=Path.GetFullPath(Arg("-sfmOriginalV2Contract"));report.contractSHA=Arg("-sfmOriginalV2ContractSHA");Need(Hash(cp)==report.contractSHA,"Exact launch contract");var c=JsonUtility.FromJson<Contract>(File.ReadAllText(cp));
                foreach(var p in c.inputs)Need(File.Exists(IoPath(p.path))&&new FileInfo(IoPath(p.path)).Length==p.bytes&&Hash(p.path)==p.sha256,"Bound source/scene/resource input "+p.path);
                report.editorGameSHA=Hash(typeof(InGameManager).Assembly.Location);Need(report.editorGameSHA==c.editorGameSHA&&c.editorGameSHA=="052C5CABD501305F9E4674E2B6961CC901E38D9EF0ADBB2FE60D5865307142CF","Exact all-original-linked Editor Game");
                Need(Application.companyName=="SFMRecovery"&&Application.productName==c.product&&c.product=="SFMOriginalGameplayRuntimeV2","Independent persistent identity");report.persistentDataPath=Application.persistentDataPath;
                var args=Environment.GetCommandLineArgs();foreach(string flag in new[]{"-sfmCoreSmoke","-sfmEquipmentSmoke","-sfmOriginalActionSmoke","-sfmMvpOriginalBundle","-sfmNpcAnimationProbeV1"})Need(Array.IndexOf(args,flag)<0,"No alternate/funding entry "+flag);
                SessionState.SetString(Key+"deadline",DateTime.UtcNow.AddSeconds(150).Ticks.ToString());SessionState.SetBool(Key+"finished",false);Save();SessionState.SetBool(Key+"armed",true);
                EditorSceneManager.OpenScene(c.scene,OpenSceneMode.Single);EditorApplication.isPlaying=true;
            }
            catch(Exception e){Finish(e);}
        }
        static void Log(string message,string stack,LogType kind)
        {
            if(!Armed||finished||kind!=LogType.Exception&&kind!=LogType.Error&&kind!=LogType.Assert)return;Load();if(EditorApplication.isPlaying)report.errors.Add(kind+": "+message+"\n"+stack);else report.transitionErrors.Add(kind+": "+message+"\n"+stack);Save();
        }
        static void Poll()
        {
            if(!Armed||finished)return;Load();try
            {
                if(DateTime.UtcNow.Ticks>long.Parse(SessionState.GetString(Key+"deadline","0")))throw new TimeoutException("Natural original lifecycle deadline");
                if(!EditorApplication.isPlaying)return;
                report.actualPlayMode=true;report.playDomain=Domain;report.domainReloadObserved=Domain!=report.beginDomain;
                int frame=Time.frameCount;if(frame==lastFrame||frame<1)return;lastFrame=frame;
                var adapters=UnityEngine.Object.FindObjectsOfType<OriginalGameplayBootstrapV2>();var managers=UnityEngine.Object.FindObjectsOfType<InGameManager>();var players=UnityEngine.Object.FindObjectsOfType<PlayerController>();
                var s=new Sample{frame=frame,managers=managers.Length,players=players.Length};var adapter=adapters.SingleOrDefault();s.adapterReady=adapter&&adapter.Ready;s.resourceAssignments=adapter?adapter.AppliedBindings+adapter.ExistingBindings:0;
                var manager=managers.SingleOrDefault();if(manager){var ir=manager.InGameReferencer;s.realPlayerBound=ir&&ir.Player&&players.Contains(ir.Player);s.pcaPresent=s.realPlayerBound&&ir.Player.Pca!=null;s.stageControllerPresent=manager.CurrentStageController;var dispatcher=manager.GetComponent<EventMethodManager>();s.dispatcherBound=dispatcher&&ReferenceEquals(Field(dispatcher,"inGameReferencer"),ir);s.updaters=Count(Field(dispatcher,"updaterList"));s.lateUpdaters=Count(Field(dispatcher,"lateUpdaterList"));s.starters=Count(Field(dispatcher,"starterList"));}
                if(s.realPlayerBound&&manager.InGameReferencer.Player.PlayerAvatarObjectReferencer)
                {
                    var ir=manager.InGameReferencer;var bc=ir.BaseCameraController;var camera=Field(bc,"camera")as UnityEngine.Camera;
                    s.baseCameraReady=bc&&camera&&bc.Camera==camera;
                    s.thirdPersonReady=ReferenceEquals(Field(ir.TpCameraController,"player"),ir.Player)&&ReferenceEquals(Field(ir.TpCameraController,"baseCamera"),bc);
                    var fp=ir.Player.PlayerAvatarObjectReferencer.FPCameraController;
                    s.firstPersonReady=ReferenceEquals(Field(fp,"player"),ir.Player)&&ReferenceEquals(Field(fp,"baseCamera"),bc);
                    s.inventoryTargetReady=ReferenceEquals(Field(ir.InventoryCameraController,"cameraTarget"),ir.Player.Pca.PlayerAvatarObjectReferencer.CameraTarget);
                    if(camera){s.cameraFov=camera.fieldOfView;s.cameraMask=camera.cullingMask;if(GameState.OptionData!=null){int expected=ir.Player.Pca.PlayerState.IsStrafe?GameState.OptionData.CameraAngle1st:GameState.OptionData.CameraAngle3rd;s.fovMatchesOriginalOption=Mathf.Abs(s.cameraFov-expected)<0.001f;}}
                }
                var inputManagers=UnityEngine.Object.FindObjectsOfType<ExposureUnnoticed2.Scripts.Base.InputRebindSaveManager>();
                s.inputBindingManagers=inputManagers.Length;var binding=ExposureUnnoticed2.Scripts.Base.InputRebindSaveManager.Instance;
                s.inputBindingInstanceBound=binding&&inputManagers.Length==1&&ReferenceEquals(binding,inputManagers[0]);
                if(binding){var asset=Field(binding,"_actionAsset")as UnityEngine.InputSystem.InputActionAsset;s.inputActionAssetPresent=asset;if(asset)s.inputActionMaps=asset.actionMaps.Count;string filename=Field(binding,"_savePath")as string;s.inputFilenameMatchesOriginal=filename=="InputActionOverrides.json";if(s.inputFilenameMatchesOriginal)s.inputOverridesFilePresent=File.Exists(Path.Combine(Application.persistentDataPath,filename));}
                if(GameState.GameStateData!=null){s.currentRp=GameState.GameStateData.CurrentRp;s.earnedRp=GameState.GameStateData.CurrentEarnThisTimeRp;s.missionProgressEntries=GameState.GameStateData.MissionMaxRpBonus100Dict.Count;}
                report.samples.Add(s);Save();
                if(report.errors.Count>0)throw new InvalidOperationException("Original lifecycle emitted engine errors; first failure retained");
                if(frame>=90)
                {
                    Need(report.actualPlayMode&&report.domainReloadObserved,"Observed PlayMode and new domain");Need(s.adapterReady&&s.resourceAssignments==25,"Exact reviewed resource adapter ready");Need(s.managers==1&&s.players==1&&s.realPlayerBound&&s.pcaPresent,"Original Start created and bound real Player/PCA");Need(s.dispatcherBound&&s.updaters==5&&s.lateUpdaters==4&&s.starters==1,"Original dispatcher registered original participants");Need(s.stageControllerPresent,"Original Stage lifecycle registered controller");Need(s.baseCameraReady&&s.thirdPersonReady&&s.firstPersonReady&&s.inventoryTargetReady&&s.fovMatchesOriginalOption,"Original four camera initialization graph and option-derived FOV");Need(s.inputBindingManagers==1&&s.inputBindingInstanceBound&&s.inputActionAssetPresent&&s.inputActionMaps>0&&s.inputFilenameMatchesOriginal,"Original input binding component/Awake/serialized action maps and filename");Need(s.currentRp==0&&s.earnedRp==0,"No owned funding or reward");report.passed=true;Finish(null);
                }
            }
            catch(Exception e){Finish(e);}
        }
        static void Changed(PlayModeStateChange s)
        {
            if(!Armed)return;Load();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool(Key+"armed",false);if(!SessionState.GetBool(Key+"finished",false)){report.failure="Unexpected PlayMode exit";report.passed=false;Write();}EditorApplication.delayCall+=()=>EditorApplication.Exit(report.passed?0:6);}
        }
        static void Write(){string output=SessionState.GetString(Key+"output","");if(output.Length>0)File.WriteAllText(Path.Combine(output,"observation.json"),JsonUtility.ToJson(report,true)+"\n");}
        static void Finish(Exception e)
        {
            if(finished)return;finished=true;SessionState.SetBool(Key+"finished",true);if(e!=null){report.failure=e.ToString();report.passed=false;Debug.LogError("SFM_ORIGINAL_LIFECYCLE_V8_FAILURE: "+e);}Save();Write();if(EditorApplication.isPlayingOrWillChangePlaymode)EditorApplication.isPlaying=false;else EditorApplication.delayCall+=()=>EditorApplication.Exit(report.passed?0:6);
        }
    }
}
#endif
