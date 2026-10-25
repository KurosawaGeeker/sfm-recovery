using System;
using System.Collections.Generic;
using System.Reflection;
using ExposureUnnoticed2.Object3D.IngameManager;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;
namespace SfmMvp
{
    // Owned MVP resource adapter. Original Game code/lifecycle owns all gameplay.
    // The manifest must bind each resource to the original models container and
    // existing Title callback target before this source may be deployed.
    [DefaultExecutionOrder(-31000)]
    public sealed class OriginalGameplayBootstrapV2 : MonoBehaviour
    {
        [Serializable]
        public sealed class ResourceBinding
        {
            public string originalBundleKey;
            public string originalContainer;
            public string originalGuid;
            public string field;
            public int index = -1;
            public UnityEngine.Object asset;
            public UnityEngine.Object expectedBefore;
        }
        public ResourceBinding[] bindings;
        public GameObject originalInGameManagerPrefab;
        public bool Ready { get; private set; }
        public string Failure { get; private set; }
        public int AppliedBindings { get; private set; }
        public int ExistingBindings { get; private set; }
        bool began;
        static void Need(bool condition,string label){if(!condition)throw new InvalidOperationException(label);}
        void Start()
        {
            if(began)return;began=true;
            try
            {
                // Owned resource bootstrap identity only; the Player build keeps the original product requirement.
                bool acceptedProduct=Application.productName=="SFMOriginalGameplayRuntimeV2";
#if UNITY_EDITOR
                const string diagnosticPrefix="SFMOriginalGameplayV23Diag-";
                string diagnosticProduct=Application.productName;
                if(Array.IndexOf(Environment.GetCommandLineArgs(),"-sfmNpcDetectionV23Contract")>=0
                    &&diagnosticProduct.StartsWith(diagnosticPrefix,StringComparison.Ordinal)
                    &&diagnosticProduct.Length==diagnosticPrefix.Length+32
                    &&Guid.TryParseExact(diagnosticProduct.Substring(diagnosticPrefix.Length),"N",out _))acceptedProduct=true;
                const string successPrefix="SFMOriginalGameplaySuccessDiag-";
                if(Array.IndexOf(Environment.GetCommandLineArgs(),"-sfmHomeStreetSuccessV1Contract")>=0
                    &&diagnosticProduct.StartsWith(successPrefix,StringComparison.Ordinal)
                    &&diagnosticProduct.Length==successPrefix.Length+32
                    &&Guid.TryParseExact(diagnosticProduct.Substring(successPrefix.Length),"N",out _))acceptedProduct=true;
#endif
                // Owned standalone QA identity only. Game lifecycle/save implementation stays unchanged.
                const string visualPrefix="SFMOriginalGameplayInputVisual-";
                string visualProduct=Application.productName;
                if((Array.IndexOf(Environment.GetCommandLineArgs(),"--sfm-home-input-visual")>=0 || Array.IndexOf(Environment.GetCommandLineArgs(),"--sfm-manual-preview")>=0)
                    &&visualProduct.StartsWith(visualPrefix,StringComparison.Ordinal)
                    &&visualProduct.Length==visualPrefix.Length+32
                    &&Guid.TryParseExact(visualProduct.Substring(visualPrefix.Length),"N",out _))acceptedProduct=true;
                Need(Application.companyName=="SFMRecovery"&&acceptedProduct,"Independent gameplay V2 or explicitly isolated Editor diagnostic persistent-data identity required");
                Need(bindings!=null&&bindings.Length==25,"Exact original Title models-request count");
                // Apartment already has the original system. Residence relies on
                // the original Resources prefab. Its original Awake owns Instance.
                SystemManager.CreateIfNoExist();
                var assets=AssetReferencer.Instance;
                Need(assets,"Original AssetReferencer.Awake must establish the real instance");
                var seen=new HashSet<string>();
                var writes=new List<Action>();
                foreach(var row in bindings)
                {
                    Need(row!=null&&row.asset&&!string.IsNullOrEmpty(row.originalBundleKey)&&!string.IsNullOrEmpty(row.originalContainer)&&row.originalGuid!=null&&row.originalGuid.Length==32,"Complete original resource identity binding");
                    Need(ReviewedTitleIdentityV2.Match(row),"Exact reviewed original Title resource identity/target required");
                    Need(seen.Add(row.field+":"+row.index),"Duplicate original Title resource target");
                    var field=typeof(AssetReferencer).GetField(row.field,BindingFlags.Public|BindingFlags.Instance);
                    Need(field!=null&&!field.IsStatic,"Original public AssetReferencer resource field required");
                    UnityEngine.Object current;
                    if(row.index<0)
                    {
                        Need(typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType)&&field.FieldType.IsInstanceOfType(row.asset),"Exact original resource field type");
                        current=(UnityEngine.Object)field.GetValue(assets);
                        if(current!=row.asset)writes.Add(()=>field.SetValue(assets,row.asset));
                    }
                    else
                    {
                        Need(field.FieldType.IsArray&&typeof(UnityEngine.Object).IsAssignableFrom(field.FieldType.GetElementType()),"Original resource array type");
                        var array=field.GetValue(assets)as Array;
                        Need(array!=null&&row.index<array.Length&&field.FieldType.GetElementType().IsInstanceOfType(row.asset),"Existing original serialized resource-array slot/type");
                        current=(UnityEngine.Object)array.GetValue(row.index);
                        if(current!=row.asset)writes.Add(()=>array.SetValue(row.asset,row.index));
                    }
                    if(current==row.asset)ExistingBindings++;
                    else Need(current==row.expectedBefore,"Existing resource must match the exact original serialized before-reference: "+row.field+":"+row.index);
                }
                // Preflight every resource before any assignment. Only these
                // resource fields change; no save/rank/task/currency data is set.
                foreach(var write in writes){write();AppliedBindings++;}
                var managers=UnityEngine.Object.FindObjectsOfType<InGameManager>();
                var instance=InGameManager.Instance;
                bool persistent=instance&&IsPersistentManager(instance);
                Need(!instance||persistent,"Existing original InGameManager must be persistent");
                Need(managers.Length<=2,"Bounded original InGameManager population");
                if(managers.Length==2)
                {
                    bool member=instance&&(managers[0]==instance||managers[1]==instance);
                    var copy=member?(managers[0]==instance?managers[1]:managers[0]):null;
                    Need(ReturnPairIsAllowed(managers.Length,persistent,member,IsApartmentCopy(copy)),
                        "Only original persistent manager plus reviewed Apartment scene copy");
                    expectedManager=instance;
                }
                else
                {
                    Need(!instance||(managers.Length==1&&managers[0]==instance),
                        "Original singleton must match the enumerated manager");
                    if(managers.Length==0)
                    {
                        Need(originalInGameManagerPrefab&&originalInGameManagerPrefab.GetComponent<InGameManager>(),"Original serialized InGameManager prefab required");
                        expectedManager=UnityEngine.Object.Instantiate(originalInGameManagerPrefab).GetComponent<InGameManager>();
                    }
                    else expectedManager=managers[0];
                }
                Need(expectedManager&&expectedManager.enabled&&expectedManager.gameObject.activeInHierarchy,
                    "Expected original manager must run its own Start");
                preservePlayer=persistent;
                expectedPlayer=preservePlayer?global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance:null;
                Need(!preservePlayer||(expectedPlayer&&expectedPlayer.transform.IsChildOf(expectedManager.transform)),
                    "Persistent original player identity and ownership required");
                // Original Start performs initialization or duplicate destruction.
                // Verify after one complete SDK frame; do not invoke that lifecycle.
                verificationStartFrame=Time.frameCount;
                verificationPending=true;
            }
            catch(Exception e){Failure=e.ToString();Debug.LogError("SFM_ORIGINAL_GAMEPLAY_V2 "+Failure);enabled=false;}
        }
        // Appended owned fields keep the existing resource-lambda identities.
        InGameManager expectedManager;
        global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController expectedPlayer;
        bool preservePlayer;
        int verificationStartFrame;
        bool verificationPending;
        static bool ReturnPairIsAllowed(int count,bool persistent,bool member,bool apartmentCopy)
        {
            return count==2&&persistent&&member&&apartmentCopy;
        }
        static bool IsNextVerificationFrame(int start,int current)
        {
            return start<int.MaxValue&&current==start+1;
        }
        static bool ManagerSurvived(int count,bool expectedAlive,bool onlyExpected,bool sameInstance,
            bool playerAlive,bool samePlayer,bool playerBelongs)
        {
            return count==1&&expectedAlive&&onlyExpected&&sameInstance&&playerAlive&&samePlayer&&playerBelongs;
        }
        static bool IsPersistentManager(InGameManager manager)
        {
            if(!manager)return false;
            var scene=manager.gameObject.scene;
            return scene.IsValid()&&scene.isLoaded&&scene.path=="DontDestroyOnLoad"&&scene.name=="DontDestroyOnLoad";
        }
        bool IsApartmentCopy(InGameManager manager)
        {
            if(!manager||!manager.enabled||!manager.gameObject.activeInHierarchy)return false;
            var scene=manager.gameObject.scene;
            if(!scene.IsValid()||!scene.isLoaded||scene!=gameObject.scene||
                scene.path!="Assets/SfmOriginalGameplayV3/Apartment.unity"||
                manager.gameObject.name!="InGameManager"||manager.transform.parent)return false;
            var referencer=manager.GetComponent<InGameReferencer>();
            return referencer&&referencer.inGameManager==manager&&manager.GetComponent<EventMethodManager>();
        }
        void Update()
        {
            if(!verificationPending)return;
            try
            {
                int currentFrame=Time.frameCount;
                if(currentFrame==verificationStartFrame)return;
                Need(IsNextVerificationFrame(verificationStartFrame,currentFrame),
                    "Original manager verification must occur on the next SDK frame");
                verificationPending=false;
                var managers=UnityEngine.Object.FindObjectsOfType<InGameManager>();
                var player=global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance;
                Need(ManagerSurvived(managers.Length,expectedManager,
                    managers.Length==1&&managers[0]==expectedManager,InGameManager.Instance==expectedManager,
                    player,!preservePlayer||(expectedPlayer&&player==expectedPlayer),
                    player&&expectedManager&&player.transform.IsChildOf(expectedManager.transform)),
                    "Original manager and player must survive uniquely after their own lifecycle");
                Need(IsPersistentManager(expectedManager),"Original Start must establish manager persistence");
                Ready=true;
                BodyPreviewFramingV1.Attach(expectedManager);
                Debug.Log("SFM_ORIGINAL_GAMEPLAY_V2 resources-ready applied="+AppliedBindings+" existing="+ExistingBindings);
            }
            catch(Exception e)
            {
                verificationPending=false;Failure=e.ToString();
                Debug.LogError("SFM_ORIGINAL_GAMEPLAY_V2 "+Failure);enabled=false;
            }
        }
    }
}
