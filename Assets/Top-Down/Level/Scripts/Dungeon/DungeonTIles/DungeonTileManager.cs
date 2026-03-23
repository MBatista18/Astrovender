using UnityEngine;
using System.Collections;

public class DungeonTileManager : MonoBehaviour
{
    DungeonTile[,] dungeonTiles;
    public DungeonTile[,] GetDungeonTiles() { return dungeonTiles; }
    
    Vector2 topLeftTilePosition;
    Vector2 bottomRightTilePosition;
    public Vector2 GetTopLeftPosition() { return topLeftTilePosition; }
    public Vector2 GetBottomRightPosition() { return bottomRightTilePosition; }

    Vector2 tileSize = new Vector2(15, 10);

    private void Awake()
    {
        // find the corner edges

        DungeonTile[] allDungeonTiles = FindObjectsByType<DungeonTile>(FindObjectsSortMode.None);

        float lowestX = float.MaxValue;
        float highestX = float.MinValue;

        float lowestY = float.MaxValue;
        float highestY = float.MinValue;

        for (int i = 0; i < allDungeonTiles.Length; i++)
        {
            if (allDungeonTiles[i].transform.position.x < lowestX) { lowestX = allDungeonTiles[i].transform.position.x; }
            if (allDungeonTiles[i].transform.position.x > highestX) { highestX = allDungeonTiles[i].transform.position.x; }

            if (allDungeonTiles[i].transform.position.y < lowestY) { lowestY = allDungeonTiles[i].transform.position.y; }
            if (allDungeonTiles[i].transform.position.y > highestY) { highestY = allDungeonTiles[i].transform.position.y; }
        }

        topLeftTilePosition = new Vector2(lowestX, highestY);
        bottomRightTilePosition = new Vector2(highestX, lowestY);

        Debug.Log("Top Left: " + topLeftTilePosition);
        Debug.Log("Bottom Right: " + bottomRightTilePosition);

        // determines the length of the 2d array

        int lengthX = ValueX(topLeftTilePosition.x) + 1;
        // adds one, as, for instance the distance between 2 tiles would yield a value of 1, so the added 1 acts as a buffer
        int lengthY = ValueY(bottomRightTilePosition.y) + 1;
        dungeonTiles = new DungeonTile[lengthX, lengthY];

        // map tiles along grid

        int indexX = 0;
        int indexY = 0;

        for(int i = 0; i < allDungeonTiles.Length; i++)
        {
          //  Debug.Log("Tile " + indexX + ", " + indexY + " = " + allDungeonTiles[i].gameObject.name);

            indexX = (lengthX - 1) - ValueX(allDungeonTiles[i].gameObject.transform.position.x);
            indexY = (lengthY - 1) - ValueY(allDungeonTiles[i].gameObject.transform.position.y);

            dungeonTiles[indexX, indexY] = allDungeonTiles[i];
        }
    }

    int ValueX(float positionX)
    {
        return Mathf.RoundToInt((bottomRightTilePosition.x - positionX) / tileSize.x);
    }

    int ValueY(float positionY)
    {
        return Mathf.RoundToInt((topLeftTilePosition.y - positionY) / tileSize.y);
    }
}
