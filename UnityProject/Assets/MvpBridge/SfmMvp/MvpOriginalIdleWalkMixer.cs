using System;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace SfmMvp
{
    // Temporary MVP playback only. Original controller, state machine and event handlers are not restored.
    public sealed class MvpOriginalIdleWalkMixer : MonoBehaviour
    {
        public Animator target;
        public AnimationClip originalIdle;
        public AnimationClip originalWalk;
        public float temporaryFullWalkSpeed = 2.5f;
        private PlayableGraph graph;
        private AnimationMixerPlayable mixer;
        private Vector3 lastPosition;
        private float walkWeight;
        private bool stateCaptured, oldEnabled, oldEvents, oldRootMotion;
        private AnimatorCullingMode oldCulling;
        private RuntimeAnimatorController oldController;

        private void OnEnable()
        {
            if (!Application.isPlaying) return;
            try
            {
                if (target == null || target.avatar == null || !target.avatar.isValid || !target.avatar.isHuman)
                    throw new InvalidOperationException("Original valid humanoid Avatar/Animator required.");
                if (originalIdle == null || originalWalk == null || !originalIdle.isHumanMotion || !originalWalk.isHumanMotion)
                    throw new InvalidOperationException("Audited original humanoid idle/walk clips required.");
                if (!(temporaryFullWalkSpeed > 0f)) throw new InvalidOperationException("Temporary motor speed must be positive.");
                oldEnabled = target.enabled; oldEvents = target.fireEvents; oldRootMotion = target.applyRootMotion;
                oldController = target.runtimeAnimatorController; oldCulling = target.cullingMode; stateCaptured = true;
                target.runtimeAnimatorController = null;
                target.enabled = true;
                target.applyRootMotion = false;
                target.fireEvents = false; // Instance only; both original clip event tables remain intact.
                target.cullingMode = AnimatorCullingMode.AlwaysAnimate;
                target.Rebind();
                graph = PlayableGraph.Create("SFM_MVP_OriginalIdleWalk_TemporaryMixer");
                graph.SetTimeUpdateMode(DirectorUpdateMode.Manual);
                mixer = AnimationMixerPlayable.Create(graph, 2, true);
                AnimationClipPlayable idle = AnimationClipPlayable.Create(graph, originalIdle);
                AnimationClipPlayable walk = AnimationClipPlayable.Create(graph, originalWalk);
                idle.SetApplyFootIK(false); walk.SetApplyFootIK(false);
                graph.Connect(idle, 0, mixer, 0); graph.Connect(walk, 0, mixer, 1);
                mixer.SetInputWeight(0, 1f); mixer.SetInputWeight(1, 0f);
                AnimationPlayableOutput output = AnimationPlayableOutput.Create(graph, "Original humanoid Avatar", target);
                output.SetSourcePlayable(mixer);
                lastPosition = transform.position; walkWeight = 0f;
                graph.Play(); graph.Evaluate(0f);
                Debug.Log("SFM_MVP original_idle_walk temporary_mixer=true original_events_retained=true fireEvents=false controller_restored=false");
            }
            catch (Exception error)
            {
                Debug.LogError("SFM_MVP original_idle_walk unavailable: " + error);
                Release(); enabled = false;
            }
        }

        private void LateUpdate()
        {
            if (!graph.IsValid()) return;
            float dt = Time.deltaTime;
            Vector3 delta = transform.position - lastPosition; delta.y = 0f;
            lastPosition = transform.position;
            // Observes the existing temporary motor; does not inject input, alter collision or move the root.
            float speed = dt > 0f ? delta.magnitude / dt : 0f;
            float requested = Mathf.Clamp01(speed / temporaryFullWalkSpeed);
            walkWeight = Mathf.MoveTowards(walkWeight, requested, Mathf.Max(0f, dt) * 8f);
            mixer.SetInputWeight(0, 1f - walkWeight); mixer.SetInputWeight(1, walkWeight);
            graph.Evaluate(Mathf.Max(0f, dt));
        }

        private void Release()
        {
            if (graph.IsValid()) graph.Destroy();
            if (stateCaptured && target != null)
            {
                target.runtimeAnimatorController = oldController;
                target.applyRootMotion = oldRootMotion; target.fireEvents = oldEvents;
                target.cullingMode = oldCulling; target.enabled = oldEnabled;
            }
            stateCaptured = false;
        }
        private void OnDisable() { Release(); }
        private void OnDestroy() { Release(); }
    }
}
