[System.Serializable]
public class GameData
{
    public int day;
    public int coins;
    public int gems;

    public GameData(GameManager gameManager)
    {
        day = gameManager.CurrentDay;
        gems = gameManager.CurrentGems;
        coins = gameManager.CurrentCoins;
    }
}
