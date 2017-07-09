using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class VtkPolygonalMesh : MonoBehaviour
{
	public GameObject mesh;
	public string meshFilename;
	public string dataDirname;
	public int nTimeSteps = 30;
	public VTK.DataSetAttributes[] attributes;
	public string currentArrayName = "";
	public float[] currentRelValues = new float[0];
	public int currentArrayLength = 0;
	public Color[] currentColors = new Color[0];
	public int currentArrayIdx;
	public int previousArrayIdx;

	public float currentYear { set; get; }

	public float curArrayIdx { set; get; }

	public int currentTimeStep;
	public float prevArrayIdx;
	public float previousYear;

	// Use this for initialization
	void Start ()
	{
		previousArrayIdx = -1;
		previousYear = -1;
		prevArrayIdx = -1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (currentYear != previousYear) {
			currentTimeStep = (int)currentYear / 100;
			if (currentTimeStep > nTimeSteps) {
				currentTimeStep = nTimeSteps;
			}
			SetColorsByCurrentPointArray (currentTimeStep);
			previousYear = currentYear;
		}
		if (curArrayIdx != prevArrayIdx) {
			int currentArrayIdx = (int)curArrayIdx;
			SetCurrentPointArray (currentArrayIdx);
			SetColorsByCurrentPointArray (currentTimeStep);
			prevArrayIdx = curArrayIdx;
		}
	}

	public void Read ()
	{
		DirectoryInfo di = new DirectoryInfo (Application.dataPath + "/" + dataDirname);
		FileInfo[] fi = di.GetFiles ("*.vtk");
		Array.Sort (fi, (f1, f2) => f1.CreationTime.CompareTo (f2.CreationTime));
		attributes = new VTK.DataSetAttributes[nTimeSteps];
		for (int i = 0; i < nTimeSteps; i++) {
			Debug.Log ("Reading " + fi [i].Name);
			attributes [i] = new VTK.DataSetAttributes (Application.dataPath + "/" + dataDirname + "/" + fi [i].Name);
			attributes [i].Read ();
		}
	}

	public void ImportMesh ()
	{
		VTK.PolyData dataset = new VTK.PolyData (Application.dataPath + "/" + meshFilename);
		dataset.Read ();
		mesh = dataset.ConvertToUnityMesh ();
		mesh.transform.parent = transform;
	}

	public VTK.Array GetTimeStepCellArray (int timeStepIdx, int arrayIdx)
	{
		return attributes [timeStepIdx].GetCellArray (arrayIdx);
	}

	public VTK.Array GetTimeStepCellArray (int timeStepIdx, string arrayName)
	{
		return attributes [timeStepIdx].GetCellArray (arrayName);
	}

	public VTK.Array GetTimeStepPointArray (int timeStepIdx, int arrayIdx)
	{
		return attributes [timeStepIdx].GetPointArray (arrayIdx);
	}

	public VTK.Array GetTimeStepPointArray (int timeStepIdx, string arrayName)
	{
		return attributes [timeStepIdx].GetPointArray (arrayName);
	}

	public float[] GetPointArrayLimits (string arrayName)
	{
		float[] limits = new float[2]; // [max, min]
		if (attributes.Length > 0) {
			float max = GetTimeStepPointArray (0, arrayName).limits [0];
			float min = GetTimeStepPointArray (0, arrayName).limits [1];
			for (int i = 0; i < attributes.Length; i++) {
				float[] arrayLimits = GetTimeStepPointArray (i, arrayName).limits;
				Debug.Log (string.Format ("{3} {0} Limits Max: {1} Min: {2}", arrayName, arrayLimits [0], arrayLimits [1], i));
				if (arrayLimits [0] > max) {
					max = arrayLimits [0];
				} else if (arrayLimits [1] < min) {
					min = arrayLimits [1];
				}
			}
			limits [0] = max;
			limits [1] = min;
		}
		Debug.Log (string.Format ("Global {0} Limits Max: {1} Min: {2}", arrayName, limits [0], limits [1]));
		return limits;
	}

	public float[] GetCellArrayLimits (string arrayName)
	{
		float[] limits = new float[2]; // [max, min]
		if (attributes.Length > 0) {
			float max = GetTimeStepCellArray (0, arrayName).limits [0];
			float min = GetTimeStepCellArray (0, arrayName).limits [1];
			for (int i = 0; i < attributes.Length; i++) {
				float[] arrayLimits = GetTimeStepCellArray (i, arrayName).limits;
				Debug.Log (string.Format ("{0} Limits Max: {1} Min: {2}", arrayName, arrayLimits [0], arrayLimits [1]));
				if (arrayLimits [0] > max) {
					max = arrayLimits [0];
				} else if (arrayLimits [1] < min) {
					min = arrayLimits [1];
				}
			}
			limits [0] = max;
			limits [1] = min;
		}
		Debug.Log (string.Format ("Global {0} Limits Max: {1} Min: {2}", arrayName, limits [0], limits [1]));
		return limits;
	}

	public void SetCurrentArray (int arrayIdx)
	{
		currentArrayIdx = arrayIdx;
		currentArrayName = GetTimeStepCellArray (0, arrayIdx).arrayName;
		if (attributes.Length > 0) {
			List<float> relValues = new List<float> ();
			float[] limits = GetCellArrayLimits (currentArrayName);
			for (int i = 0; i < attributes.Length; i++) {
				float[] timeStepRelValues = GetTimeStepCellArray (i, currentArrayName).RelativeValues (limits [0], limits [1]);
				currentArrayLength = timeStepRelValues.Length;
				for (int j = 0; j < timeStepRelValues.Length; j++) {
					relValues.Add (timeStepRelValues [j]);
				}
			}
			currentRelValues = relValues.ToArray ();
		}
	}

	public void SetCurrentPointArray (int arrayIdx)
	{
		currentArrayIdx = arrayIdx;
		currentArrayName = GetTimeStepPointArray (0, arrayIdx).arrayName;
		if (attributes.Length > 0) {
			List<float> relValues = new List<float> ();
			float[] limits = GetPointArrayLimits (currentArrayName);
			for (int i = 0; i < attributes.Length; i++) {
				float[] timeStepRelValues = GetTimeStepPointArray (i, currentArrayName).RelativeValues (limits [0], limits [1]);
				currentArrayLength = timeStepRelValues.Length;
				for (int j = 0; j < timeStepRelValues.Length; j++) {
					relValues.Add (timeStepRelValues [j]);
				}
			}
			currentRelValues = relValues.ToArray ();
		}
	}

	public void SetColorsByCurrentPointArray (int timeStepIdx)
	{
		System.DateTime startTime = System.DateTime.Now;
		for (int i = 0; i < mesh.transform.childCount; i++) {
			GameObject childMeshObject = mesh.transform.GetChild (i).gameObject;
			Mesh childMesh = childMeshObject.GetComponent<MeshFilter> ().sharedMesh;
			List<int> globalVertices = childMeshObject.GetComponent<GlobalVertices> ().globalVertices;
			Color[] colors = new Color[childMesh.vertices.Length];
			int startIdx = currentArrayLength * timeStepIdx;
			for (int j = 0; j < colors.Length; j++) {
				float relValue = currentRelValues [startIdx + globalVertices [j]];
				if (relValue < 0.25) {
					colors [j] = Color.Lerp (Color.blue, Color.cyan, relValue * 4f);
				} else if (relValue < 0.5) {
					colors [j] = Color.Lerp (Color.cyan, Color.green, (relValue - 0.25f) * 4f);
				} else if (relValue < 0.75) {
					colors [j] = Color.Lerp (Color.green, Color.yellow, (relValue - 0.5f) * 4f);
				} else {
					colors [j] = Color.Lerp (Color.yellow, Color.red, (relValue - 0.75f) * 4f);
				}
			}
			childMesh.colors = colors;
		}
		Debug.Log (System.DateTime.Now - startTime);
	}



	public void SetCurrentArrayByColors (int arrayIdx)
	{
		currentArrayIdx = arrayIdx;
		currentArrayName = GetTimeStepCellArray (0, arrayIdx).arrayName;
		if (attributes.Length > 0) {
			List<Color> colors = new List<Color> ();
			float[] limits = GetCellArrayLimits (currentArrayName);
			for (int i = 0; i < attributes.Length; i++) {
				float[] timeStepRelValues = 
					GetTimeStepCellArray (i, currentArrayName).RelativeValues (
						limits [0], limits [1]);
				currentArrayLength = timeStepRelValues.Length;
				for (int j = 0; j < timeStepRelValues.Length; j++) {
					if (timeStepRelValues [j] < 0.25) {
						colors.Add (Color.Lerp (Color.blue, Color.cyan, timeStepRelValues [j] * 4f));
					} else if (timeStepRelValues [j] < 0.5) {
						colors.Add (Color.Lerp (Color.cyan, Color.green, (timeStepRelValues [j] - 0.25f) * 4f));
					} else if (timeStepRelValues [j] < 0.75) {
						colors.Add (Color.Lerp (Color.green, Color.yellow, (timeStepRelValues [j] - 0.5f) * 4f));
					} else {
						colors.Add (Color.Lerp (Color.yellow, Color.red, (timeStepRelValues [j] - 0.75f) * 4f));
					}
				}
			}
			currentColors = colors.ToArray ();
		}
	}

	public void SetMeshCellsColorsByColors (int timeStepIdx)
	{
		System.DateTime startTime = System.DateTime.Now;
		for (int i = 0; i < mesh.transform.childCount; i++) {
			GameObject childMeshObject = mesh.transform.GetChild (i).gameObject;
			Mesh childMesh = childMeshObject.GetComponent<MeshFilter> ().sharedMesh;
			List<int> globalCells = childMeshObject.GetComponent<GlobalVertices> ().globalCells;
			Color[] colors = new Color[childMesh.vertices.Length];
			int trianglesCnt = 0;
			int startIdx = currentArrayLength * timeStepIdx;
			for (int j = 0; j < childMesh.triangles.Length; j++) {
				if (j != 0 && j % 3 == 0) {
					trianglesCnt++;
				}
				colors [childMesh.triangles [j]] = currentColors [startIdx + globalCells [trianglesCnt]];
			}
			childMesh.colors = colors;
		}
		Debug.Log (System.DateTime.Now - startTime);
	}

	public void SetMeshCellsColors (int timeStepIdx)
	{
		System.DateTime startTime = System.DateTime.Now;
		for (int i = 0; i < mesh.transform.childCount; i++) {
			GameObject childMeshObject = mesh.transform.GetChild (i).gameObject;
			Mesh childMesh = childMeshObject.GetComponent<MeshFilter> ().sharedMesh;
			List<int> globalCells = childMeshObject.GetComponent<GlobalVertices> ().globalCells;
			Color[] colors = new Color[childMesh.vertices.Length];
			int trianglesCnt = 0;
			int startIdx = currentArrayLength * timeStepIdx;
			for (int j = 0; j < childMesh.triangles.Length; j++) {
				if (j != 0 && j % 3 == 0) {
					trianglesCnt++;
				}
				float relValue = currentRelValues [startIdx + globalCells [trianglesCnt]];
				if (relValue < 0.25) {
					colors [childMesh.triangles [j]] = Color.Lerp (Color.blue, Color.cyan, relValue * 4f);
				} else if (relValue < 0.5) {
					colors [childMesh.triangles [j]] = Color.Lerp (Color.cyan, Color.green, (relValue - 0.25f) * 4f);
				} else if (relValue < 0.75) {
					colors [childMesh.triangles [j]] = Color.Lerp (Color.green, Color.yellow, (relValue - 0.5f) * 4f);
				} else {
					colors [childMesh.triangles [j]] = Color.Lerp (Color.yellow, Color.red, (relValue - 0.75f) * 4f);
				}
			}
			childMesh.colors = colors;
		}
		Debug.Log (System.DateTime.Now - startTime);
	}
}
