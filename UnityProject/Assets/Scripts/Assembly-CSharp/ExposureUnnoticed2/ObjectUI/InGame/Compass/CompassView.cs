using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.InGame;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InGame.Compass
{
	public class CompassView : MonoBehaviour
	{
		private struct TargetModel
		{
			public DroppedItemManager.DropData DropData;

			public GameObject IconObject;

			public Transform PortalTransform;
		}

		[SerializeField]
		private float a;

		[SerializeField]
		private Transform w;

		[SerializeField]
		private Transform n;

		[SerializeField]
		private Transform e;

		[SerializeField]
		private Transform s;

		[SerializeField]
		private Transform[] childs;

		[SerializeField]
		private Transform targetIconParent;

		[SerializeField]
		private GameObject targetIcon;

		[SerializeField]
		private GameObject homeIconObject;

		private List<TargetModel> portalChildList = new List<TargetModel>();

		private List<TargetModel> targetList = new List<TargetModel>();

		public static CompassView Instance { get; private set; }

		public void Initialize()
		{
            Instance = this;
            ExternalMethodManager.DestroyChildren(targetIconParent);
            foreach (var item in GameState.GameStateData.DropDataDict)
                if (item.Value.IsDropped) RegisterTarget(item.Value);
            foreach (var item in GameState.GameStateData.ReserveDropList) RegisterTarget(item);
            EventManager.AddListener<CompleteTransitionStageEvent>(OnCompleteTransStage);
            Scheduler.DelayFlame(() => OnCompleteTransStage(null), 2);
        }

		public void Dispose()
		{
		}

		private void OnCompleteTransStage(CompleteTransitionStageEvent evt)
		{
            foreach (var item in portalChildList) UnityEngine.Object.Destroy(item.IconObject);
            portalChildList.Clear();
            foreach (var portal in InGameManager.Instance.CurrentStageController.PortalManager.Portals)
            {
                if ((int)portal.StageType != 0 && portal.CanEntryByRank)
                {
                    var icon = UnityEngine.Object.Instantiate(targetIcon, targetIconParent);
                    icon.GetComponent<CompassIconView>().InitializePortal();
                    var list = portalChildList;
                    var target = new TargetModel { DropData = null, IconObject = icon, PortalTransform = portal.transform };
                    list.Add(target);
                }
            }
        }

		private void Update()
		{
		}

		private void UpdatePortal()
		{
		}

		private void UpdateHome()
		{
		}

		private void UpdateDropItem()
		{
		}

		public void RegisterTarget(DroppedItemManager.DropData dropData)
		{
            var type = dropData.ItemType;
            if (!DroppedItemManager.Instance.IsClothes(type) &&
                type != DroppedItemManager.DropItemType.HandcuffKey &&
                type != DroppedItemManager.DropItemType.VibeRemocon) return;
            var icon = UnityEngine.Object.Instantiate(targetIcon, targetIconParent);
            icon.GetComponent<CompassIconView>().Initialize(dropData.ItemType);
            targetList.Add(new TargetModel { DropData = dropData, IconObject = icon, PortalTransform = null });
        }
	}
}
