using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.ObjectUI.OptionMenu
{
	public class OptionData
	{
		[ES3Serializable]
		public bool FutanariDisable;

		[ES3Serializable]
		public bool PeeDisable;

		[ES3Serializable]
		public bool SexDisable;

		[ES3Serializable]
		public bool InteractEffectInvisible;

		[ES3Serializable]
		public int MaleRate100;

		[ES3Serializable]
		public int OldPeopleRate100;

		[ES3Serializable]
		public Localizer.Language Language;

		[ES3Serializable]
		public bool IsBlackCensor;

		[ES3Serializable]
		public int BlackCensorSize100;

		[ES3Serializable]
		public bool VibeStateColor;

		[ES3Serializable]
		public int NpcNumScaleMallDaytime;

		[ES3Serializable]
		public bool IsUseHsv;

		[ES3Serializable]
		public bool InvisibleRedWall;

		[ES3Serializable]
		public bool FutanariFemaleClimaxDisable;

		[ES3Serializable]
		public bool EkitaiDisable;

		[ES3Serializable]
		public bool FaceCamera;

		[ES3Serializable]
		public bool BodyCamera;

		[ES3Serializable]
		public bool Message;

		[ES3Serializable]
		public bool RingMenuMessage;

		[ES3Serializable]
		public bool PlayerInfo;

		[ES3Serializable]
		public bool EcstasyGauge;

		[ES3Serializable]
		public bool Compass;

		[ES3Serializable]
		public bool MissionPanel;

		[ES3Serializable]
		public bool RightDownPanel;

		[ES3Serializable]
		public bool InteractPanel;

		[ES3Serializable]
		public bool HeartBeatEffect;

		[ES3Serializable]
		public int HeartBeatEffectAlpha100;

		[ES3Serializable]
		public int MissionPanelSize100;

		[ES3Serializable]
		public bool Manual;

		[ES3Serializable]
		public int GamePadIconType;

		[ES3Serializable]
		public int SoundMaster100;

		[ES3Serializable]
		public int SoundSe100;

		[ES3Serializable]
		public int SoundUi100;

		[ES3Serializable]
		public int SoundBgm100;

		[ES3Serializable]
		public int SoundVibe100;

		[ES3Serializable]
		public int SoundHeartBeat100;

		[ES3Serializable]
		public bool MouseInverseX;

		[ES3Serializable]
		public bool MouseInverseY;

		[ES3Serializable]
		public bool PadInverseX;

		[ES3Serializable]
		public bool PadInverseY;

		[ES3Serializable]
		public bool WheelInverse;

		[ES3Serializable]
		public bool PadUpDownInverse;

		[ES3Serializable]
		public int MouseSensitive100;

		[ES3Serializable]
		public int WheelSensitive100;

		[ES3Serializable]
		public int PadSensitive100;

		[ES3Serializable]
		public int LongPressTimeScale10;

		[ES3Serializable]
		public int AspectRatioIndex;

		[ES3Serializable]
		public int Quality;

		[ES3Serializable]
		public int Resolusion;

		[ES3Serializable]
		public int WindowMode;

		[ES3Serializable]
		public int MaxFrameRate;

		[ES3Serializable]
		public int Brightness;

		[ES3Serializable]
		public int Contrast;

		[ES3Serializable]
		public int CameraAngle1st;

		[ES3Serializable]
		public int CameraAngle3rd;

		[ES3Serializable]
		public bool VSync;

		[ES3Serializable]
		public bool AntiAliasing;

		[ES3Serializable]
		public bool SSAO;

		[ES3Serializable]
		public bool SSR;

		[ES3Serializable]
		public bool Bloom;

		[ES3Serializable]
		public int BloomStrength;

		[ES3Serializable]
		public int BloomBiasDark;

		[ES3Serializable]
		public int BloomBiasLight;

		[ES3Serializable]
		public int ColorTemperature;

		[ES3Serializable]
		public bool MotionBlur;

		[ES3Serializable]
		public bool AnsweredFirstSexOption;

		[ES3Serializable]
		public bool IsSexMaleTranslucent;

		[ES3Serializable]
		public bool IsSexMaleOnlySao;

		public void ResetGraphics()
		{
			// Recovered from GameAssembly.dll RVA 0x521E50
			Quality = 2;
			Resolusion = 0;
			AspectRatioIndex = 0;
			WindowMode = 0;
			VSync = false;
			ColorTemperature = 0;
			MaxFrameRate = 60;
			Brightness = 50;
			Contrast = 50;
			AntiAliasing = true;
			SSAO = true;
			SSR = true;
			Bloom = true;
			BloomStrength = 30;
			BloomBiasLight = 100;
			BloomBiasDark = 100;
			MotionBlur = true;
			CameraAngle1st = 90;
			CameraAngle3rd = 85;
		}

		public void ResetFov()
		{
			// Recovered from GameAssembly.dll RVA 0x521EE0
			CameraAngle1st = 90;
			CameraAngle3rd = 85;
		}

		public void ResetControl()
		{
			// Recovered from GameAssembly.dll RVA 0x521F00
			MouseInverseX = false;
			MouseInverseY = false;
			PadInverseX = false;
			PadInverseY = false;
			WheelInverse = false;
			PadUpDownInverse = false;
			MouseSensitive100 = 20;
			WheelSensitive100 = 20;
			PadSensitive100 = 30;
			LongPressTimeScale10 = 10;
		}

		public void ResetAppearRate()
		{
			// Recovered from GameAssembly.dll RVA 0x521F30
			MaleRate100 = 50;
			OldPeopleRate100 = 10;
		}

		public void ResetSound()
		{
			// Recovered from GameAssembly.dll RVA 0x521F40
			SoundMaster100 = 50;
			SoundSe100 = 50;
			SoundUi100 = 50;
			SoundBgm100 = 50;
			SoundVibe100 = 100;
			SoundHeartBeat100 = 100;
		}
	// Recovered original member RVA 0x521D20: .ctor
		public OptionData()
        {
FutanariDisable = false;
PeeDisable = false;
SexDisable = true;
InteractEffectInvisible = false;
GamePadIconType = 0;
WheelInverse = false;
PadUpDownInverse = false;
WindowMode = 0;
VSync = false;
ColorTemperature = 0;
IsBlackCensor = false;
EkitaiDisable = false;
FaceCamera = true;
VibeStateColor = false;
NpcNumScaleMallDaytime = 100;
BlackCensorSize100 = 100;
IsUseHsv = false;
InvisibleRedWall = false;
MaleRate100 = 50;
OldPeopleRate100 = 10;
BodyCamera = true;
Message = true;
RingMenuMessage = true;
PlayerInfo = true;
EcstasyGauge = true;
Compass = true;
MissionPanel = true;
RightDownPanel = true;
InteractPanel = true;
HeartBeatEffect = true;
HeartBeatEffectAlpha100 = 100;
MissionPanelSize100 = 100;
Manual = true;
SoundMaster100 = 50;
SoundSe100 = 50;
SoundUi100 = 50;
SoundBgm100 = 50;
SoundVibe100 = 100;
SoundHeartBeat100 = 100;
MouseInverseX = false;
MouseInverseY = false;
PadInverseX = false;
PadInverseY = false;
MouseSensitive100 = 20;
WheelSensitive100 = 20;
PadSensitive100 = 30;
LongPressTimeScale10 = 10;
AspectRatioIndex = 0;
Quality = 2;
Resolusion = 0;
MaxFrameRate = 60;
Brightness = 50;
Contrast = 50;
AntiAliasing = true;
SSAO = true;
SSR = true;
Bloom = true;
BloomStrength = 30;
BloomBiasLight = 100;
BloomBiasDark = 100;
MotionBlur = true;
CameraAngle1st = 90;
CameraAngle3rd = 85;
        }
	}
}
