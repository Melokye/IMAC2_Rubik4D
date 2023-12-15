using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CommandBoard : MonoBehaviour {
    GameManager handler;
    InputsBuffer buffer;
    bool clockwise = true;
    List<Vector2> AllRotations = new List<Vector2>(){new Vector2(1,2), new Vector2(0,2), 
                                                     new Vector2(0,1), new Vector2(0,3), 
                                                     new Vector2(1,3), new Vector2(2,3),
                                                     
                                                     new Vector2(2,1), new Vector2(2,0), 
                                                     new Vector2(1,0), new Vector2(3,0), 
                                                     new Vector2(3,1), new Vector2(3,2),};

    // Start is called before the first frame update
    void Start() {
        // Connect the handler with the game manager
        GameObject tmp = GameObject.Find("PuzzleGenerator");
        handler = tmp.GetComponent<GameManager>();

        tmp = GameObject.Find("TrivialSolver");
        buffer = tmp.GetComponent<InputsBuffer>();
    }

    // Update is called once per frame
    void Update() { 
        if(handler.GetSelection()!=null) {
            List<string> rotations = PossibleRotation();
            for(int i = 0 ; i < 6 ; i++){
                Transform child = transform.GetChild(i);
                if(  child.name == rotations[0] | child.name == rotations[1] | child.name == rotations[2]
                   | child.name == rotations[3] | child.name == rotations[4] | child.name == rotations[5]){
                    child.GetComponent<Button>().interactable = true;
                }
                else {
                    child.GetComponent<Button>().interactable = false;
                }
            }
        }
    }

    public List<string> PossibleRotation() {
        List<Vector2> pR = new List<Vector2>(0);;
        List<string> nameOfRotations = new List<string>();
        int usefulIndex=0;
        for(int i = 0 ; i < 4 ; i++){
            if(Mathf.Abs(handler.GetSelection().GetCoordinates()[i])==1){
                usefulIndex = i ;
            }
        }
        for(int i = 0 ; i < AllRotations.Count ; i++){
            if(AllRotations[i][0] != usefulIndex & AllRotations[i][1] != usefulIndex){
                pR.Add(AllRotations[i]);
            }
        }
        for(int i = 0 ; i < pR.Count ; i++){
            nameOfRotations.Add(new string(new char[]{Geometry.IntToChar((int)pR[i][0]),Geometry.IntToChar((int)pR[i][1])}));
        }
        return nameOfRotations;
    }



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
