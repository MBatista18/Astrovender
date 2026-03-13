using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static Vector3 playerWorldSpawn = new Vector3(10,23);
    
    static int maxOxygenlevel = 1400;
    public static int currentOxygenLevel;
    
    public static int bombCount;
    static int maxBombCount = 20;

    public static int ammoCount;
    static int maxAmmoCount = 20;

    static int currentShieldHealth;
    static int maxShieldHealth = 100;

    public static int GetCurrentShieldHealth() { return currentShieldHealth; }
    public static int GetMaxShieldHealth() { return maxShieldHealth; }

    public static void ModifyShieldHealth(int val) { currentShieldHealth = Mathf.Clamp(currentShieldHealth + val, 0, maxShieldHealth); }

    public static void ResetPlayerValues() 
    {
        currentOxygenLevel = maxOxygenlevel;
        bombCount = maxBombCount;
        ammoCount = maxAmmoCount;
        currentShieldHealth = maxShieldHealth;
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
