using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager SharedInstance;
    // Unit Pricing
    public Dictionary<string, int> unitCosts = new Dictionary<string, int>();
    public int numberOfPlanets = 1;
    public float payDelay = 1f;
    private float timeSincePay = 0f;
    public int basePay = 1;

    private void Awake()
    {
        SharedInstance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        unitCosts.Add("Fighter1", 1);
    }

    // Update is called once per frame
    void Update()
    {
        if(numberOfPlanets != ResourceTracker.SharedInstance.planetCount)
        {
            numberOfPlanets = ResourceTracker.SharedInstance.planetCount;
        }

        if (Time.time - timeSincePay >= payDelay)
        {
            ResourceTracker.SharedInstance.materialCount += (basePay * numberOfPlanets);
            ResourceTracker.SharedInstance.UpdateStats();
            timeSincePay = Time.time;
        }
    }
}
