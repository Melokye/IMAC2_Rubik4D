using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO must need to sort again
class Geometry {
    
    public enum Axis { x, y, z, w, none }

    Geometry(){
    }
    
    /// <summary>
    /// Create a vector of size 4 with a vector of size 3 and a value
    /// </summary>
    /// <param name="vec">the vector of size 3</param>
    /// <param name="value">the value we want to add to our new vector</param>
    /// <param name="pos">index to insert the value</param>
    /// <returns> Vector4 with value inserted at index pos </returns>
    public static Vector4 InsertFloat(Vector3 vec, float value, int pos) {
        // TODO rename the function?
        pos = Mathf.Clamp(pos, 0, 3); // TODO <- it's for DEBUG : to avoid out of range

        Vector4 result = Vector4.zero;
        switch (pos) {
            case 0:
                result = new Vector4(value, vec.x, vec.y, vec.z);
                break;
            case 1:
                result = new Vector4(vec.x, value, vec.y, vec.z);
                break;
            case 2:
                result = new Vector4(vec.x, vec.y, value, vec.z);
                break;
            case 3:
                result = new Vector4(vec.x, vec.y, vec.z, value);
                break;
            default:
                Debug.Log("InsertFloat: Insert position out of range!");
                break;
        }
        return result;
    }

    /// <summary>
    /// Generate a new rotationMatrix from two axes and an angle
    /// </summary>
    /// <param name="axis1"></param>
    /// <param name="axis2"></param>
    /// <param name="angle"></param>
    public static Matrix4x4 RotationMatrix(Axis axis1, Axis axis2, float angle) {
        int first = (int)axis1;
        int second = (int)axis2;

        Matrix4x4 rotationMatrix = Matrix4x4.identity;
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
            case 'W': return Axis.w;
            default:
                Debug.Log(axis + " isn't defined");
                return Axis.none; // TODO need gestion
        }
    }

    public static char IntToChar(int n) {
        switch (n) {
            case 0: return 'X';
            case 1: return 'Y';
            case 2: return 'Z';
            case 3: return 'W';
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
            case 'W': return 3 ;    
            default:
                Debug.Log("Char not matching an axis");
                return -1;    
        }
    }
    public static int AxisToInt(Axis axis) {
        return (int)axis; // TODO must delete it later
    }

    public static Axis IntToAxis(int n) {
        // TODO must delete later?
        return (Axis)n;
    }

    public static bool IsBetweenRangeExcluded(float value, float value1, float value2) {
        return value > Mathf.Min(value1, value2) && value < Mathf.Max(value1, value2);
    }

    /// <summary>
    /// Projects a 4D vector into 3D
    /// </summary>
    /// <param name="p"></param>
    /// <returns></returns>
    public static Vector4 Projection4DTo3D(Vector4 point) {
        // Vector4 point = new Vector4(p.x, p.y, p.z, p.w);
        // temp = cameraRotation * colorAssignment * temp; // TODO move it outside of the fn
        Vector3 projected = Vector3.zero;

        // Handle projection to infinity
        if (point.w + 1 != 0) {
            projected = new Vector3(point.x, point.y, point.z) / (point.w + 1);
        }else {
            projected = new Vector3(
                Mathf.Sign(point.x) * Int32.MaxValue,
                Mathf.Sign(point.y) * Int32.MaxValue,
                Mathf.Sign(point.z) * Int32.MaxValue
            );
        }
        return projected;
    }
}
