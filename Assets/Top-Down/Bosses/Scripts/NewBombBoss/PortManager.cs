using System.Collections.Generic;
using UnityEngine;

public class PortManager : MonoBehaviour
{
    [SerializeField] private BombBossSM bossSM;
    [HideInInspector] public Port[] PortsArray = new Port[5];

    private List<Port> activePortsList;
    private GameObject boss;

    private void Awake()
    {
        if (bossSM != null)
            boss = bossSM.gameObject;
        else
            Debug.LogError("Boss State Machine reference is missing in PortManager.");

        PortsArray = GetComponentsInChildren<Port>();
        activePortsList = new List<Port>();

        for (int i = 0; i < PortsArray.Length; i++)
        {
            PortsArray[i].Initialize(this);
            activePortsList.Add(PortsArray[i]);
        }
    }

    public bool GetRandomActivePort(out Port port)
    {
        if (activePortsList.Count <= 0)
        {
            port = null;
            return false;
        }

        int randomIndex = Random.Range(0, activePortsList.Count);
        port = activePortsList[randomIndex];
        return true;
    }

    public void OnPortExplosion(Port port, bool isBossPort)
    {
        RemoveActivePort(port);

        if (isBossPort && (bossSM.GetCurrentState() == bossSM.InitialState()))
        {
            // If the boss port is hit while the boss is hidden, the boss becomes vulnerable
            port.SetBossPresence(false);
            bossSM.TakeDamage(20);
            bossSM.ChangeState(bossSM.GetVulnerableState());
        }
    }

    public void SetBossPort(Port port)
    {
        foreach (Port p in PortsArray)
        {
            p.SetBossPresence(p == port);
        }

        Debug.Log($"Boss is now in port: {port.gameObject.name}");
        boss.transform.position = port.transform.position + (Vector3.up * .5f);
    }

    public void RemoveActivePort(Port port)
    {
        if (activePortsList.Contains(port))
        {
            activePortsList.Remove(port);
        }
    }
}
