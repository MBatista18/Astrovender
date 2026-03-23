using UnityEngine;
using UnityEngine.UI;

public class MapUIBehavior : MonoBehaviour
{
    DungeonTileManager tileManager;
    [SerializeField] Transform tileControlParent;
    [SerializeField] Transform tileScaleParent;
    [SerializeField] GameObject tileSprite;

    int tileIndexDistance = 100;

    private void Start()
    {
        tileManager = FindFirstObjectByType<DungeonTileManager>();

        for (int x = 0; x < tileManager.GetDungeonTiles().GetLength(0); x++)
        {
            for (int y = 0; y < tileManager.GetDungeonTiles().GetLength(1); y++)
            {
                if (tileManager.GetDungeonTiles()[x,y] == null) { continue; }

                var a = Instantiate(tileSprite, tileControlParent, false);
                a.transform.localPosition = (new Vector3(x, y) * tileIndexDistance);
                tileManager.GetDungeonTiles()[x, y].SetImage(a.GetComponent<Image>());
            }
        }

        tileControlParent.localPosition = (new Vector2((tileManager.GetTopLeftPosition().x - tileManager.GetBottomRightPosition().x) / 15,
            (tileManager.GetBottomRightPosition().y - tileManager.GetTopLeftPosition().y) / 10) / 2) * tileIndexDistance;

        Destroy(tileSprite);

        // scaling

        float scale = tileManager.GetDungeonTiles().GetLength(0) > tileManager.GetDungeonTiles().GetLength(1) ?
            tileManager.GetDungeonTiles().GetLength(0) : tileManager.GetDungeonTiles().GetLength(1);

        scale = (1 / scale) * 7;

        tileScaleParent.localScale = Vector3.one * scale;

        this.gameObject.SetActive(false);
    }
}
