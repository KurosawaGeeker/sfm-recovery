using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Master.BodyPaint
{
	[CreateAssetMenu(fileName = "MBodyPaint", menuName = "ScriptableObjects/MBodyPaint")]
	public class MBodyPaint : ScriptableObject
	{
		[SerializeField]
		private RBodyPaint[] records;

		private static Dictionary<BodyPaintType, RBodyPaint> dict;

		private static void CheckInitialize()
		{
            if (dict != null) return;
            dict = new Dictionary<BodyPaintType, RBodyPaint>();
            foreach (var record in global::ExposureUnnoticed2.Object3D.IngameManager.AssetReferencer.Instance.MBodyPaint.records)
                dict.Add(record.Type, record);
		}

		public static RBodyPaint Get(BodyPaintType type)
		{
CheckInitialize();
return dict.TryGetValue(type, out var record) ? record : null;
}

		public static Dictionary<BodyPaintType, RBodyPaint> GetAll()
		{
            CheckInitialize();
            return dict;
		}
	}
}
