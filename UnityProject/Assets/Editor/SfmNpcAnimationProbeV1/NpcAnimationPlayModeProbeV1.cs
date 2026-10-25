// Owned Editor fixture. Original Game bodies are called unmodified; Root deploys and runs.
#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Object3D.RoutePoint;
using ExposureUnnoticed2.ObjectUI.OptionMenu;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using Object = UnityEngine.Object;

namespace SfmMvp.Editor
{
 [InitializeOnLoad] public static class NpcAnimationPlayModeProbeV1
 {
  const string Project="E:/ACTgame/SFM/ReverseEngineering/tools/unity-editor/npc-core-runtime-candidate-v1/Project";
  const string Key="OwnedNpcAnimationProbeV1.", ContractPath="Assets/Editor/SfmNpcAnimationProbeV1/probe-contract.json";
  const string NpcGuid="791378a06555be54a99eb05e12d101e7", ManagerGuid="1fe6b30d2eb34b0aad848593cc30aa3a", WorldGuid="2bc045a7890745dc83c37bb2b058806a";
  static readonly string Domain=Guid.NewGuid().ToString("N");
  [Serializable] public sealed class Pin { public string path,sha256; public long bytes; }
  [Serializable] public sealed class MethodPin { public string identity,moduleMVID,rawILSHA256; public int token,rawILBytes; }
  [Serializable] public sealed class Contract { public string format,project,bootstrapSelection;public bool preparationOnly;public Pin editorGame,playerGame,playerGuard,sourceContract,selectedEquivalence; public Pin[] sourcePins;public MethodPin[] methods; }
  [Serializable] public sealed class Frame { public int frame,queueCount,delay;public float delta,current,target,moveSpeed,animatorSpeed,overrideWeight;public bool isFemale,isFaceNiya,isFaceContempt; }
  [Serializable] public sealed class Report {
   public string phase,failure,unity,beginDomain,playDomain,editorGameSHA256,playerGameSHA256,selectedEquivalenceSHA256,avatarName,avatarType,animatorController,bootstrapSelection;
   public bool passed,actualEditorPlayMode,domainReloadObserved,wholeEditorGuardClaimed=false,GameMethodsHooked=false,originalSavesAccessed=false;
   public bool originalConstructionReturned,originalAnimationInitializeReturned,originalSchedulerCreated,naturalDelayedCallbackObserved,setSpeedCalled,nonzeroCurrentObserved,targetReached,playerFixtureNull,fullNpcInitializeCalled=false,manualSchedulerTicks=false;
   public int checks,startFrame,endFrame,setSpeedFrame,driverUpdates,schedulerInitialDelay,schedulerInitialQueue,selectedMethods;
   public List<string> checksPassed=new List<string>(),engineErrors=new List<string>();public List<Frame> frames=new List<Frame>();public string[] limitations;
  }
  static Report report;static Contract contract;static bool finishing;
  static GameObject host,managerObject,worldObject;static NavMeshDataInstance navInstance;
  static NpcComponentAccessor nca;static SmoothFloat speed;static SexManager sex;static bool expectedFemale;static int reachedFrame=-1;
  static bool Armed=>SessionState.GetBool(Key+"armed",false);
  static NpcAnimationPlayModeProbeV1(){EditorApplication.playModeStateChanged+=Changed;EditorApplication.update+=Watchdog;Application.logMessageReceived+=Log;}
  static string Hash(string p){using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(File.ReadAllBytes(p))).Replace("-","");}
  static string Hash(byte[] data){using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(data)).Replace("-","");}
  static string Full(string p)=>Path.GetFullPath(p).TrimEnd(Path.DirectorySeparatorChar,Path.AltDirectorySeparatorChar);
  static void Need(bool ok,string label){report.checks++;if(!ok)throw new InvalidOperationException(label);report.checksPassed.Add(label);}
  static void Check(Pin p){Need(p!=null&&File.Exists(p.path)&&new FileInfo(p.path).Length==p.bytes&&Hash(p.path)==p.sha256,"Exact input "+(p==null?"null":p.path));}
  static void Save(){SessionState.SetString(Key+"report",JsonUtility.ToJson(report));}
  static void Load(){if(report==null)report=JsonUtility.FromJson<Report>(SessionState.GetString(Key+"report","{}"));if(contract==null)contract=JsonUtility.FromJson<Contract>(File.ReadAllText(Path.Combine(Project,ContractPath)));}
  static T Read<T>(object target,string name){var f=target.GetType().GetField(name,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance);if(f==null)throw new MissingFieldException(target.GetType().FullName,name);return(T)f.GetValue(target);}
  static GameObject Asset(string guid){string p=AssetDatabase.GUIDToAssetPath(guid);Need(!string.IsNullOrEmpty(p),"Original GUID resolves "+guid);var a=AssetDatabase.LoadAssetAtPath<GameObject>(p);Need(a!=null,"Original prefab imports "+guid);return a;}
  public static void Begin(){
   if(Armed)throw new InvalidOperationException("NPC animation probe already armed");report=new Report{phase="configured",unity=Application.unityVersion,beginDomain=Domain};
   try{
    Need(Full(Path.Combine(Application.dataPath,".."))==Full(Project),"Exact Root Project");Need(!EditorApplication.isPlayingOrWillChangePlaymode&&!EditorApplication.isCompiling,"Fresh EditMode entry");Need(!EditorSettings.enterPlayModeOptionsEnabled,"Default domain reload enabled");
    var args=Environment.GetCommandLineArgs();int k=Array.IndexOf(args,"-sfmNpcAnimationProbeOutputV1");Need(k>=0&&k+1<args.Length,"Explicit report argument");Need(Array.IndexOf(args,"-sfmNpcAnimationProbeV1")>=0,"Explicit isolated bootstrap selection");
    string output=Full(args[k+1]);Need(output.StartsWith(Full(Path.Combine(Project,"../runs"))+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)&&!Directory.Exists(output)&&!File.Exists(output),"Fresh owned non-Project output");
    Need(!args.Any(x=>x.StartsWith("-sfm",StringComparison.OrdinalIgnoreCase)&&x!="-sfmNpcAnimationProbeV1"&&x!="-sfmNpcAnimationProbeOutputV1"),"No other owned smoke/funding/bundle entry");
    contract=JsonUtility.FromJson<Contract>(File.ReadAllText(Path.Combine(Project,ContractPath)));Need(!contract.preparationOnly,"Final new frozen Editor binding required; preparation-only contract cannot run");Need(contract.format=="OwnedNpcAnimationPlayModeProbe/v1"&&Full(contract.project)==Full(Project),"Exact probe contract");Check(contract.editorGame);Check(contract.playerGame);Check(contract.playerGuard);Check(contract.sourceContract);Check(contract.selectedEquivalence);foreach(var p in contract.sourcePins)Check(p);
    Need(Hash(typeof(NpcController).Assembly.Location)==contract.editorGame.sha256,"Loaded Editor image equals frozen Editor copy");VerifyMethods();
    Directory.CreateDirectory(output);SessionState.SetString(Key+"output",output);SessionState.SetString(Key+"deadline",DateTime.UtcNow.AddSeconds(150).Ticks.ToString());SessionState.SetBool(Key+"armed",true);SessionState.SetInt(Key+"exit",3);Save();
    EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);EditorApplication.isPlaying=true;
   }catch(Exception e){Finish(e);}
  }
  static void VerifyMethods(){var module=typeof(NpcController).Module;Need(contract.methods.Length>0,"Selected complete-CIL/MM equivalence has methods");foreach(var p in contract.methods){Need(module.ModuleVersionId.ToString()==p.moduleMVID,"Selected Editor MVID "+p.identity);var m=module.ResolveMethod(p.token);var body=m.GetMethodBody();Need(body!=null,"Actual Editor body "+p.identity);var raw=body.GetILAsByteArray();Need(raw.Length==p.rawILBytes&&Hash(raw)==p.rawILSHA256,"Exact loaded complete raw IL "+p.identity);}report.selectedMethods=contract.methods.Length;}
  static void Changed(PlayModeStateChange change){if(!Armed)return;Load();if(change==PlayModeStateChange.EnteredPlayMode){try{report.playDomain=Domain;report.domainReloadObserved=report.beginDomain!=Domain;report.actualEditorPlayMode=EditorApplication.isPlaying;Need(report.domainReloadObserved&&report.actualEditorPlayMode,"Real PlayMode after domain reload");InitializeFixture();Save();}catch(Exception e){Finish(e);}}if(change==PlayModeStateChange.EnteredEditMode){if(report.phase!="finished"){report.passed=false;report.failure="PlayMode ended before completion";Write();}SessionState.SetBool(Key+"armed",false);int exit=SessionState.GetInt(Key+"exit",3);EditorApplication.delayCall+=()=>EditorApplication.Exit(exit);}}
  static void Log(string message,string stack,LogType kind){if(!Armed||!EditorApplication.isPlaying||finishing)return;Load();if(kind==LogType.Error||kind==LogType.Exception||kind==LogType.Assert){report.engineErrors.Add(kind+": "+message);Save();}}
  static void Watchdog(){if(!Armed||finishing)return;Load();if(DateTime.UtcNow.Ticks>long.Parse(SessionState.GetString(Key+"deadline","0")))Finish(new TimeoutException("Actual PlayMode fixture150s deadline"));}
  static void InitializeFixture(){
   report.phase="constructing";report.startFrame=Time.frameCount;report.editorGameSHA256=Hash(typeof(NpcController).Assembly.Location);report.playerGameSHA256=Hash(contract.playerGame.path);report.selectedEquivalenceSHA256=Hash(contract.selectedEquivalence.path);report.bootstrapSelection=contract.bootstrapSelection;
   Need(report.editorGameSHA256==contract.editorGame.sha256,"Post-reload actual Editor image pinned");VerifyMethods();
   Need(NpcManager.Instance==null&&SexManager.Instance==null&&PlayerController.Instance==null&&InGameManager.Instance==null,"No live Game singleton is replaced");Need(GameState.OptionData==null&&GameState.GameStateData==null,"Fresh domain has no user state/options");
   Need(!Object.FindObjectsOfType<MonoBehaviour>().Any(x=>x.GetType().FullName=="SfmMvp.OriginalMovementMvpBridge"),"Empty scene has no MVP actor bridge or duplicate frame driver");
   GameState.OptionData=new OptionData();sex=new SexManager();sex.Initialize();Need(ReferenceEquals(SexManager.Instance,sex)&&sex.ActiveTraceSexPlayerNpc==null,"Original SexManager.Initialize, no active trace");
   managerObject=Object.Instantiate(Asset(ManagerGuid));Need(!managerObject.activeInHierarchy,"Original Manager input forest remains inactive");var manager=managerObject.GetComponentInChildren<NpcManager>(true);Need(manager!=null,"Original Manager component");manager.Initialize(null);Need(NpcManager.Instance==manager,"Original Manager.Initialize(null)");
   worldObject=(GameObject)PrefabUtility.InstantiatePrefab(Asset(WorldGuid));Need(!worldObject.activeInHierarchy,"Original navigation input forest remains inactive");var builder=worldObject.GetComponentsInChildren<RouteBuilderConbini>(true).Single();var appear=Read<RoutePoint>(builder,"appearPoint");
   var surface=worldObject.GetComponentsInChildren<MonoBehaviour>(true).Single(x=>x.GetType().FullName=="Unity.AI.Navigation.NavMeshSurface");var data=Read<NavMeshData>(surface,"m_NavMeshData");Need(data!=null&&AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(data))=="0f51b2c2415a663488542d05557ae38a","Original baked navigation GUID");
   navInstance=NavMesh.AddNavMeshData(data,surface.transform.position,surface.transform.rotation);Need(navInstance.valid,"Owned SDK mounts original Nav data at original Surface pose");NavMeshHit spawn=default;Need(appear!=null&&NavMesh.SamplePosition(appear.transform.position,out spawn,1f,7),"Original appear point samples original Nav data");
   host=new GameObject("Owned NPC animation frame fixture");var prefab=Asset(NpcGuid);var npc=Object.Instantiate(prefab,spawn.position,prefab.transform.rotation,host.transform).GetComponent<NpcComponent>();Need(npc!=null&&npc.gameObject.activeInHierarchy,"Active original NPC prefab");
   var controller=npc.CreateController();var state=new NpcStateModel();nca=new NpcComponentAccessor(controller,npc,state,null);Need(controller!=null&&npc.NpcController==controller&&controller.Nca==null,"Original controller constructor, full Initialize remains uncalled");Need(nca.Player==null&&nca.NpcController==controller&&nca.StateModel==state,"Original NCA constructor, explicit null Player fixture");report.playerFixtureNull=true;report.originalConstructionReturned=true;
   var animator=nca.Animator;Need(animator!=null&&animator.avatar!=null&&animator.avatar.isValid&&animator.runtimeAnimatorController!=null&&animator.layerCount>2,"Actual original Animator, valid rig/controller/layers");Need(nca.AvatarComponent.Head!=null&&nca.AvatarComponent.Head.IsChildOf(npc.AvaterObject.transform),"Original serialized Head belongs to actual rig");report.avatarName=npc.AvaterObject.name;report.avatarType=state.AvatarType.ToString();report.animatorController=AssetDatabase.GetAssetPath(animator.runtimeAnimatorController);
   Need(animator.parameters.Any(p=>p.name=="IsFemale"&&p.type==AnimatorControllerParameterType.Bool)&&animator.parameters.Any(p=>p.name=="Speed"&&p.type==AnimatorControllerParameterType.Float),"Actual Animator defines original parameter types");expectedFemale=(int)state.AvatarType==0||(int)state.AvatarType==4;
   // SDK parameter sentinels make a swallowed Scheduler callback exception distinguishable from success.
   // They do not write Game state, discovery flags, SmoothFloat.Current, or replace clips/controller.
   animator.SetBool("IsFemale",!expectedFemale);animator.SetFloat("Speed",0.375f);Need(animator.GetBool("IsFemale")!=expectedFemale&&Mathf.Abs(animator.GetFloat("Speed")-0.375f)<0.00001f,"Explicit SDK-only pre-Rebind sentinels recorded");
   Need(Object.FindObjectsOfType<Scheduler>().Length==0,"No prior Scheduler or callback queue");nca.NpcAnimationManager.Initialize(nca);report.originalAnimationInitializeReturned=true;
   speed=Read<SmoothFloat>(nca.NpcAnimationManager,"speed");Need(speed.Current==0f&&speed.Target==0f&&!animator.applyRootMotion,"Original animation Initialize resets speed and root motion");var scheduler=Object.FindObjectsOfType<Scheduler>().Single();Need(scheduler.gameObject.activeInHierarchy&&scheduler.enabled,"Original DelayFlame generated active Scheduler MonoBehaviour");report.originalSchedulerCreated=true;
   var list=Read<IList>(scheduler,"modelList");report.schedulerInitialQueue=list.Count;Need(list.Count==1,"Exactly original animation delayed callback queued");report.schedulerInitialDelay=Read<int>(list[0],"delayFrame");Need(report.schedulerInitialDelay==1,"Original callback delay1 unchanged");
   report.phase="natural-scheduler";var driver=host.AddComponent<NpcAnimationFrameDriverV1>();Need(driver!=null&&driver.enabled&&driver.gameObject.activeInHierarchy,"Actual Editor assembly MonoBehaviour attached and active; engine driver platform check");
  }
  // Called once by the owned MonoBehaviour.Update after default-order Scheduler.Update. Never calls Scheduler manually.
  internal static void Tick(){if(!Armed||finishing||nca==null)return;try{
   report.driverUpdates++;float before=speed.Current;Need(nca.NpcController!=null&&sex.ActiveTraceSexPlayerNpc==null&&PlayerController.Instance==null,"Non-trace actual NPC avoids null Player branch");nca.NpcAnimationManager.OnUpdate();
   float current=speed.Current,move=nca.NpcAnimationManager.GetCurrentMoveSpeed(),actual=nca.Animator.GetFloat("Speed");Need(!float.IsNaN(current)&&!float.IsInfinity(current)&&current>=0f&&current<=1f,"Original speed remains finite in requested0..1 range");Need(move==current,"Original GetCurrentMoveSpeed reads actual SmoothFloat.Current");
   if(report.setSpeedCalled){Need(current>=before,"Original speed advances monotonically toward requested1");Need(Mathf.Abs(actual-current)<=0.0011f,"Actual Animator Speed follows original Current threshold");if(current>0f)report.nonzeroCurrentObserved=true;}
   Need(!nca.Animator.GetBool("IsFaceNiya")&&!nca.Animator.GetBool("IsFaceContempt"),"Original empty facial layers and non-trace branch produce false");
   var scheduler=Object.FindObjectsOfType<Scheduler>().Single();var queue=Read<IList>(scheduler,"modelList");report.frames.Add(new Frame{frame=Time.frameCount,delta=Time.deltaTime,current=current,target=speed.Target,moveSpeed=move,animatorSpeed=actual,overrideWeight=nca.Animator.GetLayerWeight(2),isFemale=nca.Animator.GetBool("IsFemale"),isFaceNiya=nca.Animator.GetBool("IsFaceNiya"),isFaceContempt=nca.Animator.GetBool("IsFaceContempt"),queueCount=queue.Count,delay=queue.Count==0?-1:Read<int>(queue[0],"delayFrame")});
   if(!report.naturalDelayedCallbackObserved&&queue.Count==0){Need(Time.frameCount>report.startFrame,"Original Scheduler completed on a later actual Unity frame");Need(nca.Animator.GetBool("IsFemale")==expectedFemale,"Original delayed IsFemale overwrote inverse SDK sentinel");Need(Mathf.Abs(nca.Animator.GetFloat("Speed"))<0.00001f,"Original delayed Rebind cleared SDK Speed sentinel");report.naturalDelayedCallbackObserved=true;
    nca.NpcAnimationManager.SetSpeed(1f);report.setSpeedCalled=true;report.setSpeedFrame=Time.frameCount;Need(speed.Current==0f&&speed.Target==1f,"Original SetSpeed changes Target only; Current was never forced");report.phase="original-speed-frame";
   }
   if(report.setSpeedCalled&&current==1f){if(reachedFrame<0)reachedFrame=Time.frameCount;report.targetReached=true;if(Time.frameCount>=reachedFrame+3){Need(report.nonzeroCurrentObserved&&report.naturalDelayedCallbackObserved,"Original delayed callback and actual smoothing both observed");Need(report.engineErrors.Count==0,"No actual engine Error/Exception/Assert");report.passed=true;Finish(null);return;}}
   // Wall watchdog is authoritative. Uncapped batch frames may advance faster than real smoothing time.
   Save();
  }catch(Exception e){Finish(e);}}
  static void Write(){string output=SessionState.GetString(Key+"output","");if(!string.IsNullOrEmpty(output)&&Directory.Exists(output))File.WriteAllText(Path.Combine(output,"npc-animation-frames.json"),JsonUtility.ToJson(report,true)+"\n");}
  static void Finish(Exception error){if(finishing)return;finishing=true;if(report==null)report=new Report();if(error!=null){report.passed=false;report.failure=error.ToString();}report.endFrame=Time.frameCount;
   report.limitations=new[]{"Actual Unity Editor PlayMode fixture only; Root must execute before any runtime acceptance.","Null Player is an explicit construction/non-trace animation fixture. No Controller.Initialize or NCA.Initialize/UI, route dispatch, movement displacement, detection, tasks or NPC population acceptance.","Owned frame adapter calls only original Animation.OnUpdate once per Unity Update. Original Scheduler MonoBehaviour naturally dispatches its own Update; never double-ticked.","Owned SDK Nav loader mounts original baked data at original Surface pose; no full world collision/vision geometry claim.","Two explicit SDK Animator parameter sentinels test delayed Rebind and IsFemale effects despite original Scheduler swallowing managed callback exceptions. Original controller/clips and Game fields are unchanged.","Selected original complete CIL/MM/TM binding links frozen Editor to separately whole-guarded Player; no whole Editor assembly claim. No old MVP actor/legacy32method pin consumers are loaded."};
   // Domain teardown owns Unity objects and temporary singletons; no private Game singleton setters or constructor replacement.
   report.phase="finished";SessionState.SetInt(Key+"exit",report.passed?0:3);Save();Write();if(EditorApplication.isPlayingOrWillChangePlaymode)EditorApplication.isPlaying=false;else{SessionState.SetBool(Key+"armed",false);int exit=report.passed?0:3;EditorApplication.delayCall+=()=>EditorApplication.Exit(exit);}
  }
 }
}
#endif
