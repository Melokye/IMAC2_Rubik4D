using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class Geometry
{
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
                break;
        }
        return result;
    }   
}