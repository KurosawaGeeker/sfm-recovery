#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ExposureUnnoticed2.Object3D.RoutePoint;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UObj=UnityEngine.Object;

namespace SfmMvp.Editor {
// Owned engine input audit. It never invokes Game initialization or changes any
// route point, Game field, NavMeshData, or original Surface lifecycle method.
public static class NpcWorldInputAudit {
 const string Stage=@"E:\ACTgame\SFM\ReverseEngineering\tools\unity-editor\npc-core-runtime-candidate-v1";
 const string Inputs=@"E:\ACTgame\SFM\ReverseEngineering\tools\unity-editor\npc-world-input-preparation-v1";
 [Serializable] sealed class V {public float x,y,z,w;public Vector3 v=>new Vector3(x,y,z);public Quaternion q=>new Quaternion(x,y,z,w);}
 [Serializable] sealed class Tr {public long fileID,gameObjectID,parentID;public V localPosition,localRotation,localScale;public int activeSelf;}
 [Serializable] sealed class Pt {public long fileID,gameObjectID,publisher;public int type,interact,floor;public float param,maxRight;}
 [Serializable] sealed class Expected {public string prefabGUID,navMeshGUID;public long navSurfaceID,navTransformID,routeBuilderID,appearPointID,customerPointID,queuePointID,restockParentID,productParentID;public int restockCount,productCount,originalAgentTypeID,originalAreaMask;public Tr[]transforms;public Pt[]routePoints;}
 [Serializable] sealed class Sample {public long fileID;public Vector3 original,sampled;public bool found;public float distance;public int mask;}
 [Serializable] sealed class PathRow {public long from,to;public bool found;public string status;public Vector3[]corners;}
 [Serializable] sealed class Report {
  public bool passed,inputOnly=true,Game_initialization_invoked,original_surface_lifecycle_invoked,owned_engine_loader=true;
  public int checks,routePoints,originalTransforms,triangles,vertices,areaMask,agentTypeID,restock,products,productPathsComplete,samplesFound;
  public string unity,failure,navMeshAsset,surfaceType;
  public Vector3 surfaceWorldPosition,dataPosition;public Quaternion surfaceWorldRotation,dataRotation;
  public string[]dependencies;public Sample[]samples;public PathRow[]paths;
 }
 static Report r;
 static void Need(bool b,string label){r.checks++;if(!b)throw new InvalidOperationException(label);}
 static object Get(object o,string name){for(Type t=o.GetType();t!=null;t=t.BaseType){var f=t.GetField(name,BindingFlags.Instance|BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.DeclaredOnly);if(f!=null)return f.GetValue(o);}throw new MissingFieldException(o.GetType().FullName,name);}
 static bool Near(Vector3 a,Vector3 b)=>(a-b).sqrMagnitude<=1e-10f;
 static bool Near(Quaternion a,Quaternion b)=>Mathf.Min((new Vector4(a.x-b.x,a.y-b.y,a.z-b.z,a.w-b.w)).sqrMagnitude,(new Vector4(a.x+b.x,a.y+b.y,a.z+b.z,a.w+b.w)).sqrMagnitude)<=1e-10f;
 static long ID(UObj o){var source=PrefabUtility.GetCorrespondingObjectFromSource(o);if(source!=null)o=source;string guid;long id;if(!AssetDatabase.TryGetGUIDAndLocalFileIdentifier(o,out guid,out id))throw new Exception("Original asset local ID unavailable: "+o);return id;}
 public static void Validate(){
  r=new Report{unity=Application.unityVersion};var args=Environment.GetCommandLineArgs();int at=Array.IndexOf(args,"-sfmNpcInputOutput");if(at<0||at+1==args.Length)throw new Exception("Fresh output required");var output=Path.GetFullPath(args[at+1]);if(!output.StartsWith(Stage+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||Directory.Exists(output))throw new Exception("Fresh owned output required");Directory.CreateDirectory(output);
  GameObject root=null;NavMeshDataInstance instance=default;
  try {
   Need(Path.GetFullPath(Path.Combine(Application.dataPath,".."))==Path.Combine(Stage,"Project"),"Fresh owned NPC candidate Project");
   var exp=JsonUtility.FromJson<Expected>(File.ReadAllText(Path.Combine(Inputs,"expected-engine-inputs.json")));
   EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
   Need(NavMesh.CalculateTriangulation().vertices.Length==0,"No preexisting loaded navigation data");
   const string path="Assets/MvpNpcOriginalInputs/OriginalKonbiniRouteForest.prefab";
   Need(AssetDatabase.AssetPathToGUID(path)==exp.prefabGUID,"Exact derived original input GUID");
   var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(path);Need(prefab&&!prefab.activeSelf,"Imported inactive original input forest");
   root=(GameObject)PrefabUtility.InstantiatePrefab(prefab);Need(root&&!root.activeInHierarchy,"Input instance remains inactive");
   var all=root.GetComponentsInChildren<Transform>(true);var transforms=all.ToDictionary(x=>ID(x));var gos=all.ToDictionary(x=>ID(x.gameObject),x=>x.gameObject);
   foreach(var e in exp.transforms){var t=transforms[e.fileID];Need(ID(t.gameObject)==e.gameObjectID,"Original Transform/GameObject identity "+e.fileID);Need(Near(t.localPosition,e.localPosition.v)&&Near(t.localRotation,e.localRotation.q)&&Near(t.localScale,e.localScale.v),"Original local pose "+e.fileID);Need(t.gameObject.activeSelf==(e.activeSelf!=0),"Original activeSelf "+e.fileID);Need(e.parentID==0?t.parent==root.transform:ID(t.parent)==e.parentID,"Original ancestor identity "+e.fileID);}
   r.originalTransforms=exp.transforms.Length;Need(all.Length==r.originalTransforms+1,"No omitted/extra original transforms");
   Need(all.All(x=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(x.gameObject)==0),"All original MonoBehaviour providers resolve");
   var points=root.GetComponentsInChildren<RoutePoint>(true).ToDictionary(x=>ID(x));r.routePoints=points.Count;Need(points.Count==exp.routePoints.Length,"Every original route point imported");
   foreach(var e in exp.routePoints){var p=points[e.fileID];Need(ID(p.gameObject)==e.gameObjectID,"Original RoutePoint/GameObject identity "+e.fileID);Need(Convert.ToInt32(Get(p,"<Type>k__BackingField"))==e.type&&Convert.ToInt32(Get(p,"<RouteInteractType>k__BackingField"))==e.interact&&Convert.ToInt32(Get(p,"<Floor>k__BackingField"))==e.floor&&Convert.ToSingle(Get(p,"<Param>k__BackingField"))==e.param&&Convert.ToSingle(Get(p,"<MaxPathRightScale>k__BackingField"))==e.maxRight,"Original serialized point values "+e.fileID);var publisher=Get(p,"<LinkTokenPublisher>k__BackingField")as UObj;Need(e.publisher==0?publisher==null:publisher&&ID(publisher)==e.publisher,"Original token publisher reference "+e.fileID);}
   var builder=root.GetComponentsInChildren<RouteBuilderConbini>(true).Single();Need(ID(builder)==exp.routeBuilderID,"Exact original builder");var restock=(GameObject)Get(builder,"conbiniShinadashiParent");var products=(GameObject)Get(builder,"productPickParent");Need(restock==gos[exp.restockParentID]&&products==gos[exp.productParentID],"Original typed parent references");
   r.restock=restock.GetComponentsInChildren<RoutePoint>(true).Length;r.products=products.GetComponentsInChildren<RoutePoint>(true).Length;Need(r.restock==exp.restockCount&&r.products==exp.productCount,"Every original restock/product point remains in inactive input forest");Need(restock.GetComponentsInChildren<Transform>(true).All(x=>x.gameObject.activeSelf)&&products.GetComponentsInChildren<Transform>(true).All(x=>x.gameObject.activeSelf),"Original route groups have no inactiveSelf descendants");
   Need(Get(builder,"appearPoint")==points[exp.appearPointID]&&Get(builder,"regiCustomerPoint")==points[exp.customerPointID]&&Get(builder,"regiCustomerJizenPoint")==points[exp.queuePointID],"Original explicit appear/customer/queue reference identity");
   var surface=root.GetComponentsInChildren<MonoBehaviour>(true).Single(x=>ID(x)==exp.navSurfaceID);r.surfaceType=surface.GetType().AssemblyQualifiedName;Need(ID(surface.transform)==exp.navTransformID,"Original NavMeshSurface transform");
   var data=(NavMeshData)Get(surface,"m_NavMeshData");r.navMeshAsset=AssetDatabase.GUIDToAssetPath(exp.navMeshGUID);Need(data&&AssetDatabase.GetAssetPath(data)==r.navMeshAsset,"Original serialized baked navigation data imported");
   r.surfaceWorldPosition=surface.transform.position;r.surfaceWorldRotation=surface.transform.rotation;r.dataPosition=data.position;r.dataRotation=data.rotation;var agentId=new SerializedObject(data).FindProperty("m_AgentTypeID");Need(agentId!=null,"Original serialized agent type is exposed");r.agentTypeID=agentId.intValue;r.areaMask=exp.originalAreaMask;Need(r.agentTypeID==exp.originalAgentTypeID,"Original navigation agent type");
   // Use precisely the original Surface pose, as required by its runtime AddData
   // API. The private serialized data is read directly; recovered plugin stubs
   // are never treated as a functioning Surface loader.
   instance=NavMesh.AddNavMeshData(data,surface.transform.position,surface.transform.rotation);Need(instance.valid,"Engine accepted original baked NavMeshData at original Surface pose");
   var tri=NavMesh.CalculateTriangulation();r.vertices=tri.vertices.Length;r.triangles=tri.indices.Length/3;Need(r.vertices>0&&r.triangles>0,"Engine produced nonempty original navigation geometry");
   var samples=new List<Sample>();foreach(var pair in points.OrderBy(x=>x.Key)){NavMeshHit hit;bool found=NavMesh.SamplePosition(pair.Value.transform.position,out hit,1f,exp.originalAreaMask);samples.Add(new Sample{fileID=pair.Key,original=pair.Value.transform.position,found=found,sampled=found?hit.position:default,distance=found?hit.distance:-1,mask=found?hit.mask:0});}r.samples=samples.ToArray();r.samplesFound=samples.Count(x=>x.found);
   var paths=new List<PathRow>();var destinations=new List<RoutePoint>{points[exp.customerPointID],points[exp.queuePointID]};destinations.AddRange(products.GetComponentsInChildren<RoutePoint>(true));foreach(var dest in destinations){var navPath=new NavMeshPath();bool found=NavMesh.CalculatePath(points[exp.appearPointID].transform.position,dest.transform.position,exp.originalAreaMask,navPath);paths.Add(new PathRow{from=exp.appearPointID,to=ID(dest),found=found,status=navPath.status.ToString(),corners=navPath.corners});}r.paths=paths.ToArray();
   Need(paths[0].found&&paths[0].status==NavMeshPathStatus.PathComplete.ToString(),"Original appear-to-customer complete path");Need(paths[1].found&&paths[1].status==NavMeshPathStatus.PathComplete.ToString(),"Original appear-to-queue complete path");r.productPathsComplete=paths.Skip(2).Count(x=>x.found&&x.status==NavMeshPathStatus.PathComplete.ToString());Need(r.productPathsComplete>0,"Original appear-to-product path exists");
   r.dependencies=AssetDatabase.GetDependencies(path,true);r.passed=true;
  }catch(Exception e){r.failure=e.ToString();}
  finally{if(instance.valid)instance.Remove();if(root)UObj.DestroyImmediate(root);}
  File.WriteAllText(Path.Combine(output,"npc-world-input-import.json"),JsonUtility.ToJson(r,true)+"\n");if(!r.passed)throw new Exception(r.failure);Debug.Log("SFM_NPC_WORLD_INPUT_IMPORT passed=true Game_initialization=false original_surface_lifecycle=false");
 }
}
}
#endif
