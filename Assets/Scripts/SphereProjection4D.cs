using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereProjection4D : MonoBehaviour
{
    [SerializeField]
    private Mesh sphereMesh;
    List<List<float>> points = new List<List<float>>();
    List<List<float>> points3D = new List<List<float>>();
    // Start is called before the first frame update
    void Start()
    {
        List<string> materials = new List<string>() {
            "Red", "Orange", "Blue", "Green", "Yellow", "White", "Purple", "Pink" };
        List<string> names = new List<string>() {
            "Right", "Left", "Up", "Down", "Back", "Front", "In", "Out" };
        List<float> p1 = new List<float>() { 1, 0, 0, 0 };
        List<float> p2 = new List<float>() { -1, 0, 0, 0 };
        List<float> p3 = new List<float>() { 0, 1, 0, 0 };
        List<float> p4 = new List<float>() { 0, -1, 0, 0 };
        List<float> p5 = new List<float>() { 0, 0, 1, 0 };
        List<float> p6 = new List<float>() { 0, 0, -1, 0 };
        List<float> p7 = new List<float>() { 0, 0, 0, 1 };
        List<float> p8 = new List<float>() { 0, 0, 0, -1 };
        points.Add(p1);
        points.Add(p2);
        points.Add(p3);
        points.Add(p4);
        points.Add(p5);
        points.Add(p6);
        points.Add(p7);
        points.Add(p8);

        for (int i = 0; i < points.Count; i++) {
            points3D.Add(Projection4DTo3D(points[i]));
            print(points[i]);
            GameObject sphere = new GameObject();
            Vector3 spherePos = new Vector3(points[i][0], points[i][1], points[i][2]);
            Material sphereMat = Resources.Load(materials[i], typeof(Material)) as Material;
            sphere.AddComponent<MeshFilter>();
            sphere.AddComponent<MeshRenderer>();
            sphere.GetComponent<MeshFilter>().mesh = sphereMesh;
            sphere.GetComponent<Renderer>().material = sphereMat;
            sphere.name = names[i];
            sphere.transform.localScale = 0.2f * Vector3.one;
            sphere.transform.position = spherePos;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    List<float> Projection4DTo3D(List<float> point) {
        return new List<float>() { point[0], point[1], point[2] };
    }
}
