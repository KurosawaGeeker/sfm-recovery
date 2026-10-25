using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.UI;
using UnityEngine.InputSystem.Users;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Key=UnityEngine.InputSystem.Key;
namespace SfmMvp
{
 [DefaultExecutionOrder(32000)]
 public sealed class HomeCustomizationContinuityPlayerV1:MonoBehaviour
 {
  const string Flag="--sfm-home-customization-v1",DevicePrefix="SfmHomeCustomizationV1-";
  const string Product="SFMOriginalGameplayInputVisual-31014133f09b4dad99db9b29b82fe544";
  const string Profile=@"C:\Users\Administrator\AppData\LocalLow\SFMRecovery\SFMOriginalGameplayInputVisual-31014133f09b4dad99db9b29b82fe544";
  const string OutputRoot=@"C:\Users\Administrator\sfm-layerfs-dev\mission-menu-v1";
  const string Home="Assets/SfmOriginalGameplayV3/Apartment.unity",G="ExposureUnnoticed2.Scripts.InGame.",B="ExposureUnnoticed2.Scripts.Base.";
  const string ClosetType="ExposureUnnoticed2.ObjectUI.BarberMenu.ClosetMenuView",PopupType="ExposureUnnoticed2.ObjectUI.CommonPopup.CommonPopupView";
  const BindingFlags Flags=BindingFlags.Public|BindingFlags.NonPublic|BindingFlags.Instance|BindingFlags.Static|BindingFlags.DeclaredOnly;
  static readonly Assembly Game=typeof(global::ExposureUnnoticed2.Scripts.InGame.InGameManager).Assembly;
  static readonly HashSet<string> AllowedGameFields=new HashSet<string>(new[]{
   "Common.Scripts.UI.BasePanelView::isAlreadyClose",
   "ES3Defaults::settings",
   "ES3Internal.ES3IO::persistentDataPath",
   "ES3Settings::_defaults",
   "ES3Settings::_defaultSettingsScriptableObject",
   "ES3Settings::_location",
   "ES3Settings::directory",
   "ExposureUnnoticed2.Master.Hair.MHairRecord/HairCustomizeData::B",
   "ExposureUnnoticed2.Master.Hair.MHairRecord/HairCustomizeData::G",
   "ExposureUnnoticed2.Master.Hair.MHairRecord/HairCustomizeData::HairType",
   "ExposureUnnoticed2.Master.Hair.MHairRecord/HairCustomizeData::R",
   "ExposureUnnoticed2.Master.Hair.MHairRecord/HairCustomizeData::ShadeColor",
   "ExposureUnnoticed2.Master.Hair.MHairRecord/HairCustomizeData::TextureId",
   "ExposureUnnoticed2.Object3D.Camera.BaseCameraController::IsActiveInventoryCamera",
   "ExposureUnnoticed2.Object3D.Camera.BaseCameraController::isStrafeCache",
   "ExposureUnnoticed2.Object3D.Camera.TPCameraController::<Yaw>k__BackingField",
   "ExposureUnnoticed2.Object3D.Camera.TPCameraController::<Pitch>k__BackingField",
   "ExposureUnnoticed2.Object3D.Camera.TPCameraController::cameraTarget",
   "ExposureUnnoticed2.Object3D.Camera.TPCameraController::cameraMaxDistance",
   "ExposureUnnoticed2.Object3D.Camera.TPCameraController::currentVerticalValue",
   "ExposureUnnoticed2.Object3D.Camera.TPCameraController::ignoreLayerMask",
   "ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager::<IsForceDeActive>k__BackingField",
   "ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager::<IsOpenFade>k__BackingField",
   "ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager::<isOpenSomeAllowInputUi>k__BackingField",
   "ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager::<IsOpenSomeCancelInputUi>k__BackingField",
   "ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager::basePanelStack",
   "ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager::Instance",
   "ExposureUnnoticed2.Object3D.InteractArea.InteractAreaController::isInArea",
   "ExposureUnnoticed2.Object3D.InteractArea.InteractAreaController::type",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerAvatarObjectReferencer::BraParent",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerAvatarObjectReferencer::ClothesParent",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerAvatarObjectReferencer::CosplayParent",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerAvatarObjectReferencer::FaceMesh",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerAvatarObjectReferencer::PantsParent",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerBodyCustomizeAdjuster::<HeightScale>k__BackingField",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerBodyCustomizeAdjuster::defaultBaseScale",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerClassAccessor::<AvatorTransform>k__BackingField",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerClassAccessor::<PlayerAvatarObjectReferencer>k__BackingField",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerClassAccessor::<PlayerBodyCustomizeAdjuster>k__BackingField",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerClassAccessor::<PlayerState>k__BackingField",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerClassAccessor::<Transform>k__BackingField",
   "ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController::pca",
   "ExposureUnnoticed2.ObjectUI.BarberMenu.ClosetMenuView::customizeView",
   "ExposureUnnoticed2.ObjectUI.BarberMenu.ClosetMenuView::isInitDisplayBra",
   "ExposureUnnoticed2.ObjectUI.BarberMenu.ClosetMenuView::isInitDisplayClothes",
   "ExposureUnnoticed2.ObjectUI.BarberMenu.ClosetMenuView::isInitDisplayCostume",
   "ExposureUnnoticed2.ObjectUI.BarberMenu.ClosetMenuView::isInitDisplayPants",
   "ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu.CustomizeMenuView::<Instance>k__BackingField",
   "ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu.CustomizeMenuView::bodyView",
   "ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu.CustomizeMenuView::customizeCommonTabView",
   "ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu.CustomizeMenuView::eyeHeight",
   "ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu.CustomizeMenuView::eyeSize",
   "ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu.CustomizeMenuView::faceView",
   "ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu.CustomizeMenuView::height",
   "ExposureUnnoticed2.ObjectUI.OptionMenu.CustomizeBarItemView::adjustHeightMask",
   "ExposureUnnoticed2.ObjectUI.OptionMenu.CustomizeBarItemView::isPlayerCustom",
   "ExposureUnnoticed2.ObjectUI.OptionMenu.CustomizeBarItemView::onChange",
   "ExposureUnnoticed2.ObjectUI.OptionMenu.CustomizeBarItemView::rankConditionMask",
   "ExposureUnnoticed2.ObjectUI.OptionMenu.CustomizeBarItemView::slider",
   "ExposureUnnoticed2.Scripts.Base.InputManager::inputDict",
   "ExposureUnnoticed2.Scripts.Base.InputManager::playerInput",
   "ExposureUnnoticed2.Scripts.Base.InteractManager::interactList",
   "ExposureUnnoticed2.Scripts.Base.InteractModel::Controller",
   "ExposureUnnoticed2.Scripts.Base.InteractModel::Type",
   "ExposureUnnoticed2.Scripts.Base.SaveRapper::currentDataPath",
   "ExposureUnnoticed2.Scripts.Base.SaveRapper::IsAutoSave",
   "ExposureUnnoticed2.Scripts.Base.SaveRapper::IsBackup",
   "ExposureUnnoticed2.Scripts.Base.SaveRapper::IsHomeSave",
   "ExposureUnnoticed2.Scripts.Base.SaveRapper::IsInterrupt",
   "ExposureUnnoticed2.Scripts.Base.SaveRapper::IsSystemData",
   "ExposureUnnoticed2.Scripts.InGame.GameState::DifficultData",
   "ExposureUnnoticed2.Scripts.InGame.GameState::GameStateData",
   "ExposureUnnoticed2.Scripts.InGame.GameState::ManualSaveTime",
   "ExposureUnnoticed2.Scripts.InGame.GameState::PlayerCustomData",
   "ExposureUnnoticed2.Scripts.InGame.GameState::RankData",
   "ExposureUnnoticed2.Scripts.InGame.GameState::TotalPlayTimeData",
   "ExposureUnnoticed2.Scripts.InGame.GameStateData::ClothesDict",
   "ExposureUnnoticed2.Scripts.InGame.GameStateData::CurrentRp",
   "ExposureUnnoticed2.Scripts.InGame.GameStateData::ItemDict",
   "ExposureUnnoticed2.Scripts.InGame.GameStateData::MAdultGoodsDict",
   "ExposureUnnoticed2.Scripts.InGame.GameStateData::PlayerCosplayPartsDict",
   "ExposureUnnoticed2.Scripts.InGame.GameStateData::PlayerCostumeDict",
   "ExposureUnnoticed2.Scripts.InGame.GameStateData::PlayerCurrentClothes",
   "ExposureUnnoticed2.Scripts.InGame.GameStateData::StatistisTotalUseRp",
   "ExposureUnnoticed2.Scripts.InGame.InGameManager::<InGameReferencer>k__BackingField",
   "ExposureUnnoticed2.Scripts.InGame.InGameManager::Instance",
   "ExposureUnnoticed2.Scripts.InGame.InGameReferencer::<BaseCameraController>k__BackingField",
   "ExposureUnnoticed2.Scripts.InGame.InGameReferencer::<Player>k__BackingField",
   "ExposureUnnoticed2.Scripts.InGame.InGameReferencer::<TpCameraController>k__BackingField",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::BodyPaintTypeDict",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CurrentHair",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomIsSkinColorFree",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeAdjustHeight",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeArm",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeBoobs",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeButt",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeCheekFat",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeChinLength",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEarElf",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeBlowColorB",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeBlowColorG",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeBlowColorR",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeDark",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeDist",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeHeight",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeLashColorB",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeLashColorG",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeLashColorR",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeLeftPattern",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeOpenness",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyePattern",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeEyeSize",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeFutanariKitouSize",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeFutanariSaoSize",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeFutanariStraight",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeFutanariTamaSize",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeFutanariTamaVisible",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeFutanariWidth",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeHead",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeHeight",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeHitomiSize",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeHyperBoobs",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeInmou1",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeInmou2",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeInmouColorB",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeInmouColorG",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeInmouColorR",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeLegInterval",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeLoliFace",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeLowerLeg",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeLowerLegLength",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeMatsugeLength",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeMatsugeThick",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeMayuDog",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeMayuThick",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeNeck",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeNeckThick",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeNipple",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeOddEye",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeShoulder",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeSkinColor",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeSkinColorB",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeSkinColorG",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeSkinColorR",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeSuperBoobs",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeTareme",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeThighs",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeThighsLength",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeUpperBody",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeUpperBody2",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeUpperBodyLength",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeUpperBodyLength2",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeWaist",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::CustomizeYaeba",
   "ExposureUnnoticed2.Scripts.InGame.PlayerCustomData::HairCustomizeDataDict",
   "ExposureUnnoticed2.Scripts.InGame.SaveManager::CurrentSaveSlot"
  },StringComparer.Ordinal);
  [Serializable]public sealed class Pin{public string path,sha256;public long bytes;}
  [Serializable]public sealed class ProtectedProfile{public string root;public Pin[] files;}
  [Serializable]public sealed class Contract{public string schema,mode,buildRoot,output,testProduct,testProfilePath,playerGameSHA,observerAssemblySHA,executableSHA;public bool launchReady;public Pin[] inputs,testSaves;public ProtectedProfile[] protectedProfiles;public Pin priorEditReport;public int expectedSaveSlot;}
  [Serializable]public sealed class Row{public string key,type,value;}
  [Serializable]public sealed class Shape{public int index;public string name;public float actual,expected;}
  [Serializable]public sealed class Snapshot{public string role,scene,meshName;public int frame,height,eyeSize,eyeHeight,meshId,meshCount,faceId,avatarId;public float time,heightScale,defaultBaseScale;public Vector3 position,avatarScale;public bool modelMatches;public List<Row> custom=new List<Row>(),economy=new List<Row>();public List<Shape> shapes=new List<Shape>();}
  [Serializable]public sealed class Capture{public string role,path,sha256;public long bytes;public int requestFrame,width,height;public float requestTime;public bool ready;}
  [Serializable]public sealed class Tap{public string role,action,path;public int key,raw,pressFrame,releaseFrame;public bool pressSeen,releaseSeen,rawOne;}
  [Serializable]public sealed class PointerTxn{public string role,path,kind,moveHit,pressHit,releaseHit;public int target,startFrame,moveFrame,pressFrame,releaseFrame;public float readyTime;public Vector2 point;public bool pressed,released,moveSeen,pressSeen,releaseSeen,moveRaycast,pressRaycast;}
  [Serializable]public sealed class SliderTxn{public string role,field,customField;public int targetValue,startFrame,scrollFrames,progressFrames,stableFrames,sliderId;public float lastY,windowTime,windowY;public bool hasWindow,complete;public Snapshot result;}
  [Serializable]public sealed class Movement{public int frame;public float time,step;public Vector3 position;public Vector2 sdk;public bool pressed;}
  [Serializable]public sealed class RouteProbe{public int frame,headType,headControllerId,sensorId,listCount;public float time,sphereRadius,characterRadius,xzDistance;public Vector3 playerPosition,sensorCenter,sensorScale;public bool sensorActive,triggerEnabled,isInArea,headMatches,ready;}
  [Serializable]public sealed class CameraPose{public int id;public string path;public Vector3 position,forward,lossyScale;public Quaternion rotation;}
  [Serializable]public sealed class CameraView{public int id,mask,targetTextureId;public string path,targetTextureName,clearFlags;public bool enabled,isBaseCamera;public float near,far,fov,depth;public Rect rect,pixelRect;public CameraPose pose;}
  [Serializable]public sealed class CameraCollider{public int id,layer;public string path,type,layerName;public bool trigger,boundsContainCamera;public Vector3 boundsCenter,boundsSize;}
  [Serializable]public sealed class CameraObservation
  {
   public string role,scene;public int frame,height,ignoreLayerMask,castLayerMask,nearbyLayerMask,nearbyCount;public float time,scheduledSeconds,yaw,pitch,currentVerticalValue,maxDistance,targetToTpDistance,targetToMainDistance,castRadius,hitDistance;public double realtime,elapsed;
   public bool inventoryCamera,isStrafeCache,hit,nearbyTruncated;public Vector3 avatarScale,castOrigin,castDirection,hitPoint,hitNormal;public CameraPose player,avatar,tp,baseCamera,cameraTarget;public CameraCollider hitCollider;public List<CameraCollider> nearby=new List<CameraCollider>();public List<CameraView> cameras=new List<CameraView>();
   public string boundsLimit="Bounds are world AABBs; containment is not proof of point-inside-mesh. Runtime IDs are not serialized file IDs. SphereCast is a read-only SDK query sampled after original camera updates, not a call to the Game method.";
  }

  [Serializable]public sealed class SaveSample{public int frame,slot;public long ticks;public string path;public bool system,backup,home,autoSave,interrupt;}
  [Serializable]public sealed class Report
  {
   public string schema="HomeCustomizationContinuityPlayerObservation/v1",mode,status="preflight",failure,testProduct,testProfilePath,playerGameSHA,observerAssemblySHA,executableSHA;
   public int phase,phaseFrame,checks,frames,routeWaypoint,routeStartFrame,originalKeyboardId,originalMouseId,closeFrame,closedFrame,saveSlot,terminalExitCode=6;
   public float phaseTime,routeWalk,routeWindowTime,routeWindowDistance,routeWindowWalk,releaseTime,tailStart;public Vector3 routePrevious,moveStart,releasePosition,tailPosition;
   public bool reopenedUiMatched,secondSaveObserved;public long firstCloseSaveTicks;public bool baselineReady,closetEntered,bodyChanged,bodyRestored,faceChanged,faceRestored,defaultCancelled,defaultConfirmed,finalDifferent,originalCloseObserved,saveObserved,movementAccepted,releaseAccepted,coldMatched;
   public bool actualPlayer,flowAccepted,reportComplete,acceptanceClean,visualAccepted=false,passed=false,renderingAccepted=false,screenshotEvidenceComplete,sdkRestored,protectedProfilesUnchanged,identityUnchanged,inputReleaseObserved,pairingRestored,autoSwitchRestored,originalSchemePresent,originalNeverAutoSwitch;
   public uint originalInputUserId;public string originalScheme;public int[] originalDeviceIds;public bool finalReportWriteSucceeded;
   public bool diagnosticComplete,cameraTestFilesUnchanged;public int cameraReadyFrame;public float cameraReadyTime;public double cameraReadyRealtime;public List<CameraObservation> cameraObservations=new List<CameraObservation>();
   public RouteProbe lastRouteProbe;public Snapshot baseline,savedAnchor,lastSample;public List<Snapshot> snapshots=new List<Snapshot>();public List<Capture> captures=new List<Capture>();public List<Tap> taps=new List<Tap>();public List<PointerTxn> pointer=new List<PointerTxn>();public List<SliderTxn> sliders=new List<SliderTxn>();public List<Movement> movement=new List<Movement>();public List<SaveSample> saves=new List<SaveSample>();
   public List<Pin> testInventoryBefore=new List<Pin>(),finalTestFiles=new List<Pin>();public List<Row> originalClothes=new List<Row>();public List<string> errors=new List<string>(),cleanupErrors=new List<string>(),readResolutions=new List<string>();
   public string visualLimit="Original screenshots require independent image review. Screenshot existence/model telemetry does not set visualAccepted/renderingAccepted/passed. No save payload is parsed.";
  }
  static Contract contract;static Report report,prior;static bool Armed,finished,finishing,completing,outputOwned,backgroundCaptured,originalRunInBackground,pairingCaptured,pairingWasCaptured;
  static DateTime deadline,finishingDeadline;static Keyboard keyboard;static Mouse mouse;static int keyboardId,mouseId,lastFrame=-1,releaseUpdates,coldFrames,cameraOrdinal;static bool cameraStarted;
  static Vector2 mousePoint,scroll;static bool mouseLeft;static Key[] queued=new Key[0];static PlayerInput routedInput;static InputControlScheme? originalUserScheme;static InputSettings.BackgroundBehavior originalBackground;
  static object manager,pca,state,gsd,custom,ui;static Component player;static Transform playerTransform;static Behaviour closet,view,popup,sensor;static Vector3 lastPosition;static float lastSaveCheck,firstBoundTime=-1f;static string lastSaveKey,lastStableFiles;static float stableFilesSince;static Snapshot popupBaseline;
  static void Need(bool v,string m){if(report!=null)report.checks++;if(!v)throw new InvalidOperationException(m);}
  static object Read(object value,string name)
  {
   if(value==null)return null;for(Type t=value is Type?(Type)value:value.GetType();t!=null;t=t.BaseType){var f=t.GetField(name,Flags)??t.GetField("<"+name+">k__BackingField",Flags);if(f==null)continue;string id=f.DeclaringType.FullName.Replace('+','/')+"::"+f.Name;if(f.DeclaringType.Assembly==Game)Need(AllowedGameFields.Contains(id),"Unreviewed Game field "+id);if(report!=null&&!report.readResolutions.Contains(id)){Need(report.readResolutions.Count<512,"Bounded field resolution log");report.readResolutions.Add(id);}return f.GetValue(value is Type?null:value);}throw new MissingFieldException(value.GetType().FullName,name);
  }
  static object Static(string type,string name)=>Read(Game.GetType(type,true),name);
  static int I(object o){Need(o!=null,"Known integer");return Convert.ToInt32(o);}
  static float F(object o){Need(o!=null,"Known float");return Convert.ToSingle(o);}
  static bool Z(object o){Need(o is bool,"Known bool");return (bool)o;}
  static bool Alive(object o)=>o!=null&&(!(o is UnityEngine.Object)||(UnityEngine.Object)o);
  static int Id(object o)=>o is UnityEngine.Object?((UnityEngine.Object)o).GetInstanceID():0;
  static string Full(string p)=>Path.GetFullPath(p).TrimEnd('\\','/');
  static bool SamePath(string a,string b)=>string.Equals(Full(a),Full(b),StringComparison.OrdinalIgnoreCase);
  static bool Inside(string p,string r)=>Full(p).StartsWith(Full(r)+Path.DirectorySeparatorChar,StringComparison.OrdinalIgnoreCase);
  static string Hash(string p){using(var h=SHA256.Create())using(var f=File.OpenRead(p))return BitConverter.ToString(h.ComputeHash(f)).Replace("-","");}
  static Pin PinFile(string p)=>new Pin{path=Full(p),bytes=new FileInfo(p).Length,sha256=Hash(p)};
  static void CheckPin(Pin p){Need(p!=null&&File.Exists(p.path)&&new FileInfo(p.path).Length==p.bytes&&Hash(p.path)==p.sha256,"Exact sealed pin "+(p==null?"null":p.path));}
  static void CheckTree(string root){Need(Directory.Exists(root),"Existing profile directory");foreach(string d in new[]{root}.Concat(Directory.GetDirectories(root,"*",SearchOption.AllDirectories)))Need((File.GetAttributes(d)&FileAttributes.ReparsePoint)==0,"No profile reparse directory");foreach(string p in Directory.GetFiles(root,"*",SearchOption.AllDirectories))Need((File.GetAttributes(p)&FileAttributes.ReparsePoint)==0,"No profile reparse file");}
  static List<Pin> Tree(string root){CheckTree(root);var a=Directory.GetFiles(root,"*",SearchOption.AllDirectories);Need(a.Length<=1024,"Bounded opaque inventory");return a.OrderBy(x=>x,StringComparer.OrdinalIgnoreCase).Select(PinFile).ToList();}
  static bool PinsEqual(IEnumerable<Pin> a,IEnumerable<Pin> b){var x=a.OrderBy(v=>v.path,StringComparer.OrdinalIgnoreCase).ToArray();var y=b.OrderBy(v=>v.path,StringComparer.OrdinalIgnoreCase).ToArray();return x.Length==y.Length&&x.Zip(y,(p,q)=>SamePath(p.path,q.path)&&p.bytes==q.bytes&&p.sha256==q.sha256).All(v=>v);}
  static void Protected(){foreach(var p in contract.protectedProfiles){if(!Directory.Exists(p.root)){Need(!File.Exists(p.root)&&p.files.Length==0,"Absent protected profile stays absent");continue;}Need(PinsEqual(Tree(p.root),p.files),"Protected full inventory unchanged: "+p.root);}}
  [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]static void Install()
  {
   if(!Environment.GetCommandLineArgs().Contains(Flag))return;report=new Report();try{Begin();var host=new GameObject("SfmHomeCustomizationV1-OwnedObserver");DontDestroyOnLoad(host);host.AddComponent<HomeCustomizationContinuityPlayerV1>();Application.logMessageReceived+=Log;if(contract.mode!="camera")InputSystem.onAfterUpdate+=AfterInput;}catch(Exception e){report.failure=e.ToString();report.status="preflight-refused";try{Debug.LogError(e.ToString());if(outputOwned)Write();}finally{Application.Quit(6);}}
  }
  static void Begin()
  {
   Need(!Application.isEditor&&!Armed,"Standalone only");var args=Environment.GetCommandLineArgs();Need(args.Count(x=>x==Flag)==1&&args.Count(x=>x=="--sfm-manual-preview")==1,"Unique own flag and manual-preview");foreach(string f in args.Where(x=>x.StartsWith("-sfm",StringComparison.Ordinal)||x.StartsWith("--sfm",StringComparison.Ordinal)))Need(f==Flag||f=="--sfm-manual-preview","Competing diagnostic flag rejected "+f);
   int i=Array.IndexOf(args,Flag);Need(i+2<args.Length,"Contract path/SHA required");string cp=Full(args[i+1]);Need(cp.EndsWith(".json",StringComparison.OrdinalIgnoreCase)&&(Inside(cp,@"S:\ReverseEngineering\tools\code")||Inside(cp,OutputRoot))&&!Inside(cp,Profile)&&new FileInfo(cp).Length<1024*1024,"Bounded observer contract JSON outside saves");Need(Hash(cp)==args[i+2],"Contract exact SHA");contract=JsonUtility.FromJson<Contract>(File.ReadAllText(cp));Need(contract.schema=="HomeCustomizationContinuityPlayer/v1"&&contract.launchReady&&(contract.mode=="edit-save"||contract.mode=="cold"||contract.mode=="camera"),"Sealed contract mode");
   Need(contract.testProduct==Product&&SamePath(contract.testProfilePath,Profile)&&Application.companyName=="SFMRecovery"&&Application.productName==Product&&SamePath(Application.persistentDataPath,Profile),"Exact baked identity; flags cannot select a profile");Need(SamePath(Path.GetDirectoryName(Full(contract.buildRoot)),OutputRoot),"One native build direct child");string exe=System.Diagnostics.Process.GetCurrentProcess().MainModule.FileName;Need(SamePath(Path.GetDirectoryName(exe),contract.buildRoot)&&Inside(Application.dataPath,contract.buildRoot)&&Inside(Game.Location,Path.Combine(Application.dataPath,"Managed")),"Actual Player process/data/Game paths");
   report.playerGameSHA=Hash(Game.Location);report.observerAssemblySHA=Hash(typeof(HomeCustomizationContinuityPlayerV1).Assembly.Location);report.executableSHA=Hash(exe);Need(report.playerGameSHA==contract.playerGameSHA&&report.observerAssemblySHA==contract.observerAssemblySHA&&report.executableSHA==contract.executableSHA,"Actual assembly/executable pins");Need(contract.inputs!=null&&contract.inputs.Length>0&&contract.inputs.Length<=64,"Bounded provenance");Need(contract.inputs.All(p=>!Inside(p.path,Profile)),"Test saves are separate opaque pins, never source inputs");foreach(var p in contract.inputs)CheckPin(p);
   Need(contract.testSaves!=null&&contract.testSaves.Length==3,"Existing exact three-file opaque test inventory");Need(contract.testSaves.All(p=>Inside(p.path,Profile)),"Test roots");report.testInventoryBefore=Tree(Profile);Need(PinsEqual(report.testInventoryBefore,contract.testSaves),"Entire existing test inventory pinned, no seed reset");Need(contract.expectedSaveSlot==0,"Existing continuous test slot zero");Need(contract.testSaves.Select(x=>Full(x.path)).OrderBy(x=>x,StringComparer.OrdinalIgnoreCase).SequenceEqual(new[]{Full(Path.Combine(Profile,"SaveData/0.sd")),Full(Path.Combine(Profile,"SaveData/s.sd")),Full(Path.Combine(Profile,"SaveDataBackup/0.sd"))}.OrderBy(x=>x,StringComparer.OrdinalIgnoreCase),StringComparer.OrdinalIgnoreCase),"Original complete profile31 save path keyset");
   Need(contract.protectedProfiles!=null&&contract.protectedProfiles.Length>0&&contract.protectedProfiles.Length<=32,"Protected profile coverage");foreach(var p in contract.protectedProfiles){Need(p.files!=null&&p.files.Length<=1024&&!SamePath(p.root,Profile)&&!Inside(p.root,Profile)&&!Inside(Profile,p.root)&&p.files.All(f=>Inside(f.path,p.root)),"Distinct bounded protected root");}Protected();
   if(contract.mode=="camera")Need(contract.priorEditReport==null||string.IsNullOrEmpty(contract.priorEditReport.path)&&string.IsNullOrEmpty(contract.priorEditReport.sha256)&&contract.priorEditReport.bytes==0,"Camera diagnostic admits no prior-report artifact");
   if(contract.mode=="cold"){CheckPin(contract.priorEditReport);Need(contract.inputs.Any(p=>SamePath(p.path,contract.priorEditReport.path)&&p.bytes==contract.priorEditReport.bytes&&p.sha256==contract.priorEditReport.sha256),"Prior edit report is also immutable provenance rechecked in cleanup");Need(Inside(contract.priorEditReport.path,OutputRoot)&&!Inside(contract.priorEditReport.path,Profile),"Prior observer report, never save payload");prior=JsonUtility.FromJson<Report>(File.ReadAllText(contract.priorEditReport.path));Need(prior.schema==report.schema&&prior.mode=="edit-save"&&prior.flowAccepted&&prior.acceptanceClean&&prior.reportComplete&&prior.terminalExitCode==0&&prior.sdkRestored&&prior.protectedProfilesUnchanged&&prior.identityUnchanged&&string.IsNullOrEmpty(prior.failure)&&prior.finalReportWriteSucceeded&&prior.saveObserved&&prior.secondSaveObserved&&prior.reopenedUiMatched&&prior.finalDifferent&&prior.originalCloseObserved&&prior.movementAccepted&&prior.releaseAccepted&&prior.screenshotEvidenceComplete&&prior.errors.Count==0&&prior.cleanupErrors.Count==0,"Complete clean prior edit/save observation");Need(prior.playerGameSHA==report.playerGameSHA&&prior.testProduct==Product&&SamePath(prior.testProfilePath,Profile)&&prior.savedAnchor!=null&&PinsEqual(prior.finalTestFiles,contract.testSaves),"Same actual build/profile and latest saved files; no previous seed");}
   Need(SamePath(Path.GetDirectoryName(Full(contract.output)),OutputRoot)&&!SamePath(contract.output,contract.buildRoot)&&!Directory.Exists(contract.output)&&!File.Exists(contract.output),"Fresh direct evidence directory");Directory.CreateDirectory(contract.output);outputOwned=true;report.mode=contract.mode;report.testProduct=Product;report.testProfilePath=Profile;report.actualPlayer=true;report.status="running";report.saveSlot=contract.expectedSaveSlot;deadline=DateTime.UtcNow.AddSeconds(contract.mode=="cold"||contract.mode=="camera"?120:480);Armed=true;report.phaseTime=Time.unscaledTime;Write();
  }
  void Update(){if(!Armed||finished)return;if(finishing){Drain();return;}try{Poll();}catch(Exception e){Finish(e);}}
  void OnApplicationQuit(){if(Armed&&!finished&&!completing){Invalidate("Unexpected Player quit");Complete(false);}}
  static void Log(string msg,string stack,LogType type){if(!Armed||!(type==LogType.Error||type==LogType.Exception||type==LogType.Assert))return;if(report.errors.Count<24)report.errors.Add(msg+"\n"+stack);}
  static void Write(){Need(outputOwned&&Inside(Path.Combine(contract.output,"observation.json"),contract.output),"Owned evidence output");File.WriteAllText(Path.Combine(contract.output,"observation.json"),JsonUtility.ToJson(report,true)+"\n");}
  static void Invalidate(string message){report.flowAccepted=false;report.diagnosticComplete=false;report.acceptanceClean=false;report.passed=false;report.movementAccepted=false;report.releaseAccepted=false;report.coldMatched=false;report.terminalExitCode=6;if(!string.IsNullOrEmpty(message)&&string.IsNullOrEmpty(report.failure))report.failure=message;}
  static void CleanupStep(string name,Action action){try{action();}catch(Exception e){Invalidate(name+": "+e.Message);if(report.cleanupErrors.Count<32)report.cleanupErrors.Add(name+": "+e);}}
  static void Finish(Exception e){if(finishing||finished)return;finishing=true;finishingDeadline=DateTime.UtcNow.AddSeconds(3);if(e!=null){Invalidate(e.ToString());report.status="partial-evidence";}if(contract.mode!="camera")CleanupStep("neutral-input",()=>{mouseLeft=false;scroll=Vector2.zero;Queue(new Key[0]);});CleanupStep("partial-report",Write);}
  static void Drain(){try{if(contract.mode=="camera"){Need(keyboard==null&&mouse==null&&!pairingWasCaptured&&!backgroundCaptured,"Passive camera drain has no SDK ownership");Complete(true);return;}if(keyboard==null&&mouse==null||report.inputReleaseObserved){Complete(true);return;}if(DateTime.UtcNow>finishingDeadline){Invalidate("Neutral SDK release timeout");Complete(true);return;}mouseLeft=false;scroll=Vector2.zero;Queue(new Key[0]);}catch(Exception e){Invalidate(e.ToString());Complete(true);}}
  static void Complete(bool quit)
  {
   if(completing||finished)return;completing=true;
   try{Cleanup();}catch(Exception e){Invalidate(e.ToString());report.cleanupErrors.Add(e.ToString());}
   finally{try{if(contract.mode!="camera")InputSystem.onAfterUpdate-=AfterInput;}finally{Application.logMessageReceived-=Log;finished=true;Armed=false;report.reportComplete=true;report.visualAccepted=false;report.renderingAccepted=false;report.passed=false;report.acceptanceClean=report.sdkRestored&&report.identityUnchanged&&report.protectedProfilesUnchanged&&report.errors.Count==0&&report.cleanupErrors.Count==0&&string.IsNullOrEmpty(report.failure);if(!report.acceptanceClean)Invalidate(null);report.terminalExitCode=report.acceptanceClean&&report.screenshotEvidenceComplete&&(contract.mode=="camera"?report.diagnosticComplete&&report.cameraTestFilesUnchanged&&!report.flowAccepted&&!report.coldMatched&&report.cameraObservations.Count==4&&report.captures.Count==4:report.flowAccepted)?0:6;try{report.finalReportWriteSucceeded=true;Write();}catch(Exception e){report.finalReportWriteSucceeded=false;Invalidate("Final report write: "+e);try{Write();}catch{}}finally{if(quit)Application.Quit(report.terminalExitCode);}}}
  }
  static bool SetupSDK()
  {
   var pi=PI();if(!pi||!pi.user.valid||pi.actions==null)return false;
   if(!pairingCaptured)
   {
    Need(keyboard==null&&mouse==null&&!InputSystem.devices.Any(x=>x.name.StartsWith(DevicePrefix,StringComparison.Ordinal)),"No owned device before original user snapshot");
    Need(pi.actions.controlSchemes.Any(x=>x.name=="Keyboard and Mouse"),"Original SDK user and supported scheme");
    routedInput=pi;originalUserScheme=pi.user.controlScheme;report.originalInputUserId=pi.user.id;
    report.originalSchemePresent=originalUserScheme.HasValue;report.originalScheme=originalUserScheme.HasValue?originalUserScheme.Value.name:null;
    report.originalDeviceIds=pi.user.pairedDevices.Select(x=>x.deviceId).ToArray();report.originalNeverAutoSwitch=pi.neverAutoSwitchControlSchemes;
    Need(pi.user.pairedDevices.All(x=>!x.name.StartsWith(DevicePrefix,StringComparison.Ordinal)),"Original pairing contains no observer devices");
    pairingCaptured=true;pairingWasCaptured=true;
    originalBackground=InputSystem.settings.backgroundBehavior;originalRunInBackground=Application.runInBackground;backgroundCaptured=true;
    InputSystem.settings.backgroundBehavior=InputSettings.BackgroundBehavior.IgnoreFocus;Application.runInBackground=true;
    pi.neverAutoSwitchControlSchemes=true;
    keyboard=InputSystem.AddDevice<Keyboard>(DevicePrefix+"Keyboard");keyboardId=keyboard.deviceId;
    mouse=InputSystem.AddDevice<Mouse>(DevicePrefix+"Mouse");mouseId=mouse.deviceId;
    pi.SwitchCurrentControlScheme("Keyboard and Mouse",keyboard,mouse);
   }
   Need(routedInput&&ReferenceEquals(pi,routedInput)&&routedInput.user.valid&&routedInput.user.id==report.originalInputUserId&&routedInput.neverAutoSwitchControlSchemes&&routedInput.user.pairedDevices.Count==2&&routedInput.user.pairedDevices.Any(x=>x.deviceId==keyboard.deviceId)&&routedInput.user.pairedDevices.Any(x=>x.deviceId==mouse.deviceId),"Only owned SDK devices paired; physical manual-player input excluded");return true;
  }
  static void CheckPairing()
  {
   Need(routedInput&&routedInput.user.valid&&routedInput.user.id==report.originalInputUserId,"Same original valid InputUser after cleanup");var restored=routedInput.user.controlScheme;
   bool sameScheme=restored.HasValue==report.originalSchemePresent&&(!restored.HasValue||restored.Value.Equals(originalUserScheme.Value));
   report.pairingRestored=sameScheme&&routedInput.user.pairedDevices.Select(x=>x.deviceId).OrderBy(x=>x).SequenceEqual(report.originalDeviceIds.OrderBy(x=>x));Need(report.pairingRestored,"Original nullable scheme and exact SDK pairing restored");
  }
  static void RestorePairing()
  {
   if(!pairingCaptured)return;
   CleanupStep("original-pairing",()=>
   {
    Need(routedInput,"Restore original SDK pairing while Player still exists");var devices=report.originalDeviceIds.Select(InputSystem.GetDeviceById).ToArray();Need(devices.All(x=>x!=null&&x.added),"Original SDK devices still exist");
    var user=routedInput.user;Need(user.valid&&user.id==report.originalInputUserId,"Restore the same original InputUser");user.UnpairDevices();foreach(var device in devices)InputUser.PerformPairingWithDevice(device,user:user);user.ActivateControlScheme(originalUserScheme??default(InputControlScheme));CheckPairing();
   });
  }
  static void Cleanup()
  {
   if(contract.mode=="camera")CleanupStep("passive-sdk",()=>{Need(keyboard==null&&mouse==null&&keyboardId==0&&mouseId==0&&!pairingCaptured&&!pairingWasCaptured&&!backgroundCaptured&&routedInput==null,"Camera diagnostic never owned or changed SDK routing");report.sdkRestored=true;});
   else
   {
   try{RestorePairing();}finally{try{CleanupStep("owned-keyboard",()=>{if(keyboard!=null&&keyboard.added){Need(keyboard.deviceId==keyboardId&&keyboard.name.StartsWith(DevicePrefix,StringComparison.Ordinal),"Owned keyboard identity before removal");InputSystem.RemoveDevice(keyboard);}});}finally{try{CleanupStep("owned-mouse",()=>{if(mouse!=null&&mouse.added){Need(mouse.deviceId==mouseId&&mouse.name.StartsWith(DevicePrefix,StringComparison.Ordinal),"Owned mouse identity before removal");InputSystem.RemoveDevice(mouse);}});}finally{try{CleanupStep("original-auto-switch",()=>{if(pairingWasCaptured){Need(routedInput,"Original SDK object for auto-switch restore");routedInput.neverAutoSwitchControlSchemes=report.originalNeverAutoSwitch;report.autoSwitchRestored=routedInput.neverAutoSwitchControlSchemes==report.originalNeverAutoSwitch;Need(report.autoSwitchRestored,"Original auto-switch restored");}});}finally{try{CleanupStep("background",()=>{if(backgroundCaptured)InputSystem.settings.backgroundBehavior=originalBackground;});}finally{CleanupStep("run-in-background",()=>{if(backgroundCaptured)Application.runInBackground=originalRunInBackground;});}}}}}
   CleanupStep("post-remove-pairing",()=>{if(pairingWasCaptured)CheckPairing();});pairingCaptured=false;
   CleanupStep("sdk-final",()=>{report.sdkRestored=(!backgroundCaptured||(InputSystem.settings.backgroundBehavior==originalBackground&&Application.runInBackground==originalRunInBackground))&&(keyboardId==0||InputSystem.GetDeviceById(keyboardId)==null)&&(mouseId==0||InputSystem.GetDeviceById(mouseId)==null)&&(!pairingWasCaptured||report.pairingRestored&&report.autoSwitchRestored);Need(report.sdkRestored,"All SDK state restored after owned-device removal");});
   }
   CleanupStep("identity",()=>{report.identityUnchanged=Application.companyName=="SFMRecovery"&&Application.productName==Product&&SamePath(Application.persistentDataPath,Profile);Need(report.identityUnchanged,"Baked identity unchanged");});CleanupStep("protected",()=>{Protected();report.protectedProfilesUnchanged=true;});CleanupStep("pins",()=>{foreach(var p in contract.inputs)CheckPin(p);});
   CleanupStep("test-inventory",()=>{report.finalTestFiles=Tree(Profile);if(contract.mode=="camera"){report.cameraTestFilesUnchanged=PinsEqual(report.testInventoryBefore,report.finalTestFiles);Need(report.cameraTestFilesUnchanged,"Passive camera diagnostic causes no save writes");}else if(contract.mode=="cold")Need(PinsEqual(report.testInventoryBefore,report.finalTestFiles),"Cold causes no save writes");else{var allowed=new[]{Full(Path.Combine(Profile,"SaveData/s.sd")),Full(Path.Combine(Profile,"SaveData/"+report.saveSlot+".sd"))};Need(report.finalTestFiles.Count==report.testInventoryBefore.Count,"No unexplained new/missing save files");foreach(var p in report.testInventoryBefore)if(!allowed.Any(a=>SamePath(a,p.path)))CheckPin(p);Need(report.finalTestFiles.All(p=>report.testInventoryBefore.Any(x=>SamePath(x.path,p.path))),"No unexpected side files");}});
  }
  static PlayerInput PI()=>Static(B+"InputManager","playerInput")as PlayerInput;
  static void Queue(Key[] keys){queued=keys;if(keyboard!=null)InputSystem.QueueStateEvent(keyboard,new KeyboardState(keys));if(mouse!=null)InputSystem.QueueStateEvent(mouse,new MouseState{position=mousePoint,scroll=scroll,delta=Vector2.zero}.WithButton(MouseButton.Left,mouseLeft));scroll=Vector2.zero;}
  static Key Bound(string action,string part=null){var a=PI().currentActionMap.FindAction(action,true);var rows=a.bindings.Where(b=>!b.isComposite&&(part==null?!b.isPartOfComposite:b.isPartOfComposite&&b.name==part)&&b.effectivePath!=null&&b.effectivePath.StartsWith("<Keyboard>/",StringComparison.Ordinal)).ToArray();Need(rows.Length==1,"One real keyboard binding "+action+"/"+part);var k=keyboard.TryGetChildControl<UnityEngine.InputSystem.Controls.KeyControl>(rows[0].effectivePath.Substring(11));Need(k!=null,"Real SDK key");return k.keyCode;}
  static int Raw(int id){var d=Static(B+"InputManager","inputDict")as IDictionary;Need(d!=null,"Original raw input dictionary");foreach(DictionaryEntry x in d)if(I(x.Key)==id)return I(x.Value);throw new InvalidOperationException("Missing raw input "+id);}
  static bool TapKey(string role,string action,int raw,out Key[] keys){keys=new Key[0];var t=report.taps.SingleOrDefault(x=>x.role==role);if(t==null){var k=Bound(action);t=new Tap{role=role,action=action,key=(int)k,raw=raw,path="<Keyboard>/"+keyboard[k].name,pressFrame=Time.frameCount};report.taps.Add(t);keys=new[]{k};return false;}Need(Time.frameCount-t.pressFrame<180,"Finite single key transaction "+role);if(t.releaseFrame==0&&Time.frameCount>t.pressFrame)t.releaseFrame=Time.frameCount;t.rawOne|=Raw(raw)==1;return t.pressSeen&&t.releaseSeen&&t.rawOne;}
  static void AfterInput()
  {
   if(!Armed||finished||InputState.currentUpdateType!=InputUpdateType.Dynamic||keyboard==null||mouse==null)return;
   try{if(finishing){if(!keyboard.anyKey.isPressed&&!mouse.leftButton.isPressed&&mouse.delta.ReadValue()==Vector2.zero&&mouse.scroll.ReadValue()==Vector2.zero)releaseUpdates++;else releaseUpdates=0;report.inputReleaseObserved=releaseUpdates>=2;return;}
    var pi=PI();if(!pi||pi.currentActionMap==null)return;foreach(var t in report.taps){var a=pi.currentActionMap.FindAction(t.action,false);if(a==null||!a.enabled)continue;t.pressSeen|=keyboard[(Key)t.key].isPressed&&a.ReadValue<float>()>0.5f;t.releaseSeen|=t.releaseFrame>0&&!keyboard[(Key)t.key].isPressed&&a.ReadValue<float>()==0f;t.rawOne|=Raw(t.raw)==1;}
    var es=EventSystem.current;var im=es?es.currentInputModule as InputSystemUIInputModule:null;if(!im||im.point==null||im.leftClick==null)return;var p=im.point.action;var c=im.leftClick.action;if(p==null||c==null||!p.enabled||!c.enabled)return;
    var tx=report.pointer.LastOrDefault();if(tx==null)return;bool at=(p.ReadValue<Vector2>()-tx.point).sqrMagnitude<1f&&(mouse.position.ReadValue()-tx.point).sqrMagnitude<1f;tx.moveSeen|=at;
    var hit=im.GetLastRaycastResult(mouse.deviceId).gameObject;var select=hit?hit.GetComponentInParent<Selectable>():null;bool own=select&&select.GetInstanceID()==tx.target;
    if(!tx.pressed&&at&&Time.frameCount>tx.moveFrame+1){tx.moveRaycast|=own;tx.moveHit=hit?PathOf(hit.transform):"";}
    if(tx.pressed&&!tx.released&&mouse.leftButton.isPressed&&c.ReadValue<float>()>0.5f&&at){tx.pressSeen=true;if(Time.frameCount>tx.pressFrame+1){Need(own,"Original control receives pointer press raycast");tx.pressRaycast=true;tx.pressHit=PathOf(hit.transform);}}
    if(tx.released&&!mouse.leftButton.isPressed&&c.ReadValue<float>()==0f&&at){tx.releaseSeen=true;tx.releaseHit=hit?PathOf(hit.transform):"";}
   }catch(Exception e){Finish(e);}
  }
  static string PathOf(Transform t)=>!t?"":!t.parent?t.name:PathOf(t.parent)+"/"+t.name;
  static Vector2 Center(RectTransform r){var c=r.GetComponentInParent<Canvas>();Need(c,"Real Canvas");return RectTransformUtility.WorldToScreenPoint(c.renderMode==RenderMode.ScreenSpaceOverlay?null:c.worldCamera,r.TransformPoint(r.rect.center));}
  static bool Callback(Button b,UnityEngine.Object target,string method,int? arg=null){for(int i=0;i<b.onClick.GetPersistentEventCount();i++){if(b.onClick.GetPersistentTarget(i)!=target||b.onClick.GetPersistentMethodName(i)!=method||b.onClick.GetPersistentListenerState(i)==UnityEngine.Events.UnityEventCallState.Off)continue;if(!arg.HasValue)return true;var rows=Read(Read(b.onClick,"m_PersistentCalls"),"m_Calls")as IList;Need(rows!=null&&i<rows.Count,"Real persistent arguments");if(I(Read(rows[i],"m_Mode"))==3&&I(Read(Read(rows[i],"m_Arguments"),"m_IntArgument"))==arg.Value)return true;}return false;}
  static Button ButtonFor(Behaviour root,UnityEngine.Object owner,string method,int? arg=null){Need(root&&owner,"Real UI owner");var bs=root.GetComponentsInChildren<Button>(true).Where(b=>Callback(b,owner,method,arg)).ToArray();Need(bs.Length==1,"One actual callback button "+method);return bs[0];}
  static bool Pointer(string role,Selectable control,Vector2 point,string kind)
  {
   var t=report.pointer.SingleOrDefault(x=>x.role==role);int f=Time.frameCount;if(t!=null&&t.released){mouseLeft=false;mousePoint=t.point;return t.releaseSeen&&f>t.releaseFrame+2;}
   Need(control&&control.enabled&&control.gameObject.activeInHierarchy&&control.interactable&&control.IsInteractable(),"Live original UI control "+role);Need(point.x>0&&point.y>0&&point.x<Screen.width&&point.y<Screen.height,"UI point within actual screen");var es=EventSystem.current;Need(es&&es.currentInputModule is InputSystemUIInputModule,"Original UI SDK module");
   if(t==null){Need(report.pointer.Count<64,"Bounded pointer transactions");t=new PointerTxn{role=role,path=PathOf(control.transform),kind=kind,target=control.GetInstanceID(),point=point,startFrame=f,moveFrame=f,readyTime=Time.unscaledTime};report.pointer.Add(t);}
   Need(f-t.startFrame<240,"Bounded pointer transaction "+role);Need(t.target==control.GetInstanceID(),"Same UI control instance");
   if(!t.pressed){if((point-t.point).sqrMagnitude>0.25f){t.point=point;t.moveFrame=f;t.readyTime=Time.unscaledTime;t.moveSeen=false;t.moveRaycast=false;}mousePoint=t.point;mouseLeft=false;var hits=new List<RaycastResult>();es.RaycastAll(new PointerEventData(es){position=t.point},hits);bool hit=hits.Count>0&&(hits[0].gameObject==control.gameObject||hits[0].gameObject.transform.IsChildOf(control.transform));if(!hit||!t.moveSeen||!t.moveRaycast||f<=t.moveFrame+2||Time.unscaledTime-t.readyTime<.20f)return false;t.pressed=true;t.pressFrame=f;mouseLeft=true;return false;}
   mousePoint=t.point;mouseLeft=true;if(t.pressSeen&&t.pressRaycast&&f>t.pressFrame+2){mouseLeft=false;t.released=true;t.releaseFrame=f;}return false;
  }
  static bool Click(string role,Button b){var done=report.pointer.SingleOrDefault(x=>x.role==role);if(done!=null&&done.released)return Pointer(role,null,done.point,"button");return Pointer(role,b,Center(b.transform as RectTransform),"button");}
  static bool PopupClick(string role,string method){var tx=report.pointer.SingleOrDefault(x=>x.role==role);if(tx!=null&&tx.released)return Click(role,null);popup=One(PopupType);if(!ReadyPanel(popup))return false;return Click(role,ButtonFor(popup,popup,method));}
  static Behaviour One(string type)=>UnityEngine.Object.FindObjectsOfType<MonoBehaviour>(true).Where(x=>x&&x.gameObject.scene.isLoaded&&x.GetType().FullName==type).Cast<Behaviour>().SingleOrDefault();
  static object Top(){var stack=Read(ui,"basePanelStack")as IList;Need(stack!=null,"Original panel stack");return stack.Count==0?null:stack[stack.Count-1];}
  static bool ReadyPanel(Behaviour b){if(!b||!b.enabled||!b.gameObject.activeInHierarchy||!ReferenceEquals(Top(),b)||Z(Read(b,"isAlreadyClose")))return false;var c=b.GetComponent<CanvasGroup>();return c&&c.alpha>.95f;}
  static bool Bind(){manager=Static(G+"InGameManager","Instance");if(!Alive(manager))return false;player=Read(Read(manager,"InGameReferencer"),"Player")as Component;if(!player)return false;pca=Read(player,"pca");if(pca==null)return false;state=Read(pca,"PlayerState");custom=Static(G+"GameState","PlayerCustomData");gsd=Static(G+"GameState","GameStateData");ui=Static("ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager","Instance");playerTransform=Read(pca,"Transform")as Transform;return state!=null&&custom!=null&&gsd!=null&&Alive(ui)&&playerTransform&&PI()&&PI().currentActionMap!=null;}
  static string Scalar(object v){Need(v!=null,"Known durable scalar");if(v is bool)return (bool)v?"true":"false";if(v is Enum)return Convert.ToInt64(v).ToString(CultureInfo.InvariantCulture);if(v is float)return BitConverter.ToInt32(BitConverter.GetBytes((float)v),0).ToString("X8");if(v is double)return BitConverter.DoubleToInt64Bits((double)v).ToString("X16");if(v is int||v is long||v is string)return Convert.ToString(v,CultureInfo.InvariantCulture);throw new InvalidOperationException("Unsupported durable scalar "+v.GetType());}
  static void ProjectValue(object v,string key,List<Row> rows,int depth)
  {
   Need(depth<5&&rows.Count<2048&&v!=null,"Bounded nonnull complete custom projection");if(v is IDictionary){var d=(IDictionary)v;Need(d.Count<256,"Bounded original dictionary");rows.Add(new Row{key=key+"/#count",type="count",value=d.Count.ToString()});foreach(DictionaryEntry x in d)ProjectValue(x.Value,key+"/"+Scalar(x.Key),rows,depth+1);return;}
   if(v.GetType().IsPrimitive||v is Enum||v is string){rows.Add(new Row{key=key,type=v.GetType().FullName,value=Scalar(v)});return;}
   Need(v.GetType().Assembly==Game,"Only actual custom Game records are recursively projected");foreach(var f in v.GetType().GetFields(Flags).Where(f=>!f.IsStatic).OrderBy(f=>f.Name,StringComparer.Ordinal))ProjectValue(Read(v,f.Name),key+"/"+f.Name,rows,depth+1);
  }
  static List<Row> CustomRows(){var r=new List<Row>();ProjectValue(custom,"PCD",r,0);Need(r.Select(x=>x.key).Distinct().Count()==r.Count,"Exact unique durable keyset");return r.OrderBy(x=>x.key,StringComparer.Ordinal).ToList();}
  static bool RowsEqual(IEnumerable<Row> a,IEnumerable<Row> b){var x=a.OrderBy(r=>r.key,StringComparer.Ordinal).ToArray();var y=b.OrderBy(r=>r.key,StringComparer.Ordinal).ToArray();return x.Length==y.Length&&x.Zip(y,(p,q)=>p.key==q.key&&p.type==q.type&&p.value==q.value).All(v=>v);}
  static List<Row> Economy(){var r=new List<Row>();foreach(string n in EconomyFields.Split('|'))ProjectValue(Read(gsd,n),"GSD/"+n,r,0);return r.OrderBy(x=>x.key,StringComparer.Ordinal).ToList();}
  const string EconomyFields="CurrentRp|StatistisTotalUseRp|ItemDict|ClothesDict|MAdultGoodsDict|PlayerCostumeDict|PlayerCosplayPartsDict|PlayerCurrentClothes";
  static Snapshot Observe(string role,bool retain=false)
  {
   var ar=Read(pca,"PlayerAvatarObjectReferencer");var face=Read(ar,"FaceMesh")as SkinnedMeshRenderer;var avatar=Read(pca,"AvatorTransform")as Transform;var body=Read(pca,"PlayerBodyCustomizeAdjuster");Need(face&&face.sharedMesh&&avatar&&body!=null,"Original physical character and face");var s=new Snapshot{role=role,frame=Time.frameCount,time=Time.unscaledTime,scene=SceneManager.GetActiveScene().path,position=playerTransform.position,height=I(Read(custom,"CustomizeHeight")),eyeSize=I(Read(custom,"CustomizeEyeSize")),eyeHeight=I(Read(custom,"CustomizeEyeHeight")),heightScale=F(Read(body,"HeightScale")),defaultBaseScale=F(Read(body,"defaultBaseScale")),avatarScale=avatar.localScale,avatarId=avatar.GetInstanceID(),faceId=face.GetInstanceID(),meshId=face.sharedMesh.GetInstanceID(),meshName=face.sharedMesh.name,meshCount=face.sharedMesh.blendShapeCount,custom=CustomRows(),economy=Economy()};Need(s.meshCount>414,"Original FaceMesh supports all recovered indices");if(report.baseline!=null&&contract.mode=="edit-save")Need(s.faceId==report.baseline.faceId&&s.meshId==report.baseline.meshId&&s.avatarId==report.baseline.avatarId&&s.meshName==report.baseline.meshName,"Stable original preview renderer/avatar/mesh during edits");var expect=ShapeOracle();foreach(var x in expect){int ix=int.Parse(x.key,CultureInfo.InvariantCulture);s.shapes.Add(new Shape{index=ix,name=face.sharedMesh.GetBlendShapeName(ix),actual=face.GetBlendShapeWeight(ix),expected=float.Parse(x.value,CultureInfo.InvariantCulture)});}s.modelMatches=Near(s.heightScale,s.height/160f)&&Vec(s.avatarScale,Vector3.one*s.defaultBaseScale*(s.height/160f))&&s.shapes.All(x=>Near(x.actual,x.expected));report.lastSample=s;if(retain){Need(report.snapshots.Count<40,"Bounded milestone snapshots");report.snapshots.Add(s);}return s;
  }
  static List<Row> ShapeOracle()
  {
   var r=new List<Row>();Action<int,float> set=(i,v)=>{var row=r.SingleOrDefault(x=>x.key==i.ToString());if(row==null)r.Add(new Row{key=i.ToString(),value=v.ToString("R",CultureInfo.InvariantCulture)});else row.value=v.ToString("R",CultureInfo.InvariantCulture);};Func<string,int> val=n=>I(Read(custom,n));Action<string,int,int> pair=(n,pos,neg)=>{int v=val(n);set(pos,Mathf.Clamp((v-50)*2,0,100));set(neg,Mathf.Clamp((50-v)*2,0,100));};
   if(!Z(Read(custom,"CustomizeAdjustHeight")))pair("CustomizeLoliFace",400,401);else{float a=(1f-Mathf.Min((val("CustomizeHeight")-120)/40f,1f))*100f;set(401,Mathf.Clamp(a*-2f+100f,0f,100f));set(400,Mathf.Clamp((a-50f)*2f,0f,100f));}
   pair("CustomizeEyeSize",379,380);pair("CustomizeTareme",55,64);pair("CustomizeEyeDist",375,376);pair("CustomizeEyeHeight",373,374);pair("CustomizeMatsugeThick",147,148);pair("CustomizeMatsugeThick",149,150);
   int a1=Mathf.Clamp((val("CustomizeMatsugeLength")-50)*2,0,100),a2=Mathf.Clamp((50-val("CustomizeMatsugeLength"))*2,0,100);foreach(string n in "145,158,159,160,161,166,167,168".Split(','))set(int.Parse(n),a1);foreach(string n in "146,162,163,164,165,169,170,161".Split(','))set(int.Parse(n),a2);
   set(414,Z(Read(custom,"CustomizeEyeDark"))?101:0);pair("CustomizeMayuThick",363,364);set(365,val("CustomizeMayuDog"));set(393,Mathf.Clamp((val("CustomizeCheekFat")-50)*8,0,400));set(392,Mathf.Clamp((50-val("CustomizeCheekFat"))*2,0,100));pair("CustomizeChinLength",394,395);set(388,val("CustomizeEarElf"));set(312,Z(Read(custom,"CustomizeYaeba"))?100:0);set(313,Z(Read(custom,"CustomizeYaeba"))?100:0);return r;
  }
  static bool Near(float a,float b)=>!float.IsNaN(a)&&!float.IsInfinity(a)&&Mathf.Abs(a-b)<.001f;
  static bool Vec(Vector3 a,Vector3 b)=>(a-b).sqrMagnitude<.000001f;
  static float XZ(Vector3 a,Vector3 b)=>new Vector2(a.x-b.x,a.z-b.z).magnitude;
  static List<Row> WithValue(List<Row> source,string field,int value)=>source.Select(x=>new Row{key=x.key,type=x.type,value=x.key=="PCD/"+field?value.ToString(CultureInfo.InvariantCulture):x.value}).ToList();
  static bool Shot(string role)
  {
   var c=report.captures.SingleOrDefault(x=>x.role==role);if(c==null){Need(report.captures.Count<8,"Bounded actual screenshots");c=new Capture{role=role,path=Path.Combine(contract.output,role+".png"),requestFrame=Time.frameCount,requestTime=Time.unscaledTime};Need(Inside(c.path,contract.output)&&!File.Exists(c.path),"Fresh owned screenshot path");report.captures.Add(c);ScreenCapture.CaptureScreenshot(c.path);return false;}if(c.ready)return true;Need(Time.unscaledTime-c.requestTime<15,"Actual Player screenshot must be written");if(Time.frameCount<=c.requestFrame+3||!File.Exists(c.path))return false;var f=new FileInfo(c.path);if(f.Length<24)return false;Need(f.Length<32*1024*1024,"Bounded screenshot");using(var input=File.OpenRead(c.path)){var h=new byte[24];Need(input.Read(h,0,h.Length)==h.Length&&h[0]==137&&h[1]==80&&h[2]==78&&h[3]==71,"Original PNG screenshot");c.width=(h[16]<<24)|(h[17]<<16)|(h[18]<<8)|h[19];c.height=(h[20]<<24)|(h[21]<<16)|(h[22]<<8)|h[23];}Need(c.width==Screen.width&&c.height==Screen.height&&c.width>=640&&c.height>=360,"Actual full Player framebuffer dimensions");c.bytes=f.Length;c.sha256=Hash(c.path);c.ready=true;return true;
  }
  static Rect ScreenRect(RectTransform rt){var c=rt.GetComponentInParent<Canvas>();var camera=c&&c.renderMode!=RenderMode.ScreenSpaceOverlay?c.worldCamera:null;var p=new Vector3[4];rt.GetWorldCorners(p);var q=p.Select(x=>RectTransformUtility.WorldToScreenPoint(camera,x)).ToArray();return Rect.MinMaxRect(q.Min(x=>x.x),q.Min(x=>x.y),q.Max(x=>x.x),q.Max(x=>x.y));}
  static bool SliderInput(string role,string controlName,string field,int target)
  {
   var t=report.sliders.SingleOrDefault(x=>x.role==role);if(t==null){Need(report.sliders.Count<20,"Bounded sliders");t=new SliderTxn{role=role,field=controlName,customField=field,targetValue=target,startFrame=Time.frameCount};report.sliders.Add(t);}if(t.complete)return true;Need(Time.frameCount-t.startFrame<900,"Finite original slider transaction");Need(view&&view.gameObject.activeInHierarchy,"Original customize child active");var item=Read(view,controlName)as Behaviour;var sl=Read(item,"slider")as Slider;Need(item&&item.enabled&&sl&&sl.enabled&&sl.gameObject.activeInHierarchy&&sl.IsInteractable()&&Z(Read(item,"isPlayerCustom"))&&Read(item,"onChange") is Delegate,"Original initialized slider and bound setter");Need(!((GameObject)Read(item,"rankConditionMask")).activeInHierarchy&&!((GameObject)Read(item,"adjustHeightMask")).activeInHierarchy,"Original slider unlocked");Need(sl.wholeNumbers&&target>=sl.minValue&&target<=sl.maxValue,"Target inside actual integer range");bool callback=false;for(int i=0;i<sl.onValueChanged.GetPersistentEventCount();i++)callback|=sl.onValueChanged.GetPersistentTarget(i)==item&&sl.onValueChanged.GetPersistentMethodName(i)=="OnChangeSlider"&&sl.onValueChanged.GetPersistentListenerState(i)!=UnityEngine.Events.UnityEventCallState.Off;Need(callback,"Original OnChangeSlider persistent callback");if(t.sliderId==0)t.sliderId=sl.GetInstanceID();Need(t.sliderId==sl.GetInstanceID(),"Same actual slider");
   var rect=sl.handleRect?sl.handleRect.parent as RectTransform:sl.fillRect?sl.fillRect.parent as RectTransform:sl.transform as RectTransform;Need(rect,"Original slider handle container");var sr=sl.GetComponentInParent<ScrollRect>();var viewport=sr&&sr.viewport?ScreenRect(sr.viewport):new Rect(0,0,Screen.width,Screen.height);var rr=ScreenRect(rect);bool visible=rr.yMin>=viewport.yMin+1&&rr.yMax<=viewport.yMax-1&&rr.xMin>=viewport.xMin-2&&rr.xMax<=viewport.xMax+2;
   var tx=report.pointer.SingleOrDefault(x=>x.role==role);if(tx==null&&!visible){Need(sr&&sr.enabled&&sr.vertical&&t.scrollFrames++<360,"Bounded real ScrollRect input");if(!t.hasWindow){t.hasWindow=true;t.windowTime=Time.unscaledTime;t.windowY=rr.center.y;}if(Time.unscaledTime-t.windowTime>2f){Need(Mathf.Abs(rr.center.y-t.windowY)>5f,"Actual UI scroll progress");t.progressFrames++;t.windowY=rr.center.y;t.windowTime=Time.unscaledTime;}mousePoint=viewport.center;scroll=new Vector2(0,rr.center.y<viewport.center.y?-24f:24f);mouseLeft=false;return false;}
   float fraction=(target-sl.minValue)/(sl.maxValue-sl.minValue);if(sl.direction==Slider.Direction.RightToLeft||sl.direction==Slider.Direction.TopToBottom)fraction=1-fraction;var local=rect.rect.center;if(sl.direction==Slider.Direction.LeftToRight||sl.direction==Slider.Direction.RightToLeft)local=new Vector2(rect.rect.xMin+rect.rect.width*fraction,local.y);else local=new Vector2(local.x,rect.rect.yMin+rect.rect.height*fraction);var canvas=sl.GetComponentInParent<Canvas>();var point=RectTransformUtility.WorldToScreenPoint(canvas.renderMode==RenderMode.ScreenSpaceOverlay?null:canvas.worldCamera,rect.TransformPoint(local));Need(viewport.Contains(point),"Actual target point within viewport");if(!Pointer(role,sl,point,"slider"))return false;
   var s=Observe(role);Need(Time.frameCount-t.startFrame<900,"Physical effect bound");bool ok=I(Read(custom,field))==target&&Near(sl.value,target)&&s.modelMatches;if(ok)t.stableFrames++;else t.stableFrames=0;if(t.stableFrames<3)return false;t.complete=true;t.result=s;Need(report.snapshots.Count<40,"Bounded milestone snapshots");report.snapshots.Add(s);return true;
  }
  static bool FaceTab(string role,int tab){var t=Read(view,"customizeCommonTabView")as Behaviour;Need(t,"Original nested tab");return Click(role,ButtonFor(t,t,"OnClickTab",tab));}
  static bool FaceVisible()=>((GameObject)Read(view,"faceView")).activeInHierarchy;
  static bool BodyVisible()=>((GameObject)Read(view,"bodyView")).activeInHierarchy;
  static bool InControl(){return Top()==null&&!Z(Read(ui,"IsForceDeActive"))&&!Z(Read(ui,"IsOpenSomeCancelInputUi"))&&!Z(Read(ui,"isOpenSomeAllowInputUi"))&&!Z(Read(ui,"IsOpenFade"))&&Time.timeScale>0;}
  static Key[] Steering(Vector3 target,float stopRadius=.05f)
  {
   var camera=Read(Read(manager,"InGameReferencer"),"TpCameraController")as Component;Need(camera,"Original third-person camera");var h=playerTransform.position-camera.transform.position;h=new Vector3(h.x,0,h.z);Need(h.sqrMagnitude>.00001f,"Original movement heading");h.Normalize();var v=target-playerTransform.position;v=new Vector3(v.x,0,v.z);if(v.magnitude<stopRadius)return new Key[0];v.Normalize();var right=Vector3.Cross(Vector3.up,h);var keys=new List<Key>();float x=Vector3.Dot(v,right),z=Vector3.Dot(v,h);if(z>.3826834f)keys.Add(Bound("Move","up"));if(z<-.3826834f)keys.Add(Bound("Move","down"));if(x>.3826834f)keys.Add(Bound("Move","right"));if(x<-.3826834f)keys.Add(Bound("Move","left"));return keys.ToArray();
  }
  static bool WardrobeReady()
  {
   if(!sensor){var xs=UnityEngine.Object.FindObjectsOfType<MonoBehaviour>(true).Where(x=>x&&x.gameObject.scene.path==Home&&x.GetType().FullName=="ExposureUnnoticed2.Object3D.InteractArea.InteractAreaController"&&I(Read(x,"type"))==11).ToArray();Need(xs.Length==1,"One original Home closet sensor");sensor=xs[0];}
   var list=Static(B+"InteractManager","interactList")as IList;Need(list!=null,"Original interaction registrations");var sphere=sensor.GetComponent<SphereCollider>();var character=player.GetComponent<CharacterController>();Need(sphere&&character,"Original closet sphere and Player character collider");var head=list.Count==0?null:list[list.Count-1];var probe=new RouteProbe{frame=Time.frameCount,time=Time.unscaledTime,playerPosition=playerTransform.position,sensorId=sensor.GetInstanceID(),sensorCenter=sphere.transform.TransformPoint(sphere.center),sensorScale=sphere.transform.lossyScale,sphereRadius=sphere.radius,characterRadius=character.radius,sensorActive=sensor.gameObject.activeInHierarchy&&sensor.enabled,triggerEnabled=sphere.enabled&&sphere.isTrigger,isInArea=Z(Read(sensor,"isInArea")),listCount=list.Count,headType=head==null?-1:I(Read(head,"Type")),headControllerId=head==null?0:Id(Read(head,"Controller"))};probe.headMatches=head!=null&&probe.headType==11&&ReferenceEquals(Read(head,"Controller"),sensor);probe.ready=probe.isInArea&&probe.headMatches;probe.xzDistance=XZ(probe.playerPosition,probe.sensorCenter);report.lastRouteProbe=probe;return probe.ready;
  }
  static Key[] Navigate()
  {
   Need(InControl(),"Original Home control gates");var p=playerTransform.position;if(report.routeStartFrame==0){Need(Mathf.Abs(p.y)<.3f&&p.x>-.4f&&p.x<6f&&p.z>-1.6f&&p.z<1.6f,"Known Home corridor domain, loaded position not assumed");report.routeStartFrame=Time.frameCount;report.routePrevious=p;report.routeWindowTime=Time.unscaledTime;report.routeWaypoint=0;report.routeWindowDistance=XZ(p,Vector3.zero);}
   float step=XZ(p,report.routePrevious);Need(step<.5f,"No warp in original walking");report.routeWalk+=step;report.routePrevious=p;Need(report.routeWalk<12&&Time.frameCount-report.routeStartFrame<1800,"Bounded Home walking");Vector3 target=report.routeWaypoint==0?Vector3.zero:report.routeWaypoint==1?new Vector3(0,0,-.9971253f):new Vector3(.104604885f,0,-1.230499506f);if(report.routeWaypoint<2&&XZ(p,target)<.07f){report.routeWaypoint++;report.routeWindowTime=Time.unscaledTime;report.routeWindowWalk=report.routeWalk;target=report.routeWaypoint==1?new Vector3(0,0,-.9971253f):new Vector3(.104604885f,0,-1.230499506f);report.routeWindowDistance=XZ(p,target);}if(Time.unscaledTime-report.routeWindowTime>10){Need(report.routeWalk-report.routeWindowWalk>.15f&&report.routeWindowDistance-XZ(p,target)>.1f,"Real route progress");report.routeWindowTime=Time.unscaledTime;report.routeWindowWalk=report.routeWalk;report.routeWindowDistance=XZ(p,target);}return Steering(target,report.routeWaypoint==2?.01f:.05f);
  }
  static void SaveIsolation()
  {
   Need(SamePath(Application.persistentDataPath,Profile)&&I(Static(G+"SaveManager","CurrentSaveSlot"))==report.saveSlot,"Same actual original slot/profile");var settings=Static("ES3Settings","_defaults");var asset=Read(Static("ES3Settings","_defaultSettingsScriptableObject"),"settings");Need(settings!=null&&ReferenceEquals(settings,asset)&&I(Read(settings,"_location"))==0&&I(Read(settings,"directory"))==0&&SamePath((string)Static("ES3Internal.ES3IO","persistentDataPath"),Profile),"Original ES3 File/Persistent settings/cache");foreach(string n in "ManualSaveTime|DifficultData|RankData|TotalPlayTimeData".Split('|')){var a=Static(G+"GameState",n)as Array;Need(a!=null&&report.saveSlot<a.Length,"Original save slot array "+n);}
  }
  static SaveSample SaveState(){SaveIsolation();var times=Static(G+"GameState","ManualSaveTime")as Array;var d=times.GetValue(report.saveSlot);Need(d is DateTime,"Original manual save timestamp");var s=new SaveSample{frame=Time.frameCount,slot=report.saveSlot,ticks=((DateTime)d).Ticks,path=Static(B+"SaveRapper","currentDataPath")as string,system=Z(Static(B+"SaveRapper","IsSystemData")),backup=Z(Static(B+"SaveRapper","IsBackup")),home=Z(Static(B+"SaveRapper","IsHomeSave")),autoSave=Z(Static(B+"SaveRapper","IsAutoSave")),interrupt=Z(Static(B+"SaveRapper","IsInterrupt"))};if(!string.IsNullOrEmpty(s.path))Need(Inside(Path.IsPathRooted(s.path)?s.path:Path.Combine(Profile,s.path),Profile),"Observed original save routing inside test");string k=s.ticks+"|"+s.path+"|"+s.system+s.backup+s.home+s.autoSave+s.interrupt;if(k!=lastSaveKey){Need(report.saves.Count<32,"Bounded save milestones");report.saves.Add(s);lastSaveKey=k;}return s;}
  static bool Saved()
  {
   var s=SaveState();Need(s.slot==report.saveSlot,"Same slot");if(s.ticks<=report.saves[0].ticks||s.system||s.backup||s.home||s.autoSave||s.interrupt)return false;Need(s.path!=null&&SamePath(Path.IsPathRooted(s.path)?s.path:Path.Combine(Profile,s.path),Path.Combine(Profile,"SaveData/"+report.saveSlot+".sd")),"Original final ManualSlot path");if(Time.unscaledTime-lastSaveCheck<.25f)return false;lastSaveCheck=Time.unscaledTime;var files=Tree(Profile);bool changed=true;foreach(string rel in new[]{"SaveData/"+report.saveSlot+".sd","SaveData/s.sd"}){var p=files.Single(x=>SamePath(x.path,Path.Combine(Profile,rel)));changed&=!report.testInventoryBefore.Any(x=>SamePath(p.path,x.path)&&x.bytes==p.bytes&&x.sha256==p.sha256);}if(!changed)return false;string key=string.Join("|",files.Select(x=>x.path+":"+x.sha256));if(key!=lastStableFiles){lastStableFiles=key;stableFilesSince=Time.unscaledTime;return false;}return Time.unscaledTime-stableFilesSince>=.5f;
  }
  static void MoveRecord(){Need(report.movement.Count<1024,"Bounded per-frame movement");var a=PI().currentActionMap.FindAction("Move",true);var old=report.movement.LastOrDefault();report.movement.Add(new Movement{frame=Time.frameCount,time=Time.unscaledTime,position=playerTransform.position,sdk=a.ReadValue<Vector2>(),pressed=keyboard.anyKey.isPressed,step=old==null?0:XZ(old.position,playerTransform.position)});}
  static void Advance(int n){report.phase=n;report.phaseFrame=Time.frameCount;report.phaseTime=Time.unscaledTime;mouseLeft=false;scroll=Vector2.zero;Write();}
  static CameraPose CameraPoseOf(Transform t){Need(t,"Camera diagnostic requires a live original transform");return new CameraPose{id=t.GetInstanceID(),path=PathOf(t),position=t.position,rotation=t.rotation,forward=t.forward,lossyScale=t.lossyScale};}
  static CameraCollider CameraColliderOf(Collider c,Vector3 cameraPosition){Need(c,"Live queried collider");var b=c.bounds;return new CameraCollider{id=c.GetInstanceID(),path=PathOf(c.transform),type=c.GetType().FullName,layer=c.gameObject.layer,layerName=LayerMask.LayerToName(c.gameObject.layer),trigger=c.isTrigger,boundsCenter=b.center,boundsSize=b.size,boundsContainCamera=b.Contains(cameraPosition)};}
  static CameraObservation ObserveCamera(string role,float scheduled,double elapsed)
  {
   var referencer=Read(manager,"InGameReferencer");var tp=Read(referencer,"TpCameraController")as Component;var bc=Read(referencer,"BaseCameraController")as Component;var avatar=Read(pca,"AvatorTransform")as Transform;Need(tp&&bc&&avatar,"Original TP/Base/avatar objects");var main=bc.GetComponent<Camera>();Need(main&&main.enabled&&main.gameObject.activeInHierarchy,"Active original Base camera");var target=Read(tp,"cameraTarget")as Transform;Need(target,"Original initialized camera target");
   float yaw=F(Read(tp,"Yaw")),pitch=F(Read(tp,"Pitch")),vertical=F(Read(tp,"currentVerticalValue")),maximum=F(Read(tp,"cameraMaxDistance"));int ignore=I(Read(tp,"ignoreLayerMask"));Need(!float.IsNaN(maximum)&&!float.IsInfinity(maximum)&&maximum>0&&maximum<10,"Bounded actual original camera query length");
   Vector3 origin=new Vector3(target.position.x,Mathf.Max(.3f,(target.position.y-playerTransform.position.y)*vertical)+playerTransform.position.y,target.position.z);Vector3 direction=Quaternion.Euler(pitch,yaw,0f)*Vector3.forward*-1f;
   var row=new CameraObservation{role=role,scene=SceneManager.GetActiveScene().path,frame=Time.frameCount,time=Time.unscaledTime,realtime=Time.realtimeSinceStartupAsDouble,elapsed=elapsed,scheduledSeconds=scheduled,height=I(Read(custom,"CustomizeHeight")),avatarScale=avatar.localScale,player=CameraPoseOf(playerTransform),avatar=CameraPoseOf(avatar),tp=CameraPoseOf(tp.transform),baseCamera=CameraPoseOf(bc.transform),cameraTarget=CameraPoseOf(target),yaw=yaw,pitch=pitch,currentVerticalValue=vertical,maxDistance=maximum,ignoreLayerMask=ignore,castLayerMask=~ignore,inventoryCamera=Z(Read(bc,"IsActiveInventoryCamera")),isStrafeCache=Z(Read(bc,"isStrafeCache")),castOrigin=origin,castDirection=direction,castRadius=.05f,targetToTpDistance=Vector3.Distance(origin,tp.transform.position),targetToMainDistance=Vector3.Distance(origin,main.transform.position)};
   var cameras=Camera.allCameras;Need(cameras.Length>0&&cameras.Length<=16,"At most sixteen active cameras, never silently truncate main view");foreach(var c in cameras.OrderBy(x=>x.GetInstanceID())){var rt=c.targetTexture;row.cameras.Add(new CameraView{id=c.GetInstanceID(),path=PathOf(c.transform),enabled=c.enabled,isBaseCamera=c==main,mask=c.cullingMask,targetTextureId=rt?rt.GetInstanceID():0,targetTextureName=rt?rt.name:null,clearFlags=c.clearFlags.ToString(),near=c.nearClipPlane,far=c.farClipPlane,fov=c.fieldOfView,depth=c.depth,rect=c.rect,pixelRect=c.pixelRect,pose=CameraPoseOf(c.transform)});}Need(row.cameras.Count(x=>x.isBaseCamera)==1,"Original Base camera is in active rendering cameras");
   RaycastHit hit;row.hit=Physics.SphereCast(origin,.05f,direction,out hit,maximum,~ignore,QueryTriggerInteraction.Ignore);if(row.hit){row.hitDistance=hit.distance;row.hitPoint=hit.point;row.hitNormal=hit.normal;row.hitCollider=CameraColliderOf(hit.collider,main.transform.position);}
   var nearby=new Collider[16];row.nearbyLayerMask=~0;int count=Physics.OverlapSphereNonAlloc(main.transform.position,.05f,nearby,row.nearbyLayerMask,QueryTriggerInteraction.Ignore);row.nearbyCount=count;row.nearbyTruncated=count==nearby.Length;for(int i=0;i<count;i++)row.nearby.Add(CameraColliderOf(nearby[i],main.transform.position));return row;
  }
  void LateUpdate()
  {
   if(!Armed||finished||finishing||contract==null||contract.mode!="camera")return;try{if(!Bind())return;PollCamera();}catch(Exception e){Finish(e);}
  }
  static void PollCamera()
  {
   Need(keyboard==null&&mouse==null&&!pairingWasCaptured&&!backgroundCaptured&&report.taps.Count==0&&report.pointer.Count==0&&report.sliders.Count==0&&report.movement.Count==0,"Pure passive camera diagnostic, no owned input or UI transaction");var scene=SceneManager.GetActiveScene();Need(scene.isLoaded&&scene.path==Home,"Camera diagnostic remains in original active loaded Home");
   if(!cameraStarted){if(!InControl())return;SaveIsolation();report.cameraReadyFrame=Time.frameCount;report.cameraReadyTime=Time.unscaledTime;report.cameraReadyRealtime=Time.realtimeSinceStartupAsDouble;cameraStarted=true;report.status="camera-passive-observing";Write();}
   Need(InControl(),"Original Home control remains clear during passive captures");Need(cameraOrdinal>=0&&cameraOrdinal<4,"Exactly four passive camera captures");double elapsed=Time.realtimeSinceStartupAsDouble-report.cameraReadyRealtime;float scheduled=cameraOrdinal==0?.5f:cameraOrdinal==1?2f:cameraOrdinal==2?5f:10f;if(elapsed<scheduled)return;string role="camera-passive-"+(cameraOrdinal+1).ToString("D2");
   if(!report.cameraObservations.Any(x=>x.role==role)){Need(report.cameraObservations.Count<4,"Bounded four camera readbacks");report.cameraObservations.Add(ObserveCamera(role,scheduled,elapsed));}if(!Shot(role))return;cameraOrdinal++;Write();if(cameraOrdinal<4)return;
   Need(report.cameraObservations.Count==4&&report.captures.Count==4&&report.captures.All(x=>x.ready),"Four acquired real framebuffer images and camera rows");report.screenshotEvidenceComplete=true;report.diagnosticComplete=true;report.status="camera-diagnostic-completed-pending-visual-review";Finish(null);
  }

  static void Poll()
  {
   if(Time.frameCount==lastFrame)return;lastFrame=Time.frameCount;Need(DateTime.UtcNow<deadline,"Overall wall bound");Need(report.errors.Count==0,"No actual Game errors");report.frames++;Need(report.frames<30000,"Bounded observed frames");if(!Bind())return;if(contract.mode=="camera")return;if(firstBoundTime<0)firstBoundTime=Time.unscaledTime;if(Time.unscaledTime-firstBoundTime<.5f)return;var activeScene=SceneManager.GetActiveScene();Need(activeScene.isLoaded&&activeScene.path==Home,"Remain in original active loaded Home");if(Time.timeScale<=0)return;
   if(contract.mode=="cold"){var s=Observe("cold");Need(RowsEqual(s.custom,prior.savedAnchor.custom)&&RowsEqual(s.economy,prior.savedAnchor.economy),"Exact complete cold custom/economy keyset and values");Need(s.meshName==prior.savedAnchor.meshName&&s.meshCount==prior.savedAnchor.meshCount,"Same actual face mesh identity by asset name/count");if(!s.modelMatches||!InControl()){coldFrames=0;return;}Need(Near(s.heightScale,prior.savedAnchor.heightScale)&&Near(s.defaultBaseScale,prior.savedAnchor.defaultBaseScale)&&Vec(s.avatarScale,prior.savedAnchor.avatarScale)&&s.shapes.Count==prior.savedAnchor.shapes.Count&&s.shapes.All(x=>prior.savedAnchor.shapes.Any(y=>x.index==y.index&&x.name==y.name&&Near(x.actual,y.actual)&&Near(x.expected,y.expected))),"Cold physical scale and complete reviewed shape set equal saved anchor");coldFrames++;if(coldFrames<20)return;if(!Shot("cold-loaded-face-body"))return;Observe("cold-final",true);report.coldMatched=true;report.flowAccepted=true;report.screenshotEvidenceComplete=report.captures.All(x=>x.ready);report.status="cold-completed-pending-visual-review";Finish(null);return;}
   if(!SetupSDK())return;Key[] keys=new Key[0];if(report.phase>0){Need(RowsEqual(Economy(),report.baseline.economy),"No economy/ownership/equipment changes in customization");Need(ReferenceEquals(custom,Static(G+"GameState","PlayerCustomData")),"Same live customization record");}
   switch(report.phase)
   {
    case 0:if(!InControl())break;SaveState();var initial=Observe("startup");if(!initial.modelMatches){Need(Time.unscaledTime-firstBoundTime<10,"Original initial body/face application must match recovered oracle");break;}report.baseline=Observe("loaded-baseline",true);report.baselineReady=true;Advance(1);break;
    case 1:if(!WardrobeReady()){keys=Navigate();break;}Advance(2);break;
    case 2:if(TapKey("open-closet","Interact",8,out keys))Advance(3);break;
    case 3:closet=One(ClosetType);if(!ReadyPanel(closet))break;view=Read(closet,"customizeView")as Behaviour;if(!view||!view.gameObject.activeInHierarchy||!BodyVisible())break;Need(ReferenceEquals(Static("ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu.CustomizeMenuView","Instance"),view),"Original child instance");var ar=Read(pca,"PlayerAvatarObjectReferencer");foreach(string n in "Clothes|Bra|Pants|Costume".Split('|')){string field=n=="Costume"?"CosplayParent":n+"Parent";var go=Read(ar,field)as GameObject;Need(go,"Original clothing object");report.originalClothes.Add(new Row{key=field,type=Id(go).ToString(),value=Z(Read(closet,"isInitDisplay"+n))?"true":"false"});}report.closetEntered=true;Advance(4);break;
    case 4:if(!Shot("body-baseline"))break;Advance(5);break;
    case 5:if(!SliderInput("body-change","height","CustomizeHeight",report.baseline.height<=160?180:140))break;report.bodyChanged=true;if(!Shot("body-changed"))break;Advance(6);break;
    case 6:if(!SliderInput("body-restore","height","CustomizeHeight",report.baseline.height))break;Need(RowsEqual(CustomRows(),report.baseline.custom),"Entire PCD restored after height slider");report.bodyRestored=true;Advance(7);break;
    case 7:if(!FaceTab("face-tab",1))break;Advance(8);break;
    case 8:if(!FaceVisible())break;if(!Shot("face-baseline"))break;Advance(9);break;
    case 9:if(!SliderInput("eye-size-change","eyeSize","CustomizeEyeSize",report.baseline.eyeSize<=50?75:25))break;Advance(10);break;
    case 10:if(!SliderInput("eye-height-change","eyeHeight","CustomizeEyeHeight",report.baseline.eyeHeight<=50?75:25))break;report.faceChanged=true;if(!Shot("face-changed"))break;Advance(11);break;
    case 11:if(!SliderInput("eye-size-restore","eyeSize","CustomizeEyeSize",report.baseline.eyeSize))break;Advance(12);break;
    case 12:if(!SliderInput("eye-height-restore","eyeHeight","CustomizeEyeHeight",report.baseline.eyeHeight))break;Need(RowsEqual(CustomRows(),report.baseline.custom),"Entire PCD restored after face sliders");report.faceRestored=true;popupBaseline=Observe("before-default-cancel",true);Advance(13);break;
    case 13:if(!Click("default-open-cancel",ButtonFor(view,view,"OnClickDefaultFace")))break;Advance(14);break;
    case 14:if(!PopupClick("default-cancel","OnClickTwoLeft"))break;Advance(15);break;
    case 15:if(!ReadyPanel(closet)||Alive(popup))break;Need(RowsEqual(CustomRows(),popupBaseline.custom)&&Observe("cancel-result").modelMatches,"Default popup cancel changes no parameter or original shape");report.defaultCancelled=true;Advance(16);break;
    case 16:if(!Click("default-open-confirm",ButtonFor(view,view,"OnClickDefaultFace")))break;Advance(17);break;
    case 17:if(!PopupClick("default-confirm","OnClickTwoRight"))break;Advance(18);break;
    case 18:if(!ReadyPanel(closet)||Alive(popup))break;Need(DefaultFaceMatches(popupBaseline.custom)&&Observe("default-confirmed",true).modelMatches,"Exactly25 original face defaults and event applied");report.defaultConfirmed=true;Advance(19);break;
    case 19:if(!SliderInput("final-eye-size","eyeSize","CustomizeEyeSize",report.baseline.eyeSize<=50?75:25))break;Advance(20);break;
    case 20:if(!SliderInput("final-eye-height","eyeHeight","CustomizeEyeHeight",report.baseline.eyeHeight<=50?75:25))break;Advance(21);break;
    case 21:if(!FaceTab("body-tab",0))break;Advance(22);break;
    case 22:if(!BodyVisible())break;if(!SliderInput("final-height","height","CustomizeHeight",report.baseline.height<=160?180:140))break;Advance(23);break;
    case 23:if(!FaceTab("final-face-tab",1))break;Advance(24);break;
    case 24:if(!FaceVisible()||!Shot("final-customization"))break;report.savedAnchor=Observe("save-anchor",true);Need(report.savedAnchor.modelMatches&&report.savedAnchor.height!=report.baseline.height&&report.savedAnchor.eyeSize!=report.baseline.eyeSize&&report.savedAnchor.eyeHeight!=report.baseline.eyeHeight,"Final nonbaseline body and face retained");report.finalDifferent=true;SaveIsolation();Need(PinsEqual(Tree(Profile),report.testInventoryBefore),"No unexplained pre-close save writes");Advance(25);break;
    case 25:if(report.closeFrame==0)report.closeFrame=Time.frameCount;if(TapKey("original-close-save","Cancel",10,out keys))Advance(26);break;
    case 26:if(Alive(closet)||Static("ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu.CustomizeMenuView","Instance")!=null||!InControl())break;Need(Time.frameCount>=report.closeFrame+5,"Original delayed close cadence");Need(!Z(Read(Read(Read(manager,"InGameReferencer"),"BaseCameraController"),"IsActiveInventoryCamera")),"Original inventory camera restored");foreach(var row in report.originalClothes){var go=Read(Read(pca,"PlayerAvatarObjectReferencer"),row.key)as GameObject;Need(go&&Id(go).ToString()==row.type&&go.activeInHierarchy==(row.value=="true"),"Original clothes visibility restored "+row.key);}Need(RowsEqual(CustomRows(),report.savedAnchor.custom),"Back saves retained edits rather than rolling them back");report.originalCloseObserved=true;report.closedFrame=Time.frameCount;Advance(27);break;
    case 27:if(!Saved())break;report.saveObserved=true;report.firstCloseSaveTicks=SaveState().ticks;Advance(28);break;
    case 28:if(!WardrobeReady()){keys=Navigate();break;}if(TapKey("reopen-closet","Interact",8,out keys))Advance(29);break;
    case 29:closet=One(ClosetType);if(!ReadyPanel(closet))break;view=Read(closet,"customizeView")as Behaviour;if(!view||!view.gameObject.activeInHierarchy||!BodyVisible())break;Need(Near(((Slider)Read(Read(view,"height"),"slider")).value,report.savedAnchor.height),"Reopened original height control retains final value");Need(RowsEqual(CustomRows(),report.savedAnchor.custom)&&Observe("reopened-body").modelMatches,"Reopened body preview matches final saved customization");Advance(30);break;
    case 30:if(!FaceTab("reopened-face-tab",1))break;Advance(31);break;
    case 31:if(!FaceVisible())break;Need(Near(((Slider)Read(Read(view,"eyeSize"),"slider")).value,report.savedAnchor.eyeSize)&&Near(((Slider)Read(Read(view,"eyeHeight"),"slider")).value,report.savedAnchor.eyeHeight),"Both reopened actual face sliders retain final values");Need(RowsEqual(CustomRows(),report.savedAnchor.custom)&&Observe("reopened-face",false).modelMatches,"Reopened full customization and model preview unchanged");if(!Shot("reopened-customization"))break;report.reopenedUiMatched=true;report.closeFrame=Time.frameCount;Advance(32);break;
    case 32:if(TapKey("reopened-original-close-save","Cancel",10,out keys))Advance(33);break;
    case 33:if(Alive(closet)||Static("ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu.CustomizeMenuView","Instance")!=null||!InControl())break;Need(Time.frameCount>=report.closeFrame+5&&!Z(Read(Read(Read(manager,"InGameReferencer"),"BaseCameraController"),"IsActiveInventoryCamera")),"Reopened original delayed close/camera restore");foreach(var row in report.originalClothes){var go=Read(Read(pca,"PlayerAvatarObjectReferencer"),row.key)as GameObject;Need(go&&Id(go).ToString()==row.type&&go.activeInHierarchy==(row.value=="true"),"Reopened clothing restoration "+row.key);}Advance(34);break;
    case 34:if(SaveState().ticks<=report.firstCloseSaveTicks||!Saved())break;report.secondSaveObserved=true;Need(RowsEqual(CustomRows(),report.savedAnchor.custom),"Second original close saves same final customization");report.moveStart=playerTransform.position;Advance(35);break;
    case 35:Need(InControl(),"Normal input restored");MoveRecord();if(XZ(report.moveStart,playerTransform.position)<.30f){Need(Time.unscaledTime-report.phaseTime<5,"Post-close actual walking bound");keys=Steering(Vector3.zero);break;}Need(report.movement.Any(x=>x.sdk.sqrMagnitude>.5f&&x.pressed),"SDK movement observed");report.movementAccepted=true;report.releaseTime=Time.unscaledTime;report.releasePosition=playerTransform.position;Advance(36);break;
    case 36:MoveRecord();Need(Time.unscaledTime-report.phaseTime<3,"Bounded release tail");if(Time.unscaledTime-report.phaseTime<.3f)break;var tail=report.movement.Where(x=>x.time>=Time.unscaledTime-.25f&&x.time>=report.releaseTime+.15f).ToArray();if(tail.Length<5||tail.Last().time-tail[0].time<.18f)break;Need(tail.All(x=>x.sdk==Vector2.zero&&!x.pressed)&&tail.Max(x=>XZ(tail[0].position,x.position))<.005f&&tail.All(x=>x.step<.003f),"Actual finite stopped tail with SDK release, not just delay");report.releaseAccepted=true;Need(RowsEqual(CustomRows(),report.savedAnchor.custom)&&Observe("after-walk-final",true).modelMatches,"Final saved body/face still applied");Advance(37);break;
    case 37:if(!Shot("home-after-save"))break;report.screenshotEvidenceComplete=report.captures.Count==7&&report.captures.All(x=>x.ready);Need(report.screenshotEvidenceComplete,"All real captures present");report.flowAccepted=true;report.status="edit-save-completed-pending-visual-review";Finish(null);return;
   }
   Need(Time.unscaledTime-report.phaseTime<45,"Bounded phase "+report.phase);Queue(keys);
  }
  static bool DefaultFaceMatches(List<Row> baseline){var expected=new List<Row>();foreach(var row in baseline){string value=row.value;if(row.key=="PCD/CustomizeLoliFace")value="0";if(row.key=="PCD/CustomizeEyeOpenness")value="0";if(row.key=="PCD/CustomizeEyeSize")value="30";if(row.key=="PCD/CustomizeHitomiSize")value="50";if(row.key=="PCD/CustomizeTareme")value="50";if(row.key=="PCD/CustomizeEyeDist")value="50";if(row.key=="PCD/CustomizeEyeHeight")value="100";if(row.key=="PCD/CustomizeEyeDark")value="false";if(row.key=="PCD/CustomizeOddEye")value="false";if(row.key=="PCD/CustomizeMatsugeThick")value="50";if(row.key=="PCD/CustomizeMatsugeLength")value="50";if(row.key=="PCD/CustomizeMayuThick")value="50";if(row.key=="PCD/CustomizeMayuDog")value="0";if(row.key=="PCD/CustomizeCheekFat")value="0";if(row.key=="PCD/CustomizeEarElf")value="0";if(row.key=="PCD/CustomizeChinLength")value="50";if(row.key=="PCD/CustomizeEyeLeftPattern")value="23";if(row.key=="PCD/CustomizeEyePattern")value="23";if(row.key=="PCD/CustomizeEyeBlowColorR")value="183";if(row.key=="PCD/CustomizeEyeBlowColorG")value="163";if(row.key=="PCD/CustomizeEyeBlowColorB")value="163";if(row.key=="PCD/CustomizeEyeLashColorR")value="183";if(row.key=="PCD/CustomizeEyeLashColorG")value="163";if(row.key=="PCD/CustomizeEyeLashColorB")value="163";if(row.key=="PCD/CustomizeYaeba")value="false";expected.Add(new Row{key=row.key,type=row.type,value=value});}return RowsEqual(CustomRows(),expected);}
 }
}
