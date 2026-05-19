using UnityEngine;

public class CallSceneAt : MonoBehaviour
{
    [SerializeField] string SceneName;

    public void LoadScene() 
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneName);
    }
}
