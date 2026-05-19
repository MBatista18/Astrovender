using UnityEngine;

public class TriggerMusic : MonoBehaviour
{
    MusicPlayerOverworld mpOverworld;

    private void Awake()
    {
        mpOverworld = FindAnyObjectByType<MusicPlayerOverworld>();
    }

    [SerializeField] MusicPlayerOverworld.MusicPlayer thisType;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            mpOverworld.CallMusicPlayer(thisType);
        }
    }
}
