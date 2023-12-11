using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SphereProjection4D : MonoBehaviour
{
    List<Vector4> _points = new List<Vector4>();
    List<List<Vector4>> _subpoints = new List<List<Vector4>>();
    List<Vector4> targets = new List<Vector4>();
    Matrix4x4 rotationMatrix = Matrix4x4.identity; // TODO à voir avec M. Nozick
    bool cubeRotating = false;
    List<string> _names = new List<string>() {
            "Right", "Left", "Up", "Down", "Back", "Front", "In", "Out" };
    List<string> _materials = new List<string>() {
            "Red", "Orange", "Blue", "Green", "Yellow", "White", "Purple", "Pink" };

    [SerializeField]
    private Mesh sphereMesh;
    [SerializeField]
    GameObject container;
    [SerializeField]
    float rotationSpeed = 2f;
    [SerializeField]
    int axis1 = 0;
    [SerializeField]
    int axis2 = 1;
    private float totalRotation = 0;
    [SerializeField]
    private int puzzleSize = 2;
    private float stickerDistance = 10f;
    private float stickerSize = 0.2f;

    static float s3 = 1f / Mathf.Sqrt(3f);
    static float s6 = (3f + Mathf.Sqrt(3f)) / 6f;
    static float _s6 = 1f - s6;

    Matrix4x4 cameraRotation = new Matrix4x4(
        new Vector4(-s6, 0f, _s6, s3),
        new Vector4(_s6, 0f, -s6, s3),
        new Vector4(-s3, 0f, -s3, -s3),
        new Vector4(0f, 1f, 0f, 0f));

    Matrix4x4 colorAssignment = new Matrix4x4(
        new Vector4(1, 0, 0, 0),
        new Vector4(0, 1, 0, 0),
        new Vector4(0, 0, 1, 0),
        new Vector4(0, 0, 0, 1));

    void UpdateRotationMatrix(int axis1, int axis2, float angle) {
        rotationMatrix = Matrix4x4.identity;
        rotationMatrix[axis1, axis1] = Mathf.Cos(angle * Mathf.Deg2Rad);
        rotationMatrix[axis2, axis1] = -Mathf.Sin(angle * Mathf.Deg2Rad);
        rotationMatrix[axis1, axis2] = Mathf.Sin(angle * Mathf.Deg2Rad);
        rotationMatrix[axis2, axis2] = Mathf.Cos(angle * Mathf.Deg2Rad);
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

    // Start is called before the first frame update
    void Start() {
        // Generate points
        const int nbPoints = 8;
        for (int i = 0; i < nbPoints; i++) {
            Vector4 point = new Vector4(0, 0, 0, 0);
            int pointIndex = Mathf.FloorToInt(i * 0.5f);
            point[pointIndex] = 1 - (2 * (i % 2));
            _points.Add(point);
            _subpoints.Add(new List<Vector4>());
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
                _subpoints[i].Add(subpoint);
            }
        }

        // Create a GameObject for each point and link them in the GameObject "container"
        container.name = "Container";

        for (int i = 0; i < _points.Count; i++) {
            // TODO warning : length of _names and _materials may not be the same as the number of points
            GameObject cell = new GameObject();
            cell.name = _names[i];

            // place these points in the space
            cell.transform.parent = container.transform;
            cell.transform.position = Projection4DTo3D(_points[i]);
            for (int j = 0; j < _subpoints[i].Count; j++) {
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
                sticker.transform.position = Projection4DTo3D(_subpoints[i][j]);
            }
        }

        // TODO for test purpose, must be deleted later
        UpdateRotationMatrix(axis1, axis2, 0);

        // To make animation
        StartCoroutine(Rotate90Degrees());
    }

    // Update is called once per frame
    void Update() {
        if (!cubeRotating) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                (axis1, axis2) = (axis2, axis1);
            }
            if (Input.GetKeyDown(KeyCode.R) && axis1 != axis2) {
                targets.Clear();
                totalRotation = 0;
                cubeRotating = true;
            }
        }
    }

    private IEnumerator Rotate90Degrees() {
        while (true) {
            if (!cubeRotating) {
                yield return null;
            }
            else {
                int i = 0;
                int j = 0;
                UpdateRotationMatrix(axis1, axis2, 90);
                foreach (Transform child in container.transform) {
                    targets.Add(rotationMatrix * _points[i]);
                    i++;
                }
                if (IsBetweenRangeExcluded(rotationSpeed, 0f, 90f)) {
                    float rotationSpeedTemp = rotationSpeed;
                    while (Mathf.Abs(90f - totalRotation) > Mathf.Epsilon) {
                        totalRotation += rotationSpeedTemp;
                        rotationSpeedTemp = Mathf.Clamp(rotationSpeed, 0f, 90f - totalRotation + rotationSpeedTemp);
                        totalRotation = Mathf.Clamp(totalRotation, 0f, 90f);
                        i = 0;
                        UpdateRotationMatrix(axis1, axis2, rotationSpeed);
                        foreach (Transform child in container.transform) {
                            _points[i] = rotationMatrix * _points[i];
                            child.transform.position = Projection4DTo3D(_points[i]);
                            j = 0;
                            foreach (Transform subchild in child) {
                                _subpoints[i][j] = rotationMatrix * _subpoints[i][j];
                                subchild.transform.position = Projection4DTo3D(_subpoints[i][j]);
                                print(subchild.transform.position);
                                j++;
                            }
                            i++;
                        }
                        yield return null;
                    }
                }
                i = 0;
                foreach (Transform child in container.transform) {
                    _points[i] = targets[i];
                    child.transform.position = Projection4DTo3D(_points[i]);
                    j = 0;
                    foreach (Transform subchild in child) {
                        subchild.transform.position = Projection4DTo3D(_subpoints[i][j]);
                        j++;
                    }
                    i++;
                }
                cubeRotating = false;
            }
        }
    }

    public static bool IsBetweenRangeExcluded(float value, float value1, float value2) {
        return value > Mathf.Min(value1, value2) && value < Mathf.Max(value1, value2);
    }

    Vector4 Projection4DTo3D(Vector4 point) {
        Vector4 temp = new Vector4(point.x, point.y, point.z, point.w);
        temp = cameraRotation * colorAssignment * temp;
        return new Vector3(temp.x, temp.y, temp.z) / (temp.w + 1);
    }

    List<float> AddFloatList(List<float> a, List<float> b) {
        List<float> result = new List<float>();
        for (int i = 0; i < a.Count; i++) {
            result.Add(a[i] + b[i]);
        }
        return result;
    }

    private void RotateAroundTowards(Transform a, Vector3 b, Vector3 center, int direction, float t) {
        float radius = Vector3.Distance(center, b);
        float a_angle = Mathf.Atan2(a.position.z - center.z, a.position.x - center.x) * Mathf.Rad2Deg;
        float b_angle = Mathf.Atan2(b.z - center.z, b.x - center.x) * Mathf.Rad2Deg;
        if (direction * b_angle > direction * a_angle) {
            b_angle = b_angle - 360 * direction;
        }
        a_angle = Mathf.Lerp(a_angle, b_angle, t);
        a.position = new Vector3(Mathf.Cos(a_angle * Mathf.Deg2Rad) * radius + center.x, a.position.y,
            Mathf.Sin(a_angle * Mathf.Deg2Rad) * radius + center.z);
    }
}
