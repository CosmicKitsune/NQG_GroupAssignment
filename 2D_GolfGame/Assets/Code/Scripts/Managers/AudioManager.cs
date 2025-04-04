using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("---- Audio Source ----")]
    [SerializeField] AudioSource sfxSource;

    [Header("---- Audio Clips ----")]
    public AudioClip hitBall;
    public AudioClip death;
    public AudioClip hitBlock;
    public AudioClip windingShot;
    public AudioClip bulletFire;
    
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}
