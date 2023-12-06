using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BallColor {
    Orange,
    Red,
    Blue,
    Green,
    White,
    Yellow,
    Purple,
    Pink
}

public enum Zone {
    Left,
    Right,
    Up,
    Down,
    Front,
    Back,
    In,
    Out
}

public struct MyStruct {

}

public class Ball : MonoBehaviour {
    //private static float size = 1;
    //private BallColor ballColor = BallColor.Orange;
    private Zone zone = Zone.Left;
    private Vector3 position = Vector3.zero;
    //private RotationAxis rotated;

    public Vector3 getPosition() {
        return position;
    }

    public void setPosition() {

    }

    public Zone getZone() {
        return zone;
    }

    public void setZone() {

    }
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.X)) {
            RotateXAxis();
        }
    }

    void RotateXAxis() {

    }
}

/*switch (zone)
        {
            case Zone.Left:
                switch (rotated)
                {
                    case RotationAxis.Y:
                        zone = Zone.Back;
                        break;
                    case RotationAxis.Z:
                        break;
                    default:
                        break;
                }
                break;
            case Zone.Right:
                switch (rotated)
                {
                    case RotationAxis.Y:
                        break;
                    case RotationAxis.Z:
                        break;
                    default:
                        break;
                }
                break;
            case Zone.Up:
                switch (rotated)
                {
                    case RotationAxis.X:
                        break;
                    case RotationAxis.Z:
                        break;
                    default:
                        break;
                }
                break;
            case Zone.Down:
                switch (rotated)
                {
                    case RotationAxis.X:
                        break;
                    case RotationAxis.Z:
                        break;
                    default:
                        break;
                }
                break;
            case Zone.Front:
                switch (rotated)
                {
                    case RotationAxis.X:
                        break;
                    case RotationAxis.Y:
                        break;
                    default:
                        break;
                }
                break;
            case Zone.Back:
                switch (rotated)
                {
                    case RotationAxis.X:
                        break;
                    case RotationAxis.Y:
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
}*/