using System;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.MessagePanel
{
	public class MessageItemView : MonoBehaviour
	{
		private const float DisplayTime = 3f;

		private const float RankUpDisplayTime = 5f;

		[SerializeField]
		private TextMeshProUGUI message;

		private GameObject targetDummy;

		private CanvasGroup canvas;

		private Animator animator;

		private float startTime;

		private bool isCloseProcessed;

		private bool isDestroyProcessed;

		private Action<MessageItemView> onDestroy;

		private float displayTime;

		public void Initialize(string str, GameObject targetDummy, Action<MessageItemView> onDestroy, MessagePanelView.MessageType type)
		{
			this.onDestroy = onDestroy;
			this.targetDummy = targetDummy;
			canvas = GetComponent<CanvasGroup>();
			canvas.alpha = 0f;
			message.text = str;
			animator = GetComponent<Animator>();
			startTime = Time.time;
			global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
			{
			    transform.position = global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetSetY(transform.position, targetDummy.transform.position.y);
			}, 1);
			displayTime = type == MessagePanelView.MessageType.RankUp ? RankUpDisplayTime : DisplayTime;
		}

		private void LateUpdate()
		{
			if (isCloseProcessed) return;
			transform.position = Vector3.Lerp(transform.position, targetDummy.transform.position, 0.1f);
			if (Time.time - startTime > displayTime)
			{
			    UnityEngine.Object.Destroy(targetDummy);
			    isCloseProcessed = true;
			    animator.SetTrigger("Close");
			}
		}

		public void ForceClose()
		{
			startTime = -999f;
		}

		public void OnAnimEndClose()
		{
			if (isDestroyProcessed) return;
			isDestroyProcessed = true;
			UnityEngine.Object.Destroy(gameObject);
			onDestroy?.Invoke(this);
		}
	}
}
