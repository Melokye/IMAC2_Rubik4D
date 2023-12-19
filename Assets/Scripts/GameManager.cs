using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour { // == main
    public GameObject puzzle;
    public Puzzle p;
    public Camera[] cameraArray;


    private bool _cubeRotating = false;

    [SerializeField]
    public Coords4D selectedElement; // TODO private

    // TODO for debug / test purpose?
    public int axis1 = 0;
    public int axis2 = 1;
    // ---

    // To customize the Rubik // TODO needs to be added in a Parameter Menu
    [SerializeField]
    private Mesh sphereMesh;
    private float stickerSize = 0.125f;

    public float rotationSpeed = 2f; // TODO remplace by Animation.rotationSpeed

    // to simplify the camera rotation
    // TODO move it in another file?
    static float s3 = 1f / Mathf.Sqrt(3f);
    static float s6 = (3f + Mathf.Sqrt(3f)) / 6f;
    static float _s6 = 1f - s6;

    static Matrix4x4 specialProjection = new Matrix4x4(
        new Vector4(-s6, 0f, _s6, s3),
        new Vector4(_s6, 0f, -s6, s3),
        new Vector4(-s3, 0f, -s3, -s3),
        new Vector4(0f, 1f, 0f, 0f));

    private int cameraRotationMode = 0;
    public static Matrix4x4 cameraRotation = specialProjection;

    /// <summary>
    /// Awake is called automatically before the function Start
    /// </summary>
    void Awake() {
        p = new Puzzle();

        // Create a GameObject for each point and link them in the GameObject "Puzzle"
        puzzle = p.RenderStickers(sphereMesh, stickerSize);
        puzzle.name = "Puzzle";
        puzzle.tag = "Puzzle"; // Defines this object as a Puzzle object

        // Create GameObjects representing the rotation axes, aesthetic purpose
        GameObject circleContainer = RingsRepresentation.RenderCircles("CircleContainer", p);

        // Creates the dupe puzzle to display the classical view in a UI
        ChangeProjection(); // Swap to classical view
        GameObject puzzle_UI = p.RenderStickers(sphereMesh, stickerSize);
        puzzle_UI.name = "Puzzle_UI";
        puzzle_UI.tag = "Puzzle";

        SetLayerAllChildren(puzzle_UI.transform, 3); // Change layer for camera view
        GameObject circleContainer_UI = RingsRepresentation.RenderCircles("CircleContainer_UI", p);
        SetLayerAllChildren(circleContainer_UI.transform, 3);

        ChangeProjection(); // Change back projection for the first Update() frame cycle
    }

    /// <summary>
    /// Start is called automatically before the first frame update
    /// </summary>
    void Start() {
        cameraArray = Camera.allCameras;
        // Handles rotation in parallel to the Update method
        StartCoroutine(RotationHandler());
    }

    /// <summary>
    /// Update is called automatically once per frame
    /// </summary>
    void Update() {
        if (!_cubeRotating) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                (axis1, axis2) = (axis2, axis1);
            }
            /*if (Input.GetKeyDown(KeyCode.R) && axis1 != axis2) {
                LaunchRotation();
            }*/
            
        }
        // At all times, there are two puzzle game objects.
        // The first is the special projection, the second is the classic projection.
        // The loop below projects the stickers for the first, changes projection,
        // then projects the stickers for the second, then changes projection back
        // to prepare for the next frame.
        foreach (GameObject puzzle in GameObject.FindGameObjectsWithTag("Puzzle")) {
            PuzzleProjection4DTo3D(puzzle);
            ChangeProjection();
        }
    }

    /// <summary>
    /// Initialize the necessary data to launch a rotation
    /// </summary>
    public void LaunchRotation() {
        _cubeRotating = true;
    }

    /// <summary>
    /// Handles rotations on each frame.
    /// </summary>
    /// <returns></returns>
    public IEnumerator RotationHandler() { // TODO directly in Animation.cs?
        while (true) {
            if (!_cubeRotating) {
                yield return null;
                // == continue; in c, to avoid freeze screen when used in coroutine
            }else {
                List<List<Vector4>> targets = Animation.DefineTargets(p, selectedElement, Geometry.IntToAxis(axis1), Geometry.IntToAxis(axis2));
                List<List<bool>> toBeRotated = p.whosGunnaRotate(selectedElement);
                if (Geometry.IsBetweenRangeExcluded(rotationSpeed, 0f, 90f)) {
                    float totalRotation = 0;
                    while (Mathf.Abs(90f - totalRotation) > Mathf.Epsilon) {
                        // TODO need reajustement?
                        totalRotation = Animation.RotateOverTime(p, puzzle, totalRotation, toBeRotated, Geometry.IntToAxis(axis1), Geometry.IntToAxis(axis2));
                        yield return null;
                    }
                }

                Animation.SnapToTargets(p, puzzle, targets, toBeRotated);
                _cubeRotating = false;
            }
        }
    }

    /// <summary>
    /// Toggle between classic projection and special projection
    /// </summary>
    public void ChangeProjection() {
        switch (cameraRotationMode) {
            case 0:
                cameraRotationMode = 1;
                cameraRotation = Matrix4x4.identity;
                break;
            case 1:
            default:
                cameraRotationMode = 0;
                cameraRotation = specialProjection;
                break;
        }
        // TODO: find a better way to manage cameraRotation

        // Destroy previous circles
        // GameObject circleContainer = GameObject.Find("CircleContainer");
        // Destroy(circleContainer);

        // Render new circles
        // RenderCircles();

        // Project all 4D stickers to 3D space
        // PuzzleProjection4DTo3D(gameObject);
    }

    /// <summary>
    /// Projects a GameObject and all its 4D children into 3D
    /// </summary>
    /// <param name="gameObject"></param>
    private void PuzzleProjection4DTo3D(GameObject gameObject) {
        for (int i = 0; i < gameObject.transform.childCount; i++) {
            Transform cell = gameObject.transform.GetChild(i);
            for (int j = 0; j < cell.childCount; j++) {
                Transform sticker = cell.GetChild(j);
                sticker.position = Geometry.Projection4DTo3D(cameraRotation * p.GetSticker(i, j));
            }
        }
    }

    public int GetAxis1() {
        return axis1;
    }

    public int GetAxis2() {
        return axis2;
    }

    public bool GetRotateFlag() {
        return _cubeRotating;
    }

    public Coords4D GetSelection() {
        return selectedElement;
    }

    public void SetSelection(Coords4D selection) {
        selectedElement = selection;
    }

    /// <summary>
    /// Set the plane based on two axes
    /// </summary>
    /// <param name="a1">the first axis</param>
    /// <param name="a2">the second axis</param>
    public void SetPlane(int a1, int a2) {
        axis1 = a1;
        axis2 = a2;
    }

    /// <summary>
    /// Sets display Layer of a transform and all its children
    /// </summary>
    /// <param name="root"></param>
    /// <param name="layer"></param>
    public void SetLayerAllChildren(Transform root, int layer) {
        root.gameObject.layer = layer;
        var children = root.GetComponentsInChildren<Transform>(includeInactive: true);
        foreach (var child in children) {
            //Debug.Log(child.name);
            child.gameObject.layer = layer;
        }
    }

    /// <summary>
    /// Sets display Layer of direct children
    /// </summary>
    /// <param name="root"></param>
    /// <param name="layer"></param>
    public void SetLayerDirectChildrenNoRoot(Transform root, int layer) {
        foreach (Transform child in root) {
            child.gameObject.layer = layer;
        }
    }

    // Toggle the bit using a XOR operation
    public void ToggleCameraCullingMask(Camera camera, string layerName) {
        camera.cullingMask ^= 1 << LayerMask.NameToLayer(layerName);
    }
}
