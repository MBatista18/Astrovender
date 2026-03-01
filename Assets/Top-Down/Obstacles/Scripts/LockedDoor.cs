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

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (open) { return; }

        if (lockCount > 0) { return; }
        
        open = true;

        animator.Play("LockedDoorOpen");
    }
}
