using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using UnityEngine;

public enum RotationAxis {
    X,
    Y,
    Z,
    WX,
    WY,
    WZ
}

public class GameManager : MonoBehaviour {
    [SerializeField]
    private GameObject[] balls = new GameObject[8];
    private List<Transform> cubes = new List<Transform>();
    private List<Transform> circles = new List<Transform>();
    private Vector3[] positions;
    private Vector3[] positionsX;
    private Vector3[] positionsY;
    private Vector3[] positionsZ;
    private Vector3[] positionsWX;
    private Vector3[] positionsWY;
    private Vector3[] positionsWZ;
    private Transform currentAxis;
    private bool cubeMoving = false;
    private Dictionary<Transform, Vector3> positionBuffer = new Dictionary<Transform, Vector3>();
    private int direction = 1;
    private int dimension = 1;

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
        positionsWX = new Vector3[] {
            positions[3],
            positions[6],
            positions[2],
            positions[7]
        };
        positionsWY = new Vector3[] {
            positions[5],
            positions[6],
            positions[4],
            positions[7]
        };
        positionsWZ = new Vector3[] {
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
        if (!cubeMoving) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                direction = -direction;
            }
            if (Input.GetKeyDown(KeyCode.LeftControl)) {
                dimension = -dimension;
            }
            if (dimension == 1) {
                if (Input.GetKeyDown(KeyCode.X)) {
                    AddCubes(positionsX);
                    currentAxis = circles[(int)RotationAxis.X];
                    cubeMoving = true;
                }
                if (Input.GetKeyDown(KeyCode.Y)) {
                    AddCubes(positionsY);
                    currentAxis = circles[(int)RotationAxis.Y];
                    cubeMoving = true;
                }
                if (Input.GetKeyDown(KeyCode.W)) {
                    AddCubes(positionsZ);
                    currentAxis = circles[(int)RotationAxis.Z];
                    cubeMoving = true;
                }
            }
            if (dimension == -1) {
                if (Input.GetKeyDown(KeyCode.X)) {
                    AddCubes(positionsWX);
                    currentAxis = circles[(int)RotationAxis.WX];
                    cubeMoving = true;
                }
                if (Input.GetKeyDown(KeyCode.Y)) {
                    AddCubes(positionsWY);
                    currentAxis = circles[(int)RotationAxis.WY];
                    cubeMoving = true;
                }
                if (Input.GetKeyDown(KeyCode.W)) {
                    AddCubes(positionsWZ);
                    currentAxis = circles[(int)RotationAxis.WZ];
                    cubeMoving = true;
                }
            }
        }
    }

    void AddCubes(Vector3[] positions) {
        for (int i = 0; i < positions.Length; i++) {
            foreach (Transform cube in cubes) {
                if (Vector3.Distance(positions[i], cube.position) < Vector3.kEpsilon) {
                    if (direction == 1) {
                        if (i == positions.Length - 1) {
                            positionBuffer.Add(cube, positions[0]);
                        }
                        else {
                            positionBuffer.Add(cube, positions[i + 1]);
                        }
                    }
                    if (direction == -1) {
                        if (i == 0) {
                            positionBuffer.Add(cube, positions[positions.Length - 1]);
                        }
                        else {
                            positionBuffer.Add(cube, positions[i - 1]);
                        }
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
                List<bool> destinationReached = new List<bool>();
                int count = 0;
                foreach (KeyValuePair<Transform, Vector3> entry in buffer) {
                    destinationReached.Add(false);
                    count++;
                }
                while (!IsAllTrue(destinationReached)) {
                    count = 0;
                    foreach (KeyValuePair<Transform, Vector3> entry in buffer) {
                        if (dimension == 1) {
                            RotateAroundTowards(entry.Key, entry.Value, circleAxis, direction, Time.deltaTime * 10f);
                        }
                        if (dimension == -1) {
                            RotateAroundTowards4D(entry.Key, entry.Value, circleAxis, direction, Time.deltaTime * 10f);
                        }
                        if (Vector3.Distance(entry.Key.position, entry.Value) < 500f * Vector3.kEpsilon) {
                            destinationReached[count] = true;
                        }
                        count++;
                    }
                    yield return null;
                }

                yield return null;

                foreach (KeyValuePair<Transform, Vector3> entry in buffer) {
                    entry.Key.position = entry.Value;
                }

                cubeMoving = false;
                positionBuffer.Clear();
            }
        }
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

    private void RotateAroundTowards4D(Transform a, Vector3 b, Vector3 center, int direction, float t) {
        float radius = Vector3.Distance(center, b);
        float a_angle = Mathf.Atan2(a.position.y - center.y, a.position.x - center.x) * Mathf.Rad2Deg;
        float b_angle = Mathf.Atan2(b.y - center.y, b.x - center.x) * Mathf.Rad2Deg;
        if (direction * b_angle > direction * a_angle) {
            b_angle = b_angle - 360 * direction;
        }
        a_angle = Mathf.Lerp(a_angle, b_angle, t);
        a.position = new Vector3(Mathf.Cos(a_angle * Mathf.Deg2Rad) * radius + center.x,
            Mathf.Sin(a_angle * Mathf.Deg2Rad) * radius + center.y, a.position.z);
    }
}
