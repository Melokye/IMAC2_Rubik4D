using UnityEngine;

// TODO must need to sort again
class Geometry {
    public enum Axis { x, y, z, w, none }

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
    public static int AxisToInt(Axis axis) {
        return (int)axis; // TODO must delete it later
    }

    public static Axis IntToAxis(int n) {
        // TODO must delete later?
        return (Axis)n;
    }
}
