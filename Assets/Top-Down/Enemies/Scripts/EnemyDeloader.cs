using UnityEngine;
using System.Collections.Generic;

public class EnemyDeloader : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjects;

    [SerializeField] float border;

    private void LateUpdate()
    {
        Vector3 minBounds = Camera.main.ViewportToWorldPoint(new Vector3(0, 0)); // casts the value of the bottom right corner of the camera to a world point4
        Vector3 maxBounds = Camera.main.ViewportToWorldPoint(new Vector3(1, 1));

        for (int i = 0; i < gameObjects.Count; i++)
        {
            if (gameObjects[i] == null) { gameObjects.RemoveAt(i); i--; continue; }

            if (gameObjects[i].transform.position.x > maxBounds.x + border || gameObjects[i].transform.position.x < minBounds.x - border ||
                gameObjects[i].transform.position.y > maxBounds.y + border || gameObjects[i].transform.position.y < minBounds.y - border)  //compares the minBounds to the actual transform.position
            {
                if (gameObjects[i].activeInHierarchy) { gameObjects[i].SetActive(false); }
            }
            else
            {
                Debug.Log("Can load");
                if (!gameObjects[i].activeInHierarchy) { gameObjects[i].SetActive(true); }
            }
        }

        /*foreach (GameObject a in gameObjects)
        {
            if (a ==null) { gameObjects.Remove(a); continue; }

            if (a.transform.position.x > maxBounds.x + border || a.transform.position.x < minBounds.x - border ||
                a.transform.position.y > maxBounds.y + border || a.transform.position.y < minBounds.y - border)  //compares the minBounds to the actual transform.position
            {
                if (a.activeInHierarchy) { a.SetActive(false); }
            }
            else
            {
                Debug.Log("Can load");
                if (!a.activeInHierarchy) { a.SetActive(true); } 
            }
        }*/
    }
}
