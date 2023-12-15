using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class RingsRepresentation: MonoBehaviour{
    // List<Tuple<int, int>> rotationAxes; // = Rings // TODO necessary?
    RingsRepresentation(){}

    /// <summary>
    /// Render the rings based from the axis on the 3D space
    /// Draw the axis circles on the 3D space
    /// // TODO add more info
    /// // TODO need to split into 2 functions :
    ///     - 1 generate ONLY ONE ring
    ///     - 1 generate ALL the rings
    /// </summary>    
    /// <param name="name">Name of the circles</param>
    public static GameObject RenderCircles(string name, Puzzle p) { //TODO
        GameObject circleContainer = new GameObject();
        circleContainer.name = name;

        // index for the circle material // TODO move in another function?
        List<int> matChoice = new List<int>() { 0, 1, 0, 2, 4, 4, 5, 3 };

        // create circles
            // define the parcours // TODO Geometry.Axis { x, y, z, w, none }
        List<Tuple<int, int>> rotationAxes = new List<Tuple<int, int>>() {
            Tuple.Create(0, 1), Tuple.Create(0, 2), Tuple.Create(1, 0),
            Tuple.Create(2, 1), Tuple.Create(1, 3), Tuple.Create(3, 1),
            Tuple.Create(2, 3), Tuple.Create(0, 3)
        };
        
        for (int i = 0; i < p.NbStickers(0); i++) {
            GameObject tempsticker = new GameObject();
            Vector4 stickerReference = p.GetSticker(0, i);

            // for all rotations necessary to roam all 6 circles
            for (int j = 0; j < 8; j++) {
                List<Vector3> vertices = new List<Vector3>();
                switch (j) {
                    // rotation j = 2 and j = 5 => reajust the sticker position to get on the right circle
                    case 2: case 5:
                        stickerReference = GameManager.TraverseAxis(stickerReference, tempsticker, vertices,
                            rotationAxes[j].Item1, rotationAxes[j].Item2, 90f, false);
                        break;
                    // other rotations draw the circles
                    default:
                        for (int k = 0; k < 90; k++) {
                            // need a loop to gradually reach 360°
                            stickerReference = GameManager.TraverseAxis(stickerReference, tempsticker, 
                                vertices, rotationAxes[j].Item1, rotationAxes[j].Item2, 4f);
                        }
                        Mesh circleMesh = GameManager.CreateCircleMesh(vertices);
                        GameObject circle = GameManager.CreateCircle(circleMesh, matChoice[j], i);
                        circle.transform.parent = circleContainer.transform;
                        break;
                }
            }
            Destroy(tempsticker);
        }
        return circleContainer;
    }

    // Generate one ring
    private static void GenerateRing(){
        // for all rotations necessary to roam all 6 circles
            // for (int j = 0; j < 8; j++) {
            //     List<Vector3> vertices = new List<Vector3>();
            //     switch (j) {
            //         // rotation j = 2 and j = 5 => reajust the sticker position to get on the right circle
            //         case 2: case 5:
            //             stickerReference = GameManager.TraverseAxis(stickerReference, tempsticker, vertices,
            //                 rotationAxes[j].Item1, rotationAxes[j].Item2, 90f, false);
            //             break;
            //         // other rotations draw the circles
            //         default:
            //             for (int k = 0; k < 90; k++) {
            //                 // need a loop to gradually reach 360°
            //                 stickerReference = GameManager.TraverseAxis(stickerReference, tempsticker, 
            //                     vertices, rotationAxes[j].Item1, rotationAxes[j].Item2, 4f);
            //             }
            //             Mesh circleMesh = GameManager.CreateCircleMesh(vertices);
            //             GameObject circle = GameManager.CreateCircle(circleMesh, matChoice[j], i);
            //             circle.transform.parent = circleContainer.transform;
            //             break;
            //     }
            // }
    }
}