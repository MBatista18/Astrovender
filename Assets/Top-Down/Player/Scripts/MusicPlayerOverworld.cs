using UnityEngine;

public class MusicPlayerOverworld : MonoBehaviour
{
    [SerializeField] AudioSource hills;
    [SerializeField] AudioSource beach;
    [SerializeField] AudioSource cliffs;
    [SerializeField] AudioSource volcano;

    float maxVolume = .25f;

    float speedChange = 2f;

    public enum MusicPlayer
    {
        hills,
        beach,
        cliffs,
        volcano,
        none
    }

    public MusicPlayer current = MusicPlayer.none;

    public void CallMusicPlayer(MusicPlayer musicPlayer)
    {
        if (current == MusicPlayer.none)
        {
            switch (musicPlayer)
            {
                case MusicPlayer.hills:
                    hills.volume = maxVolume;
                    break;
                case MusicPlayer.volcano:
                    volcano.volume = maxVolume;
                    break;
                case MusicPlayer.beach:
                    beach.volume = maxVolume;
                    break;
                case MusicPlayer.cliffs:
                    cliffs.volume = maxVolume;
                    break;
            }
        }

        current = musicPlayer;
    }

    private void Update()
    {
        float beachAudio = beach.volume;
        beachAudio += current == MusicPlayer.beach ? speedChange * Time.deltaTime : -speedChange * Time.deltaTime;
        beachAudio = Mathf.Clamp(beachAudio, 0, maxVolume);

        float hillsAudio = hills.volume;
        hillsAudio += current == MusicPlayer.hills ? speedChange * Time.deltaTime : -speedChange * Time.deltaTime;
        hillsAudio = Mathf.Clamp(hillsAudio, 0, maxVolume);

        float cliffsAudio = cliffs.volume;
        cliffsAudio += current == MusicPlayer.cliffs ? speedChange * Time.deltaTime : -speedChange * Time.deltaTime;
        cliffsAudio = Mathf.Clamp(cliffsAudio, 0, maxVolume);

        float volcanoAudio = volcano.volume;
        volcanoAudio += current == MusicPlayer.volcano ? speedChange * Time.deltaTime : -speedChange * Time.deltaTime;
        volcanoAudio = Mathf.Clamp(volcanoAudio, 0, maxVolume);

        hills.volume = hillsAudio;
        beach.volume = beachAudio;
        cliffs.volume = cliffsAudio;
        volcano.volume = volcanoAudio;
    }
}
