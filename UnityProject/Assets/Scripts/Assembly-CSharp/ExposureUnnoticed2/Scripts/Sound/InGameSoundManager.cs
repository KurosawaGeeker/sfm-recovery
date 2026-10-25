using System;
using ExposureUnnoticed2.Resources.Master.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Sound
{
	public class InGameSoundManager : MonoBehaviour
	{
		public enum SeType
		{
			NoType = 0,
			FootStepHeel = 1,
			FootStepHeelJog = 2,
			ClothesSound = 3,
			ClothesDrop = 4,
			HeartBeat = 5,
			FootStepHeelCrouch = 6,
			Kuchu = 7,
			Shiofuki = 8,
			DokinHigh = 9,
			DokinLow = 10,
			Ok = 11,
			Close = 12,
			BuyMachine1 = 13,
			BuyMachine2 = 14,
			Drink = 15,
			PeeNormal = 16,
			Equip = 17,
			SleepBed = 18,
			Failed = 19,
			Picking = 20,
			Unlock = 21,
			AttachHandcuffs = 22,
			RankUp = 23,
			SuburbsDaytime = 24,
			SyburbsNight = 25,
			MyRoom = 26,
			KillTime = 27,
			BaibuLow = 28,
			BaibuHigh = 29,
			Barefoot = 30,
			FootStepSneaker = 31,
			BraAttach = 32,
			BraDetach = 33,
			PantsDetach = 34,
			VibratorSwitch = 35,
			Breath1 = 36,
			Breath2 = 37,
			SlowIn = 38,
			SlowOut = 39,
			Gogogo = 40,
			Reinforce = 41,
			CommonEquip = 42,
			ShoppingMallBgm = 43,
			Piston = 44,
			PutDildo = 45,
			CollectAll = 46,
			Buy = 47,
			UiSelect = 48,
			UiDecide = 49,
			UiCancel = 50,
			UiSlideOpen = 51,
			UiSlideClose = 52,
			OpenDoor = 53,
			CloseDoor = 54,
			Pinpon = 55,
			CarIdling = 56,
			CarStart = 57,
			CarStop = 58,
			CarPass = 59,
			BgmParkDayTime = 60,
			BgmParkNight = 61,
			FootStepGrass = 62,
			FootStepGravel = 63,
			ToiletDoorOpen = 64,
			ToiletDoorClose = 65,
			ToiletFutaOpen = 66,
			ToiletFutaClose = 67,
			FootStepMetal = 68,
			MentalAlert = 69,
			Drone = 70,
			ShopDoor = 71,
			ShoppingMallNight = 72,
			ElevatorMove = 73,
			ElevatorOpen = 74,
			ElevatorClose = 75,
			ElevatorBell = 76,
			Regi = 77,
			MansionPostReceiveOpen = 78,
			MansionPostReceiveClose = 79,
			ToiletPeeWater = 80,
			ToiletHandWash = 81,
			ToiletLock = 82,
			ToiletUnlock = 83,
			ToiletWater = 84,
			BgmFashionShop = 85,
			CurtainOpen = 86,
			CurtainClose = 87,
			WasherOpen = 88,
			WasherClose = 89,
			WasherStart = 90,
			WasherLoop = 91,
			WasherBeep = 92,
			Syasei = 93,
			TimeStop = 94,
			FootStepSneakerJog = 95,
			Rotor = 96,
			UICompleteMission = 97
		}

		[Serializable]
		public struct AudioData
		{
			public AudioClip Clip;

			public float Volume;
		}

		public static InGameSoundManager Instance;

		[Header("インゲーム音源")]
		[SerializeField]
		private AudioClip[] footStepHeel;

		[SerializeField]
		private AudioClip[] footStepHeelJog;

		[SerializeField]
		private AudioClip[] footStepHeelCrouch;

		[SerializeField]
		private AudioClip[] footStepBarefootCrouch;

		[SerializeField]
		private AudioClip[] footStepSneaker;

		[SerializeField]
		private AudioClip[] footStepSneakerJog;

		[SerializeField]
		private AudioClip[] footStepGrass;

		[SerializeField]
		private AudioClip[] footStepGravel;

		[SerializeField]
		private AudioClip[] footStepMetal;

		[SerializeField]
		private AudioClip ClothesSound;

		[SerializeField]
		private AudioClip ClothesDrop;

		[SerializeField]
		private AudioClip[] HeartBeat;

		[SerializeField]
		private AudioClip[] kuchu;

		[SerializeField]
		private AudioClip[] piston;

		[SerializeField]
		private AudioClip shiofuki;

		[SerializeField]
		private AudioClip dokinHigh;

		[SerializeField]
		private AudioClip dokinLow;

		[SerializeField]
		private AudioClip buyMachine1;

		[SerializeField]
		private AudioClip buyMachine2;

		[SerializeField]
		private AudioClip drink;

		[SerializeField]
		private AudioClip peeNormal;

		[SerializeField]
		private AudioClip Equip;

		[SerializeField]
		private AudioData EquipAd;

		[SerializeField]
		private AudioData picking;

		[SerializeField]
		private AudioData attackHandcuffs;

		[SerializeField]
		private AudioData unlock;

		[SerializeField]
		private AudioData braAttach;

		[SerializeField]
		private AudioData braDetach;

		[SerializeField]
		private AudioData pantsDetach;

		[SerializeField]
		private AudioData vibratorSwitch;

		[SerializeField]
		private AudioData breath1;

		[SerializeField]
		private AudioData breath2;

		[SerializeField]
		private AudioData slowIn;

		[SerializeField]
		private AudioData slowOut;

		[SerializeField]
		private AudioData gogogo;

		[SerializeField]
		private AudioData reinforce;

		[SerializeField]
		private AudioData commonEquip;

		[SerializeField]
		private AudioData putDildo;

		[SerializeField]
		private AudioData openDoor;

		[SerializeField]
		private AudioData closeDoor;

		[SerializeField]
		private AudioData pinpon;

		[SerializeField]
		private AudioData carIdling;

		[SerializeField]
		private AudioData carStart;

		[SerializeField]
		private AudioData carStop;

		[SerializeField]
		private AudioData carPass;

		[SerializeField]
		private AudioData mentalAlert;

		[SerializeField]
		private AudioData Drone;

		[SerializeField]
		private AudioData ShopDoor;

		[SerializeField]
		private AudioData elevatorMove;

		[SerializeField]
		private AudioData elevatorOpen;

		[SerializeField]
		private AudioData elevatorClose;

		[SerializeField]
		private AudioData elevatorBell;

		[SerializeField]
		private AudioData regi;

		[SerializeField]
		private AudioData MansionPostReceiveOpen;

		[SerializeField]
		private AudioData MansionPostReceiveClose;

		[SerializeField]
		private AudioData WasherOpen;

		[SerializeField]
		private AudioData WasherClose;

		[SerializeField]
		private AudioData WasherStart;

		[SerializeField]
		private AudioData WasherLoop;

		[SerializeField]
		private AudioData WasherBeep;

		[SerializeField]
		private AudioData Syasei;

		[SerializeField]
		private AudioData TimeStop;

		[Header("おもちゃ")]
		[SerializeField]
		private AudioData baibuLow;

		[SerializeField]
		private AudioData baibuHigh;

		[SerializeField]
		private AudioData rotor;

		[SerializeField]
		[Header("Plops")]
		private AudioData ToiletDoorOpen;

		[SerializeField]
		private AudioData ToiletDoorClose;

		[SerializeField]
		private AudioData ToiletFutaOpen;

		[SerializeField]
		private AudioData ToiletFutaClose;

		[SerializeField]
		private AudioData ToiletPeeWater;

		[SerializeField]
		private AudioData ToiletHandWash;

		[SerializeField]
		private AudioData ToiletLock;

		[SerializeField]
		private AudioData ToiletUnlock;

		[SerializeField]
		private AudioData ToiletWater;

		[SerializeField]
		private AudioData CurtainOpen;

		[SerializeField]
		private AudioData CurtainClose;

		[Header("ジングル")]
		[SerializeField]
		private AudioClip sleepBed;

		[SerializeField]
		private AudioData Failed;

		[SerializeField]
		private AudioData RankUp;

		[SerializeField]
		private AudioData KillTime;

		[SerializeField]
		private AudioData collectAll;

		[Header("BGM")]
		[SerializeField]
		private AudioData suburbsDaytime;

		[SerializeField]
		private AudioData suburbsNight;

		[SerializeField]
		private AudioData parkDaytime;

		[SerializeField]
		private AudioData parkNight;

		[SerializeField]
		private AudioData myRoom;

		[SerializeField]
		private AudioData shoppingMallBgm;

		[SerializeField]
		private AudioData shoppingMallBgmNight;

		[SerializeField]
		private AudioData bgmFashionShop;

		[Header("UI音源")]
		[SerializeField]
		private AudioData Select;

		[SerializeField]
		private AudioData Decide;

		[SerializeField]
		private AudioData Cancel;

		[SerializeField]
		private AudioData Buy;

		[SerializeField]
		private AudioData SlideOpen;

		[SerializeField]
		private AudioData SlideClose;

		[SerializeField]
		private AudioData CompleteMission;

		private SeType[] FootSes;

		private SeType[] GroupSes;

		private SoundGroupController footStepGroup;

		private SoundGroupController footStepHeelJogGroup;

		private SoundGroupController footStepHeelCrouchGroup;

		private SoundGroupController footStepBarefootCrouchGroup;

		private SoundGroupController footStepSneakerGroup;

		private SoundGroupController footStepSneakerJogGroup;

		private SoundGroupController footStepGrassGroup;

		private SoundGroupController footStepGravelGroup;

		private SoundGroupController footStepMetalGroup;

		private SoundGroupController kuchuGroup;

		private SoundGroupController pistonGroup;

		public bool IsGroupSe(SeType type)
		{
			// Recovered from GameAssembly.dll RVA 0x475610
			foreach (SeType entry in Instance.GroupSes) if (entry == type) return true;
			return false;
		}

		public bool IsFootSe(SeType type)
		{
			// Recovered from GameAssembly.dll RVA 0x4756A0
			foreach (SeType entry in Instance.FootSes) if (entry == type) return true;
			return false;
		}

		public AudioData GetAudioData(SeType type)
		{
			// Recovered from GameAssembly.dll RVA 0x475730
			return type switch
			{
			    SeType.SuburbsDaytime => suburbsDaytime,
			    SeType.SyburbsNight => suburbsNight,
			    SeType.MyRoom => myRoom,
			    SeType.BaibuLow => baibuLow,
			    SeType.BaibuHigh => baibuHigh,
			    SeType.ShoppingMallBgm => shoppingMallBgm,
			    SeType.BgmParkDayTime => parkDaytime,
			    SeType.BgmParkNight => parkNight,
			    SeType.ShoppingMallNight => shoppingMallBgmNight,
			    SeType.BgmFashionShop => bgmFashionShop,
			    SeType.WasherStart => WasherStart,
			    SeType.WasherLoop => WasherLoop,
			    SeType.Rotor => rotor,
			    _ => throw new global::System.Runtime.CompilerServices.SwitchExpressionException(type)
			};
		}

		public void PlayOneShotAudioData(AudioSource audioSource, SeType type, float time = -1f)
		{
			// Recovered from GameAssembly.dll RVA 0x4758A0
			AudioData data = type switch
			{
			    (SeType)17 => EquipAd,
			    (SeType)19 => Failed,
			    (SeType)20 => picking,
			    (SeType)21 => unlock,
			    (SeType)22 => attackHandcuffs,
			    (SeType)23 => RankUp,
			    (SeType)27 => KillTime,
			    (SeType)32 => braAttach,
			    (SeType)33 => braDetach,
			    (SeType)34 => pantsDetach,
			    (SeType)35 => vibratorSwitch,
			    (SeType)38 => slowIn,
			    (SeType)39 => slowOut,
			    (SeType)40 => gogogo,
			    (SeType)41 => reinforce,
			    (SeType)42 => commonEquip,
			    (SeType)45 => putDildo,
			    (SeType)46 => collectAll,
			    (SeType)47 => Buy,
			    (SeType)48 => Select,
			    (SeType)49 => Decide,
			    (SeType)50 => Cancel,
			    (SeType)51 => SlideOpen,
			    (SeType)52 => SlideClose,
			    (SeType)53 => openDoor,
			    (SeType)54 => closeDoor,
			    (SeType)55 => pinpon,
			    (SeType)56 => carIdling,
			    (SeType)57 => carStart,
			    (SeType)58 => carStop,
			    (SeType)59 => carPass,
			    (SeType)64 => ToiletDoorOpen,
			    (SeType)65 => ToiletDoorClose,
			    (SeType)66 => ToiletFutaOpen,
			    (SeType)67 => ToiletFutaClose,
			    (SeType)69 => mentalAlert,
			    (SeType)70 => Drone,
			    (SeType)71 => ShopDoor,
			    (SeType)73 => elevatorMove,
			    (SeType)74 => elevatorOpen,
			    (SeType)75 => elevatorClose,
			    (SeType)76 => elevatorBell,
			    (SeType)77 => regi,
			    (SeType)78 => MansionPostReceiveOpen,
			    (SeType)79 => MansionPostReceiveClose,
			    (SeType)80 => ToiletPeeWater,
			    (SeType)81 => ToiletHandWash,
			    (SeType)82 => ToiletLock,
			    (SeType)83 => ToiletUnlock,
			    (SeType)84 => ToiletWater,
			    (SeType)86 => CurtainOpen,
			    (SeType)87 => CurtainClose,
			    (SeType)88 => WasherOpen,
			    (SeType)89 => WasherClose,
			    (SeType)90 => WasherStart,
			    (SeType)91 => WasherLoop,
			    (SeType)92 => WasherBeep,
			    (SeType)93 => Syasei,
			    (SeType)94 => TimeStop,
			    (SeType)97 => CompleteMission,
			    _ => throw new global::System.Runtime.CompilerServices.SwitchExpressionException(type)
			};
			audioSource.PlayOneShot(data.Clip, data.Volume);
			if (0f <= time) global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayUnscaledTime(() => audioSource.Stop(), time);
		}

		public AudioClip GetSound(SeType seTypeType)
		{
			// Recovered from GameAssembly.dll RVA 0x475E40
			return seTypeType switch
			{
			    SeType.ClothesSound => ClothesSound,
			    SeType.ClothesDrop => ClothesDrop,
			    SeType.Shiofuki => shiofuki,
			    SeType.DokinHigh => dokinHigh,
			    SeType.DokinLow => dokinLow,
			    SeType.BuyMachine1 => buyMachine1,
			    SeType.BuyMachine2 => buyMachine2,
			    SeType.Drink => drink,
			    SeType.PeeNormal => peeNormal,
			    SeType.Equip => Equip,
			    SeType.SleepBed => sleepBed,
			    SeType.Breath1 => breath1.Clip,
			    SeType.Breath2 => breath2.Clip,
			    _ => null
			};
		}

		public AudioClip GetSound(SeType seTypeType, GameObject target)
		{
			// Recovered from GameAssembly.dll RVA 0x475F20
			return seTypeType switch
			{
			    SeType.FootStepHeel => footStepGroup.Get(target),
			    SeType.FootStepHeelJog => footStepHeelJogGroup.Get(target),
			    SeType.FootStepHeelCrouch => footStepHeelCrouchGroup.Get(target),
			    SeType.Kuchu => kuchuGroup.Get(target),
			    SeType.DokinLow => dokinLow,
			    SeType.Barefoot => footStepBarefootCrouchGroup.Get(target),
			    SeType.FootStepSneaker => footStepSneakerGroup.Get(target),
			    SeType.Piston => pistonGroup.Get(target),
			    SeType.FootStepGrass => footStepGrassGroup.Get(target),
			    SeType.FootStepGravel => footStepGravelGroup.Get(target),
			    SeType.FootStepMetal => footStepMetalGroup.Get(target),
			    SeType.FootStepSneakerJog => footStepSneakerJogGroup.Get(target),
			    _ => null
			};
		}

		public AudioClip GetSound(SeType seTypeType, int index)
		{
			// Recovered from GameAssembly.dll RVA 0x476120
			return seTypeType == SeType.HeartBeat ? HeartBeat[index] : null;
		}

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x476160
			if (Instance != null)
			{
			    UnityEngine.Object.Destroy(this);
			    return;
			}
			Instance = this;
			footStepGroup = new SoundGroupController(footStepHeel, SoundGroupController.GetType.Random);
			footStepHeelJogGroup = new SoundGroupController(footStepHeelJog, SoundGroupController.GetType.Random);
			footStepHeelCrouchGroup = new SoundGroupController(footStepHeelCrouch, SoundGroupController.GetType.Random);
			footStepBarefootCrouchGroup = new SoundGroupController(footStepBarefootCrouch, SoundGroupController.GetType.Random);
			footStepSneakerGroup = new SoundGroupController(footStepSneaker, SoundGroupController.GetType.Random);
			footStepSneakerJogGroup = new SoundGroupController(footStepSneakerJog, SoundGroupController.GetType.Random);
			footStepGrassGroup = new SoundGroupController(footStepGrass, SoundGroupController.GetType.Random);
			footStepGravelGroup = new SoundGroupController(footStepGravel, SoundGroupController.GetType.Random);
			footStepMetalGroup = new SoundGroupController(footStepMetal, SoundGroupController.GetType.Random);
			kuchuGroup = new SoundGroupController(kuchu, SoundGroupController.GetType.Random);
			pistonGroup = new SoundGroupController(piston, SoundGroupController.GetType.Random);
		}

		public void RefreshState()
		{
			// Recovered from GameAssembly.dll RVA 0x4768A0
			footStepSneakerGroup.Reset();
		}
	// Recovered original member RVA 0x476910: .ctor
		public InGameSoundManager()
        {
            FootSes = new SeType[] { (SeType)1, (SeType)2, (SeType)30, (SeType)31, (SeType)62, (SeType)63, (SeType)95 };
            GroupSes = new SeType[] { (SeType)7, (SeType)44 };
        }
	}
}
