using UnityEngine;

public class BossMusicPlayer : MonoBehaviour
{
    [SerializeField] AudioClip intro;
    [SerializeField] AudioClip loop;

    AudioSource audioSource;

    AudioClip prevAudio;

    public void StartBossTheme()
    {
        audioSource = GetComponent<AudioSource>();

        if (audioSource == null) { return; }

        prevAudio = audioSource.clip;

        audioSource?.Stop();
        audioSource.clip = intro;
        audioSource?.Play();

        timer = intro.length;

        isPlaying = true;
    }

    public void EndBossTheme()
    {
        isPlaying = false;
        isPlayingLoop = false;

        if (audioSource == null) { return; }

        audioSource?.Stop();
        audioSource.clip = prevAudio;
        audioSource?.Play();
    }

    bool isPlaying;
    bool isPlayingLoop;
    float timer;

    private void Update()
    {
        if (!isPlaying) { return; }

        if (isPlayingLoop) { return; }

        timer -= Time.deltaTime;

        if (timer <= 0)
        {
            isPlayingLoop = true;

            if (audioSource == null) { return; }

            audioSource?.Stop();
            audioSource.clip = loop;
            audioSource?.Play();
        }
    }
}
