using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenShop : MonoBehaviour
{
    public void ContinueToNextDay()
    {
        if (GameManager.Instance == null) { Debug.LogError("No Game Manager"); }

        SceneManager.LoadScene("ShopScene");
    }
}
