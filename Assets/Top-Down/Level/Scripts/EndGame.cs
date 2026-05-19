using UnityEngine;

public class EndGame : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene("EndingCutscene");
        }
    }
}
