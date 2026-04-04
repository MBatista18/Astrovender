using UnityEngine;

public class Port : MonoBehaviour
{
    private PortManager portManager;
    private bool activated = false;
    private bool hasBoss = false;

    public void Initialize(PortManager manager)
    {
        portManager = manager;
    }

    public bool GetActivated() => activated;

    public void ActivatePort()
    {
        if (!activated)
        {
            activated = true;
            // Additional logic for activating the port (e.g., visual effects)
        }
    }

    public void DeactivatePort()
    {
        if (activated)
        {
            activated = false;
            hasBoss = false;
            portManager.DeactivatePort(this);
            // Additional logic for deactivating the port (e.g., visual effects)
        }
    }

    public void HideInPort()
    {
        portManager.SetBossPort(this);
    }

    public void SetBossPresence(bool presence)
    {
        hasBoss = presence;
    }
}
