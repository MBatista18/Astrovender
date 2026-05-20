using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginNextDay : MonoBehaviour
{
    string gameWorldID = "GameWorldPrototyping";

    public void LoadGame()
    {
        SaveManager.Instance.Load(GameManager.Instance);
    }

    public void Continue()
    {
        if (GameManager.Instance == null) { Debug.LogError("No Game Manager"); }

        GameManager.Instance.ContinueGame(); // Start from the start of the current saved day
        SceneManager.LoadScene(gameWorldID);
    }

    public void NewGame()
    {
        if (GameManager.Instance == null) { Debug.LogError("No Game Manager"); }

        GameManager.Instance.StartNewGame(); // Start fresh from day 1
        SceneManager.LoadScene(gameWorldID);
    }

    public void ContinueToNextDay()
    {
        if (GameManager.Instance == null) { Debug.LogError("No Game Manager"); }

        GameManager.Instance.StartDay(); // Progress to the next day
        SceneManager.LoadScene(gameWorldID);
    }
}
