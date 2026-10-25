namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionExposeIntercom : MissionBase
	{
		public override bool IsInCondition()
		{
// Complete original method; original state latch, sampler order and independent player graphs.
if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer is null) throw new System.NullReferenceException();
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
var exposed = facade.GetExposedBodyPartsByClothesAndCostume();
var required = RMission.OriginalRecord.ExposedBodyPartsParam;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
if ((required & exposed) == 0) return false;
var manager = global::ExposureUnnoticed2.Object3D.NPC.Script.PinponNpcManager.Instance;
if (manager != null)
{
    manager = global::ExposureUnnoticed2.Object3D.NPC.Script.PinponNpcManager.Instance;
    if (manager is null) throw new System.NullReferenceException();
    return manager.IsInActiveArea;
}
return false;
}
	}
}
