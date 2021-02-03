using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    // What it can spawn, and where it will spawn it
    public GameObject unit;
    private int unitPrice;
    public GameObject planet;
    [SerializeField]
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        unitPrice = EconomyManager.SharedInstance.unitCosts[unit.name];
    }

    // Update is called once per frame
    void Update()
    {
        if(planet != gameManager.selectedPlanet)
        {
            planet = gameManager.selectedPlanet;
        }
    }
    
    public void OnButtonPress()
    {
        Debug.Log("Attempting Spawn");
        if (planet != null &&  unitPrice <= ResourceTracker.SharedInstance.materialCount)
        {
            planet.GetComponent<PlanetController>().SpawnUnit(unit);
            ResourceTracker.SharedInstance.unitCount += 1;
            ResourceTracker.SharedInstance.materialCount -= unitPrice;
            Debug.Log(unitPrice);
            ResourceTracker.SharedInstance.UpdateStats();
        }

    }
}
