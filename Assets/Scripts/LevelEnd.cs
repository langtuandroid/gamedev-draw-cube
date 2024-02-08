using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Line"))       //If gameobject collides with "Line"
        {
            //Then the level is cleared
            GameManager.Instance.ClearedPanelActivation();
            Move.Instance.Stop();
            this.enabled = false;
        }
    }
}
