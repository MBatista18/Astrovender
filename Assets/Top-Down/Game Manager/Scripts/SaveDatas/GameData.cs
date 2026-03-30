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

    public int oxygenLevel;
    public readonly int MAX_OXYGENLEVEL;
    public int shieldLevel;
    public readonly int MAX_SHIELDLEVEL;
    public int gunLevel;
    public readonly int MAX_GUNLEVEL;
    public int bombLevel;
    public readonly int MAX_BOMBLEVEL;
    public int enemyDropLevel;
    public readonly int MAX_ENEMYDROPLEVEL;
    public int subgame_gridExpansionLevel;
    public readonly int MAX_SUBGAME_GRIDEXPANSIONLEVEL;
    public int subgame_turnLevel;
    public readonly int MAX_SUBGAME_TURNLEVEL;

    public List<string> puchasedHats;
    public string wornHat;

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

        oxygenLevel = 0;
        shieldLevel = 0;
        bombLevel = 0;
        gunLevel = 0;
        enemyDropLevel = 0;
        subgame_gridExpansionLevel = 0;
        subgame_turnLevel = 0;

        wornHat = "";

        MAX_OXYGENLEVEL = 10;
        MAX_SHIELDLEVEL = 10;
        MAX_BOMBLEVEL = 10;
        MAX_GUNLEVEL = 10;
        MAX_ENEMYDROPLEVEL = 5;
        MAX_SUBGAME_GRIDEXPANSIONLEVEL = 5;
        MAX_SUBGAME_TURNLEVEL = 5;

        puchasedHats = new List<string>();

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
