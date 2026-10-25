using ExposureUnnoticed2.Scripts.Base;
using TMPro;
using UnityEngine;

namespace Common.CommonUI
{
	public class CommonTextView : MonoBehaviour
	{
		[SerializeField]
		private string key;

		[SerializeField]
		private bool isListenChangeEvent;

		[SerializeField]
		private bool isNoChangeFont;

		[SerializeField]
		private int fontMaterialId;

		private TextMeshProUGUI text;

		private void Start()
		{
		}

		private void SetMaterial()
		{
		}

		private void OnDestroy()
		{
		}

		private void OnChangeLanguageEvent(ChangeLanguageEvent changeLanguageEvent)
		{
		}

		private void ChangeFont(bool isForceChange)
		{
		}
	}
}
