using UnityEngine;

namespace ExposureUnnoticed2.Master.Common
{
	[CreateAssetMenu(fileName = "MCommon", menuName = "ScriptableObjects/MCommon")]
	public class MCommon : ScriptableObject
	{
		[Header("雑操作用")]
		[SerializeField]
		public float a;

		[SerializeField]
		public float b;

		[SerializeField]
		public float c;

		[SerializeField]
		public float d;

		[Header("カメラ")]
		[SerializeField]
		private float baseCameraSensitive;

		[SerializeField]
		private float cameraScrollSensitive;

		[SerializeField]
		public float EyeMaskCameraFarClip;

		[SerializeField]
		public float NormalCameraFarClip;

		[SerializeField]
		public Color EyeMaskFogColor;

		[SerializeField]
		public float EyeMaskFogNear;

		[SerializeField]
		public float EyeMaskFogFar;

		[SerializeField]
		public float EyeMaskFogDensity;

		[Header("プレイヤー")]
		[SerializeField]
		private float playerCrouchSpeed;

		[SerializeField]
		private float playerWalkSpeed;

		[SerializeField]
		public float PlayerGamanWalkSpeed;

		[SerializeField]
		private float playerJogSpeed;

		[SerializeField]
		private float playerMaxStamina;

		[SerializeField]
		private float playerStaminaRegeneration;

		[SerializeField]
		private float playerStaminaRegenerateSecond;

		[SerializeField]
		public float PlayerStaminaDashPerSecond;

		[SerializeField]
		private float pulseMentalReduce;

		[SerializeField]
		private float dotMentalExpose;

		[SerializeField]
		private float dotMentalWatched;

		[SerializeField]
		public float EquipLoadoutMotionTime;

		[SerializeField]
		[Header("おぱーい")]
		public DynamicBoneData SmallOppai;

		[SerializeField]
		public DynamicBoneData FreeOppai;

		[SerializeField]
		public DynamicBoneData BigOppai;

		[SerializeField]
		public DynamicBoneData BraOppai;

		[SerializeField]
		public DynamicBoneData ClothesOppai;

		[SerializeField]
		public bool IsControlOppaiByInspector;

		[SerializeField]
		public float DampingBigBreastScale;

		[SerializeField]
		public float ElasBigBreastScale;

		[SerializeField]
		public float BraStiff;

		[SerializeField]
		public float ClothStiff;

		[Header("価格")]
		[SerializeField]
		public int ChangeHairPrice;

		[SerializeField]
		public int ChangeHairColorPrice;

		[SerializeField]
		public int WashingPrice;

		[SerializeField]
		public int WaterPrice;

		[SerializeField]
		public int VibeRemoconReinforcePrice;

		[SerializeField]
		public int DroneReinforcePrice;

		[Header("汎用解除ギミック")]
		[SerializeField]
		public float AttachHandcuffsTime;

		[SerializeField]
		public float HandcuffsSpeed;

		[SerializeField]
		public float HandcuffsProbability;

		[SerializeField]
		public float HandcuffsAddProbabilityByFailure;

		[SerializeField]
		[Header("心拍数")]
		public float DokidokiOpenFront;

		[SerializeField]
		public float DokidokiOpenUpper;

		[SerializeField]
		public float DokidokiDropClothes;

		[SerializeField]
		public float DokidokiOpenHip;

		[SerializeField]
		public float DokidokiDropBra;

		[SerializeField]
		public float DokidokiDropPants;

		[SerializeField]
		public float DokidokiLight;

		[SerializeField]
		public float DokidokiAdultGoodsUpperLimit;

		[SerializeField]
		public float DokidokiPee;

		[SerializeField]
		[Header("不信度")]
		public float StrangeExposeFront;

		[SerializeField]
		public float StrangeExposeUpperFromFront;

		[SerializeField]
		public float StrangeExposeUpperFromBack;

		[SerializeField]
		public float StrangeExposeHip;

		[SerializeField]
		public float StrangeExposeAll;

		[SerializeField]
		public float StrangeVibratorLow;

		[SerializeField]
		public float StrangeVibratorHigh;

		[SerializeField]
		public float ListenNoisyDist;

		[SerializeField]
		public float MaxFoundDist;

		[SerializeField]
		[Header("NPC")]
		public float StrangeLookAt;

		[SerializeField]
		[Header("RP倍率")]
		public int NoPantsRpBonus;

		[SerializeField]
		public int NoBraRpBonus;

		[SerializeField]
		public int ClothesDistanceRpBonus;

		[SerializeField]
		public int DaytimeRpBonus;

		[SerializeField]
		public int NearNpcPerOneRpBonus;

		[SerializeField]
		public int LightRpBonus;

		[SerializeField]
		public int ExposeFrontRpBonus;

		[SerializeField]
		public int ExposeUpperRpBonus;

		[SerializeField]
		public int ExposeAllRpBonus;

		[SerializeField]
		public int ExposeHipRpBonus;

		[SerializeField]
		[Header("文字色")]
		public Color[] RpBonusColor;

		[SerializeField]
		public Color[] RpBonusColorSecondary;

		[SerializeField]
		[Header("その他")]
		public int WashingSecond;

		[SerializeField]
		public float AddMoistureWater;

		[SerializeField]
		public float TransMoisturePerSecond;

		[SerializeField]
		public float PeeAbleMoistureThreshold;

		[SerializeField]
		public float PeeMotionSecond;

		[SerializeField]
		public float AddMissionSatisfyPerSecond;

		[SerializeField]
		public Vector3 ApartAutoDropPosition;

		private static MCommon instance;

		public static float BaseCameraSensitive => Instance.baseCameraSensitive;

		public static float CameraScrollSensitive => Instance.cameraScrollSensitive;

		public static float PlayerCrouchSpeed => Instance.playerCrouchSpeed;

		public static float PlayerWalkSpeed => Instance.playerWalkSpeed;

		public static float PlayerJogSpeed => Instance.playerJogSpeed;

		public static float PlayerMaxStamina => Instance.playerMaxStamina;

		public static float PlayerStaminaRegeneration => Instance.playerStaminaRegeneration;

		public static float PlayerStaminaRegenerateSecond => Instance.playerStaminaRegenerateSecond;

		public static float DotMentalWatched => Instance.dotMentalWatched;

		public static float DotMentalExpose => Instance.dotMentalExpose;

		public static float PulseMentalReduce => Instance.pulseMentalReduce;

		public static MCommon Instance
        {
            get
            {
                if (instance == null)
                    instance = global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MCommon;
                return instance;
            }
        }
	}
}
