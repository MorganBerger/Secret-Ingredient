using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    public AudioSource source;
    public AudioLibrary library;


    public void PlaySlash()
    {
        if (library.slashSounds.Length == 0) return;

        source.pitch = Random.Range(0.85f, 1.15f);
        AudioClip randomSlash = library.slashSounds[Random.Range(0, library.slashSounds.Length)];
        
        source.PlayOneShot(randomSlash);
    }

    private int stepIndex = 0;
    public void PlayStep()
    {
        if (library.footstepSounds.Length == 0) return;

        AudioClip step = library.footstepSounds[stepIndex];
        stepIndex = (stepIndex + 1) % library.footstepSounds.Length;
        
        source.pitch = Random.Range(0.85f, 1.15f);
        source.PlayOneShot(step);
    }
}
