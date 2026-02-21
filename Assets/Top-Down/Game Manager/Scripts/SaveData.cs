[System.Serializable]
public class SaveData
{
    public GameData gameData;

    public SaveData()
    {
        gameData = new GameData(GameManager.Instance);
    }
}
