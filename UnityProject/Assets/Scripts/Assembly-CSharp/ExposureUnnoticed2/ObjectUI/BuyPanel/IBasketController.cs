using UnityEngine;

namespace ExposureUnnoticed2.ObjectUI.BuyPanel
{
	public interface IBasketController
	{
		void Add(ScriptableObject master);

		void Remove(ScriptableObject master);

		bool Contains(ScriptableObject master);

		int GetBasketRp();
	}
}
