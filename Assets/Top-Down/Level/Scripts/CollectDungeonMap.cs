using UnityEngine;
using UnityEngine.SceneManagement;

public class CollectDungeonMap : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return;
        }

        string scene = SceneManager.GetActiveScene().name;
        DungeonDatObj dungeonDatObj;

        if (GameManager.Instance.currentdataObj.dungeons.TryGetValue(scene, out dungeonDatObj))
        {
            dungeonDatObj.foundMap = true;
            GameManager.Instance.currentdataObj.dungeons.Remove(scene);
            GameManager.Instance.currentdataObj.dungeons.Add(scene, dungeonDatObj);
        }

        Destroy(gameObject);
    }
}
