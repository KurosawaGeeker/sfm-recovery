using ExposureUnnoticed2.Scripts.Base;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class VisibilityFogController
	{
		private SmoothFloat visibilityLevel;

		private bool oldIsDaytime;

		private bool isForceUpdate;

		public static VisibilityFogController Instance { get; private set; }

		public bool IsBadVisibility => visibilityLevel.Target >= 1f;

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x4A9460
			Instance = this;
		}

		public void Dispose()
		{
            // Original RVA 0x4A9470 clears this type's static Instance backing field.
            Instance = null;
		}

		public void ForceUpdateOnce()
		{
			// Recovered from GameAssembly.dll RVA 0x4A9480
			// Original existing VisibilityFogController method4A9480; no added method or field.
			isForceUpdate = true;
		}

		public void OnUpdate()
		{
		}

		private float GetEyeMaskFogNear()
		{
			return 0f;
		}

		private float GetEyeMaskFogFar()
		{
			return 0f;
		}

		private float GetEyeMaskFogDensity()
		{
			return 0f;
		}

		public void UpdateEyeMask()
		{
			// Recovered from GameAssembly.dll RVA 0x4AA140
			var player = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca;
			bool masked = false;
			if (!global::ExposureUnnoticed2.Scripts.InGame.GameStateFacade.IsActivePassiveSkill((global::ExposureUnnoticed2.Master.Skill.SkillType)15))
			{
			    var adult = player.PlayerAdultGoodsController;
			    if (adult is null) throw new System.NullReferenceException();
			    var costume = player.PlayerCosplayManager;
			    if (costume is null) throw new System.NullReferenceException();
			    masked = costume.IsActiveEyeMask || adult.IsActiveEyeMask;
			}
			visibilityLevel.Target = masked ? 1f : 0f;
		}
	// Recovered original member RVA 0x4AA1F0: .ctor
		public VisibilityFogController()
        {
            visibilityLevel = new SmoothFloat(0.5f, 1f, 0f, false, false, -1f);
        }
	}
}
