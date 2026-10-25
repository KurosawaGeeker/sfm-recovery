using System.Collections.Generic;
using ExposureUnnoticed2.Master.Cosplay;
using ExposureUnnoticed2.ObjectUI.InventoryPanel.CosplaySerieseSelectPanel;
using ExposureUnnoticed2.ObjectUI.InventoryPanel.PartsOptionSetupPanel;
using HadakaCoat.ObjectsUi.Common.Button;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel
{
	public class InventoryPanelCosplayView : MonoBehaviour
	{
		[SerializeField]
		private Transform itemParent;

		[SerializeField]
		private GameObject itemPrefab;

		[SerializeField]
		private ButtonGroupManager cosplayButtonGroupManager;

		[SerializeField]
		private GameObject contentsParent;

		[SerializeField]
		private CosplaySeriesSelectPanelView cosplaySeriesSelectPanelView;

		[SerializeField]
		private PartsOptionSetupPanelView partsOptionSetupPanelView;

		[SerializeField]
		private ScrollRect scrollRect;

		private bool isShowing;

		private int currentSeriesIndex;

		private CosplayType currentSeries;

		private CanvasGroup canvasGroup;

		private InventoryEffectPanelView effectPanelView;

		private List<CosplayType> bringCosplaySeriesList;

		private InventoryPanelView parentView;

		public List<InventoryEquipItemView> ItemList;

		public void Initialize(InventoryEffectPanelView effectPanelView, InventoryPanelView parentView)
		{
            // Complete original RVA 0x4EABD0; original delegate is ChangeSeries, not its sound wrapper.
            this.parentView = parentView;
            this.effectPanelView = effectPanelView;
            canvasGroup = GetComponent<CanvasGroup>();
            bringCosplaySeriesList.Add((CosplayType)(-1));
            foreach (var pair in MCosplay.GetAll())
            {
                foreach (var part in pair.Value.GetAll())
                {
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringCosplayParts(part.UniqueId))
                    {
                        bringCosplaySeriesList.Add(pair.Value.Type);
                        break;
                    }
                }
            }
            cosplaySeriesSelectPanelView.Initialize(ChangeSeries, false);
            partsOptionSetupPanelView.Initialize();
            if (bringCosplaySeriesList.Count > 0) currentSeries = bringCosplaySeriesList[0];
        }

		public void OnLateUpdate()
		{
            // Complete original4EB0C0; this remains called after parent close.
            if (!isShowing) return;
            if (bringCosplaySeriesList.Count == 0) return;
            cosplaySeriesSelectPanelView.OnLateUpdate();
        }

		private void ChangeSeries(int changeAmount)
		{
			if (bringCosplaySeriesList.Count == 0)
			{
			    contentsParent.SetActive(false);
			    return;
			}
			contentsParent.SetActive(true);
			currentSeriesIndex = unchecked(bringCosplaySeriesList.Count + currentSeriesIndex + changeAmount) % bringCosplaySeriesList.Count;
			currentSeries = bringCosplaySeriesList[currentSeriesIndex];
			global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(itemParent);
			ItemList.Clear();
			if ((int)currentSeries == -1)
			{
			    foreach (var pair in global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerCosplayPartsDict)
			    {
			        RCosplayParts recordPartsRecord;
			        if (!pair.Value) continue;
			        recordPartsRecord = MCosplay.GetParts(pair.Key);
			        if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringCosplayParts(recordPartsRecord.UniqueId)) continue;
			        var view = global::UnityEngine.Object.Instantiate(itemPrefab, itemParent).GetComponent<InventoryEquipItemView>();
			        view.Initialize(recordPartsRecord, OnClickEquip, record => effectPanelView.Refresh(record, false), record =>
			        {
			            InventoryPanelView.Instance.IsOpenChild = true;
			            partsOptionSetupPanelView.Open(recordPartsRecord);
			        });
			        ItemList.Add(view);
			    }
			}
			else
			{
			    foreach (var recordPartsRecord in MCosplay.Get(currentSeries).GetAll())
			    {
			        if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringCosplayParts(recordPartsRecord.UniqueId)) continue;
			        var view = global::UnityEngine.Object.Instantiate(itemPrefab, itemParent).GetComponent<InventoryEquipItemView>();
			        view.Initialize(recordPartsRecord, OnClickEquip, record => effectPanelView.Refresh(record, false), record =>
			        {
			            InventoryPanelView.Instance.IsOpenChild = true;
			            partsOptionSetupPanelView.Open(recordPartsRecord);
			        });
			        ItemList.Add(view);
			    }
			}
			cosplayButtonGroupManager.ReSearchButton(false, false);
			parentView.currentSelectIndex = 0;
			global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayFlame(() =>
			{
			    parentView.ToPadModeIfNeed(false);
			    if (ItemList.Count > 0) ItemList[0].OnClickBody();
			}, 1);
			scrollRect.verticalNormalizedPosition = 1f;
			scrollRect.velocity = Vector2.zero;
		}

		private void OnClickEquip(IEquipableMaster record)
		{
			var parts = (RCosplayParts)record;
			bool isHidden = parts && (int)parts.HiddenPartsType != 0;
			if (!global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.CanUseHand(true, false, false, false, true)) return;
			if (isHidden && global::ExposureUnnoticed2.Scripts.InGame.DroneManager.Instance.IsActiveMission)
			{
			    global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_drone_cant_take_hide_costume"));
			    return;
			}
			float distance = float.MaxValue;
			var dropped = global::ExposureUnnoticed2.Scripts.InGame.DroppedItemManager.Instance.GetDropClothesInSameArea();
			if (dropped != null)
			    distance = (dropped.Position - global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.Transform.position).magnitude;
			bool unavailable = false;
			if (global::ExposureUnnoticed2.Scripts.InGame.SceneProcessManager.LaundryManager.Instance)
			    unavailable = (int)global::ExposureUnnoticed2.Scripts.InGame.SceneProcessManager.LaundryManager.Instance.GetRpBuffSourceType() != -1;
			bool isNakedAndHidden = (int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerState.CurrentClothes.ClothesType == -1 && isHidden;
			if (isNakedAndHidden && !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCosplayParts(parts.UniqueId) &&
			    !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)19))
			{
			    if (distance > 1f) unavailable = true;
			    if (unavailable)
			    {
			        global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_cant_take_hide_costume_near_coat"));
			        return;
			    }
			}
			bool isActive = !global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsEquipCosplayParts(parts.UniqueId);
			if (isActive && !InventoryPanelView.Instance.CanEquipByMaxCount()) return;
			global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)17, -1f, false);
			global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.SetCosplayActive(parts.ParentCosplayType, parts.Id, isActive);
			InventoryPanelView.Instance.RefreshEquipCount();
			foreach (var item in ItemList) item.RefreshEmit();
		}

		public void OnClickSeriesChange(int changeAmount)
		{
			ChangeSeries(changeAmount);
		}

		public void Show()
		{
            // Recovered complete original RVA 0x4EC470.
            isShowing = true;
            ChangeSeries(0);
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true;
        }

		public void Hide()
		{
            // Recovered complete original RVA 0x4EC540.
            isShowing = false;
            canvasGroup.alpha = 0f;
            canvasGroup.blocksRaycasts = false;
        }
		// Complete original constructor RVA 0x4EC600.
		public InventoryPanelCosplayView()
		{
            bringCosplaySeriesList = new List<CosplayType>();
            ItemList = new List<InventoryEquipItemView>();
		}

	}
}
