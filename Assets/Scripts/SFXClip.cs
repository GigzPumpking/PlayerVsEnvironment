using UnityEngine;

[System.Serializable]
public class SFXClip
{
    public AudioClip clip;  // The audio clip to play
    [Range(0f, 1f)] public float volume = 1.0f; // The volume to play the clip at
}
