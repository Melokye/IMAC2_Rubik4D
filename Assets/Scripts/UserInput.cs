using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// The script that computes all possible rotation.
/// </summary>
public class UserInput : MonoBehaviour {

    InputsBuffer buffer;
    GameManager handler;
    GameObject axis;
    GameObject unselect;
    GameObject scramble;
    GameObject solve;
    static List<Vector2> AllRotations = new List<Vector2>(){new Vector2(1,2), new Vector2(0,2),
                                                            new Vector2(0,1), new Vector2(0,3),
                                                            new Vector2(1,3), new Vector2(2,3),

                                                            new Vector2(2,1), new Vector2(2,0),
                                                            new Vector2(1,0), new Vector2(3,0),
                                                            new Vector2(3,1), new Vector2(3,2),};

    List<string> nameOfRotation;


    // Start is called before the first frame update
    void Start() {
        handler = GetComponent<GameManager>();
        buffer = GameObject.Find("TrivialSolver").GetComponent<InputsBuffer>();
        axis = GameObject.Find("Axis");
        unselect = GameObject.Find("Unselect");
        scramble = GameObject.Find("Scramble");
        solve = GameObject.Find("Solve");
    }

    // Update is called once per frame
    void Update() {
        setRotationsActive();
        if (handler.GetSelection() != null) {
            ApplyRotation();
        }
        if (Input.GetKeyDown(KeyCode.S) && !buffer.GetMixingFlag() && !buffer.GetsolvingFlag() && !handler.GetRotateFlag()) {
            Animation.SetRotationSpeed(6f);
            buffer.st = 0;
            buffer.SetSolvingFlag(true);
        }
        if (Input.GetKeyDown(KeyCode.M) && !buffer.GetMixingFlag() && !buffer.GetsolvingFlag() && !handler.GetRotateFlag()) {
            buffer.st = buffer.inputsBuffer.Count;
            buffer.inputsBuffer.AddRange(buffer.mixed);
            buffer.mixed.Clear();
            buffer.Scrambler(50);
            Animation.SetRotationSpeed(6f) ;
            buffer.SetMixingFlag(true);
        }
        // For each camera in the scene, toggle both relevant culling masks
        if (Input.GetKeyDown(KeyCode.P)) {
            GameObject circleContainer = GameObject.Find("CircleContainer");
            GameObject circleContainer_UI = GameObject.Find("CircleContainer_UI");
            GameObject puzzle_UI = GameObject.Find("Puzzle_UI");
            handler.SetLayerAllChildren(circleContainer.transform,
                (circleContainer.layer + 3) % 6);
            handler.SetLayerAllChildren(circleContainer_UI.transform,
                (circleContainer_UI.layer + 3) % 6);
            handler.SetLayerDirectChildrenNoRoot(handler.puzzle.transform,
                (handler.puzzle.transform.GetChild(0).gameObject.layer + 3) % 6);
            handler.SetLayerDirectChildrenNoRoot(puzzle_UI.transform,
                (handler.puzzle.transform.GetChild(0).gameObject.layer + 3) % 6);
            handler.ChangeProjection();
        }
    }
    /// <summary>
    /// With a selected sticker selected by the user, computes all the 6 rotations possible (3 trigonometric and 3 antitrigonometric).
    /// </summary>
    /// <returns> A list of the possible rotations. They are string in the form "XY", "WZ", etc...</returns>
    public List<string> PossibleRotation() {
        List<string> nameOfRotations = new List<string>();
        if (handler.GetSelection() != null) {
            List<Vector2> pR = new List<Vector2>(0); ;

            int usefulIndex = 0;
            for (int i = 0; i < 4; i++) {
                if (Mathf.Abs(handler.GetSelection().GetCoordinates()[i]) == 1) {
                    usefulIndex = i;
                }
            }
            for (int i = 0; i < AllRotations.Count; i++) {
                if (AllRotations[i][0] != usefulIndex & AllRotations[i][1] != usefulIndex) {
                    pR.Add(AllRotations[i]);
                }
            }
            for (int i = 0; i < pR.Count; i++) {
                nameOfRotations.Add(new string(new char[] { Geometry.IntToChar((int)pR[i][0]), Geometry.IntToChar((int)pR[i][1]) }));
            }
        }
        return nameOfRotations;
    }
    public static List<string> PossibleRotation(Coords4D selected) {
        List<string> nameOfRotations = new List<string>();

        List<Vector2> pR = new List<Vector2>(0); ;

        int usefulIndex = 0;
        for (int i = 0; i < 4; i++) {
            if (Mathf.Abs(selected.GetCoordinates()[i]) == 1) {
                usefulIndex = i;
            }
        }
        for (int i = 0; i < AllRotations.Count; i++) {
            if (UserInput.AllRotations[i][0] != usefulIndex & UserInput.AllRotations[i][1] != usefulIndex) {
                pR.Add(UserInput.AllRotations[i]);
            }
        }
        for (int i = 0; i < pR.Count; i++) {
            nameOfRotations.Add(new string(new char[] { Geometry.IntToChar((int)pR[i][0]), Geometry.IntToChar((int)pR[i][1]) }));
        }

        return nameOfRotations;
    }

    /// <summary>
    /// Toggles the possible rotations within the UI. 
    /// </summary>
    private void setRotationsActive() {
        if (handler.GetSelection() != null) {
            unselect.GetComponent<Button>().interactable = true;
            List<string> rotations = PossibleRotation();
            for (int i = 0; i < 6; i++) {
                Transform child = axis.transform.GetChild(i);
                if (child.name == rotations[0] | child.name == rotations[1] | child.name == rotations[2]
                | child.name == rotations[3] | child.name == rotations[4] | child.name == rotations[5]) {
                    child.GetComponent<Button>().interactable = true;
                    child.GetComponent<Image>().enabled = true;
                }
                else {
                    child.GetComponent<Button>().interactable = false;
                    child.GetComponent<Image>().enabled = false;
                }
            }
        }
        else {
            unselect.GetComponent<Button>().interactable = false;
            for (int i = 0; i < 6; i++) {
                Transform child = axis.transform.GetChild(i);
                child.GetComponent<Button>().interactable = true;
                child.GetComponent<Image>().enabled = true;
            }
        }
        if (buffer.GetMixingFlag() || buffer.GetsolvingFlag()) {
            scramble.GetComponent<Button>().interactable = false;
            solve.GetComponent<Button>().interactable = false;
        }
        else {
            scramble.GetComponent<Button>().interactable = true;
            solve.GetComponent<Button>().interactable = true;
        }
    }


    /// <summary>
    /// Launching rotations given certain user input (arrow keys or awd keys).
    /// </summary>
    private void ApplyRotation() {
        nameOfRotation = PossibleRotation();
        int axis1 = 0;
        int axis2 = 1;
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && !handler.GetRotateFlag() && !buffer.GetMixingFlag() && !buffer.GetsolvingFlag()) {
            axis1 = Geometry.CharToInt(nameOfRotation[0][0]);
            axis2 = Geometry.CharToInt(nameOfRotation[0][1]);
            handler.SetPlane(axis1, axis2);
            buffer.inputsBuffer.Add(new List<object>() { axis2, axis1, handler.GetSelection() });
            handler.LaunchRotation();
        }
        if ((Input.GetKeyDown(KeyCode.W) | Input.GetKeyDown(KeyCode.UpArrow)) & !handler.GetRotateFlag() && !buffer.GetMixingFlag() && !buffer.GetsolvingFlag()) {
            axis1 = Geometry.CharToInt(nameOfRotation[1][0]);
            axis2 = Geometry.CharToInt(nameOfRotation[1][1]);

            handler.SetPlane(axis1, axis2);
            buffer.inputsBuffer.Add(new List<object>() { axis2, axis1, handler.GetSelection() });
            handler.LaunchRotation();
        }
        if ((Input.GetKeyDown(KeyCode.D) | Input.GetKeyDown(KeyCode.RightArrow)) & !handler.GetRotateFlag() && !buffer.GetMixingFlag() && !buffer.GetsolvingFlag()) {
            axis1 = Geometry.CharToInt(nameOfRotation[2][0]);
            axis2 = Geometry.CharToInt(nameOfRotation[2][1]);

            handler.SetPlane(axis1, axis2);
            buffer.inputsBuffer.Add(new List<object>() { axis2, axis1, handler.GetSelection() });
            handler.LaunchRotation();
        }
    }
}
