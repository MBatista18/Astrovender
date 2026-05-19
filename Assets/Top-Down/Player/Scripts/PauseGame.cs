using UnityEngine;

public class PauseGame : MonoBehaviour
{
    bool gameIsPaused = false;

    public GameObject pauseMenu;

    private void Awake()
    {
        pauseMenu.SetActive(false);
    }

    private void OnEnable()
    {
        InputManager.pauseInput += Pause;
    }

    public void Pause()
    {
        gameIsPaused = !gameIsPaused;

        pauseMenu.SetActive(gameIsPaused);

        Time.timeScale = gameIsPaused ? 0 : 1;
    }

    private void OnDisable()
    {
        gameIsPaused = false;
        Time.timeScale = 1;

        InputManager.pauseInput -= Pause;
    }
}
