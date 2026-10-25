#if UNITY_EDITOR
using System;using System.Collections.Generic;using System.IO;using System.Linq;using System.Reflection;
using ExposureUnnoticed2.Master.Common;using ExposureUnnoticed2.Object3D.Player.Scripts;using UnityEditor;using UnityEditor.SceneManagement;using UnityEngine;
namespace SfmMvp.Editor{
public static class BodyBindingPreflight{
 [Serializable]public class FieldRow{public string field,originalPath,actualPath;public bool found;public string[] blendShapes;public int materialSlots,blendCount;public string[] shaderProperties;}
 [Serializable]public class Report{public bool passed,runtime_body_called,scene_saved,Full_changed,original_saves_accessed;public string scope,avatarPath;public int originalEyeTextures,originalEyeEmitTextures,originalSkinPatterns;public bool originalSkinTextures;public List<FieldRow> fields=new List<FieldRow>();public List<string> gaps=new List<string>();public string failure;}
 static string PathOf(Transform t){var n=new List<string>();for(;t;t=t.parent)n.Add(t.name);n.Reverse();return string.Join("/",n);}
 public static void Inspect(){
  var r=new Report{scope="Read-only actual Unity scene/mesh/material input inspection. No original body event or runtime execution."};
  const string stage=@"E:\ACTgame\SFM\ReverseEngineering\tools\unity-editor\body-runtime-preparation-v1";
  try{
   if(Path.GetFullPath(Path.Combine(Application.dataPath,".."))!=Path.Combine(stage,"Project"))throw new Exception("Owned preparation project required");
   var scene=EditorSceneManager.OpenScene("Assets/MvpGenerated/MvpApartmentPamPlayerV4.unity",OpenSceneMode.Single);
   var bridge=scene.GetRootGameObjects().SelectMany(x=>x.GetComponentsInChildren<OriginalMovementMvpBridge>(true)).Single();
   var candidates=bridge.GetComponentsInChildren<Transform>(true).Where(t=>t.name=="Armature"&&t.Find("Hips/Spine/Chest/Neck/Head")).Select(t=>t.parent).ToArray();
   r.gaps.Add("Original armature candidates: "+string.Join(";",candidates.Select(x=>PathOf(x))));
   if(!bridge.visibleAnimator||!bridge.visibleCoatRenderer||!bridge.visibleCoatRenderer.rootBone)throw new Exception("Existing actual visible actor references required");
   var selected=candidates.Where(x=>bridge.visibleCoatRenderer.rootBone.IsChildOf(x)&&(bridge.visibleAnimator.transform==x||bridge.visibleAnimator.transform.IsChildOf(x)||x.IsChildOf(bridge.visibleAnimator.transform))).ToArray();
   if(selected.Length!=1)throw new Exception("Unique visible-Animator/coat-bone original armature required; found "+selected.Length);
   var root=selected[0];r.avatarPath=PathOf(root);
   string[][] bindings=new[]{new[]{"FootL","Armature/Hips/UpperLeg_L/LowerLeg_L/Foot_L"},new[]{"FootR","Armature/Hips/UpperLeg_R/LowerLeg_R/Foot_R"},new[]{"LegL","Armature/Hips/UpperLeg_L"},new[]{"LegR","Armature/Hips/UpperLeg_R"},new[]{"LowerLegL","Armature/Hips/UpperLeg_L/LowerLeg_L"},new[]{"LowerLegR","Armature/Hips/UpperLeg_R/LowerLeg_R"},new[]{"Head","Armature/Hips/Spine/Chest/Neck/Head"},new[]{"Neck","Armature/Hips/Spine/Chest/Neck"},new[]{"BreastRotateL","Armature/Hips/Spine/Chest/BreastRotateL"},new[]{"BreastRotateR","Armature/Hips/Spine/Chest/BreastRotateR"},new[]{"ButtL","Armature/Hips/Butt_L"},new[]{"ButtR","Armature/Hips/Butt_R"},new[]{"Spine","Armature/Hips/Spine"},new[]{"Chest","Armature/Hips/Spine/Chest"},new[]{"ShoulderL","Armature/Hips/Spine/Chest/Shoulder_L"},new[]{"ShoulderR","Armature/Hips/Spine/Chest/Shoulder_R"},new[]{"UpperArmL","Armature/Hips/Spine/Chest/Shoulder_L/UpperArm_L"},new[]{"UpperArmR","Armature/Hips/Spine/Chest/Shoulder_R/UpperArm_R"},new[]{"LowerArmL","Armature/Hips/Spine/Chest/Shoulder_L/UpperArm_L/LowerArm_L"},new[]{"LowerArmR","Armature/Hips/Spine/Chest/Shoulder_R/UpperArm_R/LowerArm_R"},new[]{"Hip","Armature/Hips"},new[]{"bodyMeshRenderer","Selestia_body"},new[]{"FaceMesh","Body.001"}};
   foreach(var b in bindings){var t=root.Find(b[1]);var row=new FieldRow{field=b[0],originalPath="Player/Manustia/"+b[1],actualPath=t?PathOf(t):null,found=t};r.fields.Add(row);if(!t)throw new Exception("Original binding absent "+b[0]+":"+b[1]);
    if(b[0]=="bodyMeshRenderer"||b[0]=="FaceMesh"){
     var mesh=t.GetComponent<SkinnedMeshRenderer>();if(!mesh||!mesh.sharedMesh)throw new Exception("Actual original mesh missing "+b[0]);row.blendCount=mesh.sharedMesh.blendShapeCount;row.materialSlots=mesh.sharedMaterials.Length;
     row.blendShapes=Enumerable.Range(0,row.blendCount).Select(i=>i+":"+mesh.sharedMesh.GetBlendShapeName(i)).ToArray();
     row.shaderProperties=mesh.sharedMaterials.Select((m,i)=>i+":"+(m?m.name+":"+m.shader.name+":"+string.Join(",",new[]{"_BaseColor","_1st_ShadeColor","_MainTex","_Emissive_Color","_Emissive_Tex"}.Where(x=>m.HasProperty(x))):"missing")).ToArray();
     if(row.materialSlots<(b[0]=="FaceMesh"?7:2))throw new Exception("Original material slot count absent "+b[0]);if(b[0]=="bodyMeshRenderer"&&row.blendCount<36)throw new Exception("Original body shape34/35 absent");
    }
   }
   var color=AssetDatabase.LoadAssetAtPath<MCustomizeColor>(AssetDatabase.GUIDToAssetPath("485b76e5788e3b24b83a3450cc11b9f2"));if(!color)throw new Exception("Original color asset absent");
   r.originalEyeTextures=color.EyeTextures.Length;r.originalEyeEmitTextures=color.EyeEmitTextures.Length;r.originalSkinPatterns=color.SkinPattern.Length;r.originalSkinTextures=color.NormalSkinTexture&&color.HinnyuSkinTexture;
   if(r.originalEyeTextures<=23||r.originalEyeEmitTextures<=23||r.originalSkinPatterns==0||!r.originalSkinTextures)throw new Exception("Original color/index23 inputs absent");
   r.gaps.Add("Two private native Tit callbacks remain empty; this stage does not replace them with observers or call the incomplete body chain.");
   r.gaps.Add("Core shop bootstrap overwrites avatar body/material references with hidden finite receivers. Future body integration must bind these inspected actual original fields after that bootstrap.");
   r.gaps.Add("Input identity/material properties are reported; no original material setup or visible customization runtime success is claimed.");
   r.passed=true;
  }catch(Exception e){r.failure=e.ToString();}
  var args=Environment.GetCommandLineArgs();int index=Array.IndexOf(args,"-sfmBodyInputOutput");if(index<0||index+1>=args.Length)throw new Exception("Fresh owned report path required");var output=Path.GetFullPath(args[index+1]);if(!output.StartsWith(Path.Combine(stage,"runs")+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||File.Exists(output))throw new Exception("Fresh owned report path only");File.WriteAllText(output,JsonUtility.ToJson(r,true)+"\n");if(!r.passed)throw new Exception("Body input preflight failed; see retained JSON");Debug.Log("SFM_BODY_INPUTS passed=true runtime_body_called=false fields="+r.fields.Count);
 }
}
}
#endif
