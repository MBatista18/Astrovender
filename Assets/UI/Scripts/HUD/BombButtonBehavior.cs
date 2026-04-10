using UnityEngine;
using UnityEngine.UI;

public class BombButtonBehavior : MonoBehaviour
{
    Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        button.interactable = GameManager.Instance.currentdataObj.hasGun;
    }
}
