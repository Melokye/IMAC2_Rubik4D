using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Axis {x, y, z, w, none}

public class CommandBoard : MonoBehaviour
{
    GameManager handler;

    // Start is called before the first frame update
    void Start()
    {
        // connect the handler with the game manager
        GameObject tmp = GameObject.Find("SphereGenerator");
        handler = tmp.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public Axis giveAxis(char axis){
        switch(axis){
            case 'X' : return Axis.x;
            case 'Y' : return Axis.y;
            case 'Z' : return Axis.z;
            case 'W' : return Axis.w;
            default : Debug.Log(axis + " isn't defined"); return Axis.none; // TODO normally we can't acces this line but just in case...
        }
    }

    public void applyRotation(GameObject selected){
        // Extract axis
        List<int> axis = new List<int>();
        foreach(char letter in selected.name){
            axis.Add((int) giveAxis(letter));
        }
        
        // Insert axis in the GameManager
        handler.setPlane(axis[0], axis[1]);

        // TODO check if a "zone" has been selected before
        handler.launchRotation();
    }
}
