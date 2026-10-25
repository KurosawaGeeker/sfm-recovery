using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace SfmMvp
{
    // Read-only actual Unity playback observations. It does not drive the motor, camera or animation graph.
    [DefaultExecutionOrder(200)]
    public sealed class MvpAnimationPlaybackEvidence : MonoBehaviour
    {
        [Serializable] private sealed class Phase
        {
            public string name, screenshot;
            public int first_frame, last_frame, observations, pose_samples, human_bones, enabled_renderers;
            public float min_walk_weight = 1f, max_walk_weight, max_bone_position_delta, max_bone_rotation_degrees;
            public float max_skin_vertex_delta;
        }
        [Serializable] private sealed class Report
        {
            public bool passed, actual_Unity_playback = true, temporary_driver = true;
            public int original_methods_restored = 0;
            public bool original_Controller_restored = false, clip_events_modified = false;
            public bool instance_fireEvents, instance_applyRootMotion;
            public string failure, unity_version, avatar_name, idle_clip, walk_clip;
            public float idle_length, walk_length;
            public List<Phase> phases = new List<Phase>();
        }
        private sealed class Snapshot
        {
            public Vector3[] positions; public Quaternion[] rotations;
            public Vector3[][] vertices;
        }
        private MvpOriginalIdleWalkMixer driver;
        private MvpPlayerMotor motor;
        private FieldInfo weightField;
        private Transform[] bones;
        private SkinnedMeshRenderer[] skins;
        private readonly Dictionary<string, Snapshot> first = new Dictionary<string, Snapshot>();
        private readonly Dictionary<string, Phase> phases = new Dictionary<string, Phase>();
        private Report report;
        private string directory;
        private bool observedWalk;
        private int lastSample;

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void StartEvidence()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (Array.IndexOf(args, "-sfmMvpSmoke") < 0) return;
            int index = Array.IndexOf(args, "-sfmMvpEvidenceDir");
            if (index < 0 || index + 1 >= args.Length) return;
            var observer = new GameObject("MvpOriginalAnimationReadOnlyQa").AddComponent<MvpAnimationPlaybackEvidence>();
            observer.directory = Path.GetFullPath(args[index + 1]);
            if (observer.directory.IndexOf("mvp-idle-walk-candidate-v1", StringComparison.OrdinalIgnoreCase) < 0)
                throw new InvalidOperationException("Animation evidence must stay in this owned candidate stage.");
            observer.report = new Report { unity_version = Application.unityVersion };
            try
            {
                observer.driver = UnityEngine.Object.FindObjectsOfType<MvpOriginalIdleWalkMixer>().Single();
                observer.motor = observer.driver.GetComponent<MvpPlayerMotor>();
                observer.weightField = typeof(MvpOriginalIdleWalkMixer).GetField("walkWeight", BindingFlags.NonPublic | BindingFlags.Instance);
                Animator animator = observer.driver.target;
                observer.bones = Enumerable.Range(0, (int)HumanBodyBones.LastBone)
                    .Select(i => animator.GetBoneTransform((HumanBodyBones)i)).Where(t => t != null).ToArray();
                observer.skins = observer.motor.GetComponentsInChildren<SkinnedMeshRenderer>()
                    .Where(r => r.enabled && r.gameObject.activeInHierarchy).ToArray();
                observer.report.avatar_name = animator.avatar.name;
                observer.report.idle_clip = observer.driver.originalIdle.name;
                observer.report.walk_clip = observer.driver.originalWalk.name;
                observer.report.idle_length = observer.driver.originalIdle.length;
                observer.report.walk_length = observer.driver.originalWalk.length;
                foreach (string name in new[] { "initial_idle", "walking", "returned_idle" })
                {
                    var phase = new Phase { name = name, screenshot = Path.Combine(observer.directory, "animation-" + name + ".png") };
                    observer.phases.Add(name, phase); observer.report.phases.Add(phase);
                }
            }
            catch (Exception error) { observer.report.failure = error.ToString(); observer.enabled = false; }
        }

        private Snapshot ObservePose()
        {
            var result = new Snapshot { positions = bones.Select(t => t.localPosition).ToArray(),
                rotations = bones.Select(t => t.localRotation).ToArray(), vertices = new Vector3[skins.Length][] };
            for (int i = 0; i < skins.Length; i++)
            {
                Mesh mesh = new Mesh();
                try { skins[i].BakeMesh(mesh); result.vertices[i] = mesh.vertices; }
                finally { Destroy(mesh); }
            }
            return result;
        }

        private IEnumerator Capture(Phase phase)
        {
            yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshot(phase.screenshot);
        }

        private void LateUpdate()
        {
            try
            {
                float weight = (float)weightField.GetValue(driver);
                string name = weight > .95f ? "walking" : weight < .05f ? observedWalk ? "returned_idle" : "initial_idle" : null;
                if (name == null) return;
                if (name == "walking") observedWalk = true;
                Phase phase = phases[name];
                if (phase.observations++ == 0) phase.first_frame = Time.frameCount;
                phase.last_frame = Time.frameCount;
                phase.min_walk_weight = Mathf.Min(phase.min_walk_weight, weight);
                phase.max_walk_weight = Mathf.Max(phase.max_walk_weight, weight);
                if (Time.frameCount - lastSample < 15) return;
                lastSample = Time.frameCount;
                Snapshot pose = ObservePose();
                phase.pose_samples++; phase.human_bones = bones.Length;
                phase.enabled_renderers = skins.Count(r => r.enabled && r.gameObject.activeInHierarchy);
                Snapshot initial;
                if (!first.TryGetValue(name, out initial)) { first.Add(name, pose); return; }
                for (int i = 0; i < bones.Length; i++)
                {
                    phase.max_bone_position_delta = Mathf.Max(phase.max_bone_position_delta, Vector3.Distance(initial.positions[i], pose.positions[i]));
                    phase.max_bone_rotation_degrees = Mathf.Max(phase.max_bone_rotation_degrees, Quaternion.Angle(initial.rotations[i], pose.rotations[i]));
                }
                for (int i = 0; i < skins.Length; i++)
                {
                    if (initial.vertices[i].Length != pose.vertices[i].Length) throw new InvalidOperationException("Original mesh vertex count changed.");
                    for (int v = 0; v < pose.vertices[i].Length; v++)
                        phase.max_skin_vertex_delta = Mathf.Max(phase.max_skin_vertex_delta, Vector3.Distance(initial.vertices[i][v], pose.vertices[i][v]));
                }
                if (phase.pose_samples == 2) StartCoroutine(Capture(phase));
            }
            catch (Exception error) { report.failure = error.ToString(); enabled = false; }
        }

        private void OnApplicationQuit()
        {
            if (report == null) return;
            try
            {
                if (driver != null && driver.target != null)
                {
                    report.instance_fireEvents = driver.target.fireEvents;
                    report.instance_applyRootMotion = driver.target.applyRootMotion;
                }
                report.passed = String.IsNullOrEmpty(report.failure) && !report.instance_fireEvents && !report.instance_applyRootMotion &&
                    report.phases.Count == 3 && report.phases.All(p => p.pose_samples >= 2 && p.human_bones >= 50 &&
                    p.max_bone_rotation_degrees > .001f && p.max_skin_vertex_delta > .00001f &&
                    File.Exists(p.screenshot) && new FileInfo(p.screenshot).Length > 4096);
                File.WriteAllText(Path.Combine(directory, "animation-playback.json"), JsonUtility.ToJson(report, true));
            }
            catch (Exception error) { Debug.LogError("SFM_MVP animation evidence save failed: " + error); }
        }
    }
}
