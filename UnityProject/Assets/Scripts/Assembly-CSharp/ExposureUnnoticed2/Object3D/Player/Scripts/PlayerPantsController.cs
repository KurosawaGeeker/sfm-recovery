using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerPantsController
	{
		private PlayerClassAccessor pca;

		private PantsType currentPants;

		private SmoothFloat slideMankoBlend;

		private SmoothFloat slideAnalBlend;

		public float CurrentPantsSlideMankoBlend => 0f;

		public float CurrentPantsSlideAnalBlend => 0f;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x3CAB80
			this.pca = pca;
		}

		public void OnUpdate()
		{
			// SOURCE DRAFT: original656D70. Capture all required receivers before the first SmoothFloat call.
			var accessor = pca;
			var action = accessor.PlayerState.CurrentAction;
			if (action is null) throw new System.NullReferenceException();
			var adult = accessor.PlayerAdultGoodsController;
			if (adult is null) throw new System.NullReferenceException();
			var costume = accessor.PlayerCostumeController;
			if (costume is null) throw new System.NullReferenceException();
			var cosplay = accessor.PlayerCosplayManager;
			if (cosplay is null) throw new System.NullReferenceException();
			bool slideFront = (action.IsSlidePants | adult.IsSlideManko) | costume.IsSlideManko | cosplay.IsSlideManko;
			slideMankoBlend.UpdateTo(slideFront ? 100f : 0f);
			adult = pca.PlayerAdultGoodsController;
			if (adult is null) throw new System.NullReferenceException();
			accessor = pca;
			costume = accessor.PlayerCostumeController;
			if (costume is null) throw new System.NullReferenceException();
			cosplay = accessor.PlayerCosplayManager;
			if (cosplay is null) throw new System.NullReferenceException();
			bool slideBack = (adult.IsSlideAnal | costume.IsSlideAnal) | cosplay.IsSlideAnal;
			slideAnalBlend.UpdateTo(slideBack ? 100f : 0f);
		}
	// Recovered original member RVA 0x656E80: .ctor
		public PlayerPantsController()
        {
            slideMankoBlend = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 100f, 0f, false, false, -1f);
            slideAnalBlend = new global::ExposureUnnoticed2.Scripts.Base.SmoothFloat(0.2f, 100f, 0f, false, false, -1f);
        }
	}
}
