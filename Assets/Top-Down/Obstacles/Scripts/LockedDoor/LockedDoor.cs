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

    ObjectID objectID;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        objectID = GetComponent<ObjectID>();
    }

    private void Start()
    {
        /*if (GameManager.Instance.currentdataObj.saveENVGameWorld.Contains(objectID.GetID()))
        {
            Open();
        }*/
    }

    private void Update()
    {
        if (open) { return; }

        if (lockCount > 0) { return; }

        GameManager.Instance.currentdataObj.saveENVGameWorld.Add(objectID.GetID());

        Open();
    }

    void Open()
    {
        open = true;

        animator.Play("LockedDoorOpen");
    }
}
