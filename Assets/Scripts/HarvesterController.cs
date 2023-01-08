using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class HarvesterController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private LayerMask mask;

    [Header("Placement Settings")]
    [SerializeField] private GameObject harvesterPrefab;
    [SerializeField] private float harvesterHeight = 1;

    public bool harvestingEnabled;

    public static HarvesterController instance;

    private bool mouseDown;
    private float clickTimer;

    private void Awake()
    {
        instance = this;
    }

    void Update()
    {
        if (!mouseDown) clickTimer = 0;

        if (!harvestingEnabled) return;

        mouseDown = Input.GetMouseButton(0);
        if (mouseDown) clickTimer += Time.deltaTime;

        if (Input.GetMouseButtonUp(0) && clickTimer <= 0.25)
        {
            RaycastHit hit;
            if (!Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, Mathf.Infinity, mask))
                return;

            if (!hit.transform.TryGetComponent<ResourceSource>(out var resource) && !hit.transform.parent.TryGetComponent(out resource))
                return;

            if (resource.harvester != null)
                return;

            var position = hit.transform.position + hit.transform.position.normalized * harvesterHeight;
            var harvesterGO = Instantiate(harvesterPrefab, position, Quaternion.FromToRotation(Vector3.up, position.normalized), transform);

            var harvester = harvesterGO.GetComponent<HarvesterScript>();
            resource.harvester = harvester;
            harvester.Harvest(resource);
        }
    }

    public void ClearHarvesters()
    {
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Destroy(transform.GetChild(i).gameObject);
        }
    }
}
