using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLogic : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed = 20f;
    public float attackDamage = 0;
    private bool fired = false;
    public float timeToLive = 5f;
    private float targetRadius = 0;


    private void Start()
    {
        if (target.CompareTag("unit"))
        {
            targetRadius = target.GetComponent<UnitAI>().hitBoxRadius;
        }
        else if (target.CompareTag("enemy"))
        {
           targetRadius = target.GetComponent<EnemyUnitAI>().hitBoxRadius;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate((Vector3.right) * Time.deltaTime * moveSpeed);
        if (Mathf.Abs((transform.position - target.transform.position).magnitude) <= targetRadius)
        {
            if (target.CompareTag("unit"))
            {
                target.GetComponent<UnitAI>().health -= attackDamage;
            }
            else if (target.CompareTag("enemy"))
            {
                target.GetComponent<EnemyUnitAI>().health -= attackDamage;
            }
            
            Destroy(gameObject);
        }
        if (!fired)
        {
            Destroy(gameObject, timeToLive);
            fired = true;
        }
    }

}
