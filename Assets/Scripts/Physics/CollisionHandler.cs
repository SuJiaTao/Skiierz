using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CollisionHandler : MonoBehaviour
{
    public Suspension bumperFwd;
    public Suspension bumperRear;
    public Rigidbody phys;
    public PlayerSound playerSound;
    public float dismountFrontVelocity;
    public float dismountSideVelocity;
    public float dismountBumperOffsetFactor;
    public bool hasDismounted = false;
    public float respawnTimeSeconds;
    public float dismountTimeElapsedSeconds;
    public Vector3 lastSpawnPoint;
    public Quaternion lastSpawnRotation;
    public Trail playerTrail;

    public Image fadeImage;

    private void Awake() {
        lastSpawnPoint = transform.position;
        lastSpawnRotation = transform.rotation;
    }

    private void OnCollisionEnter(Collision collision) {
        print("collider enter");
        print(phys.velocity.magnitude);
        print(collision.relativeVelocity.magnitude);
        HandleCollision(collision.relativeVelocity.magnitude, phys.velocity, dismountSideVelocity);
    }

    void FixedUpdate() {
        if (hasDismounted) {
            DismountedLogic();
        } else {
            NonDismountedLogic();
        }
    }

    void DismountedLogic() {
        if (dismountTimeElapsedSeconds > respawnTimeSeconds) {
            
            dismountTimeElapsedSeconds = 0.0f;

            // enable all player physics and controls
            phys.isKinematic = false;
            GetComponent<PlayerControl>().enabled = true;
            GetComponent<Collider>().enabled = true;
            GetComponent<SlopeAngling>().enabled = true;
            GetComponent<SnowFriction>().enabled = true;
            GetComponent<PlayerDownforce>().enabled = true;

            // deactivate ragdoll
            GetComponent<RagdollHandler>().ToggleRagdoll(Vector3.zero);

            // enable sound
            playerSound.ClearSounds();
            playerSound.enabled = true;

            // respawn at last position
            transform.position = lastSpawnPoint;
            transform.rotation = lastSpawnRotation;
            hasDismounted = false;

            // clear trail
            playerTrail.trailRenderer.Clear();
            return;
        }

        dismountTimeElapsedSeconds += Time.deltaTime;
    }

    void NonDismountedLogic() {
        if (!bumperFwd.isGrounded && !bumperRear.isGrounded) {
            return;
        }

        // even if the bumpers are being touched, they need to satisfy a certain offset
        // to be considered as a proper collision
        if (bumperFwd.absOffset <= bumperFwd.maxOffset * dismountBumperOffsetFactor &&
            bumperRear.absOffset <= bumperRear.maxOffset * dismountBumperOffsetFactor) {
            return;
        }

        Vector3 ourVelocity = phys.velocity;
        float velFwd = Vector3.Dot(ourVelocity, bumperFwd.transform.rotation * Vector3.down);
        float velRear = Vector3.Dot(ourVelocity, bumperRear.transform.rotation * Vector3.down);
        float velMax = Mathf.Max(velFwd, velRear);

        HandleCollision(velMax, ourVelocity, dismountFrontVelocity);
    }

    void HandleCollision(float collisionVelocity, Vector3 ourVelocity, float threshold)
    {
        // can't DOUBLE DISMOUNT
        if (hasDismounted) {
            return;
        }

        if (collisionVelocity >= threshold)
        {
            // disable all player physics and controls
            phys.isKinematic = true;
            GetComponent<PlayerControl>().enabled = false;
            GetComponent<Collider>().enabled = false;
            GetComponent<SlopeAngling>().enabled = false;
            GetComponent<SnowFriction>().enabled = false;
            GetComponent<PlayerDownforce>().enabled = false;

            // activate ragdoll
            GetComponent<RagdollHandler>().ToggleRagdoll(ourVelocity);

            // disable sound
            playerSound.ClearSounds();
            playerSound.enabled = false;

            //fade image
            fadeMyImage();

            hasDismounted = true;
        }
    }

    void fadeMyImage() {
        StartCoroutine(FadeCoroutine());
    }

    IEnumerator FadeCoroutine() {
        Color startColor = fadeImage.color;
        float elapsedTime = 0f;
        float fadeDuration = 5f;

        while (elapsedTime < fadeDuration) {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, alpha);
            yield return null;
        }

        fadeImage.color = new Color(startColor.r, startColor.g, startColor.b, 0);
    }
}
