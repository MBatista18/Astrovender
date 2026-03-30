using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    [SerializeField] GameObject purchaseMenu;
    [SerializeField] GameObject upgradesMenu;
    [SerializeField] GameObject cosmeticsMenu;

    [Header("Money")]
    [SerializeField] TextMeshProUGUI coinsText;
    [SerializeField] TextMeshProUGUI gemsText;

    [Header("Purchase Menu")]
    [SerializeField] Image purchase_displayImage;
    [SerializeField] TextMeshProUGUI purchase_nameText;
    [SerializeField] TextMeshProUGUI purchase_descriptionText;
    [SerializeField] TextMeshProUGUI purchase_costText;

    [Header("Tabs")]
    [SerializeField] Image CosmeticTab;
    [SerializeField] Image UpgradeTab;
    [SerializeField] Color selected;
    [SerializeField] Color unselected;

    public void DisplayPurchaseable()
    {
        if (currentShopSO == null) { return; }

        purchase_displayImage.sprite = currentShopSO.Sprite;
        purchase_nameText.text = currentShopSO.Name;
        purchase_descriptionText.text = currentShopSO.Description;
        purchase_costText.text = "Costs " + currentShopSO.GetCost().coins + " Coins, " + currentShopSO.GetCost().gems + " Gems";
    }

    int previousMenu;

    ShopSO currentShopSO;
    public ShopSO GetCurrentShopSO() { return currentShopSO; }
    public void SetSelectedShopSO(ShopSO shopSO)
    {
        currentShopSO = shopSO;
    }
    public void PurchaseShopSO()
    {
        if (currentShopSO == null) { return; }

        currentShopSO.OnPurchaseFunction();
        currentShopSO = null;
    }

    private void Start()
    {
        SetMenu(0);
    }

    private void Update()
    {
        coinsText.text = "" + GameManager.Instance.currentdataObj.coins;
        gemsText.text = "" + GameManager.Instance.currentdataObj.gems;
    }

    public void SetMenu(int menu)
    {
        if (menu != 2) { previousMenu = menu; }

        upgradesMenu.SetActive(menu == 0 ? true : false);
        cosmeticsMenu.SetActive(menu == 1 ? true : false);
        purchaseMenu.SetActive(menu == 2 ? true : false);

        CosmeticTab.color = menu == 1 ? selected : unselected;
        UpgradeTab.color = menu == 0 ? selected : unselected;

        switch (menu)
        {
            case 2:
                DisplayPurchaseable();
                break;
        }
    }

    public void ExitShopMenu()
    {
        SetMenu(previousMenu);
    }
}
