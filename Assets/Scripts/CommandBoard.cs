using System.Collections.Generic;
using UnityEngine;


public class CommandBoard : MonoBehaviour {
    GameManager handler;
    InputsBuffer buffer;
    bool clockwise = true;


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



    public void ApplyRotation(GameObject selected) { // TODO maybe a way to not use param?
        // Extract axis
        if (!handler.GetRotateFlag() & !buffer.GetInputingFlag()) {
            List<Geometry.Axis> axis = new List<Geometry.Axis>();
            foreach (char letter in selected.name) {
                axis.Add(Geometry.CharToAxis(letter));
            }

            // Insert axis in the GameManager
            if (clockwise) {
                handler.SetPlane(Geometry.AxisToInt(axis[0]), Geometry.AxisToInt(axis[1]));
                buffer.inputsBuffer.Add(new List<object>() { Geometry.AxisToInt(axis[1]), Geometry.AxisToInt(axis[0]), handler.GetSelection() });
            }
            else {
                handler.SetPlane(Geometry.AxisToInt(axis[1]), Geometry.AxisToInt(axis[0]));
                buffer.inputsBuffer.Add(new List<object>() { Geometry.AxisToInt(axis[0]), Geometry.AxisToInt(axis[1]), handler.GetSelection() });
            }

            handler.LaunchRotation();
        }
    }

    public void ChangeClock() {
        clockwise = !clockwise;
    }
    // TODO Note for handling the 1-layer rotation:
    // In 2^4n no matter which sticker you select in the cell, it will always rotate the same thing.
    // Only the layer that is the closest to the cell selected will rotate. 
    // Rotations possible for each cell: 
    // In : x , y , z      : up circles
    // Out : x , y , z     : down circles

    // Up : z , xw , zw    : inner circles
    // Down : z , xw , zw  : outer circles

    // Left : x , yw , zw  : outer circles
    // Right : x , yw , zw : inner circles

    // Front : y , xw , yw : inner circles
    // Back : y , xw , yw  : outer circles
}
