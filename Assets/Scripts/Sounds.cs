using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioClip[] sounds;

    private AudioSource audioSource => GetComponent<AudioSource>();

    public void PlaySound(AudioClip audio, float volume)
    {
        audioSource.PlayOneShot(audio, volume);
    }
    public void PlayMusic(AudioClip musicClip, float volume)
    {
        audioSource.clip = musicClip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }
}
