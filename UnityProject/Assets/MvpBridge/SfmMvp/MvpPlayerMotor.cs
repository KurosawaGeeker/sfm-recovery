using UnityEngine;

namespace SfmMvp
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class MvpPlayerMotor : MonoBehaviour
    {
        // Self-owned temporary controls. Original InputResolver/PlayerController/Locomotion are not invoked.
        public float walkSpeed = 2.5f;
        public float gravity = -20f;
        private CharacterController controller;
        private Transform view;
        private float verticalSpeed;
        private bool updateLogged;
        private bool movementLogged;
        public bool ControlsCaptured { get; set; }
        public bool SmokeDriven { get; set; }

        public void BindView(Transform cameraTransform) { view = cameraTransform; }

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            if (controller == null) { enabled = false; Debug.LogError("SFM_MVP CharacterController missing."); }
        }

        private void Update()
        {
            if (controller == null || !controller.enabled) return;
            if (!updateLogged)
            {
                updateLogged = true;
                MvpBootstrap.Log("controller_update", "CharacterController enabled; native scene collision, temporary motor.");
            }
            if (SmokeDriven) return;
            float x = 0f, z = 0f;
            if (ControlsCaptured)
            {
                x = (Input.GetKey(KeyCode.D) ? 1f : 0f) - (Input.GetKey(KeyCode.A) ? 1f : 0f);
                z = (Input.GetKey(KeyCode.W) ? 1f : 0f) - (Input.GetKey(KeyCode.S) ? 1f : 0f);
            }
            Step(new Vector2(x, z));
        }

        // QA feeds this same temporary movement entry; it never writes the player's position directly.
        public void Step(Vector2 input)
        {
            if (controller == null || !controller.enabled) return;
            Vector3 forward = view != null ? view.forward : Vector3.forward;
            forward.y = 0f;
            if (forward.sqrMagnitude < .001f) forward = Vector3.forward;
            forward.Normalize();
            Vector3 right = Vector3.Cross(Vector3.up, forward);
            Vector3 direction = forward * input.y + right * input.x;
            if (direction.sqrMagnitude > 1f) direction.Normalize();
            if (direction.sqrMagnitude > .001f) transform.rotation = Quaternion.LookRotation(direction, Vector3.up);
            if (controller.isGrounded && verticalSpeed < 0f) verticalSpeed = -2f;
            float dt = Mathf.Min(Time.deltaTime, .05f);
            verticalSpeed = Mathf.Max(verticalSpeed + gravity * dt, -40f);
            Vector3 before = transform.position;
            CollisionFlags collision = controller.Move((direction * walkSpeed + Vector3.up * verticalSpeed) * dt);
            if ((collision & CollisionFlags.Below) != 0 && verticalSpeed < 0f) verticalSpeed = -2f;
            if (!movementLogged && direction.sqrMagnitude > .001f && (transform.position - before).sqrMagnitude > .000001f)
            {
                movementLogged = true;
                MvpBootstrap.Log("movement_observed", "Nonzero WASD displacement; position=" + transform.position.ToString("F3"));
            }
        }
    }
}
