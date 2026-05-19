using UnityEngine;

public class OpenSubgame : MonoBehaviour
{
    string gameWorldID = "Subgame Test";

    public void ContinueToNextDay()
    {
        if (GameManager.Instance == null) { Debug.LogError("No Game Manager"); }

        GameManager.Instance.StartDay();
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameWorldID);
    }
}
