using UnityEngine;

[CreateAssetMenu(fileName = "NewAudioLibrary", menuName = "Game Data/Audio Library")]
public class AudioLibrary : ScriptableObject
{
    [Header("Combat Sounds")]
    public AudioClip[] slashSounds;
    public AudioClip[] hitSounds;

    [Header("Movement Sounds")]
    public AudioClip[] footstepSounds;
    
}