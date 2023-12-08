using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereProjection4D : MonoBehaviour
{
    [SerializeField]
    private Mesh sphereMesh;
    [SerializeField]
    private GameObject sphere;

    [SerializeField]
    private string input = "y";
    List<List<float>> points = new List<List<float>>();
    List<List<float>> points3D = new List<List<float>>();
    Matrix4x4 rotationMatrix = Matrix4x4.identity;

    List<string> materials = new List<string>() {
            "Red", "Orange", "Blue", "Green", "Yellow", "White", "Purple", "Pink" };
        List<string> names = new List<string>() {
            "Right", "Left", "Up", "Down", "Back", "Front", "In", "Out" };
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
        bigRotation(sphere,input);
    }

    List<float> Projection4DTo3D(List<float> point) {
        return new List<float>() { point[0], point[1], point[2] };
    }

    void baseRotation(GameObject sphere, string input){
        if(input == "y"){
        rotationMatrix[0, 0] = Mathf.Cos(0.1f);
        rotationMatrix[2, 0] = -Mathf.Sin(0.1f);
        rotationMatrix[0, 2] = Mathf.Sin(0.1f);
        rotationMatrix[2, 2] = Mathf.Cos(0.1f);
        }
        if(input == "x"){
        rotationMatrix[1, 1] = Mathf.Cos(0.1f);
        rotationMatrix[2, 1] = -Mathf.Sin(0.1f);
        rotationMatrix[1, 2] = Mathf.Sin(0.1f);
        rotationMatrix[2, 2] = Mathf.Cos(0.1f);
        }
        if(input == "z"){
        rotationMatrix[0, 0] = Mathf.Cos(0.1f);
        rotationMatrix[1, 0] = -Mathf.Sin(0.1f);
        rotationMatrix[0, 1] = Mathf.Sin(0.1f);
        rotationMatrix[1, 1] = Mathf.Cos(0.1f);
        }
        Vector3 sphereCoords = sphere.transform.position;
        sphereCoords = rotationMatrix * sphereCoords;
        sphere.transform.position = sphereCoords;
    }

    void bigRotation(GameObject sphere, string input){
        List<string> toBeRotated = new List<string>(6);
        toBeRotated = whosGunnaRotate(sphere.name);
        foreach(string entry in toBeRotated){
            baseRotation(GameObject.Find(entry),input);
        }
    }

    string whosOpposite(string sphereName){
        int index = names.IndexOf(sphereName);
        return (index%2 ==0 ) ? names[index + 1] : names[index - 1];    
    }

    List<string> whosGunnaRotate(string sphereName){
        List<string> list = new List<string>();
        string opposite = whosOpposite(sphereName);
        foreach(string entry in names){
            if(entry != sphereName & entry != opposite){
                list.Add(entry);
            }
        }
        return list;
    }

}
