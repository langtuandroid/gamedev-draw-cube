using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DCGroundedCollision : MonoBehaviour
{
    public LayerMask walkables;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (walkables == (walkables | (1 << collision.gameObject.layer)))
        {
            DCPlayerController.Instance.IsGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        DCPlayerController.Instance.IsGrounded = false;       
    }
}
