using UnityEngine;

public class DCFinishController : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Line"))       //If gameobject collides with "Line"
        {
            DCGameManager.Instance.ShowWinPanel();
            DCPlayerController.Instance.StopPlayer();
            enabled = false;
        }
    }
}
