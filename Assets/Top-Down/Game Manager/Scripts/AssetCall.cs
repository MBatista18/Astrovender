using UnityEngine;

public class AssetCall : MonoBehaviour
{
    // class designed to decouple logic between unlike objects in the editor, and avoid continued usage of expensive calls for specific objects

    // classes that may need to be called between various objects (e.g. enemies calling scripts of the player class) will use the reference to the player
            // in this class, as opposed to having their own reference to the player

    private static AssetCall _instance;

    public static AssetCall instance
    {
        get
        {
            if (_instance == null)
            {
                var a = new GameObject("AssetCall");
                _instance = a.AddComponent<AssetCall>();
            }

            return _instance;
        }
    }

    public PlayerStateMachine playerSM;
    public SetHUDText HUDText;

    public GameObject coin;
    public GameObject gem;

    private void Awake()
    {
        Debug.Log("awake");

        coin = Resources.Load("Prefabs/Items/Coin") as GameObject;
        gem = Resources.Load("Prefabs/Items/Gem") as GameObject;

        playerSM = FindFirstObjectByType<PlayerStateMachine>();
        HUDText = FindFirstObjectByType<SetHUDText>();
    }
}
