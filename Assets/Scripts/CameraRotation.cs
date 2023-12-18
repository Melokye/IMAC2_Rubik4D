using UnityEngine;

public class CameraRotation : MonoBehaviour {
    public Camera cameraObj;
    private float speed = 2f;

    // Start is called before the first frame update
    void Start() {
        cameraObj = Camera.main;
    }

    // Update is called once per frame
    void Update() {
        RotateCamera();
    }

    void RotateCamera() {
        if (Input.GetMouseButton(0)) {
            cameraObj.transform.RotateAround(Vector3.zero,
                cameraObj.transform.up, Input.GetAxis("Mouse X") * speed);

            cameraObj.transform.RotateAround(Vector3.zero,
                cameraObj.transform.right, -Input.GetAxis("Mouse Y") * speed);
        }
    }
}
