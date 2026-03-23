using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public DataObj data = new DataObj("");
    
    public GameData(GameManager gameManager)
    {
        data = gameManager.currentdataObj;
    }
}

[System.Serializable]
public struct DataObj
{
    public string saveName; // this is useless, c# wouldn't let me compile without a parameter in the constructor, however

    public int day;
    public int coins;
    public int gems;

    public bool hasShield;
    public bool hasGun;
    public bool hasBombs;

    public List<string> saveObstaclesGameWorld;

    public Dictionary<string, DungeonDatObj> dungeons;

    public DataObj(string _saveName)
    {
        saveName = _saveName;

        day = 0;
        coins = 0;
        gems = 0;

        hasShield = false;
        hasGun = false;
        hasBombs = false;

        saveObstaclesGameWorld = new List<string>();

        dungeons = new Dictionary<string, DungeonDatObj>
        {
           {"Dungeon", new DungeonDatObj("Dungeon") },
           {"Rocky Dungeon", new DungeonDatObj("Rocky Dungeon") },
           {"Beach Dungeon", new DungeonDatObj("Beach Dungeon") }
        };
    }
}
[System.Serializable]
public struct DungeonDatObj
{
    string sceneName;  // this is useless, c# wouldn't let me compile without a parameter in the constructor, however

    public bool defeatedBoss;
    public List<string> saveObstacles;
    public bool foundMap;

    public DungeonDatObj(string _sceneName)
    {
        sceneName = _sceneName;
        defeatedBoss = false;
        saveObstacles = new List<string>();
        foundMap = false;
    }
}
