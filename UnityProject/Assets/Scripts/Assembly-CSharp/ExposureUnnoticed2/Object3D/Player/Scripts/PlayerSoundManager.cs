using System.Collections.Generic;
using ExposureUnnoticed2.Scripts.Base;
using ExposureUnnoticed2.Scripts.Sound;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.Player.Scripts
{
	public class PlayerSoundManager
	{
		private AudioSource audioSource;

		private GameObject root;

		private float lastFootSeTime;

		private AudioSource audioDataAudioSource;

		private bool isPlayPickingSe;

		private PlayerClassAccessor pca;

		private Dictionary<GameObject, InGameSoundManager.SeType> FootStepObjectDict;

		private float initVolume;

		public static PlayerSoundManager Instance { get; private set; }

		public void Initialize(AudioSource audioSource, GameObject root, PlayerClassAccessor pca)
		{
			// Recovered from GameAssembly.dll RVA 0x65DDB0
			Instance = this;
			this.pca = pca;
			this.audioSource = audioSource;
			this.root = root;
			initVolume = audioSource.volume;
			audioDataAudioSource = pca.PlayerAvatarObjectReferencer.PickingAudioSource;
			EventManager.AddListener<StartTransitionStageEvent>(OnTransitionStageEvent, false);
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x65E050
			Instance = null;
			EventManager.RemoveListener<StartTransitionStageEvent>(OnTransitionStageEvent);
		}

		private void OnTransitionStageEvent(StartTransitionStageEvent startTransitionStageEvent)
		{
			// Recovered from GameAssembly.dll RVA 0x65E110
			FootStepObjectDict.Clear();
		}

		public void PlaySe(InGameSoundManager.SeType type)
		{
		    // Original RVA 0x65E1A0. Footstep path owns its event and may suppress duplicates.
		    if (PlayFootSeIfInCondition(type)) return;
		    Events.PlayerSoundEvent.SeType = type;
		    EventManager.Broadcast(Events.PlayerSoundEvent);
		    if (InGameSoundManager.Instance.IsGroupSe(type))
		    {
		        audioSource.volume = initVolume;
		        var capturedSource = audioSource;
		        var clip = InGameSoundManager.Instance.GetSound(type, root);
		        if (capturedSource is null) throw new global::System.NullReferenceException();
		        capturedSource.PlayOneShot(clip, 1f);
		    }
		    else
		    {
		        var clip = InGameSoundManager.Instance.GetSound(type);
		        if (clip)
		        {
		            audioSource.volume = initVolume;
		            audioSource.PlayOneShot(clip, 1f);
		        }
		        else
		            InGameSoundManager.Instance.PlayOneShotAudioData(audioDataAudioSource, type, -1f);
		    }
		}

		private bool PlayFootSeIfInCondition(InGameSoundManager.SeType type)
		{
		    // Original RVA 0x65E460. Repeated footsteps return handled without event/audio.
		    if (!InGameSoundManager.Instance.IsFootSe(type)) return false;
		    if (Time.time - lastFootSeTime < 0.2f) return true;
		    lastFootSeTime = Time.time;
		    var adjusted = TransTerrainSeIfNeed(type);
		    Events.PlayerSoundEvent.SeType = adjusted;
		    EventManager.Broadcast(Events.PlayerSoundEvent);
		    float volume = initVolume;
		    if (adjusted == InGameSoundManager.SeType.FootStepGrass) volume = 0.3f;
		    else if (adjusted == InGameSoundManager.SeType.FootStepGravel) volume = 0.5f;
		    else if (adjusted == InGameSoundManager.SeType.FootStepSneaker || adjusted == InGameSoundManager.SeType.FootStepSneakerJog) volume = 0.6f;
		    audioSource.volume = volume;
		    var capturedSource = audioSource;
		    var clip = InGameSoundManager.Instance.GetSound(adjusted, root);
		    if (capturedSource is null) throw new global::System.NullReferenceException();
		    capturedSource.PlayOneShot(clip, 1f);
		    return true;
		}

		private InGameSoundManager.SeType TransTerrainSeIfNeed(InGameSoundManager.SeType type)
		{
		    // Original RVA 0x65E6B0. The first registered collision surface wins.
		    foreach (var go in pca.PlayerObjectReferencer.TerrainCheckCollider.CollisionList)
		    {
		        var surfaceType = GetFootStepType(go);
		        if (surfaceType != InGameSoundManager.SeType.NoType) return surfaceType;
		    }
		    Terrain terrain = null;
		    if (global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.CurrentStageController.StageType == global::ExposureUnnoticed2.Master.Stage.StageType.Park)
		        terrain = global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.CurrentStageController.Terrain;
		    if (!terrain) return type;
		    if (!pca.PlayerObjectReferencer.TerrainCheckCollider.CollisionList.Contains(terrain.gameObject)) return type;
		    if (pca.PlayerObjectReferencer.TerrainCheckCollider.CollisionList.Count > 1) return type;
		    var data = terrain.terrainData;
		    var localPosition = pca.Transform.position - terrain.transform.position;
		    if (data is null) throw new global::System.NullReferenceException();
		    int x = (int)(data.alphamapResolution * localPosition.x / data.size.x);
		    int z = (int)(data.alphamapResolution * localPosition.z / data.size.z);
		    // No added clamp: the original GetAlphamaps contract handles invalid coordinates.
		    var alpha = data.GetAlphamaps(x, z, 1, 1);
		    if (alpha[0, 0, 0] >= 0.3f) return InGameSoundManager.SeType.FootStepGrass;
		    if (alpha[0, 0, 1] >= 0.3f) return InGameSoundManager.SeType.FootStepGravel;
		    return type;
		}

		public void SetPickingSeSoundIfNeed(bool isPlay)
		{
			// Recovered from GameAssembly.dll RVA 0x65EE80
			if (isPlay)
			{
			    if (isPlayPickingSe) return;
			    isPlayPickingSe = true;
			    InGameSoundManager.Instance.PlayOneShotAudioData(audioDataAudioSource, InGameSoundManager.SeType.Picking, -1f);
			}
			else if (isPlayPickingSe)
			{
			    isPlayPickingSe = false;
			    audioDataAudioSource.Stop();
			}
		}

		public void PlayPickingSe(InGameSoundManager.SeType seType)
		{
			// Recovered from GameAssembly.dll RVA 0x65EF70
			InGameSoundManager.Instance.PlayOneShotAudioData(audioDataAudioSource, seType, -1f);
		}

		public void RegisterFootStepObject(GameObject go, InGameSoundManager.SeType type)
		{
			// Recovered from GameAssembly.dll RVA 0x65EFF0
			FootStepObjectDict.Add(go, type);
		}

		private InGameSoundManager.SeType GetFootStepType(GameObject go)
		{
			// Recovered from GameAssembly.dll RVA 0x65F080
			return FootStepObjectDict.ContainsKey(go) ? FootStepObjectDict[go] : InGameSoundManager.SeType.NoType;
		}
	// Recovered original member RVA 0x65F130: .ctor
		public PlayerSoundManager()
        {
            FootStepObjectDict = new Dictionary<GameObject, InGameSoundManager.SeType>();
        }
	}
}
