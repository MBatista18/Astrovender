using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WearHatButton : PurchaseButton
{
    [SerializeField] TextMeshProUGUI text;
    [HideInInspector] public string buttonText;

    Button button;
    [SerializeField] Image buttonImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        //buttonImage = GetComponent<Image>();
    }

    private void Update()
    {
        bool buttonActive = true;

        if (!GameManager.Instance.currentdataObj.puchasedHats.Contains(((Cosmetic_ShopSO)base.GetShopSO()).GetThisHatID()))
        {
            text.text = buttonText;
            buttonImage.color = Color.green;
        }
        else
        {
            if (GameManager.Instance.currentdataObj.wornHat == ((Cosmetic_ShopSO)base.GetShopSO()).GetThisHatID())
            {
                text.text = "Stop Wearing Hat";
                buttonImage.color = Color.red;
            }
            else
            {
                text.text = "Wear Hat";
                buttonImage.color = Color.blue;
            }
        }

        button.interactable = buttonActive;
    }

    public override void OnSelect()
    {
        if (!GameManager.Instance.currentdataObj.puchasedHats.Contains(((Cosmetic_ShopSO)base.GetShopSO()).GetThisHatID()))
        {
            base.OnSelect();
        }
        else
        {
            if (GameManager.Instance.currentdataObj.wornHat == ((Cosmetic_ShopSO)base.GetShopSO()).GetThisHatID())
            {
                GameManager.Instance.currentdataObj.wornHat = "";
            }
            else
            {
                GameManager.Instance.currentdataObj.wornHat = ((Cosmetic_ShopSO)base.GetShopSO()).GetThisHatID();
            }

        }
    }
}
