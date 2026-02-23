using UnityEngine;
using UnityEngine.SceneManagement;

public class ShipEntranceEndDay : MonoBehaviour
{
    [SerializeField] int endOfDayScene_ID = 1;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (GameManager.Instance == null) { Debug.LogError("No Game Manager present, day will not end"); return; }

        GameManager.Instance.Progress(true);

        if (collision.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            SceneManager.LoadScene(endOfDayScene_ID);
        }
    }
}
