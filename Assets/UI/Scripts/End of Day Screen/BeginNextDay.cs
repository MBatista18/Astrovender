using UnityEngine;
using UnityEngine.SceneManagement;

public class BeginNextDay : MonoBehaviour
{
    [SerializeField] int gameplayScene_ID = 0;

    public void ContinueToNextDay()
    {
        if (GameManager.Instance == null) { Debug.LogError("No Game Manager"); }

        GameManager.Instance.StartDay();
        SceneManager.LoadScene(gameplayScene_ID);
    }
}
