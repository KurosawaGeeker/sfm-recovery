using System.Collections.Generic;
using ExposureUnnoticed2.Object3D.NPC.Script;
using ExposureUnnoticed2.Object3D.ScenePlops.Toilet;
using ExposureUnnoticed2.Scripts.Base;
using UnityEngine;

namespace ExposureUnnoticed2.Object3D.ToiletManager
{
	public class ToiletManager : MonoBehaviour
	{
		[SerializeField]
		private Transform[] malePeePoint;

		[SerializeField]
		private Transform[] maleWashPoint;

		[SerializeField]
		private Transform[] femaleWashPoint;

		[SerializeField]
		private ToiletController[] maleToiletController;

		[SerializeField]
		private ToiletController[] femaleToiletController;

		[SerializeField]
		private Transform malePeeWaitPoint;

		[SerializeField]
		private Transform maleKoshitsuWaitPoint;

		[SerializeField]
		private Transform femaleKoshitsuWaitPoint;

		[SerializeField]
		private Transform maleWashWaitPoint;

		[SerializeField]
		private Transform femaleWashWaitPoint;

		[SerializeField]
		private AudioSource[] malePeeAudioSource;

		private Dictionary<int, NpcController> peeTokenDict;

		private Dictionary<int, NpcController> washMaleTokenDict;

		private Dictionary<int, NpcController> washFemaleTokenDict;

		private Dictionary<int, NpcController> koshitsuMaleTokenDict;

		private Dictionary<int, NpcController> koshitsuFemaleTokenDict;

		private Dictionary<int, bool> koshitsuMaleFoundPlayerInsideDict;

		private Dictionary<int, bool> koshitsuFemaleFoundPlayerInsideDict;

		private int[] peeIndexArray;

		private int[] maleKoshitsuIndexArray;

		private int[] femaleKoshitsuIndexArray;

		public static ToiletManager Instance { get; private set; }

		public bool IsPlayerInOpenToilet { get; private set; }

		public Transform GetPeeWaitPoint(bool isFemale)
		{
			return null;
		}

		public Transform GetWashWaitPoint(bool isFemale)
		{
			return null;
		}

		public Transform GetKoshitsuWaitPoint(bool isFemale)
		{
			return null;
		}

		private void Awake()
		{
		}

		public void Initialize()
		{
		}

		public void Dispose()
		{
		}

		private void Reset()
		{
		}

		private void OnChangeTime(ChangeTimeEvent evt)
		{
		}

		public void OnUpdate()
		{
		}

		public Transform TryGetTokenWash(NpcController npc)
		{
			return null;
		}

		public void ReturnTokenWash(NpcController npc)
		{
		}

		public Transform TryGetTokenPee(NpcController npc)
		{
			return null;
		}

		public void ReturnTokenPee(NpcController npc)
		{
		}

		public void ReportToiletInPlayer(ToiletController toiletController)
		{
		}

		public ToiletController TryGetTokenKoshitsu(NpcController npc)
		{
			return null;
		}

		public void ReturnTokenKoshitsu(NpcController npc)
		{
		}
	}
}
