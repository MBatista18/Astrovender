using UnityEngine;

[CreateAssetMenu(fileName = "CosmeticSO", menuName = "Scriptable Objects/Shop/CosmeticSO")]
public class Cosmetic_ShopSO : ShopSO
{
    [SerializeField] Cost cost;
    public override Cost GetCost()
    {
        return cost;
    }

    public override void OnPurchaseFunction()
    {
        base.OnPurchaseFunction();

        Debug.Log("Bought Hat");

        GameManager.Instance.currentdataObj.puchasedHats.Add(GetThisHatID());
    }

    public string GetThisHatID()
    {
        return "Scriptable Objects/Shop/Cosmetics/" + name;
    }

    public Sprite backFacingHat;
    public Sprite leftFacingHat;
    public Sprite rightFacingHat;
}
