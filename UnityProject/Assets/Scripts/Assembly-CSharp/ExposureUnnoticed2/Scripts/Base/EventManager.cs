using System;
using System.Collections.Generic;

namespace ExposureUnnoticed2.Scripts.Base
{
	public static class EventManager
	{
		private static readonly Dictionary<Type, Action<GameEvent>> s_Events = new Dictionary<Type, Action<GameEvent>>();

		private static readonly Dictionary<Delegate, Action<GameEvent>> s_EventLookups = new Dictionary<Delegate, Action<GameEvent>>();

		public static void AddListener<T>(Action<T> evt, bool isHead = false) where T : GameEvent
		{
			// Recovered from GameAssembly.dll RVA 0x993250
			if (s_EventLookups.ContainsKey(evt)) return;
			Action<GameEvent> callback = e => evt((T)e);
			s_EventLookups[evt] = callback;
			if (s_Events.TryGetValue(typeof(T), out Action<GameEvent> existing))
			    s_Events[typeof(T)] = isHead ? callback + existing : existing + callback;
			else
			    s_Events[typeof(T)] = callback;
		}

		public static void RemoveListener<T>(Action<T> evt) where T : GameEvent
		{
			// Recovered from GameAssembly.dll RVA 0x993960
			if (!s_EventLookups.TryGetValue(evt, out Action<GameEvent> callback)) return;
			if (s_Events.TryGetValue(typeof(T), out Action<GameEvent> existing))
			{
			    existing -= callback;
			    if (existing == null) s_Events.Remove(typeof(T));
			    else s_Events[typeof(T)] = existing;
			}
			s_EventLookups.Remove(evt);
		}

		public static void Broadcast(GameEvent evt)
		{
			// Recovered from GameAssembly.dll RVA 0x4B1F70
			if (s_Events.TryGetValue(evt.GetType(), out Action<GameEvent> callback)) callback(evt);
		}

		public static void Clear()
		{
			// Recovered from GameAssembly.dll RVA 0x4B2040
			s_Events.Clear();
			s_EventLookups.Clear();
		}
	// Recovered original member RVA 0x4B20F0: .cctor
		// Recovered original .cctor RVA 0x4B20F0: ordered field initializers preserve BeforeFieldInit.
	}
}
