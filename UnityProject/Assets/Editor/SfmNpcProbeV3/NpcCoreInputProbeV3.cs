// Owned Editor observation only. Deploy by Root after resource/code preflight.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using ExposureUnnoticed2.Object3D.IngameManager;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Object3D.RoutePoint;
using ExposureUnnoticed2.Object3D.ScenePlops.Conbini;
using ExposureUnnoticed2.ObjectUI.OptionMenu;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using Object=UnityEngine.Object;

namespace SfmMvp.Editor
{
    public static class NpcCoreInputProbeV3
    {
        const string Root="E:/ACTgame/SFM/ReverseEngineering";
        const string NpcGuid="791378a06555be54a99eb05e12d101e7";
        const string ManagerGuid="1fe6b30d2eb34b0aad848593cc30aa3a";
        static readonly string[] AvatarGuids={"83c75eab16269754d9ff8645cd88318a","00a8ac3f8d972154f8cbc687dd33ef2f","dace0fd16ce26ef4fad8381066d4f70f","d249b021817c9f743ad4d85bf75c0f3b","4364033ade803674f8e6d4cb9f72c671","3570ff0dc40b1cc4f89c87b28d2d5585","f41142aef1abbbb48b27dcc0229f34fa","b11458ef69cc2df438b3543ee3c9b738","ace9d69b68f1ff049970ec2db83e6493"};
        [Serializable] public sealed class AssetRow
        {
            public string role,guid,path,cloneName,headPath,animatorAvatar,animatorController;
            public bool actualComponent,headBelowClone,headSerialized,animatorPresent,avatarValid,avatarHuman,receiverPresent;
            public int renderers,skinnedMeshes,boneReferences,originalNullSlots,observedNullSlots,nonzeroSlotsVerified;
        }
        [Serializable] public sealed class Report
        {
            public string gate,unity,gameAssemblySHA256,failure;
            public bool passed,actualUnityEditor=true,playMode,originalNativeGameLoaded=false,originalSavesAccessed=false;
            public bool ninePrefabInputsObserved,realManagerInitializeReturned,realNcaConstructorReturned,controllerFullInitializeReturned;
            public bool originalFactoryReturned,originalRegistryListContainsNpc,originalRegistryDictContainsNpc;
            public bool realHead,lookAtHeadBound,navAgentOnNavMesh,detectorResultWrittenByProbe=false;
            public bool fixtureNpcActive,originalPhysicsBindings,originalNavDataLoaded; public int originalNpcCharacterControllers; public Vector3 originalAppearPosition,sampledAppearPosition;
            public int checks,navAgentType,optionMaleRate,optionOldRate,npcCountBefore,npcCountAfter;
            public Vector3 npcPosition;
            public string[] scope;
            public List<AssetRow> assets=new List<AssetRow>();
            public List<OriginalBoneSlotObserverV3.Result> boneChecks=new List<OriginalBoneSlotObserverV3.Result>();
            public List<string> checksPassed=new List<string>(),blocked=new List<string>(),engineErrors=new List<string>();
        }
        static Report report;
        static void Need(bool value,string label)
        {
            report.checks++;
            if(!value)throw new InvalidOperationException(label);
            report.checksPassed.Add(label);
        }
        static string Hash(string path)
        {
            using(var sha=SHA256.Create())return BitConverter.ToString(sha.ComputeHash(File.ReadAllBytes(path))).Replace("-","");
        }
        static T Read<T>(object target,string name)
        {
            var f=target.GetType().GetField(name,BindingFlags.Instance|BindingFlags.Static|BindingFlags.NonPublic|BindingFlags.Public);
            if(f==null)throw new MissingFieldException(target.GetType().FullName,name);
            return (T)f.GetValue(target);
        }
        static GameObject Asset(string guid)
        {
            string path=AssetDatabase.GUIDToAssetPath(guid);
            Need(!string.IsNullOrEmpty(path),"Original GUID resolves "+guid);
            var value=AssetDatabase.LoadAssetAtPath<GameObject>(path);
            Need(value!=null,"Original prefab imports "+guid);
            return value;
        }
        static string PathBelow(Transform item,Transform parent)
        {
            if(item==null)return null;
            var names=new List<string>();var p=item;
            while(p!=null&&p!=parent){names.Add(p.name);p=p.parent;}
            if(p!=parent)return null;
            names.Reverse();return string.Join("/",names);
        }
        static AssetRow ObserveAvatar(GameObject avatar,string role,string guid)
        {
            var c=avatar.GetComponent<NpcAvatarComponent>();var a=avatar.GetComponent<Animator>();
            Need(c!=null,role+" original root NpcAvatarComponent");
            Need(c.Head!=null,role+" original serialized Head nonnull");
            string hp=PathBelow(c.Head,avatar.transform);Need(hp!=null,role+" Head belongs to instantiated original rig");
            Need(a!=null,role+" original root Animator");
            Need(a.avatar!=null&&a.avatar.isValid,role+" original imported Avatar is valid");
            Need(a.runtimeAnimatorController!=null,role+" original RuntimeAnimatorController");
            var receiver=avatar.GetComponent<NpcAnimationEventReceiver>();Need(receiver!=null,role+" original root AnimationEventReceiver");
            var boneReport=OriginalBoneSlotObserverV3.Verify(avatar,guid,role);
            report.boneChecks.Add(boneReport);
            Need(boneReport.passed,role+" exact original null slots preserved and every nonzero slot resolves to the original Transform");
            var meshes=avatar.GetComponentsInChildren<SkinnedMeshRenderer>(true);int bones=boneReport.boneSlotCount;
            var row=new AssetRow{role=role,guid=guid,path=string.IsNullOrEmpty(guid)?null:AssetDatabase.GUIDToAssetPath(guid),cloneName=avatar.name,actualComponent=true,headPath=hp,headBelowClone=true,headSerialized=true,animatorPresent=true,avatarValid=a.avatar.isValid,avatarHuman=a.avatar.isHuman,animatorAvatar=AssetDatabase.GetAssetPath(a.avatar),animatorController=AssetDatabase.GetAssetPath(a.runtimeAnimatorController),receiverPresent=receiver!=null,renderers=avatar.GetComponentsInChildren<Renderer>(true).Length,skinnedMeshes=meshes.Length,boneReferences=bones,originalNullSlots=boneReport.originalNullSlots,observedNullSlots=boneReport.observedNullSlots,nonzeroSlotsVerified=boneReport.nonzeroSlotsVerified};
            report.assets.Add(row);return row;
        }
        static string Begin(string gate)
        {
            report=new Report{gate=gate,unity=Application.unityVersion,playMode=EditorApplication.isPlaying,gameAssemblySHA256=Hash(typeof(NpcController).Assembly.Location)};
            var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,"-sfmNpcProbeOutput");
            string output=i>=0&&i+1<args.Length?Path.GetFullPath(args[i+1]):Path.GetFullPath(Root+"/tools/unity-editor/npc-initialization-runtime-probe-preparation-v1/runs/"+gate+"-"+DateTime.UtcNow.ToString("yyyyMMddHHmmssfff"));
            string allowed=Path.GetFullPath(Root+"/tools")+Path.DirectorySeparatorChar;
            if(!output.StartsWith(allowed,StringComparison.OrdinalIgnoreCase)||Directory.Exists(output)||File.Exists(output)||output.StartsWith(Path.GetFullPath(Application.dataPath)+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase))throw new Exception("Fresh owned non-Project report directory required");
            Directory.CreateDirectory(output);return output;
        }
        static void Log(string message,string stack,LogType type)
        {
            if(type==LogType.Error||type==LogType.Exception||type==LogType.Assert)report.engineErrors.Add(type+": "+message);
        }
        static void End(string output)
        {
            File.WriteAllText(Path.Combine(output,"npc-input-probe.json"),JsonUtility.ToJson(report,true));
            Debug.Log("NPC probe "+report.gate+" passed="+report.passed+" checks="+report.checks+" report="+output);
        }

        [MenuItem("SFM/QA/NPC Gate A v2 - original inputs and NCA constructor")]
        public static void RunGateA()
        {
            string output=Begin("gate-a");GameObject host=null,managerObject=null,worldObject=null;NavMeshDataInstance navInstance=default;OptionData oldOptions=GameState.OptionData;
            Application.logMessageReceived+=Log;
            try
            {
                Need(!EditorApplication.isPlaying,"Gate A isolated edit-mode constructor fixture");
                Need(NpcManager.Instance==null,"No live Manager singleton will be replaced");
                host=new GameObject("Owned inactive NPC constructor probe");host.SetActive(false);
                var prefab=Asset(NpcGuid);var template=prefab.GetComponent<NpcComponent>();Need(template!=null,"Original NpcComponent resolves");
                var alternatives=new[]{template.PrefabFemale,template.PrefabMale,template.PrefabPreviousFemale,template.PrefabPreviousMale}.Concat(template.PrefabOjis).Concat(template.PrefabObas).ToArray();
                Need(alternatives.Length==9,"Exactly nine original serialized alternatives");
                for(int n=0;n<9;n++)
                {
                    Need(alternatives[n]!=null&&AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(alternatives[n]))==AvatarGuids[n],"Original selector reference "+n);
                    var clone=Object.Instantiate(alternatives[n],host.transform);ObserveAvatar(clone,"direct-resource-"+n,AvatarGuids[n]);Object.DestroyImmediate(clone);
                }
                report.ninePrefabInputsObserved=true;
                managerObject=Object.Instantiate(Asset(ManagerGuid));Need(!managerObject.activeInHierarchy,"Original Manager forest effectively inactive");
                var manager=managerObject.GetComponentInChildren<NpcManager>(true);Need(manager!=null,"Original Manager component");
                Need(Read<GameObject>(manager,"sightAreaPrefab")!=null&&Read<Transform>(manager,"sightAreaParent")!=null,"Original sight prefab/parent references");
                // Player is stored but not dereferenced by the original constructor. This gate is NOT full Initialize.
                manager.Initialize(null);report.realManagerInitializeReturned=true;Need(NpcManager.Instance==manager,"Original Manager.Initialize establishes Instance");
                if(GameState.OptionData==null)GameState.OptionData=new OptionData();
                report.optionMaleRate=GameState.OptionData.MaleRate100;report.optionOldRate=GameState.OptionData.OldPeopleRate100;
                worldObject=(GameObject)PrefabUtility.InstantiatePrefab(Asset("2bc045a7890745dc83c37bb2b058806a"));
                Need(!worldObject.activeInHierarchy,"Original navigation input forest stays inactive");
                var builder=worldObject.GetComponentsInChildren<RouteBuilderConbini>(true).Single();
                var appear=Read<RoutePoint>(builder,"appearPoint");Need(appear!=null,"Original serialized appear point");
                var surface=worldObject.GetComponentsInChildren<MonoBehaviour>(true).Single(x=>x.GetType().FullName=="Unity.AI.Navigation.NavMeshSurface");
                var data=Read<NavMeshData>(surface,"m_NavMeshData");Need(data!=null&&AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(data))=="0f51b2c2415a663488542d05557ae38a","Original serialized baked navigation GUID");
                Need(Hash(Path.Combine(Path.GetDirectoryName(Application.dataPath),AssetDatabase.GetAssetPath(data)))=="3D1C98EBF1A0C1221CB54D04050E406FFC99F21DCCE45669852014A6F58CAE9C","Original baked navigation bytes unchanged");
                navInstance=NavMesh.AddNavMeshData(data,surface.transform.position,surface.transform.rotation);Need(navInstance.valid,"Owned SDK loader mounts original data at original Surface pose");report.originalNavDataLoaded=true;
                var triangles=NavMesh.CalculateTriangulation();Need(triangles.vertices.Length==370&&triangles.indices.Length==528,"Same original navigation geometry as prior609-check input proof");
                NavMeshHit spawn;Need(NavMesh.SamplePosition(appear.transform.position,out spawn,1f,7),"Original appear point samples original navigation");report.originalAppearPosition=appear.transform.position;report.sampledAppearPosition=spawn.position;
                host.SetActive(true);Need(host.activeInHierarchy,"Owned host permits original active-only child lookups");
                var npc=Object.Instantiate(prefab,spawn.position,prefab.transform.rotation,host.transform).GetComponent<NpcComponent>();Need(npc!=null&&npc.gameObject.activeInHierarchy,"Original active Npc clone component");report.fixtureNpcActive=true;
                var controller=npc.CreateController();Need(controller!=null&&npc.NpcController==controller,"Original CreateController returned and bound original property");
                var state=new NpcStateModel();var nca=new NpcComponentAccessor(controller,npc,state,null);report.realNcaConstructorReturned=true;
                Need(nca.NpcController==controller&&nca.NpcComponent==npc&&nca.StateModel==state,"Original NCA parameter bindings");
                Need(nca.Player==null&&controller.Nca==null,"Constructor-only scope: no Player graph or Controller.Initialize result invented");
                Need(nca.NavAgent==npc.GetComponent<NavMeshAgent>()&&nca.NavAgent!=null,"Original real NavMeshAgent component binding");
                report.navAgentType=nca.NavAgent.agentTypeID;
                Need(nca.Rigidbody!=null&&nca.CapsuleCollider!=null&&nca.NpcCollider!=null,"Original actual Rigidbody/Capsule/NpcCollider bindings");report.originalPhysicsBindings=true;
                report.originalNpcCharacterControllers=npc.GetComponents<CharacterController>().Length;Need(report.originalNpcCharacterControllers==0,"Original NavMesh-based Npc prefab has no CharacterController; none added");
                Need(nca.AudioSource!=null,"Original prefab AudioSource exists (clip is not replaced)");
                Need(nca.PlayerSearcher!=null&&nca.PlayerSearcher.transform.IsChildOf(Read<Transform>(manager,"sightAreaParent")),"Original CreateSightArea actually instantiated Searcher under original parent");
                Need(npc.AvaterObject!=null&&npc.AvaterObject.transform.parent==npc.AvatarParent,"Original CreateAvatar selected and instantiated under original AvatarParent");
                ObserveAvatar(npc.AvaterObject,"original-NCA-selected-avatar",null);
                Need(nca.AvatarComponent==npc.AvaterObject.GetComponent<NpcAvatarComponent>()&&nca.Animator==npc.AvaterObject.GetComponent<Animator>()&&nca.AnimationEventReceiver==nca.Animator.GetComponent<NpcAnimationEventReceiver>(),"Original constructor binds actual avatar/animator/receiver");
                report.realHead=nca.AvatarComponent.Head!=null;
                report.scope=new[]{"All nine original asset alternatives directly inspected; only one genuine random selector/NCA construction executed","Real Game constructors/CreateController/Manager.Initialize/CreateSightArea/CreateAvatar run unhooked","Null player is constructor-only input; no full NCA.Initialize/Controller.Initialize, LookAt binding, route, NavMesh placement, movement or discovery accepted","New default OptionData is temporary fixture input only when absent; no persisted options or saves read","Owned fixture activates its own host and instantiates at the sampled original appear point. Original component enabled flags, bones, singletons, Head, detection values and audio are never replaced"};
                Need(report.engineErrors.Count==0,"No engine Error/Exception/Assert during fixture");report.passed=true;
            }
            catch(Exception e){report.failure=e.ToString();}
            finally
            {
                if(managerObject!=null)Object.DestroyImmediate(managerObject);
                if(host!=null)Object.DestroyImmediate(host);
                if(navInstance.valid)navInstance.Remove();
                if(worldObject!=null)Object.DestroyImmediate(worldObject);
                GameState.OptionData=oldOptions;
                Application.logMessageReceived-=Log;End(output);
            }
        }

        static void NonStub(Type owner,string method)
        {
            var methods=owner.GetMethods(BindingFlags.Instance|BindingFlags.Static|BindingFlags.Public|BindingFlags.NonPublic).Where(m=>m.Name==method).ToArray();
            Need(methods.Length==1,"Unique required original method "+owner.Name+"."+method);
            var il=methods[0].GetMethodBody()?.GetILAsByteArray();Need(il!=null,"Managed original body exists "+owner.Name+"."+method);
            bool empty=il.All(b=>b==0||b==0x2a)||il.Where(b=>b!=0).SequenceEqual(new byte[]{0x14,0x2a});Need(!empty,"Required nontrivial native method is not an empty/default-null managed body "+owner.Name+"."+method);
        }

        [MenuItem("SFM/QA/NPC Gate B v2 - one original Konbini factory initialization")]
        public static void RunGateB()
        {
            string output=Begin("gate-b");Application.logMessageReceived+=Log;
            try
            {
                Need(EditorApplication.isPlaying,"Gate B requires Root-owned real initialized play-mode context");
                NonStub(typeof(ConbiniController),"Initialize");NonStub(typeof(ConbiniController),"Spawn");
                NonStub(typeof(NpcManager),"CreateFixedNpc");NonStub(typeof(NpcController),"AdditionalInitializeForFixAction");NonStub(typeof(RouteBuilderConbini),"CreateRoute2");
                NonStub(typeof(ExposureUnnoticed2.ObjectUI.InGame.Compass.CompassView),"Initialize");
                NonStub(typeof(ExposureUnnoticed2.ObjectUI.InGame.RingMenu.RingMenuParentView),"Initialize");
                // InteractPanel.Initialize is original shared RET0x3B65E0 and must remain allowed empty.
                NonStub(typeof(ExposureUnnoticed2.ObjectUI.NpcUi.NpcUiView),"Create");
                var manager=NpcManager.Instance;var player=PlayerController.Instance;
                Need(manager!=null&&manager.gameObject.activeInHierarchy,"Actual original Manager is initialized and active");
                Need(player!=null&&player.Pca!=null,"Actual initialized original Player/PCA input");
                Need(ReferenceEquals(Read<PlayerController>(manager,"player"),player)&&Read<bool>(manager,"initialized"),"Original Manager.Initialize owns actual Player binding");
                Need(GameState.GameStateData!=null&&(int)GameState.GameStateData.CurrentStageType==6,"Actual current stage is ordinary Konbini6");
                Need(GameState.OptionData!=null&&BaseRouteBuilder.Instance is RouteBuilderConbini,"Original options and actual Konbini route builder");
                Need(AssetReferencer.Instance!=null&&AssetReferencer.Instance.NpcUiView!=null,"Original AssetReferencer/NpcUi reference after original Awake");
                Need(InGameUiManager.Instance!=null&&InGameUiManager.Instance.BackLayer!=null,"Original complete InGameUiManager.Initialize context/BackLayer");
                var shop=ConbiniController.Instance;Need(shop!=null,"Original complete Conbini.Initialize established singleton (no owned setter)");
                var audio=Read<AudioSource>(shop,"enterSound");Need(audio!=null&&audio.clip!=null&&AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(audio.clip))=="d4491ea46f88f074ebc0b30b7ce0204b","Original actual Doorbell clip/AudioSource");
                Need(NavMesh.CalculateTriangulation().vertices.Length>0,"Actual original scene NavMesh is loaded");
                report.npcCountBefore=manager.ExistNpcList.Count;
                var factory=typeof(NpcManager).GetMethod("InstantiateNpc",BindingFlags.Instance|BindingFlags.NonPublic);
                Need(factory!=null,"Original private factory entry exists");
                var controller=(NpcController)factory.Invoke(manager,new object[]{null,false,NpcStateModel.FixedType.None});
                report.originalFactoryReturned=controller!=null;Need(controller!=null&&controller.Nca!=null,"Original full Controller.Initialize returned through original factory");
                report.controllerFullInitializeReturned=true;report.npcCountAfter=manager.ExistNpcList.Count;
                Need(report.npcCountAfter==report.npcCountBefore+1&&manager.ExistNpcList.Contains(controller),"Original manager factory registered actual NPC list entry");report.originalRegistryListContainsNpc=true;
                Need(manager.ExistNpcGameObjectDict.TryGetValue(controller.GameObject,out var found)&&ReferenceEquals(found,controller),"Original factory registered actual GameObject dictionary");report.originalRegistryDictContainsNpc=true;
                var nca=controller.Nca;ObserveAvatar(nca.NpcComponent.AvaterObject,"full-original-initialized-avatar",null);
                Need(nca.LookAtController.Head!=null&&nca.LookAtController.Head==nca.AvatarComponent.Head,"Original LookAt.InitializeForNpc selected actual serialized Head");report.lookAtHeadBound=true;report.realHead=true;
                Need(ReferenceEquals(nca.Player,player)&&nca.NavAgent!=null&&nca.NavAgent.isOnNavMesh,"Actual Player binding and original Warp/agent placement");report.navAgentOnNavMesh=true;report.navAgentType=nca.NavAgent.agentTypeID;report.npcPosition=nca.Transform.position;
                report.scope=new[]{"Owned explicit test caller invokes the complete original private InstantiateNpc; automatic Repop/interval population is not accepted here","Every original Controller/NCA initializer executes in its own original order; no per-child manual skip/call bridge","Immediate full initialization/registration/Head/NavMesh outputs only; delayed Scheduler callbacks, movement, discovery and renderer/audio fidelity need separate actual frame/runtime evidence","This method requires Root original manager/UI/store/route/global bootstrap and never manufactures singletons, fake Head/audio or detection results"};
                Need(report.engineErrors.Count==0,"No engine Error/Exception/Assert during full original factory call");report.passed=true;
            }
            catch(Exception e){report.failure=e.ToString();}
            finally{Application.logMessageReceived-=Log;End(output);}
        }
    }
}

namespace SfmMvp.Editor
{
    // Owned read-only observer: exact original zeros and nonzero identities, never bone repair.
    public static class OriginalBoneSlotObserverV3
    {
        const string ManifestPath="E:/ACTgame/SFM/ReverseEngineering/tools/unity-editor/npc-original-bone-slot-audit-v1/original-bone-slot-manifest.json";
        const string ManifestSHA="216DCDE3B5C4DCF3206673132DC4AF30F20EB0DF8BB69ADD7108ED50B9ECF81D";
        [Serializable] public sealed class Pin { public string path,sha256; public long bytes; }
        [Serializable] public sealed class Slot { public int index; public string original_fileID,target_path; public bool original_null; public int[] target_child_indices; }
        [Serializable] public sealed class RendererInput
        {
            public string renderer_original_fileID,renderer_path,root_bone_original_fileID;
            public int renderer_ordinal_on_GO,bone_slot_count,original_null_count;
            public bool root_bone_original_null;
            public int[] renderer_child_indices,root_bone_child_indices;
            public List<Slot> slots;
        }
        [Serializable] public sealed class AssetInput
        {
            public string GUID,original_root_name;
            public Pin original_prefab;
            public int original_null_slots,bone_slots;
            public List<RendererInput> renderers;
        }
        [Serializable] public sealed class Manifest { public int avatar_count,original_null_slots,bone_slots; public List<AssetInput> assets; }
        [Serializable] public sealed class Mismatch
        {
            public string rendererOriginalFileID,field,originalFileID,reason,actualPath;
            public int index;
        }
        [Serializable] public sealed class Result
        {
            public bool passed,originalGuidIdentifiedUniquely,originalPrefabBytesExact,sourceFileIdentifiersChecked;
            public string role,GUID,sourceAssetPath,cloneName,manifestSHA;
            public int renderers,boneSlotCount,originalNullSlots,observedNullSlots,nonzeroSlotsVerified,rootBonesVerified;
            public List<Mismatch> mismatches=new List<Mismatch>();
        }
        static string HashFile(string path)
        {
            using(var sha=SHA256.Create())return BitConverter.ToString(sha.ComputeHash(File.ReadAllBytes(path))).Replace("-","");
        }
        static Transform At(Transform root,int[] indices)
        {
            if(root==null||indices==null)return null;
            var t=root;
            foreach(int i in indices){if(i<0||i>=t.childCount)return null;t=t.GetChild(i);}
            return t;
        }
        static string Relative(Transform item,Transform root)
        {
            if(item==null)return null;var names=new List<string>();var t=item;
            while(t!=null&&t!=root){names.Add(t.name+"["+t.GetSiblingIndex()+"]");t=t.parent;}
            if(t!=root)return "OUTSIDE:"+item.name;names.Reverse();return string.Join("/",names);
        }
        static bool SourceId(Object value,string guid,string expected)
        {
            string g;long id;return value!=null&&AssetDatabase.TryGetGUIDAndLocalFileIdentifier(value,out g,out id)&&g==guid&&id.ToString(System.Globalization.CultureInfo.InvariantCulture)==expected;
        }
        public static Result Verify(GameObject clone,string guid,string role)
        {
            if(HashFile(ManifestPath)!=ManifestSHA)throw new Exception("Exact original bone slot manifest changed");
            var manifest=JsonUtility.FromJson<Manifest>(File.ReadAllText(ManifestPath));
            if(manifest.avatar_count!=9||manifest.original_null_slots!=244||manifest.bone_slots!=11412)throw new Exception("Original slot inventory changed");
            var result=new Result{role=role,cloneName=clone.name,manifestSHA=ManifestSHA};
            var candidates=manifest.assets.Where(a=>string.IsNullOrEmpty(guid)?clone.name==a.original_root_name+"(Clone)":a.GUID==guid).ToArray();
            if(candidates.Length!=1)throw new Exception("Original clone GUID/name identity is not unique; no guessed source or bone fallback");
            var asset=candidates[0];result.GUID=asset.GUID;result.originalGuidIdentifiedUniquely=true;
            if(clone.name!=asset.original_root_name+"(Clone)")throw new Exception("Clone name differs from uniquely identified exact original prefab");
            var assetPath=AssetDatabase.GUIDToAssetPath(asset.GUID);var prefab=AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);result.sourceAssetPath=assetPath;
            if(prefab==null)throw new Exception("Original prefab asset unavailable");
            string sourceFile=Path.Combine(Path.GetDirectoryName(Application.dataPath),assetPath);
            if(HashFile(sourceFile)!=asset.original_prefab.sha256)throw new Exception("Original prefab bytes differ before slot comparison");result.originalPrefabBytesExact=true;
            void Bad(RendererInput input,string field,int index,string id,string why,Transform actual)
            {
                result.mismatches.Add(new Mismatch{rendererOriginalFileID=input==null?null:input.renderer_original_fileID,field=field,index=index,originalFileID=id,reason=why,actualPath=Relative(actual,clone.transform)});
            }
            var actualRenderers=clone.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            if(actualRenderers.Length!=asset.renderers.Count)Bad(null,"renderers",-1,null,"Exact original renderer inventory/count differs",null);
            foreach(var input in asset.renderers)
            {
                var sourceGO=At(prefab.transform,input.renderer_child_indices);var cloneGO=At(clone.transform,input.renderer_child_indices);
                if(sourceGO==null||cloneGO==null){Bad(input,"renderer",-1,input.renderer_original_fileID,"Original renderer child-index chain missing",cloneGO);continue;}
                var sr=sourceGO.GetComponents<SkinnedMeshRenderer>();var cr=cloneGO.GetComponents<SkinnedMeshRenderer>();int ordinal=input.renderer_ordinal_on_GO;
                if(ordinal<0||ordinal>=sr.Length||ordinal>=cr.Length){Bad(input,"renderer",-1,input.renderer_original_fileID,"Original renderer component ordinal missing",cloneGO);continue;}
                var source=sr[ordinal];var actual=cr[ordinal];result.renderers++;
                if(!SourceId(source,asset.GUID,input.renderer_original_fileID))Bad(input,"renderer",-1,input.renderer_original_fileID,"Original source renderer GUID/fileID mismatch",cloneGO);
                var sourceBones=source.bones;var actualBones=actual.bones;
                if(sourceBones.Length!=input.bone_slot_count||actualBones.Length!=input.bone_slot_count)Bad(input,"m_Bones",-1,null,"Exact ordered original slot count differs",cloneGO);
                foreach(var slot in input.slots)
                {
                    result.boneSlotCount++;
                    if(slot.original_null)result.originalNullSlots++;
                    if(slot.index<0||slot.index>=actualBones.Length||slot.index>=sourceBones.Length){Bad(input,"m_Bones",slot.index,slot.original_fileID,"Original ordered slot absent",null);continue;}
                    var observed=actualBones[slot.index];if(observed==null)result.observedNullSlots++;
                    if(slot.original_null)
                    {
                        if(sourceBones[slot.index]!=null||observed!=null)Bad(input,"m_Bones",slot.index,"0","Original zero/null slot was filled or differs",observed);
                        continue;
                    }
                    var sourceTarget=At(prefab.transform,slot.target_child_indices);var target=At(clone.transform,slot.target_child_indices);
                    bool sourceExact=sourceTarget!=null&&SourceId(sourceTarget,asset.GUID,slot.original_fileID)&&sourceBones[slot.index]==sourceTarget;
                    if(!sourceExact)Bad(input,"m_Bones",slot.index,slot.original_fileID,"Nonzero original slot source import/identity mismatch",observed);
                    if(target==null||observed!=target)Bad(input,"m_Bones",slot.index,slot.original_fileID,observed==null?"Nonzero original slot became null":"Nonzero original slot points to different Transform",observed);
                    else if(sourceExact)result.nonzeroSlotsVerified++;
                }
                if(input.root_bone_original_null)
                {
                    if(source.rootBone!=null||actual.rootBone!=null)Bad(input,"m_RootBone",-1,"0","Original zero/null rootBone differs",actual.rootBone);
                    else result.rootBonesVerified++;
                }
                else
                {
                    var sourceRoot=At(prefab.transform,input.root_bone_child_indices);var target=At(clone.transform,input.root_bone_child_indices);
                    if(!SourceId(sourceRoot,asset.GUID,input.root_bone_original_fileID)||source.rootBone!=sourceRoot||target==null||actual.rootBone!=target)Bad(input,"m_RootBone",-1,input.root_bone_original_fileID,"Original nonzero rootBone identity differs",actual.rootBone);
                    else result.rootBonesVerified++;
                }
            }
            result.sourceFileIdentifiersChecked=true;
            result.passed=result.mismatches.Count==0&&result.renderers==asset.renderers.Count&&result.boneSlotCount==asset.bone_slots&&result.originalNullSlots==asset.original_null_slots&&result.observedNullSlots==asset.original_null_slots&&result.nonzeroSlotsVerified==asset.bone_slots-asset.original_null_slots;
            return result;
        }
    }
}
