using UnityEngine;
using UnityEngine.UI;

public class CosmeticButton : MonoBehaviour
{
    PurchaseButton purchaseButton;
    Button button;

    private void Awake()
    {
        purchaseButton = GetComponent<PurchaseButton>();
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (purchaseButton.GetShopSO() == null)
        {
            button.interactable = false;
            return;
        }

        bool isActive = true;

        if (GameManager.Instance.currentdataObj.puchasedHats.Contains(((Cosmetic_ShopSO) purchaseButton.GetShopSO()).GetThisHatID()))
        {
            isActive = false;
        }

        button.interactable = isActive;
    }
}
