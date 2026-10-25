using ExposureUnnoticed2.Master.Stage;

namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcSpawnInterval
	{
		private MStageRecord rCurrentStage;

		private float nextSpawnRemainTime;

		private bool isOldMaxNpc;

		private bool isJustNpcReduceFromMax;

		private bool firstSpawn;

		public void OnLoadStage(MStageRecord rCurrentStage)
		{
			this.rCurrentStage = rCurrentStage;
			firstSpawn = !rCurrentStage.NpcNumRandomInitExist;
		}

		public bool IsSpawn(int currentNpcNum)
		{
			float remaining = nextSpawnRemainTime;
			float delta = UnityEngine.Time.deltaTime;
			isJustNpcReduceFromMax = false;
			remaining -= delta;
			nextSpawnRemainTime = remaining;
			float limit = ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime
			    ? rCurrentStage.NpcNum : rCurrentStage.NpcNumNight;
			if ((int)ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.DifficultType != 2)
			    limit *= 0.7f;
			if ((int)limit <= currentNpcNum)
			{
			    isOldMaxNpc = true;
			    return false;
			}
			if (isOldMaxNpc)
			{
			    isOldMaxNpc = false;
			    isJustNpcReduceFromMax = true;
			}
			int stage = (int)rCurrentStage.StageType;
			if (stage == 6)
			{
			    if (firstSpawn)
			    {
			        float random = UnityEngine.Random.value;
			        firstSpawn = false;
			        nextSpawnRemainTime = random * 40f * (ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime ? 0.7f : 1f) + 0f;
			        return false;
			    }
			    if (isJustNpcReduceFromMax)
			    {
			        float random = UnityEngine.Random.value;
			        nextSpawnRemainTime = (random * 70f + 20f) * (ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime ? 0.7f : 1f);
			        return false;
			    }
			    if (!(remaining < 0f)) return false;
			    float value = UnityEngine.Random.value;
			    nextSpawnRemainTime = (value * 70f + 20f) * (ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime ? 0.7f : 1f);
			    return true;
			}
			if (stage == 7)
			{
			    if (isJustNpcReduceFromMax)
			    {
			        float random = UnityEngine.Random.value;
			        nextSpawnRemainTime = (random * 30f + 5f) * (ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime ? 0.5f : 1f);
			        return false;
			    }
			    if (remaining < 0f)
			    {
			        float random = UnityEngine.Random.value;
			        nextSpawnRemainTime = (random * 30f + 5f) * (ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime ? 0.5f : 1f);
			        return true;
			    }
			    return false;
			}
			if (stage == 14)
			{
			    if (isJustNpcReduceFromMax)
			    {
			        float random = UnityEngine.Random.value;
			        nextSpawnRemainTime = (random * 30f + 5f) * (ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime ? 0.7f : 1f);
			        return false;
			    }
			    if (!(remaining < 0f)) return false;
			    float value = UnityEngine.Random.value;
			    nextSpawnRemainTime = (value * 30f + 5f) * (ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime ? 0.7f : 1f);
			    return true;
			}
			if (stage == 15)
			{
			    if (firstSpawn)
			    {
			        float random = UnityEngine.Random.value;
			        firstSpawn = false;
			        nextSpawnRemainTime = random * 20f * (ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime ? 0.1f : 1f) + 0f;
			        return false;
			    }
			    if (isJustNpcReduceFromMax)
			    {
			        float random = UnityEngine.Random.value;
			        nextSpawnRemainTime = (random * 100f + 10f) * (ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime ? 0.3f : 1f);
			        return false;
			    }
			    if (!(remaining < 0f)) return false;
			    float value = UnityEngine.Random.value;
			    nextSpawnRemainTime = (value * 100f + 10f) * (ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.IsDaytime ? 0.3f : 1f);
			    return true;
			}
			return false;
		}
	}
}
