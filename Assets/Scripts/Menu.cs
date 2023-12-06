using UnityEngine;
using System.Collections;

// source : https://docs.unity3d.com/2022.3/Documentation/Manual/gui-Basics.html

public class Menu : MonoBehaviour {
    public int state = 0; // TODO private ?

    void OnGUI ()
    {
        // TODO hide the rubik4D ?
        switch(state){ // TODO enum ?
            case 0 : 
                state = mainMenu(); 
                break;
            case 1 : 
                state = credit(); 
                break;
            default : 
                state = 0;
                break;
        }
    }

    int mainMenu(){
        // TODO automate resize
        GUI.BeginGroup (new Rect (Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 200));
        
        // Make a background box
        GUI.Box(new Rect(0, 0, 100, 125), "");

        // title
        GUI.Box(new Rect(0,20,100,30), "Rubik4D"); // TODO regroup to a function "title" ?
    
        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if(GUI.Button(new Rect(10,60,80,20), "Play"))
        {
            Application.LoadLevel(1); // TODO to remplace
        }
    
        // Make the second button.
        if(GUI.Button(new Rect(10,90,80,20), "Credit")) 
        {
            return 1; // TODO need ajustement ?
        }

        GUI.EndGroup ();
        return 0;
    }

    int credit(){
        GUI.BeginGroup (new Rect (Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 200));
        
        GUI.Box(new Rect(0, 0, 100, 200), "");
        GUI.Box(new Rect(0,20,100,30), "Rubik4D");
    
        GUI.Box(new Rect(10,60,80,20), "Tutors");
        GUI.Box(new Rect(10,60,80,20), "Vincent NOZICK");
        
        if(GUI.Button(new Rect(10,90,80,20), "Return")) 
            return 0;

        GUI.EndGroup();
        return 1;
    }
}