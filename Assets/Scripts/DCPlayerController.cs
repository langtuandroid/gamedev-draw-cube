using UnityEngine;

public class DCPlayerController : MonoBehaviour
{
    public static DCPlayerController Instance;

    [SerializeField] private float _angularSpeed = 43f;
    [SerializeField] private float _maxVelocityX = 12f;
    [SerializeField] private float _maxVelocityY = 4f;
    [SerializeField] private float _forwardForce = 10f;

    private Rigidbody2D _thisRb;
    private bool canRotate = true;

    public bool IsGrounded { get; set; }

    private void Awake() => Instance = this;

    void Start()
    {
        _thisRb = GetComponent<Rigidbody2D>();
    }

    public void SetRotation(Vector2 pos1, Vector2 pos2)
    {
        canRotate = Vector2.Distance(pos1, pos2) > 1.5f;

        if (!canRotate)
        {
            _thisRb.angularVelocity = 0;
            transform.rotation = Quaternion.identity;
        }
    }

    public void StopPlayer()
    {
        canRotate = false;
        _thisRb.angularVelocity = 0;
        transform.rotation = Quaternion.identity;
    }

    void FixedUpdate()
    {
        if (canRotate)
        {
            _thisRb.angularVelocity = -_angularSpeed * 10;

            if (IsGrounded && _thisRb.velocity.x > 0f) _thisRb.velocity = new Vector2(_forwardForce, _thisRb.velocity.y + 0.001f);
            _thisRb.velocity = new Vector2(Mathf.Clamp(_thisRb.velocity.x, -_maxVelocityX, _maxVelocityX), Mathf.Clamp(_thisRb.velocity.y, -15f, _maxVelocityY));
        }
    }
}
