#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Master.Common;
using ExposureUnnoticed2.Object3D.Portal;
using ExposureUnnoticed2.Object3D.ScenePlops.Elevator;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;

namespace SfmMvp.Editor
{
    // Isolated engine fixtures. These do not certify the original game failure loop.
    [InitializeOnLoad]
    public static class PlayerCollisionAnimationTests
    {
        const string PendingKey = "SfmPlayerCollisionAnimationV1.Pending";
        const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        [Serializable] sealed class Check { public string name; public bool passed; public string detail; }
        [Serializable] sealed class Report
        {
            public string schema = "SfmPlayerCollisionAnimationEngineFixtures/v1";
            public string unity, assembly, error;
            public bool passed, playMode, actualGameLoopAccepted = false;
            public List<Check> checks = new List<Check>();
            public List<string> engineErrors = new List<string>();
        }
        static Report report;
        static PlayerCollisionAnimationTests()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (state != PlayModeStateChange.EnteredPlayMode || !SessionState.GetBool(PendingKey, false)) return;
                EditorApplication.delayCall += () =>
                {
                    int exitCode = 0;
                    try { Run(); } catch (Exception error) { Debug.LogError(error); exitCode = 1; }
                    finally { SessionState.EraseBool(PendingKey); EditorApplication.Exit(exitCode); }
                };
            };
        }
        public static void RunPlay()
        {
            if (EditorApplication.isPlaying) throw new InvalidOperationException("Require a fresh Editor process");
            if (Array.IndexOf(Environment.GetCommandLineArgs(), "-sfmNpcAnimationProbeV1") < 0)
                throw new InvalidOperationException("Require the existing explicit empty-fixture bootstrap gate");
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SessionState.SetBool(PendingKey, true);
            EditorApplication.isPlaying = true;
        }
        static readonly List<GameObject> objects = new List<GameObject>();
        static string controllerPath;
        static void OnLog(string message, string stack, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
                report.engineErrors.Add(message);
        }
        static void Verify(string name, bool ok, string detail)
        {
            report.checks.Add(new Check { name = name, passed = ok, detail = detail });
            if (!ok) throw new InvalidOperationException(name + ": " + detail);
        }
        static void Set(object target, string name, object value)
        {
            var field = target.GetType().GetField(name, Private);
            if (field == null) throw new MissingFieldException(target.GetType().FullName, name);
            field.SetValue(target, value);
        }
        static GameObject Object(string name, Vector3 position)
        {
            var go = new GameObject(name); go.transform.position = position; objects.Add(go); return go;
        }
        static void ClearObjects()
        {
            foreach (var go in objects) if (go) UnityEngine.Object.DestroyImmediate(go);
            objects.Clear(); Physics.SyncTransforms();
        }
        static void CollisionCase(string name, bool solidX, bool solidZ, bool trigger, bool excluded, bool rotated)
        {
            ClearObjects();
            var actor = Object("fixture actor", new Vector3(0f, 50f, 0f));
            var capsule = actor.AddComponent<CapsuleCollider>();
            capsule.radius = 0.5f; capsule.height = 2f; capsule.center = Vector3.up;
            var accessor = (PlayerClassAccessor)FormatterServices.GetUninitializedObject(typeof(PlayerClassAccessor));
            Set(accessor, "<Transform>k__BackingField", actor.transform);
            Set(accessor, "<CapsuleCollider>k__BackingField", capsule);
            var correcter = new PlayerCollisionCorrecter(); correcter.Initialize(accessor);
            var solids = new List<Collider>();
            if (solidX || trigger || excluded)
            {
                var wall = Object("fixture x wall", new Vector3(0.7f, 51f, 0f));
                wall.layer = excluded ? LayerMask.NameToLayer("Ignore Raycast") : 0;
                if (rotated) wall.transform.rotation = Quaternion.Euler(0f, 20f, 0f);
                var box = wall.AddComponent<BoxCollider>(); box.size = new Vector3(0.6f, 2f, 3f);
                box.isTrigger = trigger;
                if (!trigger && !excluded) solids.Add(box);
            }
            if (solidZ)
            {
                var wall = Object("fixture z wall", new Vector3(0f, 51f, 0.7f));
                var box = wall.AddComponent<BoxCollider>(); box.size = new Vector3(3f, 2f, 0.6f); solids.Add(box);
            }
            Physics.SyncTransforms();
            Vector3 before = actor.transform.position;
            Quaternion rotationBefore = actor.transform.rotation;
            Vector3 expectedCorrection = Vector3.zero;
            foreach (var solid in solids)
            {
                Vector3 direction; float distance;
                bool overlap = Physics.ComputePenetration(capsule, before, Quaternion.identity,
                    solid, solid.transform.position, solid.transform.rotation, out direction, out distance);
                Verify(name + "/setup-overlaps", overlap && distance > 0f, "Physics must confirm a real overlap before recovery");
                expectedCorrection += direction * distance;
            }
            correcter.CollisionCorrect();
            Vector3 delta = actor.transform.position - before;
            Verify(name + "/correction", (delta - expectedCorrection).magnitude < 0.0001f,
                "observed=" + delta.ToString("F6") + "; independently measured penetration=" + expectedCorrection.ToString("F6"));
            Verify(name + "/shape-and-rotation", actor.transform.rotation == rotationBefore && capsule.height == 2f && capsule.radius == 0.5f,
                "Only position is corrected");
            if (solids.Count == 1)
            {
                Vector3 direction; float distance;
                bool remaining = Physics.ComputePenetration(capsule, actor.transform.position, Quaternion.identity,
                    solids[0], solids[0].transform.position, solids[0].transform.rotation, out direction, out distance);
                Verify(name + "/resolved", !remaining || distance < 0.0001f, "remaining penetration=" + distance);
            }
        }
        static void DirectionCase(PlayerAnimationManager manager, float x, float y, Constant.Direct expected, string name)
        {
            ((SmoothFloat)typeof(PlayerAnimationManager).GetField("strafeX", Private).GetValue(manager)).Current = x;
            ((SmoothFloat)typeof(PlayerAnimationManager).GetField("strafeY", Private).GetValue(manager)).Current = y;
            Constant.Direct actual = manager.NearStrafeDirect();
            Verify("direction/" + name, actual == expected, "actual=" + actual + "; expected=" + expected);
        }
        static void Directions()
        {
            var manager = new PlayerAnimationManager();
            var vectors = new[] { new Vector2(0,1), new Vector2(0,-1), new Vector2(-1,0), new Vector2(1,0),
                new Vector2(-1,1), new Vector2(1,1), new Vector2(-1,-1), new Vector2(1,-1) };
            for (int i = 0; i < vectors.Length; i++) DirectionCase(manager, vectors[i].x, vectors[i].y, (Constant.Direct)i, ((Constant.Direct)i).ToString());
            float[] boundaries = { 22.5f, 67.5f, 112.5f, 157.5f };
            var positive = new[] { Constant.Direct.Forward, Constant.Direct.ForwardRight, Constant.Direct.Right, Constant.Direct.BackRight, Constant.Direct.Back };
            var negative = new[] { Constant.Direct.Forward, Constant.Direct.ForwardLeft, Constant.Direct.Left, Constant.Direct.BackLeft, Constant.Direct.Back };
            for (int i = 0; i < boundaries.Length; i++)
                foreach (int sign in new[] { -1, 1 })
                    foreach (int side in new[] { -1, 1 })
                    {
                        float degrees = sign * (boundaries[i] + side * 0.01f);
                        float rad = degrees * Mathf.Deg2Rad;
                        DirectionCase(manager, Mathf.Sin(rad), Mathf.Cos(rad), (sign > 0 ? positive : negative)[i + (side > 0 ? 1 : 0)], degrees.ToString("F2"));
                    }
            DirectionCase(manager, 0, 0, Constant.Direct.Forward, "zero");
            // Original COMISS/JAE assembly, not Ghidra's reordered pseudocode.
            DirectionCase(manager, float.NaN, 1, Constant.Direct.Back, "nan-x-native-fallthrough");
            DirectionCase(manager, 1, float.NaN, Constant.Direct.Back, "nan-y-native-fallthrough");
        }
        static void EscapeTrigger()
        {
            ClearObjects();
            controllerPath = "Assets/SfmCollisionAnimationFixture-" + Guid.NewGuid().ToString("N") + ".controller";
            var controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            controller.AddParameter("BaretaEscape", AnimatorControllerParameterType.Trigger);
            var stateMachine = controller.layers[0].stateMachine;
            var idle = stateMachine.AddState("Idle"); var escape = stateMachine.AddState("Escape");
            stateMachine.defaultState = idle;
            var clip = new AnimationClip { name = "fixture idle" };
            // Animate a child so this fixture clip cannot overwrite navigation's root position.
            clip.SetCurve("Visual", typeof(Transform), "localPosition.x", AnimationCurve.Constant(0, 1, 0));
            AssetDatabase.AddObjectToAsset(clip, controller); idle.motion = clip; escape.motion = clip;
            var transition = idle.AddTransition(escape); transition.hasExitTime = false; transition.duration = 0;
            transition.AddCondition(AnimatorConditionMode.If, 0, "BaretaEscape");
            AssetDatabase.SaveAssets();
            var animator = Object("fixture animator", Vector3.zero).AddComponent<Animator>();
            Object("Visual", Vector3.zero).transform.SetParent(animator.transform, false);
            animator.runtimeAnimatorController = controller; animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            animator.Rebind(); animator.Update(0);
            var manager = new PlayerAnimationManager(); Set(manager, "animator", animator);
            Verify("escape/initial-idle", animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"), "Real Animator begins in Idle");
            manager.TransBaretaEscape(); animator.Update(0.05f); animator.Update(0.05f);
            Verify("escape/trigger-transition", animator.GetCurrentAnimatorStateInfo(0).IsName("Escape"), "Production method triggers real Animator transition; fixture controller only");
            bool threw = false;
            try { new PlayerAnimationManager().TransBaretaEscape(); } catch (NullReferenceException) { threw = true; }
            Verify("escape/null-receiver", threw, "Preserve native null failure rather than silently skip");
        }
        static void EscapeProduction()
        {
            ClearObjects();
            var previousManager = InGameManager.Instance;
            var elevatorField = typeof(ElevatorController).GetField("<Instance>k__BackingField", BindingFlags.Static | BindingFlags.NonPublic);
            var commonField = typeof(MCommon).GetField("instance", BindingFlags.Static | BindingFlags.NonPublic);
            var previousElevator = elevatorField.GetValue(null); var previousCommon = commonField.GetValue(null);
            NavMeshDataInstance meshInstance = default(NavMeshDataInstance);
            NavMeshData meshData = null; MActionRecord action = null; MCommon common = null;
            try
            {
                elevatorField.SetValue(null, null);
                var stage = Object("fixture stage", Vector3.zero).AddComponent<StageController>();
                stage.PortalManager = stage.gameObject.AddComponent<PortalManager>();
                var portal = Object("fixture first portal", new Vector3(0, 50, 8)).AddComponent<PortalController>();
                var nearPortal = Object("fixture nearer second portal", new Vector3(0, 50, 1)).AddComponent<PortalController>();
                Set(stage.PortalManager, "<Portals>k__BackingField", new[] { portal, nearPortal });
                Verify("escape-target/first-not-nearest", stage.GetEscapeTarget() == portal.transform, "Original first portal selection");
                Set(stage.PortalManager, "<Portals>k__BackingField", new PortalController[0]);
                bool emptyThrows = false;
                try { stage.GetEscapeTarget(); } catch (IndexOutOfRangeException) { emptyThrows = true; }
                Verify("escape-target/empty-fails", emptyThrows, "Preserve original empty-array failure");
                Set(stage.PortalManager, "<Portals>k__BackingField", new[] { portal, nearPortal });
                var gameManager = Object("fixture manager", Vector3.zero).AddComponent<InGameManager>();
                gameManager.enabled = false; gameManager.CurrentStageController = stage; InGameManager.Instance = gameManager;
                var floor = new NavMeshBuildSource { shape = NavMeshBuildSourceShape.Box, size = new Vector3(40,1,40),
                    transform = Matrix4x4.TRS(new Vector3(0,49.5f,0), Quaternion.identity, Vector3.one), area = 0 };
                meshData = NavMeshBuilder.BuildNavMeshData(NavMesh.GetSettingsByID(0), new List<NavMeshBuildSource> { floor },
                    new Bounds(new Vector3(0,50,0), new Vector3(42,8,42)), Vector3.zero, Quaternion.identity);
                Verify("escape/navmesh-built", meshData != null, "Isolated engine navigation fixture");
                meshInstance = NavMesh.AddNavMeshData(meshData);
                var actor = Object("fixture escape actor", new Vector3(0,50,0));
                var agent = actor.AddComponent<NavMeshAgent>();
                Verify("escape/navmesh-agent", agent.isOnNavMesh, "Actual agent bound to fixture NavMesh");
                agent.enabled = false;
                var cc = actor.AddComponent<CharacterController>();
                var player = actor.AddComponent<PlayerController>();
                var receiver = actor.AddComponent<PlayerAnimationReceiver>();
                var animator = actor.AddComponent<Animator>();
                Object("Visual", Vector3.zero).transform.SetParent(actor.transform, false);
                var controller = AssetDatabase.LoadAssetAtPath<AnimatorController>(controllerPath);
                controller.AddParameter("IsEcstasy", AnimatorControllerParameterType.Bool);
                controller.AddParameter("OldActionId", AnimatorControllerParameterType.Int);
                controller.AddParameter("ActionId", AnimatorControllerParameterType.Int);
                controller.AddParameter("TransAction", AnimatorControllerParameterType.Trigger);
                animator.runtimeAnimatorController = controller; animator.applyRootMotion = false; animator.Rebind(); animator.Update(0);
                var pam = new PlayerAnimationManager(); Set(pam, "animator", animator);
                action = ScriptableObject.CreateInstance<MActionRecord>(); Set(action, "type", ActionType.SadHandcuffAtMap);
                // This isolated fixture supplies only the state consumed by these methods;
                // the full PlayerStateModel constructor requires the game's master-data bootstrap.
                var state = (PlayerStateModel)FormatterServices.GetUninitializedObject(typeof(PlayerStateModel));
                state.CurrentState = PlayerStateModel.PlayerState.Idle; state.CurrentAction = action;
                var pca = (PlayerClassAccessor)FormatterServices.GetUninitializedObject(typeof(PlayerClassAccessor));
                Set(pca, "<Transform>k__BackingField", actor.transform); Set(pca, "<NavAgent>k__BackingField", agent);
                Set(pca, "<CharacterController>k__BackingField", cc); Set(pca, "<Player>k__BackingField", player);
                Set(pca, "<PlayerState>k__BackingField", state); Set(pca, "<PlayerAnimationManager>k__BackingField", pam);
                Set(player, "pca", pca); receiver.Initialize(pca);
                receiver.OnEndSad();
                Verify("escape/handcuff-return", state.CurrentState == PlayerStateModel.PlayerState.Idle && cc.enabled && !agent.enabled,
                    "Protected action does not start navigation");
                Set(action, "type", ActionType.None);
                var elevator = Object("fixture elevator", Vector3.zero).AddComponent<ElevatorController>();
                Set(elevator, "<PushBounds>k__BackingField", new Bounds(actor.transform.position, Vector3.one * 3));
                elevatorField.SetValue(null, elevator); receiver.OnEndSad();
                Verify("escape/elevator-crouch", animator.GetInteger("ActionId") == (int)ActionType.CrouchCry && !agent.enabled,
                    "Bounds branch dispatches CrouchCry without escape");
                elevatorField.SetValue(null, null);
                receiver.OnEndSad(); animator.Update(0.05f); animator.Update(0.05f);
                Verify("escape/production-transition", state.CurrentState == PlayerStateModel.PlayerState.BaretaEscape && animator.GetCurrentAnimatorStateInfo(0).IsName("Escape"),
                    "OnEndSad -> Player.ChangeState -> real Animator trigger, isolated Idle starting state");
                Verify("escape/production-switches", !cc.enabled && agent.enabled && agent.updatePosition && agent.updateRotation && pam.TargetMoveSpeed == 2f,
                    "Original navigation and animation speed settings");
                NavMeshHit sampledDestination;
                Verify("escape/target-on-mesh", NavMesh.SamplePosition(portal.transform.position, out sampledDestination, 0.5f, NavMesh.AllAreas),
                    "Use the actual baked surface projection, including its voxel-height offset");
                Verify("escape/production-destination", Vector3.Distance(agent.destination, sampledDestination.position) < 0.001f,
                    "destination=" + agent.destination + "; actor=" + actor.transform.position + "; onMesh=" + agent.isOnNavMesh);
                var path = new NavMeshPath();
                bool pathOk = NavMesh.CalculatePath(actor.transform.position, portal.transform.position, NavMesh.AllAreas, path);
                Verify("escape/path", pathOk && path.status == NavMeshPathStatus.PathComplete && agent.SetPath(path), "Synchronous path for one production update");
                common = ScriptableObject.CreateInstance<MCommon>(); Set(common, "playerJogSpeed", 2.4f); commonField.SetValue(null, common);
                var escapeState = new PlayerBaretaEscapeState(); escapeState.Initialize(pca);
                Vector3 before = actor.transform.position;
                float dt = Time.deltaTime;
                Verify("escape/update-delta", dt > 0 && dt < 1f, "Actual Unity deltaTime=" + dt);
                escapeState.OnUpdate();
                float distance = Vector3.Distance(before, actor.transform.position);
                Verify("escape/production-moves", distance > 0 && Mathf.Abs(distance - 2.4f * dt) < 0.001f, "distance=" + distance + "; expected=" + (2.4f * dt));
                Verify("escape/next-position", Vector3.Distance(agent.nextPosition, actor.transform.position) < 0.001f, "Navigation follows actual transform");
                var sad = AssetDatabase.LoadAssetAtPath<AnimationClip>("Assets/AnimationClip/Sad.anim");
                bool originalEvent = false;
                if (sad != null) foreach (var evt in AnimationUtility.GetAnimationEvents(sad))
                    if (evt.functionName == "OnEndSad" && Mathf.Abs(evt.time - 2.2666667f) < 0.0001f) originalEvent = true;
                Verify("escape/original-clip-event", originalEvent, "Original clip contains OnEndSad at 2.2666667; full original clip dispatch not tested");
            }
            finally
            {
                InGameManager.Instance = previousManager; elevatorField.SetValue(null, previousElevator); commonField.SetValue(null, previousCommon);
                ClearObjects(); if (meshInstance.valid) meshInstance.Remove();
                if (meshData) UnityEngine.Object.DestroyImmediate(meshData);
                if (action) UnityEngine.Object.DestroyImmediate(action);
                if (common) UnityEngine.Object.DestroyImmediate(common);
            }
        }
        public static void Run()
        {
            var args = Environment.GetCommandLineArgs(); int at = Array.IndexOf(args, "-sfmCollisionAnimationReport");
            if (at < 0 || at + 1 >= args.Length) throw new ArgumentException("Explicit fresh report path required");
            string output = Path.GetFullPath(args[at + 1]);
            if (File.Exists(output)) throw new IOException("Do not overwrite prior evidence");
            report = new Report { unity = Application.unityVersion, assembly = typeof(PlayerAnimationManager).Assembly.Location, playMode = Application.isPlaying };
            Application.logMessageReceived += OnLog;
            try
            {
                if (!Application.isPlaying) EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
                CollisionCase("self-only", false, false, false, false, false);
                CollisionCase("single-solid", true, false, false, false, false);
                CollisionCase("two-solids", true, true, false, false, false);
                CollisionCase("trigger-only", false, false, true, false, false);
                CollisionCase("excluded-layer", false, false, false, true, false);
                CollisionCase("rotated-solid", true, false, false, false, true);
                Directions(); EscapeTrigger(); EscapeProduction();
                Verify("engine/no-errors", report.engineErrors.Count == 0, string.Join("; ", report.engineErrors));
                report.passed = true;
            }
            catch (Exception error) { report.error = error.ToString(); }
            finally
            {
                ClearObjects();
                if (controllerPath != null) AssetDatabase.DeleteAsset(controllerPath);
                Application.logMessageReceived -= OnLog;
                Directory.CreateDirectory(Path.GetDirectoryName(output));
                File.WriteAllText(output, JsonUtility.ToJson(report, true) + "\n");
            }
            if (!report.passed) throw new InvalidOperationException(report.error);
            Debug.Log("SFM_COLLISION_ANIMATION_FIXTURES passed checks=" + report.checks.Count + " actualGameLoopAccepted=false");
        }
    }
}
#endif
