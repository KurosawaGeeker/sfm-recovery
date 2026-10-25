namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionExposeInSight : MissionBase
	{
		public override bool IsInCondition()
		{
// Complete original method: retained read order, independent player graphs and original gates.
if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer is null) throw new System.NullReferenceException();
if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible) return false;
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
var exposed = facade.GetExposedBodyPartsByClothesAndCostume();
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
if ((int)exposed == 1) return false;
return global::ExposureUnnoticed2.Object3D.NPC.Script.NpcManager.Instance.IsSomeOnePlayerFound;
}
	}
}
