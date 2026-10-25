using System.Collections.Generic;

namespace ExposureUnnoticed2.ObjectUI.TutorialPanel
{
	public class TutorialData
	{
		[ES3Serializable]
		public bool IsOpenFirstPlay;

		[ES3Serializable]
		public bool IsOpenFirstOutside;

		[ES3Serializable]
		public bool IsOpenReinforce;

		[ES3Serializable]
		public bool IsOpenWater;

		[ES3Serializable]
		public bool IsOpenDildo;

		[ES3Serializable]
		public bool IsOpenHandcuff;

		[ES3Serializable]
		public bool IsOpenVibe;

		[ES3Serializable]
		public bool IsOpenSecondMap;

		[ES3Serializable]
		public bool IsOpenPistonMachine;

		[ES3Serializable]
		public bool IsOpenBodyPaint;

		[ES3Serializable]
		public bool IsOpenInvisible;

		[ES3Serializable]
		public bool IsOpenTimeStop;

		[ES3Serializable]
		public bool IsOpenHideCostume;

		[ES3Serializable]
		public bool IsOpenEndTryal;

		[ES3Serializable]
		public HashSet<int> AlreadyViewCategorySet;
	// Recovered original member RVA 0x4BF270: .ctor
		public TutorialData()
        {
            AlreadyViewCategorySet = new HashSet<int>();
        }
	}
}
