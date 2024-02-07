using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords3D : MonoBehaviour
{
    [SerializeField]
    private Vector3 coordinates;

    /// <summary>
    /// Getter of the 3D coordinates of the object.
    /// </summary>
    /// <returns></returns>
    public Vector3 GetCoordinates() {
        return coordinates;
    }

    /// <summary>
    /// Setter of the 4D coordinates of the object.
    /// </summary>
    /// <param name="Coordinates"> A Unity.Vector4 to set from. </param>
    public void SetCoordinates(Vector3 Coordinates) {
        coordinates = Coordinates;
    }
}
