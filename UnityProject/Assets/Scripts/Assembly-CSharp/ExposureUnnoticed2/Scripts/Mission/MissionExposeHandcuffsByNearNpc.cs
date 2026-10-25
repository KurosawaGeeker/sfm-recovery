namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionExposeHandcuffsByNearNpc : MissionBase
	{
		public override bool IsInCondition()
		{
var player = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player;
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
var exposed = facade.GetExposedBodyPartsByClothesAndCostume();
var required = RMission.OriginalRecord.ExposedBodyPartsParam;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
if ((required & exposed) == 0) return false;
var handcuffs = player.Pca.PlayerHandcuffsController;
if (handcuffs is null) throw new System.NullReferenceException();
if (!handcuffs.IsConditionMission()) return false;
return player.Pca.PlayerState.IsNearNpcForMission;
}

		protected override float GetAddSatisfy()
		{
float unlockScale = 1f - global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerHandcuffsController.UnlockProgress2 * 0.5f;
var player = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player;
if ((int)player.Pca.PlayerState.CurrentAction.Type == 10022 || (int)player.Pca.PlayerState.CurrentAction.Type == 10019) unlockScale = 0f;
float scale = RMission.AddSatisfyScale;
float rate = scale > 1f ? 0.05f : global::ExposureUnnoticed2.Master.Common.MCommon.Instance.AddMissionSatisfyPerSecond;
float prefactor = rate * scale;
float delta = global::UnityEngine.Time.deltaTime;
return ((prefactor * delta) * global::ExposureUnnoticed2.Scripts.InGame.DebugController.MissionSatisfyScale) * unlockScale;
}

		protected override bool IsRpPerSecCondition()
		{
var player = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player;
var state = player.Pca.PlayerState;
if ((int)state.CurrentAction.Type == 10022) return false;
return (int)player.Pca.PlayerState.CurrentAction.Type != 10019;
}

		protected override float IsRpPerSecScale()
		{
return 1f - global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerHandcuffsController.UnlockProgress2 * 0.5f;
}
	}
}
