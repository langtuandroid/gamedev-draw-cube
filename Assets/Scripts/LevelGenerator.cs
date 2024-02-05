using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class LevelGenerator : MonoBehaviour
{
    #region Variables

    [Header("Level Generation")]
    public bool generateLevel = false;
    public bool resetLevel = false;
    public bool deleteLastSpawned = false;

    [Header("Generation Properties")]
    [Space]
    [Tooltip("You can olny change this if there are no spawned cubes")]
    public bool generateAtEnd = true;
    public bool generateOnX = true;
    public bool add3DMeshCollider = false;

    [Header("Cube Properties")]
    [Space]
    [Tooltip("How many cubes do you want to spawn")]
    public int cubesPerPattern;
    [Tooltip("Index of the curve which you would like to spawn")]
    [Range(0, 20)]
    public int whichPattern = 0;

    [Range(0.25f, 1.5f)]
    public float cubeLength = 0.5f;
    [Range(3f, 20f)]
    public float heightMultiplier = 10f;

    [Space]
    [Tooltip("You can add new patterns here")]
    public AnimationCurve[] cubePatterns;
    public Material[] materials;
    public GameObject cube;

    private Vector3 startPos, cubesStartPos;
    private float riseBy = 1f;
    private int generatedPatterns = 0;

    private List<GameObject> cubeHolders = new List<GameObject>();
    private List<Vector3> tempPositions = new List<Vector3>();

    private int tempMaterialIndex = 0;
    private bool tempGenerateAtEndVariable = false;
    private GameObject cubeHolder, tempCube;

    #endregion

    void Update()
    {
        whichPattern = Mathf.Clamp(whichPattern, 0, cubePatterns.Length - 1);       //Clamps the value of the "whichPattern" variable

        if (generateLevel)     //If Generate Level "button" is clicked in the Inspector
        {
            generateLevel = false;

            if (generatedPatterns == 0)     //If this is the first spawn
            {
                tempGenerateAtEndVariable = generateAtEnd;
                cubesStartPos = transform.position;
            }

            //Spawning the parent of the cubes
            if (tempGenerateAtEndVariable)
            {
                cubeHolder = Instantiate(new GameObject(), transform.position, Quaternion.identity);
                cubeHolder.name = "Cubes " + generatedPatterns.ToString();
                cubeHolders.Add(cubeHolder);
                DestroyImmediate(GameObject.Find("New Game Object"));
            }
            else
            {
                Instantiate(new GameObject("Cubes " + generatedPatterns), transform);
                DestroyImmediate(GameObject.Find("Cubes " + generatedPatterns.ToString()));
            }

            tempPositions.Add(startPos = transform.position);

            for (int i = 0; i < cubesPerPattern; i++)
            {
                //Calculates riseBy by evaluating the selected pattern curve
                riseBy = (cubePatterns[whichPattern].Evaluate((float)i / (float)cubesPerPattern) - cubePatterns[whichPattern].Evaluate((float)(i - 1) / (float)cubesPerPattern)) * heightMultiplier;

                //Choosing material
                if (tempMaterialIndex + 1 < materials.Length)
                    tempMaterialIndex++;
                else
                    tempMaterialIndex = 0;

                //Spawning the cube
                if (tempGenerateAtEndVariable)
                    tempCube = Instantiate(cube, cubeHolder.transform);
                else
                    tempCube = Instantiate(cube, transform.GetChild(generatedPatterns).transform);
                //Creating the cube and changing its position
                tempCube.GetComponent<MeshGenerator>().Activate(generateOnX, add3DMeshCollider, riseBy, cubeLength, materials[tempMaterialIndex]);
                tempCube.transform.localPosition = new Vector3(transform.position.x - startPos.x, transform.position.y - startPos.y, transform.position.z - startPos.z);

                //Moving this gameobject forward or right
                if (generateOnX)
                    transform.position = new Vector3(transform.position.x + cubeLength, transform.position.y + riseBy, transform.position.z);
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y + riseBy, transform.position.z + cubeLength);
            }
            if (!tempGenerateAtEndVariable)
                transform.GetChild(generatedPatterns).position = cubesStartPos;

            generatedPatterns++;
        }

        if (resetLevel)     //If Reset Level "button" is clicked in the Inspector
        {
            resetLevel = false;
            transform.position = cubesStartPos;
            ResetLevel();
        }

        if (deleteLastSpawned)     //If Delete Last Spawned "button" is clicked in the Inspector
        {
            deleteLastSpawned = false;
            if (generatedPatterns > 0)
            {
                transform.position = tempPositions[generatedPatterns - 1];
                DeleteLastSpawned();
            }
            else
                Debug.LogWarning("There are no more cubes to remove!");
        }
    }

    void DeleteLastSpawned()
    {
        if (tempGenerateAtEndVariable)
        {
            DestroyImmediate(cubeHolders[generatedPatterns - 1]);
            cubeHolders.RemoveAt(generatedPatterns - 1);
        }
        else
            DestroyImmediate(transform.GetChild(generatedPatterns - 1).gameObject);

        tempPositions.RemoveAt(generatedPatterns - 1);
        generatedPatterns--;
    }

    void ResetLevel()
    {
        generatedPatterns = 0;

        if (tempGenerateAtEndVariable)
        {
            int cubeHoldersCount = cubeHolders.Count;
            for (int i = 0; i < cubeHoldersCount; i++)        //Loops through the cubeHolders
                DestroyImmediate(cubeHolders[i]);       //And destroys the object attached to them
        }

        else
        {
            int childs = transform.childCount;
            for (int i = 0; i < childs; i++)        //Loops through the children of the gameobject
                DestroyImmediate(transform.GetChild(0).gameObject);     //And destroys them
        }

        //Clears the lists
        cubeHolders.Clear();
        tempPositions.Clear();
    }
}
