using System.Collections.Generic;
using ExposureUnnoticed2.Master.Clothes;
using ExposureUnnoticed2.Object3D.Environment.ChangingRoom;
using ExposureUnnoticed2.ObjectUI.BuyPanel;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class FashionShopManager : MonoBehaviour, IBasketController
	{
		private const string Prefix = "FashionShopManager_";

		private const string DropBasketKey = "FashionShopManager_DropBasket";

		private const string BasketKey = "FashionShopManager_basket";

		private const string IsSavedKey = "FashionShopManager_IsSaved";

		private const string CurrentTryClothesKey = "FashionShopManager_CurrentTryClothes";

		private const string LastOwnClothesKey = "FashionShopManager_LastOwnClothes";

		public static FashionShopManager Instance;

		[SerializeField]
		private PlayerEnterAreaChecker tryOnArea;

		[SerializeField]
		private ChangingRoomCurtainController curtainController;

		[SerializeField]
		private Animator clerk;

		[SerializeField]
		private Transform clerkInteractTransform;

		private List<MClothesRecord> basket;

		private Vector3 basketPosition;

		public MClothesRecord CurrentTryClothes { get; set; }

		public MClothesRecord LastOwnClothes { get; set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		public List<MClothesRecord> GetAll()
		{
			return null;
		}

		public bool Contains(ScriptableObject master)
		{
            // Complete original0x49F620: failed as-cast is null, including its List.Contains(null) behavior.
            return basket.Contains(master as MClothesRecord);
        }

		public void Add(ScriptableObject master)
		{
		}

		public void Remove(ScriptableObject master)
		{
		}

		public static void SaveDropBasket(Vector3 position)
		{
			// Recovered from GameAssembly.dll RVA 0x49F8E0
			if (Instance != null) Instance.basketPosition = position;
		}

		private void CheckDisplayBasket()
		{
		}

		public static bool IsWearTryClothes()
		{
            // Complete original0x49FA80; retain first Unity-liveness guard and current graph rereads.
            if (Instance != null)
            {
                var clothes = InGameManager.Instance.InGameReferencer.Player.Pca.PlayerState.CurrentClothes;
                return Instance.Contains(clothes);
            }
            return false;
        }

		public static bool IsInTryArea()
		{
            return Instance && Instance.tryOnArea.IsInArea && !Instance.curtainController.IsOpen;
        }

		public int GetBasketRp()
		{
			return 0;
		}

		private void OnConversationEvent(ConversationEvent evt)
		{
		}

		private MClothesRecord Trans(int typeInt)
		{
			return null;
		}
	}
}
