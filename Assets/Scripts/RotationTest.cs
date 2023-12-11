using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;
using UnityEngine.UIElements;

public class RotationTest : MonoBehaviour
{
    
    [SerializeField]
    private GameObject sphere;
    Vector4 sphereCoords = new Vector4();
    Matrix4x4 rotationMatrix = Matrix4x4.identity;
    [SerializeField]
    float angle = 0;

    // Start is called before the first frame update
    void Start()
    {
        sphereCoords = new Vector4(sphere.transform.position.x,
        sphere.transform.position.y, sphere.transform.position.z, 1);
        UpdateRotationMatrix(angle);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateRotationMatrix(angle);
        sphereCoords = rotationMatrix * sphereCoords;
        print(sphereCoords);
        sphere.transform.position = new Vector3(sphereCoords.x, sphereCoords.y, sphereCoords.z);
    }

    void UpdateRotationMatrix(float angle) {
        rotationMatrix[0, 0] = Mathf.Cos(angle);
        rotationMatrix[2, 0] = -Mathf.Sin(angle);
        rotationMatrix[0, 2] = Mathf.Sin(angle);
        rotationMatrix[2, 2] = Mathf.Cos(angle);
    }
}
