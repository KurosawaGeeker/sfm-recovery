using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Mission;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.InGame.MissionPanel
{
	public class MissionPanelView : MonoBehaviour
	{
		[SerializeField]
		private GameObject dummyItemPrefab;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private GameObject droneItemPrefab;

		[SerializeField]
		private GameObject randomItemPrefab;

		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private VerticalLayoutGroup dummyItemViewParent;

		private MissionManager missionManager;

		private Dictionary<int, MissionItemPanelView> itemViewDict = new Dictionary<int, MissionItemPanelView>();

		private MissionItemPanelView droneItemView;

		private MissionItemPanelView randomItemView;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x50A2D0
			missionManager = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.MissionManager;
			global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(dummyItemViewParent);
		}

		public void OnLateUpdate()
		{
            foreach (var current in missionManager.MissionList)
            {
                if (current.IsCreatePanel && !itemViewDict.ContainsKey(current.RMission.Id))
                {
                    var item = Object.Instantiate(itemPrefab, itemParent).GetComponent<MissionItemPanelView>();
                    var dummy = Object.Instantiate(dummyItemPrefab, dummyItemViewParent.transform);
                    var dummyTransform = dummy.transform;
                    if (item is null) throw new global::System.NullReferenceException();
                    item.Initialize(current, dummyTransform);
                    itemViewDict.Add(current.RMission.Id, item);
                }
                else if (current.IsOverDestroyTime || !current.IsCreatePanel)
                {
                    if (itemViewDict.ContainsKey(current.RMission.Id))
                    {
                        itemViewDict[current.RMission.Id].DestroyItem();
                        itemViewDict.Remove(current.RMission.Id);
                    }
                }
            }
            if (global::ExposureUnnoticed2.Scripts.InGame.DroneManager.Instance is null) throw new global::System.NullReferenceException();
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ActiveDroneMissionId != -1 || droneItemView)
            {
                var active = global::ExposureUnnoticed2.Scripts.InGame.DroneManager.Instance.GetActiveMission();
                if (droneItemView)
                {
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.ActiveDroneMissionId == -1 && droneItemView)
                    {
                        var anotherReferencer = droneItemView;
                        global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayUnscaledTime(() => anotherReferencer.DestroyItem(), 2f);
                        droneItemView = null;
                    }
                }
                else
                {
                    var item = Object.Instantiate(droneItemPrefab, itemParent).GetComponent<MissionItemPanelView>();
                    var dummy = Object.Instantiate(dummyItemPrefab, dummyItemViewParent.transform);
                    dummy.transform.SetAsLastSibling();
                    var dummyTransform = dummy.transform;
                    if (item is null) throw new global::System.NullReferenceException();
                    item.Initialize(active, dummyTransform);
                    droneItemView = item;
                }
            }
            if (global::ExposureUnnoticed2.Scripts.InGame.RandomMissionManager.Instance.IsActiveMission || randomItemView)
            {
                if (randomItemView)
                {
                    if (!global::ExposureUnnoticed2.Scripts.InGame.RandomMissionManager.Instance.IsActiveMission && randomItemView)
                    {
                        var anotherReferencer = randomItemView;
                        global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayUnscaledTime(() => anotherReferencer.DestroyItem(), 2f);
                        randomItemView = null;
                    }
                }
                else
                {
                    var item = Object.Instantiate(randomItemPrefab, itemParent).GetComponent<MissionItemPanelView>();
                    var dummy = Object.Instantiate(dummyItemPrefab, dummyItemViewParent.transform);
                    dummy.transform.SetAsLastSibling();
                    var active = global::ExposureUnnoticed2.Scripts.InGame.RandomMissionManager.Instance.MissionBase;
                    var dummyTransform = dummy.transform;
                    if (item is null) throw new global::System.NullReferenceException();
                    item.Initialize(active, dummyTransform);
                    randomItemView = item;
                }
            }
            dummyItemViewParent.SetLayoutHorizontal();
            dummyItemViewParent.SetLayoutVertical();
		}
	}
}
