using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class PlanetController : MonoBehaviour
{

    // Planet States
    public enum State
    {
        empty,
        friendly,
        enemy,
        destroyed,
    }
    private GameManager gameManager;
    public State state = State.empty;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Select Planet
    private void OnMouseDown()
    {
        if(gameManager.command == GameManager.Command.move)
        {
            GameObject[] myUnits =  GameObject.FindGameObjectsWithTag("unit");
            //get 3 closest characters (to referencePos)
            var samePlanetUnits = myUnits.Where(t => (t.GetComponent<UnitAI>().planetNode == gameManager.selectedPlanet));
            var nClosest = samePlanetUnits.OrderBy(t => (t.transform.position - gameManager.selectedPlanet.transform.position).sqrMagnitude)
                                       .Take(10)   //or use .FirstOrDefault();  if you need just one
                                       .ToArray();

            //Remove Ships that are not on this planet
            
            foreach (var ship in nClosest)
            {
                if(ship.GetComponent<UnitAI>().planetNode == gameManager.selectedPlanet)
                {
                    ship.GetComponent<UnitAI>().planetNode = gameObject;
                }
            }
            gameManager.ClearCommand();
            return;
        }
        if(state == State.friendly)
        {
            gameManager.GetComponent<GameManager>().selectedPlanet = gameObject;
            return;
        }

    }

    // Spawn Unit
    public void SpawnUnit(GameObject unit)
    {
        // Random Launch Point
        Vector3 launchPointPos = transform.position;
        Vector3 launchPointDir = transform.up;
        launchPointPos += new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), 0f).normalized * 3.531668f;
        launchPointDir -= transform.position;
        Quaternion launchPointRot = Quaternion.Euler(launchPointDir.x, launchPointDir.y, launchPointDir.z);
        GameObject newUnit = Instantiate(unit, launchPointPos, launchPointRot);
        newUnit.GetComponent<UnitAI>().planetNode = gameObject;
    }
    
}
