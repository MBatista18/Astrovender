using UnityEngine;

//[CreateAssetMenu(fileName = "ShopSO", menuName = "Scriptable Objects/BulletSO")]
public class ShopSO : ScriptableObject
{
    public string Name;
    public Sprite Sprite;
    public string Description;

    public virtual Cost GetCost() { return new Cost(0,0); }

    [System.Serializable]
    public struct Cost 
    {
        public int coins;
        public int gems;

        public Cost(int _coins, int _gems)
        {
            coins = _coins;
            gems = _gems;
        }
    }

    public virtual void OnPurchaseFunction()
    {
        Debug.Log("Extract Money");

        GameManager.Instance.ModifyDataCoinCountBy(-GetCost().coins);
        GameManager.Instance.ModifyDataGemCountBy(-GetCost().gems);
    }
}
