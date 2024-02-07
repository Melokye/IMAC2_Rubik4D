using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

        // Listen for size slider
        GameObject cameraCanvas = GameObject.Find("CameraCanvas");
        Slider cameraCanvasSize = GameObject.Find("CameraCanvasSize").GetComponent<Slider>();
        cameraCanvasSize.onValueChanged.AddListener((value) => {
            cameraCanvas.GetComponent<RectTransform>().localScale = new Vector3(0.5625f, 1f, 1f) * value;
        });
    }

    /// <summary>
    /// Toggle the selection via the command board.
    /// </summary>
    /// <param name="selected">the selected cell by the user by his click on the corresponding button.</param>
    // public void changeSelection(GameObject selected) {
    //     handler.SetterSelection(GameObject.Find(selected.name + "_0").GetComponent<SelectSticker>());
    // }

    /// <summary>
    /// Launch the selected rotation onClick on the bounded buttons of the command board.
    /// </summary>
    /// <param name="selected">The rotation plane selected by the user.</param>
    public void ApplyRotation(GameObject selected) { /// \todo maybe a way to not use param?
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

    /// <summary>
    /// Toggles visibility of the CommandBoard.
    /// Used by a button's trigger method
    /// </summary>
    public void ToggleCommandBoard() {
        GameObject panel = transform.GetChild(0).gameObject;
        panel.SetActive(!panel.activeSelf);
    }

    /// <summary>
    /// Sets the currently selected object to Idle state, and sets selection to null
    /// Used by a button's trigger method
    /// </summary>
    public void UnselectSticker() {
        if (handler.GetSelection() != null) {
            if (handler.GetSelection().GetComponent<SelectCell>() == null) {
                handler.GetSelection().GetComponent<SelectSticker>().SetState(SelectSticker.State.Idle);
            }
            else {
                handler.GetSelection().GetComponent<SelectCell>().SetState(SelectCell.State.Idle);
            }
            handler.SetSelection(null);
        }
    }

    /// <summary>
    /// Toggles the mouse hover detection on the stickers on the cells,
    /// and unselects the currently selected sticker
    /// Used by a button's trigger method
    /// </summary>
    public void ToggleSelectMode() {
        GameObject puzzle_UI = GameObject.Find("Puzzle_UI");
        foreach (Transform cell in handler.puzzle.transform) {
            cell.GetComponent<MeshCollider>().enabled = !cell.GetComponent<MeshCollider>().enabled;
            foreach (Transform sticker in cell) {
                sticker.GetComponent<MeshCollider>().enabled = !sticker.GetComponent<MeshCollider>().enabled;
            }
        }
        foreach (Transform cell in puzzle_UI.transform) {
            cell.GetComponent<MeshCollider>().enabled = !cell.GetComponent<MeshCollider>().enabled;
            foreach (Transform sticker in cell) {
                sticker.GetComponent<MeshCollider>().enabled = !sticker.GetComponent<MeshCollider>().enabled;
            }
        }
        GameObject selectModeToggle = GameObject.Find("SelectMode");
        Transform textObject = selectModeToggle.transform.Find("Text (TMP)");
        if (textObject.GetComponent<TMPro.TextMeshProUGUI>().text == "Sticker<br>Select Mode") {
            textObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Cell<br>Select Mode";
        }
        else {
            textObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Sticker<br>Select Mode";
        }
        UnselectSticker();
    }

    public void ChangeClock() {
        GameObject clockToggle = GameObject.Find("Clockwise");
        Transform textObject = clockToggle.transform.Find("Text (TMP)");
        if (clockwise) {
            textObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Counter-<br>Clockwise";
        }
        else {
            textObject.GetComponent<TMPro.TextMeshProUGUI>().text = "Clockwise";
        }
        clockwise = !clockwise;
    }
    /// \todo Note for handling the 1-layer rotation:
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
