using UnityEngine.Scripting;

namespace ES3Types
{
	[ES3Properties(new string[]
	{
		"velocity", "angularVelocity", "drag", "angularDrag", "mass", "useGravity", "maxDepenetrationVelocity", "isKinematic", "freezeRotation", "constraints",
		"collisionDetectionMode", "centerOfMass", "inertiaTensorRotation", "inertiaTensor", "detectCollisions", "position", "rotation", "interpolation", "solverIterations", "sleepThreshold",
		"maxAngularVelocity", "solverVelocityIterations"
	})]
	[Preserve]
	public class ES3Type_Rigidbody : ES3ComponentType
	{
		public static ES3Type Instance;

		// Recovered original constructor RVA 0x561180.
		public ES3Type_Rigidbody()
			: base(typeof(UnityEngine.Rigidbody))
		{
			isValueType = false;
			isES3TypeUnityObject = true;
			priority = 1;
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
