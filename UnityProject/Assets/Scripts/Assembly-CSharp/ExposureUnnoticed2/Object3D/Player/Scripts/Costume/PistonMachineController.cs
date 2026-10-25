using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Costume
{
	public class PistonMachineController
	{
		private PlayerClassAccessor pca;

		private GameObject onahoObject;

		private GameObject analPistonObject;

		private GameObject pussyPistonObject;

		private Animator animator;

		private Animator analAnimator;

		private Animator pussyAnimator;

		private SmoothFloat scaleSmooth;

		private float initLocalScale;

		private static readonly int Mode = Animator.StringToHash("Mode");

		private bool isRandomMode;

		private float changeRemainTime;

		private static readonly int IsEcstasy = Animator.StringToHash("IsEcstasy");

		private bool oldActiveFuta;

		private bool oldActiveAnal;

		private bool oldActivePussy;

		public int CurrentSpeedType { get; private set; }

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x409700
			this.pca = pca;
			onahoObject = pca.PlayerAvatarObjectReferencer.Accessorys[8];
			analPistonObject = pca.PlayerAvatarObjectReferencer.Accessorys[9];
			pussyPistonObject = pca.PlayerAvatarObjectReferencer.Accessorys[10];
			animator = onahoObject.GetComponent<Animator>();
			analAnimator = analPistonObject.GetComponent<Animator>();
			pussyAnimator = pussyPistonObject.GetComponent<Animator>();
			initLocalScale = onahoObject.transform.localScale.x;
			Apply();
		}

		public void OnUpdate()
		{
		}

		public void OnChangeSwitch()
		{
			// Recovered from GameAssembly.dll RVA 0x40A600
			var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			state.PistonMachineModeType = unchecked(state.PistonMachineModeType + 1) % 5;
			int mode = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PistonMachineModeType;
			string key = mode switch
			{
			    0 => "piston_off",
			    1 => "piston_low",
			    2 => "piston_middle",
			    3 => "piston_high",
			    4 => "piston_random",
			    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(mode)
			};
			global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.AddMessage(global::ExposureUnnoticed2.Scripts.Base.Localizer.Trans(key), (global::ExposureUnnoticed2.ObjectUI.InGame.MessagePanel.MessagePanelView.MessageType)1);
			Apply();
		}

		private void Apply()
		{
			// Recovered from GameAssembly.dll RVA 0x40A780
			var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			if (state.PistonMachineModeType == 4)
			{
			    isRandomMode = true;
			    changeRemainTime = 0f;
			    return;
			}
			CurrentSpeedType = state.PistonMachineModeType;
			isRandomMode = false;
			if (onahoObject.activeInHierarchy) animator.SetInteger(Mode, global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PistonMachineModeType);
			if (analPistonObject.activeInHierarchy) analAnimator.SetInteger(Mode, global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PistonMachineModeType);
			if (pussyPistonObject.activeInHierarchy) pussyAnimator.SetInteger(Mode, global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PistonMachineModeType);
		}
	// Recovered original member RVA 0x40AA30: .ctor
		public PistonMachineController()
        {
            scaleSmooth = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 1f, 0f, false, false, -1f);
        }
	// Recovered original member RVA 0x40AB00: .cctor
		// Recovered original .cctor RVA 0x40AB00: ordered field initializers preserve BeforeFieldInit.
	}
}
