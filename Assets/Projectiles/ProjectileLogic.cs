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
    
    // Update is called once per frame
    void Update()
    {
        transform.Translate((Vector3.right) * Time.deltaTime * moveSpeed);
        if (Mathf.Abs((transform.position - target.transform.position).magnitude) <= target.GetComponent<EnemyUnitAI>().hitBoxRadius)
        {
            target.GetComponent<EnemyUnitAI>().health -= attackDamage;
            Destroy(gameObject);
        }
        if (!fired)
        {
            Destroy(gameObject, timeToLive);
            fired = true;
        }
    }

}
