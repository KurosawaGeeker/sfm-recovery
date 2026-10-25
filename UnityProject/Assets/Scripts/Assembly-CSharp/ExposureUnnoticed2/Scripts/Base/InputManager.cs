using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class InputManager : MonoBehaviour
	{
		public enum InputType
		{
			None = -1,
			Move = 0,
			CameraMove = 1,
			GameMenu = 2,
			Dash = 3,
			ViewChange = 4,
			OpenClothesA = 5,
			CloseClothesA = 6,
			Crouch = 7,
			Interact = 8,
			Drop = 9,
			Cancel = 10,
			LeftClick = 11,
			OpenClothesB = 12,
			RightClick = 13,
			CommonRingMenu = 14,
			Select = 15,
			SystemMenu = 16,
			MiddleClick = 17,
			DropHandcuffKey = 18,
			DropVibeRemocon = 19,
			TabLeft = 20,
			TabRight = 21,
			SlowSkill = 22,
			SkillRingMenu = 23,
			Tab2Left = 24,
			Tab2Right = 25,
			Accept = 26,
			SkillRingMenu3 = 27,
			SkillRingMenu4 = 28,
			MotionSpeedUp = 29,
			MotionSpeedDown = 30,
			WalkSpeedUp = 31,
			WalkSpeedDown = 32,
			MoveForwardOneHand = 33,
			PadShift = 34,
			UIDecide = 1000,
			UIUp = 1001,
			UIDown = 1002,
			UILeft = 1003,
			UIRight = 1004,
			UIScrollA = 1005,
			UIScrollB = 1006,
			UIDecideB = 1007,
			Inventory = 1008,
			Mission = 1009,
			UiRingLeft = 1010,
			UiRingRight = 1011,
			DrinkWater = 1012,
			Costume = 1013,
			MySet = 1014,
			AdultGoods = 1015,
			EyeMask = 1016,
			UIDecideC = 1017,
			SkillRingMenu5 = 1018,
			SkillRingMenu6 = 1019,
			SkillRingMenu7 = 1020,
			SkillRingMenu8 = 1021
		}

		private const int LongPressCount = 20;

		private static bool isExist;

		private const int MouseIndexBias = 100;

		private static Dictionary<InputType, string> inputKeyDict = new Dictionary<InputType, string>();

		private static PlayerInput playerInput;

		private static List<InputType> allInputTypeList;

		private static Dictionary<InputType, int> inputDict;

		private static Dictionary<InputType, int> oldInputDict;

		private bool isAlreadyDoneUpdateThisFrame;

		private const float SampleDeltaTime = 0.016f;

		private static float averageDeltaTime = 0.016f;

		private int oldMaxFrameRate = -1;

		public static InputManager Instance { get; private set; }

		public static bool IsGamePad { get; private set; }

		public static bool IsJustToGamePad { get; private set; }

		public static bool JustChangeIsGamePad { get; private set; }

		private void Awake()
		{
			// Recovered from GameAssembly.dll RVA 0x4AF100
			if (isExist)
			{
			    UnityEngine.Object.Destroy(gameObject);
			    return;
			}
			isExist = true;
			Instance = this;
			inputKeyDict = global::System.Linq.Enumerable.ToDictionary(
			    global::System.Linq.Enumerable.Cast<InputType>(global::System.Enum.GetValues(typeof(InputType))),
			    k => k, v => v.ToString());
			playerInput = GetComponent<PlayerInput>();
			// Original preparatory ReadValue<Vector2> result is discarded; this is not Enable().
			_ = playerInput.currentActionMap["Move"].ReadValue<Vector2>();
			allInputTypeList = global::System.Linq.Enumerable.ToList(
			    global::System.Linq.Enumerable.Cast<InputType>(global::System.Enum.GetValues(typeof(InputType))));
			inputDict = new Dictionary<InputType, int>();
			oldInputDict = new Dictionary<InputType, int>();
			foreach (InputType type in allInputTypeList)
			{
			    if (type == InputType.None) continue;
			    inputDict.Add(type, 0);
			    oldInputDict.Add(type, 0);
			}
		}

		private void OnDestroy()
		{
			// Recovered from GameAssembly.dll RVA 0x4AFD10
			if (Instance == this) Instance = null;
		}

		public void OnUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x4AFEB0
			UpdateProcess();
		}

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x4AFEB0
			UpdateProcess();
		}

		private void UpdateProcess()
		{
			// Recovered from GameAssembly.dll RVA 0x4AFEC0
			if (isAlreadyDoneUpdateThisFrame) return;
			isAlreadyDoneUpdateThisFrame = true;
			if (oldMaxFrameRate != Application.targetFrameRate)
			{
			    oldMaxFrameRate = Application.targetFrameRate;
			    averageDeltaTime = 1f / Application.targetFrameRate;
			}
			if (Time.unscaledDeltaTime < 0.5f)
			    averageDeltaTime = (Time.unscaledDeltaTime - averageDeltaTime) * 0.01f + averageDeltaTime;
			foreach (InputType type in allInputTypeList)
			{
			    if (type != InputType.None) oldInputDict[type] = inputDict[type];
			}
			foreach (InputType type in allInputTypeList)
			{
			    if (type == InputType.None) continue;
			    if (IsDown(type))
			    {
			        if (inputDict[type] < 0) inputDict[type] = 0;
			        inputDict[type] = unchecked(inputDict[type] + 1);
			    }
			    else
			    {
			        if (inputDict[type] > 0) inputDict[type] = 0;
			        inputDict[type] = unchecked(inputDict[type] - 1);
			    }
			}
			bool oldIsGamePad = IsGamePad;
			// Native invalid user/missing scheme/name paths explicitly raise NullReferenceException.
			if (!playerInput.user.valid) throw new global::System.NullReferenceException();
			var scheme = playerInput.user.controlScheme;
			if (!scheme.HasValue || scheme.Value.name is null) throw new global::System.NullReferenceException();
			IsGamePad = scheme.Value.name[0] == 'G';
			IsJustToGamePad = oldIsGamePad != IsGamePad && IsGamePad;
			JustChangeIsGamePad = oldIsGamePad != IsGamePad;
			if (!JustChangeIsGamePad) return;
			if (IsGamePad)
			{
			    if (global::ExposureUnnoticed2.ObjectUI.NoPadIcon.NoPadIconView.Instance == null)
			        CursorController.SetVisible(false);
			}
			else if ((global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance != null
			          && (global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.isOpenSomeAllowInputUi
			              || global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenSomeCancelInputUi))
			         || global::ExposureUnnoticed2.Scripts.UI.TitleSceneView.Instance != null)
			{
			    CursorController.SetVisible(true);
			}
		}

		public void OnFixedUpdate()
		{
		}

		private void LateUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x422E80
			isAlreadyDoneUpdateThisFrame = false;
		}

		public static Vector3 GetVector3(InputType type)
		{
			// Recovered from GameAssembly.dll RVA 0x4B0F80
			if (type != InputType.Move) throw new global::System.ArgumentOutOfRangeException(nameof(type), type, null);
			Vector2 value = playerInput.currentActionMap["Move"].ReadValue<Vector2>();
			return new Vector3(value.x, 0f, value.y);
		}

		public static Vector2 GetVector2(InputType type)
		{
			// Recovered from GameAssembly.dll RVA 0x4B1100
			if (type == InputType.CameraMove)
			{
			    if (IsGamePad)
			    {
			        Vector2 value = playerInput.currentActionMap["CameraMove"].ReadValue<Vector2>();
			        return new Vector2((value.x > 0f ? 1f : -1f) * value.x * value.x,
			                           value.y * value.y * (value.y > 0f ? 1f : -1f));
			    }
			    return playerInput.currentActionMap["CameraMove"].ReadValue<Vector2>();
			}
			if (type == InputType.UIScrollA) return playerInput.currentActionMap["UIScrollA"].ReadValue<Vector2>();
			if (type == InputType.UIScrollB) return playerInput.currentActionMap["UIScrollB"].ReadValue<Vector2>();
			throw new global::System.ArgumentOutOfRangeException(nameof(type), type, null);
		}

		public static bool IsDownLongRepeat(InputType type, int overCount = 20, int interval = 5)
		{
			// Recovered from GameAssembly.dll RVA 0x4B13F0
			float threshold = overCount * SampleDeltaTime / averageDeltaTime;
			threshold = threshold < 1f ? 1f : threshold;
			float repeat = interval * SampleDeltaTime / averageDeltaTime;
			repeat = repeat < 1f ? 1f : repeat;
			return inputDict[type] >= (int)threshold && inputDict[type] % (int)repeat == 0;
		}

		public static bool IsDownFrame(InputType type)
		{
			// Recovered from GameAssembly.dll RVA 0x4B1530
			return playerInput.currentActionMap[inputKeyDict[type]].WasPressedThisFrame();
		}

		public static bool IsUpFrame(InputType type)
		{
			// Recovered from GameAssembly.dll RVA 0x4B1680
			return playerInput.currentActionMap[inputKeyDict[type]].WasReleasedThisFrame();
		}

		public static bool IsDown(InputType type)
		{
			// Recovered from GameAssembly.dll RVA 0x4B17D0
			return playerInput.currentActionMap[inputKeyDict[type]].IsPressed();
		}

		public static bool GetLongDown(InputType type, int longCount = 20)
		{
			// Recovered from GameAssembly.dll RVA 0x4B18E0
			float threshold = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.LongPressTimeScale10 * 0.1f * longCount * SampleDeltaTime / averageDeltaTime;
			threshold = threshold < 1f ? 1f : threshold;
			return inputDict[type] >= (int)threshold;
		}

		public static bool GetShortUp(InputType type, int longCount = 20)
		{
			// Recovered from GameAssembly.dll RVA 0x4B19F0
			float threshold = global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.LongPressTimeScale10 * 0.1f * longCount * SampleDeltaTime / averageDeltaTime;
			threshold = threshold < 1f ? 1f : threshold;
			return oldInputDict[type] > 0 && oldInputDict[type] < (int)threshold && inputDict[type] == -1;
		}

		public static InputAction GetInputAction(InputType inputType)
		{
			// Recovered from GameAssembly.dll RVA 0x4B1BA0
			return playerInput.currentActionMap[inputKeyDict[inputType]];
		}

		public static string GetString(InputType inputType)
		{
			// Recovered from GameAssembly.dll RVA 0x4B1C50
			return GetInputAction(inputType).GetBindingDisplayString(0, out _, out _);
		}
	// Recovered original member RVA 0x4B1D70: .cctor
		// Recovered original .cctor RVA 0x4B1D70: ordered field initializers preserve BeforeFieldInit.
	}
}
