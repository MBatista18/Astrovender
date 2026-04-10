using UnityEngine;
using System.Collections;

using UnityEngine.SceneManagement; // for now, the value hitting 0 will simply reset the scene for the final build, we will change this in the final build

public class PlayerHealth : MonoBehaviour
{    static bool canBeHurt = true;
    float invulnerabilityTimer;

    public static void ModifyOxygenLevel(int val, bool bypassInvulnerabilityCheck, Vector3 position)
    {
        if (GameManager.Instance.currentdataObj.hasShield && PlayerManager.ShieldCanDefend(AssetCall.instance.playerSM.transform.position, position, AssetCall.instance.playerSM.GetFacingDirection()))
        {
            if (!bypassInvulnerabilityCheck) // It's bad practice to just copy the code over to here, but it probably works, so I'll just do it I guess
            {
                if (val < 0 && !canBeHurt) { return; } else { canBeHurt = false; }
            }

            if (PlayerManager.GetCurrentShieldHealth() > 0)
            {
                AssetCall.instance.playerSM.GetAudioCall().CallAudioClip("DamageShield");

                PlayerManager.ModifyShieldHealth(val);
                AssetCall.instance.playerSM.GetShieldVisuals()?.PlayHit();

                return;
            }
        }

        ModifyOxygenLevel(val, bypassInvulnerabilityCheck);
    }

    public static void ModifyOxygenLevel(int val, bool bypassInvulnerabilityCheck) // bypass for invulnerability check in case of necessity (e.g. oxygen countdown, insta-death attack, etc.)
    {
        // only check for invincibility frames if they can't be bypassed
        // if being damaged and the player is currently invulnerable, return false
        if (!bypassInvulnerabilityCheck)
        {
            if (val < 0 && !canBeHurt) { Debug.Log("No Damage"); return; } 
            else 
            {
                //Debug.Log("Damage; " + val + "; canBehurt = " + canBeHurt + "; bypass = " + bypassInvulnerabilityCheck);
                canBeHurt = false;

                AssetCall.instance.playerSM.GetFlashOnHit()?.FlashRed();

                AssetCall.instance.playerSM.GetAudioCall()?.CallAudioClip("Damage"); 
            }
        }


        PlayerManager.currentOxygenLevel = Mathf.Clamp(PlayerManager.currentOxygenLevel + val, 0, PlayerManager.GetMaxOxygenLevel());

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
        if (canBeHurt) { invulnerabilityTimer = .7f;
        } // sets the invulnerability timer
        else // gives the player a brief period of invulnerability after being hurt
        {
            invulnerabilityTimer -= Time.deltaTime;
            if (invulnerabilityTimer <= 0)
            {
                invulnerabilityTimer = .7f; 
                // there's a second call to reset the timer here, since if the player gets damaged multiple times per frame after canbehurt is reset, then it can 
                // deal multiple attacks to the player before the next frame loads, meaning the previous call to reset the timer won't run
                canBeHurt = true;
            }
        }

        if (PlayerManager.currentOxygenLevel > PlayerManager.GetMaxOxygenLevel() * .25f)
        {
            healthDipping = false;
        }
        else
        {
            if (!healthDipping)
            {
                StartCoroutine(beepbeepbeep());
                healthDipping = true;
            }
        }
    }

    bool healthDipping;

    IEnumerator beepbeepbeep()
    {
        int i = 0;
        while (i < 3)
        {
            i++;
            AssetCall.instance.playerSM.GetAudioCall().CallAudioClip("DamageLowOxygen");
            yield return new WaitForSeconds(0.5f);
        }
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
