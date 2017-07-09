using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CellManager : MonoBehaviour
{
//	[HideInInspector]
	public List<GameObject> cells;
	public FieldsInspector fieldsManager;
	public TimeManager timeManager;

	void Reset ()
	{
		cells = new List<GameObject> ();
		timeManager = gameObject.GetComponent<TimeManager> ();
		fieldsManager = gameObject.GetComponent<FieldsInspector> ();
	}
		
	// Update is called once per frame
	void LateUpdate ()
	{
		if (fieldsManager.isChanged || timeManager.isChanged) {
			UpdateCellsColors ();
			UpdateCellsActive ();
		}

		if (fieldsManager.isThresholdChanged) {
			UpdateCellsActive ();
		}
	}

	public void UpdateCellsColors ()
	{
		Color[] currentFieldColors = fieldsManager.GetCurrentFieldColors ();
		for (int i = 0; i < currentFieldColors.Length; i++) {
			cells [i].GetComponent<MeshRenderer> ().material.SetColor ("_Color", currentFieldColors [i]);
//			Mesh mesh = cells [i].GetComponent<MeshFilter> ().sharedMesh;
//			Color[] cs = new Color[mesh.vertices.Length];
//			for (int j = 0; j < cs.Length; j++) {
//				cs [j] = currentFieldColors [i];
//			}
//			mesh.colors = cs;
		}
	}

	public void UpdateCellsActive ()
	{
		bool[] map = fieldsManager.GetCurrentFieldThresholdMap ();
		for (int i = 0; i < cells.Count; i++) {
			cells [i].SetActive (map [i]);
		}
	}

	public void DeleteCells ()
	{
		foreach (GameObject cell in cells) {
			DestroyImmediate (cell);
		}
		cells = new List<GameObject> ();
	}

	public void AddCell (GameObject cell)
	{
		cells.Add (cell);
	}
}
