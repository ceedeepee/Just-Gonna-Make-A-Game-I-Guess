using System.Collections;
using UnityEngine;

public class AudioLooper : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip[] audioClips;
    public float crossfadeTime = 1f;
    [Range(0f, 1f)]
    public float maxVolume = 1f;
    
    private void Start()
    {
        if (audioClips.Length > 0)
        {
            StartCoroutine(PlayRandomLoops());
        }
    }

    private IEnumerator PlayRandomLoops()
    {
        while (true)
        {
            int clipIndex = Random.Range(0, audioClips.Length);
            int loopCount = Random.Range(1, 6);
            
            audioSource.clip = audioClips[clipIndex];

            for (int i = 0; i < loopCount; i++)
            {
                yield return StartCoroutine(PlayClipWithFade(audioSource, crossfadeTime));
            }
        }
    }

    private IEnumerator PlayClipWithFade(AudioSource audioSource, float duration)
    {
        float startTime = Time.time;

        // Fade out
        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            audioSource.volume = maxVolume * (1f - t);
            yield return null;
        }

        audioSource.volume = 0f;

        // Start new clip and fade in
        audioSource.Play();

        startTime = Time.time;

        while (Time.time < startTime + duration)
        {
            float t = (Time.time - startTime) / duration;
            audioSource.volume = maxVolume * t;
            yield return null;
        }

        audioSource.volume = maxVolume;

        // Wait for clip to finish
        yield return new WaitForSeconds(audioSource.clip.length - duration);
    }
}