namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionExposeRiseStrangeness : MissionBase
	{
		public override bool IsInCondition()
		{
// Complete original method: retained read order, independent player graphs and original gates.
if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer is null) throw new System.NullReferenceException();
if (global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsActiveInvisible) return false;
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
var exposed = facade.GetExposedBodyPartsByClothesAndCostume();
var required = RMission.OriginalRecord.ExposedBodyPartsParam;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
if ((required & exposed) == 0) return false;
return global::ExposureUnnoticed2.Object3D.NPC.Script.NpcManager.Instance.IsRiseStrange;
}
	}
}
