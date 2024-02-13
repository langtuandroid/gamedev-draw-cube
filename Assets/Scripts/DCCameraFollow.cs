using UnityEngine;

public class DCCameraFollow : MonoBehaviour
{
    public static DCCameraFollow Instance;
    [SerializeField] private float smoothTime = 0.1f;
    [SerializeField] private bool isMainCamera = false;

    private Transform _playerTransform;
    private Vector3 _velocity;
    private Vector3 _offset;
    
    private void Awake()
    {
        if (isMainCamera) Instance = this;
    }

    void Start()
    {
        _playerTransform = DCPlayerController.Instance.transform;
        _offset = transform.position - _playerTransform.position;
    }
    
    void LateUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _playerTransform.position + _offset, ref _velocity, smoothTime);
    }

    public Vector3 GetOffset() => _offset;
}
