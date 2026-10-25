using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Master.Common;
using ExposureUnnoticed2.Master.Clothes;
using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Object3D.Player.Scripts.Clothes;
using ExposureUnnoticed2.Object3D.Player.Scripts.Costume;
using ExposureUnnoticed2.Master.Skill;
using ExposureUnnoticed2.Master.Skill.SkillSlow;
using ExposureUnnoticed2.Object3D.Camera;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using ExposureUnnoticed2.ObjectUI.OptionMenu;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using ExposureUnnoticed2.Object3D.IngameManager;

namespace SfmMvp
{
    // OWNED integration, zero recovered methods. Source-only until imported and
    // independently checked in the fresh candidate. Never use in accepted MVP.
    // Original methods own rotation, velocity, smoothing and CC.Move.
    // Fresh frame9 proposal: original main and all eight helpers are untouched.
    // Owned neutral object binding is not original bootstrap recovery. Real
    // remaining stub callees execute as currently compiled; no boundary hooks.
    [DefaultExecutionOrder(-120)]
    public sealed class OriginalMovementMvpBridge : MonoBehaviour
    {
        public CoreGameplayData originalCoreData;
        public bool UsesOriginalConstructors { get; private set; }
        public MvpPlayerMotor legacyMotor; // Disabled; ControlsCaptured only.
        public Camera movementCamera;
        public Animator visibleAnimator;
        public RuntimeAnimatorController ownedMovementController;
        public SkinnedMeshRenderer visibleCoatRenderer;
        public Material recoveredSweatMaterial;
        public string expectedControllerUpdateIL, expectedActionFrameEndIL;
        public string expectedBodyMaterialUpdateIL, expectedClothesUpdateIL;
        public string expectedFootUpdateIL, expectedHandUpdateIL, expectedHeartUpdateIL;
        public string expectedPantsUpdateIL, expectedShioUpdateIL;
        public float FrameHeartRate => heart == null ? 0f : heart.HeartRate;
        public float FrameFootBareyasusa => state == null ? 0f : state.BareyasusaByHeel;
        public float FrameFootRaw => state == null ? 0f : Get<float>(Get<PlayerFootStepController>(pca, "<PlayerFootStepController>k__BackingField"), "originalBareyasusaByHeel");
        public float FrameShioTimer => state == null ? 0f : Get<float>(Get<PlayerShiofukiController>(pca, "<PlayerShiofukiController>k__BackingField"), "syaseiSoundCancelRemainTime");
        public float FramePantsFront => state == null ? 0f : Get<SmoothFloat>(Get<PlayerPantsController>(pca, "<PlayerPantsController>k__BackingField"), "slideMankoBlend").Current;
        public float FramePantsRear => state == null ? 0f : Get<SmoothFloat>(Get<PlayerPantsController>(pca, "<PlayerPantsController>k__BackingField"), "slideAnalBlend").Current;
        public bool FrameQaSeeded { get; private set; }
        public int FrameQaSeedAtFrame { get; private set; }
        public float FrameQaSeedTime { get; private set; }
        public float FramePoseCool => state == null ? 0f : state.PoseActionCoolTime;
        public float FrameSweatValue => bodyMaterial == null ? 0f : Get<SmoothFloat>(bodyMaterial, "sweatLevel").Current;
        public float FrameSweatMaterialValue => sweatMaterial == null ? 0f : sweatMaterial.GetFloat("_Tweak_transparency");
        public Vector3 FrameCachedPosition => state == null ? Vector3.zero : state.CacheForNpcPosition;
        public Vector3 FrameCachedForward => state == null ? Vector3.zero : state.CacheForNpcForward;
        public int FrameCachedExposure => state == null ? 0 : (int)state.CacheForNpcExposedBodyParts;
        public string expectedLocomotionUpdateIL;
        public string expectedLocomotionFixedIL;
        public string expectedControllerFixedIL;
        public string expectedSetStrafeIL;
        public bool allowExplicitFlatGroundFlags;
        public InputActionAsset recoveredActions;
        public bool Grounded => groundChecker != null && groundChecker.IsGrounded;
        public bool FrontGrounded => frontChecker != null && frontChecker.IsGrounded;
        public float StaminaCurrent => stamina == null ? 0f : stamina.Current;
        public int InputUpdates { get; private set; }
        public int GroundCallbacks => groundObserver == null ? 0 : groundObserver.stays;
        public int FrontCallbacks => frontObserver == null ? 0 : frontObserver.stays;
        public Vector3 ResolvedMove { get; private set; }
        DownhillChecker groundChecker, frontChecker;
        MvpGroundObserver groundObserver, frontObserver;
        InputManager originalInput;
        InputResolver originalResolver;
        Keyboard qaKeyboard;
        Mouse qaMouse;
        InGameUiManager inputUi;
        MvpOrbitCamera orbit;
        bool inputQaMode;

        public bool Ready { get; private set; }
        public string Failure { get; private set; }
        public int OriginalUpdateCalls { get; private set; }
        public int OriginalFixedCalls { get; private set; }
        public Vector3 InitialPosition { get; private set; }
        public PlayerController BoundPlayer => player;
        public PlayerLocomotion BoundLocomotion => locomotion;
        public PlayerClassAccessor BoundAccessor => pca;
        public PlayerStateModel BoundState => state;
        public void BindOriginalActionData(MAction master)
        {
            Require(Ready && master != null, "Ready bridge and original MAction master required");
            var assets = AssetReferencer.Instance;
            Require(assets != null && assets.MAction == master, "Original core host already owns the same action master");
            state.CurrentAction = MAction.Get(ActionType.None);
            Require(state.CurrentAction != null && state.CurrentAction.Type == ActionType.None, "Native master None record");
            pca.PlayerMiscProcess.Initialize(pca);
            var receiver = pca.PlayerAnimationReceiver;
            Require(receiver != null && receiver.gameObject == visibleAnimator.gameObject, "Original PCA selected the real animation receiver");
            Set(pca, "<PlayerAnimationReceiver>k__BackingField", receiver);
            receiver.Initialize(pca);
            state.TransAction = ActionType.None;
            pca.PlayerAction.ChangeAction(ActionType.None);
        }

        public float OriginalMoveSpeed => Ready ? locomotion.CurrentMoveSpeed : 0f;
        public Vector3 OriginalMoveVector => state == null ? Vector3.zero : state.MoveV;
        public float AnimationTargetSpeed => animation == null ? 0f : animation.TargetMoveSpeed;
        public string MethodPinSource { get; private set; }
        public string MethodPinFileSha256 { get; private set; }

        [Serializable] sealed class OwnedMethodPins
        {
            public string format;
            public int candidate_checkpoint, frame_method_count;
            public string controller_update_il_sha256, action_frame_end_il_sha256;
            public string body_material_update_il_sha256, clothes_update_il_sha256;
            public string foot_update_il_sha256, hand_update_il_sha256, heart_update_il_sha256;
            public string pants_update_il_sha256, shio_update_il_sha256;
            public string locomotion_update_il_sha256, locomotion_fixed_il_sha256, controller_fixed_il_sha256, set_strafe_il_sha256;
        }

        static readonly BindingFlags Fields = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        sealed class StaticLease
        {
            public FieldInfo field;
            public object before, owned;
        }
        readonly List<StaticLease> leases = new List<StaticLease>();
        readonly List<UnityEngine.Object> ownedObjects = new List<UnityEngine.Object>();
        PlayerController player;
        PlayerClassAccessor pca;
        PlayerLocomotion locomotion;
        PlayerAnimationManager animation;
        PlayerStateModel state;
        StaminaManager stamina;
        PlayerHeartBeat heart;
        PlayerBodyMaterialController bodyMaterial;
        Material sweatMaterial;
        PlayerFacade facade;
        CoatController coat;
        NpcManager neutralNpcManager;
        object previousInputResolver;
        bool resolverBound;
        InGameManager manager;
        GameObject managerObject;
        BaseCameraController baseCamera;
        TPCameraController tpCamera;
        FPCameraController fpCamera;
        Action ownedCameraCallback;
        SmoothFloat strafeX, strafeY;
        bool initializedLocomotion, cleaned, qaInput;
        Vector3 qaMove;
        object previousPca;
        bool pcaBound;

        static FieldInfo Field(Type type, string name)
        {
            var field = type.GetField(name, Fields);
            if (field == null) throw new MissingFieldException(type.FullName, name);
            return field;
        }
        static void Set(object receiver, string name, object value) => Field(receiver.GetType(), name).SetValue(receiver, value);
        static T Get<T>(object receiver, string name) => (T)Field(receiver.GetType(), name).GetValue(receiver);
        static T Empty<T>() => (T)FormatterServices.GetUninitializedObject(typeof(T));
        void OwnStatic(Type type, string name, object value)
        {
            var field = Field(type, name);
            var lease = new StaticLease { field = field, before = field.GetValue(null), owned = value };
            leases.Add(lease);
            field.SetValue(null, value);
        }
        T Own<T>(T value) where T : UnityEngine.Object { ownedObjects.Add(value); return value; }
        static void Require(bool condition, string message)
        {
            if (!condition) throw new InvalidOperationException(message);
        }
        static string Hash(byte[] bytes)
        {
            using (var sha = SHA256.Create()) return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "");
        }
        static void Pin(Type type, string methodName, Type[] arguments, string expected)
        {
            Require(!string.IsNullOrEmpty(expected) && expected.Length == 64, "Scene builder must pin imported original IL: " + methodName);
            var method = type.GetMethod(methodName, Fields, null, arguments, null);
            Require(method != null && method.GetMethodBody() != null, "Original method body missing: " + methodName);
            Require(Hash(method.GetMethodBody().GetILAsByteArray()).Equals(expected, StringComparison.OrdinalIgnoreCase), "Original imported method IL differs: " + methodName);
        }
        static void RequireParameter(Animator animator, string name, AnimatorControllerParameterType type)
        {
            foreach (var parameter in animator.parameters)
                if (parameter.name == name && parameter.type == type) return;
            throw new InvalidOperationException("Owned visible controller parameter missing: " + name);
        }
        void LoadOwnedMethodPins()
        {
#if UNITY_EDITOR
            MethodPinSource = "Serialized values independently measured in this owned Editor candidate";
            MethodPinFileSha256 = "";
#else
            // Public build evidence only. Never resolve a configurable credential,
            // save, Resources payload or arbitrary file path from this bridge.
            const string stage = @"E:\ACTgame\SFM\ReverseEngineering\tools\unity-editor\equipment-transaction-runtime-candidate-v5";
            string root = Path.GetFullPath(stage).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            string dataPath = Path.GetFullPath(Application.dataPath);
            Require(dataPath.StartsWith(root, StringComparison.OrdinalIgnoreCase), "The Player must reside inside the new owned candidate stage.");
            string path = Path.Combine(dataPath, "owned-player-frame-method-pins.json");
            Require(File.Exists(path) && new FileInfo(path).Length <= 32768, "Public Player IL pin file is missing or invalid.");
            byte[] bytes = File.ReadAllBytes(path);
            var pins = JsonUtility.FromJson<OwnedMethodPins>(System.Text.Encoding.UTF8.GetString(bytes).TrimStart('\uFEFF'));
            Require(pins != null && pins.format == "OwnedPlayerFrameMethodPins/v1" && pins.candidate_checkpoint == 1489 && pins.frame_method_count == 9, "Public Player IL pin schema/checkpoint mismatch.");
            expectedLocomotionUpdateIL = pins.locomotion_update_il_sha256;
            expectedLocomotionFixedIL = pins.locomotion_fixed_il_sha256;
            expectedControllerFixedIL = pins.controller_fixed_il_sha256;
            expectedSetStrafeIL = pins.set_strafe_il_sha256;
            expectedControllerUpdateIL = pins.controller_update_il_sha256;
            expectedActionFrameEndIL = pins.action_frame_end_il_sha256;
            expectedBodyMaterialUpdateIL = pins.body_material_update_il_sha256;
            expectedClothesUpdateIL = pins.clothes_update_il_sha256;
            expectedFootUpdateIL = pins.foot_update_il_sha256;
            expectedHandUpdateIL = pins.hand_update_il_sha256;
            expectedHeartUpdateIL = pins.heart_update_il_sha256;
            expectedPantsUpdateIL = pins.pants_update_il_sha256;
            expectedShioUpdateIL = pins.shio_update_il_sha256;
            MethodPinSource = path;
            MethodPinFileSha256 = Hash(bytes);
#endif
        }

        void Awake()
        {
            try { Setup(); }
            catch (Exception exception) { Fail(exception); }
        }
        void Setup()
        {
            Require(Application.companyName == "SFMRecovery" && Application.productName == "SFMEquipmentTransactionRuntimeCandidateV5", "Candidate PlayerSettings must isolate its persistent data path.");
            Require(!allowExplicitFlatGroundFlags, "Real collider sensors required.");
            var arguments = Environment.GetCommandLineArgs();
            Require(Array.IndexOf(arguments, "-sfmMvpSmoke") < 0 && Array.IndexOf(arguments, "-sfmMvpOriginalBundle") < 0 && Array.IndexOf(arguments, "-sfmInputGroundSmoke") < 0 && Array.IndexOf(arguments, "-sfmOriginalMovementSmoke") < 0 && Array.IndexOf(arguments, "-sfmSmoke") < 0, "Legacy smoke/bundle entry can call temporary movement. Use the owned original-movement QA entry.");
            Require(legacyMotor != null && legacyMotor.gameObject == gameObject && !legacyMotor.enabled, "Owned scene must retain a disabled legacy motor solely for orbit ControlsCaptured.");
            legacyMotor.SmokeDriven = true;
            Require(movementCamera != null && visibleAnimator != null && visibleAnimator.transform.IsChildOf(transform), "Bind the real view and original character Animator.");
            Require(ownedMovementController != null && visibleAnimator.runtimeAnimatorController == ownedMovementController, "Owned idle/walk controller must be installed before play; legacy PlayableMixer must be removed.");
            Require(visibleAnimator.avatar != null && visibleAnimator.avatar.isValid && visibleAnimator.avatar.isHuman, "Original valid humanoid Avatar is required.");
            RequireParameter(visibleAnimator, "IsStrafe", AnimatorControllerParameterType.Bool);
            RequireParameter(visibleAnimator, "Speed", AnimatorControllerParameterType.Float);
            RequireParameter(visibleAnimator, "StrafeX", AnimatorControllerParameterType.Float);
            RequireParameter(visibleAnimator, "StrafeY", AnimatorControllerParameterType.Float);
            LoadOwnedMethodPins();
            Pin(typeof(PlayerLocomotion), "OnUpdate", new[] { typeof(InputData) }, expectedLocomotionUpdateIL);
            Pin(typeof(PlayerLocomotion), "OnFixedUpdate", Type.EmptyTypes, expectedLocomotionFixedIL);
            Pin(typeof(PlayerController), "OnFixedUpdate", Type.EmptyTypes, expectedControllerFixedIL);
            Pin(typeof(PlayerAnimationManager), "SetStrafe", new[] { typeof(bool), typeof(bool), typeof(float), typeof(float) }, expectedSetStrafeIL);
            PinFrameMethods();
            Require(UnityEngine.Object.FindObjectsOfType<InGameManager>().Length == 0, "The owned scene must not contain an active original manager/bootstrap.");

            var controller = GetComponent<CharacterController>();
            Require(controller != null && controller.enabled, "Use the existing real CharacterController and native scene colliders.");
            var agent = GetComponent<NavMeshAgent>();
            Require(agent != null && !agent.enabled, "Owned builder must serialize a disabled real NavMeshAgent: Apartment path following is not tested.");
            player = GetComponent<PlayerController>();
            if (player == null) player = Own(gameObject.AddComponent<PlayerController>());
            player.enabled = false; // Original IUpdater dispatch is not the MonoBehaviour schedule here.
            OwnStatic(typeof(PlayerController), "Instance", player);
            var objects = gameObject.GetComponent<PlayerObjectReferencer>();
            if (objects == null) objects = Own(gameObject.AddComponent<PlayerObjectReferencer>());
            Require(gameObject.GetComponents<PlayerObjectReferencer>().Length == 1, "Unique real PlayerObjectReferencer");
            objects.enabled = false;
            int playerLayer = LayerMask.NameToLayer("Player");
            Require(playerLayer >= 0, "Original Player layer missing");
            gameObject.layer = playerLayer;
            groundChecker = CreateGroundSensor("Owned foot sensor", 0f, controller, out groundObserver);
            frontChecker = CreateGroundSensor("Owned forward sensor", .45f, controller, out frontObserver);
            objects.OnGroundedChecker = groundChecker;
            objects.DownhillChecker = frontChecker;

            var action = Own(ScriptableObject.CreateInstance<MActionRecord>());
            Set(action, "type", (ActionType)(-1));
            Set(action, "canMove", true);
            Require(originalCoreData != null, "Original core master bindings required");
            var coreAssets = originalCoreData.CreateMasterBindings();
            OwnStatic(typeof(AssetReferencer), "Instance", coreAssets);
            OwnStatic(typeof(MCommon), "instance", originalCoreData.Common);
            var custom = new PlayerCustomData();
            OwnStatic(typeof(GameState), "PlayerCustomData", custom);
            GameStateFacade.SetDefaultCustomBody();
            GameStateFacade.SetDefaultCustomFace();
            var data = new GameStateData();
            OwnStatic(typeof(GameState), "GameStateData", data);
            var option = new OptionData();
            option.ResetControl();
            option.MouseSensitive100 = 20;
            option.PadSensitive100 = 30;
            option.Language = Localizer.Language.Sc;
            OwnStatic(typeof(GameState), "OptionData", option);

            tpCamera = Own(movementCamera.gameObject.AddComponent<TPCameraController>());
            fpCamera = Own(movementCamera.gameObject.AddComponent<FPCameraController>());
            baseCamera = Own(movementCamera.gameObject.AddComponent<BaseCameraController>());
            tpCamera.enabled = false;
            fpCamera.enabled = false;
            baseCamera.enabled = false;
            managerObject = Own(new GameObject("Owned inactive InGameManager - never Start or Save"));
            managerObject.SetActive(false); // Before AddComponent: even a future Awake cannot launch on an active object.
            manager = managerObject.AddComponent<InGameManager>();
            manager.enabled = false;
            OwnStatic(typeof(InGameManager), "Instance", manager);
            Set(manager, "<TempInGameState>k__BackingField", new TemporaryInGameState());
            var referencer = managerObject.GetComponent<InGameReferencer>();
            Set(referencer, "<BaseCameraController>k__BackingField", baseCamera);
            Set(manager, "<InGameReferencer>k__BackingField", referencer);

            var avatars = GetComponentsInChildren<PlayerAvatarObjectReferencer>(true);
            Require(avatars.Length <= 1, "Unique real Avatar referencer");
            if (avatars.Length == 0) Own(visibleAnimator.gameObject.AddComponent<PlayerAvatarObjectReferencer>());
            if (visibleAnimator.GetComponent<PlayerAnimationParameter>() == null) Own(visibleAnimator.gameObject.AddComponent<PlayerAnimationParameter>());
            if (visibleAnimator.GetComponent<PlayerAnimationReceiver>() == null) Own(visibleAnimator.gameObject.AddComponent<PlayerAnimationReceiver>());
            if (GetComponent<AudioSource>() == null) Own(gameObject.AddComponent<AudioSource>());
            if (GetComponent<Rigidbody>() == null) { var rb = Own(gameObject.AddComponent<Rigidbody>()); rb.isKinematic = true; rb.useGravity = false; }
            if (GetComponent<CapsuleCollider>() == null) { var capsule = Own(gameObject.AddComponent<CapsuleCollider>()); capsule.isTrigger = true; capsule.enabled = false; }
            pca = new PlayerClassAccessor(player, null);
            state = pca.PlayerState;
            stamina = pca.Stamina;
            Require(pca.Animator == visibleAnimator && pca.CharacterController == controller && pca.PlayerObjectReferencer == objects && pca.PlayerAvatarObjectReferencer != null, "Original PCA queries the actual scene components");
            Require(state.CosplayBuffList != null && state.CostumeBuffList != null && GameState.PlayerCustomData.CustomizeHeight == 160 && data.SkillLevel.Count > 3, "Original state/custom/default constructors and real master tables");
            UsesOriginalConstructors = true;
            // Explicit0count neutral data providers; no broad body/save/bootstrap initializer.
            var clock = new TimeScaleManager();
            OwnStatic(typeof(TimeScaleManager), "Instance", clock);
            clock.Initialize();
            var bodyInput = pca.PlayerBodyCustomizeAdjuster;
            Set(bodyInput, "pca", pca);
            Set(bodyInput, "<SmallBreastSize>k__BackingField", 100);
            Set(bodyInput, "<BigBreastSize>k__BackingField", 0);
            Set(pca, "<PlayerBodyCustomizeAdjuster>k__BackingField", bodyInput);
            animation = pca.PlayerAnimationManager; // Constructed by original PCA.
            Set(pca, "<PlayerAnimationManager>k__BackingField", animation);
            animation.Initialize(pca); // Real recovered small initializer.
            strafeX = Get<SmoothFloat>(animation, "strafeX");
            strafeY = Get<SmoothFloat>(animation, "strafeY");
            locomotion = pca.PlayerLocomotion; // Constructed by original PCA.
            Set(pca, "<PlayerLocomotion>k__BackingField", locomotion);
            previousPca = Get<object>(player, "pca");
            Set(player, "pca", pca);
            pcaBound = true;
            // Captures only this original callback, so cleanup leaves any other
            // camera subscribers intact. Dispose handles the original event pair.
            ownedCameraCallback = (Action)Delegate.CreateDelegate(typeof(Action), locomotion,
                typeof(PlayerLocomotion).GetMethod("OnChangeFirstPersonView", Fields));
            initializedLocomotion = true;
            locomotion.Initialize(tpCamera, fpCamera, baseCamera, pca);
            var fog = new VisibilityFogController();
            var oldFog = Field(typeof(VisibilityFogController), "<Instance>k__BackingField").GetValue(null);
            fog.Initialize();
            leases.Add(new StaticLease { field = Field(typeof(VisibilityFogController), "<Instance>k__BackingField"), before = oldFog, owned = fog });
            visibleAnimator.applyRootMotion = false;
            visibleAnimator.fireEvents = false;
            visibleAnimator.enabled = true;
            Physics.SyncTransforms();
            SetupOriginalInput();
            SetupNeutralFrameGraph(referencer);
            previousInputResolver = Get<object>(player, "inputResolver");
            Set(player, "inputResolver", originalResolver);
            resolverBound = true;
            InitialPosition = transform.position;
            Ready = true;
            MvpBootstrap.Log("original_player_frame_bridge_ready", "Original complete main dispatcher plus eight helpers; real input, ground and CC movement. Owned neutral graph, remaining real stub callees, diagnostic sweat material and temporary animation/orbit adapters.");
        }

        void PinFrameMethods()
        {
            Pin(typeof(PlayerController), "OnUpdate", Type.EmptyTypes, expectedControllerUpdateIL);
            Pin(typeof(PlayerAction), "OnFrameEnd", Type.EmptyTypes, expectedActionFrameEndIL);
            Pin(typeof(PlayerBodyMaterialController), "OnUpdate", Type.EmptyTypes, expectedBodyMaterialUpdateIL);
            Pin(typeof(PlayerClothes), "OnUpdate", Type.EmptyTypes, expectedClothesUpdateIL);
            Pin(typeof(PlayerFootStepController), "OnUpdate", Type.EmptyTypes, expectedFootUpdateIL);
            Pin(typeof(PlayerHandcuffsController), "OnUpdate", new[] { typeof(InputData) }, expectedHandUpdateIL);
            Pin(typeof(PlayerHeartBeat), "OnUpdate", Type.EmptyTypes, expectedHeartUpdateIL);
            Pin(typeof(PlayerPantsController), "OnUpdate", Type.EmptyTypes, expectedPantsUpdateIL);
            Pin(typeof(PlayerShiofukiController), "OnUpdate", Type.EmptyTypes, expectedShioUpdateIL);
        }
        public void VerifyOriginalMethodPins()
        {
            PinFrameMethods();
            Pin(typeof(PlayerLocomotion), "OnUpdate", new[] { typeof(InputData) }, expectedLocomotionUpdateIL);
            Pin(typeof(PlayerLocomotion), "OnFixedUpdate", Type.EmptyTypes, expectedLocomotionFixedIL);
            Pin(typeof(PlayerController), "OnFixedUpdate", Type.EmptyTypes, expectedControllerFixedIL);
            Pin(typeof(PlayerAnimationManager), "SetStrafe", new[] { typeof(bool), typeof(bool), typeof(float), typeof(float) }, expectedSetStrafeIL);
        }
        void BindNeutral(string property, object instance)
        {
            Set(pca, "<" + property + ">k__BackingField", instance);
            // Only bind a declared private receiver, not broad original Init.
            var receiver = instance.GetType().GetField("pca", Fields);
            if (receiver != null) receiver.SetValue(instance, pca);
        }
        void SetupNeutralFrameGraph(InGameReferencer referencer)
        {
            Require(visibleCoatRenderer != null && visibleCoatRenderer.transform.IsChildOf(transform) && visibleCoatRenderer.enabled,
                "Builder must bind the visible native coat renderer");
            Require(recoveredSweatMaterial != null && recoveredSweatMaterial.HasProperty("_Tweak_transparency"),
                "Bind native readable SweatBody material with its real shader property");
            // Existing manager host is already inactive before AddComponent.
            neutralNpcManager = managerObject.AddComponent<NpcManager>();
            neutralNpcManager.enabled = false;
            Set(referencer, "<NpcManager>k__BackingField", neutralNpcManager);
            // GetNearNpcDistance currently returns default; its actual body is
            // allowed to run unchanged and is explicitly not NPC restoration.
            BindNeutral("MoistureManager", pca.MoistureManager);
            BindNeutral("PlayerEcstasyController", pca.PlayerEcstasyController);
            BindNeutral("PlayerStrangenessController", pca.PlayerStrangenessController);
            BindNeutral("PlayerMiscProcess", pca.PlayerMiscProcess);
            BindNeutral("PlayerPeeController", pca.PlayerPeeController);
            BindNeutral("PlayerClothesAction", pca.PlayerClothesAction);
            BindNeutral("PlayerHandcuffsController", pca.PlayerHandcuffsController);
            BindNeutral("PlayerAdultGoodsController", pca.PlayerAdultGoodsController);
            BindNeutral("PlayerCostumeController", pca.PlayerCostumeController);
            BindNeutral("PlayerCosplayManager", pca.PlayerCosplayManager);
            var actionController = pca.PlayerAction;
            BindNeutral("PlayerAction", actionController);
            actionController.Initialize(pca); // Actual small receiver bindings.
            BindNeutral("PlayerReinforceEffectManager", pca.PlayerReinforceEffectManager);
            var clothes = pca.PlayerClothes; // Original PCA constructor initialized its lists.
            BindNeutral("PlayerClothes", clothes);
            Require(visibleCoatRenderer.GetComponent<CoatController>() == null, "Fresh coat must have no competing original controller");
            coat = Own(visibleCoatRenderer.gameObject.AddComponent<CoatController>());
            coat.enabled = false;
            coat.Initialize(pca); // Actual pca + renderer binding only.
            Set(clothes, "clothes", new List<IClothesController> { coat });
            var clothesRecord = MClothes.Get(ClothesType.Coat);
            Require(clothesRecord != null, "Original coat record required");
            state.CurrentClothes = clothesRecord;
            var data = GameState.GameStateData;
            data.PlayerCurrentClothes = ClothesType.Coat;
            data.PlayerCurrentClothesAState = PlayerStateModel.ClothesState.CloseA;
            data.PlayerCurrentClothesBState = PlayerStateModel.ClothesState.CloseB;
            data.PlayerIsAttachHandcuffs = false;
            data.PlayerIsAttachMapHandcuffs = false;
            data.CurrentStageType = (StageType)0;
            state.HiddenBodyPartsByCostume = (PlayerStateModel.HiddenBodyPartsByCostumeType)0;
            state.ReinforceEffectData = default(SumReinforceEffectData); // Real value type, explicit owned neutral values.
            heart = pca.PlayerHeartBeat; // Original PCA constructor starts at95.
            Set(pca, "<PlayerHeartBeat>k__BackingField", heart);
            heart.Initialize(pca);
            BindNeutral("PlayerFaceController", pca.PlayerFaceController);
            BindNeutral("PlayerTitController", pca.PlayerTitController);
            BindNeutral("PlayerFaceShadowOnlyController", pca.PlayerFaceShadowOnlyController);
            var pants = pca.PlayerPantsController;
            BindNeutral("PlayerPantsController", pants); pants.Initialize(pca);
            var foot = pca.PlayerFootStepController;
            BindNeutral("PlayerFootStepController", foot); foot.Initialize(pca);
            BindNeutral("PlayerShrinkWakiController", pca.PlayerShrinkWakiController);
            BindNeutral("PlayerBreathController", pca.PlayerBreathController);
            bodyMaterial = pca.PlayerBodyMaterialController;
            BindNeutral("PlayerBodyMaterialController", bodyMaterial);
            sweatMaterial = Own(new Material(recoveredSweatMaterial));
            sweatMaterial.name = "Owned frame sweat diagnostic material";
            // This real material is diagnostic, not a claim of whole body paint
            // setup or visible sweat rendering. Preserve current clothed mesh.
            Set(bodyMaterial, "<SweatBodyMaterial>k__BackingField", sweatMaterial);
            BindNeutral("FutanariController", pca.FutanariController);
            BindNeutral("PlayerShiofukiController", pca.PlayerShiofukiController);
            BindNeutral("BodyPaintController", pca.BodyPaintController);
            BindNeutral("PistonMachineController", pca.PistonMachineController);
            BindNeutral("PlayerMoveCalculator", pca.PlayerMoveCalculator);
            BindNeutral("PlayerRpBonusCalculator", pca.PlayerRpBonusCalculator);
            BindNeutral("PlayerAvatarOffsetManager", pca.PlayerAvatarOffsetManager);
            var sound = pca.PlayerSoundManager;
            BindNeutral("PlayerSoundManager", sound);
            // Not reached by the guarded no-game-over/no-handcuffs phase.
            // Do not call broad sound/avatar/resource bootstrap.
            facade = pca.PlayerFacade;
            var previousFacade = Field(typeof(PlayerFacade), "Instance").GetValue(null);
            facade.Initialize(pca); // Actual recovered Instance/pca binding.
            leases.Add(new StaticLease { field = Field(typeof(PlayerFacade), "Instance"), before = previousFacade, owned = facade });
        }

        DownhillChecker CreateGroundSensor(string name, float forward, CharacterController cc, out MvpGroundObserver observer)
        {
            var sensor = Own(new GameObject(name));
            sensor.transform.SetParent(transform, false);
            float foot = cc.center.y - cc.height * .5f;
            sensor.transform.localPosition = new Vector3(0f, foot + .06f, forward);
            sensor.layer = 0;
            var sphere = sensor.AddComponent<SphereCollider>(); sphere.radius = .16f; sphere.isTrigger = true;
            var rb = sensor.AddComponent<Rigidbody>(); rb.isKinematic = true; rb.useGravity = false; rb.sleepThreshold = 0f;
            observer = sensor.AddComponent<MvpGroundObserver>();
            Physics.IgnoreLayerCollision(0, 0, false);
            return sensor.AddComponent<DownhillChecker>();
        }
        void SetupOriginalInput()
        {
            Require(recoveredActions != null, "Original readable input actions missing");
            orbit = movementCamera.GetComponent<MvpOrbitCamera>();
            inputUi = managerObject.AddComponent<InGameUiManager>(); inputUi.enabled = false;
            OwnStatic(typeof(InGameUiManager), "Instance", inputUi);
            new SexManager().Initialize();
            InputSystem.settings.updateMode = InputSettings.UpdateMode.ProcessEventsInDynamicUpdate;
            InputSystem.settings.backgroundBehavior = InputSettings.BackgroundBehavior.IgnoreFocus;
#if UNITY_EDITOR
            InputSystem.settings.editorInputBehaviorInPlayMode = InputSettings.EditorInputBehaviorInPlayMode.AllDeviceInputAlwaysGoesToGameView; // Owned batch PlayMode QA only.
#endif
            inputQaMode = Array.IndexOf(Environment.GetCommandLineArgs(), "-sfmPamEngineSmoke") >= 0 || Array.IndexOf(Environment.GetCommandLineArgs(), "-sfmPamPlayerSmoke") >= 0 || Array.IndexOf(Environment.GetCommandLineArgs(), "-sfmOriginalActionSmoke") >= 0;
            Keyboard keyboard = Keyboard.current; Mouse mouse = Mouse.current;
            if (inputQaMode) { qaKeyboard = InputSystem.AddDevice<Keyboard>("OwnedMvpQaKeyboard"); qaMouse = InputSystem.AddDevice<Mouse>("OwnedMvpQaMouse"); keyboard = qaKeyboard; mouse = qaMouse; }
            Require(keyboard != null && mouse != null, "Keyboard and mouse are required");
            var host = Own(new GameObject("Original input host")); host.SetActive(false);
            var input = host.AddComponent<PlayerInput>(); input.actions = Own(Instantiate(recoveredActions));
            input.defaultActionMap = "Player"; input.defaultControlScheme = "Keyboard and Mouse";
            input.notificationBehavior = PlayerNotifications.InvokeCSharpEvents; input.neverAutoSwitchControlSchemes = true;
            host.SetActive(true); input.SwitchCurrentControlScheme("Keyboard and Mouse", keyboard, mouse);
            originalInput = host.AddComponent<InputManager>();
            foreach(var action in input.currentActionMap.actions)
                if(action.name != "Move" && action.name != "CameraMove") action.Disable();
            originalResolver = new InputResolver();
        }
        // A single owned fixture seed before observing real main frames.
        // No per-frame repairs or target/body hooks are permitted.
        public void SeedOwnedFrameQaOnce()
        {
            Require(Ready && inputQaMode && !FrameQaSeeded, "One fresh owned QA seed only");
            state.PoseActionCoolTime = 4f;
            var foot = Get<PlayerFootStepController>(pca, "<PlayerFootStepController>k__BackingField");
            Set(foot, "originalBareyasusaByHeel", .6f);
            Set(foot, "startBareyasusa", .8f);
            Set(Get<PlayerShiofukiController>(pca, "<PlayerShiofukiController>k__BackingField"), "syaseiSoundCancelRemainTime", 2f);
            state.Dokidoki = .25f;
            GameState.GameStateData.CurrentStageType = (StageType)1;
            FrameQaSeedAtFrame = Time.frameCount;
            FrameQaSeedTime = Time.time;
            FrameQaSeeded = true;
        }
        public void SetOwnedQaInput(Vector2 value)
        {
            Require(inputQaMode && qaKeyboard != null, "QA device required");
            var keys = new List<Key>();
            if(value.x > .5f) keys.Add(Key.D); if(value.x < -.5f) keys.Add(Key.A);
            if(value.y > .5f) keys.Add(Key.W); if(value.y < -.5f) keys.Add(Key.S);
            InputSystem.QueueStateEvent(qaKeyboard, new KeyboardState(keys.ToArray()));
            InputSystem.QueueStateEvent(qaMouse, new MouseState());
        }
        public void ReleaseOwnedQaInput() { if(inputQaMode) SetOwnedQaInput(Vector2.zero); }
        void CheckFrameContract()
        {
            Require(!legacyMotor.enabled && legacyMotor.SmokeDriven, "Legacy motor was reactivated.");
            Require(visibleAnimator.runtimeAnimatorController == ownedMovementController && !visibleAnimator.applyRootMotion, "A competing animator graph/root-motion writer replaced owned controller.");
            Require(!managerObject.activeInHierarchy && !manager.enabled, "Original InGameManager must stay inactive; Start/save bootstrap is prohibited.");
            Require(ReferenceEquals(PlayerController.Instance, player) && ReferenceEquals(InGameManager.Instance, manager), "Owned singleton binding changed.");
            Require(!state.IsAutoRun && !state.IsAutoChase && !state.IsStrafe && !state.IsCrouch && state.NearNpc == null, "Owned adapter permits normal third-person walking only.");
            Require(!manager.TempInGameState.IsGameOver && state.CurrentState == PlayerStateModel.PlayerState.Idle, "Neutral main path must remain Idle/non-game-over.");
            Require(ReferenceEquals(Get<object>(player, "inputResolver"), originalResolver) && ReferenceEquals(PlayerFacade.Instance, facade), "Original resolver/facade binding changed.");
            Require(neutralNpcManager != null && !neutralNpcManager.enabled && !neutralNpcManager.gameObject.activeInHierarchy, "Neutral NPC host must remain inactive; NPC search is a declared remaining stub.");
            Require(coat != null && !coat.enabled && state.CurrentClothes != null && state.CurrentClothes.ClothesType == ClothesType.Coat, "Clothed neutral interface selection changed.");
            Require(GameState.GameStateData.PlayerCurrentClothesAState == PlayerStateModel.ClothesState.CloseA && GameState.GameStateData.PlayerCurrentClothesBState == PlayerStateModel.ClothesState.CloseB, "Owned clothed neutral state changed.");
        }
        void Update()
        {
            if (!Ready) return;
            try
            {
                CheckFrameContract();
                inputUi.IsOpenFade = !inputQaMode && !legacyMotor.ControlsCaptured;
                originalInput.OnUpdate(); InputUpdates++;
                // The actual main owns Resolve, Stamina, Locomotion and all eight
                // frame helpers. Never resolve or tick them a second time here.
                player.OnUpdate();
                InputData resolved = Get<InputData>(originalResolver, "inputData");
                Require(resolved != null, "Original resolver retained result missing");
                ResolvedMove = resolved.Move; // Post-main observation, no mutation.
                CheckFrameContract();
                if(legacyMotor.ControlsCaptured) orbit.Look(resolved.CameraMove * .08f);
                OriginalUpdateCalls++;
                // Real main already dispatched PAM exactly once; readback only in owned QA.
            }
            catch (Exception exception) { Fail(exception); }
        }
        void FixedUpdate()
        {
            if (!Ready) return;
            try { CheckFrameContract(); player.OnFixedUpdate(); OriginalFixedCalls++; }
            catch (Exception exception) { Fail(exception); }
        }
        void Fail(Exception exception)
        {
            Failure = exception.ToString();
            Ready = false;
            Debug.LogError("SFM_OWNED_ORIGINAL_MOVEMENT " + Failure);
            enabled = false;
            Cleanup();
        }
        void OnDisable() { Cleanup(); }
        void OnDestroy() { Cleanup(); }
        void Cleanup()
        {
            if (cleaned) return;
            cleaned = true;
            Ready = false;
            if (state != null) { state.MoveV = Vector3.zero; state.SumFixedPushVThisFrame = Vector3.zero; }
            if (initializedLocomotion)
            {
                if (baseCamera != null && ownedCameraCallback != null) baseCamera.OnChangeFirstPersonView -= ownedCameraCallback;
                locomotion.Dispose(); // Real event removal only; no broad Player/PCA Dispose.
            }
            for (int i = leases.Count - 1; i >= 0; i--)
            {
                var lease = leases[i];
                if (ReferenceEquals(lease.field.GetValue(null), lease.owned)) lease.field.SetValue(null, lease.before);
            }
            if (resolverBound && player != null && ReferenceEquals(Get<object>(player, "inputResolver"), originalResolver)) Set(player, "inputResolver", previousInputResolver);
            if (pcaBound && player != null && ReferenceEquals(Get<object>(player, "pca"), pca)) Set(player, "pca", previousPca);
            // Destroy only integration-owned objects/components. Scene assets,
            // accepted bridge sources, saves and user settings are untouched.
            for (int i = ownedObjects.Count - 1; i >= 0; i--)
                if (ownedObjects[i] != null) UnityEngine.Object.Destroy(ownedObjects[i]);
        }
    }
}
