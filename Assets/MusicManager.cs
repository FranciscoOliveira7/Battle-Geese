using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public AudioSource calmMusicSource;
    public AudioSource battleMusicSource;

    public AudioClip calmMusicClip;
    public AudioClip battleMusicClip;

    public float crossfadeDuration = 1.0f;

    public static MusicManager Instance { get; private set; }


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

    }
    void Start()
    {
        calmMusicSource.clip = calmMusicClip;
        battleMusicSource.clip = battleMusicClip;
        calmMusicSource.Play();
        battleMusicSource.Play();

        battleMusicSource.volume = 0;
        calmMusicSource.volume = 0;
    }

    public void StartBattleMusic()
    {
        StartCoroutine(CrossfadeMusic(true));
    }

    public void StopBattleMusic()
    {
        StartCoroutine(CrossfadeMusic(false));
    }

    private IEnumerator CrossfadeMusic(bool toBattle)
    {
        float time = 0;
        float calmStartVolume = calmMusicSource.volume;
        float battleStartVolume = battleMusicSource.volume;

        // Calculate the current playback position in the source track
        float currentPlaybackTime = toBattle ? calmMusicSource.time : battleMusicSource.time;

        // Calculate the corresponding playback position in the target track
        float targetPlaybackTime = (currentPlaybackTime / (toBattle ? calmMusicClip.length : battleMusicClip.length)) * (toBattle ? battleMusicClip.length : calmMusicClip.length);

        // Set the target track to the calculated playback position
        if (toBattle)
        {
            battleMusicSource.time = targetPlaybackTime;
        }
        else
        {
            calmMusicSource.time = targetPlaybackTime;
        }

        while (time < crossfadeDuration)
        {
            time += Time.deltaTime;
            float t = time / crossfadeDuration;

            calmMusicSource.volume = Mathf.Lerp(calmStartVolume, toBattle ? 0 : 1, t);
            battleMusicSource.volume = Mathf.Lerp(battleStartVolume, toBattle ? 1 : 0, t);

            yield return null;
        }

        calmMusicSource.volume = toBattle ? 0 : 1;
        battleMusicSource.volume = toBattle ? 1 : 0;
    }
}
