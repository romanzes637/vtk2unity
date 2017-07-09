using UnityEngine;
using System.Collections;

public class TimeManager : MonoBehaviour
{
	[SerializeField]
	private int timeStepIndex;
	public int TimeStepIndex { get {return timeStepIndex;} set {timeStepIndex = value;} }

	public int previousTimeStepIndex;
	public int maxTimeStepIndex;
	public bool isChanged = false;

	// Update is called once per frame
	void Update ()
	{
		if (previousTimeStepIndex != timeStepIndex) {
			isChanged = true;
			previousTimeStepIndex = timeStepIndex;
		} else {
			isChanged = false;
		}
	}

	public void Initialize (int maxIndex)
	{
		timeStepIndex = 0;
		previousTimeStepIndex = -1;
		maxTimeStepIndex = maxIndex;
	}
}
