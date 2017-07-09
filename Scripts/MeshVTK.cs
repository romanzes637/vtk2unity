using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MeshVTK : MonoBehaviour
{
	public string path;
	public int normalType;
	public GameObject cell;
	public TimeManager timeManager;
	public FieldsInspector fieldsInspector;
	public CellManager cellManager;

	void Reset ()
	{
		path = "Modules/Gera";
		normalType = 0;
		timeManager = gameObject.AddComponent<TimeManager> ();
		fieldsInspector = gameObject.AddComponent<FieldsInspector> ();
		cellManager = gameObject.AddComponent<CellManager> ();
	}
		
	public void Import ()
	{
		Debug.Log ("Reading mesh");
		// Mesh data
		List<List<float>> points = new List<List<float>> ();
		List<int> cells = new List<int> ();
		List<int> cellTypes = new List<int> ();

		// All time steps fields
		List<List<List<float>>> timeFields = new List<List<List<float>>> ();
		List<List<string>> timeFieldsNames = new List<List<string>> ();

		// Get all *.vtk files in the path directory
		DirectoryInfo di = new DirectoryInfo (Application.dataPath + "/" + path);
		FileInfo[] fi = di.GetFiles ("*.vtk");

		// Read files
		// Read mesh data only from the first file
		bool isReadMesh = true;
		for (int j = 0; j < 1; j++) {
			if (j > 0) {
				isReadMesh = false;
			}
			Debug.Log ("Reading " + fi [j].Name);
			StreamReader sr = new StreamReader (Application.dataPath + "/" + path + "/" + fi [j].Name);

			// Fields data
			List<List<float>> fields = new List<List<float>> ();
			List<string> fieldsNames = new List<string> ();
			List<float> field = new List<float> ();

			bool isPoints = false;
			bool isCells = false;
			bool isCellTypes = false;
			bool isCellData = false;
			bool isReadField = false;

			while (!sr.EndOfStream) {
				string line = sr.ReadLine ();
				string[] tokens = line.Split ();
				if (tokens [0] == "POINTS") {
					Debug.Log ("Reading POINTS");
					isPoints = true;
					continue;
				} else if (tokens [0] == "CELLS") {
					Debug.Log ("Reading CELLS");
					isPoints = false;
					isCells = true;
					continue;
				} else if (tokens [0] == "CELL_TYPES") {
					Debug.Log ("Reading CELL_TYPES");
					isCells = false;
					isCellTypes = true;
					continue;
				} else if (tokens [0] == "CELL_DATA") {
					Debug.Log ("Reading CELL_DATA");
					isCellTypes = false;
					isCellData = true;
					continue;
				}
				if (isPoints && isReadMesh) {
					List<float> coordinates = new List<float> ();
					for (int i = 0; i < tokens.Length; i++) {
						if (!string.IsNullOrEmpty (tokens [i])) {
							coordinates.Add (float.Parse (tokens [i]));
						}
					}
					points.Add (coordinates);
				} else if (isCells && isReadMesh) {
					for (int i = 0; i < tokens.Length; i++) {
						if (!string.IsNullOrEmpty (tokens [i])) {
							cells.Add (int.Parse (tokens [i]));
						}
					}
				} else if (isCellTypes && isReadMesh) {
					for (int i = 0; i < tokens.Length; i++) {
						if (!string.IsNullOrEmpty (tokens [i])) {
							cellTypes.Add (int.Parse (tokens [i]));
						}
					}
				} else if (isCellData) {
					if (tokens [0] == "SCALARS") {
						if (isReadField) {
							isReadField = false;
							fields.Add (field);
							if (fields.Count > 1) {
								break; // TODO 1 field reading
							}
						}
						Debug.Log ("Reading " + tokens [1]);
						fieldsNames.Add (tokens [1]);
						continue;
					} else if (tokens [0] == "LOOKUP_TABLE") {
						isReadField = true;
						field = new List<float> ();
						continue;
					} else if (isReadField) {
						bool isAdded = false;
						for (int i = 0; i < tokens.Length; i++) {
							if (!string.IsNullOrEmpty (tokens [i])) {
								field.Add (float.Parse (tokens [i]));
								isAdded = true;
								break;
							}
						}
						if (!isAdded) {
							field.Add (0);
						}
					}
				}
			}
			fields.Add (field);
			sr.Close ();
			timeFields.Add (fields);
			timeFieldsNames.Add (fieldsNames);
			Debug.Log ("Number of fields = " + fields.Count);
			Debug.Log ("Number of field names = " + fieldsNames.Count);
		}

		Debug.Log ("Number of points = " + points.Count);
		Debug.Log ("Cells length = " + cells.Count);
		Debug.Log ("Number of cell types = " + cellTypes.Count);
		Debug.Log ("Number of time fields = " + timeFields.Count);

		Debug.Log ("Converting fields");
		// Creating fields
		List<VTK.FieldOld> fs = new List<VTK.FieldOld> ();
		for (int i = 0; i < timeFieldsNames.Count; i++) {
			foreach (string fn in timeFieldsNames[i]) {
				bool isNewField = true;
				// Check if field with name fn already exists in fs
				foreach (VTK.FieldOld f in fs) {
					if (f.name == fn) {
						isNewField = false;
						continue;
					}
				}
				if (isNewField) {
					fs.Add (new VTK.FieldOld (fn));
				}
			}
		}
		Debug.Log ("Number of unity fields = " + fs.Count);

		// Initialising fields
		for (int i = 0; i < timeFieldsNames.Count; i++) {
			for (int j = 0; j < timeFieldsNames [i].Count; j++) {
				foreach (VTK.FieldOld f in fs) {
					if (f.name == timeFieldsNames [i] [j]) {
						f.AddTimeStepField (timeFields [i] [j]);
						continue;
					}
				}
			}
		}

		// Print fields
//		foreach (AssemblyCSharp.Field f in fs) {
//			Debug.Log (f.name);
//			for (int i = 0; i < f.GetNTimeSteps(); i++) {
//				Debug.Log ("Time step = " + i);
//				foreach (float v in f.GetTimeStepField (i)) {
//					Debug.Log (v);
//				}
//			}
//		}

		// Functions test
		foreach (VTK.FieldOld f in fs) {
			Debug.Log (f.name);
			f.EvaluateLimits ();
//			Debug.Log ("Max value = " + f.maxValue);
//			Debug.Log ("Min value = " + f.minValue);
//			Debug.Log ("Range = " + f.range);
//			for (int i = 0; i < f.GetNTimeSteps (); i++) {
//				Debug.Log ("Time step = " + i);
//				Color[] colors = f.GetTimeStepColors (i);
//				foreach (Color c in colors) {
//					Debug.Log (c);
//				}
//			}
//			gameObject.AddComponent<UnityField> ();
		}
			
		timeManager.Initialize (timeFields.Count - 1);
		fieldsInspector.Initialize ();
		foreach (VTK.FieldOld f in fs) {
//			Debug.Log (f.name);
			fieldsInspector.AddField (f);
		}
		cellManager.DeleteCells ();

		Debug.Log ("Converting mesh");
		VTK.VTK vtk = new VTK.VTK ();
		vtk.normalType = normalType;
		int cellsCnt = 0;
		int nCellPoints = 0;
		int cellType = 0;
		bool isNewCell = true;
		List<int> cellPoints = new List<int> ();
		Mesh mesh = new Mesh ();
		for (int i = 0; i < cells.Count; i++) {
			if (isNewCell) {
				cellsCnt += 1;
				nCellPoints = cells [i];
				cellType = cellTypes [cellsCnt - 1];
				cellPoints = new List<int> ();
//				Debug.Log ("Cell type = " + cellType);
//				Debug.Log ("Number of cell points = " + nCellPoints);
//				Debug.Log ("Number of cell triangles = " + ts.Length / 3);
				// Create GameObject
				GameObject c = Instantiate (cell, transform);
//				mesh.name = (cellsCnt - 1).ToString ();
				c.GetComponent<MeshFilter> ().mesh = mesh;
				cellManager.AddCell (c);
				mesh = new Mesh ();
				isNewCell = false;
				continue;
			} else {
				cellPoints.Add (cells [i]);
				if (cellPoints.Count == nCellPoints) {
					int[] ts = vtk.GetTriangles (cellType);
					int[] map = vtk.GetPointsToVerticesMap (cellType);
					Vector3[] vs = new Vector3[map.Length];
					for (int j = 0; j < vs.Length; j++) {
						int pointIndex = cellPoints [map [j]];
						// Map X -> X, Y -> Z, Z -> Y (Unity Y is VTK Z)
						vs [j] = new Vector3 (points [pointIndex] [0], points [pointIndex] [2], points [pointIndex] [1]);
					}
					mesh.vertices = vs;
					mesh.triangles = ts;
					mesh.RecalculateBounds ();
					mesh.RecalculateNormals ();
					isNewCell = true;
				}
			}
		}
	}
}
