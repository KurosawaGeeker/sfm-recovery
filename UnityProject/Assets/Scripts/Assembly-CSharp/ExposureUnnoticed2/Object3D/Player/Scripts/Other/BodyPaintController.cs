using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public class BodyPaintController
	{
		private enum State
		{
			NoActiveNoMaterial = 0,
			NoActive = 1,
			Start = 2,
			FirstReduce = 3,
			SecondReduce = 4
		}

		private const float PaintTime = 120f;

		private const float FirstReduceTime = 60f;

		private const float SecondReduceTime = 24f;

		private const float StartAlpha = 0.4f;

		private const float FirstReduceAlpha = 0.25f;

		private const float SecondReduceAlpha = 0f;

		private const float LostAlpha = -1f;

		private SmoothFloat alpha;

		private PlayerClassAccessor pca;

		private State currentState;

		private static readonly int TweakTransparency = UnityEngine.Shader.PropertyToID("_Tweak_transparency");

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3FC940
			this.pca = pca;
			var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			if (state.PlayerRemainBodyPaintTime <= 0f)
			{
			    currentState = State.NoActiveNoMaterial;
			    alpha.Current = -1f;
			}
			else if (state.PlayerRemainBodyPaintTime <= 24f)
			{
			    currentState = State.SecondReduce;
			    alpha.Current = 0f;
			}
			else if (state.PlayerRemainBodyPaintTime <= 60f)
			{
			    currentState = State.FirstReduce;
			    alpha.Current = .25f;
			}
			else
			{
			    currentState = State.Start;
			    alpha.Current = .4f;
			}
		}

		public void OnUpdate()
		{
		}

		public float GetMaxDeltaAdd()
		{
			// Recovered from GameAssembly.dll RVA 0x3FD090
			return currentState switch
			{
			    State.NoActiveNoMaterial => 3f,
			    State.NoActive => 3f,
			    State.Start => .03f,
			    State.FirstReduce => .1f,
			    State.SecondReduce => .3f,
			    _ => throw new System.Runtime.CompilerServices.SwitchExpressionException(currentState),
			};
		}

		public bool IsPainted()
		{
			// Recovered from GameAssembly.dll RVA 0x3FD130
			return currentState != State.NoActive && currentState != State.NoActiveNoMaterial;
		}

		public void OnUseItem()
		{
			// Recovered from GameAssembly.dll RVA 0x3FD150
			var state = global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData;
			float previous = state.PlayerRemainBodyPaintTime;
			state.PlayerRemainBodyPaintTime = 120f;
			if (previous <= 0f) pca.PlayerBodyMaterialController.SetupBodyPaintFromGameState();
			currentState = State.Start;
			alpha.Current = 1f;
			if (pca.PlayerState.LastUseItemType == (global::ExposureUnnoticed2.Master.Item.ItemType)8)
			    global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveBodyPaintReusable = true;
		}

		public void OnUseWashItem()
		{
			// Recovered from GameAssembly.dll RVA 0x3FD230
			currentState = State.SecondReduce;
			global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerRemainBodyPaintTime = 0f;
		}
	// Recovered original member RVA 0x3FD290: .ctor
		public BodyPaintController()
        {
            alpha = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(2f, 0.15f, 0f, false, false, -1f);
        }
	// Recovered original member RVA 0x3FD360: .cctor
		// Recovered original .cctor RVA 0x3FD360: ordered field initializers preserve BeforeFieldInit.
	}
}
