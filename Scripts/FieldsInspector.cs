using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldsInspector : MonoBehaviour
{
	public int fieldIndex;
	public int previousFieldIndex;
	public bool isChanged = false;
	public bool isThresholdChanged = false;
	public List<VTK.FieldOld> fields;
	public TimeManager timeManager;
	[SerializeField]
	private float minThreshold;
	public float MinThreshold { get {return minThreshold;} set {minThreshold = value;} }
	[SerializeField]
	private float maxThreshold;
	public float MaxThreshold { get {return maxThreshold;} set {maxThreshold = value;} }
	public float previousMinThreshold;
	public float previousMaxThreshold;

	void Reset ()
	{
		timeManager = transform.GetComponent<TimeManager> ();
	}
		
	// Update is called once per frame
	void Update ()
	{
		if (previousFieldIndex != fieldIndex) {
			isChanged = true;
			previousFieldIndex = fieldIndex;

		} else {
			isChanged = false;
		}

		if (previousMinThreshold != minThreshold || previousMaxThreshold != maxThreshold) {
			isThresholdChanged = true;
			previousMinThreshold = minThreshold;
			previousMaxThreshold = maxThreshold;
		} else {
			isThresholdChanged = false;
		}
	}

	public void Initialize ()
	{
		fieldIndex = 0;
		previousFieldIndex = -1;
		minThreshold = 0;
		maxThreshold = 1;
		previousMinThreshold = minThreshold;
		previousMaxThreshold = maxThreshold;
		fields = new List<VTK.FieldOld> ();
	}

	public void AddField (VTK.FieldOld f)
	{
		fields.Add (f);
	}

	public VTK.FieldOld GetCurrentField ()
	{
		return fields [fieldIndex];
	}

	public Color[] GetCurrentFieldColors ()
	{
		return fields [fieldIndex].GetTimeStepColors(timeManager.TimeStepIndex);
	}

	public bool[] GetCurrentFieldThresholdMap ()
	{
		return fields [fieldIndex].GetTimeStepThresholdMap (minThreshold, maxThreshold, timeManager.TimeStepIndex);
	}
}
