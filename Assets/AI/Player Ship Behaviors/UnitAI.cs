using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

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
    private enum PlanetCombat
    {
        defense,
        attack
    }
    [SerializeField]
    private PlanetCombat combat = PlanetCombat.defense;

    private enum CombatState
    {
        idle,
        firing,
    }
    private CombatState attacking = CombatState.idle;
    private float orbitalLayer;
    [SerializeField]
    private float attackDistance = 15f;
    private GameObject enemy;
    [SerializeField]
    private float scanDelay = 3f;
    private float timeOfScan;

    // Start is called before the first frame update
    void Start()
    {
        timeOfScan = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (combat == PlanetCombat.attack)
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
            if (state != State.idle && attacking != CombatState.firing)
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

        // Attack if able
        if(attacking == CombatState.firing)
        {
            AttackEnemy();
        }

    }

    // Behaviors

    // Orbit Planet
    void OrbitPlanet()
    {
        Vector3 diff = (planetNode.transform.position - gameObject.transform.position);
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if(combat == PlanetCombat.defense)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("enemy") && Mathf.Abs(timeOfScan - Time.time) >= scanDelay && attacking != CombatState.firing)
        {
            // Attack Enemy If Close Enough
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
            //get 3 closest characters (to referencePos)
            var target = enemies.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
            if (Mathf.Abs(transform.position.magnitude - target.transform.position.magnitude) <= attackDistance)
            {
                attacking = CombatState.firing;
            }
            timeOfScan = Time.time;
        }
       
    }
    void AttackEnemy()
    {
        // Attack Enemy If Close Enough
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        //get closest characters (to referencePos)
        var target = enemies.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).FirstOrDefault();

        if (target && Mathf.Abs(transform.position.magnitude - target.transform.position.magnitude) <= attackDistance)
        {
            // Attack 
            Vector2 dir = target.transform.position - transform.position;
            Debug.Log(dir.magnitude);
            Debug.Log(attacking);
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dir.magnitude);
            Debug.DrawLine(transform.position, target.transform.position, Color.red);
            return;
        }
        else
        {
            attacking = CombatState.idle;
        }
    }

}
