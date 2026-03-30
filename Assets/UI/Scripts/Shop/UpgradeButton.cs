using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeButton : MonoBehaviour
{
    PurchaseButton purchaseButton;
    Button button;

    TextMeshProUGUI buttonText;

    private void Awake()
    {
        buttonText = GetComponentInChildren<TextMeshProUGUI>();
        purchaseButton = GetComponent<PurchaseButton>();
        button = GetComponent<Button>();

        buttonText = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (purchaseButton.GetShopSO() == null)
        {
            button.interactable = false; 
            return; 
        }

        bool isActive = true;

        string upgradeText = "";

        switch (((Upgrades_ShopSO) purchaseButton.GetShopSO()).thisUpgradeType)
        {
            case Upgrades_ShopSO.Type.oxygenMeter:

                if (GameManager.Instance.currentdataObj.oxygenLevel >= GameManager.Instance.currentdataObj.MAX_OXYGENLEVEL)
                {
                    isActive = false;
                    upgradeText = "Max Oxygen Capacity";
                }
                else
                {
                    upgradeText = "Upgrade to Oxygen Capacity Lvl " + (GameManager.Instance.currentdataObj.oxygenLevel + 1);
                }
                break;
            case Upgrades_ShopSO.Type.bombCount:
                if (GameManager.Instance.currentdataObj.bombLevel >= GameManager.Instance.currentdataObj.MAX_BOMBLEVEL)
                {
                    isActive = false;
                    upgradeText = "Max Bomb Capacity";
                }
                else
                {
                    upgradeText = "Upgrade to Bomb Capacity Lvl " + (GameManager.Instance.currentdataObj.bombLevel + 1);
                }
                break;
            case Upgrades_ShopSO.Type.ammoCount:
                if (GameManager.Instance.currentdataObj.gunLevel >= GameManager.Instance.currentdataObj.MAX_GUNLEVEL)
                {
                    isActive = false;
                    upgradeText = "Max Gun Capacity";
                }
                else
                {
                    upgradeText = "Upgrade to Gun Capacity Lvl " + (GameManager.Instance.currentdataObj.gunLevel + 1);
                }
                break;
            case Upgrades_ShopSO.Type.shieldMeter:
                if (GameManager.Instance.currentdataObj.shieldLevel >= GameManager.Instance.currentdataObj.MAX_SHIELDLEVEL)
                {
                    isActive = false;
                    upgradeText = "Max Shield Capacity";
                }
                else
                {
                    upgradeText = "Upgrade to Shield Capacity Lvl " + (GameManager.Instance.currentdataObj.shieldLevel + 1);
                }
                break;
            case Upgrades_ShopSO.Type.enemyDrops:
                if (GameManager.Instance.currentdataObj.enemyDropLevel >= GameManager.Instance.currentdataObj.MAX_ENEMYDROPLEVEL)
                {
                    isActive = false;
                    upgradeText = "Max Level Enemy Drops";
                }
                else
                {
                    upgradeText = "Upgrade to Enemy Drops Lvl " + (GameManager.Instance.currentdataObj.enemyDropLevel + 1);
                }
                break;
            case Upgrades_ShopSO.Type.gridExpansion:
                if (GameManager.Instance.currentdataObj.subgame_gridExpansionLevel >= GameManager.Instance.currentdataObj.MAX_SUBGAME_GRIDEXPANSIONLEVEL)
                {
                    isActive = false;
                    upgradeText = "Max Replicator Grid Size";
                }
                else
                {
                    upgradeText = "Upgrade Replicator Grid to Lvl " + (GameManager.Instance.currentdataObj.bombLevel + 1);
                }
                break;
            case Upgrades_ShopSO.Type.subgameTurns:
                if (GameManager.Instance.currentdataObj.subgame_turnLevel >= GameManager.Instance.currentdataObj.MAX_SUBGAME_TURNLEVEL)
                {
                    isActive = false;
                    upgradeText = "Max Replicator Turn Count";
                }
                else
                {
                    upgradeText = "Upgrade Replicator Turn Count to Lvl " + (GameManager.Instance.currentdataObj.bombLevel + 1);
                }
                break;
        }

        upgradeText = isActive ? upgradeText +
                    " - " + purchaseButton.GetShopSO().GetCost().coins + " Coins, " + purchaseButton.GetShopSO().GetCost().gems + " Gems" : upgradeText;

        buttonText.text = upgradeText;

        button.interactable = isActive;
    }
}
