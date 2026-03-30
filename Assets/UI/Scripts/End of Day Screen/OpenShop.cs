using UnityEngine;
using UnityEngine.SceneManagement;

public class OpenShop : MonoBehaviour
{
    [SerializeField] int shopScene_ID = 0;

    public void ContinueToNextDay()
    {
        if (GameManager.Instance == null) { Debug.LogError("No Game Manager"); }

        SceneManager.LoadScene(shopScene_ID);
    }
}
