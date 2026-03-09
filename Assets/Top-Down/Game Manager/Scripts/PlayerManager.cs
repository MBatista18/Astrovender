using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static Vector3 playerWorldSpawn = new Vector3(10,23);
    
    static int maxOxygenlevel = 1400;
    public static int currentOxygenLevel; // having all this stuff be static may not be the best way of handling this, may need to iterate on this later -Dirk
    
    public static int bombCount = 20;
    public static int ammoCount = 20;

    public static void ResetPlayerValues() 
    {
        currentOxygenLevel = maxOxygenlevel;
        bombCount = 20;
        ammoCount = 20;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            AssetCall.instance.playerSM.transform.position = playerWorldSpawn;
            Debug.Log("is scene; " + playerWorldSpawn);
        }
    }
}
