using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;

public enum RotationAxis {
    X,
    Y,
    Z
}

public class GameManager : MonoBehaviour {
    [SerializeField]
    private GameObject[] balls = new GameObject[6];
    private List<Transform> cubes = new List<Transform>();
    private List<Transform> circles = new List<Transform>();
    private Vector3[] positions;
    private Vector3[] positionsX;
    private Vector3[] positionsY;
    private Vector3[] positionsZ;
    List<Dictionary<Transform, Vector3>> positionBuffer = new List<Dictionary<Transform, Vector3>>();
    List<Transform> circleBuffer = new List<Transform>();


    //[SerializeField]
    //private GameObject center;
    //[SerializeField]
    //private int number;

    private void Start() {
        positions = balls.Select(o => o.transform.position).ToArray();
        positionsX = new Vector3[] {
            positions[3],
            positions[4],
            positions[2],
            positions[5]
        };
        positionsY = new Vector3[] {
            positions[5],
            positions[1],
            positions[4],
            positions[0]
        };
        positionsZ = new Vector3[] {
            positions[0],
            positions[2],
            positions[1],
            positions[3]
        };
        foreach (Transform cube in transform.Find("Cubes")) {
            cubes.Add(cube);
        }
        foreach (Transform circle in transform.Find("Circle")) {
            circles.Add(circle);
        }
    }

    void Update() {
        ProcessAnimations(positionBuffer, circleBuffer);
        if (Input.GetKeyDown(KeyCode.X)) {
            print("X");
            (Dictionary<Transform, Vector3> tempPositions,
                Transform tempCircle) = MoveCubes(positionsX, RotationAxis.X);
            positionBuffer.Add(tempPositions);
            circleBuffer.Add(tempCircle);
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            print("Y");
            (Dictionary<Transform, Vector3> tempPositions,
                Transform tempCircle) = MoveCubes(positionsY, RotationAxis.Y);
            positionBuffer.Add(tempPositions);
            circleBuffer.Add(tempCircle);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            print("Z");
            (Dictionary<Transform, Vector3> tempPositions,
                Transform tempCircle) = MoveCubes(positionsZ, RotationAxis.Z);
            positionBuffer.Add(tempPositions);
            circleBuffer.Add(tempCircle);
        }
    }

    void ProcessAnimations(List<Dictionary<Transform, Vector3>> positionBuffer,
        List<Transform> circleBuffer) {
        if (positionBuffer.Count == 1) {
            foreach (KeyValuePair<Transform, Vector3> entry in positionBuffer[0]) {
                entry.Key.position = entry.Value;
            }
            //C = Vector3.Slerp(A - O, B - O, lerpValue) + O;
            //Vector3.Slerp(circleBuffer[0].position, Vector3.up, 60 * Time.deltaTime);
        }
    }

    (Dictionary<Transform, Vector3>, Transform) MoveCubes(Vector3[] positions, RotationAxis axis) {
        Dictionary<Transform, Vector3> positionBuffer = new Dictionary<Transform, Vector3>();
        List<Transform> circleBuffer = new List<Transform>();
        for (int i = 0; i < positions.Length; i++) {
            foreach (Transform cube in cubes) {
                if (Vector3.Distance(positions[i], cube.position) < Vector3.kEpsilon) {
                    if (i == positions.Length - 1) {
                        positionBuffer.Add(cube, positions[0]);
                    }
                    else {
                        positionBuffer.Add(cube, positions[i + 1]);
                    }
                }
            }
        }
        return (positionBuffer, circles[(int)axis]);
        /*foreach (KeyValuePair<Transform, Vector3> entry in positionBuffer) {
            entry.Key.RotateAround(circles[(int)axis].position, Vector3.up, 60 * Time.deltaTime);
            entry.Key.position = entry.Value;
        }*/
    }
}

/*GameObject[] things = GameObject.FindObjectsOfType<GameObject>();
        Debug.Log(GameObject.FindObjectsOfType<GameObject>());
        foreach (GameObject ball in things)
        {
            Debug.Log(ball.name);
            if (ball.name.Contains("1_Left"))
            {
                balls[0].Append(ball);
            }
            if (ball.name.Contains("2_Right"))
            {
                balls[1].Append(ball);
            }
            if (ball.name.Contains("3_Up"))
            {
                balls[2].Append(ball);
            }
            if (ball.name.Contains("4_Down"))
            {
                balls[3].Append(ball);
            }
            if (ball.name.Contains("5_Front"))
            {
                balls[4].Append(ball);
            }
            if (ball.name.Contains("6_Back"))
            {
                balls[5].Append(ball);
            }
            if (ball.name.Contains("7_In"))
            {
                balls[6].Append(ball);
            }
            if (ball.name.Contains("8_Out"))
            {
                Debug.Log("hey bro what's up");
                balls[7].Append(ball);
            }
        }*/