using System;using System.Collections.Generic;using System.IO;using System.Linq;using System.Reflection;using System.Security.Cryptography;
using ExposureUnnoticed2.Master.Common;using ExposureUnnoticed2.Object3D.IngameManager;using ExposureUnnoticed2.Object3D.Player.Scripts;using ExposureUnnoticed2.Scripts.Base;using ExposureUnnoticed2.Scripts.InGame;using UnityEngine;
namespace SfmMvp{
// Owned input/resource binding and explicit scheduling. Original bodies compute
// every customization result. Earlier published Players remain immutable.
[DefaultExecutionOrder(0)]public sealed class BodyRuntimeData:MonoBehaviour{
 public OriginalMovementMvpBridge bridge;public MCustomizeColor colors;public string[] boneNames;public Transform[] bones;public SkinnedMeshRenderer body,face;public Material[] bodyMaterials,faceMaterials;
 public bool Ready{get;private set;}public string Failure{get;private set;}public int ApplyCount{get;private set;}public int LateCount{get;private set;}public float DefaultBaseScale{get;private set;}public string GameSHA{get;private set;}
 public PlayerClassAccessor Accessor=>bridge.BoundAccessor;public PlayerBodyCustomizeAdjuster Adjuster=>Accessor.PlayerBodyCustomizeAdjuster;
 [Serializable]sealed class Entry{public string type,method,il_sha256;}
 [Serializable]sealed class Pins{public string schema,actual_Player_DLL_sha256,complete_Game_review_sha256;public Entry[] methods;}
 [Serializable]sealed class FramePins{public string actual_Player_DLL_sha256,complete_Game_review_sha256;}
 const BindingFlags F=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static;
 readonly List<Material> owned=new List<Material>();bool initialized;
 static void Need(bool b,string n){if(!b)throw new Exception(n);}
 static string Hash(byte[] bytes){using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(bytes)).Replace("-","");}
 void CheckPins(){
  var p=JsonUtility.FromJson<Pins>(File.ReadAllText(Path.Combine(Application.dataPath,"owned-body-seven-method-pins.json")));
  var frame=JsonUtility.FromJson<FramePins>(File.ReadAllText(Path.Combine(Application.dataPath,"owned-player-frame-method-pins.json")));
  GameSHA=Hash(File.ReadAllBytes(typeof(GameStateFacade).Assembly.Location));Need(p.schema=="OwnedBodyRuntimePins/v1"&&p.actual_Player_DLL_sha256==GameSHA&&frame.actual_Player_DLL_sha256==GameSHA&&p.complete_Game_review_sha256==frame.complete_Game_review_sha256,"Body and frame bind the same full reviewed actual Player");
  var expected=new HashSet<string>(new[]{"PlayerBodyCustomizeAdjuster:CalcFatVector","PlayerBodyCustomizeAdjuster:ScaleVectorY","PlayerBodyCustomizeAdjuster:OnChangeBody","PlayerBodyCustomizeAdjuster:Calc1stShadeColor","PlayerBodyCustomizeAdjuster:OnLateUpdate","PlayerTitController:ChikubiUpdate","PlayerTitController:OnChangeCostumeCosplayEvent"});
  Need(p.methods!=null&&p.methods.Length==7,"Exactly seven approved body/dependency pins");foreach(var e in p.methods){Need(expected.Remove(e.type+":"+e.method),"Exact body pin identity");var t=typeof(PlayerBodyCustomizeAdjuster).Assembly.GetType("ExposureUnnoticed2.Object3D.Player.Scripts."+e.type,true);var method=t.GetMethod(e.method,F);Need(method!=null&&method.GetMethodBody()!=null,"Original method present");var il=method.GetMethodBody().GetILAsByteArray();Need(il.Length>3&&Hash(il)==e.il_sha256,"Complete actual body pin "+e.type+":"+e.method);}Need(expected.Count==0,"All original body/dependency pins present");
 }
 Material[] Instance(Material[] sources){return sources.Select(m=>{Need(m&&m.shader,"Original material input");var copy=new Material(m){name="Owned original body material: "+m.name};owned.Add(copy);return copy;}).ToArray();}
 void Start(){try{
  Need(bridge&&bridge.Ready&&bridge.UsesOriginalConstructors,"Actual original actor constructors ready");CheckPins();Need(colors&&colors.SkinPattern.Length==19&&colors.EyeTextures.Length==50&&colors.EyeEmitTextures.Length==50,"Original customization palette");
  Need(boneNames!=null&&bones!=null&&boneNames.Length==21&&bones.Length==21&&bones.All(x=>x)&&bones.Distinct().Count()==21,"Twenty-one actual original bones");Need(body&&face&&body.sharedMesh.blendShapeCount==42&&face.sharedMesh.blendShapeCount==473&&bodyMaterials.Length==2&&faceMaterials.Length==7,"Actual original meshes and material slots");
  var a=Accessor.PlayerAvatarObjectReferencer;for(int i=0;i<bones.Length;i++){var field=typeof(PlayerAvatarObjectReferencer).GetField(boneNames[i],F);Need(field!=null&&field.FieldType==typeof(Transform),"Original transform field "+boneNames[i]);field.SetValue(a,bones[i]);}
  a.bodyMeshRenderer=body;a.FaceMesh=face;body.materials=Instance(bodyMaterials);face.materials=Instance(faceMaterials);
  AssetReferencer.Instance.MCustomizeColor=colors;Need(MCustomizeColor.Instance==colors,"Fresh original palette singleton; no replacement of an already cached fixture");Need(Accessor.PlayerBodyMaterialController.SweatBodyMaterial&&Accessor.PlayerBodyMaterialController.SweatBodyMaterial.HasProperty("_BaseColor"),"Real original sweat receiver");
  DefaultBaseScale=Accessor.PlayerAnimationReceiver.transform.localScale.x;Accessor.PlayerTitController.Initialize(Accessor);Adjuster.Initialize(Accessor);initialized=true;Apply();Ready=true;
 }catch(Exception e){Failure=e.ToString();Debug.LogError("SFM_BODY_RUNTIME_INIT "+Failure);}}
 public void Apply(){Need(initialized,"Original body service initialized");EventManager.Broadcast(new ChangeBodyEvent());ApplyCount++;}
 public void SetInt(string name,int value){var field=typeof(PlayerCustomData).GetField(name,F);Need(field!=null&&field.FieldType==typeof(int),"Original integer customization field");field.SetValue(GameState.PlayerCustomData,value);Apply();}
 public int GetInt(string name){return(int)typeof(PlayerCustomData).GetField(name,F).GetValue(GameState.PlayerCustomData);}
 public void SetAdjustHeight(bool value){GameState.PlayerCustomData.CustomizeAdjustHeight=value;Apply();}
 public void ResetOriginalDefaults(){GameStateFacade.SetDefaultCustomBody();GameStateFacade.SetDefaultCustomFace();Apply();}
 void LateUpdate(){if(!Ready)return;try{Adjuster.OnLateUpdate();LateCount++;}catch(Exception e){Failure=e.ToString();Ready=false;Debug.LogError("SFM_BODY_RUNTIME_LATE "+Failure);}}
 void OnDestroy(){if(initialized){Adjuster.Dispose();Accessor.PlayerTitController.OnDispose();}foreach(var m in owned)if(m)Destroy(m);}
}
}
