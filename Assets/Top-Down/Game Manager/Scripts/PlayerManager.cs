using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static Vector3 playerWorldSpawn = new Vector3(10,23);
    
    static int oxygenLevelMultiplier = 60;
    public static int GetMaxOxygenLevel() { return 150 + (oxygenLevelMultiplier * GameManager.Instance.currentdataObj.oxygenLevel); }
    public static int currentOxygenLevel;
    
    public static int bombCount;
    public static void ModifyBombCount(int val) { bombCount = Mathf.Clamp(bombCount + val, 0, GetMaxBombCount()); }
    public static int GetMaxBombCount() { return 5 + (bombCountMultiplier * GameManager.Instance.currentdataObj.bombLevel); }
    static int bombCountMultiplier = 3;

    public static int ammoCount;
    public static void ModifyAmmoCount(int val) { ammoCount = Mathf.Clamp(ammoCount + val, 0, GetMaxAmmoCount()); }
    public static int GetMaxAmmoCount() { return 10 + (ammoCountMultiplier * GameManager.Instance.currentdataObj.gunLevel); }
    static int ammoCountMultiplier = 5;

    static int currentShieldHealth;
    public static int GetCurrentShieldHealth() { return currentShieldHealth; }

    static int shieldHealthMultiplier = 20;
    public static int GetMaxShieldHealth() { return 40 + (shieldHealthMultiplier * GameManager.Instance.currentdataObj.shieldLevel); }

    public static void ModifyShieldHealth(int val) { currentShieldHealth = Mathf.Clamp(currentShieldHealth + val, 0, GetMaxShieldHealth()); }

    public static void ResetPlayerValues() 
    {
        currentOxygenLevel = GetMaxOxygenLevel();
        bombCount = GetMaxBombCount();
        ammoCount = GetMaxAmmoCount();
        currentShieldHealth = GetMaxShieldHealth();
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            AssetCall.instance.playerSM.transform.position = playerWorldSpawn;
            Debug.Log("is scene; " + playerWorldSpawn);
        }
    }
    public static bool ShieldCanDefend(Vector3 playerPos, Vector3 attackPos, AstrovenderStructs.facingDirection facingDirection)
    {
        Vector3 difference = playerPos - attackPos;

        float axisDiff = Mathf.Abs(Mathf.Abs(difference.x) - Mathf.Abs(difference.y));

        float chosenAxis = 0; // 0 = x, 1 = y, 2 = both

        if (axisDiff >= .1) { chosenAxis = Mathf.Abs(difference.x) > Mathf.Abs(difference.y) ? 0 : 1; } else { chosenAxis = 2; }

        bool canDefend = false;

        if (chosenAxis == 0 || chosenAxis == 2) // x check
        {
            switch (facingDirection)
            {
                case AstrovenderStructs.facingDirection.left:

                    if (difference.x >= 0) { Debug.Log("Counter attack coming left"); canDefend = true; }

                    break;
                case AstrovenderStructs.facingDirection.right:

                    if (difference.x <= 0) { Debug.Log("Counter attack coming right"); canDefend = true; }

                    break;
            }
        }
        else if (chosenAxis == 1 || chosenAxis == 2) // y check
        {
            switch (facingDirection)
            {
                case AstrovenderStructs.facingDirection.down:

                    if (difference.y >= 0) { Debug.Log("Counter attack coming from below"); canDefend = true; }

                    break;
                case AstrovenderStructs.facingDirection.up:

                    if (difference.y <= 0) { Debug.Log("Counter attack coming from above"); canDefend = true; }

                    break;
            }
        }

        return canDefend;
    }
}
