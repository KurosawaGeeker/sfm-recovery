#if UNITY_EDITOR
using System;using System.IO;using System.Linq;using System.Reflection;using System.Security.Cryptography;
using ExposureUnnoticed2.Master.Action;using ExposureUnnoticed2.Object3D.Player.Scripts;
using UnityEditor;using UnityEditor.Animations;using UnityEditor.Build.Reporting;using UnityEditor.SceneManagement;using UnityEngine;
namespace SfmMvp.Editor
{
    public static class CoreGameplayBuild
    {
        const string Stage=@"E:\ACTgame\SFM\ReverseEngineering\tools\unity-editor\npc-core-runtime-candidate-v1";
        static string Pin(Type t,string method,params Type[] args){var m=t.GetMethod(method,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static,null,args,null);if(m==null||m.GetMethodBody()==null)throw new Exception("Original body missing "+method);using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(m.GetMethodBody().GetILAsByteArray())).Replace("-","");}
        public static void Build()
        {
            if(Path.GetFullPath(Path.Combine(Application.dataPath,".."))!=Path.Combine(Stage,"Project"))throw new Exception("Owned action project required");
            var args=Environment.GetCommandLineArgs();int k=Array.IndexOf(args,"-sfmActionBuild");if(k<0||k+1>=args.Length)throw new Exception("Fresh output build directory");
            string output=Path.GetFullPath(args[k+1]);if(!output.StartsWith(Stage+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase)||Directory.Exists(output))throw new Exception("Fresh owned build path");
            var scene=EditorSceneManager.OpenScene("Assets/MvpGenerated/MvpApartmentPamPlayerV4.unity",OpenSceneMode.Single);
            var bridge=scene.GetRootGameObjects().SelectMany(x=>x.GetComponentsInChildren<OriginalMovementMvpBridge>(true)).Single();
            var graph=AssetDatabase.LoadAssetAtPath<AnimatorController>("Assets/AnimatorController/Player_0.controller");
            var master=AssetDatabase.LoadAssetAtPath<MAction>("Assets/MonoBehaviour/MAction.asset");
            if(graph==null||master==null||graph.parameters.Length!=40||graph.layers.Length!=12||graph.animationClips.Distinct().Count()!=175)throw new Exception("Original graph/master shape");
            if(bridge.visibleAnimator.avatar==null||!bridge.visibleAnimator.avatar.isValid||!bridge.visibleAnimator.avatar.isHuman)throw new Exception("Original humanoid Avatar");
            foreach(var m in scene.GetRootGameObjects().SelectMany(x=>x.GetComponentsInChildren<MonoBehaviour>(true)).Where(x=>x!=null&&new[]{"PamEngineSmoke","PlayerFrameMvpSmoke","InputGroundMvpSmoke","OriginalMovementMvpSmoke"}.Contains(x.GetType().Name)).ToArray())UnityEngine.Object.DestroyImmediate(m);
            bridge.ownedMovementController=graph;bridge.visibleAnimator.runtimeAnimatorController=graph;bridge.visibleAnimator.fireEvents=false;bridge.visibleAnimator.applyRootMotion=false;bridge.visibleAnimator.cullingMode=AnimatorCullingMode.AlwaysAnimate;
            bridge.expectedLocomotionUpdateIL=Pin(typeof(PlayerLocomotion),"OnUpdate",typeof(InputData));bridge.expectedLocomotionFixedIL=Pin(typeof(PlayerLocomotion),"OnFixedUpdate");bridge.expectedControllerFixedIL=Pin(typeof(PlayerController),"OnFixedUpdate");bridge.expectedSetStrafeIL=Pin(typeof(PlayerAnimationManager),"SetStrafe",typeof(bool),typeof(bool),typeof(float),typeof(float));bridge.expectedControllerUpdateIL=Pin(typeof(PlayerController),"OnUpdate");bridge.expectedActionFrameEndIL=Pin(typeof(PlayerAction),"OnFrameEnd");bridge.expectedBodyMaterialUpdateIL=Pin(typeof(PlayerBodyMaterialController),"OnUpdate");bridge.expectedClothesUpdateIL=Pin(typeof(PlayerClothes),"OnUpdate");bridge.expectedFootUpdateIL=Pin(typeof(PlayerFootStepController),"OnUpdate");bridge.expectedHandUpdateIL=Pin(typeof(PlayerHandcuffsController),"OnUpdate",typeof(InputData));bridge.expectedHeartUpdateIL=Pin(typeof(PlayerHeartBeat),"OnUpdate");bridge.expectedPantsUpdateIL=Pin(typeof(PlayerPantsController),"OnUpdate");bridge.expectedShioUpdateIL=Pin(typeof(PlayerShiofukiController),"OnUpdate");
            var core=bridge.gameObject.AddComponent<CoreGameplayData>();bridge.originalCoreData=core;
            T Original<T>(string guid) where T:UnityEngine.Object {var path=AssetDatabase.GUIDToAssetPath(guid);var value=AssetDatabase.LoadAssetAtPath<T>(path);if(!value)throw new Exception("Original core data missing "+guid);return value;}
            core.Common=Original<ExposureUnnoticed2.Master.Common.MCommon>("30d7935c95dbfc84ab5dc1a3215d660f");
            core.StaminaRecords=Original<ExposureUnnoticed2.Master.Skill.SkillSlow.MSkillStamina>("bb54bb1ac5bf2c64d8a05f0fbb7d7b25");
            core.Hair=Original<ExposureUnnoticed2.Master.Hair.MHair>("4130669b7e4c82343b4af57dd0015ce9");
            core.Stage=Original<ExposureUnnoticed2.Master.Stage.MStage>("7a3f3d3d908dc5842a34c2f2d0940af5");
            core.Mission=Original<ExposureUnnoticed2.Master.Mission.MMission>("9521c9af0447ac24facb565e87c16388");
            core.Skill=Original<ExposureUnnoticed2.Master.Skill.MSkill>("ce0f23f5178eab04fa30015550041c59");
            core.Cosplay=Original<ExposureUnnoticed2.Master.Cosplay.MCosplay>("5a376b164b8190f44a2e124899c1c0cf");
            core.AdultGoods=Original<ExposureUnnoticed2.Master.AdultGoods.MAdultGoods>("66b66c7db2c1b4b46bf6dcf263dfb72b");
            core.Clothes=Original<ExposureUnnoticed2.Master.Clothes.MClothes>("31a6c8d7c5530df4383df4a794574b44");
            core.Ranks=Original<ExposureUnnoticed2.Master.Rank.MRank>("b52e0014415b2884c8e117967bcb9549");
            core.Items=Original<ExposureUnnoticed2.Master.Item.MItem>("2769bf242af69b041ba1a0041232e465");
            core.Costumes=Original<ExposureUnnoticed2.Master.Costume.MCostume>("9b585b2e062ebf547b21dbf7140a2a25");
            core.Paints=Original<ExposureUnnoticed2.Master.BodyPaint.MBodyPaint>("15d641932ffdf584d915766efd2b08cc");
            core.Capacity=Original<ExposureUnnoticed2.Master.Skill.SkillMaxAccessoryNum.MSkillMaxAccessoryNum>("78a558e3841b5c3419a7b6299b54546a");
            
            var roots=bridge.GetComponentsInChildren<Transform>(true).Where(t=>t.name=="Armature"&&t.Find("Hips/Spine/Chest/Neck/Head")).Select(t=>t.parent).Where(t=>bridge.visibleCoatRenderer.rootBone.IsChildOf(t)&&(bridge.visibleAnimator.transform==t||bridge.visibleAnimator.transform.IsChildOf(t)||t.IsChildOf(bridge.visibleAnimator.transform))).ToArray();
            if(roots.Length!=1)throw new Exception("Unique original visible actor skeleton required");var avatar=roots[0];
            var body=bridge.gameObject.AddComponent<BodyRuntimeData>();body.bridge=bridge;body.colors=Original<ExposureUnnoticed2.Master.Common.MCustomizeColor>("485b76e5788e3b24b83a3450cc11b9f2");
            body.boneNames=new[]{"FootL","FootR","LegL","LegR","LowerLegL","LowerLegR","Head","Neck","BreastRotateL","BreastRotateR","ButtL","ButtR","Spine","Chest","ShoulderL","ShoulderR","UpperArmL","UpperArmR","LowerArmL","LowerArmR","Hip"};
            body.bones=new[]{avatar.Find("Armature/Hips/UpperLeg_L/LowerLeg_L/Foot_L"),avatar.Find("Armature/Hips/UpperLeg_R/LowerLeg_R/Foot_R"),avatar.Find("Armature/Hips/UpperLeg_L"),avatar.Find("Armature/Hips/UpperLeg_R"),avatar.Find("Armature/Hips/UpperLeg_L/LowerLeg_L"),avatar.Find("Armature/Hips/UpperLeg_R/LowerLeg_R"),avatar.Find("Armature/Hips/Spine/Chest/Neck/Head"),avatar.Find("Armature/Hips/Spine/Chest/Neck"),avatar.Find("Armature/Hips/Spine/Chest/BreastRotateL"),avatar.Find("Armature/Hips/Spine/Chest/BreastRotateR"),avatar.Find("Armature/Hips/Butt_L"),avatar.Find("Armature/Hips/Butt_R"),avatar.Find("Armature/Hips/Spine"),avatar.Find("Armature/Hips/Spine/Chest"),avatar.Find("Armature/Hips/Spine/Chest/Shoulder_L"),avatar.Find("Armature/Hips/Spine/Chest/Shoulder_R"),avatar.Find("Armature/Hips/Spine/Chest/Shoulder_L/UpperArm_L"),avatar.Find("Armature/Hips/Spine/Chest/Shoulder_R/UpperArm_R"),avatar.Find("Armature/Hips/Spine/Chest/Shoulder_L/UpperArm_L/LowerArm_L"),avatar.Find("Armature/Hips/Spine/Chest/Shoulder_R/UpperArm_R/LowerArm_R"),avatar.Find("Armature/Hips")};
            body.body=avatar.Find("Selestia_body").GetComponent<SkinnedMeshRenderer>();
            body.bodyMaterials=new[]{Original<Material>("a94a278bb33d0f040abca5d6c5dc36ad"),Original<Material>("4a9d142125c70414b92d9f3de7cbe2a3")};
            body.face=avatar.Find("Body.001").GetComponent<SkinnedMeshRenderer>();
            body.faceMaterials=new[]{Original<Material>("9127534167b58774d89cd022f4a0e613"),Original<Material>("899d690eda7963a4fbd95fefb37e8220"),Original<Material>("5f9332a72aac17645aea6d24ba8fc59e"),Original<Material>("880a8ae32d4a666498554f9672cc0215"),Original<Material>("25ca0b3106415a24488f3b06b31a3e77"),Original<Material>("17883d00866556a4e8dc60463371cbbf"),Original<Material>("9dd56625d97a0584387fafee633f2ade")};
            var bodyPanel=bridge.gameObject.AddComponent<BodyCustomizePanel>();bodyPanel.data=body;bodyPanel.bridge=bridge;bridge.gameObject.AddComponent<BodyRuntimeSmoke>();
            EquipmentEditorDraft.Attach(bridge.gameObject,core,null,avatar);bridge.gameObject.AddComponent<EquipmentSmoke>();
            var originalSlow=bridge.gameObject.AddComponent<EquipmentSlowBinding>();originalSlow.Bridge=bridge;originalSlow.OriginalInputs=Original<GameObject>("38618e2321b44953979e0adf68ee23a1");originalSlow.OriginalTable=Original<ExposureUnnoticed2.Master.Skill.SkillSlow.MSkillSlow>("aa1c3ab9bfe51ce4ebf41f008d90720c");
            core.Actions=master;var panel=bridge.gameObject.AddComponent<CoreGameplayPanel>();panel.data=core;panel.bridge=bridge;bridge.gameObject.AddComponent<CoreGameplaySmoke>();
            var probe=bridge.gameObject.AddComponent<OriginalActionGameplayProbe>();probe.originalMaster=master;
            var manual=bridge.gameObject.AddComponent<EquipmentManualPanel>();manual.Equipment=bridge.GetComponent<EquipmentBootstrapDraft>();manual.Bridge=bridge;manual.Shop=panel;var manualRuntime=bridge.gameObject.AddComponent<EquipmentManualRuntime>();manualRuntime.Bridge=bridge;manualRuntime.Equipment=manual.Equipment;manualRuntime.Panel=manual;manualRuntime.Shop=panel;
            const string path="Assets/MvpGenerated/MvpApartmentOriginalActions.unity";if(!EditorSceneManager.SaveScene(scene,path))throw new Exception("Save new action scene");
            PlayerSettings.companyName="SFMRecovery";PlayerSettings.productName="SFMEquipmentTransactionRuntimeCandidateV5";PlayerSettings.runInBackground=true;PlayerSettings.defaultScreenWidth=1280;PlayerSettings.defaultScreenHeight=720;PlayerSettings.fullScreenMode=FullScreenMode.Windowed;PlayerSettings.SetScriptingBackend(BuildTargetGroup.Standalone,ScriptingImplementation.Mono2x);AssetDatabase.SaveAssets();
            Directory.CreateDirectory(output);
            var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{path},locationPathName=Path.Combine(output,"SFM_CoreGameplay.exe"),target=BuildTarget.StandaloneWindows64,options=BuildOptions.None});
            File.WriteAllText(Path.Combine(output,"build.json"),"{\"result\":\""+report.summary.result+"\",\"errors\":"+report.summary.totalErrors+",\"warnings\":"+report.summary.totalWarnings+",\"bytes\":"+report.summary.totalSize+",\"original_graph\":true,\"Player_pins_pending\":true}\n");
            if(report.summary.result!=BuildResult.Succeeded)throw new Exception("Original action Player build failed");
            Debug.Log("SFM_ORIGINAL_ACTION_BUILD succeeded=true actual_pins_pending=true");
        }
    }
}
#endif
