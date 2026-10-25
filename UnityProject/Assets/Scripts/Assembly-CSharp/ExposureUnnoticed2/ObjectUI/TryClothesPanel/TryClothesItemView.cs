using System;
using ExposureUnnoticed2.Master.Clothes;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.TryClothesPanel
{
	public class TryClothesItemView : MonoBehaviour
	{
		[SerializeField]
		private new TextMeshProUGUI name;

		[SerializeField]
		private TextMeshProUGUI necessaryRp;

		[SerializeField]
		private TextMeshProUGUI buttonText;

		private Action<MClothesRecord> onClick;

		private MClothesRecord master;

		public void Initialize(MClothesRecord master, Action<MClothesRecord> onClick)
		{
		}

		public void OnClick()
		{
		}
	}
}
