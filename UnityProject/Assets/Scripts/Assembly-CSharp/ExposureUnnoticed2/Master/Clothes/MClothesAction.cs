using System;
using ExposureUnnoticed2.Object3D.Player.Scripts;

namespace ExposureUnnoticed2.Master.Clothes
{
	[Serializable]
	public class MClothesAction
	{
		public string NameKey;

		public double Rp;

		public PlayerStateModel.ClothesState State;

		public double FrontStrangeness;

		public double SideStrangeness;

		public double BackStrangeness;

		public bool IsArmActionFromPrev;

		public bool IsArmActionFromNext;
	}
}
