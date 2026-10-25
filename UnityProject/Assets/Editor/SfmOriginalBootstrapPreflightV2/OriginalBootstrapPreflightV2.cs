#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using ExposureUnnoticed2.Object3D.IngameManager;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
namespace SfmMvp.Editor
{
    // Owned integration inventory, zero recovered methods. No PlayMode or save IO.
    public static class OriginalBootstrapPreflightV2
    {
        const string Project="E:/ACTgame/SFM/ReverseEngineering/tools/unity-editor/core-gameplay-bootstrap-runtime-candidate-v2/Project";
        [Serializable] public sealed class ReferenceRow { public string owner,field,type,path; public bool present; }
        [Serializable] public sealed class Report
        {
            public bool passed,PlayMode=false,originalSavesAccessed=false,gameplayStateWritten=false;
            public string unity,project,scene,persistentDataPath;
            public int missingScripts,originalManagers,originalReferencers,originalAssetReferencers,originalStageControllers;
            public List<string> missingRequired=new List<string>(),missingScriptObjects=new List<string>();
            public List<ReferenceRow> references=new List<ReferenceRow>();
        }
        public static void Check()
        {
            if(Path.GetFullPath(Path.Combine(Application.dataPath,".."))!=Path.GetFullPath(Project))throw new Exception("Independent V2 Project required");
            var args=Environment.GetCommandLineArgs();int k=Array.IndexOf(args,"-sfmBootstrapPreflightOutputV2");
            if(k<0||k+1>=args.Length)throw new Exception("Explicit fresh report required");
            string output=Path.GetFullPath(args[k+1]);
            if(File.Exists(output)||!output.StartsWith(Path.GetFullPath(Path.Combine(Project,"../runs"))+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase))throw new Exception("Fresh owned output required");
            Directory.CreateDirectory(Path.GetDirectoryName(output));
            PlayerSettings.companyName="SFMRecovery";PlayerSettings.productName="SFMOriginalBootstrapRuntimeCandidateV2";
            var scene=EditorSceneManager.OpenScene("Assets/ZZZ-ExternalAsset/MapAsset/JP_Apartment/Scenes/Apartment.unity",OpenSceneMode.Single);
            var roots=scene.GetRootGameObjects();var objects=roots.SelectMany(x=>x.GetComponentsInChildren<Transform>(true)).Select(x=>x.gameObject).ToArray();
            var report=new Report{unity=Application.unityVersion,project=Project,scene=scene.path,persistentDataPath=Application.persistentDataPath};
            foreach(var go in objects){int missing=GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(go);report.missingScripts+=missing;if(missing>0)report.missingScriptObjects.Add(go.name+":"+missing);}
            var managers=objects.SelectMany(x=>x.GetComponents<InGameManager>()).ToArray();var referencers=objects.SelectMany(x=>x.GetComponents<InGameReferencer>()).ToArray();var assets=objects.SelectMany(x=>x.GetComponents<AssetReferencer>()).ToArray();
            report.originalManagers=managers.Length;report.originalReferencers=referencers.Length;report.originalAssetReferencers=assets.Length;report.originalStageControllers=objects.SelectMany(x=>x.GetComponents<StageController>()).Count();
            if(managers.Length!=1||referencers.Length!=1||assets.Length!=1||report.originalStageControllers!=1)report.missingRequired.Add("Unique original manager/referencer/AR/Stage graph");
            const BindingFlags F=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance;
            foreach(var ar in assets)foreach(var f in ar.GetType().GetFields(F).Where(f=>typeof(UnityEngine.Object).IsAssignableFrom(f.FieldType))){var value=(UnityEngine.Object)f.GetValue(ar);report.references.Add(new ReferenceRow{owner="AssetReferencer",field=f.Name,type=f.FieldType.FullName,present=value,path=value?AssetDatabase.GetAssetPath(value):null});}
            foreach(var ir in referencers)foreach(var f in ir.GetType().GetFields(F).Where(f=>typeof(UnityEngine.Object).IsAssignableFrom(f.FieldType))){var value=(UnityEngine.Object)f.GetValue(ir);report.references.Add(new ReferenceRow{owner="InGameReferencer",field=f.Name,type=f.FieldType.FullName,present=value,path=value?AssetDatabase.GetAssetPath(value):null});if(!value)report.missingRequired.Add("InGameReferencer."+f.Name);}
            foreach(string name in new[]{"MStage","MCommon","MClothes","MSkill","MAction","mAdultGoods","MHair","MItem","MMission","MRank","MRankRelease","MCostume","MDroneMission","MCosplay","Player"})if(!report.references.Any(x=>x.owner=="AssetReferencer"&&x.field==name&&x.present))report.missingRequired.Add("AssetReferencer."+name);
            report.passed=report.missingRequired.Count==0&&report.missingScripts==0;
            AssetDatabase.SaveAssets();File.WriteAllText(output,JsonUtility.ToJson(report,true)+"\n");
            Debug.Log("SFM_ORIGINAL_BOOTSTRAP_PREFLIGHT "+JsonUtility.ToJson(new Report{passed=report.passed,missingScripts=report.missingScripts,missingRequired=report.missingRequired}));
        }
    }
}
#endif
