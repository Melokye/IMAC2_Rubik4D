using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Ball: MonoBehaviour {
    [SerializeField]
    private GameObject[] balls;
    [SerializeField]
    private GameObject center;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        foreach (GameObject ball in balls)
        {
            ball.transform.RotateAround(center.transform.position, Vector3.up, 20 * Time.deltaTime);

        }
    }
}
