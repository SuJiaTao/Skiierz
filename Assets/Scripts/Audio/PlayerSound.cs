using System;
using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    private EventInstance snowKickSound;
    private EventInstance skiingSound;
    private EventInstance jumpSound;
    private EventInstance landSound;


    public float snowKickMinDuration = 0.5f;
    [SerializeField] private float snowKickElapsed = 0;

    private void Start()
    {
        snowKickSound = AudioManager.instance.CreateInstance(FMODEvents.instance.snowKick);
        skiingSound = AudioManager.instance.CreateInstance(FMODEvents.instance.skiing);
        jumpSound = AudioManager.instance.CreateInstance(FMODEvents.instance.jump);
        landSound = AudioManager.instance.CreateInstance(FMODEvents.instance.land);

        PLAYBACK_STATE skiingPlaybackState;
        skiingSound.getPlaybackState(out skiingPlaybackState);
        if (skiingPlaybackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            skiingSound.setParameterByName("RPM", 400.0f);
            skiingSound.setParameterByName("Load", -1.0f);
            skiingSound.start();
        }
    }

    void Update()
    {
        PLAYBACK_STATE playbackState;
        snowKickSound.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.PLAYING))
        {
            snowKickElapsed += Time.deltaTime;
            if (snowKickElapsed > snowKickMinDuration)
            {
                snowKickSound.stop(STOP_MODE.ALLOWFADEOUT);
            }
        }
    }

    public void PlayJumpSound(float intensity)
    {
        PLAYBACK_STATE playbackState;
        jumpSound.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            float speed = Mathf.Lerp(0.0f, 3.0f, intensity);
            jumpSound.setParameterByName("Speed", speed);
            jumpSound.start();
        }
    }
    public void PlayLandSound(float intensity)
    {
        PLAYBACK_STATE playbackState;
        landSound.getPlaybackState(out playbackState);
        if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
        {
            float speed = Mathf.Lerp(0.0f, 3.0f, intensity);
            landSound.setParameterByName("Speed", speed);
            landSound.start();
        }
    }

    public void ToggleSnowKickSound(bool toggle, float intensity = 0.0f)
    {
        PLAYBACK_STATE playbackState;
        snowKickSound.getPlaybackState(out playbackState);

        if (toggle)
        {
            snowKickElapsed = 0;
            // Play snow kick sound
            snowKickSound.setParameterByName("Speed", intensity * 6.0f);

            if (playbackState.Equals(PLAYBACK_STATE.STOPPED))
            {

                snowKickSound.start();
            }
        }
    }

    public void UpdateSkiingSoundIntensity(float intensity)
    //intensity from 0 to 1
    {
        float rpm = Mathf.Lerp(0, 1800, intensity);
        skiingSound.setParameterByName("RPM", rpm);

    }
    public void ClearSounds()
    {
        skiingSound.stop(STOP_MODE.ALLOWFADEOUT);
        snowKickSound.stop(STOP_MODE.ALLOWFADEOUT);
    }
}
