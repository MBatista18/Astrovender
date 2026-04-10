using UnityEngine;

public class LockedDoorKey : MonoBehaviour
{
    ObjectID objectID;

    private void Awake()
    {
        objectID = GetComponent<ObjectID>();
    }

    private void Start()
    {
      /*  if (GameManager.Instance.currentdataObj.saveCOLGameWorld.Contains(objectID.GetID()))
        {
            Destroy(gameObject);
        }*/
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            GameManager.Instance.currentdataObj.saveCOLGameWorld.Add(objectID.GetID());
            GameManager.Instance.currentdataObj.keys++;

            AssetCall.instance.playerSM.CollectKey();
            Destroy(gameObject);
        }
    }
}
