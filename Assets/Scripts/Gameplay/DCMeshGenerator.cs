using UnityEngine;

namespace Gameplay
{
	[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(PolygonCollider2D))]
	[ExecuteInEditMode]
	public class DCMeshGenerator : MonoBehaviour
	{
		private Mesh _mesh;
		private Vector3[] _vertices;
		private int[] _triangles;
		private Vector2[] _uvs;

		void Awake()
		{
			_mesh = new Mesh();
			GetComponent<MeshFilter>().mesh = _mesh;
		}

		public void Activate(bool _generateOnX, bool _add3DMeshCollider, float _riseBy, float _lengthOnX, Material _material)
		{
			UpdateMaterial(_material);
			CreateShape(_generateOnX, _add3DMeshCollider, _riseBy, _lengthOnX);
			UpdateMesh();
		}

		void CreateShape(bool generateOnX, bool add3DMeshCollider, float riseBy, float length)
		{
			if (generateOnX)
			{
				_vertices = new Vector3[]
				{
					//front face//
					new(0, 1, 1),//left top front, 0
					new(length, 1 + riseBy, 1),//right top front, 1
					new(0,0,1),//left bottom front, 2
					new(length,riseBy,1),//right bottom front, 3

					//back face//
					new(length, 1 + riseBy, 0),//right top back, 4
					new(0, 1, 0),//left top back, 5
					new(length,riseBy,0),//right bottom back, 6
					new(0,0,0),//left bottom back, 7

					//left face//
					new(0, 1, 0),//left top back, 8
					new(0, 1, 1),//left top front, 9
					new(0,0,0),//left bottom back, 10
					new(0,0,1),//left bottom front, 11

					//right face//
					new(length, 1 + riseBy, 1),//right top front, 12
					new(length, 1 + riseBy, 0),//right top back, 13
					new(length,riseBy,1),//right bottom front, 14
					new(length,riseBy,0),//right bottom back, 15

					//top face//
					new(0, 1, 0),//left top back, 16
					new(length, 1 + riseBy, 0),//right top back, 17
					new(0, 1, 1),//left top front, 18
					new(length, 1 + riseBy, 1),//right top front, 19

					//bottom face//
					new(0,0,1),//left bottom front, 20
					new(length,riseBy,1),//right bottom front, 21
					new(0,0,0),//left bottom back, 22
					new(length,riseBy,0)//right bottom back, 23
				};
			}
			else
			{
				_vertices = new Vector3[]
				{
					//front face//
					new(0,1+riseBy,length),//left top front, 0
					new(1,1+riseBy,length),//right top front, 1
					new(0,riseBy,length),//left bottom front, 2
					new(1,riseBy,length),//right bottom front, 3

					//back face//
					new(1,1,0),//right top back, 4
					new(0,1,0),//left top back, 5
					new(1,0,0),//right bottom back, 6
					new(0,0,0),//left bottom back, 7

					//left face//
					new(0,1,0),//left top back, 8
					new(0,1+riseBy,length),//left top front, 9
					new(0,0,0),//left bottom back, 10
					new(0,riseBy,length),//left bottom front, 11

					//right face//
					new(1,1+riseBy, length),//right top front, 12
					new(1,1,0),//right top back, 13
					new(1,riseBy,length),//right bottom front, 14
					new(1,0,0),//right bottom back, 15

					//top face//
					new(0,1,0),//left top back, 16
					new(1,1,0),//right top back, 17
					new(0,1+riseBy,length),//left top front, 18
					new(1,1+riseBy,length),//right top front, 19

					//bottom face//
					new(0,riseBy,length),//left bottom front, 20
					new(1,riseBy,length),//right bottom front, 21
					new(0,0,0),//left bottom back, 22
					new(1,0,0)//right bottom back, 23
				};
			}

			_triangles = new[]
			{
				//front face//
				0,2,3,//first triangle
				3,1,0,//second triangle

				//back face//
				4,6,7,//first triangle
				7,5,4,//second triangle

				//left face//
				8,10,11,//first triangle
				11,9,8,//second triangle

				//right face//
				12,14,15,//first triangle
				15,13,12,//second triangle

				//top face//
				16,18,19,//first triangle
				19,17,16,//second triangle

				//bottom face//
				20,22,23,//first triangle
				23,21,20//second triangle
			};

			_uvs = new Vector2[]
			{
				//front face// 0,0 is bottom left, 1,1 is top right//
				new(0,1),
				new(0,0),
				new(1,1),
				new(1,0),

				new(0,1),
				new(0,0),
				new(1,1),
				new(1,0),

				new(0,1),
				new(0,0),
				new(1,1),
				new(1,0),

				new(0,1),
				new(0,0),
				new(1,1),
				new(1,0),

				new(0,1),
				new(0,0),
				new(1,1),
				new(1,0),

				new(0,1),
				new(0,0),
				new(1,1),
				new(1,0)
			};

			if (add3DMeshCollider)
			{
				gameObject.AddComponent<MeshCollider>().convex = true;
			}
			else
			{
				Vector2[] colliderNewPoints = new Vector2[4];
				for (int i = 0; i < 4; i++)
					colliderNewPoints[i] = _vertices[i];
				gameObject.GetComponent<PolygonCollider2D>().SetPath(0, colliderNewPoints);
			}
		}

		void UpdateMesh()
		{
			_mesh.Clear();
			_mesh.vertices = _vertices;
			_mesh.triangles = _triangles;
			_mesh.uv = _uvs;
			_mesh.RecalculateNormals();
			DestroyImmediate(this);
		}

		void UpdateMaterial(Material _material)
		{
			GetComponent<MeshRenderer>().material = _material;
		}
	}
}
