using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopCosmeticsManager : MonoBehaviour
{
    [SerializeField] ShopSO[] Cosmetics = new ShopSO[1];
    int index = 0;
    public void ChangeIndex(int value)
    {
        index += value;

        if (index < 0) { index = Cosmetics.Length -1; }
        if (index >= Cosmetics.Length) { index = 0; }

        DisplayCosmetic();
    }

    [Header("Cosmetics Menu")]
    [SerializeField] TextMeshProUGUI cosmetic_nameText;
    [SerializeField] TextMeshProUGUI cosmetic_descriptionText;
    [SerializeField] Image cosmetic_DisplayImage;

    [SerializeField] WearHatButton cosmetic_purchaseButton;

    private void Start()
    {
        DisplayCosmetic();
    }

    public void DisplayCosmetic()
    {
        if (Cosmetics[index] == null) { return; }

        cosmetic_purchaseButton.buttonText = "BUY - " + Cosmetics[index].GetCost().coins + " Coins, " + Cosmetics[index].GetCost().gems + " Gems";
        cosmetic_DisplayImage.sprite = Cosmetics[index].Sprite;
        cosmetic_nameText.text = Cosmetics[index].Name;
        cosmetic_descriptionText.text = Cosmetics[index].Description;

        cosmetic_purchaseButton.OverrideShopSO(Cosmetics[index]);
    }
}
