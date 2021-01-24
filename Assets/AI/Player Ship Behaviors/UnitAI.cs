using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitAI : MonoBehaviour
{

    public GameObject planetNode;

    // States for Moving
    private enum State
    {
        away,
        to,
        idle,
    }
    private State state;
    // States for Combat
    private enum Combat
    {
        defense,
        attack
    }
    [SerializeField]
    private Combat combat = Combat.defense;

    private float orbitalLayer;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (combat == Combat.attack)
        {
            orbitalLayer = 7f;
        }
        else
        {
            orbitalLayer = 5.3f;
        }
        Vector3 diff = (transform.position - planetNode.transform.position);
        float diffMag = Mathf.Abs(diff.magnitude);
        if (diffMag >= orbitalLayer && diffMag < orbitalLayer + 0.3f)
        {
            OrbitPlanet();
            if (state != State.idle)
            {
                if(planetNode.GetComponent<PlanetController>().state == PlanetController.State.empty)
                {
                    planetNode.GetComponent<PlanetController>().state = PlanetController.State.friendly;
                }
                state = State.idle;
            }
            
        }
        else if(diffMag < orbitalLayer)
        {
            state = State.away;
            MoveToOrbit();
        }
        else
        {
            state = State.to;
            MoveToOrbit();
        }

    }

    // Behaviors

    // Orbit Planet
    void OrbitPlanet()
    {
        Vector3 diff = (planetNode.transform.position - gameObject.transform.position);
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if(combat == Combat.defense)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z+180f);
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = transform.up.normalized;
    }

    // Move to Orbit
    void MoveToOrbit()
    {
        if (state == State.to)
        {
            Vector3 diff = (planetNode.transform.position - gameObject.transform.position);
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z-90f);
            gameObject.GetComponent<Rigidbody2D>().velocity = transform.up.normalized;
        }
        else if(state == State.away)
        {
            Vector3 diff = (planetNode.transform.position - gameObject.transform.position);
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z+90);
            gameObject.GetComponent<Rigidbody2D>().velocity = transform.up.normalized;
        }
    }
    // Attack Nearby Enemies in Sight

    // Change Planets

}
