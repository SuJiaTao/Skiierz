using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
/**
* Following https://www.youtube.com/watch?v=rcBHIOjZDpk
*/
public class AudioManager : MonoBehaviour
{
    private List<EventInstance> eventInstances = new List<EventInstance>();
    private List<StudioEventEmitter> emitters = new List<StudioEventEmitter>();
    public static AudioManager instance { get; private set; }

    [Header("Volume")]
    [Range(0f, 1f)]
    public float masterVolume = 1f;
    [Range(0f, 1f)]
    public float musicVolume = 1f;
    [Range(0f, 1f)]
    public float sfxVolume = 1f;
    [Range(0f, 1f)]
    public float ambientVolume = 1f;

    private Bus masterBus;
    private Bus musicBus;
    private Bus sfxBus;
    private Bus ambientBus;

    private EventInstance ambientSound;
    private EventInstance music;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than one AudioManager in the scene");
        }
        instance = this;

        masterBus = RuntimeManager.GetBus("bus:/");
        musicBus = RuntimeManager.GetBus("bus:/Music");
        sfxBus = RuntimeManager.GetBus("bus:/SFX");
        ambientBus = RuntimeManager.GetBus("bus:/Ambience");
    }

    void Start()
    {
        InitializeAmbientSound();
        InitializeMusic();
    }

    void Update()
    {
        masterBus.setVolume(masterVolume);
        musicBus.setVolume(musicVolume);
        sfxBus.setVolume(sfxVolume);
        ambientBus.setVolume(ambientVolume);
    }

    private void InitializeAmbientSound()
    {
        ambientSound = CreateInstance(FMODEvents.instance.ambience);
        ambientSound.setParameterByName("Wind", 0.4f);
        ambientSound.setParameterByName("Rain", 0f);
        ambientSound.setParameterByName("Cover", 0.2f);
        ambientSound.start();
    }

    private void InitializeMusic()
    {
        music = CreateInstance(FMODEvents.instance.music);
        music.start();
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public EventInstance CreateInstance(EventReference sound)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(sound);
        eventInstances.Add(eventInstance);
        return eventInstance;
    }

    public StudioEventEmitter InitializeEventEmitter(EventReference eventReference, GameObject gameObject)
    {
        StudioEventEmitter emitter = gameObject.AddComponent<StudioEventEmitter>();
        emitter.EventReference = eventReference;
        emitters.Add(emitter);
        return emitter;
    }

    private void OnDestroy()
    {
        foreach (EventInstance eventInstance in eventInstances)
        {
            eventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            eventInstance.release();
        }

        foreach (StudioEventEmitter emitter in emitters)
        {
            emitter.Stop();
        }
    }
}
