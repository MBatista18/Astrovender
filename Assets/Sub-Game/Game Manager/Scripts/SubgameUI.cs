using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubgameUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI movesRemainingText;

    public void UpdateMovesRemaining(int movesRemaining)
    {
        movesRemainingText.text = $"Moves Remaining: {movesRemaining}";
    }

    SubgameCommunicator communicator;
    SubgameBoard subgameBoard;

    private void Awake()
    {
        subgameBoard = FindFirstObjectByType<SubgameBoard>();
        communicator = FindFirstObjectByType<SubgameCommunicator>();
        SetState(1);
    }

    public void OnEnd()
    {
        SetState(2);
    }

    [Header("Main HUD")]
    [SerializeField] GameObject parent;
    [SerializeField] Slider oxygenSlider;
    [SerializeField] TextMeshProUGUI oxygenText;

    [SerializeField] Slider bombSlider;
    [SerializeField] TextMeshProUGUI bombText;

    [SerializeField] Slider ammoSlider;
    [SerializeField] TextMeshProUGUI ammoText;

    [SerializeField] Slider shieldSlider;
    [SerializeField] TextMeshProUGUI shieldText;

    [SerializeField] TextMeshProUGUI coinsText;

    [Header("Complete Level")]
    [SerializeField] GameObject c_parent;
    [SerializeField] TextMeshProUGUI c_oxygenText;
    [SerializeField] TextMeshProUGUI c_bombText;
    [SerializeField] TextMeshProUGUI c_ammoText;
    [SerializeField] TextMeshProUGUI c_shieldText;

    public void SetState(int state)
    {
        if (state == 1)
        {
            parent.SetActive(true);
            c_parent.SetActive(false);
        }

        if (state == 2)
        {
            parent.SetActive(false);
            c_parent.SetActive(true);
        }
    }

    private void Update()
    {
        coinsText.text = "" + GameManager.Instance.currentdataObj.coins;

        oxygenSlider.value = (float)communicator.GetOxygenVal() / (float)PlayerManager.GetMaxOxygenLevel();
        oxygenText.text = "" + communicator.GetOxygenVal();

        if (GameManager.Instance.currentdataObj.hasBombs)
        {
            bombSlider.gameObject.SetActive(true);
            bombSlider.value = (float)communicator.GetBombVal() / (float)PlayerManager.GetMaxBombCount();
            bombText.text = "" + communicator.GetBombVal();
        }
        else
        {
            bombSlider.gameObject.SetActive(false);
        }

        if (GameManager.Instance.currentdataObj.hasGun)
        {
            ammoSlider.gameObject.SetActive(true);
            ammoSlider.value = (float)communicator.GetAmmoVal() / (float)PlayerManager.GetMaxAmmoCount();
            ammoText.text = "" + communicator.GetAmmoVal();
        }
        else
        {
            ammoSlider.gameObject.SetActive(false);
        }

        if (GameManager.Instance.currentdataObj.hasShield)
        {
            shieldSlider.gameObject.SetActive(true);
            shieldSlider.value = (float)communicator.GetShieldVal() / (float)PlayerManager.GetMaxShieldHealth();
            shieldText.text = "" + communicator.GetShieldVal();
        }
        else
        {
            shieldSlider.gameObject.SetActive(false);
        }

        c_oxygenText.text = "Oxygen Level: " + communicator.GetOxygenVal();
        c_ammoText.text = "Ammo Capacity: " + communicator.GetAmmoVal();
        c_ammoText.color = new Color(1, 1, 1, GameManager.Instance.currentdataObj.hasGun ? 1 : -1);
        c_bombText.text = "Bomb Capacity: " + communicator.GetBombVal();
        c_bombText.color = new Color(1, 1, 1, GameManager.Instance.currentdataObj.hasBombs ? 1 : -1);
        c_shieldText.text = "Shield Fuel: " + communicator.GetShieldVal();
        c_shieldText.color = new Color(1, 1, 1, GameManager.Instance.currentdataObj.hasShield ? 1 : -1);
    }
}
