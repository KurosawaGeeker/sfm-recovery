using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using UnityEngine;

namespace SfmMvp
{
    // OWNED QA. Every input goes through OriginalMovementMvpBridge, never Step.
    // This does not reconstruct original input/bootstrap/ground/animation methods.
    public sealed class OriginalMovementMvpSmoke : MonoBehaviour
    {
        const string CandidateRoot = @"E:\ACTgame\SFM\ReverseEngineering\tools\unity-editor\original-movement-mvp-candidate-v1";
        const BindingFlags Methods = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;
        [Serializable] public sealed class Body
        {
            public string type, method, il_sha256;
        }
        [Serializable] public sealed class State
        {
            public int frame, original_update_calls, original_fixed_calls;
            public float time, fixed_time, realtime, delta_time, current_move_speed, animation_target_speed;
            public Vector3 position, move_v;
            public bool animator_is_strafe, bridge_ready;
            public float animator_speed, animator_strafe_x, animator_strafe_y;
        }
        [Serializable] public sealed class Sample
        {
            public string segment;
            public Vector2 supplied_input;
            public State state;
        }
        [Serializable] public sealed class ClassifiedLog
        {
            public string kind, category;
            public int frame;
        }
        [Serializable] public sealed class Evidence
        {
            public bool passed, actual_Unity_frames_executed, actual_visible_movement_verified;
            public bool temporary_QA_bridge = true;
            public bool original_method_recovery_by_bridge = false;
            public bool original_IL2CPP_loaded = false, original_save_accessed = false;
            public bool whole_original_gameplay_verified = false, whole_original_bootstrap_verified = false;
            public bool overall_clean_startup_verified = false;
            public bool observed_original_IL_unchanged;
            public string failure, company, product, unity_version, utc_started, utc_finished;
            public string log_scope = "Post TryStart handler classifies Error/Exception/Assert without copying raw log text. Launcher must retain and classify the complete process log, including earlier initializers, before any clean-startup claim.";
            public string adapter_scope = "Owned WASD/QA input, explicit flat ground flags, Mono frame scheduling, synthetic typed fields and parameter upload; original Locomotion computes movement and original PlayerController fixed entry invokes real CC physics. Original camera/ground/PAM.OnUpdate and full game lifecycle remain unrecovered.";
            public string player_exit_receipt = "Launcher writes the real process exit separately; requested code here is not an OS exit observation.";
            public int requested_exit_code, start_frame, end_frame, original_recovery_delta = 0;
            public int width, height, native_scene_colliders, active_character_renderers, visible_character_renderers;
            public float forward_displacement;
            public string initial_screenshot, walking_screenshot, final_screenshot;
            public string observed_method_pin_source, observed_method_pin_file_sha256;
            public State initial, forward_end, final;
            public List<Body> il_before = new List<Body>(), il_after = new List<Body>();
            public List<Sample> samples = new List<Sample>();
            public List<ClassifiedLog> errors_after_handler = new List<ClassifiedLog>();
        }
        OriginalMovementMvpBridge bridge;
        MvpOrbitCamera orbit;
        string outputDirectory;
        Evidence report;
        Vector2 suppliedInput;
        bool finishing;

        public static void TryStart(OriginalMovementMvpBridge movement, MvpOrbitCamera view)
        {
            var arguments = Environment.GetCommandLineArgs();
            if (Array.IndexOf(arguments, "-sfmOriginalMovementSmoke") < 0) return;
            int option = Array.IndexOf(arguments, "-sfmSmokeOutput");
            if (option < 0 || option + 1 >= arguments.Length)
                throw new ArgumentException("Owned smoke requires -sfmSmokeOutput <new candidate output directory>.");
            string path = Path.GetFullPath(arguments[option + 1]);
            string root = Path.GetFullPath(CandidateRoot).TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
            if (!path.StartsWith(root, StringComparison.OrdinalIgnoreCase) || Directory.Exists(path) || File.Exists(path))
                throw new InvalidOperationException("Smoke output must be a new directory strictly within the owned candidate stage.");
            if (movement == null || view == null || !movement.Ready)
                throw new InvalidOperationException("Original movement bridge and temporary orbit must be ready before owned smoke starts.");
            if (Application.companyName != "SFMRecovery" || Application.productName != "SFMRecoveryOriginalMovementCandidate")
                throw new InvalidOperationException("Owned smoke PlayerSettings identity is not isolated.");
            Directory.CreateDirectory(path);
            var smoke = new GameObject("Owned original-movement QA - no temporary Step").AddComponent<OriginalMovementMvpSmoke>();
            smoke.bridge = movement;
            smoke.orbit = view;
            smoke.outputDirectory = path;
            smoke.report = new Evidence
            {
                company = Application.companyName, product = Application.productName, unity_version = Application.unityVersion,
                utc_started = DateTime.UtcNow.ToString("O"), start_frame = Time.frameCount,
                initial_screenshot = Path.Combine(path, "original-movement-initial.png"),
                walking_screenshot = Path.Combine(path, "original-movement-walking.png"),
                final_screenshot = Path.Combine(path, "original-movement-stopped.png"),
                observed_method_pin_source = movement.MethodPinSource,
                observed_method_pin_file_sha256 = movement.MethodPinFileSha256
            };
            Application.targetFrameRate = 60;
            Application.logMessageReceived += smoke.ClassifyLog;
            smoke.StartCoroutine(smoke.Coordinate());
        }
        void ClassifyLog(string message, string stack, LogType type)
        {
            if (report == null || (type != LogType.Error && type != LogType.Exception && type != LogType.Assert)) return;
            string category = "other-runtime-error";
            if ((stack ?? "").Contains("EncrypterStarter") || (message ?? "").Contains("EncrypterStarter")) category = "public-encrypter-starter-resource-contract";
            else if ((message ?? "").StartsWith("SFM_OWNED_ORIGINAL_MOVEMENT")) category = "owned-movement-bridge";
            else if ((stack ?? "").Contains("PlayerLocomotion") || (stack ?? "").Contains("PlayerAnimationManager")) category = "original-movement-call";
            else if ((message ?? "").Contains("NavMesh")) category = "navmesh-engine-contract";
            else if ((message ?? "").Contains("Animator")) category = "animator-engine-contract";
            else if ((message ?? "").Contains("shader")) category = "shader-rendering";
            report.errors_after_handler.Add(new ClassifiedLog { kind = type.ToString(), category = category, frame = Time.frameCount });
        }
        static string Hash(byte[] bytes)
        {
            using (var sha = SHA256.Create()) return BitConverter.ToString(sha.ComputeHash(bytes)).Replace("-", "");
        }
        static Body Observe(Type type, string name, Type[] arguments)
        {
            var method = type.GetMethod(name, Methods, null, arguments, null);
            if (method == null || method.GetMethodBody() == null) throw new MissingMethodException(type.FullName, name);
            return new Body { type = type.FullName, method = method.ToString(), il_sha256 = Hash(method.GetMethodBody().GetILAsByteArray()) };
        }
        static List<Body> ObserveBodies()
        {
            return new List<Body>
            {
                Observe(typeof(PlayerLocomotion), "OnUpdate", new[] { typeof(InputData) }),
                Observe(typeof(PlayerLocomotion), "OnFixedUpdate", Type.EmptyTypes),
                Observe(typeof(PlayerController), "OnFixedUpdate", Type.EmptyTypes),
                Observe(typeof(PlayerAnimationManager), "SetStrafe", new[] { typeof(bool), typeof(bool), typeof(float), typeof(float) })
            };
        }
        State Capture()
        {
            var animator = bridge.visibleAnimator;
            return new State
            {
                frame = Time.frameCount, time = Time.time, fixed_time = Time.fixedTime, realtime = Time.realtimeSinceStartup, delta_time = Time.deltaTime,
                position = bridge.transform.position, move_v = bridge.OriginalMoveVector, current_move_speed = bridge.OriginalMoveSpeed,
                animation_target_speed = bridge.AnimationTargetSpeed, original_update_calls = bridge.OriginalUpdateCalls, original_fixed_calls = bridge.OriginalFixedCalls,
                bridge_ready = bridge.Ready, animator_is_strafe = animator.GetBool("IsStrafe"), animator_speed = animator.GetFloat("Speed"),
                animator_strafe_x = animator.GetFloat("StrafeX"), animator_strafe_y = animator.GetFloat("StrafeY")
            };
        }
        void Supply(Vector2 input) { suppliedInput = input; bridge.SetOwnedQaInput(input); }
        void SampleFrame(string segment)
        {
            if (!bridge.Ready) throw new InvalidOperationException("Movement bridge failed: " + bridge.Failure);
            report.samples.Add(new Sample { segment = segment, supplied_input = suppliedInput, state = Capture() });
        }
        IEnumerator CaptureImage(string path, string segment)
        {
            yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshot(path);
            for (int frame = 0; frame < 120; frame++)
            {
                if (File.Exists(path) && new FileInfo(path).Length > 4096) yield break;
                yield return new WaitForEndOfFrame();
                SampleFrame(segment + "-capture-wait");
            }
            throw new InvalidOperationException("Owned PNG capture exceeded its bounded wait: " + path);
        }
        IEnumerator Execute()
        {
            report.il_before = ObserveBodies();
            Supply(Vector2.zero);
            for (int frame = 0; frame < 20; frame++) { yield return new WaitForEndOfFrame(); SampleFrame("initial-idle"); }
            report.initial = Capture();
            yield return CaptureImage(report.initial_screenshot, "initial-idle");
            Supply(Vector2.up);
            for (int frame = 0; frame < 45; frame++)
            {
                yield return new WaitForEndOfFrame();
                SampleFrame("forward");
                if (frame == 22) yield return CaptureImage(report.walking_screenshot, "forward");
            }
            report.forward_end = Capture();
            Vector3 delta = report.forward_end.position - report.initial.position;
            delta.y = 0f;
            report.forward_displacement = delta.magnitude; // Observation only, no movement write.
            Supply(Vector2.zero);
            float stopStarted = Time.realtimeSinceStartup;
            for (int frame = 0; frame < 2000; frame++)
            {
                yield return new WaitForEndOfFrame();
                SampleFrame("released-wait-original-stop");
                if (Mathf.Abs(bridge.OriginalMoveSpeed) < .00001f && bridge.OriginalMoveVector.sqrMagnitude < .00000001f) break;
                if (frame == 1999 || Time.realtimeSinceStartup - stopStarted > 12f)
                    throw new InvalidOperationException("Original SmoothFloat stop exceeded bounded wait; report keeps final scalar values.");
            }
            yield return CaptureImage(report.final_screenshot, "stopped");
            report.final = Capture(); // Exact final values, beyond periodic samples.
            report.il_after = ObserveBodies();
            report.observed_original_IL_unchanged = true;
            for (int i = 0; i < report.il_before.Count; i++)
                report.observed_original_IL_unchanged &= report.il_before[i].il_sha256 == report.il_after[i].il_sha256;
            report.width = Screen.width; report.height = Screen.height;
            report.native_scene_colliders = UnityEngine.Object.FindObjectsOfType<Collider>().Length;
            foreach (var renderer in bridge.GetComponentsInChildren<Renderer>(true))
                if (renderer.enabled && renderer.gameObject.activeInHierarchy)
                { report.active_character_renderers++; if (renderer.isVisible) report.visible_character_renderers++; }
            report.actual_Unity_frames_executed = report.final.frame > report.initial.frame + 45 &&
                report.final.original_update_calls > report.initial.original_update_calls && report.final.original_fixed_calls > report.initial.original_fixed_calls;
            report.actual_visible_movement_verified = report.forward_displacement > .05f && report.active_character_renderers > 0 && report.visible_character_renderers > 0;
            report.passed = report.actual_Unity_frames_executed && report.actual_visible_movement_verified && report.observed_original_IL_unchanged &&
                report.final.bridge_ready && !report.final.animator_is_strafe && Mathf.Abs(report.final.current_move_speed) < .00001f &&
                report.final.move_v.sqrMagnitude < .00000001f && report.final.animation_target_speed == 0f && report.final.animator_speed == 0f &&
                report.final.animator_strafe_x == 0f && report.final.animator_strafe_y == 0f && report.width > 0 && report.height > 0 &&
                report.errors_after_handler.Count == 0;
        }
        IEnumerator Coordinate()
        {
            IEnumerator experiment = Execute();
            while (true)
            {
                bool next = false; object yielded = null; Exception failure = null;
                try { next = experiment.MoveNext(); if (next) yielded = experiment.Current; }
                catch (Exception exception) { failure = exception; }
                if (failure != null) { report.failure = failure.ToString(); break; }
                if (!next) break;
                // Nested iterators are wrapped so screenshot/QA exceptions are
                // captured in this report instead of escaping a Unity coroutine.
                if (yielded is IEnumerator nested)
                {
                    bool nestedFailed = false;
                    while (true)
                    {
                        bool nestedNext = false; object nestedValue = null;
                        try { nestedNext = nested.MoveNext(); if (nestedNext) nestedValue = nested.Current; }
                        catch (Exception exception) { report.failure = exception.ToString(); nestedFailed = true; }
                        if (nestedFailed || !nestedNext) break;
                        yield return nestedValue;
                    }
                    if (nestedFailed) break;
                }
                else yield return yielded;
            }
            if (report.final == null && bridge != null) report.final = Capture();
            Finish();
        }
        void Finish()
        {
            if (finishing) return;
            finishing = true;
            if (bridge != null) Supply(Vector2.zero);
            report.passed &= string.IsNullOrEmpty(report.failure) && report.errors_after_handler.Count == 0;
            report.end_frame = Time.frameCount; report.utc_finished = DateTime.UtcNow.ToString("O");
            report.requested_exit_code = report.passed ? 0 : 3;
            File.WriteAllText(Path.Combine(outputDirectory, "original-movement-smoke.json"), JsonUtility.ToJson(report, true));
            Application.logMessageReceived -= ClassifyLog;
            Debug.Log("SFM_OWNED_ORIGINAL_MOVEMENT_SMOKE passed=" + report.passed + "; report=" + outputDirectory + "; bridge recovery delta=0; screenshots require visual review.");
            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.Exit(report.requested_exit_code);
#else
            Application.Quit(report.requested_exit_code);
#endif
        }
        void OnDestroy() { Application.logMessageReceived -= ClassifyLog; }
    }
}
