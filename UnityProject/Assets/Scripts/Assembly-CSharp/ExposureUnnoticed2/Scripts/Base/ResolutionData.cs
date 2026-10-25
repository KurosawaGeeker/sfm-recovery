namespace ExposureUnnoticed2.Scripts.Base
{
	public class ResolutionData
	{
		public int X;

		public int Y;

		public string Name;

		public ResolutionData(int x, int y)
		{
			// Recovered from GameAssembly.dll RVA 0x4BC930
			X = x;
			Y = y;
			Name = X.ToString() + "x" + Y.ToString();
		}
	}
}
