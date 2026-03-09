using UnityEngine;
using UnityEngine.SceneManagement;

public class DungeonEntrance : MonoBehaviour
{
    [SerializeField] int dungeonSceneIndex;

    [SerializeField] bool isOutside;
    [SerializeField] Transform playerSpawn;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (isOutside)
            {
                PlayerManager.playerWorldSpawn = playerSpawn.position;

            }
            SceneManager.LoadScene(dungeonSceneIndex);
        }
    }
}
