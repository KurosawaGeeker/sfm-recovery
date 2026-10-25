using System;
using System.Collections.Generic;
using ExposureUnnoticed2.Master.Cosplay;
using TMPro;
using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.InventoryPanel.CosplaySerieseSelectPanel
{
	public class CosplaySeriesSelectPanelView : MonoBehaviour
	{
		[SerializeField]
		private TextMeshProUGUI seriesName;

		[SerializeField]
		private Transform chipParent;

		[SerializeField]
		private GameObject chipPrefab;

		private Action<int> changeSeries;

		private int currentSeriesIndex;

		private CosplayType currentSeries;

		private List<CosplaySeriesChipView> chipList;

		private List<CosplayType> bringCosplaySeriesList;

		private bool isShop;

		public void Initialize(Action<int> changeSeries, bool isShop)
		{
            // Complete original RVA 0x4F6270; lists intentionally retain prior contents.
            this.isShop = isShop;
            this.changeSeries = changeSeries;
            if (!isShop) bringCosplaySeriesList.Add((CosplayType)(-1));
            foreach (var pair in MCosplay.GetAll())
            {
                foreach (var part in pair.Value.GetAll())
                {
                    if (global::ExposureUnnoticed2.Scripts.Base.SystemManager.IsTrial)
                    {
                        if (unchecked((uint)pair.Key) > 1U)
                        {
                            if ((int)pair.Key != 2) continue;
                            if (global::ExposureUnnoticed2.ObjectUI.PcMenuPanel.OnlineShopPanel.OnlineShopPanelView.Instance != null) continue;
                        }
                    }
                    if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsBringCosplayParts(part.UniqueId))
                    {
                        bringCosplaySeriesList.Add(pair.Value.Type);
                        break;
                    }
                }
            }
            global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DestroyChildren(chipParent);
            foreach (var type in bringCosplaySeriesList)
                chipList.Add(UnityEngine.Object.Instantiate(chipPrefab, chipParent).GetComponent<CosplaySeriesChipView>());
            if (bringCosplaySeriesList.Count > 0)
            {
                currentSeries = bringCosplaySeriesList[0];
                RefreshView();
            }
        }

		public void OnLateUpdate()
		{
            if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)24) || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)24,20,5))
            {
                global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48);
                OnClickSeriesChange(-1);
            }
            else if (global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)25) || global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownLongRepeat((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)25,20,5))
            {
                global::ExposureUnnoticed2.Scripts.Sound.UiSoundPlayer.Instance.PlayOneShotAudioData((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)48);
                OnClickSeriesChange(1);
            }
        }

		private void RefreshView()
		{
            // Complete original RVA 0x4F6C10; original captures the text after master lookup.
            for (int i = 0; i < chipList.Count; ++i) chipList[i].SetActive(i == currentSeriesIndex);
            currentSeries = bringCosplaySeriesList[currentSeriesIndex];
            if ((int)currentSeries == -1)
                seriesName.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans("ui_cosplayCurrentEquipCategory");
            else
            {
                var record = MCosplay.Get(currentSeries);
                seriesName.text = global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(record.SeriesNameKey);
            }
        }

		public void OnClickSeriesChange(int changeAmount)
		{
            if (bringCosplaySeriesList.Count == 0) return;
            currentSeriesIndex = unchecked(bringCosplaySeriesList.Count + currentSeriesIndex + changeAmount) % bringCosplaySeriesList.Count;
            changeSeries(changeAmount);
            RefreshView();
        }
		// Complete original constructor RVA 0x4F6E20.
		public CosplaySeriesSelectPanelView()
		{
            chipList = new List<CosplaySeriesChipView>();
            bringCosplaySeriesList = new List<CosplayType>();
		}

	}
}
