using UnityEngine.Scripting;

namespace ES3Types
{
	[Preserve]
	public class ES3Type_EventSystem : ES3ComponentType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x55c240.
		public ES3Type_EventSystem()
			: base(typeof(UnityEngine.EventSystems.EventSystem))
		{
			isValueType = false;
			isES3TypeUnityObject = true;
			Instance = this;
		}

		protected override void WriteComponent(object obj, ES3Writer writer)
		{
		}

		protected override void ReadComponent<T>(ES3Reader reader, object obj)
		{
		}
	}
}
