using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarvesterScript : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private float harvestDuration = 60;
    [SerializeField] private float harvestTime = 0.1f;
    [SerializeField] private ParticleSystem harvestBeamParticles;

    [SerializeField] private GameObject harvestEffectPrefab;
    [SerializeField] private GameObject harvestCompleteEffect;

    private ResourceSource harvestSource;
    private float timer; 
    private float harvestTimer;
    private bool harvesting;

    private void Update()
    {
        if (harvestSource == null) return;
        if (!harvesting) return;

        timer += Time.deltaTime;
        harvestTimer += Time.deltaTime;

        if(harvestTimer > harvestTime)
        {
            float t = harvestTime / harvestDuration;
            for(int i = 0; i < harvestSource.resources.Length; i++)
            {
                var resource = harvestSource.resources[i];
                GameManager.instance.AddResources(resource.name, resource.amt * t);

                Instantiate(harvestEffectPrefab, transform.position, transform.rotation);
            }

            harvestTimer = 0;
        }

        if(timer >= harvestDuration) 
            CompleteHarvest();
    }

    public void Harvest(ResourceSource source)
    {
        harvestSource = source;
    }

    private void CompleteHarvest()
    {
        animator.SetBool("HarvestComplete", true);
        harvestBeamParticles.Stop(true, ParticleSystemStopBehavior.StopEmitting);

        if (harvestSource == null) return;

        Instantiate(harvestCompleteEffect, harvestSource.transform.position, transform.rotation);

        PlanetController.instance.RemoveChunk(harvestSource.layerName);
        Destroy(harvestSource.gameObject);
        harvestSource = null;
    }

    public void BeginHarvest()
    {
        harvestBeamParticles.Play();
        harvesting = true;
    }

    public void DestroyHarvester()
    {
        Destroy(gameObject);
    }
}
