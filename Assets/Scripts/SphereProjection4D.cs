using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class SphereProjection4D : MonoBehaviour
{
    [SerializeField]
    private Mesh sphereMesh;
    List<List<float>> points = new List<List<float>>();
    List<List<float>> points3D = new List<List<float>>();
    List<List<float>> shifts = new List<List<float>>();
    List<string> materials = new List<string>() {
            "Red", "Orange", "Blue", "Green", "Yellow", "White", "Purple", "Pink" };
    List<string> names = new List<string>() {
            "Right", "Left", "Up", "Down", "Back", "Front", "In", "Out" };
    [SerializeField]
    GameObject container;
    int selectedCube = -1;

    // Start is called before the first frame update
    void Start()
    {
        List<float> p1 = new List<float>() { 1, 0, 0, 0 };
        List<float> p2 = new List<float>() { -1, 0, 0, 0 };
        List<float> p3 = new List<float>() { 0, 1, 0, 0 };
        List<float> p4 = new List<float>() { 0, -1, 0, 0 };
        List<float> p5 = new List<float>() { 0, 0, 1, 0 };
        List<float> p6 = new List<float>() { 0, 0, -1, 0 };
        List<float> p7 = new List<float>() { 0, 0, 0, 1 };
        List<float> p8 = new List<float>() { 0, 0, 0, -1 };
        List<float> shift1 = new List<float>() { -0.1f, 0.1f, 0.1f, 0 };
        List<float> shift2 = new List<float>() { 0.1f, 0.1f, 0.1f, 0 };
        List<float> shift3 = new List<float>() { -0.1f, 0.1f, -0.1f, 0 };
        List<float> shift4 = new List<float>() { 0.1f, 0.1f, -0.1f, 0 };
        List<float> shift5 = new List<float>() { -0.1f, -0.1f, 0.1f, 0 };
        List<float> shift6 = new List<float>() { 0.1f, -0.1f, 0.1f, 0 };
        List<float> shift7 = new List<float>() { -0.1f, -0.1f, -0.1f, 0 };
        List<float> shift8 = new List<float>() { 0.1f, -0.1f, -0.1f, 0 };
        points.Add(p1);
        points.Add(p2);
        points.Add(p3);
        points.Add(p4);
        points.Add(p5);
        points.Add(p6);
        points.Add(p7);
        points.Add(p8);
        shifts.Add(shift1);
        shifts.Add(shift2);
        shifts.Add(shift3);
        shifts.Add(shift4);
        shifts.Add(shift5);
        shifts.Add(shift6);
        shifts.Add(shift7);
        shifts.Add(shift8);

        container.name = "Container";
        for (int i = 0; i < points.Count; i++) {
            GameObject parent = new GameObject();
            parent.transform.parent = container.transform;
            Vector3 parentPos = new Vector3(points[i][0], points[i][1], points[i][2]);
            parent.name = names[i];
            parent.transform.position = parentPos;
            for (int j = 0; j < 8; j++) {
                points3D.Add(Projection4DTo3D(AddFloatList(points[i], shifts[j])));
                GameObject sphere = new GameObject();
                Vector3 spherePos = new Vector3(
                    points3D[8 * i + j][0], points3D[8 * i + j][1], points3D[8 * i + j][2]);
                Material sphereMat = Resources.Load(materials[i], typeof(Material)) as Material;
                sphere.AddComponent<MeshFilter>();
                sphere.AddComponent<MeshRenderer>();
                sphere.GetComponent<MeshFilter>().mesh = sphereMesh;
                sphere.GetComponent<Renderer>().material = sphereMat;
                sphere.name = names[i] + "_" + j;
                sphere.transform.localScale = 0.2f * Vector3.one;
                sphere.transform.position = spherePos;
                sphere.transform.parent = parent.transform;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        /*Controls(container, selectedCube);
        if (selectedCube == -1) selectedCube = GetPressedNumber();*/
    }

    List<float> Projection4DTo3D(List<float> point) {
        return new List<float>() { point[0], point[1], point[2] };
    }

    List<float> AddFloatList(List<float> a, List<float> b) {
        List<float> result = new List<float>();
        for (int i = 0; i < a.Count; i++) {
            result.Add(a[i] + b[i]);
        }
        return result;
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
