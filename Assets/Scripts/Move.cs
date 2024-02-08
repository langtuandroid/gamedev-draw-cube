using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public static Move Instance;

    public float angularSpeed = 10f, maxVelocityX = 13f, maxVelocityY = 7f, forwardForce;

    private Rigidbody2D rb;
    private bool canRotate = true;

    public bool IsGrounded { get; set; }

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void CheckIfCanRotate(Vector2 pos1, Vector2 pos2)
    {
        canRotate = Vector2.Distance(pos1, pos2) > 1.5f;        //If the distance between the two given positions are bigger than x, then player can rotate

        if (!canRotate)
        {
            rb.angularVelocity = 0;                                 //Stops player
            transform.rotation = Quaternion.identity;
        }
    }

    void FixedUpdate()
    {
        if (canRotate)                                               //If player can rotate
        {
            rb.angularVelocity = -angularSpeed * 10;                 //Adds rotation

            if (IsGrounded)
            {
                if (rb.velocity.x > 0f)
                    rb.velocity = new Vector2(forwardForce, rb.velocity.y + 0.001f);        //Adds forward force
            }
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x, -maxVelocityX, maxVelocityX), Mathf.Clamp(rb.velocity.y, -15f, maxVelocityY));     //Clamps velocity
        }
    }

    public void Stop()
    {
        canRotate = false;
        rb.angularVelocity = 0;                                      //Stops player
        transform.rotation = Quaternion.identity;
    }
}
