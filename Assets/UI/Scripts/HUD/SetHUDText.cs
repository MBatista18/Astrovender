using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetHUDText : MonoBehaviour
{
    [Header("Player Consumables")]
    [SerializeField] TextMeshProUGUI ammoCount;
    public void SetAmmoText(int currentAmmo) { if (!ammoCount) return; ammoCount.text = "AMMO: " + currentAmmo; }
    [SerializeField] TextMeshProUGUI bombCount;
    public void SetBombText(int currentBombs) { if (!bombCount) return; bombCount.text = "BOMB: " + currentBombs; }
    [SerializeField] TextMeshProUGUI oxygenCount;
    public void SetOxygenText(int currentOxygen) { if (!oxygenCount) return; oxygenCount.text = "OXYGEN: " + currentOxygen; }
    [SerializeField] TextMeshProUGUI shieldCount;
    public void SetShieldText(int currentShield) { if (!shieldCount) return; shieldCount.text = "SHIELD: " + currentShield; }


    [Header("Game Management")]
    [SerializeField] TextMeshProUGUI dayText;
    public void SetDayText(int dayNumber) { if (!dayText) return; dayText.text = "DAY: " + dayNumber; }
    [SerializeField] TextMeshProUGUI coinText;
    public void SetCoinText(int currentCoin) { if (!coinText) return; coinText.text = "COLLECTED COINS: " + currentCoin; }
    [SerializeField] TextMeshProUGUI gemText;
    public void SetGemText(int currentGems) { if (!gemText) return; gemText.text = "COLLECTED GEMS: " + currentGems; }

    [Header("Fade Out")]
    public Image fadeOutCircle;
    public Image fadeOut;

    private void Start()
    {
        RefreshDailyValuesUI();
    }

    private void Update()
    {
        RefreshCollectiblesUI();
        RefreshFadeInUI();
    }

    public void RefreshCollectiblesUI()
    {
        ammoCount.alpha = GameManager.Instance.currentdataObj.hasGun ? 1 : 0;
        bombCount.alpha = GameManager.Instance.currentdataObj.hasBombs ? 1 : 0;
        shieldCount.alpha = GameManager.Instance.currentdataObj.hasShield ? 1 : 0;

        SetBombText(PlayerManager.bombCount);
        SetAmmoText(PlayerManager.ammoCount);
        SetOxygenText(PlayerManager.currentOxygenLevel);
        SetShieldText(PlayerManager.GetCurrentShieldHealth());
    }

    public void RefreshDailyValuesUI()
    {
        SetDayText(GameManager.Instance.currentdataObj.day);
        SetCoinText(GameManager.Instance.collectedCoins);
        SetGemText(GameManager.Instance.collectedGems);
    }

    float a2Timer = 0;
    float a1Timer = 0;
    
    public void RefreshFadeInUI()
    {
        if (PlayerManager.currentOxygenLevel <= 5) { a1Timer += Time.deltaTime; } else { a1Timer = 0; } 
        // hopefully there's never a situation where the player gains 3 seconds of oxygen after only having 1 second of oxygen left, because
            // that would misalign this timer from the actual oxygen meter

        if (PlayerManager.currentOxygenLevel <= 1) { a2Timer += Time.deltaTime; } else { a2Timer = 0; }

        fadeOutCircle.color = new Color(1, 1, 1, Mathf.Clamp01(a1Timer/5f));
        fadeOut.color = new Color(0, 0, 0, a2Timer);
    }
}
