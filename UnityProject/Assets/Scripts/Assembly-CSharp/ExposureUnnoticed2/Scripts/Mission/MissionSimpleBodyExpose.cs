namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionSimpleBodyExpose : MissionBase
	{
		public override bool IsInCondition()
		{
if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer is null) throw new System.NullReferenceException();
var facade = global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance;
if (facade is null) throw new System.NullReferenceException();
var exposed = facade.GetExposedBodyPartsByClothesAndCostume();
var required = RMission.OriginalRecord.ExposedBodyPartsParam;
System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::ExposureUnnoticed2.Scripts.Base.ExternalMethodManager).TypeHandle);
if ((required & exposed) != required) return false;
var area = global::ExposureUnnoticed2.Scripts.InGame.MissionArea.NpcRouteAreaManager.Instance;
if (area != null)
{
    area = global::ExposureUnnoticed2.Scripts.InGame.MissionArea.NpcRouteAreaManager.Instance;
    if (area is null) throw new System.NullReferenceException();
    return area.IsInArea;
}
return true;
}
	}
}
