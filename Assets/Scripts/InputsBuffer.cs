using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO delete _attribute used from GameManager.cs
// TODO reduce dependancies with GameManager
// TODO rename file into Solver.cs

/// <summary>
/// Keep trace of all the user or mixer rotations to undo them in case of solving.
/// Solver and Mixer.
/// </summary>
public class InputsBuffer: MonoBehaviour {
    GameManager handler;
    public GameObject rotationEngine;
    private bool inputing;
    public List<List<object>> inputsBuffer = new List<List<object>>(0);
    List<List<object>> mixed = new List<List<object>>(0);
    // Start is called before the first frame update
    void Start() {
        rotationEngine = GameObject.Find("PuzzleGenerator");
        handler = rotationEngine.GetComponent<GameManager>();
        Scrambler();
        // inputsBuffer = mixed;
        StartCoroutine(RotationHandler());
        // inputing = true;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            List<object> Entry = new List<object>() { handler.GetAxis2(), handler.GetAxis1(), handler.GetSelection() };
            inputsBuffer.Add(Entry);
            // debugLength(inputsBuffer); // TODO
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            /*un code qui permet d'executer pleins de rotations d'un coup*/
            inputsBuffer.Clear();
            inputsBuffer.AddRange(mixed);
            mixed.Clear();
            Scrambler();
            Animation.SetRotationSpeed(6f) ;
            inputing = true;
            // Debug.Log("Done mixing"); // TODO
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            /*un code qui permet d'executer pleins de rotations d'un coup*/
            Animation.SetRotationSpeed(6f) ;
            inputing = true;
        }
    }
    
    /// <summary>
    /// Generates a 50 long sequence of rotations.
    /// To be injected next in the inputBuffer.
    /// </summary>
    public void Scrambler() {
        // TODO it is not currently working with the new inputBuffer system (a selected sticker is needed).
        int axis1 = 0;
        int axis2 = 1;
        int selection = 0; 
        System.Random rnd = new System.Random();
        for (int cmp = 0 ; cmp < 50 ; cmp++) {
            axis1 = rnd.Next(0,4);
            while(axis2==axis1)axis2 = rnd.Next(0,4);
            List<int> l = new List<int>(0);
            for(int i = 0 ; i < 4 ; i++){
                if(i!=axis1 && i!=axis2) {
                    l.Add(-i);
                    l.Add(i);
                }
            }
            selection = l[rnd.Next(0,l.Count)];
            Debug.Log(selection);
            Debug.Log(axis1);
            Debug.Log(axis2);
            GameObject s = GameObject.Find("Puzzle");
            for(int i = 0 ; i < 8 ; i ++){
                SelectSticker obj = s.transform.GetChild(i).GetChild(0).gameObject.GetComponent<SelectSticker>();
                
                if(obj.GetCoordinates()[Mathf.Abs(selection)] == Mathf.Sign(selection)){
                    Debug.Log(obj);
                    mixed.Add(new List<object>(){axis1,axis2,obj});
                    break;
                }
            }            
        }
    }
    /// <summary>
    /// Injects a single command in the GameManager.
    /// </summary>
    /// <param name="command">A command is a set of three things : two axis to set a rotation and a selectSticker to set the selection.</param>
    public void InjectInput(in List<object> command) {
        //debugLength(commands); // TODO
        handler.axis1 = (int)command[0];
        handler.axis2 = (int)command[1];
        handler.SetterSelection((SelectSticker)command[2]);
    }

    /// <summary>
    /// Used as a coroutine, it listens wether some chunk of commands are to be injected or not.
    /// If this is the case, for each of the commands it will launch the matching animation and update the sticker positions.
    /// The rotation speed is increased not to make the user leave from boredom.
    /// </summary>
    /// <returns>As I recall, nothing eheh.</returns>
    private IEnumerator RotationHandler() {
        // TODO try to delete this function? just change the rotationSpeed to 6.
        while (true) {
            if (!inputing) {
                yield return null;
            }
            else {
                // TODO need reajustement
                for(int i = inputsBuffer.Count-1 ; i > -1 ; i--) {
                    InjectInput(inputsBuffer[i]);
                    float totalRotation = 0;
                    List<List<Vector4>> targets = Animation.DefineTargets(handler.p, handler.selectedSticker, Geometry.IntToAxis(handler.axis1), Geometry.IntToAxis(handler.axis2));
                    List<List<bool>> toBeRotated = handler.p.whosGunnaRotate(handler.selectedSticker); 
                    if(Geometry.IsBetweenRangeExcluded(handler.rotationSpeed, 0f, 90f)){
                        while(Mathf.Abs(90f - totalRotation) > Mathf.Epsilon){
                            totalRotation = Animation.RotateOverTime(handler.p, handler.puzzle, totalRotation, toBeRotated, Geometry.IntToAxis(handler.axis1), Geometry.IntToAxis(handler.axis2));
                            yield return null;
                        }
                    }
                    Animation.SnapToTargets(handler.p, handler.puzzle, targets, toBeRotated);
                }

                Animation.SetRotationSpeed(2f);
                inputing = false;
                inputsBuffer.Clear();
            }
        }
    }
    /// <summary>
    /// Getter of the inputing flag.
    /// </summary>
    /// <returns>A bool : if true, animations are still running
    ///                   if false, well we good to go.</returns>
    public bool GetInputingFlag() {
        return inputing;
    }

}
