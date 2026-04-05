using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioCall : MonoBehaviour
{
    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [SerializeField] AudioKey[] audioKeys;
    
    public void CallAudioClip(string key)
    {
        for (int i = 0; i < audioKeys.Length; i++)
        {
            if (audioKeys[i].key.Equals(key))
            {
                audioSource.PlayOneShot(audioKeys[i].audioClip);
                return;
            }
        }
    }
}

[System.Serializable]
public struct AudioKey
{
    public AudioClip audioClip;
    public string key;
}
