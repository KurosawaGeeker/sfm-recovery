using UnityEngine;

namespace ExposureUnnoticed2.ObjectTitle
{
	public class IntroAnimationController : MonoBehaviour
	{
		private enum State
		{
			FadeIn = 0,
			ShowName = 1,
			FadeOut = 2,
			WaitLoad = 3,
			TransTitle = 4
		}

		private State currentState;

		private Animator animator;

		public bool StartTitle { get; private set; }

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x5EA390
			animator = GetComponent<Animator>();
		}

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x5EA420
			if (currentState == State.ShowName)
			{
			    if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.LeftClick)
			        || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.Cancel)
			        || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame(global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIDecide))
			        OnStartFadeOut();
			}
			else if (currentState == State.WaitLoad)
			{
			    if (global::ExposureUnnoticed2.Scripts.UI.TitleSceneView.Instance.IsEndLoadCount < 25) return;
			    animator.SetTrigger("TransTitle");
			    currentState = State.TransTitle;
			}
		}

		public void Cancel()
		{
			// Recovered from GameAssembly.dll RVA 0x5EA5D0
			StartTitle = true;
			gameObject.SetActive(false);
		}

		public void OnShowName()
		{
			// Recovered from GameAssembly.dll RVA 0x5EA680
			currentState = State.ShowName;
		}

		public void OnStartFadeOut()
		{
			// Recovered from GameAssembly.dll RVA 0x5EA690
			if (currentState != State.ShowName) return;
			currentState = State.FadeOut;
			animator.SetTrigger("End");
		}

		public void OnEndFadeOut()
		{
			// Recovered from GameAssembly.dll RVA 0x5EA730
			currentState = State.WaitLoad;
		}

		public void OnEndFadeInTitle()
		{
			// Recovered from GameAssembly.dll RVA 0x5EA5D0
			StartTitle = true;
			gameObject.SetActive(false);
		}
	}
}
