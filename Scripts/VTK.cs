using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace VTK
{
	public class VTK
	{
		public int normalType;
		// Triangles
		// Rough Left Coordinate System
		public List<List<int>> ts;
		// Well Left Coordinate System
		public List<List<int>> ts2;
		// Well Right Coordinate System
		public List<List<int>> ts3;
		// Well Right|Left Coordinate System
		public List<List<int>> ts4;

		// Points to vertices map
		// Rough Left Coordinate System
		public List<List<int>> map;
		// Well Left Coordinate System
		public List<List<int>> map2;
		// Well Right Coordinate System
		public List<List<int>> map3;
		// Well Right|Left Coordinate System
		public List<List<int>> map4;

		public VTK ()
		{
			normalType = 2;

			ts = new List<List<int>> ();
			ts2 = new List<List<int>> ();
			ts3 = new List<List<int>> ();
			ts4 = new List<List<int>> ();

			map = new List<List<int>> ();
			map2 = new List<List<int>> ();
			map3 = new List<List<int>> ();
			map4 = new List<List<int>> ();

			for (int i = 0; i < 15; i++) {
				if (i == 10) {
					// Tetrahedron
					ts.Add (new List<int> { 
						0, 1, 2,
						0, 2, 3,
						0, 3, 1,
						1, 3, 2
					});
					ts2.Add (new List<int> { 
						0, 3, 6,
						1, 7, 9,
						2, 10, 4,
						5, 11, 8
					});
					ts3.Add (new List<int> { 
						0, 6, 3,
						1, 9, 7,
						2, 4, 10,
						5, 8, 11
					});
					ts4.Add (new List<int> { 
						0, 6, 3,
						1, 9, 7,
						2, 4, 10,
						5, 8, 11,
						12, 15, 18,
						13, 19, 21,
						14, 22, 16,
						17, 23, 20
					});
					map.Add (new List<int> { 0, 1, 2, 3 });
					map2.Add (new List<int> { 
						0, 0, 0,
						1, 1, 1,
						2, 2, 2,
						3, 3, 3
					});
					map3.Add (new List<int> { 
						0, 0, 0,
						1, 1, 1,
						2, 2, 2,
						3, 3, 3
					});
					map4.Add (new List<int> { 
						0, 0, 0,
						1, 1, 1,
						2, 2, 2,
						3, 3, 3,
						0, 0, 0,
						1, 1, 1,
						2, 2, 2,
						3, 3, 3
					});
				} else if (i == 13) {
					// Wedge
					ts.Add (new List<int> { 
						// Bottom triangle
						0, 2, 1,
						// Top triangle
						3, 4, 5,
						// Bottom quadrangle
						1, 4, 3,
						1, 3, 0,
						// Face quadrangle
						2, 0, 5,
						0, 3, 5,
						// Back quadrangle
						1, 2, 4,
						2, 5, 4
					});
					ts2.Add (new List<int> {
						// Bottom triangle
						0, 8, 4,
						// Top triangle
						12, 16, 20,
						// Bottom quadrangle
						5, 17, 13,
						6, 14, 1,
						// Face quadrangle
						9, 3, 22,
						2, 15, 21,
						// Back quadrangle
						7, 10, 18,
						11, 23, 19
					});
					ts3.Add (new List<int> {
						// Bottom triangle
						0, 4, 8,
						// Top triangle
						12, 20, 16,
						// Bottom quadrangle
						5, 13, 17,
						6, 1, 14,
						// Face quadrangle
						9, 22, 3,
						2, 21, 15,
						// Back quadrangle
						7, 18, 10,
						11, 19, 23
					});
					ts4.Add (new List<int> {
						// Bottom triangle
						0, 4, 8,
						// Top triangle
						12, 20, 16,
						// Bottom quadrangle
						5, 13, 17,
						6, 1, 14,
						// Face quadrangle
						9, 22, 3,
						2, 21, 15,
						// Back quadrangle
						7, 18, 10,
						11, 19, 23,

						// Bottom triangle
						24, 32, 28,
						// Top triangle
						36, 40, 44,
						// Bottom quadrangle
						29, 41, 37,
						30, 38, 25,
						// Face quadrangle
						33, 27, 46,
						26, 39, 45,
						// Back quadrangle
						31, 34, 42,
						35, 47, 43
					});
					map.Add (new List<int> { 0, 1, 2, 3, 4, 5 });
					map2.Add (new List<int> { 
						0, 0, 0, 0,
						1, 1, 1, 1,
						2, 2, 2, 2,
						3, 3, 3, 3,
						4, 4, 4, 4,
						5, 5, 5, 5
					});
					map3.Add (new List<int> { 
						0, 0, 0, 0,
						1, 1, 1, 1,
						2, 2, 2, 2,
						3, 3, 3, 3,
						4, 4, 4, 4,
						5, 5, 5, 5
					});
					map4.Add (new List<int> { 
						0, 0, 0, 0,
						1, 1, 1, 1,
						2, 2, 2, 2,
						3, 3, 3, 3,
						4, 4, 4, 4,
						5, 5, 5, 5,
						0, 0, 0, 0,
						1, 1, 1, 1,
						2, 2, 2, 2,
						3, 3, 3, 3,
						4, 4, 4, 4,
						5, 5, 5, 5
					});
				} else if (i == 14) {
					// Pyramid
					ts.Add (new List<int> { 
						// Quadrangle
						0, 1, 2,
						0, 2, 3,
						// Triangles
						0, 3, 4,
						0, 4, 1,
						1, 4, 2,
						2, 4, 3
					});
					ts2.Add (new List<int> { 
						// Quadrangle
						0, 4, 7,
						1, 8, 11,
						// Triangles
						2, 12, 14,
						3, 15, 5,
						6, 16, 9,
						10, 17, 13
					});
					ts3.Add (new List<int> { 
						// Quadrangle
						0, 7, 4,
						1, 11, 8,
						// Triangles
						2, 14, 12,
						3, 5, 15,
						6, 9, 16,
						10, 13, 17
					});
					ts4.Add (new List<int> { 
						// Quadrangle
						0, 7, 4,
						1, 11, 8,
						// Triangles
						2, 14, 12,
						3, 5, 15,
						6, 9, 16,
						10, 13, 17,

						// Quadrangle
						18, 22, 25,
						19, 26, 29,
						// Triangles
						20, 30, 32,
						21, 33, 23,
						24, 34, 27,
						28, 35, 31
					});
					map.Add (new List<int> { 0, 1, 2, 3, 4 });
					map2.Add (new List<int> { 
						0, 0, 0, 0,
						1, 1, 1,
						2, 2, 2, 2,
						3, 3, 3,
						4, 4, 4, 4
					});
					map3.Add (new List<int> { 
						0, 0, 0, 0,
						1, 1, 1,
						2, 2, 2, 2,
						3, 3, 3,
						4, 4, 4, 4
					});
					map4.Add (new List<int> { 
						0, 0, 0, 0,
						1, 1, 1,
						2, 2, 2, 2,
						3, 3, 3,
						4, 4, 4, 4,
						0, 0, 0, 0,
						1, 1, 1,
						2, 2, 2, 2,
						3, 3, 3,
						4, 4, 4, 4
					});
				} else {
					ts.Add (new List<int> ());
					ts2.Add (new List<int> ());
					ts3.Add (new List<int> ());
					ts4.Add (new List<int> ());
					map.Add (new List<int> ());
					map2.Add (new List<int> ());
					map3.Add (new List<int> ());
					map4.Add (new List<int> ());
				}
			}
		}

		public int[] GetTriangles (int cellType)
		{
			if (normalType == 0) {
				return ts [cellType].ToArray ();
			} else if (normalType == 1) {
				return ts2 [cellType].ToArray ();
			} else if (normalType == 2) {
				return ts3 [cellType].ToArray ();
			} else {
				return ts4 [cellType].ToArray ();
			}

		}

		public int[] GetPointsToVerticesMap (int cellType)
		{
			if (normalType == 0) {
				return map [cellType].ToArray ();
			} else if (normalType == 1) {
				return map2 [cellType].ToArray ();
			} else if (normalType == 2) {
				return map3 [cellType].ToArray ();
			} else {
				return map4 [cellType].ToArray ();
			}
		}
	}

	public class PolyData
	{
		// Max faces in mesh = 65534/3 = 21844, where 65534 - max vertices in mesh
		public const int MAX_MESH_VERTICES = 65534;
		public const int MAX_MESH_TRIANGLES = 21844;
		public string filename;
		float[] points;
		int[] vertices;
		int nVertices;
		int[] lines;
		int nLines;
		int[] polygons;
		int nPolygons;
		int[] triangleStrips;
		int nTriangleStrips;

		public PolyData (string filename)
		{
			this.filename = filename;
			points = new float[0];
			vertices = new int[0];
			nVertices = 0;
			lines = new int[0];
			nLines = 0;
			polygons = new int[0];
			nPolygons = 0;
			triangleStrips = new int[0];
			nTriangleStrips = 0;
		}

		public void Read ()
		{
			bool isPoints = false;
			bool isVertices = false;
			bool isLines = false;
			bool isPolygons = false;
			bool isTriangleStrips = false;
			int cnt = 0;
			try {
				using (StreamReader sr = new StreamReader (filename)) {
					string line;
					while ((line = sr.ReadLine ()) != null) {
						string[] tokens = line.Split ();
						if (tokens [0] == "POINTS") {
							Debug.Log ("Reading POINTS");
							isPoints = true;
							points = new float[int.Parse (tokens [1]) * 3];
							cnt = 0;
							continue;
						} else if (tokens [0] == "VERTICES") {
							Debug.Log ("Reading VERTICES");
							isPoints = false;
							isVertices = true;
							nVertices = int.Parse (tokens [1]);
							vertices = new int[int.Parse (tokens [2])];
							cnt = 0;
							continue;
						} else if (tokens [0] == "LINES") {
							Debug.Log ("Reading LINES");
							isPoints = false;
							isVertices = false;
							isLines = true;
							nLines = int.Parse (tokens [1]);
							lines = new int[int.Parse (tokens [2])];
							cnt = 0;
							continue;
						} else if (tokens [0] == "POLYGONS") {
							Debug.Log ("Reading POLYGONS");
							isPoints = false;
							isVertices = false;
							isLines = false;
							isPolygons = true;
							nPolygons = int.Parse (tokens [1]);
							polygons = new int[int.Parse (tokens [2])];
							cnt = 0;
							continue;
						} else if (tokens [0] == "TRIANGLE_STRIPS") {
							Debug.Log ("Reading TRIANGLE_STRIPS");
							isPoints = false;
							isVertices = false;
							isLines = false;
							isPolygons = false;
							isTriangleStrips = true;
							nTriangleStrips = int.Parse (tokens [1]);
							triangleStrips = new int[int.Parse (tokens [2])];
							cnt = 0;
							continue;
						} else if (tokens [0] == "CELL_DATA" || tokens [0] == "POINT_DATA") {
							break;
						}

						if (isPoints) {
							for (int i = 0; i < tokens.Length; i++) {
								if (!string.IsNullOrEmpty (tokens [i])) {
									points [cnt] = float.Parse (tokens [i]);
									cnt += 1;
								}
							}
						} else if (isVertices) {
							for (int i = 0; i < tokens.Length; i++) {
								if (!string.IsNullOrEmpty (tokens [i])) {
									vertices [cnt] = int.Parse (tokens [i]);
									cnt += 1;
								}
							}
						} else if (isLines) {
							for (int i = 0; i < tokens.Length; i++) {
								if (!string.IsNullOrEmpty (tokens [i])) {
									lines [cnt] = int.Parse (tokens [i]);
									cnt += 1;
								}
							}
						} else if (isPolygons) {
							for (int i = 0; i < tokens.Length; i++) {
								if (!string.IsNullOrEmpty (tokens [i])) {
									polygons [cnt] = int.Parse (tokens [i]);
									cnt += 1;
								}
							}
						} else if (isTriangleStrips) {
							for (int i = 0; i < tokens.Length; i++) {
								if (!string.IsNullOrEmpty (tokens [i])) {
									triangleStrips [cnt] = int.Parse (tokens [i]);
									cnt += 1;
								}
							}
						}
					}
				}
			} catch (Exception e) {
				Debug.Log (string.Format ("Error reading {0}: {1}", filename, e.Message));
			}
		}

		public GameObject ConvertToUnityMesh ()
		{
			GameObject unityMesh = new GameObject ();			
			unityMesh.name = Path.GetFileNameWithoutExtension (filename);
			int meshCnt = 0;
			int verticesCnt = 0;
			int trianglesCnt = 0;
			int cnt = 0;
			GameObject childMesh = new GameObject ();
			childMesh.name = string.Format ("{0}MeshPart_{1:D}", Path.GetFileNameWithoutExtension (filename), meshCnt);
			childMesh.transform.parent = unityMesh.transform;
			childMesh.AddComponent<MeshFilter> ();
			childMesh.AddComponent<MeshRenderer> ();
			childMesh.AddComponent<GlobalVertices> ();
			Mesh mesh = new Mesh ();
			mesh.name = string.Format ("{0}MeshPart_{1:D}", Path.GetFileNameWithoutExtension (filename), meshCnt);
			childMesh.GetComponent<MeshFilter> ().mesh = mesh;
			List<int> triangles = new List<int> ();
			List<Vector3> vertices = new List<Vector3> ();
			List<int> globalVertices = new List<int> ();
			List<int> globalCells = new List<int> ();
			while (cnt < polygons.Length) {
				int nPolygonPoints = polygons [cnt];
				if (trianglesCnt + nPolygonPoints - 2 > MAX_MESH_TRIANGLES) {
					// Initialize previous child mesh
					mesh.vertices = vertices.ToArray ();
					mesh.triangles = triangles.ToArray ();
					mesh.RecalculateNormals ();
					mesh.RecalculateBounds ();
					childMesh.GetComponent<GlobalVertices> ().globalVertices = globalVertices;
					childMesh.GetComponent<GlobalVertices> ().globalCells = globalCells;
					// Create new child mesh
					meshCnt += 1;
					childMesh = new GameObject ();
					childMesh.name = string.Format ("{0}MeshPart_{1:D}", Path.GetFileNameWithoutExtension (filename), meshCnt);
					childMesh.transform.parent = unityMesh.transform;
					childMesh.AddComponent<MeshFilter> ();
					childMesh.AddComponent<MeshRenderer> ();
					childMesh.AddComponent<GlobalVertices> ();
					mesh = new Mesh ();
					mesh.name = string.Format ("{0}MeshPart_{1:D}", Path.GetFileNameWithoutExtension (filename), meshCnt);
					childMesh.GetComponent<MeshFilter> ().mesh = mesh;
					triangles = new List<int> ();
					vertices = new List<Vector3> ();
					globalVertices = new List<int> ();
					globalCells = new List<int> ();
				}
				if (nPolygonPoints == 3) {
					for (int i = cnt + 1; i < cnt + 1 + nPolygonPoints; i++) {
						triangles.Add (verticesCnt);
						vertices.Add (new Vector3 (points [polygons [i] * 3], points [polygons [i] * 3 + 2], points [polygons [i] * 3 + 1]));
						globalVertices.Add (polygons [i]);
						verticesCnt += 1;
					}
					globalCells.Add (trianglesCnt);
					trianglesCnt += 1;
				}
				cnt += nPolygonPoints + 1;
			}
			mesh.vertices = vertices.ToArray ();
			mesh.triangles = triangles.ToArray ();
			mesh.RecalculateNormals ();
			mesh.RecalculateBounds ();
			childMesh.GetComponent<GlobalVertices> ().globalVertices = globalVertices;
			childMesh.GetComponent<GlobalVertices> ().globalCells = globalCells;

			return unityMesh;
		}

		public void Clear ()
		{
			points = new float[0];
			vertices = new int[0];
			nVertices = 0;
			lines = new int[0];
			nLines = 0;
			polygons = new int[0];
			nPolygons = 0;
			triangleStrips = new int[0];
			nTriangleStrips = 0;
		}
	}

	public class LookupTable
	{
		public const string DEFAULT_TABLE_NAME = "default";
		public string tableName;
		int size;
		int numComp;
		public float[] values;

		public LookupTable (int size, string tableName = DEFAULT_TABLE_NAME, int numComp = 4)
		{
			this.tableName = tableName;
			this.size = size;
			this.numComp = numComp;
			values = new float[size * numComp];
		}
	}

	public class Scalars
	{
		string dataName;
		string dataType;
		public LookupTable table;

		public Scalars (string dataName, string dataType, int size, string tableName = LookupTable.DEFAULT_TABLE_NAME, int numComp = 1)
		{
			this.dataName = dataName;
			this.dataType = dataType;
			table = new LookupTable (size, tableName, numComp);
		}
	}

	[System.Serializable]
	public class Array
	{
		public string arrayName;
		public int numComp;
		public int numTuples;
		public string dataType;
		public float[] values;
		public float[] limits;

		public Array (string arrayName, int numComp, int numTuples, string dataType)
		{
			this.arrayName = arrayName;
			this.numComp = numComp;
			this.numTuples = numTuples;
			values = new float[numComp * numTuples];
			limits = new float[2];
		}

		void CalculateLimits ()
		{
			limits = new float[2]; // [max, min]
			if (values.Length > 0) {
				float max = values [0];
				float min = values [0];
				for (int i = 0; i < values.Length; i++) {
					if (values [i] > max) {
						max = values [i];
					} else if (values [i] < min) {
						min = values [i];
					}
				}
				limits [0] = max;
				limits [1] = min;
			} else {
				throw new System.Exception ("Array values.Length == 0");
			}
		}

		public float[] RelativeValues (float max, float min)
		{
			float[] relValues = new float[values.Length];
			float delta = max - min;
			if (delta > 0) {
				for (int i = 0; i < values.Length; i++) {
					relValues [i] = (values [i] - min) / delta;
				}
			}
			return relValues;
		}

		public void SetValues (float[] values)
		{
			if (values.Length != this.values.Length) {
				throw new System.ArgumentOutOfRangeException ("Array values.Length != new values.Length");
			}
			this.values = values;
			CalculateLimits ();
		}
	}

	[System.Serializable]
	public class Field
	{
		public string dataName;
		public int numArrays;
		public Array[] arrays;

		public Field (string dataName, int numArrays)
		{
			this.dataName = dataName;
			this.numArrays = numArrays;
			arrays = new Array[numArrays];
		}
	}

	[System.Serializable]
	public class DataSetAttributes
	{
		public string filename;
		public List<Scalars> scalars;
		public List<LookupTable> tables;
		public List<Field> fields;
		public Field pointField;

		public DataSetAttributes (string filename)
		{
			this.filename = filename;
			scalars = new List<Scalars> ();
			tables = new List<LookupTable> ();
			fields = new List<Field> ();
			pointField = new Field ("default", 0);
		}

		public Array GetCellArray (int arrayIdx)
		{
			return fields [0].arrays [arrayIdx];
		}

		public Array GetCellArray (string arrayName)
		{
			foreach (Array array in fields[0].arrays) {
				if (array.arrayName == arrayName) {
					return array;
				}
			}
			throw new System.Exception ("Array not found");
		}

		public Array GetPointArray (int arrayIdx)
		{
			Debug.Log (pointField.numArrays);
			return pointField.arrays [arrayIdx];
		}

		public Array GetPointArray (string arrayName)
		{
			foreach (Array array in pointField.arrays) {
				if (array.arrayName == arrayName) {
					return array;
				}
			}
			throw new System.Exception ("Array not found");
		}

		public void Read ()
		{
			Clear ();
			bool isCellData = false;
			int nCells = 0;
			bool isPointData = false;
			int nPoints = 0;

			bool isScalars = false;
			string scalarsDataName = "";
			string scalarsDataType = "";
			int scalarsNumComp = 1;
			bool isTables = false;
			bool isField = false;
			bool isNewArray = false;
			int cnt = 0;
			float[] values = new float[0];
			int arrayCnt = 0;
			try {
				using (StreamReader sr = new StreamReader (filename)) {
					string line;
					while ((line = sr.ReadLine ()) != null) {
						string[] tokens = line.Split ();
						if (tokens [0] == "CELL_DATA") {
//							Debug.Log ("Reading CELL_DATA");
//							Debug.Log (line);
							isCellData = true;
							nCells = int.Parse (tokens [1]);
							continue;
						} else if (tokens [0] == "SCALARS") {
//							Debug.Log ("Reading SCALARS");
//							Debug.Log (line);
							isScalars = true;
							scalarsDataName = tokens [1];
							scalarsDataType = tokens [2];
							if (tokens.Length > 3) {
								scalarsNumComp = int.Parse (tokens [3]);
							}
							if (isCellData) {
								scalars.Add (new Scalars (scalarsDataName, scalarsDataType, nCells, LookupTable.DEFAULT_TABLE_NAME, scalarsNumComp));
							} else if (isPointData) {
								scalars.Add (new Scalars (scalarsDataName, scalarsDataType, nPoints, LookupTable.DEFAULT_TABLE_NAME, scalarsNumComp));
							}
							cnt = 0;
							continue;
						} else if (tokens [0] == "LOOKUP_TABLE") {
//							Debug.Log ("Reading LOOKUP_TABLE");
//							Debug.Log (line);
							if (isScalars) {
								scalars [scalars.Count - 1].table.tableName = tokens [1];
							}
							continue;
						} else if (tokens [0] == "POINT_DATA") {
							isCellData = false;
							isPointData = true;
							nPoints = int.Parse (tokens [1]);
							continue;
						} else if (tokens [0] == "CELL_DATA") {
							isCellData = true;
							isPointData = false;
							nCells = int.Parse (tokens [1]);
							continue;
						} else if (tokens [0] == "FIELD") {
//							Debug.Log ("Reading FIELDS");
//							Debug.Log (line);
							isScalars = false;
							isField = true;
							isNewArray = true;
							if (isCellData) {
								fields.Add (new Field (tokens [1], int.Parse (tokens [2])));
							} else if (isPointData) {
								pointField = new Field (tokens [1], int.Parse (tokens [2]));
							}
							arrayCnt = 0;
							continue;
						} else if (isNewArray) {
							if (!string.IsNullOrEmpty (line)) {
								Array array = new Array (tokens [0], int.Parse (tokens [1]), int.Parse (tokens [2]), tokens [3]);
								arrayCnt += 1;
								if (isCellData) {
									fields [fields.Count - 1].arrays[arrayCnt - 1] = array;
								} else if (isPointData) {
									pointField.arrays[arrayCnt - 1] = array;
								}
								values = new float[array.values.Length];
								cnt = 0;
								isNewArray = false;
							}
							continue;
						}
						if (isScalars) {
							for (int i = 0; i < tokens.Length; i++) {
								if (!string.IsNullOrEmpty (tokens [i])) {
									scalars [scalars.Count - 1].table.values [cnt] = float.Parse (tokens [i]);
									cnt += 1;
								}
							}
						}
						if (isField) {
							for (int i = 0; i < tokens.Length; i++) {
								if (!string.IsNullOrEmpty (tokens [i])) {
									values [cnt] = float.Parse (tokens [i]);
									cnt += 1;
								}
							}
							if (cnt == values.Length) {
								if (isCellData) {
									fields [fields.Count - 1].arrays [arrayCnt - 1].SetValues (values);
								} else if (isPointData) {
									pointField.arrays [arrayCnt - 1].SetValues (values);
								}
								isNewArray = true;
							}
						}
					}
				}
			} catch (Exception e) {
				Debug.Log (string.Format ("Error while reading {0}: {1}", filename, e));
			}
		}

		public void Clear ()
		{
			scalars = new List<Scalars> ();
			tables = new List<LookupTable> ();
			fields = new List<Field> ();
			pointField = new Field ("default", 0);
		}
	}
}

