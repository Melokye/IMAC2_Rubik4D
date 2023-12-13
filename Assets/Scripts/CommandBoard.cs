using System.Collections.Generic;
using UnityEngine;

public enum Axis { x, y, z, w, none }

public class CommandBoard: MonoBehaviour {
    GameManager handler;
    bool clockwise = true;

    // Start is called before the first frame update
    void Start() {
        // Connect the handler with the game manager
        GameObject tmp = GameObject.Find("PuzzleGenerator");
        handler = tmp.GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update() {}
    
    public Axis GiveAxis(char axis) {
        switch(axis) {
            case 'X': return Axis.x;
            case 'Y': return Axis.y;
            case 'Z': return Axis.z;
            case 'W': return Axis.w;

            // TODO normally we can't access this line but just in case...
            default: Debug.Log(axis + " isn't defined"); return Axis.none; 
        }
    }

    public void FindRotation(GameObject sticker) {
        Debug.Log(sticker.name);
        foreach (string needRotate in handler.whosGunnaRotate(sticker.name)) {
            Debug.Log(needRotate);
        }

        // TODO
    }

    public void ApplyRotation(GameObject selected) { // TODO maybe a way to not use param?
        // Extract axis
        List<int> axis = new List<int>();
        foreach (char letter in selected.name) {
            axis.Add((int) GiveAxis(letter));
        }
        
        // Insert axis in the GameManager
        if (clockwise) {
            handler.SetPlane(axis[0], axis[1]);
        }
        else {
            handler.SetPlane(axis[1], axis[0]);
        }

        // TODO check if a "zone" has been selected before
        handler.LaunchRotation();
    }

    public void ChangeClock() {
        clockwise = !clockwise;
    }
}
