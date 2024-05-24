using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public GameObject soundSourcePrefab;

    public void PlaySound(AudioClip sound, Transform position, float volume)
    {
        AudioSource soundSource = Instantiate(soundSourcePrefab, position.position, Quaternion.identity).GetComponent<AudioSource>();
        soundSource.clip = sound;
        soundSource.volume = volume;
        soundSource.Play();

        Destroy(soundSource.gameObject, soundSource.clip.length);
    }
}
