namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionBukkake : MissionBase
	{
		public override bool IsInCondition()
		{
// Complete original method: retained read order, independent player graphs and original gates.
if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer is null) throw new System.NullReferenceException();
if (!global::ExposureUnnoticed2.Object3D.SpermBullet.SpermDecalManager.Instance.IsBukkakeNpcMission) return false;
return !global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.FutanariDisable;
}
	}
}
