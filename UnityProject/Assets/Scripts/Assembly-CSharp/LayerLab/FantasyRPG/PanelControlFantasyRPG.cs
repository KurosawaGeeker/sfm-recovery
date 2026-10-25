using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace LayerLab.FantasyRPG
{
	public class PanelControlFantasyRPG : MonoBehaviour
	{
		private int page;

		private bool isReady;

		[SerializeField]
		private List<GameObject> panels;

		private TextMeshProUGUI textTitle;

		[SerializeField]
		private Transform panelTransform;

		[SerializeField]
		private Button buttonPrev;

		[SerializeField]
		private Button buttonNext;

		private void Start()
		{
		}

		private void Update()
		{
		}

		public void Click_Prev()
		{
		}

		public void Click_Next()
		{
		}

		private void SetArrowActive()
		{
		}

		private void CheckControl()
		{
		}
	}
}
