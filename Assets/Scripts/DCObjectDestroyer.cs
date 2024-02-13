using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCObjectDestroyer : MonoBehaviour {

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Destroy(other.gameObject);
        }
    }
}
