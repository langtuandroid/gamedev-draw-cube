using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera2Follow : MonoBehaviour
{
    public float smoothTime = 0.5f;

    private Transform player;
    private Vector3 velocity;

    [HideInInspector]
    public Vector3 offset;
    [HideInInspector]
    public static Vector3 playerPos;

    public static CameraFollow Instance;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        offset = transform.position - player.position;
    }

    void LateUpdate()
    {
        //Smoothly follows the player with the offset
        transform.position = Vector3.SmoothDamp(transform.position, player.position + offset, ref velocity, smoothTime);

        playerPos = player.position + offset;
    }
}
