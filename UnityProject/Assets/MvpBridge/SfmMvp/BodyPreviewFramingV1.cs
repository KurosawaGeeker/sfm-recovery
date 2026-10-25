using System;
using ExposureUnnoticed2.Object3D.Camera;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace SfmMvp
{
    // Owned UI presentation adjustment, not recovered original gameplay.
    // Fits five required bone centres and available toe anchors. Head margin is 12 cm; hands, feet and optional toes use 20 cm.
    // It does not promise complete hair, clothing or skinned-mesh bounds.
    public sealed class BodyPreviewFramingV1 : MonoBehaviour
    {
        const float PaddingMetres = 0.20f;
        const float HeadPaddingMetres = 0.12f;
        const float MaximumFieldOfView = 179f; // Unity perspective FOV boundary, not a fit guarantee.

        InGameManager owner;
        Camera bodyCamera;
        PlayerController actor;
        Animator animator;
        Avatar avatar;
        Transform head, leftHand, rightHand, leftFoot, rightFoot, leftToes, rightToes;
        float originalFov, appliedFov;
        bool configured, ownsFov, suspended, bonesBound;

        public float OriginalFieldOfView { get { return originalFov; } }
        public float RequiredFieldOfView { get; private set; }
        public string LastFitStatus { get; private set; }

        public static void Attach(InGameManager manager)
        {
            if (!manager || manager != InGameManager.Instance || manager.InGameReferencer == null) return;
            var refs = manager.InGameReferencer;
            var target = refs.BodyCameraObject;
            if (!target || !refs.SelfCameraParent || target.transform.parent != refs.SelfCameraParent.transform) return;
            var camera = target.GetComponent<Camera>();
            if (!camera || !target.GetComponent<BodyCameraController>() || camera.orthographic || !camera.targetTexture) return;
            // Persistent manager returns through multiple scene bootstraps. Never recapture an expanded FOV.
            if (target.GetComponent<BodyPreviewFramingV1>()) return;
            float baseline = camera.fieldOfView;
            if (!Finite(baseline) || baseline <= 0f || baseline > MaximumFieldOfView) return;
            var framing = target.AddComponent<BodyPreviewFramingV1>();
            framing.owner = manager;
            framing.bodyCamera = camera;
            framing.originalFov = baseline;
            framing.appliedFov = baseline;
            framing.RequiredFieldOfView = baseline;
            framing.LastFitStatus = "awaiting-body-render";
            framing.configured = true;
        }

        void OnEnable()
        {
            // Recheck ownership after an explicit disable/enable cycle; never adopt an expanded baseline.
            suspended = false;
        }

        void OnDisable() { RestoreOwnedFov(); }
        void OnDestroy() { RestoreOwnedFov(); }

        void RestoreOwnedFov()
        {
            // Do not overwrite a value written by another component after our last assignment.
            if (ownsFov && bodyCamera && bodyCamera.fieldOfView == appliedFov)
                bodyCamera.fieldOfView = originalFov;
            ownsFov = false;
        }

        bool BindActor(PlayerController current)
        {
            if (!current || !current.gameObject.activeInHierarchy || current.Pca == null) return false;
            var currentAnimator = current.Pca.Animator;
            if (!currentAnimator || !currentAnimator.enabled || !currentAnimator.gameObject.activeInHierarchy ||
                !currentAnimator.isHuman || !currentAnimator.avatar || !currentAnimator.avatar.isValid) return false;
            if (actor == current && animator == currentAnimator && avatar == currentAnimator.avatar)
                return bonesBound && head && leftHand && rightHand && leftFoot && rightFoot;
            // One bounded lookup per actual actor/Animator/Avatar replacement, never a global search.
            actor = current;
            animator = currentAnimator;
            avatar = currentAnimator.avatar;
            head = animator.GetBoneTransform(HumanBodyBones.Head);
            leftHand = animator.GetBoneTransform(HumanBodyBones.LeftHand);
            rightHand = animator.GetBoneTransform(HumanBodyBones.RightHand);
            leftFoot = animator.GetBoneTransform(HumanBodyBones.LeftFoot);
            rightFoot = animator.GetBoneTransform(HumanBodyBones.RightFoot);
            leftToes = animator.GetBoneTransform(HumanBodyBones.LeftToes);
            rightToes = animator.GetBoneTransform(HumanBodyBones.RightToes);
            if (leftToes && !OwnBone(leftToes)) leftToes = null;
            if (rightToes && !OwnBone(rightToes)) rightToes = null;
            bonesBound = OwnBone(head) && OwnBone(leftHand) && OwnBone(rightHand) && OwnBone(leftFoot) && OwnBone(rightFoot);
            return bonesBound;
        }

        bool OwnBone(Transform bone)
        {
            return bone && actor && bone.IsChildOf(actor.transform);
        }

        static bool Finite(float value) { return !float.IsNaN(value) && !float.IsInfinity(value); }

        static bool IncludePoint(Vector3 point, float padding, float aspect, float near, float far, ref float tangent)
        {
            if (!Finite(point.x) || !Finite(point.y) || !Finite(point.z)) return false;
            float depth = point.z - padding;
            // Changing FOV cannot repair a point/padding crossing the near/far planes or behind the camera.
            if (depth <= near || point.z + padding >= far) return false;
            float vertical = (Math.Abs(point.y) + padding) / depth;
            float horizontal = (Math.Abs(point.x) + padding) / (depth * aspect);
            if (!Finite(vertical) || !Finite(horizontal)) return false;
            tangent = Math.Max(tangent, Math.Max(vertical, horizontal));
            return true;
        }

        void OnPreCull()
        {
            if (!configured || suspended) return;
            if (!owner || owner != InGameManager.Instance || owner.InGameReferencer == null ||
                !bodyCamera || owner.InGameReferencer.BodyCameraObject != gameObject ||
                !owner.InGameReferencer.SelfCameraParent || transform.parent != owner.InGameReferencer.SelfCameraParent.transform)
            {
                LastFitStatus = "binding-unavailable";
                RestoreOwnedFov();
                return;
            }
            if ((ownsFov && bodyCamera.fieldOfView != appliedFov) || (!ownsFov && bodyCamera.fieldOfView != originalFov))
            {
                ownsFov = false;
                suspended = true;
                LastFitStatus = "external-fov-writer-respected";
                return;
            }
            float aspect = bodyCamera.aspect, near = bodyCamera.nearClipPlane, far = bodyCamera.farClipPlane;
            if (bodyCamera.orthographic || bodyCamera.usePhysicalProperties || !bodyCamera.targetTexture ||
                !Finite(aspect) || aspect <= 0f || !Finite(near) || !Finite(far) || near <= 0f || far <= near ||
                bodyCamera.lensShift != Vector2.zero || !BindActor(owner.InGameReferencer.Player))
            {
                LastFitStatus = "unsupported-camera-or-actor";
                RestoreOwnedFov();
                return;
            }
            // Rotation-only camera coordinates retain metre units even when a parent has scale.
            Quaternion inverse = Quaternion.Inverse(transform.rotation);
            Vector3 position = transform.position;
            float tangent = 0f;
            if (!IncludePoint(inverse * (head.position - position), HeadPaddingMetres, aspect, near, far, ref tangent) ||
                !IncludePoint(inverse * (leftHand.position - position), PaddingMetres, aspect, near, far, ref tangent) ||
                !IncludePoint(inverse * (rightHand.position - position), PaddingMetres, aspect, near, far, ref tangent) ||
                !IncludePoint(inverse * (leftFoot.position - position), PaddingMetres, aspect, near, far, ref tangent) ||
                !IncludePoint(inverse * (rightFoot.position - position), PaddingMetres, aspect, near, far, ref tangent) ||
                (leftToes && !IncludePoint(inverse * (leftToes.position - position), PaddingMetres, aspect, near, far, ref tangent)) ||
                (rightToes && !IncludePoint(inverse * (rightToes.position - position), PaddingMetres, aspect, near, far, ref tangent)))
            {
                LastFitStatus = "sampled-bone-padding-not-projectable";
                RestoreOwnedFov();
                return;
            }
            float required = Math.Max(originalFov, (float)(2.0 * Math.Atan(tangent) * (180.0 / Math.PI)));
            RequiredFieldOfView = required;
            if (!Finite(required) || required > MaximumFieldOfView)
            {
                LastFitStatus = "required-fov-outside-supported-range";
                RestoreOwnedFov();
                return;
            }
            if (required != bodyCamera.fieldOfView) bodyCamera.fieldOfView = required;
            appliedFov = required;
            ownsFov = required != originalFov;
            LastFitStatus = ownsFov ? "fitted-sampled-bone-padding" : "original-fov-sufficient";
        }
    }
}
