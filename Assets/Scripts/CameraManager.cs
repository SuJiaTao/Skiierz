using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public float cameraHeight = 10.0f;
    public float cameraDistance = 2.4f;
    public float dampening = 1f;
    public Vector3 rotation;

    private void Start()
    {
        player = GameObject.FindWithTag("CamTrackingTarget").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            player.position + (Quaternion.Euler(rotation) * player.TransformDirection(new Vector3(0, cameraHeight, -cameraDistance))),
            dampening * Time.deltaTime);
        transform.LookAt(player);
    }
}
