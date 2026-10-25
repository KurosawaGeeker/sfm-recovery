namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerShiofukiController
	{
		private PlayerClassAccessor pca;

		private PlayerAvatarObjectReferencer referencer;

		private float syaseiSoundCancelRemainTime;

		public void Initialize(PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x65CD60
			this.pca = pca;
			referencer = pca.PlayerAvatarObjectReferencer;
			referencer.SpermParticle.Stop(true, UnityEngine.ParticleSystemStopBehavior.StopEmitting);
		}

		public void OnUpdate()
		{
			// SOURCE DRAFT: original65CEA0, capture private timer before real Time read.
			float oldTimer = syaseiSoundCancelRemainTime;
			float delta = UnityEngine.Time.deltaTime;
			syaseiSoundCancelRemainTime = oldTimer - delta;
		}

		public void EmitShiofuki()
		{
            // Complete original RVA 0x65CF10..0x65D1A8; preserve each state/option re-read.
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveFutanari)
            {
                if (syaseiSoundCancelRemainTime < 0f)
                {
                    PlayerClassAccessor capturedPca = pca;
                    syaseiSoundCancelRemainTime = 4f;
                    capturedPca.FutanariController.FireSperm();
                    pca.PlayerSoundManager.PlaySe((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)93);
                }
                uint exposed = (uint)PlayerFacade.Instance.GetExposedBodyPartsByClothesAndCostume();
                System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
                if ((exposed & 0x2Au) != 0u && !global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.EkitaiDisable)
                {
                    referencer.SpermParticle.Play();
                    global::ExposureUnnoticed2.Scripts.Base.Scheduler.DelayScaledTime(() =>
                    {
                        // Original <EmitShiofuki>b__5_0, RVA 0x65D1B0: re-read at callback time.
                        referencer.SpermParticle.Stop(true, UnityEngine.ParticleSystemStopBehavior.StopEmitting);
                    }, 0.3f);
                }
            }
            if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveFutanari
                && global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.FutanariFemaleClimaxDisable)
                return;
            pca.PlayerSoundManager.PlaySe((global::ExposureUnnoticed2.Scripts.Sound.InGameSoundManager.SeType)8);
            if (!global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.EkitaiDisable)
                referencer.ShiofukiParticle.Play(true);
        }
	}
}
