using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class RingsRepresentation: MonoBehaviour{
    List<Tuple<int, int>> rotationAxes; // = Rings
    RingsRepresentation(){
        // TODO convert into loop?
        // rotationAxes = new List<Tuple<int, int>>() {
        //     Tuple.Create(0, 1), Tuple.Create(0, 2), Tuple.Create(1, 0),
        //     Tuple.Create(2, 1), Tuple.Create(1, 3), Tuple.Create(3, 1),
        //     Tuple.Create(2, 3), Tuple.Create(0, 3)
        // };
    }

    /// <summary>
    /// Draw the axis circles on the 3D space
    /// // TODO add more info
    /// </summary>    
    /// <param name="name">Name of the circles</param>
    public static GameObject RenderCircles(string name, Puzzle p) { //TODO
        List<Vector4> tempstickers = new List<Vector4>();
        List<Vector3> vertices = new List<Vector3>();

        // copy position from actual stickers // TODO?
        for (int i = 0; i < p.NbStickers(0); i++) {
            tempstickers.Add(p.GetSticker(0, i));
        }

        // create circles
        List<Tuple<int, int>> rotationAxes = new List<Tuple<int, int>>() {
            Tuple.Create(0, 1), Tuple.Create(0, 2), Tuple.Create(1, 0),
            Tuple.Create(2, 1), Tuple.Create(1, 3), Tuple.Create(3, 1),
            Tuple.Create(2, 3), Tuple.Create(0, 3)
        };
        List<int> matChoice = new List<int>() { 0, 1, 0, 2, 4, 4, 5, 3 };
        GameObject circleContainer = new GameObject();
        circleContainer.name = name;
        for (int i = 0; i < tempstickers.Count; i++) {
            GameObject tempsticker = new GameObject();
            // for all rotations necessary to roam all 6 circles
            for (int j = 0; j < 8; j++) {
                switch (j) {
                    // rotation j = 2 and j = 5 are only to get on the right circle
                    case 2:
                    case 5:
                        GameManager.TraverseAxis(tempstickers, tempsticker, vertices, i,
                            rotationAxes[j].Item1, rotationAxes[j].Item2, 90f, false);
                        break;
                    // other rotations draw the circles
                    default:
                        for (int k = 0; k < 90; k++) {
                            GameManager.TraverseAxis(tempstickers, tempsticker, vertices, i,
                                rotationAxes[j].Item1, rotationAxes[j].Item2, 4f);
                        }
                        Mesh circleMesh = GameManager.CreateCircleMesh(vertices);
                        GameObject circle = GameManager.CreateCircle(circleMesh, matChoice[j], i);
                        circle.transform.parent = circleContainer.transform;
                        vertices.Clear();
                        break;
                }
            }
            Destroy(tempsticker);
        }
        return circleContainer;
    }

    
}