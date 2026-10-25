using System;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.FadeOutPanel
{
	public class FadePanelView : MonoBehaviour
	{
		[SerializeField]
		private GameObject nowLoadingObject;

		private Action onPureBlack;

		private Action onStartFadeIn;

		private Action onCompleteFade;

		private IFadeInStarter starter;

		private bool isAlreadyOnPureBlack;

		private bool isAlreadyOnTrigger;

		private Animator animator;

		private static readonly int StartFadeIn = Animator.StringToHash("StartFadeIn");

		public static FadePanelView Instance { get; private set; }

		private void Initialize(IFadeInStarter starter, Action onPureBlack, Action onStartFadeIn, Action onCompleteFade, bool isShowNowLoading)
		{
			// Recovered from GameAssembly.dll RVA 0x513450
			Instance = this;
			this.starter = starter;
			this.onPureBlack = onPureBlack;
			this.onStartFadeIn = onStartFadeIn;
			this.onCompleteFade = onCompleteFade;
			nowLoadingObject.SetActive(isShowNowLoading);
			animator = GetComponent<Animator>();
		}

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x5136F0
			if (isAlreadyOnPureBlack && starter.IsStart() && !isAlreadyOnTrigger)
			{
			    // Original compiler-generated callback RVA 0x513AD0.
			    global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
			    {
			        animator.SetTrigger(StartFadeIn);
			        onStartFadeIn?.Invoke();
			    }, 1);
			    isAlreadyOnTrigger = true;
			}
		}

		public void OnPureBlack()
		{
			// Recovered from GameAssembly.dll RVA 0x5137C0
			isAlreadyOnPureBlack = true;
			onPureBlack?.Invoke();
		}

		public void OnExitAnim()
		{
			// Recovered from GameAssembly.dll RVA 0x5137E0
			Instance = null;
			onCompleteFade?.Invoke();
			UnityEngine.Object.Destroy(gameObject);
		}

		public static void Open(IFadeInStarter starter, Action onPureBlack, Action onStartFadeIn = null, Action onCompleteFade = null, bool isShowNowLoading = true)
		{
			// Recovered from GameAssembly.dll RVA 0x5138C0
			GameObject obj = UnityEngine.Object.Instantiate(global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.FadePanel);
			obj.GetComponentInChildren<FadePanelView>().Initialize(starter, onPureBlack, onStartFadeIn, onCompleteFade, isShowNowLoading);
			UnityEngine.Object.DontDestroyOnLoad(obj);
		}
	}
}
