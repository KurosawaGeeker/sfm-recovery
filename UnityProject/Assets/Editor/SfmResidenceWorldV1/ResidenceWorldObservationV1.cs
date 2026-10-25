#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ExposureUnnoticed2.Object3D.IngameManager;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Object3D.RoutePoint;
using ExposureUnnoticed2.Scripts.InGame;
namespace SfmMvp.Editor
{
    // Read-only original world lifecycle observation. No Game calls/ticks, input,
    // saved state, actor/route construction or NavMesh changes are performed.
    [InitializeOnLoad]public static class ResidenceWorldObservationV1
    {
        const string Project="E:/ACTgame/SFM/ReverseEngineering/tools/unity-editor/core-gameplay-bootstrap-runtime-candidate-v2/Project";
        const string Key="SfmResidenceWorldV1.";
        static readonly string Domain=Guid.NewGuid().ToString("N");
        [Serializable]public sealed class Sample
        {
            public int frame,managers,players,stages,stageType,currentSavedStage,npcManagers,npcs,npcDictionary,sceneNpcComponents,resourceAssignments,missionEntries;
            public bool adapterReady,playerBound,stageBound,routeInstance,stageInitialized,npcInitialized,npcLoadedData;
            public string routeType;public float x,y,z;public double rp,earnedRp;
        }
        [Serializable]public sealed class Report
        {
            public bool passed,actualPlayMode,domainReloadObserved,normalStartupAccepted,npcWorldGameplayAccepted=false,naturalPortalTransitionAccepted=false;
            public bool GameHooks=false,manualGameTicks=false,ownedInput=false,ownedFunding=false,ownedProgress=false,ownedSpawn=false,ownedRouteInitialization=false,navMeshModified=false,originalSavesAccessed=false;
            public string failure,beginDomain,playDomain,editorGameSHA,sceneSHA,contractSHA,persistentDataPath;
            public List<string> errorLifecycleSnapshots=new List<string>();public int checks;public List<Sample> samples=new List<Sample>();public List<string> errors=new List<string>();public List<string> transitionErrors=new List<string>();
        }
        static Report report;static bool finished;static int previous=-1;
        static bool Armed=>SessionState.GetBool(Key+"armed",false);
        static string Arg(string name){var a=Environment.GetCommandLineArgs();int k=Array.IndexOf(a,name);if(k<0||k+1>=a.Length)throw new InvalidOperationException(name);return a[k+1];}
        static string Hash(string p){using(var s=File.OpenRead(p))using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(s)).Replace("-","");}
        static object Field(object v,string n)=>v==null?null:v.GetType().GetField(n,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance)?.GetValue(v);
        static void Load(){if(report==null)report=JsonUtility.FromJson<Report>(SessionState.GetString(Key+"report","{}"));}
        static void Save(){SessionState.SetString(Key+"report",JsonUtility.ToJson(report));}
        static void Need(bool b,string label){report.checks++;if(!b)throw new InvalidOperationException(label);}
        static ResidenceWorldObservationV1(){EditorApplication.update+=Poll;EditorApplication.playModeStateChanged+=Changed;Application.logMessageReceived+=Log;}
        public static void Begin()
        {
            if(Armed)throw new InvalidOperationException("Already armed");report=new Report{beginDomain=Domain};
            try
            {
                string output=Path.GetFullPath(Arg("-sfmResidenceWorldOutput"));Need(!Directory.Exists(output)&&output.StartsWith(Path.GetFullPath(Path.Combine(Project,"../runs"))+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase),"Fresh owned world report");Directory.CreateDirectory(output);SessionState.SetString(Key+"output",output);Save();
                Need(Path.GetFullPath(Path.Combine(Application.dataPath,".."))==Path.GetFullPath(Project),"Independent Project");Need(!EditorApplication.isPlayingOrWillChangePlaymode&&!EditorSettings.enterPlayModeOptionsEnabled,"Default domain reload");
                report.contractSHA=Arg("-sfmResidenceWorldContractSHA");Need(Hash(Arg("-sfmResidenceWorldContract"))==report.contractSHA,"Exact externally pre/post-verified complete input contract");
                Need(Application.companyName=="SFMRecovery"&&Application.productName=="SFMOriginalGameplayRuntimeV2","Owned persistent identity");report.persistentDataPath=Application.persistentDataPath;
                report.editorGameSHA=Hash(typeof(InGameManager).Assembly.Location);Need(report.editorGameSHA=="C04085FCED40784695E8FD59CB923E7D8A570BB2DB4B4DD14970C9A44B2E06BD","Exact all-original-linked Root90 Game");
                string scene="Assets/SfmOriginalResidenceV2/Night_Demo.unity";report.sceneSHA=Hash(Path.Combine(Project,scene));Need(report.sceneSHA=="A9697572042FADC831D007DE31124F01E4572EFF33A921297FC5CE444B7E1A50","Exact original scene plus3 owned resource docs");
                foreach(string flag in new[]{"-sfmCoreSmoke","-sfmEquipmentSmoke","-sfmOriginalActionSmoke","-sfmMvpOriginalBundle","-sfmNpcAnimationProbeV1"})Need(Array.IndexOf(Environment.GetCommandLineArgs(),flag)<0,"No alternate gameplay entry "+flag);
                SessionState.SetString(Key+"deadline",DateTime.UtcNow.AddSeconds(180).Ticks.ToString());SessionState.SetBool(Key+"finished",false);Save();SessionState.SetBool(Key+"armed",true);EditorSceneManager.OpenScene(scene,OpenSceneMode.Single);EditorApplication.isPlaying=true;
            }
            catch(Exception e){Finish(e);}
        }
        static void Log(string message,string stack,LogType type)
        {
            if(!Armed||finished||(type!=LogType.Exception&&type!=LogType.Error&&type!=LogType.Assert))return;Load();var list=EditorApplication.isPlaying?report.errors:report.transitionErrors;list.Add(type+": "+message+"\n"+stack);if(EditorApplication.isPlaying){try{var ms=UnityEngine.Object.FindObjectsOfType<InGameManager>();var ds=UnityEngine.Object.FindObjectsOfType<EventMethodManager>();report.errorLifecycleSnapshots.Add("frame="+Time.frameCount+" managerInstance="+(bool)InGameManager.Instance+" managers="+ms.Length+" managerIR="+(ms.Length==1&&(bool)ms[0].InGameReferencer)+" drivers="+ds.Length+" driverIR="+string.Join(",",ds.Select(d=>Field(d,"inGameReferencer")!=null?"bound":"null")));}catch(Exception e){report.errorLifecycleSnapshots.Add(e.ToString());}}Save();
        }
        static void Poll()
        {
            if(!Armed||finished)return;Load();
            try
            {
                Need(DateTime.UtcNow.Ticks<long.Parse(SessionState.GetString(Key+"deadline","0")),"Original world observation deadline");if(!EditorApplication.isPlaying||EditorApplication.isPaused)return;
                int frame=Time.frameCount;if(frame==previous)return;previous=frame;report.actualPlayMode=true;report.playDomain=Domain;report.domainReloadObserved=Domain!=report.beginDomain;
                var managers=UnityEngine.Object.FindObjectsOfType<InGameManager>();var players=UnityEngine.Object.FindObjectsOfType<PlayerController>();var stages=UnityEngine.Object.FindObjectsOfType<StageController>();var nm=UnityEngine.Object.FindObjectsOfType<NpcManager>();var adapters=UnityEngine.Object.FindObjectsOfType<OriginalGameplayBootstrapV2>();
                var s=new Sample{frame=frame,managers=managers.Length,players=players.Length,stages=stages.Length,npcManagers=nm.Length,sceneNpcComponents=UnityEngine.Object.FindObjectsOfType<NpcComponent>().Length};
                if(adapters.Length==1){s.adapterReady=adapters[0].Ready;s.resourceAssignments=adapters[0].AppliedBindings+adapters[0].ExistingBindings;}
                if(stages.Length==1){s.stageType=(int)stages[0].StageType;s.stageInitialized=Field(stages[0],"isAlreadyInit")is bool b&&b;}
                if(managers.Length==1){var ir=managers[0].InGameReferencer;s.playerBound=ir&&ir.Player&&players.Length==1&&ReferenceEquals(ir.Player,players[0]);s.stageBound=stages.Length==1&&ReferenceEquals(managers[0].CurrentStageController,stages[0]);}
                if(players.Length==1){var pos=players[0].transform.position;s.x=pos.x;s.y=pos.y;s.z=pos.z;}
                s.routeInstance=BaseRouteBuilder.Instance;if(s.routeInstance)s.routeType=BaseRouteBuilder.Instance.GetType().FullName;
                if(nm.Length==1){s.npcs=nm[0].ExistNpcList.Count;s.npcDictionary=nm[0].ExistNpcGameObjectDict.Count;s.npcInitialized=Field(nm[0],"initialized")is bool b&&b;s.npcLoadedData=Field(nm[0],"isLoadedData")is bool l&&l;}
                var data=GameState.GameStateData;if(data!=null){s.currentSavedStage=(int)data.CurrentStageType;s.rp=data.CurrentRp;s.earnedRp=data.CurrentEarnThisTimeRp;s.missionEntries=data.MissionMaxRpBonus100Dict.Count;}
                report.samples.Add(s);Save();
                if(report.errors.Count>0)throw new InvalidOperationException("Original Residence lifecycle emitted an engine error; full first error retained");
                if(frame>=120)
                {
                    Need(report.actualPlayMode&&report.domainReloadObserved,"Original PlayMode/domain reload");Need(s.adapterReady&&s.resourceAssignments==25,"Original Title resource adapter completed");Need(s.managers==1&&s.players==1&&s.playerBound,"Original manager Start bound real Player");Need(s.stages==1&&s.stageType==10&&s.stageBound&&s.stageInitialized,"Original Residence lifecycle initialized Stage10");Need(s.npcManagers==1&&s.npcInitialized,"Original manager initialized NPC manager");Need(s.missionEntries==196&&s.rp==0&&s.earnedRp==0,"Original new-game196 missions and zero funding");
                    report.normalStartupAccepted=true;report.passed=true;Finish(null);
                }
            }
            catch(Exception e){Finish(e);}
        }
        static void Write(){string output=SessionState.GetString(Key+"output","");if(output.Length>0)File.WriteAllText(Path.Combine(output,"observation.json"),JsonUtility.ToJson(report,true)+"\n");}
        static void Finish(Exception e){if(finished)return;finished=true;SessionState.SetBool(Key+"finished",true);if(e!=null){report.failure=e.ToString();report.passed=false;Debug.LogError("SFM_RESIDENCE_WORLD_V1_FAILURE: "+e);}Save();Write();if(EditorApplication.isPlayingOrWillChangePlaymode)EditorApplication.isPlaying=false;else EditorApplication.delayCall+=()=>EditorApplication.Exit(report.passed?0:6);}
        static void Changed(PlayModeStateChange state){if(!Armed)return;Load();if(state==PlayModeStateChange.EnteredEditMode){SessionState.SetBool(Key+"armed",false);if(!SessionState.GetBool(Key+"finished",false)){report.failure="Unexpected PlayMode exit";report.passed=false;}Write();EditorApplication.delayCall+=()=>EditorApplication.Exit(report.passed?0:6);}}
    }
}
#endif
