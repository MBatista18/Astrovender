using UnityEngine;

public class DisableOverworldArea : MonoBehaviour
{
    [SerializeField] GameObject[] gameObjects;

    private void Awake()
    {
        SetAll(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SetAll(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SetAll(false);
        }
    }

    void SetAll(bool a)
    {

        for (int i = 0; i < gameObjects.Length; i++)
        {
            gameObjects[i].SetActive(a);
        }
    }
}
