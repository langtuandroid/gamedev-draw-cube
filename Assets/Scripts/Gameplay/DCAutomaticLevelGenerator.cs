using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    [ExecuteInEditMode]
    public class DCAutomaticLevelGenerator : MonoBehaviour
    {
        [Header("Level Generation")]
        [SerializeField] private bool _reasighnMaterials;
        [SerializeField] private bool _generateLevel;
        [SerializeField] private bool _resetLevel;

        [Header("Generation Properties")]
        [Space]
        [SerializeField] private bool _generateOnX = true;
        [SerializeField] private bool _add3DMeshCollider;

        [Header("Cube Properties")]
        [Space]
        [Tooltip("How many cubes do you want to spawn")]
        [FormerlySerializedAs("countOfCubes")] [SerializeField] private int _countOfCubes = 20;
        [Range(0.25f, 1.5f)]
        [SerializeField] private float _cubeLength = 1f;

        [Space]
        [SerializeField] private int _minCubesPerPattern = 30;
        [SerializeField] private int _maxCubesPerPattern = 80;

        [Space]
        [SerializeField] private float _cubeHeightMin = 3f;
        [SerializeField] private float _cubeHeightMax = 40f;

        [Space]
        [Tooltip("You can add new patterns here")]
        [FormerlySerializedAs("cubePatterns")] [SerializeField] private AnimationCurve[] _cubePatterns;
        [FormerlySerializedAs("materials")] [SerializeField] private Material[] _materialsForCubes;
        [FormerlySerializedAs("cube")] [SerializeField] private GameObject _cubePrefab;
        [FormerlySerializedAs("levenEnd")] [SerializeField] private GameObject _levelEndPrefab;

        private Vector3 _startPos;
        private float _riseByYAxis = 1f;
        private int _cubesPerPattern;
        private float _tempCubeHeight;
        private static int _tempMaterialIndex;
        private static int _spawnedCubesAmount;
        private int _patternIndex;

        void Update()
        {
            if (_reasighnMaterials)
            {
                ReasighnMaterials();
            }
            if (_generateLevel)     //If Generate Level "button" is clicked in the Inspector
            {
                _generateLevel = false;
                _startPos = transform.position;
                ResetLevel();       //First resets level

                do
                {
                    //Calculates the random preferencies
                    _cubesPerPattern = Random.Range(_minCubesPerPattern, _maxCubesPerPattern);
                    _patternIndex = Random.Range(0, _cubePatterns.Length);
                    _tempCubeHeight = Random.Range(_cubeHeightMin, _cubeHeightMax);

                    for (int i = 0; i < _cubesPerPattern; i++)
                    {
                        //Calculates riseBy by evaluating the randomly selected pattern curve
                        _riseByYAxis = (_cubePatterns[_patternIndex].Evaluate((float)i / (float)_cubesPerPattern) - _cubePatterns[_patternIndex].Evaluate((float)(i - 1) / (float)_cubesPerPattern)) * _tempCubeHeight;

                        //Choosing material
                        if (_tempMaterialIndex + 1 < _materialsForCubes.Length)
                            _tempMaterialIndex++;
                        else
                            _tempMaterialIndex = 0;

                        //Spawning the cube
                        GameObject tempCube = Instantiate(_cubePrefab, transform);
                        tempCube.GetComponent<DCMeshGenerator>().Activate(_generateOnX, _add3DMeshCollider, _riseByYAxis, _cubeLength, _materialsForCubes[_tempMaterialIndex]);
                        tempCube.transform.localPosition = new Vector3(transform.position.x - _startPos.x, transform.position.y - _startPos.y, transform.position.z - _startPos.z);

                        //Moving this gameobject forward or right
                        if (_generateOnX)
                            transform.position = new Vector3(transform.position.x + _cubeLength, transform.position.y + _riseByYAxis, transform.position.z);
                        else
                            transform.position = new Vector3(transform.position.x, transform.position.y + _riseByYAxis, transform.position.z + _cubeLength);

                        //Checks if spawned all of the cubes
                        _spawnedCubesAmount++;
                        if (_spawnedCubesAmount >= _countOfCubes)
                            break;
                    }
                } while (_spawnedCubesAmount < _countOfCubes);

                GameObject levelEnd = Instantiate(_levelEndPrefab, transform);
                levelEnd.transform.localPosition = new Vector3(transform.position.x - _startPos.x, transform.position.y - _startPos.y, transform.position.z - _startPos.z);
                _spawnedCubesAmount = 0;
                transform.position = _startPos;      //Resets position of the gameobject to the starting position
            }

            if (_resetLevel)     //If Reset Level "button" is clicked in the Inspector
            {
                _resetLevel = false;
                ResetLevel();
            }
        }

        void ResetLevel()
        {
            int childs = transform.childCount;
            for (int i = 0; i < childs; i++)        //Loops through the children of the gameobject
                DestroyImmediate(transform.GetChild(0).gameObject);     //And destroys them
        }

        void ReasighnMaterials()
        {
            int currentMaterialIndex = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                currentMaterialIndex = currentMaterialIndex >= _materialsForCubes.Length ? 0 : currentMaterialIndex;
                transform.GetChild(i).gameObject.GetComponent<MeshRenderer>().material = _materialsForCubes[currentMaterialIndex];
                currentMaterialIndex = currentMaterialIndex >= _materialsForCubes.Length ? 0 : currentMaterialIndex + 1;
            }
        }
    }
}
