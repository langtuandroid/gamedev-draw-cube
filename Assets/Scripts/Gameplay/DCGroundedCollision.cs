using UnityEngine;
using UnityEngine.Serialization;

namespace Gameplay
{
    public class DCGroundedCollision : MonoBehaviour
    {
        [FormerlySerializedAs("walkables")] [SerializeField] private LayerMask _walkableLayers;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (_walkableLayers == (_walkableLayers | (1 << collision.gameObject.layer)))
            {
                DCPlayerController.Instance.IsOnGround = true;
            }
        }

        private void OnCollisionExit2D()
        {
            DCPlayerController.Instance.IsOnGround = false;       
        }
    }
}
