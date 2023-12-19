using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords4D : MonoBehaviour
{
    [SerializeField]
    private Vector4 coordinates;
    public static Vector4 selectedCoordinates;

    /// <summary>
    /// Getter of the 4D coordinates of the object.
    /// </summary>
    /// <returns></returns>
    public Vector4 GetCoordinates() {
        return coordinates;
    }

    /// <summary>
    /// Setter of the 4D coordinates of the object.
    /// </summary>
    /// <param name="Coordinates"> A Unity.Vector4 to set from. </param>
    public void SetCoordinates(Vector4 Coordinates) {
        coordinates = Coordinates;
    }
}
