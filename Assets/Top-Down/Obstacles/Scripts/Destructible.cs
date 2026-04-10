using UnityEngine;
using System;

public class Destructible : MonoBehaviour
{
    [SerializeField] GameObject particle;
    ObjectID objectID;

    private void Awake()
    {
        objectID = GetComponent<ObjectID>();
    }

    //Destroys the game object when the function is called upon
    public void CallDestroy()
    {
        GameManager.Instance.currentdataObj.saveENVGameWorld.Add(objectID.GetID());

        Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Start()
    {
        /*if (GameManager.Instance.currentdataObj.saveENVGameWorld.Contains(objectID.GetID()))
        {
            gameObject.SetActive(false);
        }*/
    }
}
