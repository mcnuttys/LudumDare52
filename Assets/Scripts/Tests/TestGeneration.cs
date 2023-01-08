using UnityEngine;

public class TestGeneration : MonoBehaviour
{
    [SerializeField] private Transform center;
    [SerializeField] private GameObject emptyPrefab;
    [SerializeField] private GameObject mehSpherePrefab;
    [SerializeField] private float planetRadius = 100;

    [Header("Test 1: Random Inside Sphere")]
    [SerializeField] private int planetDensity = 1000;

    [Header("Test 2: On Crusts && Test 3: Even on Crusts")]
    [SerializeField] private Crust[] crusts;
    [SerializeField] private float evenRandomOffset = 0.25f;

    void Start()
    {
        EvenOnCrusts();
    }

    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Alpha1)) RandomInsideSphere();
        if (Input.GetKeyDown(KeyCode.Alpha2)) RandomOnCrusts();
        if (Input.GetKeyDown(KeyCode.Alpha3)) EvenOnCrusts();
    }

    void ClearShape()
    {
        var newCenter = Instantiate(emptyPrefab, transform);
        newCenter.name = "Planet Center";

        Destroy(center.gameObject);
        center = newCenter.transform;
    }

    void RandomInsideSphere()
    {
        ClearShape();

        var centerPosition = center.position;
        for (int i = 0; i < planetDensity; i++)
        {
            var x = Random.Range(-planetRadius, planetRadius);
            var y = Random.Range(-planetRadius, planetRadius);
            var z = Random.Range(-planetRadius, planetRadius);
            var position = new Vector3(x, y, z);

            var dist = Vector3.Distance(center.position, position);

            if(dist > planetRadius)
            {
                i--;
                continue;
            }

            var fromCenter = position - centerPosition;
            Instantiate(mehSpherePrefab, position, Quaternion.LookRotation(fromCenter), center);
        }
    }   

    void RandomOnCrusts()
    {
        ClearShape();

        var centerPosition = center.position;
        for (int i = 0; i < crusts.Length; i++)
        {
            var layer = crusts[i];
            var crustGO = Instantiate(emptyPrefab, center);
            crustGO.name = layer.name;

            for(int j = 0; j < layer.density; j++)
            {
                var position = Random.onUnitSphere * layer.radius;

                var fromCenter = position - centerPosition;
                fromCenter = fromCenter.normalized;

                var prefab = layer.prefabs[Random.Range(0, layer.prefabs.Length)];

                if (prefab == null) continue;
                var go = Instantiate(prefab, position, Quaternion.identity, crustGO.transform);
                go.transform.localScale = layer.scale;

                if (fromCenter == Vector3.zero) continue;
                go.transform.forward = fromCenter;
            }
        }
    }

    void EvenOnCrusts()
    {
        //Random.InitState(0);

        ClearShape();

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
                var go = Instantiate(prefab, position, Quaternion.identity, crustGO.transform);

                if (fromCenter == Vector3.zero) continue;
                go.transform.forward = fromCenter;
            }
        }
    }
}
