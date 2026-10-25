namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionHandcuffsMapExpose : MissionBase
	{
		public override bool IsInCondition()
		{
// Complete original method: retained read order, independent player graphs and original gates.
var player = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player;
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
var exposed = facade.GetExposedBodyPartsByClothesAndCostume();
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
if ((int)exposed == 1 || !global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.PlayerIsAttachMapHandcuffs) return false;
var handcuffs = player.Pca.PlayerHandcuffsController;
if (handcuffs is null) throw new System.NullReferenceException();
if (!handcuffs.IsConditionMission()) return false;
var area = global::ExposureUnnoticed2.Scripts.InGame.MissionArea.NpcRouteAreaManager.Instance;
if (area != null)
{
    area = global::ExposureUnnoticed2.Scripts.InGame.MissionArea.NpcRouteAreaManager.Instance;
    if (area is null) throw new System.NullReferenceException();
    return area.IsInArea;
}
return true;
}

		protected override float GetAddSatisfy()
		{
// Complete original method: retained read order, independent player graphs and original gates.
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
// Complete original method: retained read order, independent player graphs and original gates.
var player = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer.Player;
var state = player.Pca.PlayerState;
if ((int)state.CurrentAction.Type == 10022) return false;
return (int)player.Pca.PlayerState.CurrentAction.Type != 10019;
}

		protected override float IsRpPerSecScale()
		{
// Complete original method: retained read order, independent player graphs and original gates.
return 1f - global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerController.Instance.Pca.PlayerHandcuffsController.UnlockProgress2 * 0.5f;
}
	}
}
