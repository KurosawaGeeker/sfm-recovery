namespace ExposureUnnoticed2.Scripts.InGame
{
	public class StatisticsManager
	{
		private float tmpStatisticsTotalPlayTime;

		private float tmpStatisticsTotalExposeTime;

		public void OnUpdate()
		{
			// Recovered from GameAssembly.dll RVA 0x4A7F10
			tmpStatisticsTotalPlayTime += UnityEngine.Time.deltaTime;
			if (tmpStatisticsTotalPlayTime >= 1f)
			{
			    unchecked { ++GameState.GameStateData.StatistisTotalPlayTime; }
			    tmpStatisticsTotalPlayTime -= 1f;
			}
			if (((int)global::ExposureUnnoticed2.Object3D.Player.Scripts.PlayerFacade.Instance.GetExposedBodyPartsByClothesAndCostume() & 1) == 0)
			{
			    tmpStatisticsTotalExposeTime += UnityEngine.Time.deltaTime;
			    if (tmpStatisticsTotalExposeTime >= 1f)
			    {
			        unchecked { ++GameState.GameStateData.StatistisTotalExposeTime; }
			        tmpStatisticsTotalExposeTime -= 1f;
			    }
			}
		}
	}
}
