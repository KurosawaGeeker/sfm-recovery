using ExposureUnnoticed2.Master.Sex;
using ExposureUnnoticed2.Object3D.NPC.Script;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame
{
	public class SexManager
	{
		public bool IsJustEcstasy;

		private Quaternion startPlayerRotation;

		private Vector3 startPlayerPosition;

		public Vector3 Offset;

		public static SexManager Instance { get; private set; }

		public bool IsSexMode { get; private set; }

		public NpcController ActiveTraceSexPlayerNpc { get; private set; }

		public bool IsPlaySomeSexMotion { get; private set; }

		public void Initialize()
		{
			// Recovered from GameAssembly.dll RVA 0x4A08C0
			Instance = this;
		}

		public void Dispose()
		{
			// Recovered from GameAssembly.dll RVA 0x4A08D0
			Instance = null;
		}

		public bool IsActiveSex()
		{
			return false;
		}

		public void UnregisterTrace(NpcController npc)
		{
		}

		public bool TryRegisterTrace(NpcController npc)
		{
			if (!GameState.OptionData.SexDisable && GameStateFacade.IsActivePassiveSkill(ExposureUnnoticed2.Master.Skill.SkillType.Sex) && ActiveTraceSexPlayerNpc is null)
			{
			    ActiveTraceSexPlayerNpc = npc;
			    return true;
			}
			return false;
		}

		public void SetNpcMotionSpeed()
		{
		}

		public void TransSexMotion(RSex rSex)
		{
		}

		public void ActiveSex()
		{
		}

		public void DeActiveSex()
		{
		}

		public void SetMaleOnlySao(bool isOnlySao)
		{
		}

		public void SetMaleHantoumei(bool isHantoumei)
		{
		}

		public void UpdateRotate(float newRotate)
		{
		}

		public void UpdateOffsetX(float offsetX)
		{
		}

		public void UpdateOffsetY(float offsetY)
		{
		}

		public void UpdateOffsetZ(float offsetZ)
		{
		}

		public void JustEcstasy()
		{
		}

		public void OnEndEcstasy()
		{
		}
	}
}
