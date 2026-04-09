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
    public CameraEffectors cameraEffectors;
    public MapUIBehavior mapUIBehavior;
    public MapGetSprites mapGetSprites;

    public GameObject coin;
    public GameObject gem;
    public GameObject oxygen;
    public GameObject shield;
    public GameObject ammo;
    public GameObject bomb;

    public GameObject explosion;

    private void Awake()
    {
        Debug.Log("awake");

        coin = Resources.Load("Prefabs/Items/Coin") as GameObject;
        gem = Resources.Load("Prefabs/Items/Gem") as GameObject;
        oxygen = Resources.Load("Prefabs/Items/Oxygen") as GameObject;
        shield = Resources.Load("Prefabs/Items/ShieldEnergy") as GameObject;
        ammo = Resources.Load("Prefabs/Items/Ammo") as GameObject;
        bomb = Resources.Load("Prefabs/Items/Bomb") as GameObject;

        explosion = Resources.Load("Prefabs/Obstacles/ExplosionObj") as GameObject;

        playerSM = FindFirstObjectByType<PlayerStateMachine>();
        HUDText = FindFirstObjectByType<SetHUDText>();
        cameraEffectors = FindFirstObjectByType<CameraEffectors>();
        mapUIBehavior = FindFirstObjectByType<MapUIBehavior>();
        mapGetSprites = FindFirstObjectByType<MapGetSprites>();
    }
}
