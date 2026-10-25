using System;
using ExposureUnnoticed2.Scripts.Base;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ExposureUnnoticed2.ObjectUI.SaveDataSelectPanel
{
	public class SaveSlotItemView : MonoBehaviour
	{
		[SerializeField]
		private Image screenShot;

		[SerializeField]
		private TextMeshProUGUI typeNameText;

		[SerializeField]
		private TextMeshProUGUI dateText;

		[SerializeField]
		private TextMeshProUGUI difficultText;

		[SerializeField]
		private TextMeshProUGUI playTimeText;

		[SerializeField]
		private CanvasGroup emitCanvasGroup;

		private Action<SaveRapper.SaveDataType, int> onClick;

		private int slot;

		private SaveRapper.SaveDataType type;

		private bool isBrokenData;

		public void Initialize(SaveRapper.SaveDataType type, Action<SaveRapper.SaveDataType, int> onClick, int slot = -1, bool isShowEmpty = false)
		{
			// Recovered from GameAssembly.dll RVA 0x4E5D00
			this.type = type;
			this.slot = slot;
			this.onClick = onClick;
			if (!global::ExposureUnnoticed2.Scripts.InGame.SaveManager.IsExistSaveData(type, slot))
			{
			    if (isShowEmpty && type == SaveRapper.SaveDataType.ManualSlot)
			    {
			        typeNameText.text = Localizer.Trans("ui_load_empty_data");
			        dateText.gameObject.SetActive(false);
			        difficultText.gameObject.SetActive(false);
			        playTimeText.gameObject.SetActive(false);
			    }
			    else gameObject.SetActive(false);
			    return;
			}
			if (SystemManager.IsTrial)
			{
			    SaveRapper.PreProcess(false, slot, false);
			    isBrokenData = !SaveRapper.Load("IsTrial", true);
			    if (isBrokenData)
			    {
			        typeNameText.text = Localizer.Trans("ui_load_fullVer_data");
			        dateText.gameObject.SetActive(false);
			        difficultText.gameObject.SetActive(false);
			        playTimeText.gameObject.SetActive(false);
			        return;
			    }
			}
			System.DateTime time = type switch
			{
			    SaveRapper.SaveDataType.Interrupt => global::ExposureUnnoticed2.Scripts.InGame.GameState.InterruptSaveTime,
			    SaveRapper.SaveDataType.AutoSave => global::ExposureUnnoticed2.Scripts.InGame.GameState.AutoSaveTime[slot],
			    SaveRapper.SaveDataType.HomeSave => global::ExposureUnnoticed2.Scripts.InGame.GameState.HomeSaveTime,
			    SaveRapper.SaveDataType.ManualSlot => global::ExposureUnnoticed2.Scripts.InGame.GameState.ManualSaveTime[slot],
			    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(type)
			};
			dateText.text = time.ToString("yyyy/MM/dd HH:mm:ss");
			typeNameText.text = Localizer.Trans("ui_common_roshutsu_rank", Localizer.Trans(global::ExposureUnnoticed2.Master.Rank.MRank.GetByRank(global::ExposureUnnoticed2.Scripts.InGame.GameState.RankData[slot]).NameKey));
			global::ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel.DifficultType difficulty = global::ExposureUnnoticed2.Scripts.InGame.GameState.DifficultData[slot];
			string difficultyKey = difficulty switch
			{
			    global::ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel.DifficultType.Casual => "difficulty_casual",
			    global::ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel.DifficultType.Easy => "difficulty_easy",
			    global::ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel.DifficultType.Original => "difficulty_original",
			    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(difficulty)
			};
			difficultText.text = Localizer.Trans("rp_buff_difficulty", Localizer.Trans(difficultyKey));
			playTimeText.text = Localizer.Trans("ui_load_play_time", global::ExposureUnnoticed2.ObjectUI.BuffPanel.StatusPanelStatisticsView.ToTime(global::ExposureUnnoticed2.Scripts.InGame.GameState.TotalPlayTimeData[slot]));
			if (type == SaveRapper.SaveDataType.AutoSave) _ = "/a" + slot.ToString() + ".png";
			else if (type == SaveRapper.SaveDataType.ManualSlot) _ = "/m" + slot.ToString() + ".png";
		}

		public void ForceSetEmit(int alpha)
		{
			// Recovered from GameAssembly.dll RVA 0x4E6590
			emitCanvasGroup.alpha = alpha;
		}

		public void OnClick()
		{
			// Recovered from GameAssembly.dll RVA 0x4E6600
			if (!isBrokenData) onClick?.Invoke(type, slot);
		}
	}
}
