using UnityEngine;

public class LevelEnd : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Line"))       //If gameobject collides with "Line"
        {
            GameManager.Instance.ClearedPanelActivation();
            PlayerController.Instance.StopPlayer();
            enabled = false;
        }
    }
}
