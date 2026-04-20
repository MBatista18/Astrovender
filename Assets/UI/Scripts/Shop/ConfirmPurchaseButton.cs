using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ConfirmPurchaseButton : MonoBehaviour
{
    ShopManager shopManager;
    Button button;

    [SerializeField] TextMeshProUGUI text;

    private void Awake()
    {
        shopManager = FindFirstObjectByType<ShopManager>();
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        if (shopManager.GetCurrentShopSO() == null)
        {
            button.interactable = false;
            return;
        }

        bool isActive = true;

        int currentCoins = GameManager.Instance.currentdataObj.coins;
        int currentGems = GameManager.Instance.currentdataObj.gems;

        if (currentCoins < shopManager.GetCurrentShopSO().GetCost().coins || currentGems < shopManager.GetCurrentShopSO().GetCost().gems)
        {
            isActive = false;
        }

        button.interactable = isActive;

        text.text = isActive ? "BUY" : "Not enough funds";
        text.fontSize = isActive ? 60 : 45;
    }

    private void Update()
    {
    }
}
