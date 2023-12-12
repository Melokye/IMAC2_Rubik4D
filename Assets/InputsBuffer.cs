using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

// TODO delete _attribute used from GameManager.cs
// TODO reduce dependancies with GameManager

public class InputsBuffer : MonoBehaviour {
    GameManager handler;
    public GameObject rotationEngine;
    private bool inputing;
    List<List<int>> inputsBuffer = new List<List<int>>(0);
    List<List<int>> mixed = new List<List<int>>(0);
    // Start is called before the first frame update
    void Start() {
        rotationEngine = GameObject.Find("SphereGenerator");
        handler = rotationEngine.GetComponent<GameManager>();
        Scrambler(ref mixed);
        // inputsBuffer = mixed;
        StartCoroutine(RotationHandler());
        // inputing = true;
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.R)) {
            List<int> Entry = new List<int>(){handler.GetAxis2(), handler.GetAxis1()};
            inputsBuffer.Add(Entry);
            // debugLength(inputsBuffer); // TODO
        }
        if (Input.GetKeyDown(KeyCode.M)) {
            /*un code qui permet d'executer pleins de rotations d'un coup*/
            foreach(var entry in mixed){
                InjectInput(in entry);
            }
            inputsBuffer.AddRange(mixed);
            mixed.Clear();
            Scrambler(ref mixed);
            // Debug.Log("Done mixing"); // TODO
        }
        if (Input.GetKeyDown(KeyCode.S)) {
            /*un code qui permet d'executer pleins de rotations d'un coup*/
            inputing = true;
            // Debug.Log("Done solving"); // TODO
        }
    }

    public void Scrambler(ref List<List<int>> mixed) {
        // Generation of a 50 entries set of inputs.
        // For each entry must be specified 2 values :
        // axis1 (0,1,2,3), axis2 (0,1,2,3)
        // rotation speed will remain untouched.
        System.Random rnd = new System.Random();
        for (int cmp = 0 ; cmp < 50 ; cmp++) {
            int axis1 = rnd.Next(0,4);
            int axis2 = rnd.Next(0,4);
            mixed.Add(new List<int>(){axis1,axis2});
        }
    }

    public void InjectInput(in List<int> command) {
        //debugLength(commands); // TODO
        handler.targets.Clear();
        handler.axis1 = command[0];
        handler.axis2 = command[1];
    }

    private IEnumerator RotationHandler() {
        while (true) {
            if (!inputing) {
                yield return null;
            }
            else {
                // Debug.Log("wtf"); // TODO
                foreach (var entry in inputsBuffer) {
                    InjectInput(in entry);
                    
                    int i = 0;
                    Matrix4x4 rotate = handler.RotationMatrix(handler.axis1,handler.axis2, 90);
                    foreach (Transform child in handler.puzzle.transform) {
                        handler.targets.Add(rotate * handler._cells[i]);
                        i++;
                    }

                    i = 0;
                    rotate = handler.RotationMatrix(handler.axis1, handler.axis2, handler.rotationSpeed);
                    while (Vector4.Distance(handler._cells[0], handler.targets[0]) > Vector4.kEpsilon) {
                        i = 0;
                        foreach (Transform child in handler.puzzle.transform) {
                            handler._cells[i] = rotate * handler._cells[i];
                            child.transform.position = handler.Projection4DTo3D(handler._cells[i]);
                            i++;
                        }
                        yield return null;
                    }
                }
                inputing = false;
                inputsBuffer.Clear();
            }
        }        
    }

    void debugLength(in List<List<int>> list){
        int cmp=0;
        foreach(var entry in list){
            cmp++;
        }
        //Debug.Log(cmp);
    }

}
