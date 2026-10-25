using System;
using System.Collections.Generic;
using ExposureUnnoticed2.Master.Mission;
using ExposureUnnoticed2.Object3D.Player.Scripts;
using UnityEngine;
using UnityEngine.AI;

namespace ExposureUnnoticed2.Scripts.Base
{
	internal static class ExternalMethodManager
	{
		// Recovered original static initializer RVA 0x4B3940; original BeforeFieldInit retained.
		private static NavMeshPath path = new NavMeshPath();

		private static int FrameLogCount;

		public static void NullSafeInvoke(this Action action)
		{
		}

		public static void NullSafeInvoke<T1, T2>(this Action<T1, T2> action, T1 arg1, T2 arg2)
		{
		}

		public static void NullSafeInvoke<T>(this Action<T> action, T arg)
		{
		}

		public static T NullSafeInvoke<T>(this Func<T> action)
		{
			return default;
		}

		public static Vector3 GetSetHeight(this Vector3 v, float a)
		{
			return default;
		}

		public static Vector3 SetWidth(this Vector3 v, float a)
		{
			return default;
		}

		public static Vector3 GetSetZ(this Vector3 v, float a)
		{
			return default;
		}

		public static Vector3 GetSetY(this Vector3 v, float a)
		{
			v.y = a; return v;
		}

		public static Vector3 GetSetX(this Vector3 v, float a)
		{
			return default;
		}

		public static Vector2 GetSetX(this Vector2 v, float a)
		{
			return default;
		}

		public static Vector2 GetSetY(this Vector2 v, float a)
		{
			return default;
		}

		public static float DistSqrt(this GameObject self, GameObject other)
		{
            // Recovered complete original native body.
            Vector3 selfPosition = self.transform.position;
            Vector3 otherPosition = other.transform.position;
            float dx = selfPosition.x - otherPosition.x;
            float dy = selfPosition.y - otherPosition.y;
            float dz = selfPosition.z - otherPosition.z;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(System.Math).TypeHandle);
            float squaredY = dy * dy;
            float squaredX = dx * dx;
            float squaredZ = dz * dz;
            float squared = (squaredY + squaredX) + squaredZ;
            return (float)System.Math.Sqrt((double)squared);
        }

		public static float DistSqr(this GameObject self, GameObject other)
		{
			return 0f;
		}

		public static float DistSqrt(this Transform self, Transform other)
		{
			// Draft from complete original 4B26E0..4B27F4 and outlined magnitude 3D4630.
			Vector3 selfPosition = self.position;
			Vector3 otherPosition = other.position;
			float dx = selfPosition.x - otherPosition.x;
			float dy = selfPosition.y - otherPosition.y;
			float dz = selfPosition.z - otherPosition.z;
			System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(System.Math).TypeHandle);
			float squaredY = dy * dy;
			float squaredX = dx * dx;
			float squaredZ = dz * dz;
			float squared = (squaredY + squaredX) + squaredZ;
			return (float)System.Math.Sqrt((double)squared);
		}

		public static float DistSqr(this Transform self, Transform other)
		{
			return 0f;
		}

		public static float DistSqrt(this GameObject self, Vector3 other)
		{
            Vector3 a = self.transform.position;
            float dx = a.x - other.x, dy = a.y - other.y, dz = a.z - other.z;
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(System.Math).TypeHandle);
            return (float)System.Math.Sqrt((double)((dy * dy + dx * dx) + dz * dz));
        }

		public static Vector3 Direct(this GameObject self, GameObject other)
		{
			Vector3 otherPosition = other.transform.position;
			Vector3 selfPosition = self.transform.position;
			float dx = otherPosition.x - selfPosition.x;
			float dy = otherPosition.y - selfPosition.y;
			float dz = otherPosition.z - selfPosition.z;
			float length = (float)System.Math.Sqrt((double)((dy * dy + dx * dx) + dz * dz));
			if (1e-5f < length) return new Vector3(dx / length, dy / length, dz / length);
			return Vector3.zero;
		}

		public static void DestroyChildren(this Component gameObject)
		{
			// Recovered from GameAssembly.dll RVA 0x4B2C70
			foreach (Transform child in gameObject.transform) UnityEngine.Object.Destroy(child.gameObject);
		}

		public static void GetSetX(this RectTransform rect, float x)
		{
		}

		public static void SetWidth(this RectTransform rect, float x)
		{
			// Recovered from GameAssembly.dll RVA 0x4B3000
			Vector2 oldSize = rect.sizeDelta;
			rect.sizeDelta = new Vector2(x, oldSize.y);
		}

		public static void SetHeight(this RectTransform rect, float y)
		{
			// Recovered from GameAssembly.dll RVA 0x4B30D0
			Vector2 oldSize = rect.sizeDelta;
			rect.sizeDelta = new Vector2(oldSize.x, y);
		}

		public static Vector3 Vector2to3(this Vector2 before)
		{
			return default;
		}

		public static Vector3 Div(this Vector3 a, Vector3 b)
		{
			return default;
		}

		public static List<T> GetComponentsInAllChildren<T>(this GameObject gameObject)
		{
			return null;
		}

		private static void RecursiveSetActive<T>(GameObject a_CheckObject, List<T> list)
		{
		}

		public static void Shuffle<T>(this IList<T> array)
		{
			for (int i = array.Count - 1; i > 0; --i)
			{
			    int selected = UnityEngine.Random.Range(0, i + 1);
			    T selectedValue = array[selected];
			    T currentValue = array[i];
			    array[i] = selectedValue;
			    array[selected] = currentValue;
			}
		}

		public static T GetRandom<T>(this T[] array)
		{
			if (array.Length == 0) return default;
			return array[UnityEngine.Random.Range(0, array.Length)];
		}

		public static int GetRandomIndex<T>(this T[] array, int ignore = int.MaxValue)
		{// Complete original method; original thresholds, array capture, RNG and lifecycle order.
int selected = UnityEngine.Random.Range(0, array.Length - 1);
return selected < ignore ? selected : unchecked(selected + 1);
}

		public static int GetRandomIndex<T>(this List<T> array, int ignore = int.MaxValue)
		{
			return 0;
		}

		public static T GetRandom<T>(this List<T> list)
		{
			if (list.Count == 0) return default;
			return list[UnityEngine.Random.Range(0, list.Count)];
		}

		public static void DeepCopy<T>(this List<T> source, List<T> to)
		{
		}

		public static float Dot(this Vector2 a, Vector2 b)
		{
			return 0f;
		}

		public static float Cross(this Vector2 a, Vector2 b)
		{
			return 0f;
		}

		public static float ReduceUntil0(float target, float sub)
		{
			return 0f;
		}

		public static float ConvertRotateUnder180(float val)
		{
			return 0f;
		}

		public static Color GetSetAlpha(this Color c, float alpha)
		{
			return default;
		}

		public static float CalcPathLength(Vector3 start, Vector3 end)
        {
            int stage = (int)global::ExposureUnnoticed2.Scripts.InGame.GameState.GameStateData.CurrentStageType;
            int mask = stage == 10 || stage == 11 || stage == 12 ? 8 : -1;
            global::UnityEngine.AI.NavMesh.CalculatePath(start, end, mask, path);
            var corners = path.corners;
            float length = 0f;
            for (int i = 1; i < corners.Length; ++i)
            {
                Vector3 a = corners[i];
                Vector3 b = i == 1 ? start : corners[i - 1];
                float dx = a.x - b.x;
                float dy = a.y - b.y;
                float dz = a.z - b.z;
                global::System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(typeof(global::System.Math).TypeHandle);
                float squared = ((dy * dy) + (dx * dx)) + (dz * dz);
                length += (float)global::System.Math.Sqrt((double)squared);
            }
            return length;
        }

		public static void FrameLog(string str, int a)
		{
		}

		public static void SetLayerRecursively(this GameObject self, int layer)
		{
		}

		public static void RemoveInCondition<T>(this List<T> list, Func<T, bool> condition)
		{
		}

		public static bool IsIncludeLayer(int layerMask, int targetLayer)
		{
			return false;
		}

		public static bool MyHasFlag(this ExposedBodyParts self, ExposedBodyParts sub)
		{
			return false;
		}

		public static bool HasSomeFlag(this ExposedBodyParts self, ExposedBodyParts sub)
		{
			return false;
		}

		public static bool OnlyHasFlag(this ExposedBodyParts self, ExposedBodyParts sub)
		{
			// Recovered from GameAssembly.dll RVA 0x4B3920
			return (self & sub) == sub && self == sub;
		}

		public static bool MyHasFlag(this PlayerStateModel.HiddenBodyPartsByCostumeType self, PlayerStateModel.HiddenBodyPartsByCostumeType sub)
		{
			return false;
		}

		public static bool MyHasFlag(this Constant.DiscomfortDirect self, Constant.DiscomfortDirect sub)
		{
			return false;
		}
	}
}
