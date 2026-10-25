using UnityEngine;

namespace ExposureUnnoticed2.Scripts.InGame.Util
{
	public interface IColliderTriggerController
	{
		bool IsInArea { get; }

		float PenetrateDist { get; }

		GameObject CenterObject { get; }

		GameObject GameObject { get; }
	}
}
