using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginNextDay : MonoBehaviour
{
    string gameWorldID = "GameWorldPrototyping";

    public void LoadGame()
    {
        SaveManager.Instance.Load(GameManager.Instance);
    }

    public void ContinueToNextDay()
    {
        if (GameManager.Instance == null) { Debug.LogError("No Game Manager"); }

        GameManager.Instance.StartDay();
        SceneManager.LoadScene(gameWorldID);
    }
}
