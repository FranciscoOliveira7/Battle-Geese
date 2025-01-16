using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource soundObject;

    private void Awake()
    {
        if (instance == null) instance = this;
    }

    public void PlayClip(AudioClip clip, Transform soundTransform, float volume)
    {
        AudioSource audioSource = Instantiate(soundObject, soundTransform.position, Quaternion.identity);

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLenght = clip.length;

        Destroy(audioSource.gameObject, clipLenght);
    }

    public void PlayClipPitched(AudioClip clip, Transform soundTransform, float volume, float pitch)
    {
        AudioSource audioSource = Instantiate(soundObject, soundTransform.position, Quaternion.identity);

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.Play();

        float clipLenght = clip.length;

        Destroy(audioSource.gameObject, clipLenght);
    }
    
    public AudioSource PlayClipPitchedLoop(AudioClip clip, Transform soundTransform, float volume, float pitch)
    {
        AudioSource audioSource = Instantiate(soundObject, soundTransform.position, Quaternion.identity);

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.pitch = pitch;
        audioSource.loop = true;
        audioSource.Play();

        return audioSource;
    }

    public void PlayRandomClip(AudioClip[] clips, Transform soundTransform, float volume)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];

        AudioSource audioSource = Instantiate(soundObject, soundTransform.position, Quaternion.identity);

        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLenght = clip.length;

        Destroy(audioSource.gameObject, clipLenght);
    }
    public void PLayWithStartEnd(AudioClip clip, Transform soundTransform, float startTime, float duration, float Volume)
    {


        AudioSource audioSource = Instantiate(soundObject, soundTransform.position, Quaternion.identity);
        audioSource.clip = clip;
        audioSource.time = startTime;
        audioSource.volume = Volume;
        audioSource.Play();


        Destroy(audioSource.gameObject, duration);
    }

    public void PlayRandomClipWithRandomPitch(AudioClip[] clips, Transform soundTransform, float volume, float minPitch, float maxPitch)
    {
        AudioClip clip = clips[Random.Range(0, clips.Length)];

        AudioSource audioSource = Instantiate(soundObject, soundTransform.position, Quaternion.identity);

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLenght = clip.length;

        Destroy(audioSource.gameObject, clipLenght);
    }

    public void PlayClipWithRandomPitch(AudioClip clip, Transform soundTransform, float volume, float minPitch, float maxPitch)
    {
        AudioSource audioSource = Instantiate(soundObject, soundTransform.position, Quaternion.identity);

        audioSource.pitch = Random.Range(minPitch, maxPitch);
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.Play();

        float clipLenght = clip.length;

        Destroy(audioSource.gameObject, clipLenght);
    }
}
