using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SphereProjection4D : MonoBehaviour
{
    List<Vector4> points = new List<Vector4>();
    List<Vector4> targets = new List<Vector4>();
    Matrix4x4 rotationMatrix = Matrix4x4.identity;
    bool cubeRotating = false;
    List<string> names = new List<string>() {
            "Right", "Left", "Up", "Down", "Back", "Front", "In", "Out" };
    List<string> materials = new List<string>() {
            "Red", "Orange", "Blue", "Green", "Yellow", "White", "Purple", "Pink" };

    [SerializeField]
    private Mesh sphereMesh;
    [SerializeField]
    GameObject container;
    [SerializeField]
    float rotationSpeed = 0.1f;
    [SerializeField]
    int axis1 = 0;
    [SerializeField]
    int axis2 = 1;
    [SerializeField]
    private int direction = 1;
    
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

    // Start is called before the first frame update
    void Start()
    {
        Vector4 p1 = new Vector4( 1, 0, 0, 0);
        Vector4 p2 = new Vector4(-1, 0, 0, 0);
        Vector4 p3 = new Vector4( 0, 1, 0, 0);
        Vector4 p4 = new Vector4( 0,-1, 0, 0);
        Vector4 p5 = new Vector4( 0, 0, 1, 0);
        Vector4 p6 = new Vector4( 0, 0,-1, 0);
        Vector4 p7 = new Vector4( 0, 0, 0, 1);
        Vector4 p8 = new Vector4( 0, 0, 0,-1);
        /*List<float> shift1 = new List<float>() { -0.1f, 0.1f, 0.1f, 0 };
        List<float> shift2 = new List<float>() { 0.1f, 0.1f, 0.1f, 0 };
        List<float> shift3 = new List<float>() { -0.1f, 0.1f, -0.1f, 0 };
        List<float> shift4 = new List<float>() { 0.1f, 0.1f, -0.1f, 0 };
        List<float> shift5 = new List<float>() { -0.1f, -0.1f, 0.1f, 0 };
        List<float> shift6 = new List<float>() { 0.1f, -0.1f, 0.1f, 0 };*/
        //List<float> shift7 = new List<float>() { -0.1f, -0.1f, -0.1f, 0 };
        //List<float> shift8 = new List<float>() { 0.1f, -0.1f, -0.1f, 0 };
        points.Add(p1);
        points.Add(p2);
        points.Add(p3);
        points.Add(p4);
        points.Add(p5);
        points.Add(p6);
        points.Add(p7);
        points.Add(p8);
        /*shifts.Add(shift1);
        shifts.Add(shift2);
        shifts.Add(shift3);
        shifts.Add(shift4);
        shifts.Add(shift5);
        shifts.Add(shift6);*/
        //shifts.Add(shift7);
        //shifts.Add(shift8);

        container.name = "Container";
        /*for (int i = 0; i < points.Count; i++) {
            GameObject parent = new GameObject();
            parent.transform.parent = container.transform;
            //Vector3 parentPos = new Vector3(points[i][0], points[i][1], points[i][2]);
            parent.name = names[i];
            parent.transform.position = parentPos;*/

        for (int j = 0; j < points.Count; j++) {
            GameObject sphere = new GameObject();
            Material sphereMat = Resources.Load(materials[j], typeof(Material)) as Material;
            sphere.AddComponent<MeshFilter>();
            sphere.AddComponent<MeshRenderer>();
            sphere.GetComponent<MeshFilter>().mesh = sphereMesh;
            sphere.GetComponent<Renderer>().material = sphereMat;
            sphere.name = names[j];
            sphere.transform.localScale = 0.2f * Vector3.one;
            sphere.transform.parent = container.transform;
            sphere.transform.position = Projection4DTo3D(points[j]);
        }

        UpdateRotationMatrix(axis1, axis2, 0);
        StartCoroutine(Rotate90Degrees());
    }

    // Update is called once per frame
    void Update()
    {
        if (!cubeRotating) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                direction = -direction;
            }
            if (Input.GetKeyDown(KeyCode.R)) {
                targets.Clear();
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
                UpdateRotationMatrix(axis1, axis2, 90);
                foreach (Transform child in container.transform) {
                    targets.Add(rotationMatrix * points[i]);
                    i++;
                }
                i = 0;
                UpdateRotationMatrix(axis1, axis2, rotationSpeed);
                while (Vector4.Distance(points[0], targets[0]) > Vector4.kEpsilon) {
                    i = 0;
                    foreach (Transform child in container.transform) {
                        points[i] = rotationMatrix * points[i];
                        child.transform.position = Projection4DTo3D(points[i]);
                        i++;
                    }
                    yield return null;
                }
                cubeRotating = false;
            }
        }
    }

    Vector4 Projection4DTo3D(Vector4 point) {
        Vector4 temp = new Vector4(point.x, point.y, point.z, point.w);
        temp = cameraRotation * colorAssignment * temp;
        return new Vector3(temp[0], temp[1], temp[2]) / (temp[3] + 1);
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
        //print(a.name + ": " + "a_angle: " + a_angle + ", b_angle: " + b_angle);
        a_angle = Mathf.Lerp(a_angle, b_angle, t);
        a.position = new Vector3(Mathf.Cos(a_angle * Mathf.Deg2Rad) * radius + center.x, a.position.y,
            Mathf.Sin(a_angle * Mathf.Deg2Rad) * radius + center.z);
    }

    /*void Controls(GameObject c, int selectedCube) {
        switch (selectedCube) {
            case 0:
                print("Right Selected!!");
                if (Input.GetKeyDown(KeyCode.X)) {
                    GameObject center = GameObject.Find(
                        "/" + container.name + "/" + names[selectedCube]);
                    foreach (Transform child in c.transform) {
                        foreach (Transform subchild in child) {
                            if (subchild.transform.position.x > 0) {
                                subchild.RotateAround(center.transform.position, Vector3.right, 90f);
                            }
                        }
                    }
                    selectedCube = -1;
                }
                if (Input.GetKeyDown(KeyCode.Y)) {
                    selectedCube = -1;
                }
                if (Input.GetKeyDown(KeyCode.W)) {
                    selectedCube = -1;
                }
                break;
            case 1:

            case 2:

            case 3:

            case 4:

            case 5:

            case 6:

            case 7:

            default:
                break;
        }
    }

    void Rotate(bool,) {

    }

    int GetPressedNumber() {
        for (int number = 0; number <= 7; number++) {
            if (Input.GetKeyDown("[" + number.ToString() + "]"))
                return number;
        }

        return -1;
    }*/
}
