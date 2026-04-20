using UnityEngine;
using TMPro;

public class SubgameCommunicator : MonoBehaviour
{
    public int oxygenTick = 20;
    public int shieldTick = 3;
    public int bombTick = 1;
    public int ammoTick = 1;

    public float oxygenBase = 100;

    int oxygenVal;
    int shieldVal;
    int bombVal;
    int ammoVal;
    int coinsVal;

    private void Start()
    {
        oxygenVal = PlayerManager.min_OxygenValue;
        shieldVal = PlayerManager.min_ShieldValue;
        bombVal = PlayerManager.min_BombValue;
        ammoVal = PlayerManager.min_AmmoValue;
        coinsVal = GameManager.Instance.collectedCoins;

        Debug.Log("Oxygen = " + PlayerManager.GetMaxOxygenLevel());
        Debug.Log("Shield = " + PlayerManager.GetMaxShieldHealth());
        Debug.Log("Bombs = " + PlayerManager.GetMaxBombCount());
        Debug.Log("Ammo = " + PlayerManager.GetMaxAmmoCount());
    }

    public void OnLineCompletion(NodeType nodeType, int integerVal)
    {
        Debug.Log("Nodey type = " + nodeType);

       /* if (nodeType == NodeType.Bombs && GameManager.Instance.currentdataObj.hasBombs) { nodeType = NodeType.Coins; }
        if (nodeType == NodeType.Ammo && GameManager.Instance.currentdataObj.hasGun) { nodeType = NodeType.Coins; }
        if (nodeType == NodeType.Shield && GameManager.Instance.currentdataObj.hasShield) { nodeType = NodeType.Coins; }*/

        switch (nodeType)
        {
            case NodeType.Oxygen:
                Debug.Log("Oxygen update");
                oxygenVal = Mathf.RoundToInt(Mathf.Clamp(oxygenVal + (oxygenTick * integerVal), PlayerManager.min_OxygenValue, PlayerManager.GetMaxOxygenLevel()));
                break;
            case NodeType.Bombs:
                Debug.Log("Bombs update");
                bombVal = Mathf.RoundToInt(Mathf.Clamp(bombVal + (bombTick * integerVal), PlayerManager.min_BombValue, PlayerManager.GetMaxBombCount()));
                break;
            case NodeType.Ammo:
                Debug.Log("Ammo update");
                ammoVal = Mathf.RoundToInt(Mathf.Clamp(ammoVal + (ammoTick * integerVal), PlayerManager.min_AmmoValue, PlayerManager.GetMaxAmmoCount()));
                break;
            case NodeType.Shield:
                Debug.Log("Shield update");
                shieldVal = Mathf.RoundToInt(Mathf.Clamp(shieldVal + (shieldTick * integerVal), PlayerManager.min_ShieldValue, PlayerManager.GetMaxShieldHealth()));
                break;
            case NodeType.Coins:
                Debug.Log("Coins update");
                GameManager.Instance.ModifyDataCoinCountBy(integerVal);
                break;
        }
    }

    [SerializeField] TextMeshProUGUI oxygenText;
    [SerializeField] TextMeshProUGUI shieldText;
    [SerializeField] TextMeshProUGUI bombText;
    [SerializeField] TextMeshProUGUI ammoText;
    [SerializeField] TextMeshProUGUI coinsText;

    private void Update()
    {
        oxygenText.text = "Oxygen: " + oxygenVal;
        shieldText.text = "Shield: " + shieldVal;
        ammoText.text = "Ammo: " + ammoVal;
        coinsText.text = "Coins: " + coinsVal;
        bombText.text = "Bombs: " + bombVal;
    }
}
