using ExposureUnnoticed2.Master.Costume;

namespace ExposureUnnoticed2.Object3D.Player.Scripts.Other
{
	public interface IShoesRecord
	{
		float HeelSize { get; }

		CostumeEffectType EffectType { get; }

		bool IsNotHeelToe { get; }

		float HeelToeShapeScale { get; }
	}
}
