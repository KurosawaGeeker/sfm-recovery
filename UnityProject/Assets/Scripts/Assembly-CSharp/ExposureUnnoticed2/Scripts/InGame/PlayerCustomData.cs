using System.Collections.Generic;
using ExposureUnnoticed2.Master.BodyPaint;
using ExposureUnnoticed2.Master.Hair;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class PlayerCustomData
	{
		[ES3Serializable]
		public int CustomizeHeight;

		[ES3Serializable]
		public int CustomizeHead;

		[ES3Serializable]
		public int CustomizeNeck;

		[ES3Serializable]
		public int CustomizeNeckThick;

		[ES3Serializable]
		public int CustomizeBoobs;

		[ES3Serializable]
		public int CustomizeSuperBoobs;

		[ES3Serializable]
		public int CustomizeHyperBoobs;

		[ES3Serializable]
		public int CustomizeNipple;

		[ES3Serializable]
		public int CustomizeButt;

		[ES3Serializable]
		public int CustomizeWaist;

		[ES3Serializable]
		public int CustomizeLegInterval;

		[ES3Serializable]
		public int CustomizeThighs;

		[ES3Serializable]
		public int CustomizeThighsLength;

		[ES3Serializable]
		public int CustomizeLowerLeg;

		[ES3Serializable]
		public int CustomizeLowerLegLength;

		[ES3Serializable]
		public int CustomizeUpperBody;

		[ES3Serializable]
		public int CustomizeUpperBody2;

		[ES3Serializable]
		public int CustomizeUpperBodyLength;

		[ES3Serializable]
		public int CustomizeUpperBodyLength2;

		[ES3Serializable]
		public int CustomizeArm;

		[ES3Serializable]
		public int CustomizeShoulder;

		[ES3Serializable]
		public int CustomizeInmou1;

		[ES3Serializable]
		public int CustomizeInmou2;

		[ES3Serializable]
		public int CustomizeInmouColorR;

		[ES3Serializable]
		public int CustomizeInmouColorG;

		[ES3Serializable]
		public int CustomizeInmouColorB;

		[ES3Serializable]
		public int CustomizeSkinColor;

		[ES3Serializable]
		public int CustomizeSkinColorR;

		[ES3Serializable]
		public int CustomizeSkinColorG;

		[ES3Serializable]
		public int CustomizeSkinColorB;

		[ES3Serializable]
		public bool CustomIsSkinColorFree;

		[ES3Serializable]
		public int CustomizeFutanariSaoSize;

		[ES3Serializable]
		public int CustomizeFutanariTamaSize;

		[ES3Serializable]
		public int CustomizeFutanariStraight;

		[ES3Serializable]
		public int CustomizeFutanariKitouSize;

		[ES3Serializable]
		public int CustomizeFutanariWidth;

		[ES3Serializable]
		public bool CustomizeFutanariTamaVisible;

		[ES3Serializable]
		public int CustomizeEyeBlowColorR;

		[ES3Serializable]
		public int CustomizeEyeBlowColorG;

		[ES3Serializable]
		public int CustomizeEyeBlowColorB;

		[ES3Serializable]
		public int CustomizeEyeLashColorR;

		[ES3Serializable]
		public int CustomizeEyeLashColorG;

		[ES3Serializable]
		public int CustomizeEyeLashColorB;

		[ES3Serializable]
		public int CustomizeLoliFace;

		[ES3Serializable]
		public int CustomizeEyeOpenness;

		[ES3Serializable]
		public int CustomizeEyeSize;

		[ES3Serializable]
		public int CustomizeHitomiSize;

		[ES3Serializable]
		public int CustomizeTareme;

		[ES3Serializable]
		public int CustomizeEyeDist;

		[ES3Serializable]
		public int CustomizeEyeHeight;

		[ES3Serializable]
		public bool CustomizeEyeDark;

		[ES3Serializable]
		public bool CustomizeOddEye;

		[ES3Serializable]
		public int CustomizeMatsugeThick;

		[ES3Serializable]
		public int CustomizeMatsugeLength;

		[ES3Serializable]
		public int CustomizeMayuThick;

		[ES3Serializable]
		public int CustomizeMayuDog;

		[ES3Serializable]
		public int CustomizeCheekFat;

		[ES3Serializable]
		public int CustomizeEarElf;

		[ES3Serializable]
		public int CustomizeChinLength;

		[ES3Serializable]
		public int CustomizeEyePattern;

		[ES3Serializable]
		public int CustomizeEyeLeftPattern;

		[ES3Serializable]
		public bool CustomizeYaeba;

		[ES3Serializable]
		public HairType CurrentHair;

		[ES3Serializable]
		public Dictionary<BodyPaintType, bool> BodyPaintTypeDict;

		[ES3Serializable]
		public bool CustomizeAdjustHeight;

		[ES3Serializable]
		public Dictionary<HairType, MHairRecord.HairCustomizeData> HairCustomizeDataDict;
	// Recovered original member RVA 0x48D3E0: .ctor
		public PlayerCustomData()
        {
            CustomizeHeight = -999;
            CurrentHair = (HairType)0;
            BodyPaintTypeDict = new Dictionary<BodyPaintType, bool>();
            HairCustomizeDataDict = GameStateFacade.GetDefaultHairCustomDataDict();
        }
	}
}
