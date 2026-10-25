namespace ExposureUnnoticed2.Object3D.NPC.Script
{
	public class NpcAvatarSelector
	{
		private NpcComponentAccessor nca;

		public void CreateAvatar(NpcComponentAccessor nca)
		{
			// Original complete RVA0x4227A0. Preserve independent random calls and component/state re-reads.
			this.nca = nca;
			float femaleRandom = UnityEngine.Random.value;
			bool isFemale = femaleRandom >= ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.MaleRate100 * 0.01f;
			float oldRandom = UnityEngine.Random.value;
			bool isOld = ExposureUnnoticed2.Scripts.InGame.GameState.OptionData.OldPeopleRate100 * 0.01f >= oldRandom;
			NpcStateModel.FixedType fixedType = nca.StateModel.CurrentFixedType;
			isOld = fixedType != (NpcStateModel.FixedType)10 && isOld;
			bool onlyYoungWomen = ExposureUnnoticed2.Scripts.InGame.DebugController.IsOnlyYoungWomen;
			isFemale = onlyYoungWomen || isFemale;
			nca.StateModel.isFemale = isFemale;
			if (!onlyYoungWomen && isOld)
			{
			    UnityEngine.GameObject selected;
			    if (isFemale)
			        selected = ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandom(nca.NpcComponent.PrefabObas);
			    else
			        selected = ExposureUnnoticed2.Scripts.Base.ExternalMethodManager.GetRandom(nca.NpcComponent.PrefabOjis);
			    UnityEngine.Transform parent = nca.NpcComponent.AvatarParent;
			    UnityEngine.GameObject clone = UnityEngine.Object.Instantiate(selected, parent);
			    nca.NpcComponent.AvaterObject = clone;
			    nca.StateModel.AvatarType = isFemale ? NpcAvatarComponent.AvatarType.NewOba : NpcAvatarComponent.AvatarType.NewOji;
			    return;
			}
			if (!isFemale)
			{
			    float previousRandom = UnityEngine.Random.value;
			    bool previous = nca.StateModel.CurrentFixedType != (NpcStateModel.FixedType)10 && previousRandom >= 0.5f;
			    NpcComponent component = nca.NpcComponent;
			    UnityEngine.GameObject prefab = previous ? component.PrefabPreviousMale : component.PrefabMale;
			    UnityEngine.Transform parent = component.AvatarParent;
			    UnityEngine.GameObject clone = UnityEngine.Object.Instantiate(prefab, parent);
			    nca.NpcComponent.AvaterObject = clone;
			    nca.StateModel.AvatarType = previous ? NpcAvatarComponent.AvatarType.PreviousMale : NpcAvatarComponent.AvatarType.NewMale;
			    return;
			}
			float femalePreviousRandom = UnityEngine.Random.value;
			bool previousFemale = nca.StateModel.CurrentFixedType != (NpcStateModel.FixedType)10 && femalePreviousRandom >= 0.6f;
			if (ExposureUnnoticed2.Scripts.InGame.DebugController.IsOnlyPreviousWomen)
			    previousFemale = true;
			NpcComponent femaleComponent = nca.NpcComponent;
			UnityEngine.GameObject femalePrefab = previousFemale ? femaleComponent.PrefabPreviousFemale : femaleComponent.PrefabFemale;
			UnityEngine.Transform femaleParent = femaleComponent.AvatarParent;
			UnityEngine.GameObject femaleClone = UnityEngine.Object.Instantiate(femalePrefab, femaleParent);
			nca.NpcComponent.AvaterObject = femaleClone;
			nca.StateModel.AvatarType = previousFemale ? NpcAvatarComponent.AvatarType.PreviousFemale : NpcAvatarComponent.AvatarType.NewFemale;
		}
	}
}
