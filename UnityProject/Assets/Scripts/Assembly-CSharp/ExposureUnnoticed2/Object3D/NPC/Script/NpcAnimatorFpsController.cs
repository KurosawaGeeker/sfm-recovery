using Common.Scripts.Util;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcAnimatorFpsController
	{
		private class CullingData
		{
			public int ActiveDist;

			public int Level1Dist;

			public int Level1Frame;

			public int Level2Dist;

			public int Level2Frame;

			public int Level3Dist;

			public int Level3Frame;

			public int Level4Frame;
		}

		private AnimatorFPSController animatorFPSController;

		private NpcComponentAccessor nca;

		private int flame;

		private float initTime;

		private CullingData cullingData;

		public void Initialize(NpcComponentAccessor nca)
		{
			this.nca = nca;
			animatorFPSController = new AnimatorFPSController();
			animatorFPSController.Initialize(nca.Animator);
			initTime = UnityEngine.Time.time;
			if (ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType == (ExposureUnnoticed2.Master.Stage.StageType)11)
			    cullingData = new CullingData { ActiveDist = 10, Level1Dist = 20, Level1Frame = 30, Level2Dist = 25, Level2Frame = 10, Level3Dist = 35, Level3Frame = 5, Level4Frame = 2 };
			else
			    cullingData = new CullingData { ActiveDist = 20, Level1Dist = 30, Level1Frame = 30, Level2Dist = 40, Level2Frame = 10, Level3Dist = 50, Level3Frame = 5, Level4Frame = 2 };
		}

		public void OnUpdate()
		{
			flame = unchecked(flame + 1);
			if (flame < 3) return;
			float distance = ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.DistSqrt(ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.GameObject, nca.GameObject);
			if (!(distance > cullingData.ActiveDist))
			{
			    animatorFPSController.DeActive();
			    return;
			}
			int fps;
			if (cullingData.Level1Dist > distance) fps = cullingData.Level1Frame;
			else if (cullingData.Level2Dist > distance) fps = cullingData.Level2Frame;
			else if (cullingData.Level3Dist > distance) fps = cullingData.Level3Frame;
			else fps = cullingData.Level4Frame;
			if (!nca.AvatarComponent.MeshParent.activeInHierarchy) fps = 2;
			animatorFPSController.Active(fps);
			animatorFPSController.OnUpdate();
		}
	}
}
