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
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.InGame;
namespace SfmMvp.Editor
{
    // Owned EditMode integration inventory. No PlayMode/gameplay-state writes.
    public static class OriginalBootstrapScenePreparationV3
    {
        const string Project="E:/ACTgame/SFM/ReverseEngineering/tools/unity-editor/core-gameplay-bootstrap-runtime-candidate-v2/Project";
        public const string DerivedScene="Assets/SfmOriginalGameplayV3/Apartment.unity";
        [Serializable]public sealed class Row {public string owner,field;public bool present,originalIntentionalNull;}
        [Serializable]public sealed class Report
        {
            public bool passed,PlayMode=false,originalSavesAccessed=false,gameplayStateWritten=false;
            public string unity,project,scene,sceneSHA,editorGameSHA,persistentDataPath,failure;
            public int originalManagers,originalReferencers,originalAssetReferencers,originalStageControllers,missingScripts,reviewedResources,playerPrefabMissingScripts,playerControllers,playerObjectReferencers,playerAvatarReferencers;
            public List<Row> references=new List<Row>();public List<string> missingRequired=new List<string>();
        }
        static void Need(bool v,string label){if(!v)throw new InvalidOperationException(label);}
        static string Hash(string p){using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(File.ReadAllBytes(p))).Replace("-","");}
        public static void Prepare()
        {
            Need(Path.GetFullPath(Path.Combine(Application.dataPath,".."))==Path.GetFullPath(Project),"Independent V2 Project required");
            var args=Environment.GetCommandLineArgs();int k=Array.IndexOf(args,"-sfmOriginalPreparationOutputV2");Need(k>=0&&k+1<args.Length,"Explicit fresh output");string output=Path.GetFullPath(args[k+1]);Need(!File.Exists(output)&&output.StartsWith(Path.GetFullPath(Path.Combine(Project,"../runs"))+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase),"Fresh owned report");Directory.CreateDirectory(Path.GetDirectoryName(output));
            var report=new Report{unity=Application.unityVersion,project=Project};
            try
            {
                Need(!EditorApplication.isPlayingOrWillChangePlaymode,"EditMode required");
                Need(!File.Exists(Path.Combine(Project,DerivedScene)),"Fresh derived original scene");
                PlayerSettings.companyName="SFMRecovery";PlayerSettings.productName="SFMOriginalGameplayRuntimeV2";
                report.persistentDataPath=Application.persistentDataPath;
                var scene=EditorSceneManager.OpenScene("Assets/ZZZ-ExternalAsset/MapAsset/JP_Apartment/Scenes/Apartment.unity",OpenSceneMode.Single);
                var objects=scene.GetRootGameObjects().SelectMany(x=>x.GetComponentsInChildren<Transform>(true)).Select(x=>x.gameObject).ToArray();
                report.missingScripts=objects.Sum(GameObjectUtility.GetMonoBehavioursWithMissingScriptCount);
                var managers=objects.SelectMany(x=>x.GetComponents<InGameManager>()).ToArray();var referencers=objects.SelectMany(x=>x.GetComponents<InGameReferencer>()).ToArray();var assets=objects.SelectMany(x=>x.GetComponents<AssetReferencer>()).ToArray();
                report.originalManagers=managers.Length;report.originalReferencers=referencers.Length;report.originalAssetReferencers=assets.Length;report.originalStageControllers=objects.SelectMany(x=>x.GetComponents<StageController>()).Count();
                Need(managers.Length==1&&referencers.Length==1&&assets.Length==1&&report.originalStageControllers==1,"Unique original scene manager/referencer/AR/Stage graph");
                Need(objects.All(x=>x.GetComponent<MvpPlayerMotor>()==null&&x.GetComponent<OriginalMovementMvpBridge>()==null),"Original scene contains no legacy gameplay bridge");
                foreach(var ir in referencers)foreach(var f in ir.GetType().GetFields(BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance).Where(f=>(f.IsPublic||f.IsDefined(typeof(SerializeField),true))&&typeof(UnityEngine.Object).IsAssignableFrom(f.FieldType)))
                {
                    var value=(UnityEngine.Object)f.GetValue(ir);bool expected=f.Name=="<Player>k__BackingField"||f.Name=="<ClothesPathSearchAgent>k__BackingField";
                    report.references.Add(new Row{owner="InGameReferencer",field=f.Name,present=value,originalIntentionalNull=expected&&!value});
                    if(!value&&!expected)report.missingRequired.Add("InGameReferencer."+f.Name);
                }
                var bindings=TitleModelsWhitelistV1.LoadReviewedBindings();report.reviewedResources=bindings.Length;
                foreach(var b in bindings)
                {
                    var f=typeof(AssetReferencer).GetField(b.field,BindingFlags.Public|BindingFlags.Instance);
                    var current=b.index<0?(UnityEngine.Object)f.GetValue(assets[0]):(UnityEngine.Object)((Array)f.GetValue(assets[0])).GetValue(b.index);
                    string expectedGuid=b.field=="DroppedPrefabs"&&b.index==0?"6d69b5227cd2bee4fa3addd5abe4029c":b.field=="DroppedPrefabs"&&b.index==5?"e309a7952484ae942878c4b141e64af2":b.field=="DroppedPrefabs"&&b.index==6?"7a9d91868c0f70842b7bc6034f852410":null;
                    if(expectedGuid==null)Need(!current,"Exact original null Title before-reference: "+b.field+":"+b.index);
                    else {string beforeGuid;long beforeFileId;Need(current&&AssetDatabase.TryGetGUIDAndLocalFileIdentifier(current,out beforeGuid,out beforeFileId)&&beforeGuid==expectedGuid&&beforeFileId==1709254077376921L,"Exact original Title overwritten prefab before-reference");}
                    b.expectedBefore=current;
                }
                var player=(GameObject)bindings.Single(x=>x.field=="Player"&&x.index==-1).asset;
                report.playerPrefabMissingScripts=player.GetComponentsInChildren<Transform>(true).Sum(t=>GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(t.gameObject));
                report.playerControllers=player.GetComponentsInChildren<PlayerController>(true).Length;
                report.playerObjectReferencers=player.GetComponentsInChildren<PlayerObjectReferencer>(true).Length;
                report.playerAvatarReferencers=player.GetComponentsInChildren<PlayerAvatarObjectReferencer>(true).Length;
                Need(report.playerControllers==1&&report.playerObjectReferencers==1&&report.playerAvatarReferencers==1,"Complete original Player core component graph");
                Need(report.playerPrefabMissingScripts==0&&report.missingScripts==0&&report.missingRequired.Count==0,"No missing original core scripts or serialized required references");
                // Only the owned adapter's resource-binding records are serialized.
                // Original AR fields and all original scene objects remain intact.
                var host=new GameObject("Owned original gameplay resource adapter V2");var adapter=host.AddComponent<OriginalGameplayBootstrapV2>();adapter.bindings=bindings;
                adapter.originalInGameManagerPrefab=AssetDatabase.LoadAssetAtPath<GameObject>("Assets/GameObject/InGameManager.prefab");
                Need(adapter.originalInGameManagerPrefab&&adapter.originalInGameManagerPrefab.GetComponent<InGameManager>(),"Original manager prefab");
                Directory.CreateDirectory(Path.Combine(Project,"Assets/SfmOriginalGameplayV3"));Need(EditorSceneManager.SaveScene(scene,DerivedScene),"Save derived scene");
                report.scene=DerivedScene;report.sceneSHA=Hash(Path.Combine(Project,DerivedScene));report.editorGameSHA=Hash(typeof(InGameManager).Assembly.Location);
                AssetDatabase.SaveAssets();report.passed=true;
            }
            catch(Exception e){report.failure=e.ToString();Debug.LogError("SFM_ORIGINAL_PREPARATION_V2 "+e);}
            File.WriteAllText(output,JsonUtility.ToJson(report,true)+"\n");Debug.Log("SFM_ORIGINAL_PREPARATION_V2 passed="+report.passed+" resources="+report.reviewedResources+" missingScripts="+report.missingScripts+" playerMissing="+report.playerPrefabMissingScripts);
            if(!report.passed)throw new InvalidOperationException(report.failure);
        }
    }
}
#endif
