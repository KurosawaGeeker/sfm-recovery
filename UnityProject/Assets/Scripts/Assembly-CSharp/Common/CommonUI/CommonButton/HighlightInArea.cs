using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.CommonUI.CommonButton
{
	public class HighlightInArea : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		private static readonly int IsHighlighted;

		private bool isEntered;

		private Animator animator;

		public void Awake()
		{
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		public void OnPointerExit(PointerEventData eventData)
		{
		}
	}
}
