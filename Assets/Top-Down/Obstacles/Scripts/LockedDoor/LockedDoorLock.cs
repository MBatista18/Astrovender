using UnityEngine;

public class LockedDoorLock : MonoBehaviour
{
    [SerializeField] LockedDoor lockedDoor;
    LineRenderer lineRenderer;

    Animator animator;

    bool unlocked;


    ObjectID objectID;

    AudioSource audioSource;

    [SerializeField] LockedDoorKey.KeyColor thisLockColor;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

        lockedDoor.AddLock();

        lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, lockedDoor.transform.position + Vector3.up);


        objectID = GetComponent<ObjectID>();

        if (thisLockColor == LockedDoorKey.KeyColor.Red)
        {
            GetComponent<SpriteRenderer>().color = Color.red;
        }
        else if (thisLockColor == LockedDoorKey.KeyColor.Blue)
        {
            GetComponent<SpriteRenderer>().color = Color.blue;
        }
        else if (thisLockColor == LockedDoorKey.KeyColor.Green)
        {
            GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    private void Start()
    {
        if (GameManager.Instance.currentdataObj.saveENVGameWorld.Contains(objectID.GetID()))
        {
            Debug.Log("saveENV contains " + objectID.GetID());
            Unlock();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (unlocked) { return; } // ensures player won't spend multiple keys by pressing up against the same lock multiple times

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            DungeonDatObj a = GameManager.Instance.currentdataObj.dungeons[UnityEngine.SceneManagement.SceneManager.GetActiveScene().name];

            if (thisLockColor == LockedDoorKey.KeyColor.Red && !a.hasRedKey) { return; }
            if (thisLockColor == LockedDoorKey.KeyColor.Blue && !a.hasBlueKey) { return; }
            if (thisLockColor == LockedDoorKey.KeyColor.Green && !a.hasGreenKey) { return; }

            AssetCall.instance.playerSM.UseKey();
            GameManager.Instance.currentdataObj.saveENVGameWorld.Add(objectID.GetID());
            Unlock();
        }
    }

    public void Unlock()
    {
        DungeonDatObj dataObj;
        string currentSceneName = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;

        if (GameManager.Instance.currentdataObj.dungeons.TryGetValue(currentSceneName, out dataObj))
        {
            GameManager.Instance.currentdataObj.dungeons.Remove(currentSceneName);

            switch (thisLockColor)
            {
                case LockedDoorKey.KeyColor.Red:
                    dataObj.hasRedKey = true;
                    break;
                case LockedDoorKey.KeyColor.Green:
                    dataObj.hasGreenKey = true;
                    break;
                case LockedDoorKey.KeyColor.Blue:
                    dataObj.hasBlueKey = true;
                    break;
            }

            GameManager.Instance.currentdataObj.dungeons.Add(currentSceneName, dataObj);
        }

        audioSource.Play();
        unlocked = true;

        Destroy(lineRenderer);
        lockedDoor.RemoveLock();

        animator.Play("LockUnlock");
    }
}
