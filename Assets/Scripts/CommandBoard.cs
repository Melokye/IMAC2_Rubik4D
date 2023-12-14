using System.Collections.Generic;
using UnityEngine;

public enum Axis { x, y, z, w, none }

public class CommandBoard : MonoBehaviour {
    GameManager handler;
    InputsBuffer buffer;
    bool clockwise = true;

    Ray ray;

    // Start is called before the first frame update
    void Start() {
        // Connect the handler with the game manager
        GameObject tmp = GameObject.Find("PuzzleGenerator");
        handler = tmp.GetComponent<GameManager>();

        tmp = GameObject.Find("TrivialSolver");
        buffer = tmp.GetComponent<InputsBuffer>();

        
    }

    // Update is called once per frame
    void Update() { }

    public Axis GiveAxis(char axis) {
        switch (axis) {
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
        if (!handler.GetRotateFlag() & !buffer.GetInputingFlag()) {
            List<int> axis = new List<int>();
            foreach (char letter in selected.name) {
                axis.Add((int)GiveAxis(letter));
            }

            // Insert axis in the GameManager
            if (clockwise) {
                handler.SetPlane(axis[0], axis[1]);
                buffer.inputsBuffer.Add(new List<int>() { axis[1], axis[0] });
            }
            else {
                handler.SetPlane(axis[1], axis[0]);
                buffer.inputsBuffer.Add(new List<int>() { axis[0], axis[1] });
            }

            // TODO check if a "zone" has been selected before
            handler.LaunchRotation();
        }
    }

    public void ChangeClock() {
        clockwise = !clockwise;
    }
    // TODO Note for handling the 1 layer rotation :
    // In 2^4 no matter which sticker you select in the cell, it will always rotate the same thing.
    // Only the layer that is the closest to the cell selected will rotate. 
    // Rotations possible for each cells : 
    // In : x , y , z      : up circles
    // Out : x , y ,z      : down circles
    
    // Up : z , xw , zw    : inner circles
    // Down : z , xw , zw  : outter circles

    // Left : x , yw , zw  : outter circles
    // Right : x , yw ,zw  : inner circles

    // Front : y , xw , yw : inner circles
    // Back : y , xw , yw  : outter circles
}

