using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// \todo delete _attribute used from GameManager.cs
/// \todo reduce dependancies with GameManager
/// \todo rename file into Solver.cs

/// <summary>
/// Keep trace of all the user or mixer rotations to undo them in case of solving.
/// Solver and Mixer.
/// </summary>
public class InputsBuffer3D : MonoBehaviour {
    GameManager3D handler;
    public GameObject rotationEngine;
    private bool solving;
    private bool mixing;
    public int st;
    public List<List<object>> inputsBuffer3D = new List<List<object>>(0);
    public List<List<object>> mixed = new List<List<object>>(0);
    // Start is called before the first frame update
    void Start() {
        rotationEngine = GameObject.Find("PuzzleGenerator3D");
        handler = rotationEngine.GetComponent<GameManager3D>();
        Scrambler(50);
        // inputsBuffer3D = mixed;
        StartCoroutine(RotationHandler());
        // solving = true;
    }

    /// <summary>
    /// Generates a 50 long sequence of rotations.
    /// To be injected next in the inputBuffer.
    /// </summary>
    public void Scrambler(int it) {
        /// \todo it is not currently working with the new inputBuffer system (a selected sticker is needed).
        int axis1 = 0;
        int axis2 = 1;
        Coords3D selection;
        GameObject p = GameObject.Find("Puzzle");
        System.Random rnd = new System.Random();
        for (int cmp = 0 ; cmp < it ; cmp++) {
            int tmp = rnd.Next(0,8);
            selection = p.transform.GetChild(tmp).gameObject.GetComponent<Coords3D>();
            List<string> possibleRotations = UserInputs3D.PossibleRotation(selection);
            string rotation = possibleRotations[rnd.Next(0, possibleRotations.Count)];
            axis1 = Geometry3D.CharToInt(rotation[0]);
            axis2 = Geometry3D.CharToInt(rotation[1]);
            mixed.Add(new List<object>() { axis1, axis2, selection });
        }
    }
    /// <summary>
    /// Injects a single command in the GameManager.
    /// </summary>
    /// <param name="command">A command is a set of three things : two axis to set a rotation and a selectSticker to set the selection.</param>
    public void InjectInput(in List<object> command) {
        //debugLength(commands); /// \todo
        handler.axis1 = (int)command[0];
        handler.axis2 = (int)command[1];
        handler.SetSelection((Coords3D)command[2]);
    }

    /// <summary>
    /// Used as a coroutine, it listens wether some chunk of commands are to be injected or not.
    /// If this is the case, for each of the commands it will launch the matching animation and update the sticker positions.
    /// The rotation speed is increased not to make the user leave from boredom.
    /// </summary>
    /// <returns>As I recall, nothing eheh.</returns>
    private IEnumerator RotationHandler() {
        /// \todo try to delete this function? just change the rotationSpeed to 6.
        while (true) {
            if (!solving && !mixing) {
                yield return null;
            }
            else {
                /// \todo need reajustement
                List<List<object>> mixBuffer = new List<List<object>>();
                for(int i = inputsBuffer3D.Count-1 ; i > st-1 ; i--) {
                    InjectInput(inputsBuffer3D[i]);
                    if(mixing == true){
                        mixBuffer.Add(new List<object>(){inputsBuffer3D[i][1],inputsBuffer3D[i][0],inputsBuffer3D[i][2]});
                    }
                    float totalRotation = 0;
                    List<List<Vector3>> targets = Animation.DefineTargets(handler.p, handler.selectedElement, Geometry3D.IntToAxis(handler.axis1), Geometry3D.IntToAxis(handler.axis2));
                    List<List<bool>> toBeRotated = handler.p.whosGunnaRotate(handler.selectedElement);
                    if (Geometry3D.IsBetweenRangeExcluded(handler.rotationSpeed, 0f, 90f)) {
                        while (Mathf.Abs(90f - totalRotation) > Mathf.Epsilon) {
                            totalRotation = Animation.RotateOverTime(handler.p, handler.puzzle, totalRotation, toBeRotated, Geometry3D.IntToAxis(handler.axis1), Geometry3D.IntToAxis(handler.axis2));
                            yield return null;
                        }
                    }
                    Animation.SnapToTargets(handler.p, handler.puzzle, targets, toBeRotated);
                }
                Animation.SetRotationSpeed(2f);
                if(mixing == true ){
                    inputsBuffer3D.RemoveRange(st,mixed.Count);
                    inputsBuffer3D.AddRange(mixBuffer);
                    mixing = false;
                }
                if(solving == true){
                    inputsBuffer3D.Clear();
                    solving = false;
                }

                
            }
        }
    }
    /// <summary>
    /// Getter of the solving flag.
    /// </summary>
    /// <returns>A bool : if true, animations are still running
    ///                   if false, well we good to go.</returns>
    public bool GetsolvingFlag() {
        return solving;
    }

    public bool GetMixingFlag() {
        return mixing;
    }

    public void SetSolvingFlag(bool b) {
        solving = b;
    }

    public void SetMixingFlag(bool b) {
        mixing = b;
    }
}
