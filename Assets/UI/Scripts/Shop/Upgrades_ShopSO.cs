using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeSO", menuName = "Scriptable Objects/Shop/UpgradeSO")]
public class Upgrades_ShopSO : ShopSO
{
    public Type thisUpgradeType;
    public enum Type
    {
        oxygenMeter,
        bombCount,
        ammoCount,
        shieldMeter,
        enemyDrops,
        gridExpansion,
        subgameTurns
    }

    [SerializeField] Cost[] tiersCost;
    public override Cost GetCost()
    {
        int referenceIndex = 0;
        int index;

        DataObj dataObj = GameManager.Instance.currentdataObj;

        switch (thisUpgradeType)
        {
            case Type.oxygenMeter:
                referenceIndex = dataObj.oxygenLevel;
                break;
            case Type.bombCount:
                referenceIndex = dataObj.bombLevel;
                break;
            case Type.ammoCount:
                referenceIndex = dataObj.gunLevel;
                break;
            case Type.shieldMeter:
                referenceIndex = dataObj.shieldLevel;
                break;
            case Type.enemyDrops:
                referenceIndex = dataObj.enemyDropLevel;
                break;
            case Type.gridExpansion:
                referenceIndex = dataObj.subgame_gridExpansionLevel;
                break;
            case Type.subgameTurns:
                referenceIndex = dataObj.subgame_turnLevel;
                break;
        }

        index = Mathf.Clamp(referenceIndex, 0, tiersCost.Length - 1);

        return tiersCost[index];
    }

    public override void OnPurchaseFunction()
    {
        base.OnPurchaseFunction();

        Debug.Log("Bought Upgrade");
        int val;

        switch (thisUpgradeType)
        {
            case Type.oxygenMeter:
                val = GameManager.Instance.currentdataObj.oxygenLevel;

                GameManager.Instance.currentdataObj.oxygenLevel = Mathf.Clamp(val + 1, 0, GameManager.Instance.currentdataObj.MAX_OXYGENLEVEL);
                break;
            case Type.bombCount:
                val = GameManager.Instance.currentdataObj.bombLevel;

                GameManager.Instance.currentdataObj.bombLevel = Mathf.Clamp(val + 1, 0, GameManager.Instance.currentdataObj.MAX_BOMBLEVEL); 
                break;
            case Type.ammoCount:
                val = GameManager.Instance.currentdataObj.gunLevel;

                GameManager.Instance.currentdataObj.gunLevel = Mathf.Clamp(val + 1, 0, GameManager.Instance.currentdataObj.MAX_GUNLEVEL);
                break;
            case Type.shieldMeter:
                val = GameManager.Instance.currentdataObj.shieldLevel;

                GameManager.Instance.currentdataObj.shieldLevel = Mathf.Clamp(val + 1, 0, GameManager.Instance.currentdataObj.MAX_SHIELDLEVEL);
                break;
            case Type.enemyDrops:
                val = GameManager.Instance.currentdataObj.enemyDropLevel;

                GameManager.Instance.currentdataObj.enemyDropLevel = Mathf.Clamp(val + 1, 0, GameManager.Instance.currentdataObj.MAX_ENEMYDROPLEVEL);
                break;
            case Type.gridExpansion:
                val = GameManager.Instance.currentdataObj.subgame_gridExpansionLevel;

                GameManager.Instance.currentdataObj.subgame_gridExpansionLevel = Mathf.Clamp(val + 1, 0, GameManager.Instance.currentdataObj.MAX_SUBGAME_GRIDEXPANSIONLEVEL);
                break;
            case Type.subgameTurns:
                val = GameManager.Instance.currentdataObj.subgame_turnLevel;

                GameManager.Instance.currentdataObj.subgame_turnLevel = Mathf.Clamp(val + 1, 0, GameManager.Instance.currentdataObj.MAX_SUBGAME_TURNLEVEL);
                break;
        }
    }
}
