using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement; // for now, the value hitting 0 will simply reset the scene for the final build, we will change this in the final build

public class PlayerHealth : MonoBehaviour
{
    SetHUDText hudText;

    private void Awake()
    {
        hudText = FindFirstObjectByType<SetHUDText>();
    }
    
    [SerializeField] int maxOxygenlevel;
    static int currentOxygenLevel; // having all this stuff be static may not be the best way of handling this, may need to iterate on this later -Dirk
    
    static bool canBeHurt;
    float invulnerabilityTimer;

    public static void ModifyOxygenLevel(int val, bool bypassInvulnerabilityCheck) // bypass for invulnerability check in case of necessity (e.g. oxygen countdown, insta-death attack, etc.)
    {
        if (val < 0 && (!canBeHurt && !bypassInvulnerabilityCheck)) { return; } else { canBeHurt = false; } // if being damaged and the player is currently invulnerable, return false

        currentOxygenLevel += val;
      
        if (currentOxygenLevel <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Start()
    {
        currentOxygenLevel = maxOxygenlevel;
        StartCoroutine(oxygenCountDown());
    }



    private void Update()
    {
        if (canBeHurt) { invulnerabilityTimer = 1.5f; } // sets the invulnerability timer
        else // gives the player a brief period of invulnerability after being hurt
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0)
            {
                canBeHurt = true;
            }
        }

        hudText.SetOxygenText(currentOxygenLevel);
    }

    IEnumerator oxygenCountDown()
    {
        while (true) 
        {
            yield return new WaitForSeconds(1f);
            ModifyOxygenLevel(-1, true);
        }
    }
}
