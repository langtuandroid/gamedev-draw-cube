using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Token : MonoBehaviour
{
    public GameObject tokenParticle;
    public float rotationSpeed = 200f;

    void Update()
    {
        transform.Rotate(Vector3.up, -rotationSpeed * Time.deltaTime);        //Rotates the gameObject on the Y axis    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") || collision.CompareTag("Line"))     //If token collides with player or line
        {
            //Increases tokencounter, spawns particle and destroys gameobject
            ScoreManager.Instance.IncrementToken();
            Destroy(Instantiate(tokenParticle, transform.position, Quaternion.identity), 1.2f);
            Destroy(gameObject);
        }
    }
}
