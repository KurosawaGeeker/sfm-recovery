using System.Collections.Generic;
using ExposureUnnoticed2.Master.Accessory;
using ExposureUnnoticed2.ObjectUI.BuyPanel;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.AdultShopScene
{
	public class AdultShopManager : MonoBehaviour, IBasketController
	{
		[SerializeField]
		private Animator clerk;

		[SerializeField]
		private Transform clerkInteractTransform;

		private List<MAdultGoodsRecord> basket;

		public static AdultShopManager Instance { get; private set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		public void Add(ScriptableObject master)
		{
		}

		public void Remove(ScriptableObject master)
		{
		}

		public bool Contains(ScriptableObject master)
		{
			return false;
		}

		private void CheckDisplayBasket()
		{
		}

		private void OnConversationEvent(ConversationEvent evt)
		{
		}

		public int GetBasketRp()
		{
			return 0;
		}
	}
}
