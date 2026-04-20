using UnityEngine;

public class LockedDoor : MonoBehaviour
{
    int lockCount = 0;

    public void AddLock()
    {
        lockCount++;
    }

    public void RemoveLock()
    {
        lockCount--;
    }

    bool open; // maybe this should be able to be modified by the player's save file to be open if the player previously opened this door

    Animator animator;

    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
    }


    private void Update()
    {
        if (open) { return; }

        if (lockCount > 0) { return; }

        Open();
    }

    void Open()
    {
        audioSource.Play();
        open = true;

        animator.Play("LockedDoorOpen");
    }
}
