﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public LayerMask walkables;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (walkables == (walkables | (1 << collision.gameObject.layer)))
        {
            Move.Instance.IsGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Move.Instance.IsGrounded = false;       
    }
}
