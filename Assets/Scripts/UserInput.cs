using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// The script that computes all possible rotation.
/// </summary>
public class UserInput : MonoBehaviour
{

    InputsBuffer buffer;
    GameManager handler;
    GameObject axis;
    GameObject unselect;
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
    }

    // Update is called once per frame
    void Update() {
        setRotationsActive();
        if (handler.GetSelection() != null) {
            ApplyRotation();
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
                }
                else {
                    child.GetComponent<Button>().interactable = false;
                }
            }
        }
        else {
            unselect.GetComponent<Button>().interactable = false;
            for (int i = 0; i < 6; i++) {
                Transform child = axis.transform.GetChild(i);
                child.GetComponent<Button>().interactable = true;
            }
        }
    }


    /// <summary>
    /// Launching rotations given certain user input (arrow keys or awd keys).
    /// </summary>
    private void ApplyRotation(){
        nameOfRotation = PossibleRotation();
        int axis1 = 0;
        int axis2 = 1;
        if ((Input.GetKeyDown(KeyCode.A) | Input.GetKeyDown(KeyCode.LeftArrow)) & !handler.GetRotateFlag()) {
            axis1 = Geometry.CharToInt(nameOfRotation[0][0]);
            axis2 = Geometry.CharToInt(nameOfRotation[0][1]);
            handler.SetPlane(axis1, axis2);
            buffer.inputsBuffer.Add(new List<object>() { axis2, axis1, handler.GetSelection() });
            handler.LaunchRotation();
        }
        if ((Input.GetKeyDown(KeyCode.W) | Input.GetKeyDown(KeyCode.UpArrow)) & !handler.GetRotateFlag()) {
            axis1 = Geometry.CharToInt(nameOfRotation[1][0]);
            axis2 = Geometry.CharToInt(nameOfRotation[1][1]);

            handler.SetPlane(axis1, axis2);
            buffer.inputsBuffer.Add(new List<object>() { axis2, axis1, handler.GetSelection() });
            handler.LaunchRotation();
        }
        if ((Input.GetKeyDown(KeyCode.D) | Input.GetKeyDown(KeyCode.RightArrow)) & !handler.GetRotateFlag()) {
            axis1 = Geometry.CharToInt(nameOfRotation[2][0]);
            axis2 = Geometry.CharToInt(nameOfRotation[2][1]);

            handler.SetPlane(axis1, axis2);
            buffer.inputsBuffer.Add(new List<object>() { axis2, axis1, handler.GetSelection() });
            handler.LaunchRotation();
        }
    }
}
