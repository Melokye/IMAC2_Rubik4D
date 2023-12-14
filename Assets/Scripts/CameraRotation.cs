using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRotation : MonoBehaviour
{

    public Camera cameraObj;
    public GameObject centerOfBlackHole;
    private float speed = 2f;
    // Start is called before the first frame update


    void Start()
    {
        cameraObj = Camera.main;
        centerOfBlackHole = GameObject.Find("Up");

    }

    // Update is called once per frame
    void Update()
    {
        RotateCamera();
    }

    void RotateCamera(){
        if(Input.GetMouseButton(0))
        {
         cameraObj.transform.RotateAround(centerOfBlackHole.transform.position,
                                        cameraObj.transform.up,
                                        Input.GetAxis("Mouse X")*speed);

         cameraObj.transform.RotateAround(centerOfBlackHole.transform.position, 
                                        cameraObj.transform.right,
                                        -Input.GetAxis("Mouse Y")*speed);
        } 
    }
}
