using UnityEngine;
using TMPro;

public class SetHUDText : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI ammoCount;
    public void SetAmmoText(int currentAmmo) { if (!ammoCount) return; ammoCount.text = "AMMO: " + currentAmmo; }
    [SerializeField] TextMeshProUGUI bombCount;
    public void SetBombText(int currentBombs) { if (!bombCount) return; bombCount.text = "BOMB: " + currentBombs; }
}
