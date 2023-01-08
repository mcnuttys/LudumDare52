using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] private GameObject menuHolder;
    [SerializeField] private Button newPlanetButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;

    private void Start()
    {
        newPlanetButton.onClick.AddListener(NewPlanet);
        startButton.onClick.AddListener(StartButton);
        quitButton.onClick.AddListener(Quit);
    }

    private void OnDestroy()
    {
        newPlanetButton.onClick.RemoveListener(NewPlanet);
        startButton.onClick.RemoveListener(StartButton);
        quitButton.onClick.RemoveListener(Quit);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            menuHolder.SetActive(true);
            GameManager.instance.DisableHarvesting();
            CameraMovement.instance.wander = true;
        }
    }

    private void NewPlanet()
    {
        var seed = Random.Range(0, 1000000);
        PlanetController.instance.GeneratePlanet(seed);
        GameManager.instance.ClearResources();
        HarvesterController.instance.ClearHarvesters();
    }

    private void StartButton()
    {
        GameManager.instance.EnableHarvesting();
        menuHolder.SetActive(false);
        CameraMovement.instance.wander = false;
    }

    private void Quit()
    {
        Application.Quit();
    }
}
