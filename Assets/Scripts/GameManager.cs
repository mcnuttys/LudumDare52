using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class GameManager : MonoBehaviour
{
    private Dictionary<string, float> aquiredResources;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        aquiredResources = new Dictionary<string, float>();
    }

    public void EnableHarvesting()
    {
        HarvesterController.instance.harvestingEnabled = true;
    }

    public void DisableHarvesting()
    {
        HarvesterController.instance.harvestingEnabled = false;
    }

    public void ClearResources()
    {
        aquiredResources = new Dictionary<string, float>();
    }

    public void AddResources(string name, float amt)
    {
        if (!aquiredResources.ContainsKey(name)) aquiredResources.Add(name, 0);
        aquiredResources[name] += amt;
    }

    public int GetAmt(string resource)
    {
        if (!aquiredResources.ContainsKey(resource)) return -1;

        return Mathf.RoundToInt(aquiredResources[resource]);
    }
}
