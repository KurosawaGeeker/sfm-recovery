using UnityEngine;
using UnityEngine.UI;

namespace Common.CommonUI.CommonScrollView
{
	public class CommonScrollRectView : MonoBehaviour
	{
		[SerializeField]
		private int targetInputId = -1;

		[SerializeField]
		private ScrollRect scrollRect;

		[SerializeField]
		private RectTransform viewport;

		[SerializeField]
		private RectTransform scrollContent;

		private void Start()
		{
			// Recovered from GameAssembly.dll RVA 0x581AD0
			scrollRect.scrollSensitivity = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.WheelSensitive100 * 0.005f;
		}

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x581B40
			if (targetInputId == -1) return;
			float height = scrollContent.rect.height - viewport.rect.height;
			global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType input = targetInputId switch
			{
			    0 => global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIScrollA,
			    1 => global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType.UIScrollB,
			    _ => throw new global::System.Runtime.CompilerServices.SwitchExpressionException(targetInputId)
			};
			if (height > 0f)
			{
			    Vector2 value = global::ExposureUnnoticed2.Scripts.Base.InputManager.GetVector2(input);
			    float delta = Time.unscaledDeltaTime;
			    float speed = value.y * 1000f;
			    float position = scrollRect.verticalNormalizedPosition + delta * speed / height;
			    if (position < 0f) position = 0f;
			    else if (position > 1f) position = 1f;
			    scrollRect.verticalNormalizedPosition = position;
			}
		}

		public float ScrollToBeBottom(GameObject go)
		{
			// Recovered from GameAssembly.dll RVA 0x581D80
			return ScrollToCore(go, 0f);
		}

		public float ScrollToBeTop(GameObject go)
		{
			// Recovered from GameAssembly.dll RVA 0x581D90
			return ScrollToCore(go, 1f);
		}

		public float ScrollToCentering(GameObject go)
		{
			// Recovered from GameAssembly.dll RVA 0x581DA0
			return ScrollToCore(go, 0.5f);
		}

		public void ResetScroll()
		{
			// Recovered from GameAssembly.dll RVA 0x581DB0
			scrollRect.verticalNormalizedPosition = 1f;
			scrollRect.velocity = Vector2.zero;
		}

		private float ScrollToCore(GameObject go, float align)
		{
			// Recovered from GameAssembly.dll RVA 0x581E40
			RectTransform target = go.transform.GetComponent<RectTransform>();
			float contentHeight = scrollRect.content.rect.height;
			float viewportHeight = viewport.rect.height;
			if (contentHeight < viewportHeight) return 0f;
			float y = GetPosY(target);
			float position = ((contentHeight + y + target.rect.height * align) - viewportHeight * align)
			                 / (contentHeight - viewportHeight);
			if (position < 0f) position = 0f;
			else if (position > 1f) position = 1f;
			scrollRect.verticalNormalizedPosition = position;
			return position;
		}

		private float GetPosY(RectTransform transform)
		{
			// Recovered from GameAssembly.dll RVA 0x5821B0
			float y = transform.localPosition.y;
			return transform.rect.y + y;
		}
	}
}
