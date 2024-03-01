using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coords4D : MonoBehaviour
{
    [SerializeField]
    private Vector4 coordinates;
    private int cellNumber; // To know which axis the sticker belongs to

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

    /// <summary>
    /// Setter of the cellNumber of the object.
    /// </summary>
    /// <param name="cell"> An int to set from. </param>
    public void SetCellNumber(int cell) {
        cellNumber = cell;
    }
}
