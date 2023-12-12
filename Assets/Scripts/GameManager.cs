using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

enum Axis { x, y, z, w}

// TODO in project: clean up old useless files and scene
public class GameManager : MonoBehaviour {
    // TODO const attributs
    List<string> _names = new List<string>() {
            "Right", "Left", "Up", "Down", "Back", "Front", "In", "Out" };
    List<string> _materials = new List<string>() {
            "Red", "Orange", "Blue", "Green", "Yellow", "White", "Purple", "Pink" };
    // ---

    // TODO move in another file? 
    // TODO Create a specific struct?
    public List<Vector4> _cells = new List<Vector4>();
    List<List<Vector4>> _stickers = new List<List<Vector4>>(); 

    // TODO delete these "attributes" -> function
    public List<Vector4> targets = new List<Vector4>(); // TODO may not be useful
    List<List<Vector4>> subtargets = new List<List<Vector4>>();
    // ---
    
    private bool _cubeRotating = false;

    public GameObject puzzle;
    
    public int axis1 = 0;
    public int axis2 = 1;
    private float totalRotation = 0; // TODO not an attribute?
    
    // To customize the Rubik
    [SerializeField]
    private Mesh sphereMesh;
    [SerializeField]
    private int puzzleSize = 2;
    private float stickerDistance = 10f;
    private float stickerSize = 0.2f;
    public float rotationSpeed = 2f;

    // to simplify the camera rotation
    // TODO move it in another file?
    static float s3 = 1f / Mathf.Sqrt(3f);
    static float s6 = (3f + Mathf.Sqrt(3f)) / 6f;
    static float _s6 = 1f - s6;

    Matrix4x4 cameraRotation = new Matrix4x4(
        new Vector4(-s6, 0f, _s6, s3),
        new Vector4(_s6, 0f, -s6, s3),
        new Vector4(-s3, 0f, -s3, -s3),
        new Vector4(0f, 1f, 0f, 0f));

    // secondary rotation matrix to eventually use later
    Matrix4x4 colorAssignment = new Matrix4x4(
        new Vector4(1, 0, 0, 0),
        new Vector4(0, 1, 0, 0),
        new Vector4(0, 0, 1, 0),
        new Vector4(0, 0, 0, 1));

    // Start is called before the first frame update
    void Start() {
        GenerateStickerCoordinates();

        puzzle.name = "Container";

        // Create a GameObject for each point and link them in the GameObject "container"
        RenderStickers();

        // Handles rotation in parallel to the Update method
        StartCoroutine(RotationHandler());
    }

    // Update is called once per frame
    void Update() {
        if (!_cubeRotating) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                (axis1, axis2) = (axis2, axis1);
            }
            if (Input.GetKeyDown(KeyCode.R) && axis1 != axis2) {
                targets.Clear();
                subtargets.Clear();
                totalRotation = 0;

                _cubeRotating = true;
            }
        }
    }

    void GenerateStickerCoordinates() {
        const int nbPoints = 8;
        for (int i = 0; i < nbPoints; i++) {
            Vector4 point = new Vector4(0, 0, 0, 0);
            int pointIndex = Mathf.FloorToInt(i * 0.5f);
            int altSign = 1 - (2 * (i % 2));
            point[pointIndex] = altSign;
            _cells.Add(point);
            _stickers.Add(new List<Vector4>());
            for (int j = 0; j < Mathf.Pow(puzzleSize, 3); j++) {
                Vector3 temp = new Vector3(0, 0, 0);
                temp.x = Mathf.Lerp(-1f, 1f,
                    (Mathf.FloorToInt(j / Mathf.Pow(puzzleSize, 2)) % puzzleSize) / (puzzleSize - 1f));
                temp.y = Mathf.Lerp(-1f, 1f,
                    (Mathf.FloorToInt(j / puzzleSize) % puzzleSize) / (puzzleSize - 1f));
                temp.z = Mathf.Lerp(-1f, 1f,
                    (j % puzzleSize) / (puzzleSize - 1f));

                Vector4 subpoint = new Vector4(0, 0, 0, 0);
                subpoint = InsertFloat(temp / stickerDistance, point[pointIndex], pointIndex);
                _stickers[i].Add(subpoint);
            }
        }
    }

    void RenderStickers() {
        for (int i = 0; i < _cells.Count; i++) {
            // TODO warning : length of _names and _materials may not be the same as the number of points
            GameObject cell = new GameObject();
            cell.name = _names[i];

            // place these points in the space
            cell.transform.parent = puzzle.transform;
            cell.transform.position = Projection4DTo3D(_cells[i]);
            for (int j = 0; j < _stickers[i].Count; j++) {
                GameObject sticker = new GameObject();
                sticker.name = _names[i] + "_" + j;

                // add mesh
                sticker.AddComponent<MeshFilter>();
                sticker.GetComponent<MeshFilter>().mesh = sphereMesh;

                // add material
                Material stickerMat = Resources.Load(_materials[i], typeof(Material)) as Material;
                sticker.AddComponent<MeshRenderer>();
                sticker.GetComponent<Renderer>().material = stickerMat;

                // place these points in the space
                sticker.transform.localScale = stickerSize * Vector3.one;
                sticker.transform.parent = cell.transform;
                sticker.transform.position = Projection4DTo3D(_stickers[i][j]);

                // circle generation from movement trails test
                /*GameObject trail = new GameObject();
                trail.name = "TrailRenderer";
                trail.AddComponent<MeshFilter>();
                trail.AddComponent<MeshRenderer>();
                trail.GetComponent<Renderer>().material = stickerMat;
                trail.AddComponent<MeshTrail>();
                trail.transform.parent = sticker.transform;*/
            }
        }
    }
    
    // Inserts value in Vector3 at pos, making it a Vector4
    Vector4 InsertFloat(Vector3 vec, float value, int pos) {
        pos = Mathf.Clamp(pos, 0, 3);
        Vector4 result = new Vector4(0, 0, 0, 0);
        switch (pos) {
            case 0:
                result = new Vector4(value, vec.x, vec.y, vec.z);
                break;
            case 1:
                result = new Vector4(vec.x, value, vec.y, vec.z);
                break;
            case 2:
                result = new Vector4(vec.x, vec.y, value, vec.z);
                break;
            case 3:
                result = new Vector4(vec.x, vec.y, vec.z, value);
                break;
            default:
                break;
        }
        return result;
    }

    /// <summary>
    /// Generate a new rotationMatrix from two axis and an angle
    /// </summary>
    /// <param name="axis1"></param>
    /// <param name="axis2"></param>
    /// <param name="angle"></param>
    public Matrix4x4 RotationMatrix(int axis1, int axis2, float angle) {
        Matrix4x4 rotationMatrix = Matrix4x4.identity;
        rotationMatrix[axis1, axis1] = Mathf.Cos(angle * Mathf.Deg2Rad);
        rotationMatrix[axis2, axis1] = -Mathf.Sin(angle * Mathf.Deg2Rad);
        rotationMatrix[axis1, axis2] = Mathf.Sin(angle * Mathf.Deg2Rad);
        rotationMatrix[axis2, axis2] = Mathf.Cos(angle * Mathf.Deg2Rad);
        return rotationMatrix;
    }

    public IEnumerator RotationHandler() {
        while (true) {
            if (!_cubeRotating) {
                yield return null; 
                // == continue; in c, to avoid freeze screen when used in coroutine
            }
            else {
                DefineTargets();
                if (IsBetweenRangeExcluded(rotationSpeed, 0f, 90f)) {
                    while (Mathf.Abs(90f - totalRotation) > Mathf.Epsilon) {
                        RotateOverTime(rotationSpeed);
                        yield return null;
                    }
                }
                    
                SnapToTargets();
                _cubeRotating = false;
            }
        }
    }

    /// <summary>
    /// Determine the destination of each cell and sticker
    /// </summary>
    private void DefineTargets() {
        // TODO put "puzzle" in param? + return "targets" and "subtargets"?
        for (int i = 0; i < puzzle.transform.childCount; i++) {
            Matrix4x4 rotate = RotationMatrix(axis1, axis2, 90);
            targets.Add(rotate * _cells[i]);
            subtargets.Add(new List<Vector4>());
            
            Transform cell = puzzle.transform.GetChild(i);
            for (int j = 0; j < cell.childCount; j++) {
                subtargets[i].Add(rotate * _stickers[i][j]);
            }
        }
    }

    /// <summary>
    /// Rotates by 90 degrees with animation
    /// </summary>
    /// <param name="rotationSpeed"> </param> 
    private void RotateOverTime(float rotationSpeed) {
        totalRotation += rotationSpeed;
        rotationSpeed = Mathf.Clamp(rotationSpeed, 0f, 90f - totalRotation + rotationSpeed);
        totalRotation = Mathf.Clamp(totalRotation, 0f, 90f);
        for (int i = 0; i < puzzle.transform.childCount; i++) {
            // Rotates cells
            Matrix4x4 rotate = RotationMatrix(axis1, axis2, rotationSpeed);
            _cells[i] = rotate * _cells[i];
            Transform cell = puzzle.transform.GetChild(i);
            cell.position = Projection4DTo3D(_cells[i]);
            for (int j = 0; j < cell.childCount; j++) {
                Transform sticker = cell.GetChild(j);
                // Rotates stickers
                _stickers[i][j] = rotate * _stickers[i][j];
                sticker.position = Projection4DTo3D(_stickers[i][j]);
            }
        }
    }

    /// <summary>
    /// Snaps each cell and sticker to its final position
    /// </summary>
    private void SnapToTargets() {
        for (int i = 0; i < puzzle.transform.childCount; i++) {
            _cells[i] = targets[i];
            Transform cell = puzzle.transform.GetChild(i);
            cell.position = Projection4DTo3D(_cells[i]);
            for (int j = 0; j < cell.childCount; j++) {
                Transform sticker = cell.GetChild(j);
                _stickers[i][j] = subtargets[i][j];
                sticker.position = Projection4DTo3D(_stickers[i][j]);
            }
        }
    }

    public static bool IsBetweenRangeExcluded(float value, float value1, float value2) {
        return value > Mathf.Min(value1, value2) && value < Mathf.Max(value1, value2);
    }

    public Vector4 Projection4DTo3D(Vector4 point) {
        Vector4 temp = new Vector4(point.x, point.y, point.z, point.w);
        temp = cameraRotation * colorAssignment * temp;
        return new Vector3(temp.x, temp.y, temp.z) / (temp.w + 1);
    }

    // TODO for debug purpose ----
    public int GetAxis1() {
        return axis1;
    }

    public int GetAxis2() {
        return axis2;
    }

    // Old unused function may be reused later on
    /*List<float> AddFloatList(List<float> a, List<float> b) {
        List<float> result = new List<float>();
        for (int i = 0; i < a.Count; i++) {
            result.Add(a[i] + b[i]);
        }
        return result;
    }*/

    // Old unused function may be reused later on
    /*private void RotateAroundTowards(Transform a, Vector3 b, Vector3 center, int direction, float t) {
        float radius = Vector3.Distance(center, b);
        float a_angle = Mathf.Atan2(a.position.z - center.z, a.position.x - center.x) * Mathf.Rad2Deg;
        float b_angle = Mathf.Atan2(b.z - center.z, b.x - center.x) * Mathf.Rad2Deg;
        if (direction * b_angle > direction * a_angle) {
            b_angle = b_angle - 360 * direction;
        }
        a_angle = Mathf.Lerp(a_angle, b_angle, t);
        a.position = new Vector3(Mathf.Cos(a_angle * Mathf.Deg2Rad) * radius + center.x, a.position.y,
            Mathf.Sin(a_angle * Mathf.Deg2Rad) * radius + center.z);
    }*/
}
