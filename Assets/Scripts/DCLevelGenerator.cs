using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class DCLevelGenerator : MonoBehaviour
{
    #region Variables

    [Header("Level Generation")]
    [SerializeField] private bool generateLevel = false;
    [SerializeField] private bool resetLevel = false;
    [SerializeField] private bool deleteLastSpawned = false;

    [Header("Generation Properties")]
    [Space]
    [Tooltip("You can olny change this if there are no spawned cubes")]
    [SerializeField] private bool generateAtEnd = true;
    [SerializeField] private bool generateOnX = true;
    [SerializeField] private bool add3DMeshCollider = false;

    [Header("Cube Properties")]
    [Space]
    [Tooltip("How many cubes do you want to spawn")]
    [SerializeField] private int cubesPerPattern;
    [Tooltip("Index of the curve which you would like to spawn")]
    [Range(0, 20)]
    [SerializeField] private int whichPattern = 0;

    [Range(0.25f, 1.5f)]
    [SerializeField] private float cubeLength = 0.5f;
    [Range(3f, 20f)]
    [SerializeField] private float heightMultiplier = 10f;

    [Space]
    [Tooltip("You can add new patterns here")]
    [SerializeField] private AnimationCurve[] cubePatterns;
    [SerializeField] private Material[] materials;
    [SerializeField] private GameObject cube;

    private Vector3 _startPos, _cubesStartPos;
    private float _riseBy = 1f;
    private int _generatedPatterns = 0;

    private List<GameObject> _cubeHolders = new();
    private List<Vector3> _tempPositions = new();

    private int _tempMaterialIndex = 0;
    private bool _tempGenerateAtEndVariable = false;
    private GameObject _cubeHolder, _tempCube;

    #endregion

    void Update()
    {
        whichPattern = Mathf.Clamp(whichPattern, 0, cubePatterns.Length - 1);       //Clamps the value of the "whichPattern" variable

        if (generateLevel)     //If Generate Level "button" is clicked in the Inspector
        {
            generateLevel = false;

            if (_generatedPatterns == 0)     //If this is the first spawn
            {
                _tempGenerateAtEndVariable = generateAtEnd;
                _cubesStartPos = transform.position;
            }

            //Spawning the parent of the cubes
            if (_tempGenerateAtEndVariable)
            {
                _cubeHolder = Instantiate(new GameObject(), transform.position, Quaternion.identity);
                _cubeHolder.name = "Cubes " + _generatedPatterns.ToString();
                _cubeHolders.Add(_cubeHolder);
                DestroyImmediate(GameObject.Find("New Game Object"));
            }
            else
            {
                Instantiate(new GameObject("Cubes " + _generatedPatterns), transform);
                DestroyImmediate(GameObject.Find("Cubes " + _generatedPatterns.ToString()));
            }

            _tempPositions.Add(_startPos = transform.position);

            for (int i = 0; i < cubesPerPattern; i++)
            {
                //Calculates _riseBy by evaluating the selected pattern curve
                _riseBy = (cubePatterns[whichPattern].Evaluate((float)i / (float)cubesPerPattern) - cubePatterns[whichPattern].Evaluate((float)(i - 1) / (float)cubesPerPattern)) * heightMultiplier;

                //Choosing material
                if (_tempMaterialIndex + 1 < materials.Length)
                    _tempMaterialIndex++;
                else
                    _tempMaterialIndex = 0;

                //Spawning the cube
                if (_tempGenerateAtEndVariable)
                    _tempCube = Instantiate(cube, _cubeHolder.transform);
                else
                    _tempCube = Instantiate(cube, transform.GetChild(_generatedPatterns).transform);
                //Creating the cube and changing its position
                _tempCube.GetComponent<DCMeshGenerator>().Activate(generateOnX, add3DMeshCollider, _riseBy, cubeLength, materials[_tempMaterialIndex]);
                _tempCube.transform.localPosition = new Vector3(transform.position.x - _startPos.x, transform.position.y - _startPos.y, transform.position.z - _startPos.z);

                //Moving this gameobject forward or right
                if (generateOnX)
                    transform.position = new Vector3(transform.position.x + cubeLength, transform.position.y + _riseBy, transform.position.z);
                else
                    transform.position = new Vector3(transform.position.x, transform.position.y + _riseBy, transform.position.z + cubeLength);
            }
            if (!_tempGenerateAtEndVariable)
                transform.GetChild(_generatedPatterns).position = _cubesStartPos;

            _generatedPatterns++;
        }

        if (resetLevel)     //If Reset Level "button" is clicked in the Inspector
        {
            resetLevel = false;
            transform.position = _cubesStartPos;
            ResetLevel();
        }

        if (deleteLastSpawned)     //If Delete Last Spawned "button" is clicked in the Inspector
        {
            deleteLastSpawned = false;
            if (_generatedPatterns > 0)
            {
                transform.position = _tempPositions[_generatedPatterns - 1];
                DeleteLastSpawned();
            }
            else
                Debug.LogWarning("There are no more cubes to remove!");
        }
    }

    void DeleteLastSpawned()
    {
        if (_tempGenerateAtEndVariable)
        {
            DestroyImmediate(_cubeHolders[_generatedPatterns - 1]);
            _cubeHolders.RemoveAt(_generatedPatterns - 1);
        }
        else
            DestroyImmediate(transform.GetChild(_generatedPatterns - 1).gameObject);

        _tempPositions.RemoveAt(_generatedPatterns - 1);
        _generatedPatterns--;
    }

    void ResetLevel()
    {
        _generatedPatterns = 0;

        if (_tempGenerateAtEndVariable)
        {
            int cubeHoldersCount = _cubeHolders.Count;
            for (int i = 0; i < cubeHoldersCount; i++)        //Loops through the _cubeHolders
                DestroyImmediate(_cubeHolders[i]);       //And destroys the object attached to them
        }

        else
        {
            int childs = transform.childCount;
            for (int i = 0; i < childs; i++)        //Loops through the children of the gameobject
                DestroyImmediate(transform.GetChild(0).gameObject);     //And destroys them
        }

        //Clears the lists
        _cubeHolders.Clear();
        _tempPositions.Clear();
    }
}
