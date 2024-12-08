using UnityEngine;

public static class AudioUtils
{
    /// <summary>
    /// Plays an SFXClip using the given AudioSource.
    /// </summary>
    /// <param name="audioSource">The AudioSource to use for playback.</param>
    /// <param name="sfxClip">The SFXClip to play.</param>
    public static void Play(this AudioSource audioSource, SFXClip sfxClip)
    {
        if (audioSource != null && sfxClip != null && sfxClip.clip != null)
        {
            audioSource.clip = sfxClip.clip;
            audioSource.volume = sfxClip.volume;
            audioSource.Play();
        }
    }
}
