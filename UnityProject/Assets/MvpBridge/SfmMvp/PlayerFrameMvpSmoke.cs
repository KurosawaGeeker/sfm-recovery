using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Object3D.Player.Scripts.Other;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace SfmMvp
{
    // Owned input/geometry/one-time state scenario, zero recovered algorithms.
    // Every observed game frame executes the actual complete original main.
    public sealed class PlayerFrameMvpSmoke : MonoBehaviour
    {
        const string Stage = @"E:\ACTgame\SFM\ReverseEngineering\tools\unity-editor\player-animation-mvp-candidate-v1";
        [Serializable] public sealed class Sample
        {
            public string phase; public int frame, updates, fixedCalls, inputUpdates, groundCallbacks, frontCallbacks;
            public Vector3 position, velocity, resolvedMove, cachedPosition, cachedForward, actualForward;
            public bool grounded, frontGrounded, ccGrounded, ready, animatorStrafe;
            public float delta, time, fixedTime, stamina, speed, animatorSpeed, animatorX, animatorY;
            public float footRaw, footState, poseCool, shioTimer, heart, sweat, sweatMaterial, pantsFront, pantsRear;
            public List<PamEngineSmoke.Smooth> pam_smooth = new List<PamEngineSmoke.Smooth>(); public List<PamEngineSmoke.Scalar> pam_parameters = new List<PamEngineSmoke.Scalar>(); public List<PamEngineSmoke.Scalar> pam_layers = new List<PamEngineSmoke.Scalar>(); public float pam_pulse; public string pam_pulse_bits; public int exposed; public string footRawBits, poseBits, shioBits, heartBits, sweatBits, materialBits;
        }
        [Serializable] public sealed class MethodPin { public string method, sha256; }
        [Serializable] public sealed class Result
        {
            public bool passed, actual_Unity_frames = true, queued_input_events = true;
            public bool physical_keyboard_tested, ground_flags_written, target9_hooked, original_bootstrap_recovered;
            public bool saw_ground, saw_forward_edge, saw_airborne, saw_downward_velocity, saw_landing;
            public bool stamina_increased, cache_tracks_real_actor, clothed_exposure, foot_changed, pose_changed, shio_changed, heart_changed, sweat_changed, runtime_method_IL_unchanged;
            public string failure, unity_version, company, product;
            public string scope = "Complete original frame dispatcher + eight helpers on owned neutral Idle/walk state; real input/ground/CC. Actual remaining stub callees execute unchanged. Owned camera/material observer and compatible 14-parameter/11-layer sink graph. Two candidate original PAM bodies upload actual Current values; original graph fidelity remains unverified. Not complete game bootstrap or gameplay.";
            public string seed_scope = "One initial Foot(.6/.8), Pose4, Shio2, Dokidoki.25/nonzero stage seed. No subsequent per-frame repairs or replacement target algorithms.";
            public string placement = "One CC-disabled placement onto remote owned step after Apartment walk; no actor position writes during edge/fall/landing.";
            public string log_scope = "Error categories cover post-TryStart only. Launcher must retain and classify the entire raw process log, including pre-Awake/initializer errors, and record Player exit.";
            public int seed_frame, seed_main_calls, visible_renderers, active_renderers, pam_readback_checks, pam_single_tick_checks, pam_current_target_divergence_witnesses; public bool pam_two_body_pins_verified; public float apartment_input_warmup_realtime; public int apartment_press_calls,apartment_release_calls,platform_press_calls,platform_release_calls;
            public float seed_time, observed_main_delta, apartment_displacement, platform_height_drop;
            public Sample seed, final;
            public List<Sample> samples = new List<Sample>();
            public List<string> error_categories = new List<string>();
            public List<MethodPin> methods_before, methods_after;
        }
        OriginalMovementMvpBridge bridge; MvpOrbitCamera orbit; string output;
        readonly Result result = new Result(); bool done; float started; int lastMainCalls;
        static string Bits(float f) => BitConverter.ToUInt32(BitConverter.GetBytes(f), 0).ToString("X8");
        static void Require(bool condition, string text) { if (!condition) throw new Exception(text); }
        public static void TryStart(OriginalMovementMvpBridge b, MvpOrbitCamera o)
        {
            var args = Environment.GetCommandLineArgs(); if (Array.IndexOf(args, "-sfmPamPlayerSmoke") < 0) return;
            int i = Array.IndexOf(args, "-sfmSmokeOutput"); Require(i >= 0 && i + 1 < args.Length, "Missing owned QA output");
            string p = Path.GetFullPath(args[i + 1]);
            Require(p.StartsWith(Stage + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) && !Directory.Exists(p), "Fresh owned QA output required");
            Directory.CreateDirectory(p);
            var qa = new GameObject("Owned complete player frame QA").AddComponent<PlayerFrameMvpSmoke>();
            qa.bridge = b; qa.orbit = o; qa.output = p; qa.started = Time.realtimeSinceStartup;
            qa.result.unity_version = Application.unityVersion; qa.result.company = Application.companyName; qa.result.product = Application.productName;
            QualitySettings.vSyncCount = 0; Application.targetFrameRate = 60;
            Application.logMessageReceived += qa.Log; qa.StartCoroutine(qa.Run());
        }
        void Log(string message, string stack, LogType type)
        {
            if (type == LogType.Error || type == LogType.Exception || type == LogType.Assert)
                result.error_categories.Add(type + ":" + (stack != null && stack.Contains("OriginalMovementMvpBridge") ? "bridge" : "runtime"));
        }
        void Update() { if (!done && Time.realtimeSinceStartup - started > 50f) Finish("Owned frame QA timeout"); }
        Sample Capture(string phase)
        {
            Require(bridge != null && bridge.Ready, "Bridge failed: " + bridge?.Failure);
            var a = bridge.visibleAnimator;
            var s = new Sample { phase = phase, frame = Time.frameCount, delta = Time.deltaTime, time = Time.time, fixedTime = Time.fixedTime,
                updates = bridge.OriginalUpdateCalls, fixedCalls = bridge.OriginalFixedCalls, inputUpdates = bridge.InputUpdates,
                groundCallbacks = bridge.GroundCallbacks, frontCallbacks = bridge.FrontCallbacks, position = bridge.transform.position,
                actualForward = bridge.transform.forward, velocity = bridge.OriginalMoveVector, resolvedMove = bridge.ResolvedMove,
                cachedPosition = bridge.FrameCachedPosition, cachedForward = bridge.FrameCachedForward, exposed = bridge.FrameCachedExposure,
                grounded = bridge.Grounded, frontGrounded = bridge.FrontGrounded, ccGrounded = bridge.GetComponent<CharacterController>().isGrounded,
                ready = bridge.Ready, stamina = bridge.StaminaCurrent, speed = bridge.OriginalMoveSpeed,
                animatorSpeed = a.GetFloat("Speed"), animatorX = a.GetFloat("StrafeX"), animatorY = a.GetFloat("StrafeY"), animatorStrafe = a.GetBool("IsStrafe"),
                footRaw = bridge.FrameFootRaw, footState = bridge.FrameFootBareyasusa, poseCool = bridge.FramePoseCool, shioTimer = bridge.FrameShioTimer,
                heart = bridge.FrameHeartRate, sweat = bridge.FrameSweatValue, sweatMaterial = bridge.FrameSweatMaterialValue,
                pantsFront = bridge.FramePantsFront, pantsRear = bridge.FramePantsRear };
            s.footRawBits = Bits(s.footRaw); s.poseBits = Bits(s.poseCool); s.shioBits = Bits(s.shioTimer);
            s.heartBits = Bits(s.heart); s.sweatBits = Bits(s.sweat); s.materialBits = Bits(s.sweatMaterial);
            Require(s.updates == s.inputUpdates, "Main and input dispatch must each occur once");
            int newCalls = s.updates - lastMainCalls; Require(newCalls >= 0 && newCalls <= 1, "Unobserved or duplicate original main frames");
            if (newCalls == 1)
            {
                result.observed_main_delta += s.delta;
                Require((s.cachedPosition - s.position).sqrMagnitude < .00000001f && (s.cachedForward - s.actualForward).sqrMagnitude < .00000001f, "Original cached pose differs from real CC actor");
                Require(s.exposed == 1, "Original clothed exposure must remain None1");
                Require(s.sweat == s.sweatMaterial, "Original material scalar upload differs from actual SmoothFloat");
                result.cache_tracks_real_actor = true; result.clothed_exposure = true;
            }
            PamReadback(s,a,newCalls); lastMainCalls = s.updates; result.samples.Add(s); return s;
        }
        const BindingFlags OwnedF = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
        PlayerAnimationManager pam; float previousPulse;
        static object OwnedGet(object o, string n) => o.GetType().GetField(n, OwnedF).GetValue(o);
        static void OwnedSet(object o,string n,object v) => o.GetType().GetField(n,OwnedF).SetValue(o,v);
        [Serializable] sealed class PamPins { public string format,pam_update_il_sha256,pam_helper_il_sha256; public int candidate_methods; }
        void PamSeed()
        {
            pam=(PlayerAnimationManager)OwnedGet(bridge,"animation");
            string p=Path.Combine(Application.dataPath,"owned-pam-two-method-pins.json");
            Require(File.Exists(p)&&new FileInfo(p).Length<32768,"Public two-PAM Player pins missing");
            var pins=JsonUtility.FromJson<PamPins>(File.ReadAllText(p));
            Require(pins!=null&&pins.format=="OwnedPamTwoMethodPins/v1"&&pins.candidate_methods==2,"Public two-PAM pin contract");
            var m=Methods();Require(m.Single(x=>x.method.EndsWith("PlayerAnimationManager::OnUpdate")).sha256==pins.pam_update_il_sha256&&m.Single(x=>x.method.EndsWith("PlayerAnimationManager::UpdateHandcuffsWeight")).sha256==pins.pam_helper_il_sha256,"Actual two PAM method IL pins");
            result.pam_two_body_pins_verified=true;
            OwnedSet(pam,"pulseTargetArmLayerWeight",.75f);((ExposureUnnoticed2.Scripts.Base.SmoothFloat)OwnedGet(pam,"strafeX")).Current=.4f;((ExposureUnnoticed2.Scripts.Base.SmoothFloat)OwnedGet(pam,"strafeY")).Current=-.2f; previousPulse=.75f;
        }
        void PamReadback(Sample s,Animator a,int newCalls)
        {
            s.pam_pulse=(float)OwnedGet(pam,"pulseTargetArmLayerWeight");s.pam_pulse_bits=Bits(s.pam_pulse);
            if(newCalls==1&&previousPulse>0f){float expected=previousPulse-.1f;if(expected<0f)expected=0f;Require(Bits(expected)==s.pam_pulse_bits,"Exactly one original PAM tick");result.pam_single_tick_checks++;}previousPulse=s.pam_pulse;
            foreach(var n in new[]{"coatStateA","coatStateB","headLayerWeight","faceLayerWeight","armLayerWeight","overrideArmForHandcuffLayerWeight","strafeX","strafeY","dokidoki","moveSpeedBlend","handcaffsWeightBlend","anotherMotionIndex","locomotionMotionSpeed"}){var v=(ExposureUnnoticed2.Scripts.Base.SmoothFloat)OwnedGet(pam,n);s.pam_smooth.Add(new PamEngineSmoke.Smooth{name=n,current=v.Current,target=v.Target,current_bits=Bits(v.Current),target_bits=Bits(v.Target)});if(newCalls==1&&Bits(v.Current)!=Bits(v.Target))result.pam_current_target_divergence_witnesses++;}
            foreach(var n in new[]{"CoatState","CoatStateB","Speed","BreastSize","StrafeX","StrafeY","Dokidoki","PlayerEcstasy","MotionSpeedScaleWhenSlow","AnotherMotionIndex","LocomotionMotionSpeed","HandcuffsWeight"}){float v=a.GetFloat(n);s.pam_parameters.Add(new PamEngineSmoke.Scalar{name=n,value=v,bits=Bits(v)});}
            foreach(int i in new List<int>{3,8,10}){float v=a.GetLayerWeight(i);s.pam_layers.Add(new PamEngineSmoke.Scalar{name=i.ToString(),value=v,bits=Bits(v)});}
            if(newCalls==0)return; // Before the first main, collect observations only; no original upload has occurred.
            var map=new Dictionary<string,string>{{"CoatState","coatStateA"},{"CoatStateB","coatStateB"},{"Speed","moveSpeedBlend"},{"StrafeX","strafeX"},{"StrafeY","strafeY"},{"Dokidoki","dokidoki"},{"AnotherMotionIndex","anotherMotionIndex"},{"LocomotionMotionSpeed","locomotionMotionSpeed"},{"HandcuffsWeight","handcaffsWeightBlend"}};
            foreach(var p in map){Require(s.pam_parameters.Single(x=>x.name==p.Key).bits==s.pam_smooth.Single(x=>x.name==p.Value).current_bits,"Real PAM Current upload "+p.Key);result.pam_readback_checks++;}
            var layers=new Dictionary<string,string>{{"3","armLayerWeight"},{"8","overrideArmForHandcuffLayerWeight"},{"10","faceLayerWeight"}};
            foreach(var p in layers){Require(s.pam_layers.Single(x=>x.name==p.Key).bits==s.pam_smooth.Single(x=>x.name==p.Value).current_bits,"Real original PAM layer "+p.Key);result.pam_readback_checks++;}
        }
        List<MethodPin> Methods()
        {
            var selected = new Dictionary<Type, string[]> {
                { typeof(InputManager), new[] { "Awake", "OnUpdate" } }, { typeof(InputResolver), new[] { "Resolve" } },
                { typeof(StaminaManager), new[] { "OnUpdate" } }, { typeof(DownhillChecker), new[] { "Awake", "FixedUpdate", "OnTriggerStay", "get_IsFrontDownhill" } },
                { typeof(PlayerLocomotion), new[] { "OnUpdate", "OnFixedUpdate" } }, { typeof(PlayerController), new[] { "OnUpdate", "OnFixedUpdate" } },
                { typeof(PlayerAnimationManager), new[] { "SetStrafe", "OnUpdate", "UpdateHandcuffsWeight" } }, { typeof(PlayerAction), new[] { "OnFrameEnd" } },
                { typeof(PlayerBodyMaterialController), new[] { "OnUpdate" } }, { typeof(PlayerClothes), new[] { "OnUpdate" } },
                { typeof(PlayerFootStepController), new[] { "OnUpdate" } }, { typeof(PlayerHandcuffsController), new[] { "OnUpdate" } },
                { typeof(PlayerHeartBeat), new[] { "OnUpdate" } }, { typeof(PlayerPantsController), new[] { "OnUpdate" } }, { typeof(PlayerShiofukiController), new[] { "OnUpdate" } }
            };
            var rows = new List<MethodPin>();
            foreach (var type in selected) foreach (string name in type.Value)
            {
                var method = type.Key.GetMethod(name, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
                using (var sha = SHA256.Create()) rows.Add(new MethodPin { method = type.Key.FullName + "::" + name, sha256 = BitConverter.ToString(sha.ComputeHash(method.GetMethodBody().GetILAsByteArray())).Replace("-", "") });
            }
            return rows;
        }
        static void Platform(string name, Vector3 center, Vector3 size, Color color)
        {
            var p = GameObject.CreatePrimitive(PrimitiveType.Cube); p.name = name; p.transform.position = center; p.transform.localScale = size;
            p.GetComponent<Renderer>().material.color = color;
        }
        IEnumerator Scenario()
        {
            Require(bridge != null && bridge.Ready && orbit != null, "Original frame bridge/camera not ready");
            bridge.VerifyOriginalMethodPins(); result.methods_before = Methods();
            lastMainCalls = bridge.OriginalUpdateCalls; bridge.SeedOwnedFrameQaOnce(); PamSeed();
            result.seed_frame = bridge.FrameQaSeedAtFrame; result.seed_time = bridge.FrameQaSeedTime; result.seed_main_calls = lastMainCalls;
            result.seed = Capture("one-time-owned-seed"); bridge.SetOwnedQaInput(Vector2.zero); float inputWarmupStarted=Time.realtimeSinceStartup;
            for (int i = 0; i < 30; i++) { yield return new WaitForEndOfFrame(); Capture("apartment-idle"); }
            while(Time.realtimeSinceStartup-inputWarmupStarted<.35f){yield return new WaitForEndOfFrame();Capture("minimum-input-warmup");} result.apartment_input_warmup_realtime=Time.realtimeSinceStartup-inputWarmupStarted; Require(result.apartment_input_warmup_realtime>=.35f,"Minimum .35s input warmup"); var initial = Capture("apartment-walk-start"); ScreenCapture.CaptureScreenshot(Path.Combine(output, "apartment-start.png"));
            bridge.SetOwnedQaInput(Vector2.up);result.apartment_press_calls++;
            for (int i = 0; i < 55; i++) { yield return new WaitForEndOfFrame(); Capture("apartment-walk"); }
            var moved = Capture("apartment-walk-end"); Vector3 movement = moved.position - initial.position; movement.y = 0f;
            result.apartment_displacement = movement.magnitude; ScreenCapture.CaptureScreenshot(Path.Combine(output, "apartment-walk.png"));
            bridge.SetOwnedQaInput(Vector2.zero);result.apartment_release_calls++;
            for (int i = 0; i < 30; i++) { yield return new WaitForEndOfFrame(); Capture("apartment-stop"); }
            Require(result.apartment_displacement > .05f, "Apartment did not move under actual original main");
            foreach (var r in bridge.GetComponentsInChildren<Renderer>(true)) if (r.enabled && r.gameObject.activeInHierarchy) { result.active_renderers++; if (r.isVisible) result.visible_renderers++; }
            Vector3 center = new Vector3(1000f, 100f, 1000f);
            Platform("Owned upper step", center + Vector3.down * .2f, new Vector3(2f, .4f, 2f), Color.gray);
            Platform("Owned lower landing", center + Vector3.down * 1.1f, new Vector3(20f, .4f, 20f), Color.blue);
            var cc = bridge.GetComponent<CharacterController>(); cc.enabled = false;
            bridge.transform.position = center + Vector3.up * (.05f - cc.center.y + cc.height * .5f); cc.enabled = true;
            Physics.SyncTransforms(); orbit.Bind(bridge.legacyMotor);
            for (int i = 0; i < 35; i++) { yield return new WaitForEndOfFrame(); Capture("platform-settle"); }
            var top = Capture("platform-walk-start"); Require(top.grounded && top.frontGrounded, "Original sensors did not see upper step");
            result.saw_ground = true; bridge.SetOwnedQaInput(Vector2.up);result.platform_press_calls++;
            for (int i = 0; i < 170; i++)
            {
                yield return new WaitForEndOfFrame(); var s = Capture("walk-off-and-land");
                if (s.grounded && !s.frontGrounded) result.saw_forward_edge = true;
                if (!s.grounded && top.position.y - s.position.y > .06f) result.saw_airborne = true;
                if (!s.grounded && s.velocity.y < 0f) result.saw_downward_velocity = true;
                if (result.saw_airborne && s.grounded && top.position.y - s.position.y > .5f) { result.saw_landing = true; result.platform_height_drop = top.position.y - s.position.y; break; }
            }
            bridge.SetOwnedQaInput(Vector2.zero);result.platform_release_calls++;
            for (int i = 0; i < 35; i++) { yield return new WaitForEndOfFrame(); Capture("landed-stop"); }
            ScreenCapture.CaptureScreenshot(Path.Combine(output, "platform-landed.png"));
            for (int i = 0; i < 5; i++) { yield return new WaitForEndOfFrame(); Capture("screenshot-flush"); }
            string[] pngs = { "apartment-start.png", "apartment-walk.png", "platform-landed.png" };
            for (int i = 0; i < 120 && pngs.Any(p => !File.Exists(Path.Combine(output, p)) || new FileInfo(Path.Combine(output, p)).Length == 0); i++) { yield return new WaitForEndOfFrame(); Capture("png-write-wait"); }
            Require(pngs.All(p => File.Exists(Path.Combine(output, p)) && new FileInfo(Path.Combine(output, p)).Length > 0), "Missing three PNG writes");
            result.final = Capture("actual-last-frame"); var f = result.final;
            result.stamina_increased = f.stamina > initial.stamina;
            result.foot_changed = f.footRaw < result.seed.footRaw; result.pose_changed = f.poseCool < result.seed.poseCool;
            result.shio_changed = f.shioTimer < result.seed.shioTimer; result.heart_changed = f.heart > result.seed.heart && f.heart <= 109.251f;
            result.sweat_changed = f.sweat < result.seed.sweat;
            Require(result.saw_forward_edge && result.saw_airborne && result.saw_downward_velocity && result.saw_landing, "Incomplete real edge/fall/landing");
            Require(result.stamina_increased && result.foot_changed && result.pose_changed && result.shio_changed && result.heart_changed && result.sweat_changed, "Missing original helper/stamina observations");
            Require(Mathf.Abs((result.seed.poseCool - result.observed_main_delta) - f.poseCool) < .001f && Mathf.Abs((result.seed.shioTimer - result.observed_main_delta) - f.shioTimer) < .001f, "Actual timer delta history mismatch");
            Require(Mathf.Abs((result.seed.footRaw - result.observed_main_delta * .35f) - f.footRaw) < .001f, "Actual foot raw delta history mismatch");
            Require(f.grounded && f.frontGrounded && f.frontCallbacks > 0 && f.groundCallbacks > 0 && f.fixedCalls > initial.fixedCalls, "Missing actual physics dispatch/landing");
            Require(result.active_renderers == 4 && result.visible_renderers > 0 && result.clothed_exposure && result.cache_tracks_real_actor, "Visible clothed actor/cache missing");
            Require(f.resolvedMove == Vector3.zero && new Vector2(f.velocity.x, f.velocity.z) == Vector2.zero && f.speed == 0f, "Final actual original movement must stop");
            Require(f.animatorSpeed == 0f && !f.animatorStrafe && f.pam_smooth.Single(x=>x.name=="strafeX").target==0f && f.pam_smooth.Single(x=>x.name=="strafeY").target==0f, "Original final Speed/Strafe state and targets must stop; smoothed Current residuals are recorded unchanged"); Require(bridge.visibleAnimator.GetCurrentAnimatorClipInfo(0).Any(x=>x.clip!=null&&x.clip.name.Contains("Idle")),"Original visible idle evaluates at stopped Speed");
            bridge.VerifyOriginalMethodPins(); result.methods_after = Methods();
            result.runtime_method_IL_unchanged = result.methods_before.Count == result.methods_after.Count && result.methods_before.Zip(result.methods_after, (a, b) => a.method == b.method && a.sha256 == b.sha256).All(x => x);
            Require(result.runtime_method_IL_unchanged, "Original method IL changed"); Require(result.apartment_press_calls==1&&result.apartment_release_calls==1&&result.platform_press_calls==1&&result.platform_release_calls==1,"Exactly one press/release per existing QA phase"); Require(result.pam_single_tick_checks>=7&&result.pam_current_target_divergence_witnesses>0,"Missing single-tick/Current versus Target PAM evidence");
        }
        IEnumerator Run()
        {
            var sequence = Scenario();
            while (!done)
            {
                bool more = false; object current = null; string failure = null;
                try { more = sequence.MoveNext(); if (more) current = sequence.Current; } catch (Exception e) { failure = e.ToString(); }
                if (failure != null) { Finish(failure); yield break; }
                if (!more) { Finish(null); yield break; } yield return current;
            }
        }
        void Finish(string failure)
        {
            if (done) return; done = true; result.failure = failure; result.passed = failure == null && result.error_categories.Count == 0;
            if (bridge != null && bridge.Ready) bridge.ReleaseOwnedQaInput();
            File.WriteAllText(Path.Combine(output, "pam-player-smoke.json"), JsonUtility.ToJson(result, true));
            Application.logMessageReceived -= Log; Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
            Application.Quit(result.passed ? 0 : 3);
        }
    }
}
