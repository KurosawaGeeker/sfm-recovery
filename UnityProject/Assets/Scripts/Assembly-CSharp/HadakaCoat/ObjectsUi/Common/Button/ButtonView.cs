using System;
using ExposureUnnoticed2.Scripts.Sound;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HadakaCoat.ObjectsUi.Common.Button
{
	public class ButtonView : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, ButtonGroupManager.IButtonView
	{
		[SerializeField]
		private InGameSoundManager.SeType seType;

		[SerializeField]
		private bool canHighlighting;

		[SerializeField]
		private float longPressTime;

		[SerializeField]
		private UnityEvent onLongPress;

		[SerializeField]
		private TextMeshProUGUI text;

		[SerializeField]
		private GameObject maskObject;

		[SerializeField]
		private TextMeshProUGUI maskText;

		private Animator animator;

		private UnityEngine.UI.Button button;

		private static readonly int IsHighlighted = Animator.StringToHash("IsHighlighted");

		private static readonly int IsEmit = Animator.StringToHash("IsEmit");

		private Action<ButtonView> sendOnClickNotify;

		private static readonly int IsMasked = Animator.StringToHash("IsMasked");

		private bool isMasked;

		private bool isInitialized;

		private static readonly int ReleaseButton = Animator.StringToHash("ReleaseButton");

		private float downStartTime;

		private bool isPressed;

		private bool isEntered;

		private Action onClick;

		public bool IsEmitting { get; set; }

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x57D6C0
			Initialize();
		}

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x57D6D0
			if (isInitialized) return;
			isInitialized = true;
			animator = GetComponent<Animator>();
			button = GetComponent<UnityEngine.UI.Button>();
			button.onClick.AddListener(OnClick);
		}

		public void InitializeFromButtonGroupManager(Action<ButtonGroupManager.IButtonView> sendOnClickNotify)
		{
			// Recovered from GameAssembly.dll RVA 0x57D870
			Initialize();
			this.sendOnClickNotify = sendOnClickNotify;
		}

		public void SetText(string str)
		{
			// Recovered from GameAssembly.dll RVA 0x57D8E0
			text.text = str;
		}

		public void RegisterAction(Action onClick)
		{
			// Recovered from GameAssembly.dll RVA 0x57D910
			this.onClick += onClick;
		}

		private void OnClick()
		{
			// Recovered from GameAssembly.dll RVA 0x57DA00
			if ((int)seType != 0) UiSoundPlayer.Instance.PlayOneShotAudioData(seType);
			sendOnClickNotify?.Invoke(this);
			onClick?.Invoke();
		}

		public void InvokeOnClick()
		{
			// Recovered from GameAssembly.dll RVA 0x57DAB0
			button.onClick?.Invoke();
		}

		public GameObject GameObject()
		{
			// Recovered from GameAssembly.dll RVA 0x4AADE0
			return gameObject;
		}

		private void OnUpdateSelect(bool isSelected)
		{
		}

		private void Update()
		{
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			// Recovered from GameAssembly.dll RVA 0x57DD30
			isEntered = true;
			animator.SetBool(IsHighlighted, !isMasked && canHighlighting);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			// Recovered from GameAssembly.dll RVA 0x57DE40
			isEntered = false;
			animator.SetBool(IsHighlighted, false);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			// Recovered from GameAssembly.dll RVA 0x57DF00
			downStartTime = Time.time;
			isPressed = true;
		}

		public void OnPointerUp(PointerEventData eventData)
		{
		}

		public void Emit()
		{
			// Recovered from GameAssembly.dll RVA 0x57DF60
			IsEmitting = true;
			animator.SetBool(IsEmit, true);
		}

		public void Extinction()
		{
			// Recovered from GameAssembly.dll RVA 0x57E020
			IsEmitting = false;
			animator.SetBool(IsEmit, false);
		}

		public bool IsMask()
		{
			// Recovered from GameAssembly.dll RVA 0x57E0E0
			return isMasked;
		}

		public void SetMask(bool isMasked, string text = null)
		{
			// Recovered from GameAssembly.dll RVA 0x57E0F0
			maskObject.SetActive(isMasked);
			button.interactable = !isMasked;
			if (text != null) maskText.text = text;
			this.isMasked = isMasked;
		}
	// Recovered original member RVA 0x57E200: .ctor
		public ButtonView()
        {
            seType = (InGameSoundManager.SeType)48;
            canHighlighting = true;
            longPressTime = float.MaxValue;
        }
	// Recovered original member RVA 0x57E260: .cctor
		// Recovered original .cctor RVA 0x57E260: ordered field initializers preserve BeforeFieldInit.
	}
}
