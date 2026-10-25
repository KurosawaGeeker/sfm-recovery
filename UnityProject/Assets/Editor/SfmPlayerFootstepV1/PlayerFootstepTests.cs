#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;
using UnityEditor;
using UnityEditor.Animations;
using UnityEditor.SceneManagement;
using UnityEngine;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using ExposureUnnoticed2.Scripts.Sound;
using ExposureUnnoticed2.Master.Action;
using ExposureUnnoticed2.Master.Costume;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using ExposureUnnoticed2.Object3D.Player.ScriptableObject;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Object3D.NPC.Script.State;
using Surface = ExposureUnnoticed2.Object3D.ScenePlops.FootStepSoundObject.FootStepSoundObject;
using Se = ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType;

namespace SfmMvp.Editor
{
    // Controlled engine fixtures only. No save files, original gameplay rewards or failures are exercised.
    [InitializeOnLoad]
    public static class PlayerFootstepTests
    {
        const string Pending = "SfmPlayerFootstepV1.Pending";
        const BindingFlags Private = BindingFlags.Instance | BindingFlags.NonPublic;
        const BindingFlags StaticPrivate = BindingFlags.Static | BindingFlags.NonPublic;
        [Serializable] sealed class Check { public string name, detail; public bool passed; }
        [Serializable] sealed class EngineErrorSample
        {
            public string kind, message, stack;
            public int count;
        }
        [Serializable] sealed class Report
        {
            public string schema = "SfmPlayerFootstepEngineFixture/v1", unity, assembly, error;
            public bool passed, playMode, actualGameLoopAccepted = false, originalCharacterAnimationDispatched = false;
            public List<Check> checks = new List<Check>();
            public List<int> eventTypes = new List<int>();
            public List<string> engineErrors = new List<string>();
            public List<EngineErrorSample> engineErrorSamples = new List<EngineErrorSample>();
            public int engineErrorCount, engineErrorUnrecordedCount;
        }
        sealed class Wait { public int frame; public float time; }
        sealed class Shoes : IShoesRecord
        {
            public CostumeEffectType EffectType { get; set; }
            public float HeelSize => 0f;
            public bool IsNotHeelToe => false;
            public float HeelToeShapeScale => 0f;
        }
        static Report report;
        static IEnumerator run;
        static Wait wait;
        static double started;
        static string controllerPath;
        static readonly List<GameObject> objects = new List<GameObject>();
        static readonly List<UnityEngine.Object> assets = new List<UnityEngine.Object>();
        static readonly List<Action> restores = new List<Action>();
        static PlayerSoundManager sound;
        static NpcStrangenessManager npc;
        static Action<PlayerSoundEvent> capture;
        static SimulationMode savedPhysics;
        static UnityEngine.Random.State savedRandom;
        static bool isolatedScheduler;
        const int MaxEngineErrorSamples = 64;
        static readonly Dictionary<string, int> engineErrorIndices = new Dictionary<string, int>();

        static PlayerFootstepTests()
        {
            EditorApplication.playModeStateChanged += state =>
            {
                if (state == PlayModeStateChange.EnteredPlayMode && SessionState.GetBool(Pending, false))
                    EditorApplication.delayCall += Begin;
            };
        }
        public static void RunPlay()
        {
            if (EditorApplication.isPlaying) throw new InvalidOperationException("Require a fresh Editor process");
            if (Array.IndexOf(Environment.GetCommandLineArgs(), "-sfmNpcAnimationProbeV1") < 0)
                throw new InvalidOperationException("Require the existing explicit empty-fixture bootstrap flag");
            EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            SessionState.SetBool(Pending, true);
            EditorApplication.isPlaying = true;
        }
        static void Begin()
        {
            report = new Report { unity = Application.unityVersion, assembly = typeof(PlayerFootStepController).Assembly.Location, playMode = Application.isPlaying };
            engineErrorIndices.Clear();
            started = EditorApplication.timeSinceStartup;
            savedPhysics = Physics.simulationMode;
            savedRandom = UnityEngine.Random.state;
            Application.logMessageReceived += OnLog;
            run = Execute();
            EditorApplication.update += Tick;
        }
        static void Tick()
        {
            try
            {
                if (EditorApplication.timeSinceStartup - started > 45) throw new TimeoutException("Owned footstep fixture exceeded 45 seconds");
                if (wait != null && (Time.frameCount < wait.frame || Time.time < wait.time)) return;
                wait = null;
                if (!run.MoveNext()) { Finish(null); return; }
                wait = run.Current as Wait;
            }
            catch (Exception error) { Finish(error); }
        }
        static Wait Seconds(float seconds) => new Wait { time = Time.time + seconds };
        static Wait Frames(int count) => new Wait { frame = Time.frameCount + count };
        static void OnLog(string text, string stack, LogType kind)
        {
            if (kind != LogType.Error && kind != LogType.Exception && kind != LogType.Assert) return;
            report.engineErrorCount++;
            string key = kind + "\n" + text + "\n" + stack;
            if (engineErrorIndices.TryGetValue(key, out int index))
            {
                report.engineErrorSamples[index].count++;
                return;
            }
            // Retain the first complete stack for every recorded signature; bound unique payloads.
            // Overflow still fails the fixture and is included in the exact total count.
            if (report.engineErrorSamples.Count >= MaxEngineErrorSamples)
            {
                report.engineErrorUnrecordedCount++;
                return;
            }
            engineErrorIndices.Add(key, report.engineErrorSamples.Count);
            report.engineErrors.Add(text);
            report.engineErrorSamples.Add(new EngineErrorSample { kind = kind.ToString(), message = text, stack = stack, count = 1 });
        }
        static void Verify(string name, bool ok, string detail)
        {
            report.checks.Add(new Check { name = name, passed = ok, detail = detail });
            if (!ok) throw new InvalidOperationException(name + ": " + detail);
        }
        static void Set(object target, string field, object value)
        {
            var info = target.GetType().GetField(field, Private | BindingFlags.Public);
            if (info == null) throw new MissingFieldException(target.GetType().FullName, field);
            info.SetValue(target, value);
        }
        static object Get(object target, string field) => target.GetType().GetField(field, Private | BindingFlags.Public).GetValue(target);
        static T Uninitialized<T>() => (T)FormatterServices.GetUninitializedObject(typeof(T));
        static void SaveStatic(Type owner, string field)
        {
            var info = owner.GetField(field, StaticPrivate | BindingFlags.Public);
            var value = info.GetValue(null);
            restores.Add(() => info.SetValue(null, value));
        }
        static void IsolateEvents()
        {
            foreach (string name in new[] { "s_Events", "s_EventLookups" })
            {
                var dict = (IDictionary)typeof(EventManager).GetField(name, StaticPrivate).GetValue(null);
                var old = new List<DictionaryEntry>(); foreach (DictionaryEntry entry in dict) old.Add(entry);
                dict.Clear();
                restores.Add(() => { dict.Clear(); foreach (var entry in old) dict.Add(entry.Key, entry.Value); });
            }
            var oldEvent = Events.PlayerSoundEvent;
            var oldType = oldEvent.SeType; bool oldPiston = oldEvent.IsPistonMachine;
            restores.Add(() => { Events.PlayerSoundEvent = oldEvent; oldEvent.SeType = oldType; oldEvent.IsPistonMachine = oldPiston; });
        }
        static GameObject Object(string name, Vector3 position, bool active = true)
        {
            var go = new GameObject(name); go.SetActive(active); go.transform.position = position; objects.Add(go); return go;
        }
        static T Asset<T>() where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>(); assets.Add(asset); return asset;
        }
        static void Simulate()
        {
            Physics.SyncTransforms(); Physics.Simulate(0.02f); Physics.SyncTransforms();
        }
        static void ExpectSound(string name, Action step, Se expected, AudioSource source, float volume, float? noise = null)
        {
            int before = report.eventTypes.Count;
            step();
            Verify(name + "/event", report.eventTypes.Count == before + 1 && report.eventTypes[before] == (int)expected, "Production EventManager emitted " + expected);
            Verify(name + "/volume", Mathf.Abs(source.volume - volume) < 0.0001f, "Actual AudioSource.volume=" + source.volume);
            if (noise.HasValue)
                Verify(name + "/npc-listener", Mathf.Abs((float)Get(npc, "addNoisyByPlayerSound") - noise.Value) < 0.00001f,
                    "Original NPC subscriber pending noise=" + Get(npc, "addNoisyByPlayerSound") + "; this is not NPC discovery acceptance");
        }
        static void ExpectSuppressed(string name, Action step)
        {
            int before = report.eventTypes.Count; step();
            Verify(name, report.eventTypes.Count == before, "No player-sound event through production dispatch");
        }
        static IEnumerator Execute()
        {
            Verify("fixture/play-mode", Application.isPlaying, "Actual Unity PlayMode");
            IsolateEvents();
            SaveStatic(typeof(InGameSoundManager), "Instance");
            SaveStatic(typeof(InGameManager), "Instance");
            SaveStatic(typeof(PlayerController), "Instance");
            SaveStatic(typeof(PlayerSoundManager), "<Instance>k__BackingField");
            SaveStatic(typeof(Scheduler), "instance");
            // Own a separate scheduler so existing delayed work cannot run in this fixture.
            typeof(Scheduler).GetField("instance", StaticPrivate).SetValue(null, null);
            isolatedScheduler = true;
            Physics.simulationMode = SimulationMode.Script;
            int level = LayerAccessor.LevelLayer;
            Verify("fixture/level-layer", level >= 0 && level < 32, "Real project Level layer=" + level);

            var listener = Object("footstep fixture listener", new Vector3(0, 50, 0)).AddComponent<AudioListener>();
            var soundRoot = Object("footstep fixture sound library", Vector3.zero, false);
            InGameSoundManager.Instance = null;
            var library = soundRoot.AddComponent<InGameSoundManager>();
            var clips = new AudioClip[2];
            for (int i = 0; i < clips.Length; i++)
            {
                clips[i] = AudioClip.Create("fixture tone " + i, 22050, 1, 22050, false); assets.Add(clips[i]);
                var data = new float[22050];
                for (int n = 0; n < data.Length; n++) data[n] = 0.01f * Mathf.Sin(2f * Mathf.PI * (220 + i * 40) * n / 22050f);
                clips[i].SetData(data, 0);
            }
            foreach (string field in new[] { "footStepHeel", "footStepHeelJog", "footStepHeelCrouch", "footStepBarefootCrouch", "footStepSneaker", "footStepSneakerJog", "footStepGrass", "footStepGravel", "footStepMetal", "kuchu", "piston" }) Set(library, field, clips);
            soundRoot.SetActive(true);
            Verify("audio/library-awake", InGameSoundManager.Instance == library, "Original Awake created sound groups from two short fixture clips");

            var managerRoot = Object("footstep fixture manager", Vector3.zero, false);
            var manager = managerRoot.AddComponent<InGameManager>();
            manager.enabled = false;
            // InGameManager requires this component, but its full-world Initialize is outside this fixture.
            // Disable only the dispatcher created on our owned inactive object, before any frame can run.
            var dispatcher = managerRoot.GetComponent<EventMethodManager>();
            Verify("fixture/owned-dispatcher-present", dispatcher != null && dispatcher.gameObject == managerRoot,
                "InGameManager RequireComponent created the fixture-owned EventMethodManager");
            dispatcher.enabled = false;
            Set(manager, "<TempInGameState>k__BackingField", new TemporaryInGameState()); InGameManager.Instance = manager;
            managerRoot.SetActive(true);
            Verify("fixture/unconfigured-dispatcher-disabled", !dispatcher.isActiveAndEnabled && !manager.isActiveAndEnabled,
                "Uninitialized full-world dispatcher and manager cannot execute their automatic frame callbacks");
            var stage = Object("footstep fixture home stage", Vector3.zero).AddComponent<StageController>(); stage.enabled = false;
            manager.CurrentStageController = stage;
            var actor = Object("footstep fixture actor", new Vector3(0, 50, 0));
            var source = actor.AddComponent<AudioSource>(); source.playOnAwake = false; source.spatialBlend = 0; source.volume = 0.8f;
            var avatar = actor.AddComponent<PlayerAvatarObjectReferencer>(); avatar.PickingAudioSource = source;
            var state = Uninitialized<PlayerStateModel>();
            var action = Asset<MActionRecord>(); Set(action, "canMove", false); state.CurrentAction = action;
            var pca = Uninitialized<PlayerClassAccessor>();
            var animation = new PlayerAnimationManager();
            var foot = new PlayerFootStepController(); foot.Initialize(pca);
            var player = actor.AddComponent<PlayerController>(); player.enabled = false; Set(player, "pca", pca); PlayerController.Instance = player;
            var refObject = actor.AddComponent<PlayerObjectReferencer>();
            Set(pca, "<Transform>k__BackingField", actor.transform); Set(pca, "<GameObject>k__BackingField", actor);
            Set(pca, "<PlayerState>k__BackingField", state); Set(pca, "<PlayerAvatarObjectReferencer>k__BackingField", avatar);
            Set(pca, "<PlayerObjectReferencer>k__BackingField", refObject); Set(pca, "<PlayerAnimationManager>k__BackingField", animation);
            Set(pca, "<PlayerFootStepController>k__BackingField", foot);
            sound = new PlayerSoundManager(); Set(pca, "<PlayerSoundManager>k__BackingField", sound);
            sound.Initialize(source, actor, pca);
            var receiver = actor.AddComponent<PlayerAnimationReceiver>(); receiver.Initialize(pca);
            var record = Asset<MFootAnimationEventRecord>(); record.Direct = Constant.Direct.Forward; record.SeType = Se.FootStepHeelJog;
            var evt = new AnimationEvent { objectReferenceParameter = record };
            capture = e => report.eventTypes.Add((int)e.SeType); EventManager.AddListener(capture, false);

            var terrainGo = Object("footstep fixture ground sensor", new Vector3(0, 50, 0));
            terrainGo.layer = LayerAccessor.PlayerLayer;
            Verify("terrain/player-level-collisions-enabled", !Physics.GetIgnoreLayerCollision(terrainGo.layer, level), "Actual Player prefab sensor uses Player layer 6; project collision matrix permits Level contacts");
            var trigger = terrainGo.AddComponent<BoxCollider>(); trigger.isTrigger = true; trigger.size = new Vector3(2, 2, 2);
            var body = terrainGo.AddComponent<Rigidbody>(); body.isKinematic = true; body.useGravity = false;
            var terrain = terrainGo.AddComponent<TerrainCheckCollider>(); refObject.TerrainCheckCollider = terrain;
            Verify("terrain/constructor-list", terrain.CollisionList != null && terrain.CollisionList.Count == 0, "Actual component constructor owns initialized collision list");
            var solid = Object("footstep fixture solid", new Vector3(5, 50, 0)); solid.layer = level; solid.AddComponent<BoxCollider>();
            var ignoredTrigger = Object("footstep fixture ignored trigger", new Vector3(5, 50, 0)); ignoredTrigger.layer = level; ignoredTrigger.AddComponent<BoxCollider>().isTrigger = true;
            var ignoredLayer = Object("footstep fixture ignored layer", new Vector3(5, 50, 0)); ignoredLayer.layer = LayerMask.NameToLayer("Ignore Raycast"); ignoredLayer.AddComponent<BoxCollider>();
            Simulate(); solid.transform.position = new Vector3(0, 50, 0); Simulate();
            Verify("terrain/real-physx-enter", terrain.CollisionList.Contains(solid), "PhysX OnTriggerEnter added Level solid without direct callback invocation");
            ignoredTrigger.transform.position = new Vector3(0, 50, 0); ignoredLayer.transform.position = new Vector3(0, 50, 0); Simulate();
            Verify("terrain/trigger-layer-exclusion", terrain.CollisionList.Count == 1, "Trigger and non-Level solids excluded");
            Simulate(); Verify("terrain/no-duplicate", terrain.CollisionList.Count == 1, "Persistent physical overlap keeps a single entry");
            solid.transform.position = new Vector3(5, 50, 0); Simulate();
            Verify("terrain/real-physx-exit", terrain.CollisionList.Count == 0, "PhysX OnTriggerExit removed Level solid");

            var npcObject = Object("footstep fixture listener NPC", new Vector3(1, 50, 0));
            var nca = Uninitialized<NpcComponentAccessor>(); var ns = Uninitialized<NpcStateModel>();
            ns.CurrentFixedType = NpcStateModel.FixedType.None; ns.CurrentState = NpcStateModel.State.Idle; ns.TurnNoisyLevel = 0.5f; ns.StrangeLookAt = 0.3f;
            var searcher = npcObject.AddComponent<PlayerSearcher>(); searcher.enabled = false;
            var chair = new NpcChairSubState(); chair.Initialize(nca);
            npc = new NpcStrangenessManager();
            Set(nca, "<StateModel>k__BackingField", ns); Set(nca, "<Player>k__BackingField", player);
            Set(nca, "<PlayerSearcher>k__BackingField", searcher); Set(nca, "<GameObject>k__BackingField", npcObject);
            Set(nca, "<StrangenessManager>k__BackingField", npc); Set(nca, "<NpcChairSubState>k__BackingField", chair);
            npc.Initialize(nca);

            state.CurrentShoesRecord = new Shoes { EffectType = CostumeEffectType.Sneaker };
            yield return Seconds(0.21f);
            ExpectSound("sneaker-walk-footL", () => receiver.FootL(evt), Se.FootStepSneaker, source, 0.6f, 0.025f);
            Verify("audio/real-voice-playing", source.isPlaying, "Production PlayOneShot activated actual AudioSource; fixture clips only");
            ExpectSuppressed("throttle/same-frame-footR", () => receiver.FootR(evt));
            yield return Seconds(0.08f);
            ExpectSuppressed("throttle/before-200ms", () => receiver.FootL(evt));
            yield return Seconds(0.15f);
            ExpectSound("throttle/after-200ms-footR", () => receiver.FootR(evt), Se.FootStepSneaker, source, 0.6f, 0.025f);
            state.IsDash = true; yield return Seconds(0.21f);
            ExpectSound("sneaker-dash", () => receiver.FootL(evt), Se.FootStepSneakerJog, source, 0.6f, 0.025f);
            state.IsDash = false; state.CurrentShoesRecord = null; yield return Seconds(0.21f);
            ExpectSound("barefoot", () => receiver.FootR(evt), Se.Barefoot, source, 0.8f, 0.01f);
            state.CurrentShoesRecord = new Shoes { EffectType = CostumeEffectType.Heel }; yield return Seconds(0.21f);
            ExpectSound("heel-walk", () => receiver.FootL(evt), Se.FootStepHeelJog, source, 0.8f, 0.025f);
            Verify("heel/walk-pulse", Mathf.Abs((float)Get(foot, "startBareyasusa") - 0.201f) < 0.00001f && Mathf.Abs((float)Get(foot, "originalBareyasusaByHeel") - 0.301f) < 0.00001f, "Original 0.201 cap plus 0.1 reservoir; does not itself prove NPC use");
            state.IsDash = true; yield return Seconds(0.21f);
            ExpectSound("heel-dash", () => receiver.FootR(evt), Se.FootStepHeelJog, source, 0.8f, 0.025f);
            Verify("heel/dash-pulse", Mathf.Abs((float)Get(foot, "startBareyasusa") - 0.301f) < 0.00001f && Mathf.Abs((float)Get(foot, "originalBareyasusaByHeel") - 0.401f) < 0.00001f, "Original dash pulse cap and reservoir");
            foot.OnUpdate(); Verify("heel/decay-output", state.BareyasusaByHeel > 0 && state.BareyasusaByHeel <= 0.301f, "Existing real Time.deltaTime decay writes bounded output");
            state.IsCrouch = true; yield return Seconds(0.21f);
            ExpectSound("crouch-heel-is-barefoot", () => receiver.FootL(evt), Se.Barefoot, source, 0.8f, 0.01f);

            state.IsCrouch = false; state.IsDash = false; state.IsStrafe = true;
            ((SmoothFloat)Get(animation, "strafeX")).Current = 1f; ((SmoothFloat)Get(animation, "strafeY")).Current = 0f;
            yield return Seconds(0.21f);
            ExpectSuppressed("direction/reject-other-blend-event", () => receiver.FootR(evt));
            record.Direct = Constant.Direct.Right;
            ExpectSound("direction/accept-matching-event", () => receiver.FootR(evt), Se.FootStepHeelJog, source, 0.8f);
            record.Direct = Constant.Direct.Forward; state.IsGamanBaibu = true; yield return Seconds(0.21f);
            ExpectSound("direction/gaman-bypasses-gate", () => receiver.FootL(evt), Se.FootStepHeelJog, source, 0.8f);
            state.IsGamanBaibu = false; state.IsStrafe = false; Set(action, "canMove", true); action.FloatParam = 1f; record.IsMoveAction = false;
            yield return Seconds(0.21f);
            ExpectSuppressed("action/reject-non-move-record", () => receiver.FootL(evt));
            record.IsMoveAction = true;
            ExpectSound("action/accept-move-record", () => receiver.FootL(evt), Se.FootStepHeelJog, source, 0.8f);
            record.IsMoveAction = false; action.FloatParam = 0f; yield return Seconds(0.21f);
            ExpectSound("action/zero-param-bypasses", () => receiver.FootR(evt), Se.FootStepHeelJog, source, 0.8f);
            action.FloatParam = float.NaN; yield return Seconds(0.21f);
            ExpectSuppressed("action/nan-is-nonzero", () => receiver.FootR(evt));
            action.FloatParam = 0f; Set(action, "canMove", false);
            bool badCast = false, nullRecord = false;
            try { receiver.FootL(new AnimationEvent { objectReferenceParameter = clips[0] }); } catch (InvalidCastException) { badCast = true; }
            state.CurrentShoesRecord = null;
            try { receiver.FootR(new AnimationEvent()); } catch (NullReferenceException) { nullRecord = true; }
            Verify("record/checked-cast", badCast, "Wrong objectReferenceParameter raises original cast failure");
            Verify("record/null-even-barefoot", nullRecord, "Missing record is not silently accepted on barefoot branch");
            state.CurrentShoesRecord = new Shoes { EffectType = CostumeEffectType.Sneaker };

            controllerPath = "Assets/SfmFootstepFixture-" + Guid.NewGuid().ToString("N") + ".controller";
            var controller = AnimatorController.CreateAnimatorControllerAtPath(controllerPath);
            var animationClip = new AnimationClip { name = "fixture FootL event" };
            animationClip.SetCurve("Visual", typeof(Transform), "localPosition.x", AnimationCurve.Constant(0, 1, 0));
            animationClip.AddEvent(new AnimationEvent { time = 0.1f, functionName = "FootL", objectReferenceParameter = record });
            AssetDatabase.AddObjectToAsset(animationClip, controller);
            var machine = controller.layers[0].stateMachine; var clipState = machine.AddState("Step"); clipState.motion = animationClip; machine.defaultState = clipState;
            AssetDatabase.SaveAssets();
            var eventActor = Object("footstep fixture animated receiver", new Vector3(0, 50, 0));
            Object("Visual", Vector3.zero).transform.SetParent(eventActor.transform, false);
            eventActor.AddComponent<PlayerAnimationReceiver>().Initialize(pca);
            var animator = eventActor.AddComponent<Animator>(); animator.runtimeAnimatorController = controller; animator.cullingMode = AnimatorCullingMode.AlwaysAnimate;
            animator.Rebind(); animator.Update(0);
            ExpectSound("animation/actual-event-dispatch", () => animator.Update(0.2f), Se.FootStepSneaker, source, 0.6f);
            UnityEngine.Object.DestroyImmediate(eventActor);

            var grass = Object("footstep fixture grass surface", new Vector3(5, 50, 0), false); grass.layer = level; grass.AddComponent<BoxCollider>();
            var grassSurface = grass.AddComponent<Surface>(); Set(grassSurface, "footStepType", Se.FootStepGrass); grass.SetActive(true);
            var dictionary = (Dictionary<GameObject, Se>)Get(sound, "FootStepObjectDict");
            Verify("surface/registration-delayed", !dictionary.ContainsKey(grass), "Original Awake schedules registration");
            yield return Frames(3);
            Verify("surface/registration-scheduled", dictionary.TryGetValue(grass, out Se grassType) && grassType == Se.FootStepGrass, "Actual Scheduler dispatched original one-frame registration");
            grass.transform.position = new Vector3(0, 50, 0); Simulate(); yield return Seconds(0.21f);
            ExpectSound("surface/grass-conversion", () => receiver.FootL(evt), Se.FootStepGrass, source, 0.3f, 0f);
            var gravel = Object("footstep fixture gravel surface", new Vector3(5, 50, 0), false); gravel.layer = level; gravel.AddComponent<BoxCollider>();
            var gravelSurface = gravel.AddComponent<Surface>(); Set(gravelSurface, "footStepType", Se.FootStepGravel); gravel.SetActive(true);
            yield return Frames(3); gravel.transform.position = new Vector3(0.5f, 50, 0); Simulate(); yield return Seconds(0.21f);
            Verify("surface/physical-list-order", terrain.CollisionList.Count == 2 && terrain.CollisionList[0] == grass, "PhysX entered grass before gravel");
            ExpectSound("surface/first-registered-collision", () => receiver.FootR(evt), Se.FootStepGrass, source, 0.3f, 0f);
            grass.transform.position = new Vector3(5, 50, 0); Simulate(); yield return Seconds(0.21f);
            ExpectSound("surface/next-after-exit", () => receiver.FootL(evt), Se.FootStepGravel, source, 0.5f, 0f);
            gravel.transform.position = new Vector3(5, 50, 0); Simulate(); yield return Seconds(0.21f);
            ExpectSound("surface/fallback-after-exit", () => receiver.FootR(evt), Se.FootStepSneaker, source, 0.6f, 0.025f);
            solid.transform.position = new Vector3(0, 50, 0); Simulate();
            EventManager.Broadcast(new StartTransitionStageEvent());
            Verify("transition/immediate-clean", !trigger.enabled && terrain.CollisionList.Count == 0 && dictionary.Count == 0, "Production event clears collision and sound registries and disables sensor");
            yield return Frames(3);
            Verify("transition/scheduled-reenable", trigger.enabled, "Actual Scheduler restores sensor");
            Verify("scope/no-discovery-claim", !report.actualGameLoopAccepted && !report.originalCharacterAnimationDispatched, "Synthetic isolated fixtures do not certify natural discovery, failure, or original-character clip playback");
        }
        static void Finish(Exception failure)
        {
            EditorApplication.update -= Tick;
            try { (run as IDisposable)?.Dispose(); } catch (Exception e) { failure = failure ?? e; }
            try
            {
                if (npc != null) npc.Dispose();
                if (capture != null) EventManager.RemoveListener(capture);
                if (sound != null) sound.Dispose();
                foreach (var go in objects) if (go) UnityEngine.Object.DestroyImmediate(go);
                objects.Clear();
                var schedulerField = typeof(Scheduler).GetField("instance", StaticPrivate);
                var ownScheduler = isolatedScheduler ? schedulerField.GetValue(null) as Scheduler : null;
                if (ownScheduler) UnityEngine.Object.DestroyImmediate(ownScheduler.gameObject);
                foreach (var asset in assets) if (asset) UnityEngine.Object.DestroyImmediate(asset);
                assets.Clear();
                if (!string.IsNullOrEmpty(controllerPath)) AssetDatabase.DeleteAsset(controllerPath);
            }
            catch (Exception e) { failure = failure ?? e; }
            finally
            {
                for (int i = restores.Count - 1; i >= 0; --i) { try { restores[i](); } catch (Exception e) { failure = failure ?? e; } }
                restores.Clear(); Physics.simulationMode = savedPhysics; UnityEngine.Random.state = savedRandom;
                Application.logMessageReceived -= OnLog;
                report.error = failure?.ToString(); report.passed = failure == null && report.engineErrorCount == 0;
                string[] args = Environment.GetCommandLineArgs(); int index = Array.IndexOf(args, "-sfmFootstepReport");
                if (index >= 0 && index + 1 < args.Length)
                {
                    string output = Path.GetFullPath(args[index + 1]); Directory.CreateDirectory(Path.GetDirectoryName(output));
                    File.WriteAllText(output, JsonUtility.ToJson(report, true));
                }
                SessionState.EraseBool(Pending);
                EditorApplication.Exit(report.passed ? 0 : 1);
            }
        }
    }
}
#endif
