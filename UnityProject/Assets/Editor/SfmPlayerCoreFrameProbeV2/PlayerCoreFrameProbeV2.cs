#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.InGame;
using SfmMvp;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace SfmMvp.Editor
{
    [InitializeOnLoad]
    public static class PlayerCoreFrameProbeV2
    {
        const string Root="E:/ACTgame/SFM/ReverseEngineering";
        const string Project=Root+"/tools/unity-editor/npc-core-runtime-candidate-v1/Project";
        const string Key="SfmOwnedPlayerCoreFrameV2.";
        const string Scene="Assets/MvpGenerated/MvpApartmentPlayerCoreFramesV2.unity";
        const string ContractPath="Assets/Editor/SfmPlayerCoreFrameProbeV2/probe-contract.json";
        static readonly string Domain=Guid.NewGuid().ToString("N");
        const BindingFlags Flags=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Static|BindingFlags.Instance;
        [Serializable]public sealed class Pin {public string path,sha256;public long bytes;}
        [Serializable]public sealed class Contract {public string format,project;public Pin editorGame,playerGame,playerGuard,sourceContract,scene;public Pin[] sources;}
        [Serializable]public sealed class Sample {public int frame,updateCalls,fixedCalls;public float x,y,z,realtime,just,last,cool;public bool captured;}
        [Serializable]public sealed class Report
        {
            public bool passed,actualPlayMode,domainReloadObserved,originalStateInitialize,originalMovementInitialize,originalOnWarp,originalClothesActionInitialize,originalNpcManagerInitialize,originalStrangenessInitialize;
            public bool runtimeCallbackObserved,originalRingInitialize,inactiveOriginalUi;public int ringShortcutCount,commonRingModels,poseRingModels;
            public bool GameHooks=false,manualCalculatorUpdates=false,originalSavesAccessed=false,ownedCredits=false,realWorldNavigationAccepted=false;
            public string phase,failure,beginDomain,playDomain,unity,editorSHA,playerSHA;
            public int checks,startFrame,endFrame;
            public double startingRp,endingRp,startingEarned,endingEarned;
            public List<string>checksPassed=new List<string>(),engineErrors=new List<string>();
            public List<Sample>samples=new List<Sample>();public string[]scope;
        }
        static Report report;static Contract contract;static OriginalMovementMvpBridge bridge;static MvpPlayerMotor motor;static bool finished;
        static bool Armed=>SessionState.GetBool(Key+"armed",false);
        static string Hash(string p){using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(File.ReadAllBytes(p))).Replace("-","");}
        static void Need(bool b,string label){report.checks++;if(!b)throw new InvalidOperationException(label);report.checksPassed.Add(label);}
        static void Load(){if(report==null)report=JsonUtility.FromJson<Report>(SessionState.GetString(Key+"report","{}"));}
        static void Save(){SessionState.SetString(Key+"report",JsonUtility.ToJson(report));}
        static void PinInput(Pin p){Need(p!=null&&File.Exists(p.path)&&new FileInfo(p.path).Length==p.bytes&&Hash(p.path)==p.sha256,"Exact bound input "+(p==null?"null":p.path));}
        static void EnsureDevices(){if(!Armed)return;if(Keyboard.current==null)InputSystem.AddDevice<Keyboard>("OwnedPlayerCoreV1Keyboard");if(Mouse.current==null)InputSystem.AddDevice<Mouse>("OwnedPlayerCoreV1Mouse");}
        static PlayerCoreFrameProbeV2(){EditorApplication.playModeStateChanged+=Changed;EditorApplication.update+=Deadline;Application.logMessageReceived+=Log;EnsureDevices();}
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void BeforeScene()
        {
            if(!Armed)return;EnsureDevices();PlayerCoreFrameDriverV1.InitializeCallback=InitializeOriginalReceivers;PlayerCoreFrameDriverV1.ObserveCallback=Observe;
            var host=new GameObject("Owned original player core frame observation V1");UnityEngine.Object.DontDestroyOnLoad(host);host.AddComponent<PlayerCoreFrameDriverV1>();
        }
        public static void PrepareScene()
        {
            if(Path.GetFullPath(Path.Combine(Application.dataPath,".."))!=Path.GetFullPath(Project))throw new Exception("Exact owned Project required");
            if(File.Exists(Path.Combine(Project,Scene)))throw new Exception("Fresh owned scene required");
            var scene=EditorSceneManager.OpenScene("Assets/MvpGenerated/MvpApartmentOriginalActions.unity",OpenSceneMode.Single);
            var b=scene.GetRootGameObjects().SelectMany(x=>x.GetComponentsInChildren<OriginalMovementMvpBridge>(true)).Single();
            string Pin(Type t,string name,params Type[]ps){var m=t.GetMethod(name,Flags,null,ps,null);using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(m.GetMethodBody().GetILAsByteArray())).Replace("-","");}
            b.expectedLocomotionUpdateIL=Pin(typeof(PlayerLocomotion),"OnUpdate",typeof(InputData));b.expectedLocomotionFixedIL=Pin(typeof(PlayerLocomotion),"OnFixedUpdate");b.expectedControllerFixedIL=Pin(typeof(PlayerController),"OnFixedUpdate");b.expectedSetStrafeIL=Pin(typeof(PlayerAnimationManager),"SetStrafe",typeof(bool),typeof(bool),typeof(float),typeof(float));b.expectedControllerUpdateIL=Pin(typeof(PlayerController),"OnUpdate");b.expectedActionFrameEndIL=Pin(typeof(PlayerAction),"OnFrameEnd");b.expectedBodyMaterialUpdateIL=Pin(typeof(PlayerBodyMaterialController),"OnUpdate");b.expectedClothesUpdateIL=Pin(typeof(PlayerClothes),"OnUpdate");b.expectedFootUpdateIL=Pin(typeof(PlayerFootStepController),"OnUpdate");b.expectedHandUpdateIL=Pin(typeof(PlayerHandcuffsController),"OnUpdate",typeof(InputData));b.expectedHeartUpdateIL=Pin(typeof(PlayerHeartBeat),"OnUpdate");b.expectedPantsUpdateIL=Pin(typeof(PlayerPantsController),"OnUpdate");b.expectedShioUpdateIL=Pin(typeof(PlayerShiofukiController),"OnUpdate");
            // Explicitly isolated original UI forest: real serialized inputs and constructors,
            // inactive so this probe does not claim complete UIManager/window startup.
            if(UnityEngine.Object.FindObjectsOfType<ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView>(true).Length!=0)throw new Exception("Fresh scene must not already contain a Ring");
            const string originalUi="Assets/GameObject/InGameCanvas.prefab";
            if(Hash(Path.Combine(Project,originalUi))!="D37E610591D17319C67C77F51BDA25B084B17B37463254902AB8FF98FE940EFF")throw new Exception("Original UI prefab bytes");
            var uiAsset=AssetDatabase.LoadAssetAtPath<GameObject>(originalUi);if(!uiAsset)throw new Exception("Original UI prefab missing");
            var ui=(GameObject)PrefabUtility.InstantiatePrefab(uiAsset,scene);ui.SetActive(false);
            if(ui.GetComponentsInChildren<ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView>(true).Length!=1)throw new Exception("Unique real Ring serialized tree");
            if(!EditorSceneManager.SaveScene(scene,Scene))throw new Exception("Save owned diagnostic scene");AssetDatabase.SaveAssets();
            Debug.Log("SFM_PLAYER_CORE_PREPARED "+Hash(typeof(PlayerController).Assembly.Location));
        }
        public static void Begin()
        {
            if(Armed)throw new Exception("Run already armed");report=new Report{phase="configured",beginDomain=Domain,unity=Application.unityVersion};
            try{
                Need(!EditorApplication.isPlayingOrWillChangePlaymode&&!EditorApplication.isCompiling&&!EditorSettings.enterPlayModeOptionsEnabled,"Fresh EditMode with default domain reload");
                contract=JsonUtility.FromJson<Contract>(File.ReadAllText(Path.Combine(Project,ContractPath)));Need(contract.format=="OwnedPlayerCoreFrames/v2"&&Path.GetFullPath(contract.project)==Path.GetFullPath(Project),"Owned contract identity");
                foreach(var p in new[]{contract.editorGame,contract.playerGame,contract.playerGuard,contract.sourceContract,contract.scene})PinInput(p);foreach(var p in contract.sources)PinInput(p);
                var args=Environment.GetCommandLineArgs();int k=Array.IndexOf(args,"-sfmPlayerCoreOutputV2");Need(k>=0&&k+1<args.Length,"Explicit fresh evidence output");string output=Path.GetFullPath(args[k+1]);Need(output.StartsWith(Path.GetFullPath(Path.Combine(Project,"../runs"))+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)&&!Directory.Exists(output),"Fresh Root runs output");
                foreach(string flag in new[]{"-sfmCoreSmoke","-sfmEquipmentSmoke","-sfmOriginalActionSmoke","-sfmPamEngineSmoke","-sfmPamPlayerSmoke","-sfmMvpOriginalBundle","-sfmNpcAnimationProbeV1"})Need(Array.IndexOf(args,flag)<0,"No competing or funding entry "+flag);
                Directory.CreateDirectory(output);SessionState.SetString(Key+"output",output);SessionState.SetString(Key+"deadline",DateTime.UtcNow.AddSeconds(180).Ticks.ToString());SessionState.SetBool(Key+"armed",true);Save();EditorSceneManager.OpenScene(Scene,OpenSceneMode.Single);EditorApplication.isPlaying=true;
            }catch(Exception e){Finish(e);}
        }
        static void Changed(PlayModeStateChange s)
        {
            if(!Armed)return;Load();if(s==PlayModeStateChange.EnteredPlayMode){report.actualPlayMode=EditorApplication.isPlaying;report.playDomain=Domain;report.domainReloadObserved=Domain!=report.beginDomain;Save();}
            if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool(Key+"armed",false);if(report.phase!="finished"){report.failure="Unexpected PlayMode exit";report.passed=false;Write();}int code=report.passed?0:6;EditorApplication.delayCall+=()=>EditorApplication.Exit(code);}
        }
        static void InitializeOriginalReceivers()
        {
            if(finished)return;Load();try{
                // Sample before any original call can fail; EnteredPlayMode may arrive later.
                report.actualPlayMode=EditorApplication.isPlaying;report.playDomain=Domain;report.domainReloadObserved=Domain!=report.beginDomain;report.runtimeCallbackObserved=true;Save();
                Need(report.actualPlayMode&&report.domainReloadObserved,"Runtime callback directly observes real PlayMode and new domain");
                bridge=UnityEngine.Object.FindObjectsOfType<OriginalMovementMvpBridge>().Single();Need(bridge.Ready&&bridge.UsesOriginalConstructors,"Same original actor/PCA constructor graph ready");motor=bridge.GetComponent<MvpPlayerMotor>();Need(motor&&!motor.enabled,"Existing disabled legacy motor used only for input capture");
                var pca=bridge.BoundAccessor;Need(ReferenceEquals(PlayerController.Instance,bridge.BoundPlayer)&&ReferenceEquals(PlayerController.Instance.Pca,pca),"Actual singleton actor/PCA identity");
                report.startingRp=GameState.GameStateData.CurrentRp;report.startingEarned=GameState.GameStateData.CurrentEarnThisTimeRp;Need(report.startingRp==0&&report.startingEarned==0,"No owned funding or progress");
                InitializeOriginalRing();
                pca.PlayerState.Initialize();report.originalStateInitialize=true;pca.PlayerMoveCalculator.Initialize(pca);report.originalMovementInitialize=true;pca.PlayerMoveCalculator.OnWarp();report.originalOnWarp=true;pca.PlayerClothesAction.Initialize(pca);report.originalClothesActionInitialize=true;pca.PlayerStrangenessController.Initialize(pca);report.originalStrangenessInitialize=true;
                var manager=InGameManager.Instance.InGameReferencer.NpcManager;manager.Initialize(bridge.BoundPlayer);report.originalNpcManagerInitialize=true;
                Need(typeof(ExposureUnnoticed2.Object3D.Player.Scripts.Other.PlayerMoveCalculator).GetField("path",Flags).GetValue(pca.PlayerMoveCalculator)!=null,"Original Initialize allocated actual NavMeshPath");
                report.startFrame=Time.frameCount;report.editorSHA=Hash(typeof(PlayerController).Assembly.Location);contract=JsonUtility.FromJson<Contract>(File.ReadAllText(Path.Combine(Project,ContractPath)));report.playerSHA=Hash(contract.playerGame.path);Need(report.editorSHA==contract.editorGame.sha256,"Exact loaded Editor image");report.phase="observing";Save();
            }catch(Exception e){Finish(e);}
        }
        static void InitializeOriginalRing()
        {
            var assets=ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
            Need(assets&&assets.MSkill&&AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(assets.MSkill))=="ce0f23f5178eab04fa30015550041c59","Actual original skill master asset");
            Need(GameState.GameStateData!=null&&GameState.OptionData!=null,"Existing actual new-game state/options");
            var all=ExposureUnnoticed2.Master.Skill.MSkill.GetAll();Need(all!=null&&all.Count>23,"Original master GetAll initialized records and pose list");
            ExposureUnnoticed2.Scripts.Base.Localizer.LoadLocalizeDataIfNot();
            var ring=UnityEngine.Object.FindObjectsOfType<ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView>(true).Single();
            Need(!ring.gameObject.activeInHierarchy&&ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView.Instance==null,"Inactive original UI tree before complete original startup");
            var shortcuts=(ExposureUnnoticed2.ObjectUI.InGame.RingMenu.ShortcutItemView[])ring.GetType().GetField("shortcutItemViews",Flags).GetValue(ring);
            Need(shortcuts!=null&&shortcuts.Length==8&&shortcuts.All(x=>x&&x.GetComponent<Animator>()),"Eight original serialized shortcut/Animator inputs");
            foreach(var key in new[]{14,23,27,28,1018,1019,1020,1021})Need(ExposureUnnoticed2.Scripts.Base.InputManager.GetInputAction((ExposureUnnoticed2.Scripts.Base.InputManager.InputType)key)!=null,"Real original Input action "+key);
            var beforeModels=(ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuItemModel[])ring.GetType().GetField("commonRingMenuModel",Flags).GetValue(ring);
            Need(beforeModels!=null&&beforeModels.Length==8,"Original constructor common models before startup");var originalEight=beforeModels.ToArray();
            ring.Initialize();
            Need(ReferenceEquals(ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView.Instance,ring)&&!ring.IsOpenRing,"Complete original Initialize assigned the real Ring instance");
            var common=(ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuItemModel[])ring.GetType().GetField("commonRingMenuModel",Flags).GetValue(ring);
            Need(common!=null&&common.Length==31,"Original Initialize merged eight common and twenty-three pose models");
            for(int i=0;i<8;i++)Need(ReferenceEquals(originalEight[i],common[i]),"Original constructor model identity preserved "+i);
            for(int i=8;i<31;i++)Need(common[i]!=null&&common[i].Index==i&&(int)common[i].RingItemType==10000+i-8&&common[i].ReleaseCondition!=null,"Original merged pose model "+i);
            // The original unused poseRingMenuModel field stays declared and protected;
            // Initialize neither assigns nor reads it.
            report.originalRingInitialize=true;report.inactiveOriginalUi=true;report.ringShortcutCount=shortcuts.Length;report.commonRingModels=common.Length;report.poseRingModels=common.Length-8;Save();
        }
        static void Observe()
        {
            if(finished||!Armed)return;Load();try{
                if(report.phase!="observing")return;Need(string.IsNullOrEmpty(bridge.Failure)&&bridge.Ready,"Original frame bridge remains ready");var p=bridge.BoundAccessor;var cal=p.PlayerMoveCalculator;var pos=p.Transform.position;
                report.samples.Add(new Sample{frame=Time.frameCount,updateCalls=bridge.OriginalUpdateCalls,fixedCalls=bridge.OriginalFixedCalls,x=pos.x,y=pos.y,z=pos.z,realtime=cal.JustMoveAmountRealtime,just=cal.JustMoveAmount,last=cal.LastUpdateJustMoveAmount,cool=(float)cal.GetType().GetField("calcCoolUTime",Flags).GetValue(cal),captured=motor.ControlsCaptured});
                int elapsed=Time.frameCount-report.startFrame;if(elapsed>=4){motor.ControlsCaptured=true;InputSystem.QueueStateEvent(Keyboard.current,elapsed<94?new KeyboardState(global::UnityEngine.InputSystem.Key.W):new KeyboardState());}
                if(elapsed<154){Save();return;}Need(report.domainReloadObserved&&report.actualPlayMode,"Real PlayMode/domain reload");Need(report.samples.Last().updateCalls>report.samples.First().updateCalls+100,"Original main receives natural updates");Need(report.samples.Any(s=>s.realtime>0f),"Original movement producer observed real displacement");Need(report.samples.All(s=>!float.IsNaN(s.realtime)&&!float.IsInfinity(s.realtime)),"Finite original realtime quantities");Need(report.engineErrors.Count==0,"No engine errors during actual frames");Finish(null);
            }catch(Exception e){Finish(e);}
        }
        static void Deadline(){if(!Armed||finished)return;Load();if(DateTime.UtcNow.Ticks>long.Parse(SessionState.GetString(Key+"deadline","0")))Finish(new TimeoutException("Actual frame probe180s deadline"));}
        static void Log(string text,string stack,LogType type){if(!Armed||!EditorApplication.isPlaying||finished)return;Load();if(type==LogType.Error||type==LogType.Exception||type==LogType.Assert){report.engineErrors.Add(type+": "+text+"\n"+stack);Save();}}
        static void Write(){File.WriteAllText(Path.Combine(SessionState.GetString(Key+"output",""),"player-core-frames-v2.json"),JsonUtility.ToJson(report,true)+"\n");}
        static void Finish(Exception e)
        {
            if(finished)return;finished=true;Load();report.phase="finished";report.failure=e==null?null:e.ToString();report.passed=e==null;report.endFrame=Time.frameCount;if(GameState.GameStateData!=null){report.endingRp=GameState.GameStateData.CurrentRp;report.endingEarned=GameState.GameStateData.CurrentEarnThisTimeRp;}
            report.scope=new[]{"Actual existing original actor with original State/MoveCalculator/ClothesAction/Strangeness/NpcManager initializers; complete original Ring.Initialize runs on the real inactive InGameCanvas/Shortcut tree after original MSkill.GetAll. Other UIManager/window/render/input-menu lifecycle is outside this isolated probe.","Input uses actual paired virtual Keyboard W events and existing capture flag; real original Player.OnUpdate computes movement quantities. No calculator per-frame calls, distance/progress/RP injection or Game hooks.","Original navigation APIs execute against current scene data; a pass would not prove a complete world NavMesh, NPC detection, clothing event delivery, tasks/rewards or Player publication."};Save();string output=SessionState.GetString(Key+"output","");if(Directory.Exists(output))Write();if(EditorApplication.isPlayingOrWillChangePlaymode)EditorApplication.isPlaying=false;else{SessionState.SetBool(Key+"armed",false);EditorApplication.delayCall+=()=>EditorApplication.Exit(e==null?0:6);}
        }
    }
}
#endif
