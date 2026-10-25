using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.LifePanel
{
	public class LifePanelView : MonoBehaviour
	{
		[SerializeField]
		private GameObject lifeItemPrefab;

		[SerializeField]
		private Transform parent;

		private List<LifeIconView> lifeIconList;

		private int cacheMaxLife;

		private int cacheLife;

		private void LateUpdate()
		{
		}

		private void OnChangeMaxLife()
		{
		}

		private void OnChangeLife()
		{
		}
	}
}
