using System;
using System.Collections.Generic;
using UnityEngine;

namespace ExposureUnnoticed2.Scripts.Base
{
	public class Scheduler : MonoBehaviour
	{
		private class DelayFrameModel
		{
			private Action action;

			private int delayFrame;

			public DelayFrameModel(Action action, int delayFrame)
			{
			// Recovered from GameAssembly.dll RVA 0x4B4E80
			this.action = action;
			this.delayFrame = delayFrame;
		}

			public bool CountdownAndInvokeIfNeed()
			{
            // Complete original RVA4B4EE0 including original managed-exception catch/Console path.
            if (delayFrame != 0)
            {
                --delayFrame;
                return false;
            }
            try { action(); }
            catch (System.Exception exception) { System.Console.WriteLine(exception); }
            return true;
        }
		}

		private class DelayTimeModel
		{
			private Action action;

			private float delayTime;

			private float startTime;

			private float remainTime;

			private bool isUnscaled;

			public DelayTimeModel(Action action, float delayTime, bool isUnscaled = false)
			{
			// Recovered from GameAssembly.dll RVA 0x4B4F50
			this.action = action;
			this.delayTime = delayTime;
			remainTime = delayTime;
			this.isUnscaled = isUnscaled;
			startTime = isUnscaled ? Time.unscaledTime : Time.time;
		}

			public bool InvokeIfNeed()
			{
			// Recovered from GameAssembly.dll RVA 0x4B5050
			if (!global::ExposureUnnoticed2.Scripts.InGame.InGameManager.Instance.TempInGameState.IsTimeStop || isUnscaled)
			    remainTime -= isUnscaled ? Time.unscaledDeltaTime : Time.deltaTime;
			if (remainTime > 0f) return false;
			action();
			return true;
		}
		}

		private class DelayConditionModel
		{
			private Action action;

			private Func<bool> condition;

			private float maxSecond;

			private float startTime;

			public DelayConditionModel(Action action, Func<bool> condition, float maxSecond)
			{
			// Recovered from GameAssembly.dll RVA 0x4B51C0
			this.action = action;
			this.condition = condition;
			this.maxSecond = maxSecond;
			startTime = Time.time;
		}

			public bool InvokeIfNeed()
			{
			// Recovered from GameAssembly.dll RVA 0x4B52D0
			if (Time.time - startTime > maxSecond)
			    throw new Exception("Scheduler: MaxSecond経過しても条件は満たされませんでした");
			if (!condition()) return false;
			action();
			return true;
		}
		}

		private static Scheduler instance;

		private List<DelayFrameModel> modelList;

		private List<DelayFrameModel> removeList;

		private List<DelayFrameModel> modelListClone;

		private List<DelayTimeModel> timeModelList;

		private List<DelayTimeModel> timeRemoveList;

		private List<DelayTimeModel> timeModelListClone;

		private List<DelayConditionModel> conditionModelList;

		private List<DelayConditionModel> conditionRemoveList;

		private List<DelayConditionModel> conditionModelListClone;

		// Recovered from GameAssembly.dll RVA 0x4B3A30.
        private static Scheduler Instance
        {
            get
            {
                if (instance == null)
                {
                    GameObject obj = new GameObject("Scheduler");
                    UnityEngine.Object.DontDestroyOnLoad(obj);
                    instance = obj.AddComponent<Scheduler>();
                }
                return instance;
            }
        }

		public static void DelayFlame(Action action, int delayFrame = 1)
		{
			// Recovered from GameAssembly.dll RVA 0x4B3C60
			Instance.modelList.Add(new DelayFrameModel(action, delayFrame));
		}

		public static void DelayUnscaledTime(Action action, float delayTime)
		{
			// Recovered from GameAssembly.dll RVA 0x4B3D40
			Instance.timeModelList.Add(new DelayTimeModel(action, delayTime, true));
		}

		public static void DelayScaledTime(Action action, float delayTime)
		{
			// Recovered from GameAssembly.dll RVA 0x4B3DF0
			Instance.timeModelList.Add(new DelayTimeModel(action, delayTime, false));
		}

		public static void DelayCondition(Action action, Func<bool> condition, float maxSecond = 10f)
		{
			// Recovered from GameAssembly.dll RVA 0x4B3EA0
			Instance.conditionModelList.Add(new DelayConditionModel(action, condition, maxSecond));
		}

		private void Update()
		{
			// Recovered from GameAssembly.dll RVA 0x4B4040
			removeList.Clear();
			modelListClone.Clear();
			foreach (DelayFrameModel model in modelList) modelListClone.Add(model);
			foreach (DelayFrameModel model in modelListClone)
			    if (model.CountdownAndInvokeIfNeed()) removeList.Add(model);
			foreach (DelayFrameModel model in removeList) modelList.Remove(model);
			timeRemoveList.Clear();
			timeModelListClone.Clear();
			foreach (DelayTimeModel model in timeModelList) timeModelListClone.Add(model);
			foreach (DelayTimeModel model in timeModelListClone)
			    if (model.InvokeIfNeed()) timeRemoveList.Add(model);
			foreach (DelayTimeModel model in timeRemoveList) timeModelList.Remove(model);
			conditionRemoveList.Clear();
			conditionModelListClone.Clear();
			foreach (DelayConditionModel model in conditionModelList) conditionModelListClone.Add(model);
			foreach (DelayConditionModel model in conditionModelListClone)
			    if (model.InvokeIfNeed()) conditionRemoveList.Add(model);
			foreach (DelayConditionModel model in conditionRemoveList) conditionModelList.Remove(model);
		}
	// Recovered original member RVA 0x4B49C0: .ctor
		public Scheduler()
        {
            modelList = new List<DelayFrameModel>();
            removeList = new List<DelayFrameModel>();
            modelListClone = new List<DelayFrameModel>();
            timeModelList = new List<DelayTimeModel>();
            timeRemoveList = new List<DelayTimeModel>();
            timeModelListClone = new List<DelayTimeModel>();
            conditionModelList = new List<DelayConditionModel>();
            conditionRemoveList = new List<DelayConditionModel>();
            conditionModelListClone = new List<DelayConditionModel>();
        }
	}
}
