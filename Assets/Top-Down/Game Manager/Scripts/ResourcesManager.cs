using UnityEngine;

public class ResourcesManager : MonoBehaviour
{
    // this is a temporary solution for manually loading resources
    // please revise this with a more workable system
        // - Dirk

    public GameObject coin;
    public GameObject gem;

    public static ResourcesManager Instance;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of SaveManager exists
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
