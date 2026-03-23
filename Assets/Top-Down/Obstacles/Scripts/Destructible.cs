using UnityEngine;
using System;

public class Destructible : MonoBehaviour
{
    [SerializeField] string uniqueID;
    public string GetUniqueID() { return uniqueID + "_" + gameObject.name; }

    [SerializeField] GameObject particle;

    //Destroys the game object when the function is called upon
    public void CallDestroy()
    {
        GameManager.Instance.currentdataObj.saveObstaclesGameWorld.Add(GetUniqueID());

        Instantiate(particle, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private void Start()
    {
        if (GameManager.Instance.currentdataObj.saveObstaclesGameWorld.Contains(GetUniqueID()))
        {
            gameObject.SetActive(false);
        }
    }
}
