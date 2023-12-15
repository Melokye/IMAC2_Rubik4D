using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO delete _attribute used from GameManager.cs
// TODO reduce dependancies with GameManager
// TODO rename file into Solver.cs
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
        Scrambler(ref mixed);
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
            Scrambler(ref mixed);
            inputing = true;
            // Debug.Log("Done mixing"); // TODO
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            /*un code qui permet d'executer pleins de rotations d'un coup*/
            inputing = true;
            //Debug.Log("Done solving"); // TODO
        }
    }

    public void Scrambler(ref List<List<object>> mixed) {
        // Generation of a 50 entries set of inputs.
        // For each entry must be specified 2 values :
        // axis1 (0,1,2,3), axis2 (0,1,2,3)
        // rotation speed will remain untouched.
        int axis1 = 0;
        int axis2 = 1;
        System.Random rnd = new System.Random();
        for (int cmp = 0 ; cmp < 50 ; cmp++) {
            axis1 = rnd.Next(0,3);
            while(axis2==axis1)axis2 = rnd.Next(1,4);
            mixed.Add(new List<object>(){axis1,axis2});
        }
    }

    public void InjectInput(in List<object> command) {
        //debugLength(commands); // TODO
        // handler.targets.Clear();
        handler.axis1 = (int)command[0];
        handler.axis2 = (int)command[1];
        handler.SetterSelection((SelectSticker)command[2]);
    }

    private IEnumerator RotationHandler() {
        while (true) {
            if (!inputing) {
                yield return null;
            }
            else {
                handler.rotationSpeed = 6 ;
                for(int i = inputsBuffer.Count-1 ; i > -1 ; i--) {
                    InjectInput(inputsBuffer[i]);
                    float totalRotation = 0;
                    List<List<Vector4>> targets = handler.DefineTargets();
                    List<List<bool>> toBeRotated = handler.p.whosGunnaRotate(handler.selectedSticker); // TODO need reajustement
                    if(Geometry.IsBetweenRangeExcluded(handler.rotationSpeed, 0f, 90f)){
                        while(Mathf.Abs(90f - totalRotation) > Mathf.Epsilon){
                            totalRotation = handler.RotateOverTime(handler.rotationSpeed, totalRotation, toBeRotated);
                            yield return null;
                        }
                    }
                    handler.SnapToTargets(targets, toBeRotated);
                }

                handler.rotationSpeed = 2;
                inputing = false;
                inputsBuffer.Clear();
            }
        }
    }

    void DebugLength(in List<List<int>> list) {
        int cmp = 0;
        foreach (var entry in list) {
            cmp++;
        }
        //Debug.Log(cmp);
    }


    public bool GetInputingFlag() {
        return inputing;
    }

}
