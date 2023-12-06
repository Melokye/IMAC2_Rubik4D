using UnityEngine;
using System.Collections;

// source : https://docs.unity3d.com/2022.3/Documentation/Manual/gui-Basics.html

public class Menu : MonoBehaviour {
    enum MenuType {Main, Credit};
    private MenuType state = MenuType.Main;
    private int width = 200;
    private int padding = 10;

    
    void OnGUI ()
    {
        // TODO hide the rubik4D ?
        switch(state){
            case MenuType.Main : 
                state = mainMenu(); 
                break;
            case MenuType.Credit : 
                state = credit(); 
                break;
            default : 
                state = MenuType.Main;
                break;
        }
    }

    MenuType mainMenu(){
        newWindow();
    
        // Make the first button. If it is pressed, Application.Loadlevel (1) will be executed
        if(GUI.Button(new Rect(padding, 60, width - padding * 2, 20), "Play")) // TODO automate the values
        {
            Application.LoadLevel(1); // TODO to remplace
        }
    
        if(GUI.Button(new Rect(padding, 90, width - padding * 2, 20), "Credit")) 
            return MenuType.Credit;
        
        closeWindow();
        return MenuType.Main;
    }

    MenuType credit(){
        newWindow();
    
        GUI.Box(new Rect(padding, 60, width - padding * 2, 20), "Tutors");
        GUI.Box(new Rect(padding, 90, width - padding * 2, 20), "Vincent NOZICK");
        // TODO complete the credit
        // TODO animate ?

        if(GUI.Button(new Rect(padding, 120, width - padding * 2, 20), "Return"))
            return 0;

        closeWindow();
        return MenuType.Credit;
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
