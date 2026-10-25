using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Resources.Master.Sound
{
	public class SoundGroupController
	{
		public new enum GetType
		{
			Order = 0,
			Random = 1
		}

		private AudioClip[] clips;

		private GetType type;

		private Dictionary<GameObject, int> orderIndexDict;

		private Dictionary<GameObject, int> randomLastIndexDict;

		public SoundGroupController(AudioClip[] clips, GetType type)
		{
			// Recovered from GameAssembly.dll RVA 0x5E9C10
			orderIndexDict = new Dictionary<GameObject, int>();
			randomLastIndexDict = new Dictionary<GameObject, int>();
			if (clips.Length == 0) throw new global::System.Exception("SoundGroupControllerに要素0のClipsが渡された");
			this.clips = clips;
			this.type = type;
		}

		public AudioClip Get(GameObject target)
		{
			// Recovered from GameAssembly.dll RVA 0x5E9E20
			if (type == GetType.Order) return GetSoundAndUpdate(target);
			if (type == GetType.Random) return GetRandom(target);
			return null;
		}

		private AudioClip GetSoundAndUpdate(GameObject target)
		{
			// Recovered from GameAssembly.dll RVA 0x5E9FC0
			if (!orderIndexDict.ContainsKey(target)) orderIndexDict.Add(target, 0);
			int index = orderIndexDict[target];
			orderIndexDict[target] = unchecked(index + 1) % clips.Length;
			return clips[index];
		}

		private AudioClip GetRandom(GameObject target)
		{
			// Recovered from GameAssembly.dll RVA 0x5EA130
			if (!randomLastIndexDict.ContainsKey(target)) randomLastIndexDict.Add(target, -1);
			int index;
			do
			{
			    float candidate = clips.Length * UnityEngine.Random.value;
			    if (candidate < 0f) candidate = 0f;
			    else if (clips.Length - 1 < candidate) candidate = clips.Length - 1;
			    index = (int)candidate;
			}
			while (index == randomLastIndexDict[target]);
			randomLastIndexDict[target] = index;
			return clips[index];
		}

		public void Reset()
		{
			// Recovered from GameAssembly.dll RVA 0x5EA330
			orderIndexDict.Clear();
			randomLastIndexDict.Clear();
		}
	}
}
