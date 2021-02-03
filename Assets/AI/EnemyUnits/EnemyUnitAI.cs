using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyUnitAI : MonoBehaviour
{

    public GameObject planetNode;
    public LayerMask ableToHit;
    public GameObject projectile;
    public string enemyTag = "unit";
    public float hitBoxRadius = 0;
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
    public float attackDelay = 0.3f;
    private float timeSinceAttack;
    public float attackDamage = 1f;
    private GameObject enemy;
    [SerializeField]
    private float scanDelay = 3f;
    private float timeOfScan;
    public float health = 4f;

    // Start is called before the first frame update
    void Start()
    {
        timeOfScan = Time.time;
        timeSinceAttack = Time.time;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (planetNode.GetComponent<PlanetController>().state == PlanetController.State.friendly)
        {
            combat = PlanetCombat.attack;
        }
        else
        {
            combat = PlanetCombat.defense;
        }
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
                if (planetNode.GetComponent<PlanetController>().state == PlanetController.State.empty)
                {
                    planetNode.GetComponent<PlanetController>().state = PlanetController.State.enemy;
                }
                state = State.idle;
            }

        }
        else if (diffMag < orbitalLayer)
        {
            state = State.away;
            MoveToOrbit();
        }
        else
        {
            state = State.to;
            MoveToOrbit();
        }
        // Scan For Enemies
        // Attack Enemy If Close Enough
        if (Time.time - timeOfScan > scanDelay)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
            //get 3 closest characters (to referencePos)
            var target = enemies.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
            if (Mathf.Abs(transform.position.magnitude - target.transform.position.magnitude) <= attackDistance)
            {
                attacking = CombatState.firing;
            }
            timeOfScan = Time.time;
        }

        // Attack if able
        if (attacking == CombatState.firing)
        {
            AttackEnemy();
        }

        // Death
        if (health <= 0)
        {
            Death();
        }

    }

    // Behaviors

    void Death()
    {
        Destroy(gameObject);
    }

    // Orbit Planet
    void OrbitPlanet()
    {
        Vector3 diff = (planetNode.transform.position - gameObject.transform.position);
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        if (combat == PlanetCombat.defense)
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z);
        }
        else
        {
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 180f);
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
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90f);
            gameObject.GetComponent<Rigidbody2D>().velocity = transform.up.normalized;
        }
        else if (state == State.away)
        {
            Vector3 diff = (planetNode.transform.position - gameObject.transform.position);
            float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            gameObject.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 90);
            gameObject.GetComponent<Rigidbody2D>().velocity = transform.up.normalized;
        }
    }
    // Attack Nearby Enemies in Sight

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.IsTouching(gameObject.GetComponent<CircleCollider2D>()) && collision.CompareTag("fighterbullet"))
        { 
            health -= collision.GetComponent<ProjectileLogic>().attackDamage;
            collision.gameObject.SetActive(false);
        }

    }
    void AttackEnemy()
    {
        // Attack Enemy If Close Enough
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        //get closest characters (to referencePos)
        float varySelection = Random.value;
        GameObject target;
        if (varySelection > 0.6)
        {
            target = enemies.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).FirstOrDefault();
        }
        else
        {
            var targetQuery = enemies.OrderBy(t => (t.transform.position - transform.position).sqrMagnitude).Take(5).ToArray();
            target = targetQuery[Random.Range(0, targetQuery.Length)];
        }


        if (target && Mathf.Abs(transform.position.magnitude - target.transform.position.magnitude) <= attackDistance)
        {
            // Attack 
            if (Mathf.Abs(timeSinceAttack - Time.time) < attackDelay + Random.Range(0, attackDelay/10))
            {
                return;
            }
            Vector2 dir = target.transform.position - transform.position;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, dir.magnitude, ableToHit);

            if (hit.collider != null && hit.collider.CompareTag(enemyTag))
            {
                FireEffect(hit.collider.gameObject);
                //Debug.DrawLine(transform.position, target.transform.position, Color.red);
                //hit.collider.gameObject.GetComponent<EnemyUnitAI>().health -= attackDamage;
            }
            timeSinceAttack = Time.time;

            return;
        }
        else
        {
            attacking = CombatState.idle;
        }
    }

    void FireEffect(GameObject target)
    {
        Vector3 diff = (transform.position - target.transform.position);
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        GameObject bullet = ObjectPooler.SharedInstance.GetPooledObject("enemybullet");
        if (bullet != null)
        {
            bullet.transform.position = transform.position + (-diff.normalized);
            bullet.transform.rotation = Quaternion.Euler(0f, 0f, rot_z + 180);
            bullet.SetActive(true);
        }
        //GameObject bullet = Instantiate(projectile, transform.position + (-diff.normalized), Quaternion.Euler(0f, 0f, rot_z + 180));
        bullet.GetComponent<ProjectileLogic>().target = target;
        bullet.GetComponent<ProjectileLogic>().attackDamage = attackDamage;
    }

}
