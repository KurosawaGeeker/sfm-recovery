using System.Collections.Generic;
using ExposureUnnoticed2.Master.Clothes;
using ExposureUnnoticed2.Master.Item;
using ExposureUnnoticed2.Master.Stage;
using ExposureUnnoticed2.Object3D.DropedCoat;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class DroppedItemManager
	{
		public enum DropItemType
		{
			None = -1,
			Coat = 0,
			Hoodie = 1,
			Basket = 2,
			Pants = 3,
			Bra = 4,
			HandcuffKey = 5,
			VibeRemocon = 6,
			DildoFloor = 10000,
			DildoWall = 10100
		}

		public class DropData
		{
			[ES3Serializable]
			public bool IsDropped;

			[ES3Serializable]
			public bool IsReserve;

			[ES3Serializable]
			public bool IsViewCompassInDroneMission;

			[ES3Serializable]
			public Vector3 Position;

			[ES3Serializable]
			public Quaternion Rotation;

			[ES3Serializable]
			public StageType StageType;

			[ES3Serializable]
			public DropItemType ItemType;

			public DropData(DropItemType type)
			{
			// Recovered from GameAssembly.dll RVA 0x3B85F0
			ItemType = type;
		}
		}

		public const int MaxSamePutNum = 10;

		private Dictionary<DropItemType, DroppedItemController> cache;

		public DropItemType[] ClothesTypes;

		public static DroppedItemManager Instance { get; private set; }

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x4889E0
			Instance = null;
		}

		public bool IsClothes(DropItemType type)
		{
			// Recovered from GameAssembly.dll RVA 0x4889F0
			foreach (var clothing in ClothesTypes)
			    if (clothing == type) return true;
			return false;
		}

		public DroppedItemController GetCache(DropItemType type)
		{
			// Recovered from GameAssembly.dll RVA 0x488A50
			if (cache.ContainsKey(type) && cache[type] == null) cache.Remove(type);
			if (!cache.ContainsKey(type))
			{
			    GameObject prefab;
			    if ((int)type < 10000)
			        prefab = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.DroppedPrefabs[(int)type];
			    else
			        prefab = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MultipleDroppedPrefabs[(int)type / 100 - 100];
			    var parent = InGameManager.Instance.transform;
			    var obj = UnityEngine.Object.Instantiate(prefab, parent);
			    var controller = obj.GetComponent<DroppedItemController>();
			    controller.Initialize(type);
			    cache.Add(type, controller);
			}
			return cache[type];
		}

		public void ApplyReserveDropIfNeed()
		{
// Complete original native body; retain original callback and failure order.
var list = GameState.GameStateData.ReserveDropList;
System.Func<DropData, bool> predicate = reserveData =>
{
    if (reserveData.StageType != GameState.GameStateData.CurrentStageType) return false;
    reserveData.IsReserve = false;
    var random = InGameManager.Instance.CurrentStageController.ItemRandomPositionManager;
    var playerTransform = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.Transform;
    var near = playerTransform.position;
    if (random is null) throw new System.NullReferenceException();
    var point = random.GetRandomPosition(near);
    if (point is null) throw new System.NullReferenceException();
    var position = point.position;
    var rotation = point.rotation;
    var dropped = Drop(position, rotation, reserveData.ItemType);
    global::ExposureUnnoticed2.ObjectUI.InGame.Compass.CompassView.Instance.RegisterTarget(dropped);
    return true;
};
int i = 0;
while (i < list.Count)
{
    if (predicate(list[i])) list.Remove(list[i]);
    else i++;
}
}

		public DropData Drop(Vector3 position, Quaternion rotation, DropItemType type)
		{
			// Recovered from GameAssembly.dll RVA 0x488F70
			var controller = GetCache(type);
			controller.gameObject.SetActive(true);
			controller.transform.SetPositionAndRotation(position, rotation);
			global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[type].IsDropped = true;
			global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[type].Position = position;
			global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[type].Rotation = rotation;
			var stageTarget = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[type];
			stageTarget.StageType = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType;
			if ((int)type >= 10000)
			{
			    controller.Interact.gameObject.SetActive(true);
			    var dildo = controller.GetComponent<global::HadakaCoat.Objects3D.Dildo.DildoController>();
			    dildo.DildoId = type;
			    dildo.IsSit = false;
			}
			return global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[type];
		}

		public DropItemType GetDropClothesType()
		{
			return DropItemType.Coat;
		}

		public DropItemType TransNonUseItemId(DropItemType type)
		{
			// Recovered from GameAssembly.dll RVA 0x489490
			if ((int)type < 10000) return type;
			for (int i = 0; i < 10; ++i)
			{
			    var candidate = (DropItemType)unchecked((int)type + i);
			    if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[candidate].IsDropped) return candidate;
			}
			return (DropItemType)(-1);
		}

		public DropData ReserveDrop(DropItemType itemType, StageType stageType)
		{
			return null;
		}

		public void SendToHome(DropItemType type)
		{
		}

		private void Hide(DropItemType type)
		{
			// Recovered from GameAssembly.dll RVA 0x4897A0
			GetCache(type).gameObject.SetActive(false);
		}

		public void PushInElevator(Bounds pushBounds, Vector3 pushV, bool isFixed = false, bool isConstant = false)
		{
		}

		public void WarpElevatorHeight(float y, Bounds pushBounds)
		{
		}

		public void Collect(DropItemType type)
		{
			// Recovered from GameAssembly.dll RVA 0x48A0E0
			Hide(type);
			if ((int)type != 2)
			    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict[type].IsDropped = false;
			else
			    FashionShopManager.SaveDropBasket(global::ExposureUnnoticed2.Scripts.Base.Constant.NoDataVector3);
		}

		public void HideAllUnderwear()
		{
		}

		public bool CollectAll()
		{
// Complete original RVA 0x48A210; PE/typed identity is audited in this owned stage.
bool collected = false;
foreach (var entry in GameState.GameStateData.DropDataDict)
{
    if (!entry.Value.IsDropped) continue;
    Collect(entry.Key);
    if ((int)entry.Key > 9999) GameStateFacade.ChangeItemNum((global::ExposureUnnoticed2.Master.Item.ItemType)1, 1);
    if ((int)entry.Key == 6) GameStateFacade.ChangeItemNum((global::ExposureUnnoticed2.Master.Item.ItemType)10001, 1);
    collected = true;
    if ((int)entry.Key == 5)
    {
        GameStateFacade.ChangeItemNum((global::ExposureUnnoticed2.Master.Item.ItemType)10000, 1);
        collected = true;
    }
}
return collected;
}

		public bool IsPutSomeDildoAtOutsideHome()
		{
			return false;
		}

		public void DisposeAllPutDildo()
		{
            // Complete original0x48A650; live dictionary enumeration and the original global non-home test.
            foreach (var entry in GameState.GameStateData.DropDataDict)
            {
                var data = entry.Value;
                if (data.IsDropped && data.StageType != global::ExposureUnnoticed2.Master.Stage.StageType.Apart && (int)entry.Key > 9999)
                    Collect(entry.Key);
            }
        }

		public bool CollectAllClothes()
		{
			return false;
		}

		public DropData GetDropClothesInSameArea()
		{
            // Complete original0x48A920; three possible dictionary lookups and current state rereads are intentional.
            var types = ClothesTypes;
            for (int i = 0; i < types.Length; i++)
            {
                var type = types[i];
                if (GameState.GameStateData.DropDataDict[type].IsDropped &&
                    GameState.GameStateData.DropDataDict[type].StageType == GameState.GameStateData.CurrentStageType)
                    return GameState.GameStateData.DropDataDict[type];
            }
            return null;
        }

		public void ActiveDroppedItemIfNeed()
		{
			// Recovered from GameAssembly.dll RVA 0x48AA90
			foreach (var pair in global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DropDataDict)
			{
			    var data = pair.Value;
			    if (!data.IsDropped) continue;
			    if (data.StageType == global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType)
			        Drop(data.Position, data.Rotation, data.ItemType);
			    else if (data.IsDropped)
			        Hide(data.ItemType);
			}
		}

		private bool IsDildo(DropItemType type)
		{
			// Recovered from GameAssembly.dll RVA 0x48ACE0
			return (int)type >= 10000;
		}

		public static DropItemType ToDropItemType(ClothesType type)
		{
			// Recovered from GameAssembly.dll RVA 0x48ACF0
			return (DropItemType)((int)type == 0 ? 0 : (int)type == 1 ? 1 : -1);
		}

		public static ClothesType ToClothesType(DropItemType type)
		{
			// Recovered from GameAssembly.dll RVA 0x48ACF0
			return (ClothesType)((int)type == 0 ? 0 : (int)type == 1 ? 1 : -1);
		}

		public static ItemType ToItemType(DropItemType type)
		{
// Complete original RVA 0x48AD10; PE/typed identity is audited in this owned stage.
return (ItemType)((int)type == 5 ? 10000 : (int)type == 6 ? 10001 : -1);
}
	// Recovered original member RVA 0x488870: .ctor
		public DroppedItemManager()
        {
            cache = new Dictionary<DropItemType, DroppedItemController>();
            ClothesTypes = new DropItemType[] { DropItemType.Coat, DropItemType.Hoodie };
            Instance = this;
            ActiveDroppedItemIfNeed();
        }
	}
}
