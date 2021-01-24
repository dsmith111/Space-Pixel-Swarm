using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSpawner : MonoBehaviour
{
    // What it can spawn, and where it will spawn it
    public GameObject unit;
    public GameObject planet;
    [SerializeField]
    private GameManager gameManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
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
        if (planet != null)
        {
            planet.GetComponent<PlanetController>().SpawnUnit(unit);
        }

    }
}
