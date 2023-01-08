using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSource : MonoBehaviour
{
    [SerializeField] private string sourceName;

    public Resource[] resources;
    public HarvesterScript harvester;
    public string layerName;
}

[System.Serializable]
public struct Resource
{
    public string name;
    public float amt;
}