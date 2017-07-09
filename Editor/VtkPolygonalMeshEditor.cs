using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(VtkPolygonalMesh))]
public class VtkPolygonalMeshEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		VtkPolygonalMesh myScript = (VtkPolygonalMesh)target;
		if (GUILayout.Button ("Read")) {
			myScript.Read ();
		}
		if (GUILayout.Button ("Import")) {
			myScript.ImportMesh ();
		}
		if (GUILayout.Button ("Set Current Array")) {
			myScript.SetCurrentPointArray (myScript.currentArrayIdx);
		}
			
		if (GUILayout.Button ("Set Current Array Colors")) {
			myScript.SetCurrentArrayByColors (myScript.currentArrayIdx);
		}
	}
}
