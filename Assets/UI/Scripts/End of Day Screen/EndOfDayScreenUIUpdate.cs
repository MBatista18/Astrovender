using UnityEngine;
using TMPro;

public class EndOfDayScreenUIUpdate : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI header;
    [SerializeField] TextMeshProUGUI coinCount;
    [SerializeField] TextMeshProUGUI gemCount;
    [SerializeField] TextMeshProUGUI deathCheck;

    private void Start()
    {
        if (GameManager.Instance == null) { return; }
        if (!header || !coinCount || !gemCount) { return; }

        header.text = "End of Day " + GameManager.Instance.CurrentDay;
        coinCount.text = "Total Count Count: " + GameManager.Instance.CurrentCoins;
        gemCount.text = "Current Gem Count: " + GameManager.Instance.CurrentGems;
        deathCheck.text = "Died today? " + (GameManager.Instance.GetProgressSuccessful() ? "No" : "Yes");
    }
}
