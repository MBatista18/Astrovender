using UnityEngine;

public class ShockZone : MonoBehaviour
{
    [SerializeField] private float lifetime = 2f; // How long the shock stays active

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
