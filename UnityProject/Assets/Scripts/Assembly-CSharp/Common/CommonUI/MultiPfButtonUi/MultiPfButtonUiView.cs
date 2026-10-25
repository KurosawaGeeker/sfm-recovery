using ExposureUnnoticed2.Scripts.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.CommonUI.MultiPfButtonUi
{
	public class MultiPfButtonUiView : MonoBehaviour
	{
		public enum DisplayPlatform
		{
			None = -1,
			Windows = 0,
			PS4 = 1,
			Xbox = 2
		}

		public static DisplayPlatform CurrentPlatform;

		[SerializeField]
		private bool isOnlyShowPad;

		[SerializeField]
		private TextMeshProUGUI text;

		[SerializeField]
		private Image buttonImage;

		[SerializeField]
		private RectTransform buttonImageRect;

		[SerializeField]
		private InputManager.InputType inputType;

		[SerializeField]
		private Sprite keyboard;

		[SerializeField]
		private Sprite[] North;

		[SerializeField]
		private Sprite[] South;

		[SerializeField]
		private Sprite[] East;

		[SerializeField]
		private Sprite[] West;

		[SerializeField]
		private Sprite[] Select;

		[SerializeField]
		private Sprite[] Start;

		[SerializeField]
		private Sprite[] R1;

		[SerializeField]
		private Sprite[] R2;

		[SerializeField]
		private Sprite[] L1;

		[SerializeField]
		private Sprite[] L2;

		[SerializeField]
		private Sprite[] up;

		[SerializeField]
		private Sprite[] down;

		[SerializeField]
		private Sprite[] left;

		[SerializeField]
		private Sprite[] right;

		private DisplayPlatform currentPlatformInstance;

		private bool displayGamePad;

		private Vector2 initRect;

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		private void Update()
		{
		}

		private bool CheckChangePlatform()
		{
			return false;
		}

		private int PfId()
		{
			return 0;
		}

		private void RefreshView()
		{
		}

		private void OnChange(KeyOptionChangeEvent evt)
		{
		}
	}
}
