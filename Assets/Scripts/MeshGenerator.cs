using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer), typeof(PolygonCollider2D))]
[ExecuteInEditMode]
public class MeshGenerator : MonoBehaviour
{
    #region Variables

    private Mesh mesh;
    private Vector3[] vertices;
    private int[] triangles;
    private Vector2[] uvs;

    #endregion

    void Awake()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
    }

    public void Activate(bool _generateOnX, bool _add3DMeshCollider, float _riseBy, float _lengthOnX, Material _material)
    {
        UpdateMaterial(_material);
        CreateShape(_generateOnX, _add3DMeshCollider, _riseBy, _lengthOnX);
        UpdateMesh();
    }

    void CreateShape(bool generateOnX, bool add3DMeshCollider, float riseBy, float length)
    {
        #region Generating Verticles

        if (generateOnX)
        {
            vertices = new Vector3[]
            {
			//front face//
			new Vector3(0, 1, 1),//left top front, 0
			new Vector3(length, 1 + riseBy, 1),//right top front, 1
			new Vector3(0,0,1),//left bottom front, 2
			new Vector3(length,riseBy,1),//right bottom front, 3

			//back face//
			new Vector3(length, 1 + riseBy, 0),//right top back, 4
			new Vector3(0, 1, 0),//left top back, 5
			new Vector3(length,riseBy,0),//right bottom back, 6
			new Vector3(0,0,0),//left bottom back, 7

			//left face//
			new Vector3(0, 1, 0),//left top back, 8
			new Vector3(0, 1, 1),//left top front, 9
			new Vector3(0,0,0),//left bottom back, 10
			new Vector3(0,0,1),//left bottom front, 11

			//right face//
			new Vector3(length, 1 + riseBy, 1),//right top front, 12
			new Vector3(length, 1 + riseBy, 0),//right top back, 13
			new Vector3(length,riseBy,1),//right bottom front, 14
			new Vector3(length,riseBy,0),//right bottom back, 15

			//top face//
			new Vector3(0, 1, 0),//left top back, 16
			new Vector3(length, 1 + riseBy, 0),//right top back, 17
			new Vector3(0, 1, 1),//left top front, 18
			new Vector3(length, 1 + riseBy, 1),//right top front, 19

			//bottom face//
			new Vector3(0,0,1),//left bottom front, 20
			new Vector3(length,riseBy,1),//right bottom front, 21
			new Vector3(0,0,0),//left bottom back, 22
			new Vector3(length,riseBy,0)//right bottom back, 23
            };
        }
        else
        {
            vertices = new Vector3[]
            {
			//front face//
			new Vector3(0,1+riseBy,length),//left top front, 0
			new Vector3(1,1+riseBy,length),//right top front, 1
			new Vector3(0,riseBy,length),//left bottom front, 2
			new Vector3(1,riseBy,length),//right bottom front, 3

			//back face//
			new Vector3(1,1,0),//right top back, 4
			new Vector3(0,1,0),//left top back, 5
			new Vector3(1,0,0),//right bottom back, 6
			new Vector3(0,0,0),//left bottom back, 7

			//left face//
			new Vector3(0,1,0),//left top back, 8
			new Vector3(0,1+riseBy,length),//left top front, 9
			new Vector3(0,0,0),//left bottom back, 10
			new Vector3(0,riseBy,length),//left bottom front, 11

			//right face//
			new Vector3(1,1+riseBy, length),//right top front, 12
			new Vector3(1,1,0),//right top back, 13
			new Vector3(1,riseBy,length),//right bottom front, 14
			new Vector3(1,0,0),//right bottom back, 15

			//top face//
			new Vector3(0,1,0),//left top back, 16
			new Vector3(1,1,0),//right top back, 17
			new Vector3(0,1+riseBy,length),//left top front, 18
			new Vector3(1,1+riseBy,length),//right top front, 19

			//bottom face//
			new Vector3(0,riseBy,length),//left bottom front, 20
			new Vector3(1,riseBy,length),//right bottom front, 21
			new Vector3(0,0,0),//left bottom back, 22
			new Vector3(1,0,0)//right bottom back, 23
            };
        }

        #endregion

        #region Generating Triangles

        triangles = new int[]
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

        #endregion

        #region Generating UVs

        uvs = new Vector2[]
        {
			//front face// 0,0 is bottom left, 1,1 is top right//
			new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0),

            new Vector2(0,1),
            new Vector2(0,0),
            new Vector2(1,1),
            new Vector2(1,0)
        };

        #endregion

        #region Generating Collider

        if (add3DMeshCollider)
        {
            //Adds Mesh Collider to the gameobject
            gameObject.AddComponent<MeshCollider>().convex = true;
        }
        else
        {
            //Adds Polygon Collider 2D to the gameobject
            Vector2[] colliderNewPoints = new Vector2[4];
            for (int i = 0; i < 4; i++)
                colliderNewPoints[i] = vertices[i];
            gameObject.GetComponent<PolygonCollider2D>().SetPath(0, colliderNewPoints);
        }

        #endregion
    }

    void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        DestroyImmediate(this);
    }

    void UpdateMaterial(Material _material)
    {
        GetComponent<MeshRenderer>().material = _material;
    }
}
