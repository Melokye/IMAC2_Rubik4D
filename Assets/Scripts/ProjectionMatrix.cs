using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectionMatrix : MonoBehaviour {
    [SerializeField]
    private Matrix4x4 matrix;

    /// <summary>
    /// Getter of the projection matrix of the object.
    /// </summary>
    /// <returns></returns>
    public Matrix4x4 GetMatrix() {
        return matrix;
    }

    /// <summary>
    /// Setter of the projection matrix of the object.
    /// </summary>
    /// <param name="Coordinates"> A Unity.Vector4 to set from. </param>
    public void SetMatrix(Matrix4x4 newMatrix) {
        matrix = newMatrix;
    }
}
