using UnityEngine;
using System.Collections;

// source : https://docs.unity3d.com/2022.3/Documentation/Manual/gui-Basics.html

public class Menu : MonoBehaviour {
    private int state = 0;
    private int width = 200;
    private int padding = 10;

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
        newWindow();
    
        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if(GUI.Button(new Rect(padding, 60, width - padding * 2, 20), "Play")) // TODO automate the values
        {
            Application.LoadLevel(1); // TODO to remplace
        }
    
        if(GUI.Button(new Rect(padding, 90, width - padding * 2, 20), "Credit")) 
            return 1; // TODO need ajustement ?
        
        closeWindow();
        return 0;
    }

    int credit(){
        newWindow();
    
        GUI.Box(new Rect(padding, 60, width - padding * 2, 20), "Tutors");
        GUI.Box(new Rect(padding, 90, width - padding * 2, 20), "Vincent NOZICK");
        // TODO complete the credit
        // TODO animate ?

        if(GUI.Button(new Rect(padding, 120, width - padding * 2, 20), "Return"))
            return 0;

        closeWindow();
        return 1;
    }

    void newWindow(){ // TODO param nb button to display ?
        // center the box
        GUI.BeginGroup (new Rect (Screen.width / 2 - width / 2, Screen.height / 2 - 50, width, 200));

        // Make a background box
        GUI.Box(new Rect(0, 0, width, 150), "");

        // title
        GUI.Box(new Rect(0,20,width,30), "Rubik4D");
    }

    void closeWindow(){
        GUI.EndGroup();
    }
}
