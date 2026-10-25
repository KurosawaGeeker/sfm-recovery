using UnityEngine;
using UnityEngine.EventSystems;

namespace Ricimi
{
	public class Tooltip : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler
	{
		public GameObject tooltip;

		public float fadeTime;

		public virtual void OnPointerEnter(PointerEventData eventData)
		{
		}

		public virtual void OnPointerExit(PointerEventData eventData)
		{
		}
	}
}
