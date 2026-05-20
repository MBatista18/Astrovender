using UnityEngine;

public class OpenSubgame : MonoBehaviour
{
    string gameWorldID = "Subgame Test";

    public void ContinueToNextDay()
    {
        if (GameManager.Instance == null) { Debug.LogError("No Game Manager"); }

        GameManager.Instance.StartDay(0);
        UnityEngine.SceneManagement.SceneManager.LoadScene(gameWorldID);
    }
}
