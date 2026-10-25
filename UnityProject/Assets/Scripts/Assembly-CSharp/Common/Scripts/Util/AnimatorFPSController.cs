using UnityEngine;

namespace Common.Scripts.Util
{
	public class AnimatorFPSController
	{
		private bool isActive;

		private int _fps = 5;

		private Animator _animator;

		private float _thresholdTime;

		private float _skippedTime;

		public void Initialize(Animator animator)
		{
			_animator = animator;
		}

		public void Active(int fps = -1)
		{
			if (fps != -1)
			{
			    _fps = fps;
			    InitializeThresholdTime();
			}
			if (!isActive)
			{
			    UnityEngine.Animator animator = _animator;
			    isActive = true;
			    animator.enabled = false;
			    _skippedTime = 0f;
			    InitializeThresholdTime();
			}
		}

		public void DeActive()
		{
			if (!isActive) return;
			UnityEngine.Animator animator = _animator;
			isActive = false;
			animator.enabled = true;
		}

		private void InitializeThresholdTime()
		{
			_thresholdTime = 1f / (float)_fps;
		}

		public void OnUpdate()
		{
			if (!isActive) return;
			float skipped = _skippedTime;
			float delta = UnityEngine.Time.deltaTime;
			float next = delta + skipped;
			float threshold = _thresholdTime;
			_skippedTime = next;
			if (threshold > next) return;
			_animator.Update(next);
			_skippedTime = 0f;
		}
	}
}
