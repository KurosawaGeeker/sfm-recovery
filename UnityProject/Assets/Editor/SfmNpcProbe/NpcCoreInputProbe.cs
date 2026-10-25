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
    public static class NpcCoreInputProbe
    {
        const string Root="E:/ACTgame/SFM/ReverseEngineering";
        const string NpcGuid="791378a06555be54a99eb05e12d101e7";
        const string ManagerGuid="1fe6b30d2eb34b0aad848593cc30aa3a";
        static readonly string[] AvatarGuids={"83c75eab16269754d9ff8645cd88318a","00a8ac3f8d972154f8cbc687dd33ef2f","dace0fd16ce26ef4fad8381066d4f70f","d249b021817c9f743ad4d85bf75c0f3b","4364033ade803674f8e6d4cb9f72c671","3570ff0dc40b1cc4f89c87b28d2d5585","f41142aef1abbbb48b27dcc0229f34fa","b11458ef69cc2df438b3543ee3c9b738","ace9d69b68f1ff049970ec2db83e6493"};
        [Serializable] public sealed class AssetRow
        {
            public string role,guid,path,cloneName,headPath,animatorAvatar,animatorController;
            public bool actualComponent,headBelowClone,headSerialized,animatorPresent,avatarValid,avatarHuman,receiverPresent;
            public int renderers,skinnedMeshes,boneReferences,missingBones;
        }
        [Serializable] public sealed class Report
        {
            public string gate,unity,gameAssemblySHA256,failure;
            public bool passed,actualUnityEditor=true,playMode,originalNativeGameLoaded=false,originalSavesAccessed=false;
            public bool ninePrefabInputsObserved,realManagerInitializeReturned,realNcaConstructorReturned,controllerFullInitializeReturned;
            public bool originalFactoryReturned,originalRegistryListContainsNpc,originalRegistryDictContainsNpc;
            public bool realHead,lookAtHeadBound,navAgentOnNavMesh,detectorResultWrittenByProbe=false;
            public int checks,navAgentType,optionMaleRate,optionOldRate,npcCountBefore,npcCountAfter;
            public Vector3 npcPosition;
            public string[] scope;
            public List<AssetRow> assets=new List<AssetRow>();
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
            var meshes=avatar.GetComponentsInChildren<SkinnedMeshRenderer>(true);int bones=0,missing=0;
            foreach(var m in meshes)foreach(var b in m.bones){bones++;if(b==null)missing++;}
            Need(missing==0,role+" all serialized skinned bone references resolve");
            var row=new AssetRow{role=role,guid=guid,path=string.IsNullOrEmpty(guid)?null:AssetDatabase.GUIDToAssetPath(guid),cloneName=avatar.name,actualComponent=true,headPath=hp,headBelowClone=true,headSerialized=true,animatorPresent=true,avatarValid=a.avatar.isValid,avatarHuman=a.avatar.isHuman,animatorAvatar=AssetDatabase.GetAssetPath(a.avatar),animatorController=AssetDatabase.GetAssetPath(a.runtimeAnimatorController),receiverPresent=receiver!=null,renderers=avatar.GetComponentsInChildren<Renderer>(true).Length,skinnedMeshes=meshes.Length,boneReferences=bones,missingBones=missing};
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

        [MenuItem("SFM/QA/NPC Gate A - original inputs and NCA constructor")]
        public static void RunGateA()
        {
            string output=Begin("gate-a");GameObject host=null,managerObject=null;OptionData oldOptions=GameState.OptionData;
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
                var npc=Object.Instantiate(prefab,host.transform).GetComponent<NpcComponent>();Need(npc!=null,"Original Npc clone component");
                var controller=npc.CreateController();Need(controller!=null&&npc.NpcController==controller,"Original CreateController returned and bound original property");
                var state=new NpcStateModel();var nca=new NpcComponentAccessor(controller,npc,state,null);report.realNcaConstructorReturned=true;
                Need(nca.NpcController==controller&&nca.NpcComponent==npc&&nca.StateModel==state,"Original NCA parameter bindings");
                Need(nca.Player==null&&controller.Nca==null,"Constructor-only scope: no Player graph or Controller.Initialize result invented");
                Need(nca.NavAgent==npc.GetComponent<NavMeshAgent>()&&nca.NavAgent!=null,"Original real NavMeshAgent component binding");
                report.navAgentType=nca.NavAgent.agentTypeID;
                Need(nca.Rigidbody!=null&&nca.CapsuleCollider!=null&&nca.NpcCollider!=null&&npc.GetComponent<CharacterController>()!=null,"Original Rigidbody/Capsule/NpcCollider/CharacterController");
                Need(nca.AudioSource!=null,"Original prefab AudioSource exists (clip is not replaced)");
                Need(nca.PlayerSearcher!=null&&nca.PlayerSearcher.transform.IsChildOf(Read<Transform>(manager,"sightAreaParent")),"Original CreateSightArea actually instantiated Searcher under original parent");
                Need(npc.AvaterObject!=null&&npc.AvaterObject.transform.parent==npc.AvatarParent,"Original CreateAvatar selected and instantiated under original AvatarParent");
                ObserveAvatar(npc.AvaterObject,"original-NCA-selected-avatar",null);
                Need(nca.AvatarComponent==npc.AvaterObject.GetComponent<NpcAvatarComponent>()&&nca.Animator==npc.AvaterObject.GetComponent<Animator>()&&nca.AnimationEventReceiver==nca.Animator.GetComponent<NpcAnimationEventReceiver>(),"Original constructor binds actual avatar/animator/receiver");
                report.realHead=nca.AvatarComponent.Head!=null;
                report.scope=new[]{"All nine original asset alternatives directly inspected; only one genuine random selector/NCA construction executed","Real Game constructors/CreateController/Manager.Initialize/CreateSightArea/CreateAvatar run unhooked","Null player is constructor-only input; no full NCA.Initialize/Controller.Initialize, LookAt binding, route, NavMesh placement, movement or discovery accepted","New default OptionData is temporary fixture input only when absent; no persisted options or saves read","No singleton field/property, Head, detection flag, active result or fake AudioSource is written by this probe"};
                Need(report.engineErrors.Count==0,"No engine Error/Exception/Assert during fixture");report.passed=true;
            }
            catch(Exception e){report.failure=e.ToString();}
            finally
            {
                if(managerObject!=null)Object.DestroyImmediate(managerObject);
                if(host!=null)Object.DestroyImmediate(host);
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

        [MenuItem("SFM/QA/NPC Gate B - one original Konbini factory initialization")]
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
