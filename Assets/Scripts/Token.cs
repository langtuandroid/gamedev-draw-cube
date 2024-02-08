using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    [SerializeField] private GameObject _tokenParticle;
    [SerializeField] private float _rotationSpeed = 200f;

    void Update()
    {
        transform.Rotate(Vector3.up, -_rotationSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Line"))
        {
            ScoreManager.AddToken();
            Destroy(Instantiate(_tokenParticle, transform.position, Quaternion.identity), 1.2f);
            Destroy(gameObject);
        }
    }
}
