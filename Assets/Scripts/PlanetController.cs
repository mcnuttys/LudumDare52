using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private GameObject emptyPrefab;

    [Header("Generator Settings")]
    [SerializeField] private Crust[] crusts;
    [SerializeField] private bool randomSeed = true;
    [SerializeField] private int seed = 0;

    [Header("Liquid Settings")]
    [SerializeField] private Transform liquidLayer;
    [SerializeField] private MeshRenderer liquidLayerRenderer;
    [SerializeField] private Gradient liquidLayerColors;
    [SerializeField] private float maxLiquidLevel, minLiquidLevel;
    //[SerializeField] private float collapseLevel = 6; // Can not harvest more when the liquid level reaches this... Maybe an explosion particle effect or something...

    private Dictionary<string, int> layerDensities;

    public static PlanetController instance;
    private int totalSources;
    private float currentWaterLevel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (randomSeed) seed = Random.Range(0, 1000000);
        GeneratePlanet(seed);
    }

    private void Update()
    {
        liquidLayer.localScale = Vector3.Lerp(liquidLayer.localScale, new Vector3(currentWaterLevel, currentWaterLevel, currentWaterLevel), 3 * Time.deltaTime);
    }

    public void GeneratePlanet(int seed = 0)
    {
        Random.InitState(seed);

        GeneratePlanet();
    }

    void ClearPlanet()
    {
        var newCenter = Instantiate(emptyPrefab, transform);
        newCenter.name = "Planet Center";

        Destroy(center.gameObject);
        center = newCenter.transform;
    }

    void GeneratePlanet()
    {
        ClearPlanet();
        currentWaterLevel = maxLiquidLevel;

        layerDensities = new Dictionary<string, int>();
        var centerPosition = center.position;
        float phi = Mathf.PI * (3.0f - Mathf.Sqrt(5.0f));

        for (int i = 0; i < crusts.Length; i++)
        {
            var layer = crusts[i];
            var crustGO = Instantiate(emptyPrefab, center);
            crustGO.name = layer.name;

            for (int j = 0; j < layer.density; j++)
            {
                float y = 0;
                if (layer.density - 1 > 0)
                    y = 1 - ((float)j / (layer.density - 1)) * 2;

                float radius = Mathf.Sqrt(1 - y * y) * layer.radius;
                y *= layer.radius;

                float randomOffset = Random.Range(-layer.randomOffset, layer.randomOffset);
                float theta = phi * j + randomOffset;

                float x = Mathf.Cos(theta) * radius;
                float z = Mathf.Sin(theta) * radius;

                var position = new Vector3(x, y, z);

                var fromCenter = position - centerPosition;
                fromCenter = fromCenter.normalized;

                var prefab = layer.prefabs[Random.Range(0, layer.prefabs.Length)];

                if (prefab == null) continue;

                if (!layerDensities.ContainsKey(layer.name)) layerDensities.Add(layer.name, 0);
                layerDensities[layer.name]++;

                var go = Instantiate(prefab, position, Random.rotation, crustGO.transform);
                go.transform.localScale = layer.scale;

                if(go.TryGetComponent<ResourceSource>(out var resource)) resource.layerName = layer.name;

                if (layer.randomRotation || fromCenter == Vector3.zero) continue;
                go.transform.forward = fromCenter;
            }
        }

        totalSources = CalculateTotalTiles();
        UpdateLiquidLevel();
    }

    public void RemoveChunk(string layerName)
    {
        if (layerDensities != null && !layerDensities.ContainsKey(layerName)) return;
        layerDensities[layerName]--;

        UpdateLiquidLevel();
    }

    private int CalculateTotalTiles()
    {
        int totalTiles = 0;
        for (int i = 0; i < crusts.Length; i++)
        {
            var layer = crusts[i];
            if (!layerDensities.ContainsKey(layer.name)) continue;

            totalTiles += layerDensities[layer.name];
        }

        return totalTiles;
    }

    private void UpdateLiquidLevel()
    {
        int currentSources = CalculateTotalTiles();
        float percentRemaining = 1 - ((float)(totalSources - currentSources) / totalSources);
        float liquidLevel = percentRemaining * (maxLiquidLevel - minLiquidLevel) + minLiquidLevel;
        currentWaterLevel = liquidLevel;

        liquidLayerRenderer.material.color = liquidLayerColors.Evaluate(1f - percentRemaining);
    }
}

[System.Serializable]
public struct Crust
{
    public string name;
    public int density;
    public float radius;
    public GameObject[] prefabs;
    public float randomOffset;
    public bool randomRotation;

    public Vector3 scale;
}
