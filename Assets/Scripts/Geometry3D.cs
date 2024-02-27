using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Geometry3D : MonoBehaviour
{
    public enum Axis { x, y, z, none }

    Geometry3D(){
    }
    
    /// <summary>
    /// Create a vector of size 3 with a vector of size 2 and a value
    /// </summary>
    /// <param name="vec">the vector of size 2</param>
    /// <param name="value">the value we want to add to our new vector</param>
    /// <param name="pos">index to insert the value</param>
    /// <returns> Vector3 with value inserted at index pos </returns>
    public static Vector3 InsertFloat(Vector2 vec, float value, int pos) {
        /// \todo rename the function?
        pos = Mathf.Clamp(pos, 0, 2); /// \todo <- it's for DEBUG : to avoid out of range

        Vector3 result = Vector3.zero;
        switch (pos) {
            case 0:
                result = new Vector3(value, vec.x, vec.y);
                break;
            case 1:
                result = new Vector3(vec.x, value, vec.y);
                break;
            case 2:
                result = new Vector3(vec.x, vec.y, value);
                break;
            default:
                Debug.Log("InsertFloat: Insert position out of range!");
                break;
        }
        return result;
    }


    //todo : in 3d we only need 1 axis !!!!!!!
    /// <summary>
    /// Generate a new rotationMatrix from two axes and an angle
    /// </summary>
    /// <param name="axis1"></param>
    /// <param name="axis2"></param>
    /// <param name="angle"></param>
    public static Matrix4x4 RotationMatrix(Axis axis1, Axis axis2, float angle) {
        int first = (int)axis1;
        int second = (int)axis2;

        Matrix3x3 rotationMatrix = Matrix3x3.identity;
        rotationMatrix[first, first] = Mathf.Cos(angle * Mathf.Deg2Rad);
        rotationMatrix[second, first] = -Mathf.Sin(angle * Mathf.Deg2Rad);
        rotationMatrix[first, second] = Mathf.Sin(angle * Mathf.Deg2Rad);
        rotationMatrix[second, second] = Mathf.Cos(angle * Mathf.Deg2Rad);
        return rotationMatrix;
    }

    public static Axis CharToAxis(char axis) {
        switch (axis) {
            case 'X': return Axis.x;
            case 'Y': return Axis.y;
            case 'Z': return Axis.z;
            default:
                Debug.Log(axis + " isn't defined");
                return Axis.none; /// \todo need gestion
        }
    }

    public static char IntToChar(int n) {
        switch (n) {
            case 0: return 'X';
            case 1: return 'Y';
            case 2: return 'Z';
            default:
                Debug.Log("Int not matching an axis");
                return  'o';
        }
    }

    public static int CharToInt(char c) {
        switch(c) {
            case 'X': return 0 ;
            case 'Y': return 1 ;
            case 'Z': return 2 ;   
            default:
                Debug.Log("Char not matching an axis");
                return -1;    
        }
    }
    public static int AxisToInt(Axis axis) {
        return (int)axis; /// \todo must delete it later
    }

    public static Axis IntToAxis(int n) {
        /// \todo must delete later?
        return (Axis)n;
    }

    public static bool IsBetweenRangeExcluded(float value, float value1, float value2) {
        return value > Mathf.Min(value1, value2) && value < Mathf.Max(value1, value2);
    }

    /// <summary>
    /// Projects a 3D vector into 2D
    /// </summary>
    /// <param name="point"></param>
    /// <returns></returns>
    public static Vector2 Projection3DTo2D(Vector3 point) {
        Vector3 projected = Vector3.zero;

        // Handle projection to infinity
        if (point.z + 1 != 0) {
            projected = new Vector3(point.x, point.y,0) / (point.z + 1f); /// \todo : add it in the presentation
        }
        else {
            projected = new Vector3(
                Mathf.Sign(point.x) * Int16.MaxValue,
                Mathf.Sign(point.y) * Int16.MaxValue,
                0
            );
        }
        return projected;
    }

    /// <summary>
    /// Projects a 3D vector into 2D orthographically
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public static Vector2 Projection3DTo2DOrthographic(Vector3 point) {
        // Vector4 point = new Vector4(p.x, p.y, p.z, p.w);
        // temp = cameraRotation * colorAssignment * temp; /// \todo move it outside of the fn
        Vector3 projected = Vector3.zero;
        projected = new Vector3(point.x, point.y,0);
        return projected;
    }
}
