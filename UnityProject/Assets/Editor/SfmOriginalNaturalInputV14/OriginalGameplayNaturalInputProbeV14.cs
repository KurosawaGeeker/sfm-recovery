#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Object3D.IngameManager;
using ExposureUnnoticed2.Object3D.Player.Scripts;
namespace SfmMvp.Editor
{
    // Observation only. Original MonoBehaviour messages own all Game calls.
    [InitializeOnLoad]
    public static class OriginalGameplayNaturalInputProbeV14
    {
        const string Project="E:/ACTgame/SFM/ReverseEngineering/tools/unity-editor/core-gameplay-bootstrap-runtime-candidate-v2/Project";
        const string Key="SfmOriginalNaturalInputV14.";
        static readonly string Domain=Guid.NewGuid().ToString("N");
        [Serializable]public sealed class Pin {public string path,sha256;public long bytes;}
        [Serializable]public sealed class Contract {public Pin[] inputs;public string editorGameSHA,scene,product,bindingEditorSHA;public bool bindingPassed;public int originalMethods,originalOwners,bindingChecks,originalMethodExclusions,originalFieldExclusions;}
        [Serializable]public sealed class Sample {public int frame,managers,players,updaters,lateUpdaters,starters,resourceAssignments;public bool adapterReady,realPlayerBound,pcaPresent,dispatcherBound,stageControllerPresent;public double currentRp,earnedRp;public int missionProgressEntries;public bool baseCameraReady,thirdPersonReady,firstPersonReady,inventoryTargetReady,fovMatchesOriginalOption;public float cameraFov;public int cameraMask;public int inputBindingManagers,inputActionMaps;public bool inputBindingInstanceBound,inputActionAssetPresent,inputFilenameMatchesOriginal,inputOverridesFilePresent;public bool queuedW,inputMapEnabled,characterGrounded,characterEnabled;public float actionX,actionY,worldX,worldY,worldZ,horizontalStep,calcRealtime,calcJust,calcLast,currentMoveSpeed;public bool virtualKeyboardW,virtualKeyboardPaired,mapHasVirtualKeyboard;public int pairedDevices,moveControls;public string controlScheme;public int activePlayerInputs,unpairedDeviceActivity,currentPlayerState;public bool inputAutoSwitchDisabled,applicationFocused,moveActionEnabled;public string moveActionPhase,editorInputRouting,backgroundBehavior;}
        [Serializable]public sealed class GameInputSample {public int frame;public float actionX,actionY;public bool rawEscape,cancelActionPressed,rawW,enabled,characterGrounded,originalGroundTrigger,originalDownhillTrigger,floorSupport;public float floorDistance;public string phase,updateType,floorCollider;}
        [Serializable]public sealed class UIAssetInput {public string field,guid,path;public long fileID;public int sourceMonoDocs;}
        [Serializable]public sealed class UIInputManifest {public UIAssetInput[] assets;}
        [Serializable]public sealed class UIAssetObservation {public string field,path,guid,componentType;public long fileID;public int monoComponents,missingScripts,sourceMonoDocs;public bool loaded,identityMatches;public int inventoryButtons,validInventoryCallbacks;}
        [Serializable]public sealed class CoreReadiness {public bool sampled,originalPresetNamesReady,originalUiPlayerBound,originalUiLayersBound,VolumeComponentBound,VolumeLayerFieldMatches,VolumeLayerPresent,originalLensBound;public string[] cosplayNames,customizeNames;public int eventSystems,activeInputModules;public string currentInputModule;public List<UIAssetObservation> assets=new List<UIAssetObservation>();}
        [Serializable]public sealed class UIState {public int frame,menus,menuState,inventoryPanels,originalCancelCount;public bool menuStaticBound,menuClone,menuChild,inventoryStaticBound,inventoryClone,inventoryActive,inventoryButtonActive,inventoryButtonInteractable,exactInventoryPersistentTarget,queuedTab,queuedEscape,queuedMouse,queuedClick;public float menuAlpha,mouseX,mouseY;public string menuParent,inventoryParent;}
        [Serializable]public sealed class Report
        {
            public bool passed,actualPlayMode,domainReloadObserved,manualGameTicks=false,GameHooks=false,ownedFunding=false,ownedMissionProgress=false,originalSavesAccessed=false,ownedPersistentDataOnly=true;
            public bool SDKEditorRoutingSetup=true,SDKRoutingRestored=false;public string previousEditorRouting,previousBackgroundBehavior;public bool SDKVirtualKeyboardSetup=true,SDKInputUserDevicePairing=false,SDKVirtualMouseSetup=true,originalSDKAutoSchemeSelection=true,SDKExplicitOriginalKeyboardMouseProfileSetup=false,SDKQueuedKeyboardEvents=true,dynamicInputMovementAccepted=false;public bool fullGameplayAccepted=false;public string failure,beginDomain,playDomain,editorGameSHA,persistentDataPath,contractSHA;
            public bool SDKDynamicActionSamplesAccepted,originalHorizontalMovementObserved,originalGroundAndPhysicsSupportAccepted,CharacterControllerGroundedFlagAcceptance;public List<GameInputSample> gameInputSamples=new List<GameInputSample>();
            public List<UIState> uiSamples=new List<UIState>();public bool naturalMenuAndInventoryAccepted=false,naturalInventoryReturnAccepted=false;public CoreReadiness coreReadiness=new CoreReadiness();
            public int checks;public List<string> transitionErrors=new List<string>();public List<string> errors=new List<string>();public List<Sample> samples=new List<Sample>();
        }
        static Report report;static bool finished;static int lastFrame=-1;static UnityEngine.InputSystem.Keyboard keyboard;static UnityEngine.InputSystem.Mouse mouse;static Vector3 previousPosition;static bool hasPreviousPosition;
        static bool Armed=>SessionState.GetBool(Key+"armed",false);
        // Mono rejects some absolute Windows paths at 260 characters. Resolve only
        // files inside the exact Project relative to the verified process cwd.
        static string IoPath(string p){string full=Path.GetFullPath(p),root=Path.GetFullPath(Project)+Path.DirectorySeparatorChar;return full.StartsWith(root,StringComparison.OrdinalIgnoreCase)?full.Substring(root.Length):full;}
        [System.Runtime.InteropServices.DllImport("kernel32.dll",CharSet=System.Runtime.InteropServices.CharSet.Unicode,SetLastError=true)]static extern IntPtr CreateFileW(string path,uint access,uint share,IntPtr security,uint creation,uint flags,IntPtr template);
        [System.Runtime.InteropServices.DllImport("kernel32.dll",SetLastError=true)]static extern bool GetFileSizeEx(IntPtr handle,out long size);
        [System.Runtime.InteropServices.DllImport("kernel32.dll",SetLastError=true)]static extern bool ReadFile(IntPtr handle,byte[] buffer,uint count,out uint read,IntPtr overlapped);
        [System.Runtime.InteropServices.DllImport("kernel32.dll",SetLastError=true)]static extern bool CloseHandle(IntPtr handle);
        static byte[] ReadBoundFile(string p)
        {
            string full=Path.GetFullPath(p);if(full.Length<260)return File.ReadAllBytes(full);
            // Mono normalizes relative paths back to MAX_PATH-limited absolute paths.
            // Read the same exact existing file through the Windows extended-path API.
            IntPtr handle=CreateFileW(@"\\?\"+full,0x80000000,7,IntPtr.Zero,3,128,IntPtr.Zero);
            if(handle==new IntPtr(-1))throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error(),full);
            try{long size;if(!GetFileSizeEx(handle,out size))throw new System.ComponentModel.Win32Exception(System.Runtime.InteropServices.Marshal.GetLastWin32Error());if(size<0||size>int.MaxValue)throw new IOException("Bound input size");var bytes=new byte[(int)size];uint read;if(!ReadFile(handle,bytes,(uint)bytes.Length,out read,IntPtr.Zero)||read!=bytes.Length)throw new IOException("Exact bound input read failed: "+full);return bytes;}finally{CloseHandle(handle);}
        }
        static string Hash(string p){using(var h=SHA256.Create())return BitConverter.ToString(h.ComputeHash(ReadBoundFile(p))).Replace("-","");}
        static void Load(){if(report==null)report=JsonUtility.FromJson<Report>(SessionState.GetString(Key+"report","{}"));}
        static void Save(){SessionState.SetString(Key+"report",JsonUtility.ToJson(report));}
        static void Need(bool b,string label){report.checks++;if(!b)throw new InvalidOperationException(label);}
        static string Arg(string n){var a=Environment.GetCommandLineArgs();int k=Array.IndexOf(a,n);if(k<0||k+1>=a.Length)throw new InvalidOperationException(n);return a[k+1];}
        static object Field(object value,string name){return value==null?null:value.GetType().GetField(name,BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance)?.GetValue(value);}
        static int Count(object value){return value is ICollection c?c.Count:-1;}
        static OriginalGameplayNaturalInputProbeV14(){EditorApplication.update+=Poll;EditorApplication.playModeStateChanged+=Changed;Application.logMessageReceived+=Log;UnityEngine.InputSystem.InputSystem.onAfterUpdate+=AfterGameInput;}
        public static void Begin()
        {
            if(Armed)throw new InvalidOperationException("Already armed");report=new Report{beginDomain=Domain};
            try
            {
                // Establish the owned report destination before any preflight can fail.
                string diagnosticOutput=Path.GetFullPath(Arg("-sfmOriginalV2Output"));
                Need(!Directory.Exists(diagnosticOutput)&&diagnosticOutput.StartsWith(Path.GetFullPath(Path.Combine(Project,"../runs"))+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase),"Fresh owned diagnostic output");
                Directory.CreateDirectory(diagnosticOutput);SessionState.SetString(Key+"output",diagnosticOutput);Save();
                Need(Path.GetFullPath(Path.Combine(Application.dataPath,".."))==Path.GetFullPath(Project),"Exact independent Project");Need(string.Equals(Path.GetFullPath(Environment.CurrentDirectory),Path.GetFullPath(Project),StringComparison.OrdinalIgnoreCase),"Exact Project cwd for relative IO");
                Need(!EditorApplication.isPlayingOrWillChangePlaymode&&!EditorSettings.enterPlayModeOptionsEnabled,"Fresh default domain reload");
                string cp=Path.GetFullPath(Arg("-sfmOriginalV2Contract"));report.contractSHA=Arg("-sfmOriginalV2ContractSHA");Need(Hash(cp)==report.contractSHA,"Exact launch contract");var c=JsonUtility.FromJson<Contract>(File.ReadAllText(cp));
                foreach(var p in c.inputs)Need(File.Exists(IoPath(p.path))&&new FileInfo(IoPath(p.path)).Length==p.bytes&&Hash(p.path)==p.sha256,"Bound source/scene/resource input "+p.path);
                report.editorGameSHA=Hash(typeof(InGameManager).Assembly.Location);Need(report.editorGameSHA==c.editorGameSHA&&c.editorGameSHA==c.bindingEditorSHA&&c.bindingPassed&&c.originalMethods==7160&&c.originalOwners==1292&&c.bindingChecks>=17279&&c.originalMethodExclusions==0&&c.originalFieldExclusions==0,"Exact all-original-linked Editor Game");
                Need(Application.companyName=="SFMRecovery"&&Application.productName==c.product&&c.product=="SFMOriginalGameplayRuntimeV2","Independent persistent identity");report.persistentDataPath=Application.persistentDataPath;
                var args=Environment.GetCommandLineArgs();foreach(string flag in new[]{"-sfmCoreSmoke","-sfmEquipmentSmoke","-sfmOriginalActionSmoke","-sfmMvpOriginalBundle","-sfmNpcAnimationProbeV1"})Need(Array.IndexOf(args,flag)<0,"No alternate/funding entry "+flag);
                SessionState.SetString(Key+"deadline",DateTime.UtcNow.AddSeconds(150).Ticks.ToString());SessionState.SetBool(Key+"finished",false);Save();SessionState.SetBool(Key+"armed",true);
                EditorSceneManager.OpenScene(c.scene,OpenSceneMode.Single);EditorApplication.isPlaying=true;
            }
            catch(Exception e){Finish(e);}
        }
        static void Log(string message,string stack,LogType kind)
        {
            if(!Armed||finished||kind!=LogType.Exception&&kind!=LogType.Error&&kind!=LogType.Assert)return;Load();if(EditorApplication.isPlaying)report.errors.Add(kind+": "+message+"\n"+stack);else report.transitionErrors.Add(kind+": "+message+"\n"+stack);Save();
        }
        static void ObserveCoreReadiness(InGameManager manager)
        {
            var r=report.coreReadiness;r.sampled=true;var gsd=GameState.GameStateData;r.cosplayNames=gsd.CosplayMySetNames==null?null:(string[])gsd.CosplayMySetNames.Clone();r.customizeNames=gsd.CustomizeMySetNames==null?null:(string[])gsd.CustomizeMySetNames.Clone();r.originalPresetNamesReady=r.cosplayNames!=null&&r.customizeNames!=null&&r.cosplayNames.Length==20&&r.customizeNames.Length==20&&r.cosplayNames.All(x=>!string.IsNullOrEmpty(x))&&r.customizeNames.All(x=>!string.IsNullOrEmpty(x));
            var ui=ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance;r.originalUiPlayerBound=ui&&ReferenceEquals(Field(ui,"player"),manager.InGameReferencer.Player);r.originalUiLayersBound=ui&&ui.middleCloneLayer&&ui.frontLayer&&ui.BackLayer;
            var volume=manager.InGameReferencer.GlobalVolume;if(volume){var actual=volume.GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessVolume>();var layer=volume.GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessLayer>();r.VolumeComponentBound=actual&&ReferenceEquals(Field(volume,"volume"),actual);r.VolumeLayerPresent=layer;r.VolumeLayerFieldMatches=ReferenceEquals(Field(volume,"layer"),layer);var lens=Field(volume,"lensDistortion")as UnityEngine.Rendering.PostProcessing.LensDistortion;r.originalLensBound=lens;}
            var events=UnityEngine.Object.FindObjectsOfType<UnityEngine.EventSystems.EventSystem>();r.eventSystems=events.Length;r.activeInputModules=events.Sum(x=>x.GetComponents<UnityEngine.EventSystems.BaseInputModule>().Count(y=>y.isActiveAndEnabled));var current=UnityEngine.EventSystems.EventSystem.current;r.currentInputModule=current&&current.currentInputModule?current.currentInputModule.GetType().FullName:null;
            var input=JsonUtility.FromJson<UIInputManifest>(File.ReadAllText(Arg("-sfmCoreUIManifest")));var ar=ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance;
            foreach(var row in input.assets){var s=new UIAssetObservation{field=row.field,sourceMonoDocs=row.sourceMonoDocs};var panel=ar?typeof(ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer).GetField(row.field).GetValue(ar)as Common.Scripts.UI.BasePanelView:null;s.loaded=panel;if(panel){s.componentType=panel.GetType().FullName;s.path=AssetDatabase.GetAssetPath(panel);string guid;long id;bool found=AssetDatabase.TryGetGUIDAndLocalFileIdentifier(panel,out guid,out id);s.guid=guid;s.fileID=id;s.identityMatches=found&&guid==row.guid&&id==row.fileID&&s.path==row.path;var monos=panel.gameObject.GetComponentsInChildren<MonoBehaviour>(true);s.monoComponents=monos.Length;s.missingScripts=monos.Count(x=>!x);if(row.field=="InGameMenu")foreach(var button in panel.gameObject.GetComponentsInChildren<UnityEngine.UI.Button>(true))for(int k=0;k<button.onClick.GetPersistentEventCount();k++)if(button.onClick.GetPersistentMethodName(k)=="OnClickInventory"){s.inventoryButtons++;var target=button.onClick.GetPersistentTarget(k);if(target==panel&&(int)button.onClick.GetPersistentListenerState(k)==2)s.validInventoryCallbacks++;}}r.assets.Add(s);}
            Need(r.originalPresetNamesReady,"Original Load produced both20 nonempty preset-name arrays");Need(r.originalUiPlayerBound&&r.originalUiLayersBound,"Original UI manager Player and actual panel parent layers bound");Need(r.VolumeComponentBound&&r.VolumeLayerFieldMatches&&r.originalLensBound,"Original Volume Awake populated actual component and LensDistortion");Need(r.eventSystems>0&&r.activeInputModules>0,"Original active event system and input module exist");foreach(var s in r.assets)Need(s.loaded&&s.identityMatches&&s.monoComponents>0&&s.missingScripts==0,"Exact original core UI asset and scripts loaded "+s.field);var menu=r.assets.Single(x=>x.field=="InGameMenu");Need(menu.inventoryButtons==1&&menu.validInventoryCallbacks==1,"Original menu Inventory Button has exact persistent target/method/runtime call state");
        }
        static UIState ObserveAndQueueUI(int frame)
        {
            var r=new UIState{frame=frame,queuedTab=frame==15,queuedEscape=frame==100};
            var counts=typeof(ExposureUnnoticed2.Scripts.Base.InputManager).GetField("inputDict",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)as System.Collections.IDictionary;var cancel=ExposureUnnoticed2.Scripts.Base.InputManager.InputType.Cancel;r.originalCancelCount=counts!=null&&counts.Contains(cancel)?(int)counts[cancel]:-1;
            var menus=UnityEngine.Object.FindObjectsOfType<ExposureUnnoticed2.ObjectUI.InGameMenu.InGameMenuView>();r.menus=menus.Length;
            var inventory=UnityEngine.Object.FindObjectsOfType<ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryPanelView>();r.inventoryPanels=inventory.Length;
            UnityEngine.UI.Button target=null;
            if(menus.Length==1)
            {
                var menu=menus[0];r.menuStaticBound=ReferenceEquals(ExposureUnnoticed2.ObjectUI.InGameMenu.InGameMenuView.Instance,menu);r.menuClone=string.IsNullOrEmpty(UnityEditor.AssetDatabase.GetAssetPath(menu));r.menuState=Convert.ToInt32(Field(menu,"currentState"));r.menuChild=Field(menu,"isOpenChild")is bool child&&child;r.menuParent=menu.transform.parent?menu.transform.parent.name:null;
                var group=Field(menu,"baseCanvasGroup")as CanvasGroup;if(group)r.menuAlpha=group.alpha;
                foreach(var button in menu.GetComponentsInChildren<UnityEngine.UI.Button>(true))for(int k=0;k<button.onClick.GetPersistentEventCount();k++)if(button.onClick.GetPersistentMethodName(k)=="OnClickInventory"&&button.onClick.GetPersistentTarget(k)==menu&&(int)button.onClick.GetPersistentListenerState(k)==2){if(target)throw new InvalidOperationException("Unique actual original Inventory button");target=button;r.exactInventoryPersistentTarget=true;}
                if(target){r.inventoryButtonActive=target.gameObject.activeInHierarchy;r.inventoryButtonInteractable=target.interactable;}
            }
            if(inventory.Length==1){var panel=inventory[0];r.inventoryStaticBound=ReferenceEquals(ExposureUnnoticed2.ObjectUI.InventoryPanel.InventoryPanelView.Instance,panel);r.inventoryClone=string.IsNullOrEmpty(UnityEditor.AssetDatabase.GetAssetPath(panel));r.inventoryActive=panel.gameObject.activeInHierarchy;r.inventoryParent=panel.transform.parent?panel.transform.parent.name:null;}
            if(target&&frame>=60&&frame<=63)
            {
                var rect=target.GetComponent<RectTransform>();var canvas=target.GetComponentInParent<Canvas>();var root=canvas?canvas.rootCanvas:null;Camera camera=root&&root.renderMode!=RenderMode.ScreenSpaceOverlay?root.worldCamera:null;
                Vector2 point=RectTransformUtility.WorldToScreenPoint(camera,rect.TransformPoint(rect.rect.center));r.mouseX=point.x;r.mouseY=point.y;r.queuedMouse=true;r.queuedClick=frame==61;
                var state=new UnityEngine.InputSystem.LowLevel.MouseState{position=point}.WithButton(UnityEngine.InputSystem.LowLevel.MouseButton.Left,r.queuedClick);
                UnityEngine.InputSystem.InputSystem.QueueStateEvent(mouse,state);
            }
            report.uiSamples.Add(r);return r;
        }
        static void AfterGameInput()
        {
            if(!Armed||finished||!EditorApplication.isPlaying||UnityEngine.InputSystem.LowLevel.InputState.currentUpdateType!=UnityEngine.InputSystem.LowLevel.InputUpdateType.Dynamic)return;
            Load();var originalInput=typeof(ExposureUnnoticed2.Scripts.Base.InputManager).GetField("playerInput",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)as UnityEngine.InputSystem.PlayerInput;
            if(!originalInput||originalInput.currentActionMap==null||keyboard==null)return;
            var action=originalInput.currentActionMap["Move"];var value=action.ReadValue<Vector2>();var s=new GameInputSample{frame=Time.frameCount,actionX=value.x,actionY=value.y,enabled=action.enabled,rawW=keyboard.wKey.isPressed,rawEscape=keyboard.escapeKey.isPressed,cancelActionPressed=originalInput.currentActionMap["Cancel"].ReadValue<float>()>0.5f,phase=action.phase.ToString(),updateType=UnityEngine.InputSystem.LowLevel.InputState.currentUpdateType.ToString()};
            var managers=UnityEngine.Object.FindObjectsOfType<InGameManager>();if(managers.Length==1&&managers[0].InGameReferencer.Player&&managers[0].InGameReferencer.Player.Pca!=null){var pca=managers[0].InGameReferencer.Player.Pca;s.characterGrounded=pca.CharacterController.isGrounded;var checker=pca.PlayerObjectReferencer.OnGroundedChecker;var downhill=pca.PlayerObjectReferencer.DownhillChecker;s.originalGroundTrigger=checker&&checker.IsGrounded;s.originalDownhillTrigger=downhill&&downhill.IsGrounded;if(checker){int mask=(int)Field(checker,"ignoreLayerMask");RaycastHit hit;s.floorSupport=Physics.Raycast(pca.Transform.position+Vector3.up*0.2f,Vector3.down,out hit,0.5f,~mask,QueryTriggerInteraction.Ignore);if(s.floorSupport){s.floorDistance=hit.distance;s.floorCollider=hit.collider.gameObject.name;}}}
            report.gameInputSamples.Add(s);Save();
        }
        static void Poll()
        {
            if(!Armed||finished)return;Load();try
            {
                if(DateTime.UtcNow.Ticks>long.Parse(SessionState.GetString(Key+"deadline","0")))throw new TimeoutException("Natural original lifecycle deadline");
                if(!EditorApplication.isPlaying)return;
                report.actualPlayMode=true;report.playDomain=Domain;report.domainReloadObserved=Domain!=report.beginDomain;
                int frame=Time.frameCount;if(frame==lastFrame||frame<1)return;lastFrame=frame;
                var adapters=UnityEngine.Object.FindObjectsOfType<OriginalGameplayBootstrapV2>();var managers=UnityEngine.Object.FindObjectsOfType<InGameManager>();var players=UnityEngine.Object.FindObjectsOfType<PlayerController>();
                var s=new Sample{frame=frame,managers=managers.Length,players=players.Length};var adapter=adapters.SingleOrDefault();s.adapterReady=adapter&&adapter.Ready;s.resourceAssignments=adapter?adapter.AppliedBindings+adapter.ExistingBindings:0;
                var manager=managers.SingleOrDefault();if(manager){var ir=manager.InGameReferencer;s.realPlayerBound=ir&&ir.Player&&players.Contains(ir.Player);s.pcaPresent=s.realPlayerBound&&ir.Player.Pca!=null;s.stageControllerPresent=manager.CurrentStageController;var dispatcher=manager.GetComponent<EventMethodManager>();s.dispatcherBound=dispatcher&&ReferenceEquals(Field(dispatcher,"inGameReferencer"),ir);s.updaters=Count(Field(dispatcher,"updaterList"));s.lateUpdaters=Count(Field(dispatcher,"lateUpdaterList"));s.starters=Count(Field(dispatcher,"starterList"));}
                if(s.realPlayerBound&&manager.InGameReferencer.Player.PlayerAvatarObjectReferencer)
                {
                    var ir=manager.InGameReferencer;var bc=ir.BaseCameraController;var camera=Field(bc,"camera")as UnityEngine.Camera;
                    s.baseCameraReady=bc&&camera&&bc.Camera==camera;
                    s.thirdPersonReady=ReferenceEquals(Field(ir.TpCameraController,"player"),ir.Player)&&ReferenceEquals(Field(ir.TpCameraController,"baseCamera"),bc);
                    var fp=ir.Player.PlayerAvatarObjectReferencer.FPCameraController;
                    s.firstPersonReady=ReferenceEquals(Field(fp,"player"),ir.Player)&&ReferenceEquals(Field(fp,"baseCamera"),bc);
                    s.inventoryTargetReady=ReferenceEquals(Field(ir.InventoryCameraController,"cameraTarget"),ir.Player.Pca.PlayerAvatarObjectReferencer.CameraTarget);
                    if(camera){s.cameraFov=camera.fieldOfView;s.cameraMask=camera.cullingMask;if(GameState.OptionData!=null){int expected=ir.Player.Pca.PlayerState.IsStrafe?GameState.OptionData.CameraAngle1st:GameState.OptionData.CameraAngle3rd;s.fovMatchesOriginalOption=Mathf.Abs(s.cameraFov-expected)<0.001f;}}
                }
                var inputManagers=UnityEngine.Object.FindObjectsOfType<ExposureUnnoticed2.Scripts.Base.InputRebindSaveManager>();
                s.inputBindingManagers=inputManagers.Length;var binding=ExposureUnnoticed2.Scripts.Base.InputRebindSaveManager.Instance;
                s.inputBindingInstanceBound=binding&&inputManagers.Length==1&&ReferenceEquals(binding,inputManagers[0]);
                if(binding){var asset=Field(binding,"_actionAsset")as UnityEngine.InputSystem.InputActionAsset;s.inputActionAssetPresent=asset;if(asset)s.inputActionMaps=asset.actionMaps.Count;string filename=Field(binding,"_savePath")as string;s.inputFilenameMatchesOriginal=filename=="InputActionOverrides.json";if(s.inputFilenameMatchesOriginal)s.inputOverridesFilePresent=File.Exists(Path.Combine(Application.persistentDataPath,filename));}
                if(!SessionState.GetBool(Key+"routingSetup",false))
                {
                    var settings=UnityEngine.InputSystem.InputSystem.settings;
                    SessionState.SetInt(Key+"oldRouting",(int)settings.editorInputBehaviorInPlayMode);SessionState.SetInt(Key+"oldBackground",(int)settings.backgroundBehavior);
                    report.previousEditorRouting=settings.editorInputBehaviorInPlayMode.ToString();report.previousBackgroundBehavior=settings.backgroundBehavior.ToString();
                    SessionState.SetBool(Key+"routingSetup",true);
                    settings.editorInputBehaviorInPlayMode=UnityEngine.InputSystem.InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView;
                    settings.backgroundBehavior=UnityEngine.InputSystem.InputSettings.BackgroundBehavior.IgnoreFocus;
                }
                s.applicationFocused=Application.isFocused;s.editorInputRouting=UnityEngine.InputSystem.InputSystem.settings.editorInputBehaviorInPlayMode.ToString();s.backgroundBehavior=UnityEngine.InputSystem.InputSystem.settings.backgroundBehavior.ToString();
                if(keyboard==null){keyboard=UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Keyboard>("SfmOriginalNaturalInputProbe");SessionState.SetInt(Key+"keyboardDeviceId",keyboard.deviceId);mouse=UnityEngine.InputSystem.InputSystem.AddDevice<UnityEngine.InputSystem.Mouse>("SfmOriginalNaturalInputProbeMouse");SessionState.SetInt(Key+"mouseDeviceId",mouse.deviceId);}
                s.queuedW=false;var uiState=ObserveAndQueueUI(frame);
                UnityEngine.InputSystem.InputSystem.QueueStateEvent(keyboard,uiState.queuedTab?new UnityEngine.InputSystem.LowLevel.KeyboardState(UnityEngine.InputSystem.Key.Tab):uiState.queuedEscape?new UnityEngine.InputSystem.LowLevel.KeyboardState(UnityEngine.InputSystem.Key.Escape):new UnityEngine.InputSystem.LowLevel.KeyboardState());
                var originalInput=typeof(ExposureUnnoticed2.Scripts.Base.InputManager).GetField("playerInput",BindingFlags.NonPublic|BindingFlags.Static).GetValue(null)as UnityEngine.InputSystem.PlayerInput;
                s.activePlayerInputs=UnityEngine.InputSystem.PlayerInput.all.Count;s.unpairedDeviceActivity=UnityEngine.InputSystem.Users.InputUser.listenForUnpairedDeviceActivity;
                if(originalInput){var user=originalInput.user;s.inputAutoSwitchDisabled=originalInput.neverAutoSwitchControlSchemes;s.virtualKeyboardPaired=user.valid&&user.pairedDevices.Any(x=>x.deviceId==keyboard.deviceId);s.pairedDevices=user.valid?user.pairedDevices.Count:0;s.controlScheme=originalInput.currentControlScheme;}s.virtualKeyboardW=keyboard.wKey.isPressed;
                if(originalInput&&originalInput.currentActionMap!=null){s.inputMapEnabled=originalInput.currentActionMap.enabled;var action=originalInput.currentActionMap["Move"];s.moveActionEnabled=action.enabled;s.moveActionPhase=action.phase.ToString();var move=action.ReadValue<Vector2>();s.actionX=move.x;s.actionY=move.y;s.moveControls=originalInput.currentActionMap["Move"].controls.Count;s.mapHasVirtualKeyboard=originalInput.currentActionMap["Move"].controls.Any(x=>x.device.deviceId==keyboard.deviceId);}
                if(s.realPlayerBound){var pca=manager.InGameReferencer.Player.Pca;Vector3 pos=pca.Transform.position;s.worldX=pos.x;s.worldY=pos.y;s.worldZ=pos.z;if(hasPreviousPosition){Vector3 d=pos-previousPosition;s.horizontalStep=Mathf.Sqrt(d.x*d.x+d.z*d.z);}previousPosition=pos;hasPreviousPosition=true;s.calcRealtime=pca.PlayerMoveCalculator.JustMoveAmountRealtime;s.calcJust=pca.PlayerMoveCalculator.JustMoveAmount;s.calcLast=pca.PlayerMoveCalculator.LastUpdateJustMoveAmount;s.currentMoveSpeed=pca.PlayerLocomotion.CurrentMoveSpeed;s.currentPlayerState=(int)pca.PlayerState.CurrentState;s.characterGrounded=pca.CharacterController.isGrounded;s.characterEnabled=pca.CharacterController.enabled;}
                if(GameState.GameStateData!=null){s.currentRp=GameState.GameStateData.CurrentRp;s.earnedRp=GameState.GameStateData.CurrentEarnThisTimeRp;s.missionProgressEntries=GameState.GameStateData.MissionMaxRpBonus100Dict.Count;}
                report.samples.Add(s);Save();
                if(report.errors.Count>0)throw new InvalidOperationException("Original lifecycle emitted engine errors; first failure retained");
                if(frame>=180)
                {
                    ObserveCoreReadiness(manager);Need(report.actualPlayMode&&report.domainReloadObserved,"Actual original PlayMode and domain reload");Need(s.adapterReady&&s.resourceAssignments==25&&s.managers==1&&s.players==1&&s.realPlayerBound&&s.pcaPresent,"Original Start/resources/real Player");Need(s.dispatcherBound&&s.updaters==5&&s.lateUpdaters==4&&s.starters==1&&s.stageControllerPresent,"Normal original dispatcher and stage");Need(s.currentRp==0&&s.earnedRp==0&&s.missionProgressEntries==196,"Original new-game196 entries and zero funding");
                    Need(report.uiSamples.Any(x=>x.frame>15&&x.menus==1&&x.menuStaticBound&&x.menuClone&&x.menuState==1&&x.menuAlpha>0.95f&&x.exactInventoryPersistentTarget),"SDK Tab opened and original Animator displayed actual serialized Menu clone");
                    Need(report.uiSamples.Count(x=>x.queuedTab)==1&&report.uiSamples.Count(x=>x.queuedClick&&x.exactInventoryPersistentTarget&&x.inventoryButtonActive&&x.inventoryButtonInteractable)==1,"Only one queued Tab and one SDK mouse click at the real original Inventory button");
                    Need(report.uiSamples.Any(x=>x.frame>61&&x.inventoryPanels==1&&x.inventoryStaticBound&&x.inventoryClone&&x.inventoryActive&&x.menuChild),"Original EventSystem delivered actual persistent button and original Menu opened real Inventory child");
                    report.naturalMenuAndInventoryAccepted=true;
                    Need(report.gameInputSamples.Any(x=>x.rawEscape&&x.cancelActionPressed)&&report.uiSamples.Any(x=>x.frame>=100&&x.originalCancelCount==1),"SDK Escape reached actual Dynamic Cancel action and original InputManager frame-count1");
                    Need(report.uiSamples.Any(x=>x.frame>=140&&x.inventoryPanels==0&&!x.menuChild),"Original Cancel must close Inventory and invoke original parent-child return callback");
                    report.naturalInventoryReturnAccepted=true;report.passed=true;Finish(null);
                }
            }
            catch(Exception e){Finish(e);}
        }
        static void Changed(PlayModeStateChange s)
        {
            if(!Armed)return;Load();if(s==PlayModeStateChange.EnteredEditMode){SessionState.SetBool(Key+"armed",false);int deviceId=SessionState.GetInt(Key+"keyboardDeviceId",0);var ownedDevice=UnityEngine.InputSystem.InputSystem.GetDeviceById(deviceId);if(ownedDevice!=null&&ownedDevice.name.StartsWith("SfmOriginalNaturalInputProbe",StringComparison.Ordinal))UnityEngine.InputSystem.InputSystem.RemoveDevice(ownedDevice);SessionState.EraseInt(Key+"keyboardDeviceId");int mouseId=SessionState.GetInt(Key+"mouseDeviceId",0);var ownedMouse=UnityEngine.InputSystem.InputSystem.GetDeviceById(mouseId);if(ownedMouse!=null&&ownedMouse.name.StartsWith("SfmOriginalNaturalInputProbeMouse",StringComparison.Ordinal))UnityEngine.InputSystem.InputSystem.RemoveDevice(ownedMouse);SessionState.EraseInt(Key+"mouseDeviceId");if(SessionState.GetBool(Key+"routingSetup",false)){var settings=UnityEngine.InputSystem.InputSystem.settings;int oldRouting=SessionState.GetInt(Key+"oldRouting",-1),oldBackground=SessionState.GetInt(Key+"oldBackground",-1);settings.editorInputBehaviorInPlayMode=(UnityEngine.InputSystem.InputSettings.EditorInputBehaviorInPlayMode)oldRouting;settings.backgroundBehavior=(UnityEngine.InputSystem.InputSettings.BackgroundBehavior)oldBackground;report.SDKRoutingRestored=(int)settings.editorInputBehaviorInPlayMode==oldRouting&&(int)settings.backgroundBehavior==oldBackground;SessionState.EraseBool(Key+"routingSetup");Save();Write();}if(!SessionState.GetBool(Key+"finished",false)){report.failure="Unexpected PlayMode exit";report.passed=false;Write();}EditorApplication.delayCall+=()=>EditorApplication.Exit(report.passed?0:6);}
        }
        static void Write(){string output=SessionState.GetString(Key+"output","");if(output.Length>0)File.WriteAllText(Path.Combine(output,"observation.json"),JsonUtility.ToJson(report,true)+"\n");}
        static void Finish(Exception e)
        {
            if(finished)return;finished=true;SessionState.SetBool(Key+"finished",true);if(e!=null){report.failure=e.ToString();report.passed=false;Debug.LogError("SFM_ORIGINAL_NATURAL_INPUT_V14_FAILURE: "+e);}Save();Write();if(EditorApplication.isPlayingOrWillChangePlaymode)EditorApplication.isPlaying=false;else EditorApplication.delayCall+=()=>EditorApplication.Exit(report.passed?0:6);
        }
    }
}
#endif
