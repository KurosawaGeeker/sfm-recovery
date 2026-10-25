using System.Collections.Generic;
using ExposureUnnoticed2.Master.BodyPaint;
using ExposureUnnoticed2.Master.Hair;

namespace ExposureUnnoticed2.ObjectUI.ClosetMenu.CustomizeMenu
{
	public class LoadoutCustomizeData
	{
		[ES3Serializable]
		private int CustomizeHeight;

		[ES3Serializable]
		private int CustomizeHead;

		[ES3Serializable]
		private int CustomizeNeck;

		[ES3Serializable]
		private int CustomizeNeckThick;

		[ES3Serializable]
		private int CustomizeBoobs;

		[ES3Serializable]
		private int CustomizeSuperBoobs;

		[ES3Serializable]
		private int CustomizeHyperBoobs;

		[ES3Serializable]
		private int CustomizeNipple;

		[ES3Serializable]
		private int CustomizeButt;

		[ES3Serializable]
		private int CustomizeWaist;

		[ES3Serializable]
		private int CustomizeLegInterval;

		[ES3Serializable]
		private int CustomizeThighs;

		[ES3Serializable]
		private int CustomizeThighsLength;

		[ES3Serializable]
		private int CustomizeLowerLeg;

		[ES3Serializable]
		private int CustomizeLowerLegLength;

		[ES3Serializable]
		private int CustomizeUpperBody;

		[ES3Serializable]
		private int CustomizeUpperBody2;

		[ES3Serializable]
		private int CustomizeUpperBodyLength;

		[ES3Serializable]
		private int CustomizeUpperBodyLength2;

		[ES3Serializable]
		private int CustomizeArm;

		[ES3Serializable]
		private int CustomizeShoulder;

		[ES3Serializable]
		private int CustomizeInmou1;

		[ES3Serializable]
		private int CustomizeInmou2;

		[ES3Serializable]
		private int CustomizeInmouColorR;

		[ES3Serializable]
		private int CustomizeInmouColorG;

		[ES3Serializable]
		private int CustomizeInmouColorB;

		[ES3Serializable]
		private int CustomizeSkinColor;

		[ES3Serializable]
		private bool CustomizeIsSkinColorFree;

		[ES3Serializable]
		private int CustomizeSkinColorR;

		[ES3Serializable]
		private int CustomizeSkinColorG;

		[ES3Serializable]
		private int CustomizeSkinColorB;

		[ES3Serializable]
		private int CustomizeFutanariSaoSize;

		[ES3Serializable]
		private int CustomizeFutanariTamaSize;

		[ES3Serializable]
		private bool CustomizeFutanariTamaVisible;

		[ES3Serializable]
		private int CustomizeFutanariStraight;

		[ES3Serializable]
		private int CustomizeFutanariKitouSize;

		[ES3Serializable]
		private int CustomizeFutanariWidth;

		[ES3Serializable]
		private int CustomizeEyeBlowColorR;

		[ES3Serializable]
		private int CustomizeEyeBlowColorG;

		[ES3Serializable]
		private int CustomizeEyeBlowColorB;

		[ES3Serializable]
		private int CustomizeEyeLashColorR;

		[ES3Serializable]
		private int CustomizeEyeLashColorG;

		[ES3Serializable]
		private int CustomizeEyeLashColorB;

		[ES3Serializable]
		private int CustomizeLoliFace;

		[ES3Serializable]
		private int CustomizeEyeOpenness;

		[ES3Serializable]
		private int CustomizeEyeSize;

		[ES3Serializable]
		private int CustomizeHitomiSize;

		[ES3Serializable]
		private int CustomizeTareme;

		[ES3Serializable]
		private int CustomizeEyeDist;

		[ES3Serializable]
		private int CustomizeEyeHeight;

		[ES3Serializable]
		private bool CustomizeEyeDark;

		[ES3Serializable]
		private bool CustomizeOddEye;

		[ES3Serializable]
		private int CustomizeMatsugeThick;

		[ES3Serializable]
		private int CustomizeMatsugeLength;

		[ES3Serializable]
		private int CustomizeMayuThick;

		[ES3Serializable]
		private int CustomizeMayuDog;

		[ES3Serializable]
		private int CustomizeCheekFat;

		[ES3Serializable]
		private int CustomizeEarElf;

		[ES3Serializable]
		private int CustomizeChinLength;

		[ES3Serializable]
		private int CustomizeEyePattern;

		[ES3Serializable]
		private int CustomizeEyeLeftPattern;

		[ES3Serializable]
		private bool CustomizeYaeba;

		[ES3Serializable]
		private HairType CurrentHair;

		[ES3Serializable]
		private MHairRecord.HairCustomizeData HairCustomize;

		[ES3Serializable]
		private Dictionary<BodyPaintType, bool> BodyPaintTypeDict;

		public void ApplyToGameState()
		{
		}

		public void SaveCurrentCustomize()
		{
			// Recovered from GameAssembly.dll RVA 0x531510
			CustomizeHeight = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHeight;
			CustomizeHead = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHead;
			CustomizeNeck = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNeck;
			CustomizeNeckThick = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNeckThick;
			CustomizeBoobs = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeBoobs;
			CustomizeSuperBoobs = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSuperBoobs;
			CustomizeHyperBoobs = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHyperBoobs;
			CustomizeNipple = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeNipple;
			CustomizeButt = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeButt;
			CustomizeWaist = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeWaist;
			CustomizeLegInterval = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLegInterval;
			CustomizeThighs = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeThighs;
			CustomizeThighsLength = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeThighsLength;
			CustomizeLowerLeg = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLowerLeg;
			CustomizeLowerLegLength = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLowerLegLength;
			CustomizeUpperBody = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBody;
			CustomizeUpperBody2 = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBody2;
			CustomizeUpperBodyLength = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBodyLength;
			CustomizeUpperBodyLength2 = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeUpperBodyLength2;
			CustomizeArm = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeArm;
			CustomizeShoulder = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeShoulder;
			CustomizeInmou1 = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou1;
			CustomizeInmou2 = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmou2;
			CustomizeInmouColorR = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorR;
			CustomizeInmouColorG = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorG;
			CustomizeInmouColorB = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeInmouColorB;
			CustomizeSkinColor = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColor;
			CustomizeIsSkinColorFree = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomIsSkinColorFree;
			CustomizeSkinColorR = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorR;
			CustomizeSkinColorG = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorG;
			CustomizeSkinColorB = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeSkinColorB;
			CustomizeFutanariSaoSize = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariSaoSize;
			CustomizeFutanariTamaSize = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariTamaSize;
			CustomizeFutanariTamaVisible = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariTamaVisible;
			CustomizeFutanariStraight = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariStraight;
			CustomizeFutanariKitouSize = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariKitouSize;
			CustomizeFutanariWidth = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeFutanariWidth;
			CustomizeEyeBlowColorR = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorR;
			CustomizeEyeBlowColorG = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorG;
			CustomizeEyeBlowColorB = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeBlowColorB;
			CustomizeEyeLashColorR = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorR;
			CustomizeEyeLashColorG = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorG;
			CustomizeEyeLashColorB = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLashColorB;
			CustomizeLoliFace = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeLoliFace;
			CustomizeEyeOpenness = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeOpenness;
			CustomizeEyeSize = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeSize;
			CustomizeHitomiSize = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeHitomiSize;
			CustomizeTareme = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeTareme;
			CustomizeEyeDist = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeDist;
			CustomizeEyeHeight = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeHeight;
			CustomizeEyeDark = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeDark;
			CustomizeOddEye = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeOddEye;
			CustomizeMatsugeThick = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMatsugeThick;
			CustomizeMatsugeLength = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMatsugeLength;
			CustomizeMayuThick = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMayuThick;
			CustomizeMayuDog = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeMayuDog;
			CustomizeCheekFat = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeCheekFat;
			CustomizeEarElf = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEarElf;
			CustomizeChinLength = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeChinLength;
			CustomizeEyePattern = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyePattern;
			CustomizeEyeLeftPattern = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeEyeLeftPattern;
			CustomizeYaeba = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CustomizeYaeba;
			CurrentHair = global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.CurrentHair;
			HairCustomize = new MHairRecord.HairCustomizeData(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.HairCustomizeDataDict[CurrentHair]);
			BodyPaintTypeDict = new Dictionary<BodyPaintType, bool>(global::ExposureUnnoticed2.Scripts.InGame.GameState.PlayerCustomData.BodyPaintTypeDict);
		}
	}
}
