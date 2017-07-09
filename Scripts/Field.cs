using System;
using System.Collections.Generic;
using UnityEngine;

namespace VTK
{
	[System.Serializable]
	public class FieldOld : System.Object
	{
		[HideInInspector]
		public List<float> data;
		public string name;
		public float maxValue;
		public float minValue;
		public float range;
		public int nElements;

		public FieldOld (string n)
		{
			data = new List<float> ();
			name = n;
		}

		public void AddTimeStepField (List<float> f)
		{
			data.AddRange (f);
			nElements = f.Count;
		}

		public List<float> GetTimeStepField (int i)
		{
			List<float> timeStepField = data.GetRange (i * nElements, nElements);
			return timeStepField;
		}

		public int GetNTimeSteps ()
		{
			return data.Count / nElements;
		}

		public void EvaluateLimits ()
		{
			maxValue = GetMaxValue ();
			minValue = GetMinValue ();
			range = maxValue - minValue;
		}

		public float GetMaxValue ()
		{
			float maxValue = float.MinValue;
			foreach (float value in data) {
				if (value > maxValue) {
					maxValue = value;
				}
			}
			return maxValue;
		}

		public float GetMinValue ()
		{
			float minValue = float.MaxValue;
			foreach (float value in data) {
				if (value < minValue) {
					minValue = value;
				}
			}
			return minValue;
		}

		public Color[] GetTimeStepColors (int i)
		{
			List<float> timeStepField = GetTimeStepField (i);
			Color[] colors = new Color[timeStepField.Count];
			for (int j = 0; j < timeStepField.Count; j++) {
				float relativeValue;
				if (range != 0) {
					relativeValue = (timeStepField [j] - minValue) / range;
				} else {
					relativeValue = 0;
				}
				if (relativeValue < 0.25) {
					colors [j] = Color.Lerp (Color.blue, Color.cyan, relativeValue * 4f);
				} else if (relativeValue < 0.5) {
					colors [j] = Color.Lerp (Color.cyan, Color.green, (relativeValue - 0.25f) * 4f);
				} else if (relativeValue < 0.75) {
					colors [j] = Color.Lerp (Color.green, Color.yellow, (relativeValue - 0.5f) * 4f);
				} else {
					colors [j] = Color.Lerp (Color.yellow, Color.red, (relativeValue - 0.75f) * 4f);
				}
			}
			return colors;
		}

		public bool[] GetTimeStepThresholdMap (float min, float max, int i)
		{
			List<float> timeStepField = GetTimeStepField (i);
			bool[] map = new bool[timeStepField.Count];
			float minV = minValue + min * range;
			float maxV = minValue + max * range;
			for (int j = 0; j < timeStepField.Count; j++) {
				if (timeStepField [j] >= minV && timeStepField [j] <= maxV) {
					map [j] = true;
				}
			}
			return map;
		}
	}
}

