using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ball : MonoBehaviour
{
    private bool motion = false;
    [SerializeField] // TODO need to recuperate auto
    private GameObject axe;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey("mouse 0") && axeSelected()){
            motion = !motion;
        }
        
        if(motion)
            Move(axe);
    }

    bool axeSelected(){
        GameObject selected = Selection.activeGameObject; // TODO need to find another thing than Selection
        if(selected != null){
            // print(selected.name); // TODO
            axe = selected;
            // TODO check the type of object selected
            return true;
        }
        return false;
    }

    void Move(GameObject axe){
        // center.transform.position
        transform.RotateAround(axe.transform.position, Vector3.up, 60 * Time.deltaTime);
    }
}
