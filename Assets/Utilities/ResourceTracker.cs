using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceTracker : MonoBehaviour
{
    public static ResourceTracker SharedInstance;
    public int unitCount = 0;
    public int materialCount = 0;
    public int planetCount = 1;
    private Text materialText;
    private Text unitText;
    private Text planetText;

    private void Awake()
    {
        SharedInstance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        materialText = GameObject.Find("MaterialsNumber").GetComponent<Text>();
        unitText = GameObject.Find("UnitsNumber").GetComponent<Text>();
        planetText = GameObject.Find("PlanetsNumber").GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateStats()
    {
        materialText.text = materialCount.ToString();
        unitText.text = unitCount.ToString();
        planetText.text = planetCount.ToString();
    }
}
