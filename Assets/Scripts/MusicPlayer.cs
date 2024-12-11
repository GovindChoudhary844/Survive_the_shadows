using UnityEngine;

public class MusicPlayer : MonoBehaviour
{
    public AudioSource introSource, loopSource;

    private void Start()
    {
        introSource.Play();
        loopSource.PlayScheduled(AudioSettings.dspTime + introSource.clip.length);
    }
}
