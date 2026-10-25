using ExposureUnnoticed2.ObjectUI.InGame.RingMenu;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class InputResolver
	{
		private Vector3 oldMove;

		public static RingMenuParentView.RingItemType RingShortcutInput;

		public static bool ShiftRing;

		private InputData inputData;

		public InputData Resolve()
		{
			// Recovered from GameAssembly.dll RVA 0x620800
			
			            // Native 620800: deliberately clear only the fields covered by original stores.
			            InputData target = inputData;
			            if (target is null) throw new global::System.NullReferenceException();
			            target.Move = Vector3.zero;
			            target.CameraMove = Vector2.zero;
			            target.JustPoseSkill = (RingMenuParentView.RingItemType)0;
			            target.JustDrinkWater = false;
			            target.JustEyeMask = false;
			            target.MoveForwardOneHand = false;
			            target.JustOpenClothesA = false;
			            target.JustCloseClothesA = false;
			            target.JustOpenClothesB = false;
			            target.JustSwitchPistonMachine = false;
			            target.JustRingDildo = false;
			            target.JustRingHandcuff = false;
			            target.JustRingEyeMask = false;
			            target.Interact = false;
			            target.JustInteract = false;
			            target.JustDrop = false;
			            target.JustDropHandcuffKey = false;
			            target.JustDropVibeRemocon = false;
			            target.JustAutoChase = false;
			            target.JustAutoRun = false;
			            target.JustTimeStop = false;
			            target.JustSwitchVibrator = false;
			            target.JustBra = false;
			            target.JustPants = false;
			            target.JustPee = false;
			            target.JustViewChange = false;
			            target.JustMove = false;
			            target.Dash = false;
			            target.JustCrouch = false;
			            target.Jump = false;
			            InputData result = inputData;
			            if (!global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenSomeCancelInputUi
			                && !global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenFade
			                && !global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsGameOver
			                && !global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsPlayingSomePerformance)
			            {
			                Vector3 source = global::ExposureUnnoticed2.Scripts.Base.InputManager.GetVector3((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)0);
			                Vector3 planar = new Vector3(source.x, 0f, source.z);
			                if (planar.magnitude >= 0.5f) planar = planar.normalized;
			                if (result is null) throw new global::System.NullReferenceException();
			                result.Move = planar;
			                result.MoveForwardOneHand = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDown((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)33);
			                if (!global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDown((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)34))
			                {
			                    result.JustOpenClothesA = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)5);
			                    result.JustOpenClothesB = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)12);
			                    result.JustCrouch = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)7);
			                    result.Dash = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDown((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)3);
			                    result.JustCloseClothesA = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)6);
			                    result.JustViewChange = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)4);
			                }
			                result.CameraMove = global::ExposureUnnoticed2.Scripts.Base.InputManager.GetVector2((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1);
			                result.JustCancel = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)10);
			                result.HoldCommonRingMenu = global::ExposureUnnoticed2.Scripts.Base.InputManager.GetLongDown((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)14, 10);
			                result.HoldSkillRingMenu = global::ExposureUnnoticed2.Scripts.Base.InputManager.GetLongDown((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)23, 10);
			                result.JustSelect = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)15);
			                result.JustSystemMenu = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)16);
			                result.JustTabLeft = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)20);
			                result.JustTabRight = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)21);
			                result.JustSlowSkill = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)22);
			                result.JustDrinkWater = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1012);
			                result.JustEyeMask = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)1016);
			                if ((int)RingShortcutInput != 0)
			                {
			                    if (!global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDown((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)34) || ShiftRing)
			                    {
			                        switch ((int)RingShortcutInput)
			                        {
			                            case 1: result.JustPee = true; break;
			                            case 2: result.JustBra = true; break;
			                            case 3: result.JustPants = true; break;
			                            case 4: result.JustSwitchVibrator = true; break;
			                            case 5: result.JustDrop = true; break;
			                            case 6: result.JustDropHandcuffKey = true; break;
			                            case 7: result.JustDropVibeRemocon = true; break;
			                            case 8: result.JustAutoRun = true; break;
			                            case 9: result.JustAutoChase = true; break;
			                            case 10: result.JustTimeStop = true; break;
			                            case 11: result.JustSwitchPistonMachine = true; break;
			                            case 12: result.JustRingDildo = true; break;
			                            case 13: result.JustRingHandcuff = true; break;
			                            case 14: result.JustRingEyeMask = true; break;
			                        }
			                        if ((int)RingShortcutInput > 9999) result.JustPoseSkill = RingShortcutInput;
			                    }
			                    RingShortcutInput = (RingMenuParentView.RingItemType)0;
			                }
			                if (!global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.isOpenSomeAllowInputUi
			                    && !global::ExposureUnnoticed2.Object3D.IngameManager.InGameUiManager.Instance.IsOpenSomeCancelInputUi
			                    && !global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDown((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)34))
			                {
			                    result.JustInteract = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)8);
			                    result.Interact = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDown((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)8);
			                }
			                if (global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)18))
			                {
			                    var facade = PlayerFacade.Instance;
			                    if (facade is null) throw new global::System.NullReferenceException();
			                    int exposed = (int)facade.GetExposedBodyPartsByClothesAndCostume();
			                    global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
			                    if (exposed != 1) result.Dash = false;
			                }
			                result.JustMove = oldMove.sqrMagnitude <= 0f && result.Move.sqrMagnitude > 0f;
			                oldMove = result.Move;
			                return result;
			            }
			            if (global::ExposureUnnoticed2.Scripts.InGame.SexManager.Instance.IsSexMode)
			            {
			                bool close = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)6);
			                if (result is null) throw new global::System.NullReferenceException();
			                result.JustCloseClothesA = close;
			                if (!global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDown((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)34))
			                {
			                    result.JustViewChange = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)4);
			                    result.JustOpenClothesA = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)5);
			                    result.JustOpenClothesB = global::ExposureUnnoticed2.Scripts.Base.InputManager.IsDownFrame((global::ExposureUnnoticed2.Scripts.Base.InputManager.InputType)12);
			                }
			            }
			            RingShortcutInput = (RingMenuParentView.RingItemType)0;
			            return result;
		}

		public static bool IsPoseSkillInput(RingMenuParentView.RingItemType type)
		{
			return false;
		}
	// Recovered original member RVA 0x621080: .ctor
		public InputResolver()
        {
            inputData = new InputData();
        }
	}
}
