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
using ExposureUnnoticed2.Object3D.RoutePoint;
using ExposureUnnoticed2.Scripts.InGame;
namespace SfmMvp.Editor
{
    // Owned, read-only EditMode inventory. Does not invoke gameplay initialization.
    public static class ResidenceSceneReadinessV2
    {
        const string Project="E:/ACTgame/SFM/ReverseEngineering/tools/unity-editor/core-gameplay-bootstrap-runtime-candidate-v2/Project";
        const string Scene="Assets/SfmOriginalResidenceV1/Night_Demo.unity";
        [Serializable]public sealed class ReferenceRow {public string owner,field,type,guid,path;public long fileID;public bool present;}
        [Serializable]public sealed class ComponentRow {public string name,type;public Vector3 position;public Quaternion rotation;public int descendants;}
        [Serializable]public sealed class Report
        {
            public bool passed,actualPlayMode=false,gameplayStateWritten=false,manualGameInitialization=false,navMeshBaked=false,sceneSaved=false;
            public string failure,unity,scene,sceneSHA,editorGameSHA,persistentDataPath,routeType;
            public int checks,rootObjects,gameObjects,missingScripts,originalManagers,originalReferencers,originalAssetReferencers,stages,portals,routeBuilders,resourceBindings,navSurfaces,navDataReferences;
            public List<ReferenceRow> references=new List<ReferenceRow>();public List<ComponentRow> components=new List<ComponentRow>();
        }
        static Report report;
        static void Need(bool v,string label){report.checks++;if(!v)throw new InvalidOperationException(label);}
        static string Hash(string p){using(var s=File.OpenRead(p))using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(s)).Replace("-","");}
        static string Arg(string n){var a=Environment.GetCommandLineArgs();int k=Array.IndexOf(a,n);if(k<0||k+1>=a.Length)throw new InvalidOperationException(n);return a[k+1];}
        static void Ref(string owner,string field,UnityEngine.Object value)
        {
            var r=new ReferenceRow{owner=owner,field=field,present=value,type=value?value.GetType().FullName:null};
            if(value){string g;long id;if(AssetDatabase.TryGetGUIDAndLocalFileIdentifier(value,out g,out id)){r.guid=g;r.fileID=id;r.path=AssetDatabase.GetAssetPath(value);}}
            report.references.Add(r);
        }
        static ComponentRow Comp(Component c){return new ComponentRow{name=c.name,type=c.GetType().FullName,position=c.transform.position,rotation=c.transform.rotation,descendants=c.GetComponentsInChildren<Transform>(true).Length};}
        public static void Run()
        {
            report=new Report{unity=Application.unityVersion,scene=Scene};string output=Path.GetFullPath(Arg("-sfmResidenceReadinessOutput"));
            Need(!File.Exists(output)&&output.StartsWith(Path.GetFullPath(Path.Combine(Project,"../runs"))+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase),"Fresh owned report path");Directory.CreateDirectory(Path.GetDirectoryName(output));
            try
            {
                Need(Path.GetFullPath(Path.Combine(Application.dataPath,".."))==Path.GetFullPath(Project),"Independent Project");
                Need(!EditorApplication.isPlayingOrWillChangePlaymode,"Read-only EditMode");
                Need(Application.companyName=="SFMRecovery"&&Application.productName=="SFMOriginalGameplayRuntimeV2","Owned persistent identity");
                report.persistentDataPath=Application.persistentDataPath;report.editorGameSHA=Hash(typeof(InGameManager).Assembly.Location);
                Need(report.editorGameSHA=="C04085FCED40784695E8FD59CB923E7D8A570BB2DB4B4DD14970C9A44B2E06BD","Previously all-original-linked Root90 Editor Game unchanged");
                report.sceneSHA=Hash(Path.Combine(Project,Scene));Need(report.sceneSHA==Arg("-sfmResidenceSceneSHA"),"Exact append-only derived scene");
                var scene=EditorSceneManager.OpenScene(Scene,OpenSceneMode.Single);
                var roots=scene.GetRootGameObjects();var objects=roots.SelectMany(x=>x.GetComponentsInChildren<Transform>(true)).Select(x=>x.gameObject).ToArray();report.rootObjects=roots.Length;report.gameObjects=objects.Length;
                report.missingScripts=objects.Sum(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount);Need(report.missingScripts==0,"No missing original scene scripts");
                report.originalManagers=objects.Sum(x=>x.GetComponents<InGameManager>().Length);report.originalReferencers=objects.Sum(x=>x.GetComponents<InGameReferencer>().Length);report.originalAssetReferencers=objects.Sum(x=>x.GetComponents<AssetReferencer>().Length);
                Need(report.originalManagers==0&&report.originalReferencers==0&&report.originalAssetReferencers==0,"Original Residence relies on original Resources system/manager prefabs");
                var stages=objects.SelectMany(x=>x.GetComponents<StageController>()).ToArray();report.stages=stages.Length;Need(stages.Length==1&&(int)stages[0].StageType==10,"Unique original Residence stage 10");
                report.components.Add(Comp(stages[0]));
                var routes=objects.SelectMany(x=>x.GetComponents<BaseRouteBuilder>()).ToArray();report.routeBuilders=routes.Length;Need(routes.Length==1,"Unique original route builder");report.routeType=routes[0].GetType().FullName;report.components.Add(Comp(routes[0]));
                var routeField=typeof(StageController).GetField("baseRouteBuilder",BindingFlags.NonPublic|BindingFlags.Instance);Need(ReferenceEquals(routeField.GetValue(stages[0]),routes[0]),"Stage original route reference");
                var portalManagers=objects.SelectMany(x=>x.GetComponents<PortalManager>()).ToArray();Need(portalManagers.Length==1&&ReferenceEquals(stages[0].PortalManager,portalManagers[0]),"Original portal manager stage binding");
                var portals=objects.SelectMany(x=>x.GetComponents<ExposureUnnoticed2.Object3D.Portal.PortalController>()).ToArray();report.portals=portals.Length;Need(portals.Length>0,"Original portals present without manual Initialize");
                foreach(var p in portals)report.components.Add(Comp(p));
                var adapters=objects.SelectMany(x=>x.GetComponents<OriginalGameplayBootstrapV2>()).ToArray();Need(adapters.Length==1,"Only accepted original resource adapter");var adapter=adapters[0];
                Need(objects.All(x=>x.GetComponent<MvpPlayerMotor>()==null&&x.GetComponent<OriginalMovementMvpBridge>()==null),"No legacy gameplay bridge");
                var reviewed=TitleModelsWhitelistV1.LoadReviewedBindings();Need(adapter.bindings!=null&&adapter.bindings.Length==25&&reviewed.Length==25,"Exact original Title resource count");report.resourceBindings=25;
                var system=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/Resources/SystemManager.prefab");Need(system,"Original System prefab");var ar=system.GetComponentsInChildren<AssetReferencer>(true);Need(ar.Length==1,"Unique original System prefab AR");
                for(int i=0;i<reviewed.Length;i++)
                {
                    var a=adapter.bindings[i];var r=reviewed[i];Need(a.originalBundleKey==r.originalBundleKey&&a.originalContainer==r.originalContainer&&a.originalGuid==r.originalGuid&&a.field==r.field&&a.index==r.index&&ReferenceEquals(a.asset,r.asset),"Exact reviewed resource identity "+i);
                    var f=typeof(AssetReferencer).GetField(a.field,BindingFlags.Public|BindingFlags.Instance);var before=a.index<0?(UnityEngine.Object)f.GetValue(ar[0]):(UnityEngine.Object)((Array)f.GetValue(ar[0])).GetValue(a.index);
                    Ref("SystemManager.AssetReferencer",a.field+":"+a.index,before);Ref("CopiedAdapter.ExpectedBefore",a.field+":"+a.index,a.expectedBefore);Need(before==a.expectedBefore,"Original System serialized before-reference (Unity object equality) "+a.field+":"+a.index);
                }
                var manager=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObject/InGameManager.prefab");Need(ReferenceEquals(adapter.originalInGameManagerPrefab,manager),"Exact original manager template");
                Need(manager.GetComponentsInChildren<InGameManager>(true).Length==1,"One original manager in template");var ir=manager.GetComponentsInChildren<InGameReferencer>(true);Need(ir.Length==1,"One original IR in template");
                foreach(var f in typeof(InGameReferencer).GetFields(BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic).Where(f=>(f.IsPublic||f.IsDefined(typeof(SerializeField),true))&&typeof(UnityEngine.Object).IsAssignableFrom(f.FieldType)))
                {
                    var v=(UnityEngine.Object)f.GetValue(ir[0]);Ref("InGameManager.InGameReferencer",f.Name,v);bool optional=f.Name=="<Player>k__BackingField"||f.Name=="<ClothesPathSearchAgent>k__BackingField";Need(v||optional,"Original required IR reference "+f.Name);
                }
                foreach(var c in objects.SelectMany(x=>x.GetComponents<Component>()).Where(x=>x&&x.GetType().Name=="NavMeshSurface"))
                {
                    report.navSurfaces++;report.components.Add(Comp(c));var so=new SerializedObject(c);var prop=so.FindProperty("m_NavMeshData");Need(prop!=null,"Original serialized nav data field");var data=prop.objectReferenceValue;Ref(c.name,"m_NavMeshData",data);if(data)report.navDataReferences++;
                }
                Need(report.navSurfaces>0&&report.navDataReferences>0,"Original nav surface/data exist; no rebake/pose changes");
                Need(report.sceneSHA==Hash(Path.Combine(Project,Scene)),"Scene bytes unchanged after read-only import/open");
                Need(report.editorGameSHA==Hash(typeof(InGameManager).Assembly.Location),"Game assembly unchanged");report.passed=true;
            }
            catch(Exception e){report.failure=e.ToString();Debug.LogError("SFM_RESIDENCE_READINESS_V2 "+e);}
            File.WriteAllText(output,JsonUtility.ToJson(report,true)+"\n");Debug.Log("SFM_RESIDENCE_READINESS_V2 passed="+report.passed+" route="+report.routeType+" objects="+report.gameObjects);
            if(!report.passed)throw new InvalidOperationException(report.failure);
        }
    }
}
#endif
