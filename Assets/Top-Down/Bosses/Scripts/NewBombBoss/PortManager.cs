using System.Collections.Generic;
using UnityEngine;

public class PortManager : MonoBehaviour
{
    [SerializeField] private BombBossSM bossSM;

    private Port[] allPorts = new Port[5];
    private List<Port> activePortsList;
    private GameObject boss;
    private int bossPortIndex = -1;

    private void Awake()
    {
        if (bossSM != null)
            boss = bossSM.gameObject;
        else
            Debug.LogError("Boss State Machine reference is missing in PortManager.");

        allPorts = GetComponentsInChildren<Port>();
        activePortsList = new List<Port>();

        for (int i = 0; i < allPorts.Length; i++)
        {
            allPorts[i].Initialize(this);
            activePortsList.Add(allPorts[i]);
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
        bossPortIndex = System.Array.IndexOf(allPorts, port);
        return true;
    }

    public void SetBossPort(Port port)
    {
        foreach (Port p in allPorts)
        {
            p.SetBossPresence(p == port);
        }

        boss.transform.position = port.transform.position;
    }

    public void DeactivatePort(Port port)
    {
        if (activePortsList.Contains(port))
        {
            activePortsList.Remove(port);
        }
    }
}
