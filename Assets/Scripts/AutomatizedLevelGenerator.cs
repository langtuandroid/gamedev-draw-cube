using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutomatizedLevelGenerator : MonoBehaviour
{
    #region Variables

    [Header("Level Generation")]
    public bool generateLevel = false;
    public bool resetLevel = false;

    [Header("Generation Properties")]
    [Space]
    public bool generateOnX = true;
    public bool add3DMeshCollider = false;

    [Header("Cube Properties")]
    [Space]
    [Tooltip("How many cubes do you want to spawn")]
    public int countOfCubes = 20;
    [Range(0.25f, 1.5f)]
    public float cubeLength = 0.5f;

    [Space]
    public int minCubesPerPattern = 10;
    public int maxCubesPerPattern = 80;

    [Space]
    public float cubeHeightMin = 3f;
    public float cubeHeightMax = 20f;

    [Space]
    [Tooltip("You can add new patterns here")]
    public AnimationCurve[] cubePatterns;
    public Material[] materials;
    public GameObject cube, levenEnd;

    private Vector3 startPos;
    private float riseBy = 1f;
    private int cubesPerPattern;
    private float tempCubeHeight;
    private static int tempMaterialIndex = 0;
    private static int spawnedCubes = 0;
    private int whichPattern = 0;

    #endregion

    void Update()
    {
        if (generateLevel)     //If Generate Level "button" is clicked in the Inspector
        {
            generateLevel = false;
            startPos = transform.position;
            ResetLevel();       //First resets level

            do
            {
                //Calculates the random preferencies
                cubesPerPattern = Random.Range(minCubesPerPattern, maxCubesPerPattern);
                whichPattern = Random.Range(0, cubePatterns.Length);
                tempCubeHeight = Random.Range(cubeHeightMin, cubeHeightMax);

                for (int i = 0; i < cubesPerPattern; i++)
                {
                    //Calculates riseBy by evaluating the randomly selected pattern curve
                    riseBy = (cubePatterns[whichPattern].Evaluate((float)i / (float)cubesPerPattern) - cubePatterns[whichPattern].Evaluate((float)(i - 1) / (float)cubesPerPattern)) * tempCubeHeight;

                    //Choosing material
                    if (tempMaterialIndex + 1 < materials.Length)
                        tempMaterialIndex++;
                    else
                        tempMaterialIndex = 0;

                    //Spawning the cube
                    GameObject tempCube = Instantiate(cube, transform);
                    tempCube.GetComponent<MeshGenerator>().Activate(generateOnX, add3DMeshCollider, riseBy, cubeLength, materials[tempMaterialIndex]);
                    tempCube.transform.localPosition = new Vector3(transform.position.x - startPos.x, transform.position.y - startPos.y, transform.position.z - startPos.z);

                    //Moving this gameobject forward or right
                    if (generateOnX)
                        transform.position = new Vector3(transform.position.x + cubeLength, transform.position.y + riseBy, transform.position.z);
                    else
                        transform.position = new Vector3(transform.position.x, transform.position.y + riseBy, transform.position.z + cubeLength);

                    //Checks if spawned all of the cubes
                    spawnedCubes++;
                    if (spawnedCubes >= countOfCubes)
                        break;
                }
            } while (spawnedCubes < countOfCubes);

            GameObject levelEnd = Instantiate(levenEnd, transform);
            levelEnd.transform.localPosition = new Vector3(transform.position.x - startPos.x, transform.position.y - startPos.y, transform.position.z - startPos.z);
            spawnedCubes = 0;
            transform.position = startPos;      //Resets position of the gameobject to the starting position
        }

        if (resetLevel)     //If Reset Level "button" is clicked in the Inspector
        {
            resetLevel = false;
            ResetLevel();
        }
    }

    void ResetLevel()
    {
        int childs = transform.childCount;
        for (int i = 0; i < childs; i++)        //Loops through the children of the gameobject
            DestroyImmediate(transform.GetChild(0).gameObject);     //And destroys them
    }
}
