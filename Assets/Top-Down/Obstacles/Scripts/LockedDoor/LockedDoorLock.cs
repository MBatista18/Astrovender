using UnityEngine;

public class LockedDoorLock : MonoBehaviour
{
    [SerializeField] LockedDoor lockedDoor;
    LineRenderer lineRenderer;

    Animator animator;

    bool unlocked;


    ObjectID objectID;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();

        lockedDoor.AddLock();

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, lockedDoor.transform.position + Vector3.up);


        objectID = GetComponent<ObjectID>();
    }

    private void Start()
    {
       /* if (GameManager.Instance.currentdataObj.saveENVGameWorld.Contains(objectID.GetID()))
        {
            Debug.Log("saveENV contains " + objectID.GetID());
            Unlock();
        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (unlocked) { return; } // ensures player won't spend multiple keys by pressing up against the same lock multiple times

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (AssetCall.instance.playerSM.GetKeyCount() <= 0) { return; }

            AssetCall.instance.playerSM.UseKey();
            GameManager.Instance.currentdataObj.saveENVGameWorld.Add(objectID.GetID());
            Unlock();
        }
    }

    public void Unlock()
    {
        unlocked = true;

        Destroy(lineRenderer);
        lockedDoor.RemoveLock();

        animator.Play("LockUnlock");
    }
}
