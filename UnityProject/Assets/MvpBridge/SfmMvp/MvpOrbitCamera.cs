using UnityEngine;
using UnityEngine.Rendering;

namespace SfmMvp
{
    [RequireComponent(typeof(Camera))]
    public sealed class MvpOrbitCamera : MonoBehaviour
    {
        public bool useOriginalInput;
        public float distance = 2.2f;
        public float focusHeight = 1.15f;
        public float sensitivity = 2f;
        public float pitch = 18f;
        private float yaw;
        private MvpPlayerMotor player;
        private bool cameraLogged;
        private bool lookLogged;
        private Renderer[] playerRenderers;
        private ShadowCastingMode[] originalShadowModes;
        private bool nearCameraHidden;

        public void Look(Vector2 input)
        {
            yaw += input.x * sensitivity;
            pitch = Mathf.Clamp(pitch - input.y * sensitivity, -25f, 65f);
        }

        public void Bind(MvpPlayerMotor motor)
        {
            RestorePlayerRendering();
            player = motor;
            playerRenderers = player.GetComponentsInChildren<Renderer>(true);
            originalShadowModes = new ShadowCastingMode[playerRenderers.Length];
            for (int i = 0; i < playerRenderers.Length; i++)
                originalShadowModes[i] = playerRenderers[i].shadowCastingMode;
            yaw = player.transform.eulerAngles.y;
            // One initial selection only. Subsequent mouse/QA Look calls own the orbit.
            Physics.SyncTransforms();
            Vector3 focus = player.transform.position + Vector3.up * focusHeight;
            float initialYaw = yaw;
            float bestDistance = -1f;
            for (int direction = 0; direction < 8; direction++)
            {
                float candidateYaw = initialYaw + direction * 45f;
                Vector3 candidateDirection = -(Quaternion.Euler(pitch, candidateYaw, 0f) * Vector3.forward);
                float freeDistance = FreeViewDistance(focus, candidateDirection);
                if (freeDistance > bestDistance)
                { bestDistance = freeDistance; yaw = candidateYaw; }
            }
            MvpBootstrap.Log("initial_camera_view", "yaw=" + yaw.ToString("F1") +
                "; freeDistance=" + bestDistance.ToString("F2") + "; focusHeight=" + focusHeight.ToString("F2") +
                "; testedDirections=8; one initial choice only.");
            Capture(false);
        }

        private void Capture(bool captured)
        {
            Cursor.lockState = captured ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !captured;
            if (player != null) player.ControlsCaptured = captured;
            MvpBootstrap.Log(captured ? "input_captured" : "input_released", "Esc releases cursor; second Esc or Quit exits.");
        }

        private void Update()
        {
            if (player == null) return;
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (player.ControlsCaptured) Capture(false); else MvpBootstrap.Quit();
                return;
            }
            if (Input.GetMouseButtonDown(0) && !player.ControlsCaptured && Input.mousePosition.y < Screen.height - 105 && !CoreGameplayPanel.PointerOverPanel)
                Capture(true);
            if (!player.ControlsCaptured || useOriginalInput) return;
            float x = Input.GetAxisRaw("Mouse X"), y = Input.GetAxisRaw("Mouse Y");
            Look(new Vector2(x, y));
            if (!lookLogged && (Mathf.Abs(x) + Mathf.Abs(y)) > .001f)
            { lookLogged = true; MvpBootstrap.Log("mouse_look_observed", "Temporary orbit changed from real mouse input."); }
        }

        private void LateUpdate()
        {
            if (player == null) return;
            Vector3 focus = player.transform.position + Vector3.up * focusHeight;
            Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);
            Vector3 direction = -(rotation * Vector3.forward);
            float actualDistance = FreeViewDistance(focus, direction);
            // Hysteresis prevents flicker at walls. No active/enabled flag is changed.
            if (!nearCameraHidden && actualDistance < .8f) SetNearCameraHidden(true, actualDistance);
            else if (nearCameraHidden && actualDistance > 1f) SetNearCameraHidden(false, actualDistance);
            transform.SetPositionAndRotation(focus + direction * actualDistance, rotation);
            if (!cameraLogged)
            { cameraLogged = true; MvpBootstrap.Log("viewport_update", "camera=" + GetComponent<Camera>().name + "; mouse orbit available."); }
        }

        private float FreeViewDistance(Vector3 focus, Vector3 direction)
        {
            float freeDistance = distance;
            foreach (RaycastHit hit in Physics.SphereCastAll(focus, .12f, direction, distance, ~0, QueryTriggerInteraction.Ignore))
            {
                if (hit.transform == null || hit.transform.IsChildOf(player.transform)) continue;
                freeDistance = Mathf.Min(freeDistance, Mathf.Max(.25f, hit.distance - .08f));
            }
            return freeDistance;
        }

        private void SetNearCameraHidden(bool hidden, float actualDistance)
        {
            if (hidden)
            {
                for (int i = 0; i < playerRenderers.Length; i++)
                {
                    Renderer renderer = playerRenderers[i];
                    if (renderer != null && renderer.enabled && renderer.gameObject.activeInHierarchy)
                        renderer.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
                }
                nearCameraHidden = true;
            }
            else RestorePlayerRendering();
            MvpBootstrap.Log(hidden ? "near_camera_player_shadows_only" : "near_camera_player_rendering_restored",
                "distance=" + actualDistance.ToString("F2") + "; original renderer modes retained; no active/enabled changes.");
        }

        private void RestorePlayerRendering()
        {
            if (!nearCameraHidden || playerRenderers == null || originalShadowModes == null) return;
            for (int i = 0; i < playerRenderers.Length; i++)
                if (playerRenderers[i] != null) playerRenderers[i].shadowCastingMode = originalShadowModes[i];
            nearCameraHidden = false;
        }

        private void OnGUI()
        {
            GUI.Box(new Rect(10, 10, 470, 95), "SFM recovery MVP - walk and explore");
            GUI.Label(new Rect(20, 36, 450, 22), "Click world: capture mouse | WASD: move | Esc: release / exit");
            if (GUI.Button(new Rect(20, 66, 160, 27), "Capture controls")) Capture(true);
            if (GUI.Button(new Rect(195, 66, 120, 27), "Quit MVP")) MvpBootstrap.Quit();
        }

        private void OnDisable()
        {
            RestorePlayerRendering();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            if (player != null) player.ControlsCaptured = false;
        }
    }
}
