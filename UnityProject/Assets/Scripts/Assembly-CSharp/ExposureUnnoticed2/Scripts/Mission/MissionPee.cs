namespace ExposureUnnoticed2.Scripts.Mission
{
	public class MissionPee : MissionBase
	{
		public override bool IsInCondition()
		{
var referencer = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.InGameReferencer;
if (referencer is null) throw new System.NullReferenceException();
if (global::ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.PeeDisable) return false;
if (!referencer.Player.Pca.PlayerState.IsPeeing) return false;
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
