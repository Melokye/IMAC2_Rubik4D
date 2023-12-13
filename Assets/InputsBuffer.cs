using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputsBuffer : MonoBehaviour
{
    SphereProjection4D handler;
    public GameObject rotationEngine;
    private bool inputing;
    List<List<int>> inputsBuffer = new List<List<int>>(0);
    List<List<int>> mixed = new List<List<int>>(0);
    // Start is called before the first frame update
    void Start()
    {
        rotationEngine = GameObject.Find("SphereGenerator");
        handler = rotationEngine.GetComponent<SphereProjection4D>();
        scrambler(ref mixed);
        //inputsBuffer = mixed;
        StartCoroutine(autoInput());
        //inputing = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R)) {
            List<int> Entry = new List<int>(){rotationEngine.GetComponent<SphereProjection4D>().GetAxis2(),rotationEngine.GetComponent<SphereProjection4D>().GetAxis1()};
            inputsBuffer.Add(Entry);
            debugLength(inputsBuffer);
            }
        if (Input.GetKeyDown(KeyCode.M)) {
            /*un code qui permet d'executer pleins de rotations d'un coup*/
            foreach(var entry in mixed){
                injectInput(in entry);
            }
            inputsBuffer.AddRange(mixed);
            mixed.Clear();
            scrambler(ref mixed);
            Debug.Log("Done mixing");
        }
        if (Input.GetKeyDown(KeyCode.S)){
            /*un code qui permet d'executer pleins de rotations d'un coup*/
            inputing = true;
            Debug.Log("Done solving");
        }
    }

    public void scrambler(ref List<List<int>> mixed){
        // Generation of a 50 entries set of inputs.
        // For each entry must be specified 2 values :
        // axis1 (0,1,2,3), axis2 (0,1,2,3)
        // rotation speed will remain untouched.
        System.Random rnd = new System.Random();
        for(int cmp = 0 ; cmp < 50 ; cmp++){
            int axis1 = rnd.Next(0,4);
            int axis2 = rnd.Next(0,4);
            mixed.Add(new List<int>(){axis1,axis2});
        }
    }

    public void injectInput(in List<int> command){
        //debugLength(commands);
        handler.targets.Clear();
        handler.axis1 = command[0];
        handler.axis2 = command[1];

        
    }

    IEnumerator autoInput(){
        while (true) {
            if (!inputing) {
                yield return null;
            }
            else {
                Debug.Log("wtf");
                foreach(var entry in inputsBuffer){
                    injectInput(in entry);
                    
                    int i = 0;
                    handler.UpdateRotationMatrix(handler.axis1,handler.axis2, 90);
                    foreach (Transform child in handler.container.transform) {
                        handler.targets.Add(handler.rotationMatrix * handler.points[i]);
                        i++;
                    }
                    i = 0;
                    handler.UpdateRotationMatrix(handler.axis1, handler.axis2, handler.rotationSpeed);
                    while (Vector4.Distance(handler.points[0], handler.targets[0]) > Vector4.kEpsilon) {
                        i = 0;
                        foreach (Transform child in handler.container.transform) {
                            handler.points[i] = handler.rotationMatrix * handler.points[i];
                            child.transform.position = handler.Projection4DTo3D(handler.points[i]);
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
        Debug.Log(cmp);
    }

}
