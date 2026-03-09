using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement; // for now, the value hitting 0 will simply reset the scene for the final build, we will change this in the final build

public class PlayerHealth : MonoBehaviour
{    static bool canBeHurt = true;
    float invulnerabilityTimer;

    public static void ModifyOxygenLevel(int val, bool bypassInvulnerabilityCheck) // bypass for invulnerability check in case of necessity (e.g. oxygen countdown, insta-death attack, etc.)
    {
        // only check for invincibility frames if they can't be bypassed
        // if being damaged and the player is currently invulnerable, return false
        if (!bypassInvulnerabilityCheck)
        {
            if (val < 0 && !canBeHurt) { Debug.Log("No Damage"); return; } else { canBeHurt = false; Debug.Log("Damage"); }
        }

        PlayerManager.currentOxygenLevel += val;

        if (PlayerManager.currentOxygenLevel <= 0)
        {
            GameManager.Instance.Progress(false);
            SceneManager.LoadScene(1);
        }
    }

    private void Start()
    {
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

        AssetCall.instance.HUDText.SetOxygenText(PlayerManager.currentOxygenLevel);
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
