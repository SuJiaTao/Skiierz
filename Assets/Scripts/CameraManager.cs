using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    Transform camTarget;
    Transform player;
    public float cameraHeight = 10.0f;
    public float cameraDistance = 2.4f;
    public float dampening = 1f;
    public Vector3 rotation;
    public float tiltMagnitude = 0;
    public float maxTiltAmount = 10;
    public float tiltDampening = 3;

    float currentTilt = 0f;

    private void Start()
    {
        camTarget = GameObject.FindWithTag("CamTrackingTarget").transform;
        player = GameObject.FindWithTag("Player").transform;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = Vector3.Lerp(
            transform.position,
            camTarget.position + (Quaternion.Euler(rotation) * camTarget.TransformDirection(new Vector3(0, cameraHeight, -cameraDistance))),
            Time.deltaTime / dampening);

        transform.LookAt(camTarget);

        float tiltAmount = player.GetComponent<Rigidbody>().angularVelocity.z * tiltMagnitude;

        currentTilt = Mathf.Lerp(currentTilt, tiltAmount, Time.deltaTime / tiltDampening);
        transform.rotation *= Quaternion.AngleAxis(Mathf.Clamp(currentTilt, -maxTiltAmount, maxTiltAmount), Vector3.forward);
    }
}