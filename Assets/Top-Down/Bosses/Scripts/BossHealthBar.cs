using UnityEngine;
using UnityEngine.UI;

public class BossHealthBar : MonoBehaviour
{
    [SerializeField] EnemySM boss;
    [SerializeField] Slider healthSlider;
    void Update()
    {
       // Debug.Log(boss.GetHealth() + " / " + boss.GetMaxHealth());

        healthSlider.value = Mathf.Clamp01((float) boss.GetHealth() / (float)boss.GetMaxHealth());
    }
}
