using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapButtonBehavior : MonoBehaviour
{
    Button button;
    //bool mapIsActive = true;

    [SerializeField] GameObject mapUI;

    private void Awake()
    {
        button = GetComponent<Button>();

        mapUI.SetActive(true);

        OnInteract(); // disables the map on start and sets map is active to false
    }

    private void Update()
    {

        bool isInteractible = false;

        DungeonDatObj obj;

        if (GameManager.Instance.currentdataObj.dungeons.TryGetValue(SceneManager.GetActiveScene().name, out obj))
        {
            isInteractible = obj.foundMap;
        }

        button.interactable = isInteractible;
    }

    public void OnInteract()
    {
        mapUI.SetActive(!mapUI.activeSelf);
    }
}
