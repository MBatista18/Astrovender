using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SetHUDText : MonoBehaviour
{
    [Header("Player Consumables")]
    [SerializeField] TextMeshProUGUI ammoCount;
    public void SetAmmoText(int currentAmmo) { if (!ammoCount) return; ammoCount.text = "" + currentAmmo; }
    [SerializeField] TextMeshProUGUI bombCount;
    public void SetBombText(int currentBombs) { if (!bombCount) return; bombCount.text = "" + currentBombs; }
    [SerializeField] TextMeshProUGUI oxygenCount;
    [SerializeField] Slider oxygenSlider;
    public void SetOxygenText(int currentOxygen)
    {
        if (!oxygenCount) { return; } oxygenCount.text = "" + currentOxygen;
        if (!oxygenSlider) { return; } oxygenSlider.value = (float) currentOxygen / (float) PlayerManager.GetMaxOxygenLevel();
    }
    [SerializeField] TextMeshProUGUI shieldCount;
    [SerializeField] Slider shieldSlider;
    public void SetShieldText(int currentShield) 
    {
        if (!shieldCount) { return; } shieldCount.text = "" + currentShield;
        if (!shieldSlider) { return; } shieldSlider.value = (float) currentShield / (float) PlayerManager.GetMaxShieldHealth();
    }


    [Header("Game Management")]
    [SerializeField] TextMeshProUGUI dayText;
    public void SetDayText(int dayNumber) { if (!dayText) return; dayText.text = "DAY: " + dayNumber; }
    [SerializeField] TextMeshProUGUI coinText;
    public void SetCoinText(int currentCoin) { if (!coinText) return; coinText.text = "" + currentCoin; }
    [SerializeField] TextMeshProUGUI gemText;
    public void SetGemText(int currentGems) { if (!gemText) return; gemText.text = "" + currentGems; }

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
        RefreshKeys();
    }

    public void RefreshCollectiblesUI()
    {
        if (GameManager.Instance.currentdataObj.hasBombs)
        {
            bombCount.gameObject.SetActive(true);
            SetBombText(PlayerManager.bombCount);
        }
        else
        {
            bombCount.gameObject.SetActive(false);
        }

        if (GameManager.Instance.currentdataObj.hasShield)
        {
            shieldSlider.gameObject.SetActive(true);
            shieldCount.alpha = 1;
            SetShieldText(PlayerManager.GetCurrentShieldHealth());
        }
        else
        {
            shieldCount.alpha = 0;
            shieldSlider.gameObject.SetActive(false);
        }

        if (GameManager.Instance.currentdataObj.hasGun)
        {
            ammoCount.gameObject.SetActive(true);
            SetAmmoText(PlayerManager.ammoCount);
        }
        else
        {
            ammoCount.gameObject.SetActive(false);
        }

        SetOxygenText(PlayerManager.currentOxygenLevel);
    }

    public void RefreshDailyValuesUI()
    {
        SetDayText(GameManager.Instance.currentdataObj.day);
        SetCoinText(GameManager.Instance.collectedCoins);
        SetGemText(GameManager.Instance.collectedGems);
    }

    float MAXTIMER = 2f;
    float a2Timer = 0;
    float a1Timer = 0;

    bool beginCountdown = false;
    public void BeginCountdown() { beginCountdown = true; }
    
    public void RefreshFadeInUI()
    {
        if (!beginCountdown) { return; }

        a1Timer += Time.deltaTime;
        a2Timer += Time.deltaTime;

        fadeOutCircle.color = new Color(1, 1, 1, Mathf.Clamp01(a1Timer/ MAXTIMER));
        fadeOut.color = new Color(0, 0, 0, Mathf.Clamp01(a1Timer - MAXTIMER));
    }

    public Image greenKey;
    public Image redKey;
    public Image blueKey;

    public void RefreshKeys()
    {
        bool showBlue = false;
        bool showRed = false;
        bool showGreen = false;

        if (GameManager.Instance.currentdataObj.dungeons.ContainsKey(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name))
        {
            if (GameManager.Instance.currentdataObj.dungeons[UnityEngine.SceneManagement.SceneManager.GetActiveScene().name].hasBlueKey)
            {
                showBlue = true;
            }

            if (GameManager.Instance.currentdataObj.dungeons[UnityEngine.SceneManagement.SceneManager.GetActiveScene().name].hasRedKey)
            {
                showRed = true;
            }

            if (GameManager.Instance.currentdataObj.dungeons[UnityEngine.SceneManagement.SceneManager.GetActiveScene().name].hasGreenKey)
            {
                showGreen = true;
            }
        }

        greenKey.color = new Color(greenKey.color.r, greenKey.color.g, greenKey.color.b, showGreen ? 1 : 0);
        redKey.color = new Color(redKey.color.r, redKey.color.g, redKey.color.b, showRed ? 1 : 0);
        blueKey.color = new Color(blueKey.color.r, blueKey.color.g, blueKey.color.b, showBlue ? 1 : 0);
    }
}
