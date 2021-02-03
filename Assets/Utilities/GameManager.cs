using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    // Selected Planet
    public GameObject selectedPlanet;
    // Orbit Heights
    public float defenseLayer = 5.3f;
    public float attackLayer = 7f;

    //Selected Command
    public enum Command
    {
        none,
        move,
    }
    public Command command;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // Set State to Command
    public void MoveCommand()
    {
        if(command != Command.move)
        {
            command = Command.move;
            Color hexColor;
            ColorUtility.TryParseHtmlString("#0AEE28", out hexColor);
            GameObject.Find("CommandButtonMove").GetComponentInChildren<Text>().color = hexColor;
        }
        else
        {
            command = Command.none;
            GameObject.Find("CommandButtonMove").GetComponentInChildren<Text>().color = Color.white;
        }

    }

    public void ClearCommand()
    {
        command = Command.none;
        Transform[] highLevel = GameObject.Find("CommandTab").GetComponentsInChildren<Transform>();
        Text[] childColors = null;
        foreach (Transform t in highLevel)
        {
            if (t.name == "Slider")
            {
                childColors = t.GetComponentsInChildren<Text>();
                break;
            }
        }
        if (childColors != null && childColors.Length > 0)
        {
            foreach(Text child in childColors)
            {
                child.color = Color.white;
            }
        }
        
    }
}
