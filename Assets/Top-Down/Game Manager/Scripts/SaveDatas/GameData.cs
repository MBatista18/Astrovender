[System.Serializable]
public class GameData
{
    public int day;
    public int coins;
    public int gems;

    public bool hasShield;
    public bool hasGun;
    public bool hasBombs;

    public bool deafeatedBombsBoss;
    public bool deafeatedGunsBoss;

    public GameData(GameManager gameManager)
    {
        day = gameManager.CurrentDay;
        gems = gameManager.CurrentGems;
        coins = gameManager.CurrentCoins;

        hasShield = gameManager.PermaCollectedShield;
        hasGun = gameManager.PermaCollectedGun;
        hasBombs = gameManager.PermaCollectedBombs;

        deafeatedBombsBoss = gameManager.PermaDefeatedBombsBoss;
        deafeatedGunsBoss = gameManager.PermaDefeatedGunsBoss;
    }
}
