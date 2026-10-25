using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace SfmMvp
{
    // Temporary Unity-executed QA. No report claims pass before real engine frames and capture complete.
    public sealed class MvpSmoke : MonoBehaviour
    {
        [Serializable]
        private sealed class Segment
        {
            public string input;
            public Vector3 before;
            public Vector3 after;
            public float displacement;
            public int start_frame;
            public int end_frame;
        }

        [Serializable]
        private sealed class Evidence
        {
            public bool passed;
            public bool actual_unity_frames_executed;
            public bool temporary_QA_bridge = true;
            public bool original_player_locomotion_reconstructed = false;
            public bool original_save_accessed = false;
            public string failure;
            public string screenshot;
            public string initial_screenshot;
            public string company;
            public string product;
            public int start_frame;
            public int end_frame;
            public int active_renderers;
            public int visible_renderers;
            public int native_scene_colliders;
            public int width;
            public int height;
            public bool controller_grounded;
            public bool native_ground_hit;
            public string native_ground_name;
            public Vector3 initial_position;
            public Vector3 final_position;
            public float largest_segment_displacement;
            public float camera_rotation_degrees;
            public List<Segment> movement = new List<Segment>();
        }

        private MvpPlayerMotor motor;
        private MvpOrbitCamera view;
        private string directory;
        private Evidence evidence;

        public static void TryStart(MvpPlayerMotor motor, MvpOrbitCamera view)
        {
            string[] args = Environment.GetCommandLineArgs();
            if (Array.IndexOf(args, "-sfmMvpSmoke") < 0) return;
            int option = Array.IndexOf(args, "-sfmMvpEvidenceDir");
            if (option < 0 || option + 1 >= args.Length)
            { Debug.LogError("SFM_MVP smoke requires -sfmMvpEvidenceDir <owned directory>"); Application.Quit(2); return; }
            string path = Path.GetFullPath(args[option + 1]);
            if (path.IndexOf("ReverseEngineering", StringComparison.OrdinalIgnoreCase) < 0)
            { Debug.LogError("SFM_MVP smoke evidence must stay in owned ReverseEngineering output."); Application.Quit(2); return; }
            Directory.CreateDirectory(path);
            MvpSmoke smoke = new GameObject("MvpSmokeQa").AddComponent<MvpSmoke>();
            smoke.motor = motor; smoke.view = view; smoke.directory = path;
            motor.SmokeDriven = true;
            smoke.evidence = new Evidence { start_frame=Time.frameCount, company=Application.companyName,
                product=Application.productName, initial_position=motor.transform.position,
                screenshot=Path.Combine(path,"mvp-smoke.png") };
            smoke.StartCoroutine(smoke.Coordinate());
        }

        private IEnumerator Coordinate()
        {
            IEnumerator run = Execute();
            while (true)
            {
                bool next = false; object current = null; Exception failure = null;
                try { next = run.MoveNext(); if (next) current = run.Current; }
                catch (Exception error) { failure = error; }
                if (failure != null) { evidence.failure = failure.ToString(); break; }
                if (!next) break;
                yield return current;
            }
            evidence.end_frame = Time.frameCount;
            evidence.final_position = motor.transform.position;
            evidence.actual_unity_frames_executed = evidence.end_frame > evidence.start_frame + 30;
            evidence.passed = String.IsNullOrEmpty(evidence.failure) && evidence.actual_unity_frames_executed &&
                evidence.largest_segment_displacement > .15f && evidence.camera_rotation_degrees > 10f &&
                evidence.active_renderers > 0 && evidence.visible_renderers > 0 && evidence.native_ground_hit &&
                evidence.width > 0 && evidence.height > 0 && File.Exists(evidence.screenshot) && new FileInfo(evidence.screenshot).Length > 4096;
            File.WriteAllText(Path.Combine(directory,"mvp-smoke.json"), JsonUtility.ToJson(evidence,true));
            MvpBootstrap.Log(evidence.passed ? "smoke_passed" : "smoke_failed", "Evidence="+directory+"; actual frame/Move/camera/capture tests; image still needs visual review.");
            yield return new WaitForSecondsRealtime(.2f);
            Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit(evidence.passed ? 0 : 3);
#endif
        }

        private IEnumerator Execute()
        {
            MvpBootstrap.Log("smoke_started", "Actual Unity frame QA; same temporary movement and look entries.");
            for (int frame = 0; frame < 120; frame++) { motor.Step(Vector2.zero); yield return null; }
            // Extra real-frame QA capture only. Existing pass/fail gates remain unchanged.
            evidence.initial_screenshot = Path.Combine(directory, "mvp-initial.png");
            yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshot(evidence.initial_screenshot);
            for (int frame = 0; frame < 120; frame++)
            {
                if (File.Exists(evidence.initial_screenshot) && new FileInfo(evidence.initial_screenshot).Length > 4096) break;
                motor.Step(Vector2.zero);
                yield return null;
            }
            MvpBootstrap.Log("initial_view_screenshot", "path=" + evidence.initial_screenshot +
                "; exists=" + File.Exists(evidence.initial_screenshot) + "; visual review required.");
            foreach (var pair in new[] { new { Name="W", Value=Vector2.up }, new { Name="D", Value=Vector2.right },
                new { Name="S", Value=Vector2.down }, new { Name="A", Value=Vector2.left } })
            {
                Segment segment = new Segment { input=pair.Name, before=motor.transform.position, start_frame=Time.frameCount };
                for (int frame = 0; frame < 30; frame++) { motor.Step(pair.Value); yield return null; }
                segment.after=motor.transform.position; segment.end_frame=Time.frameCount;
                Vector3 planarDelta=segment.after-segment.before;
                planarDelta.y=0f;
                segment.displacement=planarDelta.magnitude;
                evidence.largest_segment_displacement=Mathf.Max(evidence.largest_segment_displacement,segment.displacement);
                evidence.movement.Add(segment);
            }
            // Candidate-only real-frame QA: observe a stationary return to original idle before camera look.
            for (int frame = 0; frame < 120; frame++) { motor.Step(Vector2.zero); yield return null; }
            Quaternion beforeCamera = view.transform.rotation;
            view.Look(new Vector2(18f,-3f));
            for (int frame = 0; frame < 8; frame++) { motor.Step(Vector2.zero); yield return null; }
            evidence.camera_rotation_degrees=Quaternion.Angle(beforeCamera,view.transform.rotation);
            Renderer[] renderers=motor.GetComponentsInChildren<Renderer>();
            foreach (Renderer renderer in renderers) if (renderer.enabled) { evidence.active_renderers++; if (renderer.isVisible) evidence.visible_renderers++; }
            evidence.native_scene_colliders=UnityEngine.Object.FindObjectsOfType<Collider>().Length;
            evidence.controller_grounded=motor.GetComponent<CharacterController>().isGrounded;
            foreach (RaycastHit hit in Physics.RaycastAll(motor.transform.position+Vector3.up*.5f,Vector3.down,2f,~0,QueryTriggerInteraction.Ignore))
            {
                if (hit.transform==null || hit.transform.IsChildOf(motor.transform)) continue;
                evidence.native_ground_hit=true;evidence.native_ground_name=hit.transform.name;break;
            }
            evidence.width=Screen.width;evidence.height=Screen.height;
            yield return new WaitForEndOfFrame();
            ScreenCapture.CaptureScreenshot(evidence.screenshot);
            float deadline=Time.realtimeSinceStartup+10f;
            while ((!File.Exists(evidence.screenshot) || new FileInfo(evidence.screenshot).Length<=4096) && Time.realtimeSinceStartup<deadline) yield return null;
            if (!File.Exists(evidence.screenshot)) throw new InvalidOperationException("Actual Unity screenshot was not produced.");
        }
    }
}
