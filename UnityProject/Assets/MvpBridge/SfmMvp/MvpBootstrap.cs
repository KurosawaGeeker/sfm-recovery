using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace SfmMvp
{
    // Temporary MVP integration. This class does not reconstruct an original SFM method.
    public static class MvpBootstrap
    {
        [Serializable]
        private sealed class Milestone
        {
            public string event_name;
            public string detail;
            public int frame;
            public int width;
            public int height;
            public string scene;
            public bool temporary_bridge = true;
        }

        public static void Log(string eventName, string detail)
        {
            Debug.Log("SFM_MVP " + JsonUtility.ToJson(new Milestone
            {
                event_name = eventName, detail = detail, frame = Time.frameCount,
                width = Screen.width, height = Screen.height,
                scene = SceneManager.GetActiveScene().name
            }));
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        public static void AfterSceneLoaded()
        {
#if UNITY_EDITOR
            // Explicit owned Editor fixture selects an empty scene, with no MVP actor bootstrap.
            if (global::System.Array.IndexOf(global::System.Environment.GetCommandLineArgs(), "-sfmNpcAnimationProbeV1") >= 0) return;
#endif
            // Explicit V2 original-lifecycle entry; its reviewed resource adapter owns startup.
            if (UnityEngine.Object.FindObjectOfType<OriginalGameplayBootstrapV2>() != null) return;
            Application.runInBackground = true;
            string[] arguments = Environment.GetCommandLineArgs();
            int bundleArgument = Array.IndexOf(arguments, "-sfmMvpOriginalBundle");
            if (bundleArgument >= 0)
            {
                // Check the native PlayerSettings identity before loading the
                // temporary MVP or producing any persistent evidence.
                Log("runtime_identity", "company=" + Application.companyName +
                    "; product=" + Application.productName +
                    "; persistentDataPath=" + Application.persistentDataPath);
                if (Application.companyName != "SFMRecovery" ||
                    Application.productName != "SFMRecoveryMvpAlpha")
                {
                    Debug.LogError("SFM_MVP owned PlayerSettings identity is not isolated.");
                    Application.Quit(5);
                    return;
                }
                if (bundleArgument + 1 >= arguments.Length)
                { Debug.LogError("SFM_MVP missing owned models.bundle argument"); Application.Quit(2); return; }
                MvpOriginalBundleLoader.Begin(arguments[bundleArgument + 1]);
                return;
            }
            MvpPlayerMotor motor = UnityEngine.Object.FindObjectOfType<MvpPlayerMotor>();
            MvpOrbitCamera view = UnityEngine.Object.FindObjectOfType<MvpOrbitCamera>();
            Log("scene_loaded", "Original Apartment native assets in generated MVP scene; temporary controls.");
            if (motor == null || view == null)
            {
                Debug.LogError("SFM_MVP bridge components missing. Run SfmMvp.Editor.MvpBuild.PrepareScene before building.");
                return;
            }
            int renderers = motor.GetComponentsInChildren<Renderer>(true).Length;
            Log("original_character_present", "renderers=" + renderers);
            view.Bind(motor);
            motor.BindView(view.transform);
            Log("camera_bound", "Third person viewport; source scene and original character remain native asset copies.");
            Application.targetFrameRate = 60;
            PlayerFrameMvpSmoke.TryStart(motor.GetComponent<OriginalMovementMvpBridge>(), view);
        }

        public static void Quit()
        {
            Log("quit_requested", "User requested exit from independent MVP.");
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
