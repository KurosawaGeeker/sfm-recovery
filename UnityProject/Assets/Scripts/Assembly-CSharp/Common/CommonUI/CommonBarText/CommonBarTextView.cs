using TMPro;
using UnityEngine;

namespace Common.CommonUI.CommonBarText
{
	public class CommonBarTextView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		public void Initialize(string name)
		{
this.name.text = name;
}
	}
}
