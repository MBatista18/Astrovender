using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ContinueButton : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Start()
    {
        // Disables the button if there is no saved data to load
        if (SaveManager.Instance == null || !SaveManager.Instance.SaveFileExists())
        {
            button.interactable = false;
            text.color = Color.gray; // Change text color to indicate disabled state
        }
        else 
        {
            button.interactable = true;
            text.color = Color.white; // Ensure text color is normal if the button is interactable
        }
    }
}
