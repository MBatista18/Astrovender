using TMPro;
using UnityEngine;

public class SubgameCommunicator : MonoBehaviour
{
    public int oxygenTick = 10;
    public int shieldTick = 5;
    public int bombTick = 1;
    public int ammoTick = 1;

    int oxygenVal;
    public int GetOxygenVal() { return oxygenVal; }
    int shieldVal;
    public int GetShieldVal() { return shieldVal; }
    int bombVal;
    public int GetBombVal() { return bombVal; }
    int ammoVal;
    public int GetAmmoVal() { return ammoVal; }
    int coinsVal;
    public int GetCoinsVal() { return coinsVal; }

    private void Start()
    {
        oxygenVal = PlayerManager.min_OxygenValue;
        shieldVal = PlayerManager.min_ShieldValue;
        bombVal = PlayerManager.min_BombValue;
        ammoVal = PlayerManager.min_AmmoValue;
        coinsVal = GameManager.Instance.collectedCoins;
    }

    public void OnLineCompletion(MatchResult matchResults)
    {
        /* if (nodeType == NodeType.Bombs && GameManager.Instance.currentdataObj.hasBombs) { nodeType = NodeType.Coins; }
         if (nodeType == NodeType.Ammo && GameManager.Instance.currentdataObj.hasGun) { nodeType = NodeType.Coins; }
         if (nodeType == NodeType.Shield && GameManager.Instance.currentdataObj.hasShield) { nodeType = NodeType.Coins; }*/
        var nodeTypes = matchResults.ClearedCounts.Keys;
        foreach (var nodeType in nodeTypes)
        {
            int integerVal = matchResults.ClearedCounts[nodeType];
            switch (nodeType)
            {
                case NodeType.Oxygen:
                  //  Debug.Log("Oxygen tick = " + oxygenTick + " ; integer val = " + integerVal);
                   // Debug.Log("Oxygen update = " + (oxygenTick * integerVal));
                    oxygenVal = Mathf.RoundToInt(Mathf.Clamp(oxygenVal + (oxygenTick * integerVal), PlayerManager.min_OxygenValue, PlayerManager.GetMaxOxygenLevel()));
                    break;
                case NodeType.Bombs:
                 //   Debug.Log("Bombs update = " + (bombTick * integerVal));
                    bombVal = Mathf.RoundToInt(Mathf.Clamp(bombVal + (bombTick * integerVal), PlayerManager.min_BombValue, PlayerManager.GetMaxBombCount()));
                    break;
                case NodeType.Ammo:
                 //   Debug.Log("Ammo update = " + (ammoTick * integerVal));
                    ammoVal = Mathf.RoundToInt(Mathf.Clamp(ammoVal + (ammoTick * integerVal), PlayerManager.min_AmmoValue, PlayerManager.GetMaxAmmoCount()));
                    break;
                case NodeType.Shield:
                   // Debug.Log("Shield update = " + (shieldTick * integerVal));
                    shieldVal = Mathf.RoundToInt(Mathf.Clamp(shieldVal + (shieldTick * integerVal), PlayerManager.min_ShieldValue, PlayerManager.GetMaxShieldHealth()));
                    break;
                case NodeType.Coins:
                    //Debug.Log("Coins update = " + (integerVal));
                    GameManager.Instance.ModifyDataCoinCountBy(integerVal);
                    break;
                case NodeType.Junk:
                   // Debug.Log("Junk update");
                    break;
            }
        }
    }

    public void OnEnd()
    {
        PlayerManager.currentOxygenLevel = oxygenVal;
        PlayerManager.ammoCount = ammoVal;
        PlayerManager.bombCount = bombVal;
        PlayerManager.SetCurrentShieldHealth(shieldVal);
    }
}
