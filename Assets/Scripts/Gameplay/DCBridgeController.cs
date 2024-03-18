using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    [ExecuteAlways]
    public class DCBridgeController : MonoBehaviour
    {
        [FormerlySerializedAs("part")] [SerializeField] private GameObject _bridgePartPrefab;
        [FormerlySerializedAs("partMaterials")] [SerializeField] private Material[] _materialsForParts;
        [SerializeField] private int _amountOfParts = 10;
        [SerializeField] private float _distanceBetweenParts = 1f;
        [SerializeField] private float _startBridgeHeight = 0.5f;
        [SerializeField] private bool _isCreateNewBridge;

        private int lastChildIndex, tempMaterialIndex = 0;

        void Start()
        {
            CreateBridge();
        }

        void Update()
        {
            if (_isCreateNewBridge)
            {
                _isCreateNewBridge = false;
                CreateBridge();
            }
        }

        private void CreateBridge()
        {
            //Bridge needs 3 parts at least
            if (_amountOfParts < 3)
                _amountOfParts = 3;

            lastChildIndex = _amountOfParts - 1;
            tempMaterialIndex = 0;

            //Deleting parts
            for (int i = transform.childCount - 1; i >= 0; i--)
                DestroyImmediate(transform.GetChild(i).gameObject);

            //Spawning and spacing the parts
            for (int i = 0; i < _amountOfParts; i++)
            {
                GameObject tempPart = Instantiate(_bridgePartPrefab, transform);
                tempPart.transform.localPosition = new Vector2(i * _distanceBetweenParts, 0f);
                tempPart.GetComponent<MeshRenderer>().material = _materialsForParts[tempMaterialIndex];
                if (tempMaterialIndex + 1 >= _materialsForParts.Length)
                    tempMaterialIndex = 0;
                else
                    tempMaterialIndex++;
            }

            //Setting up the middle parts
            for (int i = 1; i < lastChildIndex; i++)
            {
                if (transform.GetChild(i).gameObject.GetComponent<DistanceJoint2D>() != null)
                    continue;

                transform.GetChild(i).gameObject.AddComponent<DistanceJoint2D>().connectedBody = transform.GetChild(i - 1).gameObject.GetComponent<Rigidbody2D>();
                transform.GetChild(i).gameObject.AddComponent<DistanceJoint2D>().connectedBody = transform.GetChild(i + 1).gameObject.GetComponent<Rigidbody2D>();
                transform.GetChild(i).gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            }

            //Setting up the first and last part
            transform.GetChild(0).localPosition = new Vector2(transform.GetChild(0).localPosition.x, _startBridgeHeight);
            transform.GetChild(lastChildIndex).localPosition = new Vector2(transform.GetChild(lastChildIndex).localPosition.x, _startBridgeHeight);
        }
    }
}
