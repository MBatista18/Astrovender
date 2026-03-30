using UnityEngine;

public class PurchaseButton : MonoBehaviour
{
    [SerializeField] ShopSO currentShopSO;

    public void OverrideShopSO(ShopSO overrideSO)
    {
        currentShopSO = overrideSO;
    }

    public ShopSO GetShopSO() { return currentShopSO; }

    ShopManager shopManager;
    private void Start()
    {
        shopManager = FindFirstObjectByType<ShopManager>();
    }

    public virtual void OnSelect()
    {
        Debug.Log("Select");
        shopManager.SetSelectedShopSO(currentShopSO);
        shopManager.SetMenu(2);
    }
}
