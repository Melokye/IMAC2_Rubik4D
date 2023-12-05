using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GameManager: MonoBehaviour {
    [SerializeField]
    private GameObject[] balls = new GameObject[8];
    [SerializeField]
    private GameObject center;
    [SerializeField]
    private int number;

    private void Start() {
    }

    // Update is called once per frame
    void Update() {
        foreach (GameObject ball in balls)
        {
            ball.transform.RotateAround(center.transform.position, Vector3.up, 60 * Time.deltaTime);
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