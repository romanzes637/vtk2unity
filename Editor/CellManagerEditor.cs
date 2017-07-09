using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(CellManager))]
public class CellManagerEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		CellManager myScript = (CellManager)target;
		if (GUILayout.Button ("Delete Cells")) {
			myScript.DeleteCells ();
		}
	}
}
