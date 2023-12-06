using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using static UnityEngine.GraphicsBuffer;
using static UnityEngine.GridBrushBase;

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
    private Transform currentAxis;
    private bool cubeMoving = false;
    private Dictionary<Transform, Vector3> positionBuffer = new Dictionary<Transform, Vector3>();


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
        foreach (Transform circle in transform.Find("Circles")) {
            circles.Add(circle);
        }
        StartCoroutine(RotateCubesOverTime(positionBuffer));
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.X) && !cubeMoving) {
            print("X");
            MoveCubes(positionsX, RotationAxis.X);
            currentAxis = circles[(int)RotationAxis.X];
            cubeMoving = true;
        }
        if (Input.GetKeyDown(KeyCode.Y) && !cubeMoving) {
            print("Y");
            MoveCubes(positionsY, RotationAxis.Y);
            currentAxis = circles[(int)RotationAxis.Y];
            cubeMoving = true;
        }
        if (Input.GetKeyDown(KeyCode.W) && !cubeMoving) {
            print("Z");
            MoveCubes(positionsZ, RotationAxis.Z);
            currentAxis = circles[(int)RotationAxis.Z];
            cubeMoving = true;
        }
    }

    void MoveCubes(Vector3[] positions, RotationAxis axis) {
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
    }

    
    private bool IsAllTrue(List<bool> list) {
        for (int i = 0; i < list.Count; i++) {
            if (list[i] == false) {
                return false;
            }
        }
        return true;
    }

    private IEnumerator RotateCubesOverTime(Dictionary<Transform, Vector3> buffer) {
        while (true) {
            if (!cubeMoving) {
                yield return null;
            }
            else {
                Vector3 circleAxis = currentAxis.position;
                /*List<Vector3> startPosition = new List<Vector3>();
                List<Vector3> endPosition = new List<Vector3>();
                

                // Calculate the rotation direction based on the axis
                List<Vector3> rotationDirection = new List<Vector3>();

                // Calculate the angles between the start and end positions
                List<float> angleStart = new List<float>();
                List<float> angleEnd = new List<float>();

                int count = 0;

                foreach (KeyValuePair<Transform, Vector3> entry in buffer) {
                    startPosition.Add(entry.Key.position);
                    endPosition.Add(entry.Value);

                    // Calculate the rotation direction based on the axis
                    rotationDirection.Add(Vector3.Cross(startPosition[count] - circleAxis, Vector3.up).normalized);

                    // Calculate the angles between the start and end positions
                    angleStart.Add(Vector3.SignedAngle(startPosition[count] - circleAxis, rotationDirection[count], Vector3.up));
                    angleEnd.Add(Vector3.SignedAngle(endPosition[count] - circleAxis, rotationDirection[count], Vector3.up));

                    count++;
                }*/
                List<bool> destinationReached = new List<bool>();
                int count = 0;
                foreach (KeyValuePair<Transform, Vector3> entry in buffer) {
                    destinationReached.Add(false);
                    count++;
                }
                while (!IsAllTrue(destinationReached)) {
                    count = 0;
                    foreach (KeyValuePair<Transform, Vector3> entry in buffer) {
                        // Calculate the rotation direction based on the axis
                        Vector3 rotationDirection = Vector3.Cross(entry.Key.position - circleAxis, Vector3.up).normalized;

                        // Calculate the angles between the start and end positions
                        float angleStart = Vector3.SignedAngle(entry.Key.position - circleAxis, rotationDirection, Vector3.up);
                        float angleEnd = Vector3.SignedAngle(entry.Value - circleAxis, rotationDirection, Vector3.up);
                        print(angleStart);
                        print(angleEnd);
                        //entry.Key.position = Vector3.Slerp(entry.Key.position - circleAxis,
                        //    entry.Value - circleAxis, Time.deltaTime * 5f) + circleAxis;
                        //if (Vector3.Distance(entry.Key.position, entry.Value) < Vector3.kEpsilon) {
                            destinationReached[count] = true;
                        //}
                        count++;
                    }
                    yield return null;
                }

                yield return null;

                // Ensure the cubes reach their final positions
                /*foreach (KeyValuePair<Transform, Vector3> entry in buffer) {
                    entry.Key.position = entry.Value;
                }*/

                cubeMoving = false;
                positionBuffer.Clear();
            }
        }
    }
}