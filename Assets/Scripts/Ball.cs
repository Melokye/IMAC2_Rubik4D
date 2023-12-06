using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

using UnityEngine;
using System.Collections;

public class Ball: MonoBehaviour {
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
            Move(ball);
        }
    }

    void Move(GameObject ball){
        ball.transform.RotateAround(center.transform.position, Vector3.up, 60 * Time.deltaTime);
    }
}
