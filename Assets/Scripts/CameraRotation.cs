using UnityEngine;

/// <summary>
/// Handle the rotation of the camera around the puzzles.
/// </summary>
public class CameraRotation : MonoBehaviour {
    public Camera cameraMain;
    public Camera cameraUI;
    private float speed = 2f;

    // Start is called before the first frame update
    void Start() {
        cameraMain = Camera.main;
        cameraUI = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update() {
        RotateCamera(cameraMain);
        RotateCamera(cameraUI);
    }

    /// <summary>
    /// Moves the camera around the puzzles in order to match a mouse drag on the screen.
    /// </summary>
    void RotateCamera(Camera camera) {
        if (Input.GetMouseButton(0)) {
            camera.transform.RotateAround(Vector3.zero,
                camera.transform.up, Input.GetAxis("Mouse X") * speed);
            camera.transform.RotateAround(Vector3.zero,
                camera.transform.right, -Input.GetAxis("Mouse Y") * speed);
        }
    }
}
