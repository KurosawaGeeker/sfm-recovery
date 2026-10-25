using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.ResultPanel
{
	public class CompletedMissionItem : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private TextMeshProUGUI rp;

		[SerializeField]
		private TextMeshProUGUI rpBonus;

		[SerializeField]
		private GameObject newObject;

		public void Initialize(string name, string rpText, string rpBonus, bool isNew)
		{
this.name.text = name;
rp.text = rpText;
newObject.SetActive(isNew);
this.rpBonus.text = rpBonus;
}
	}
}
