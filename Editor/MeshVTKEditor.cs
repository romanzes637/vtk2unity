using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(MeshVTK))]
public class MeshVTKEditor : Editor
{
	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();

		MeshVTK myScript = (MeshVTK)target;
		if (GUILayout.Button ("Import")) {
			myScript.Import ();
		}
	}
}
