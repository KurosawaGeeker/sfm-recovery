using System;
using ExposureUnnoticed2.ObjectUI.OptionMenu;
using ExposureUnnoticed2.ObjectUI.SelectDifficultyPanel;
using ExposureUnnoticed2.ObjectUI.TutorialPanel;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class GameState
	{
		public static DateTime[] AutoSaveTime;

		public static DateTime[] ManualSaveTime;

		public static DateTime InterruptSaveTime;

		public static DateTime HomeSaveTime;

		public static DifficultType[] DifficultData;

		public static int[] RankData;

		public static uint[] TotalPlayTimeData;

		public static OptionData OptionData;

		public static TutorialData TutorialData;

		public static GameStateData GameStateData;

		public static int SaveDataSlotIndex;

		public static bool IsTrial;

		public static int OptionFaceRankBias;

		public static bool OptionFaceRange;

		public static PlayerCustomData PlayerCustomData;
	}
}
