using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement; // for now, the value hitting 0 will simply reset the scene for the final build, we will change this in the final build

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxOxygenlevel;
    static int currentOxygenLevel; // having all this stuff be static may not be the best way of handling this, may need to iterate on this later -Dirk
    
    static bool canBeHurt = true;
    float invulnerabilityTimer;

    public static void ModifyOxygenLevel(int val, bool bypassInvulnerabilityCheck) // bypass for invulnerability check in case of necessity (e.g. oxygen countdown, insta-death attack, etc.)
    {
        // only check for invincibility frames if they can't be bypassed
        // if being damaged and the player is currently invulnerable, return false
        if (!bypassInvulnerabilityCheck) {
            if (val < 0 && !canBeHurt) { Debug.Log("No Damage"); return; } else { canBeHurt = false; Debug.Log("Damage"); } 
        }

        currentOxygenLevel += val;

        GameManager.Instance.Progress(false);

        if (currentOxygenLevel <= 0)
        {
            SceneManager.LoadScene(1);
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

        AssetCall.instance.HUDText.SetOxygenText(currentOxygenLevel);
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
