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
    /// <summary>
    /// Toggle the selection via the command board.
    /// </summary>
    /// <param name="selected">the selected cell by the user by his click on the corresponding button.</param>
    /*public void changeSelection(GameObject selected) {
        Debug.Log(selected.name);
        if (selected.name == "Right") {
            handler.SetterSelection(GameObject.Find("Right_0").GetComponent<SelectSticker>());
        }
        if (selected.name == "Left") {
            handler.SetterSelection(GameObject.Find("Left_0").GetComponent<SelectSticker>());
        }
        if (selected.name == "Up") {
            handler.SetterSelection(GameObject.Find("Up_0").GetComponent<SelectSticker>());
        }
        if (selected.name == "Down") {
            handler.SetterSelection(GameObject.Find("Down_0").GetComponent<SelectSticker>());
        }
        if (selected.name == "Back") {
            handler.SetterSelection(GameObject.Find("Back_0").GetComponent<SelectSticker>());
        }
        if (selected.name == "Front") {
            handler.SetterSelection(GameObject.Find("Front_0").GetComponent<SelectSticker>());
        }
        if (selected.name == "Inside") {
            handler.SetterSelection(GameObject.Find("Inside_0").GetComponent<SelectSticker>());
        }
        if (selected.name == "Outside") {
            handler.SetterSelection(GameObject.Find("Outside_0").GetComponent<SelectSticker>());
        }
    }*/

    /// <summary>
    /// Lanch the selected rotation onClick on the bounded buttons of the command board.
    /// </summary>
    /// <param name="selected">The rotation plane selected by the user.</param>
    public void ApplyRotation(GameObject selected) { // TODO maybe a way to not use param?
        // Extract axis
        if (!handler.GetRotateFlag() & !buffer.GetMixingFlag() & !buffer.GetsolvingFlag()) {
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

    public void ToggleCommandBoard() {
        GameObject panel = transform.GetChild(0).gameObject;
        panel.SetActive(!panel.activeSelf);
    }

    public void UnselectSticker() {
        handler.SetterSelection(null);
    }

    public void ToggleSelectMode() {
        GameObject puzzle_UI = GameObject.Find("Puzzle_UI");
        foreach (Transform cell in handler.puzzle.transform) {
            cell.GetComponent<MeshCollider>().enabled = !cell.GetComponent<MeshCollider>().enabled;
            cell.GetComponent<Renderer>().enabled = false;
            foreach (Transform sticker in cell) {
                sticker.GetComponent<MeshCollider>().enabled = !sticker.GetComponent<MeshCollider>().enabled;
                sticker.GetComponent<Renderer>().material.color = sticker.GetComponent<SelectSticker>().GetBaseColor();
            }
        }
        foreach (Transform cell in puzzle_UI.transform) {
            cell.GetComponent<MeshCollider>().enabled = !cell.GetComponent<MeshCollider>().enabled;
            cell.GetComponent<Renderer>().enabled = false;
            foreach (Transform sticker in cell) {
                sticker.GetComponent<MeshCollider>().enabled = !sticker.GetComponent<MeshCollider>().enabled;
                sticker.GetComponent<Renderer>().material.color = sticker.GetComponent<SelectSticker>().GetBaseColor();
            }
        }
        handler.SetterSelection(null);
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
