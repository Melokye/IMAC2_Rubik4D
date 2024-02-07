using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
class RingsRepresentation: MonoBehaviour{
    private static float trailWidth = 0.0078125f;

    /// <summary>
    /// Render the rings based from the axis on the 3D space
    /// Draw the axis circles on the 3D space
    /// /// \todo add more info
    /// /// \todo need to split into 2 functions :
    ///     - 1 generate ONLY ONE ring
    ///     - 1 generate ALL the rings
    /// </summary>    
    /// <param name="name">Name of the circles</param>
    public static GameObject RenderCircles(string name, Puzzle p) {
        GameObject circleContainer = new GameObject();
        circleContainer.name = name;

        // index for the circle material /// \todo move in another function?
        List<int> matChoice = new List<int>() { 0, 1, 0, 2, 4, 4, 5, 3 };
        
        // define the parcours /// \todo Geometry.Axis { x, y, z, w, none }
        List<Tuple<int, int>> rotationAxes = new List<Tuple<int, int>>() {
            Tuple.Create(0, 1), Tuple.Create(0, 2), Tuple.Create(1, 0),
            Tuple.Create(2, 1), Tuple.Create(1, 3), Tuple.Create(3, 1),
            Tuple.Create(2, 3), Tuple.Create(0, 3)
        };

        /// \todo: make it work for 3x3x3x3 and above
        List<int> stickerChoice = new List<int>() { 0, 3, 5, 6 };

        // create circles
        for (int i = 0; i < stickerChoice.Count; i++) {
            /// \todo maybe we don't need the loop i
            GameObject tempsticker = new GameObject();
            Vector4 stickerReference = p.GetSticker(0, stickerChoice[i]);

            // for all rotations necessary to roam all 6 circles
            for (int j = 0; j < 8; j++) {
                List<Vector3> vertices = new List<Vector3>();
                switch (j) {
                    // rotation j = 2 and j = 5 => reajust the sticker position to get on the right circle
                    case 2: case 5:
                        stickerReference = RingsRepresentation.TraverseAxis(stickerReference, tempsticker, vertices,
                            rotationAxes[j].Item1, rotationAxes[j].Item2, 90f, false);
                        break;
                    
                    // other rotations draw the circles
                    default:
                        /// \todo part of the code to generate the rings from one plane
                        for (int k = 0; k < 90; k++) {
                            // need a loop to gradually reach 360°
                            stickerReference = RingsRepresentation.TraverseAxis(stickerReference, tempsticker, 
                                vertices, rotationAxes[j].Item1, rotationAxes[j].Item2, 4f);
                        }
                        Mesh circleMesh = RingsRepresentation.CreateCircleMesh(vertices);
                        GameObject circle = RingsRepresentation.CreateCircle(circleMesh, matChoice[j], i);
                        circle.transform.parent = circleContainer.transform;
                        break;
                }
            }
            Destroy(tempsticker);
        }
        return circleContainer;
    }

    /// <summary>
    /// Create circle from mesh
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="plane"></param>
    /// <param name="tempstickerIndex"></param>
    /// <returns></returns>
    static GameObject CreateCircle(Mesh mesh, int plane, int tempstickerIndex) {
        List<string> _circle_materials = new List<string>() {
        "XY", "XZ", "YZ", "XW", "YW", "ZW" }; /// \todo attribute? rename

        // create gameobject
        GameObject circle = new GameObject();
        circle.name = _circle_materials[plane] + "_" + tempstickerIndex;

        // add mesh
        circle.AddComponent<MeshFilter>();
        circle.GetComponent<MeshFilter>().mesh = mesh;

        // add material
        Material circleMat = Resources.Load(_circle_materials[plane], typeof(Material)) as Material;
        circle.AddComponent<MeshRenderer>();
        circle.GetComponent<Renderer>().material = circleMat;

        return circle;
    }

    
    /// <summary>
    /// Create circle mesh from vertices
    /// </summary>
    /// /// \todo need to split into multiples functions
    /// <param name="vertices"></param>
    /// <returns></returns>
    static Mesh CreateCircleMesh(List<Vector3> vertices) {
        /// \todo into RingsRepresentation.cs
        Mesh mesh = new Mesh();

        // add vertices
        mesh.vertices = vertices.ToArray();

        // add colors for transparency
        var transparencyValues = vertices.Select(vec => Mathf.Clamp(1f
            - Vector3.Distance(vec, Vector3.zero) / 20f, 0f, 1f)).ToArray();
        Color[] colorArray = transparencyValues.Select(value => new Color(1.0f, 1.0f, 1.0f, value)).ToArray();
        mesh.colors = colorArray;

        // create uvs
        Vector2[] uvs = new Vector2[vertices.Count];
        for (int i = 1; i < vertices.Count; i++) {
            if (i % 2 == 0)
                uvs[i] = new Vector2(i / (vertices.Count - 2f), 0);
            else
                uvs[i] = new Vector2((i - 1) / (vertices.Count - 2f), 1);
        }
        mesh.uv = uvs;

        // create triangles
        int[] triangles = new int[vertices.Count * 6];
        int tri = 0;
        for (int i = 0; i < vertices.Count * 2; i++) {
            int shift = Mathf.FloorToInt(i / 6) * 3;
            int j = i - shift;
            switch (i % 6) {
                case 0:
                    triangles[tri] = (j + 0) % vertices.Count;
                    triangles[tri + 1] = (j + 1) % vertices.Count;
                    triangles[tri + 2] = (j + 3) % vertices.Count;
                    break;
                case 1:
                    triangles[tri] = (j + 0) % vertices.Count;
                    triangles[tri + 1] = (j + 3) % vertices.Count;
                    triangles[tri + 2] = (j + 2) % vertices.Count;
                    break;
                case 2:
                    triangles[tri] = (j - 1) % vertices.Count;
                    triangles[tri + 1] = (j + 0) % vertices.Count;
                    triangles[tri + 2] = (j + 2) % vertices.Count;
                    break;
                case 3:
                    triangles[tri] = (j - 1) % vertices.Count;
                    triangles[tri + 1] = (j + 2) % vertices.Count;
                    triangles[tri + 2] = (j + 1) % vertices.Count;
                    break;
                case 4:
                    triangles[tri] = (j - 2) % vertices.Count;
                    triangles[tri + 1] = (j - 4) % vertices.Count;
                    triangles[tri + 2] = (j + 1) % vertices.Count;
                    break;
                case 5:
                    triangles[tri] = (j - 5) % vertices.Count;
                    triangles[tri + 1] = (j - 2) % vertices.Count;
                    triangles[tri + 2] = (j + 0) % vertices.Count;
                    break;
                default:
                    break;
            }
            tri += 3;
        }
        mesh.triangles = triangles;

        return mesh;
    }

    /// <summary>
    /// Rotate a certain amount around a rotation plane and create vertices
    /// /// \todo rephrase the doc + rename fn?
    /// </summary>
    /// <param name="stickers"></param>
    /// <param name="sticker"></param>
    /// <param name="vertices"></param>
    /// <param name="axis1"></param>
    /// <param name="axis2"></param>
    /// <param name="angle"></param>
    /// <param name="makeVertices"></param>
    static Vector4 TraverseAxis(
        Vector4 stickers, GameObject sticker, 
        List<Vector3> vertices, 
        int axis1, int axis2,
        float angle, bool makeVertices = true) 
    {
        /// \todo remove GameManager "attributes"?
        sticker.transform.position = Geometry.Projection4DTo3D(GameManager.cameraRotation * stickers);
        if (makeVertices) {
            float vertexX = trailWidth * Mathf.Sin(angle);
            float vertexY = trailWidth * Mathf.Sin(angle);
            float vertexZ = trailWidth * Mathf.Cos(angle);
            vertices.Add(new Vector3(vertexX, vertexY, vertexZ) + sticker.transform.position);
            vertices.Add(new Vector3(-vertexX, vertexY, -vertexZ) + sticker.transform.position);
            vertices.Add(new Vector3(0, -vertexY, 0) + sticker.transform.position);
        }

        /// \todo improve reajustement
        return Geometry.RotationMatrix((Geometry.Axis)axis1, (Geometry.Axis)axis2, angle) * stickers;
    }

    //Following : add the 3D counter part.
    public static GameObject RenderCircles(string name, Puzzle3D p) {
        GameObject circleContainer = new GameObject();
        circleContainer.name = name;

        // index for the circle material /// \todo move in another function?
        List<int> matChoice = new List<int>() { 0, 1, 0, 2, 4, 4, 5, 3 };
        
        // define the parcours /// \todo Geometry3D.Axis { x, y, z, none }
        List<Tuple<int, int>> rotationAxes = new List<Tuple<int, int>>() {
            Tuple.Create(0, 1), Tuple.Create(0, 2), Tuple.Create(1, 0),
            Tuple.Create(2, 1)
        };

        /// \todo: make it work for 3x3x3 and above
        List<int> stickerChoice = new List<int>() { 0, 3, 5, 6 };

        // create circles
        for (int i = 0; i < stickerChoice.Count; i++) {
            /// \todo maybe we don't need the loop i
            GameObject tempsticker = new GameObject();
            Vector3 stickerReference = p.GetSticker(0, stickerChoice[i]);

            // for all rotations necessary to roam all 6 circles
            for (int j = 0; j < 6; j++) {
                List<Vector3> vertices = new List<Vector3>();
                switch (j) {
                    // rotation j = 2 and j = 5 => reajust the sticker position to get on the right circle
                    case 2: case 5:
                        stickerReference = RingsRepresentation.TraverseAxis(stickerReference, tempsticker, vertices,
                            rotationAxes[j].Item1, rotationAxes[j].Item2, 90f, false);
                        break;
                    
                    // other rotations draw the circles
                    default:
                        /// \todo part of the code to generate the rings from one plane
                        for (int k = 0; k < 90; k++) {
                            // need a loop to gradually reach 360°
                            stickerReference = RingsRepresentation.TraverseAxis(stickerReference, tempsticker, 
                                vertices, rotationAxes[j].Item1, rotationAxes[j].Item2, 4f);
                        }
                        Mesh circleMesh = RingsRepresentation.CreateCircleMesh(vertices);
                        GameObject circle = RingsRepresentation.CreateCircle(circleMesh, matChoice[j], i);
                        circle.transform.parent = circleContainer.transform;
                        break;
                }
            }
            Destroy(tempsticker);
        }
        return circleContainer;
    }

    static Vector3 TraverseAxis(
        Vector3 stickers, GameObject sticker, 
        List<Vector3> vertices, 
        int axis1, int axis2,
        float angle, bool makeVertices = true) 
    {
        /// \todo remove GameManager "attributes"?
        sticker.transform.position = Geometry3D.Projection3DTo2D(GameManager3D.cameraRotation * stickers);
        if (makeVertices) {
            float vertexX = trailWidth * Mathf.Sin(angle);
            float vertexY = trailWidth * Mathf.Sin(angle);
            float vertexZ = trailWidth * Mathf.Cos(angle);
            vertices.Add(new Vector3(vertexX, vertexY, vertexZ) + sticker.transform.position);
            vertices.Add(new Vector3(-vertexX, vertexY, -vertexZ) + sticker.transform.position);
            vertices.Add(new Vector3(0, -vertexY, 0) + sticker.transform.position);
        }

        /// \todo improve reajustement
        return Geometry3D.RotationMatrix((Geometry3D.Axis)axis1, (Geometry3D.Axis)axis2, angle) * stickers;
    }

}