using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager : MonoBehaviour {
    [SerializeField]
    private GameObject[] balls = new GameObject[6];
    private List<Transform> cubes = new List<Transform>();
    private Vector3[] positions;
    private Vector3[] positionsX;
    private Vector3[] positionsY;
    private Vector3[] positionsZ;

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
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.X)) {
            print("X");
            MoveCubes(positionsX);
        }
        if (Input.GetKeyDown(KeyCode.Y)) {
            print("Y");
            MoveCubes(positionsY);
        }
        if (Input.GetKeyDown(KeyCode.W)) {
            print("Z");
            MoveCubes(positionsZ);
        }
        /*foreach (GameObject ball in balls)
        {
            ball.transform.RotateAround(center.transform.position, Vector3.up, 60 * Time.deltaTime);
        }*/
    }

    void MoveCubes(Vector3[] positions) {
        Dictionary<Transform, Vector3> positionBuffer = new Dictionary<Transform, Vector3>();
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
        foreach (KeyValuePair<Transform, Vector3> entry in positionBuffer) {
            entry.Key.position = entry.Value;
        }
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