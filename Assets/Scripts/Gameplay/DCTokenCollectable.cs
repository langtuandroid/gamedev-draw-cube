using Managers;
using UnityEngine;

namespace Gameplay
{
    public class DCTokenCollectable : MonoBehaviour
    {
        [SerializeField] private GameObject _tokenParticleReference;
        [SerializeField] private float _tokenRotationSpeed = 200f;

        void Update()
        {
            transform.Rotate(Vector3.up, -_tokenRotationSpeed * Time.deltaTime);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player") || collision.CompareTag("Line"))
            {
                DCScoreManager.TokenAdd();
                Destroy(Instantiate(_tokenParticleReference, transform.position, Quaternion.identity), 1.2f);
                Destroy(gameObject);
            }
        }
    }
}
