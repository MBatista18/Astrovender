[System.Serializable]
public class GameData
{
    public int day;
    public int resources;

    public GameData(GameManager gameManager)
    {
        day = gameManager.CurrentDay;
        resources = gameManager.Resources;
    }
}
