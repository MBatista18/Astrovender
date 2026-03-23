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

        header.text = "End of Day " + GameManager.Instance.currentdataObj.day;
        coinCount.text = "Total Count Count: " + GameManager.Instance.currentdataObj.coins;
        gemCount.text = "Current Gem Count: " + GameManager.Instance.currentdataObj.gems;
        deathCheck.text = "Died today? " + (GameManager.Instance.GetProgressSuccessful() ? "No" : "Yes");
    }
}
