using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager: MonoBehaviour { // == main
    // TODO const static attributs
    List<string> _names = new List<string>() {
            "Right", "Left", "Up", "Down", "Back", "Front", "In", "Out" };
    List<string> _materials = new List<string>() {
            "Red", "Orange", "Blue", "Green", "Yellow", "White", "Purple", "Pink" };
    List<string> _circle_materials = new List<string>() {
            "XY", "XZ", "YZ", "XW", "YW", "ZW" };
    // ---

    public GameObject puzzle; // TODO delete it?
    Puzzle p;

    private bool _cubeRotating = false;

    [SerializeField]
    private SelectSticker selectedSticker;

    // TODO for debug / test purpose?
    public int axis1 = 0;
    public int axis2 = 1;
    // ---

    // To customize the Rubik // TODO need to be added in a Parameter Menu
    [SerializeField]
    private Mesh sphereMesh;
    private float stickerSize = 0.125f;
    private float trailWidth = 0.0078125f;
    public float rotationSpeed = 2f;

    // to simplify the camera rotation
    // TODO move it in another file?
    static float s3 = 1f / Mathf.Sqrt(3f);
    static float s6 = (3f + Mathf.Sqrt(3f)) / 6f;
    static float _s6 = 1f - s6;

    static Matrix4x4 specialProjection = new Matrix4x4(
        new Vector4(-s6, 0f, _s6, s3),
        new Vector4(_s6, 0f, -s6, s3),
        new Vector4(-s3, 0f, -s3, -s3),
        new Vector4(0f, 1f, 0f, 0f));

    private int cameraRotationMode = 0;
    Matrix4x4 cameraRotation = specialProjection;

    // secondary rotation matrix to eventually use later
    Matrix4x4 colorAssignment = new Matrix4x4(
        new Vector4(1, 0, 0, 0),
        new Vector4(0, 1, 0, 0),
        new Vector4(0, 0, 1, 0),
        new Vector4(0, 0, 0, 1));

    /// <summary>
    /// Awake is called automatically before the function Start
    /// </summary>
    void Awake() {
        puzzle = new GameObject();
        puzzle.name = "Puzzle";
        p = new Puzzle();


        // Create a GameObject for each point and link them in the GameObject "Puzzle"
        RenderStickers();

        // Create GameObjects representing the rotation axes, aesthetic purpose
        RenderCircles();
    }

    /// <summary>
    /// Start is called automatically before the first frame update
    /// </summary>
    void Start(){
        // Handles rotation in parallel to the Update method
        StartCoroutine(RotationHandler());
    }

    /// <summary>
    /// Update is called automatically once per frame
    /// </summary>
    void Update() {
        if (!_cubeRotating) {
            if (Input.GetKeyDown(KeyCode.LeftShift)) {
                (axis1, axis2) = (axis2, axis1);
            }
            if (Input.GetKeyDown(KeyCode.R) && axis1 != axis2) {
                LaunchRotation();
            }
            if (Input.GetKeyDown(KeyCode.P)) {
                ChangeProjection();
            }
        }
    }

    /// <summary>
    /// initialize the data to lauch a rotation
    /// </summary>
    public void LaunchRotation() {
        _cubeRotating = true;
    }

    /// <summary>
    /// Create coordinates for each sticker
    /// </summary>

    /// <summary>
    /// Draw the circles on the 3D space
    /// </summary>
    void RenderStickers() {
        for (int i = 0; i < p.NbCells(); i++) {
            // TODO warning : length of _names and _materials may not be the same as the number of points
            GameObject cell = new GameObject();
            cell.name = _names[i];

            // place these points in the space
            cell.transform.parent = puzzle.transform;
            for (int j = 0; j < p.NbStickers(i); j++) {
                GameObject sticker = new GameObject();
                sticker.name = _names[i] + "_" + j;

                // add mesh
                sticker.AddComponent<MeshFilter>();
                sticker.GetComponent<MeshFilter>().mesh = sphereMesh;

                // add material
                Material stickerMat = Resources.Load(_materials[i], typeof(Material)) as Material;
                sticker.AddComponent<MeshRenderer>();
                sticker.GetComponent<Renderer>().material = stickerMat;

                // add the Select Scipt
                sticker.AddComponent<SelectSticker>();
                sticker.GetComponent<SelectSticker>().SetCoordinates(p.GetSticker(i,j));
                sticker.AddComponent<MeshCollider>();

                // place these points in the space
                sticker.transform.localScale = stickerSize * Vector3.one;
                sticker.transform.parent = cell.transform;
                sticker.transform.position = Projection4DTo3D(p.GetSticker(i, j));
            }
        }
    }

    /// <summary>
    /// Rotate a certain amount around a rotation plane and create vertices
    /// // TODO rephrase the doc + rename fn?
    /// </summary>
    /// <param name="stickers"></param>
    /// <param name="sticker"></param>
    /// <param name="vertices"></param>
    /// <param name="index"></param>
    /// <param name="axis1"></param>
    /// <param name="axis2"></param>
    /// <param name="angle"></param>
    /// <param name="makeVertices"></param>
    void TraverseAxis(List<Vector4> stickers, GameObject sticker, List<Vector3> vertices, int index, int axis1, int axis2,
            float angle, bool makeVertices = true) {
        sticker.transform.position = Projection4DTo3D(stickers[index]);
        if (makeVertices) {
            float vertexX = trailWidth * Mathf.Sin(angle);
            float vertexY = trailWidth * Mathf.Sin(angle);
            float vertexZ = trailWidth * Mathf.Cos(angle);
            vertices.Add(new Vector3(vertexX, vertexY, vertexZ) + sticker.transform.position);
            vertices.Add(new Vector3(-vertexX, vertexY, -vertexZ) + sticker.transform.position);
            vertices.Add(new Vector3(0, -vertexY, 0) + sticker.transform.position);
        }

        // TODO improve reajustement
        stickers[index] = Geometry.RotationMatrix((Geometry.Axis) axis1, (Geometry.Axis) axis2, angle) * stickers[index];
    }

    /// <summary>
    /// Create circle mesh from vertices
    /// </summary>
    /// <param name="vertices"></param>
    /// <returns></returns>
    Mesh CreateCircleMesh(List<Vector3> vertices) {
        Mesh mesh = new Mesh();

        // add vertices
        mesh.vertices = vertices.ToArray();

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
    /// Create circle from mesh
    /// </summary>
    /// <param name="mesh"></param>
    /// <param name="axisIndex"></param>
    /// <param name="tempstickerIndex"></param>
    /// <returns></returns>
    GameObject CreateCircle(Mesh mesh, int axisIndex, int tempstickerIndex) {
        // create gameobject
        GameObject circle = new GameObject();
        circle.name = _circle_materials[axisIndex] + "_" + tempstickerIndex;

        // add mesh
        circle.AddComponent<MeshFilter>();
        circle.GetComponent<MeshFilter>().mesh = mesh;

        // add material
        Material circleMat = Resources.Load(_circle_materials[axisIndex], typeof(Material)) as Material;
        circle.AddComponent<MeshRenderer>();
        circle.GetComponent<Renderer>().material = circleMat;

        return circle;
    }

    /// <summary>
    /// Draw the axis circles on the 3D space
    /// </summary>
    void RenderCircles() {
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
        circleContainer.name = "CircleContainer";
        for (int i = 0; i < tempstickers.Count; i++) {
            GameObject tempsticker = new GameObject();
            // for all rotations necessary to roam all 6 circles
            for (int j = 0; j < 8; j++) {
                switch (j) {
                    // rotation j = 2 and j = 5 are only to get on the right circle
                    case 2:
                    case 5:
                        TraverseAxis(tempstickers, tempsticker, vertices, i,
                            rotationAxes[j].Item1, rotationAxes[j].Item2, 90f, false);
                        break;
                    // other rotations draw the circles
                    default:
                        for (int k = 0; k < 90; k++) {
                            TraverseAxis(tempstickers, tempsticker, vertices, i,
                                rotationAxes[j].Item1, rotationAxes[j].Item2, 4f);
                        }
                        Mesh circleMesh = CreateCircleMesh(vertices);
                        GameObject circle = CreateCircle(circleMesh, matChoice[j], i);
                        circle.transform.parent = circleContainer.transform;
                        vertices.Clear();
                        break;
                }
            }
            Destroy(tempsticker);
        }
    }

    public IEnumerator RotationHandler() {
        while (true) {
            if (!_cubeRotating) {
                yield return null;
                // == continue; in c, to avoid freeze screen when used in coroutine
            }
            else {
                List<List<Vector4>> targets = DefineTargets();
                if (IsBetweenRangeExcluded(rotationSpeed, 0f, 90f)) {
                    float totalRotation = 0;
                    while (Mathf.Abs(90f - totalRotation) > Mathf.Epsilon) {
                        totalRotation = RotateOverTime(rotationSpeed, totalRotation);
                        yield return null;
                    }
                }

                SnapToTargets(targets);
                _cubeRotating = false;
            }
        }
    }

    string whosOpposite(string sphereName) {
        int index = _names.IndexOf(sphereName);
        return (index % 2 == 0) ? _names[index + 1] : _names[index - 1];
    }

    public List<string> whosGunnaRotate(string sphereName) { // TODO remove public?
        // TODO not complete yet?
        List<string> mustRotate = new List<string>();
        string opposite = whosOpposite(sphereName);
        foreach (string entry in _names) {
            if (entry != sphereName & entry != opposite) {
                mustRotate.Add(entry);
            }
        }
        return mustRotate;
    }

    /// <summary>
    /// Determine the destination of each cell and sticker
    /// </summary>
    public List<List<Vector4>> DefineTargets() {
        // TODO put "puzzle" in param?
        // TODO need change for differents layers
        List<List<Vector4>> targets = new List<List<Vector4>>(); // TODO may be simplified with List<Vector4>?
        Matrix4x4 rotate = Geometry.RotationMatrix(Geometry.IntToAxis(axis1), Geometry.IntToAxis(axis2), 90);

        for (int i = 0; i < puzzle.transform.childCount; i++) { // TODO change conditions
            targets.Add(new List<Vector4>());

            Transform cell = puzzle.transform.GetChild(i);
            for (int j = 0; j < cell.childCount; j++) {
                targets[i].Add(rotate * p.GetSticker(i, j));
            }
        }
        return targets;
    }

    /// <summary>
    /// Rotates by 90 degrees with animation
    /// </summary>
    /// <param name="rotationSpeed"> </param>
    public float RotateOverTime(float rotationSpeed, float totalRotation) {
        // TODO need optimization?
        Matrix4x4 rotate = Geometry.RotationMatrix(Geometry.IntToAxis(axis1), Geometry.IntToAxis(axis2), rotationSpeed);
        rotationSpeed = Mathf.Clamp(rotationSpeed, 0f, 90f - totalRotation);
        totalRotation = Mathf.Clamp(totalRotation + rotationSpeed, 0f, 90f);
        for (int i = 0; i < puzzle.transform.childCount; i++) {
            Transform cell = puzzle.transform.GetChild(i);
            for (int j = 0; j < cell.childCount; j++) {
                Transform sticker = cell.GetChild(j);
                p.setSticker(i, j, rotate * p.GetSticker(i, j));
                sticker.position = Projection4DTo3D(p.GetSticker(i, j));
            }
        }
        return totalRotation;
    }

    /// <summary>
    /// Snaps each cell sticker to its final position
    /// </summary>
    public void SnapToTargets(List<List<Vector4>> targets) {
        for (int i = 0; i < puzzle.transform.childCount; i++) {
            Transform cell = puzzle.transform.GetChild(i);
            for (int j = 0; j < cell.childCount; j++) {
                Transform sticker = cell.GetChild(j);
                p.setSticker(i, j, targets[i][j]);
                sticker.position = Projection4DTo3D(p.GetSticker(i, j));
            }
        }
    }

    /// <summary>
    /// Toggle between classic projection and special projection
    /// </summary>
    private void ChangeProjection() {
        switch (cameraRotationMode) {
            case 0:
                cameraRotationMode = 1;
                cameraRotation = Matrix4x4.identity;
                break;
            case 1:
            default:
                cameraRotationMode = 0;
                cameraRotation = specialProjection;
                break;
        }
        GameObject circleContainer = GameObject.Find("CircleContainer");
        Destroy(circleContainer);
        RenderCircles();
        for (int i = 0; i < puzzle.transform.childCount; i++) {
            Transform cell = puzzle.transform.GetChild(i);
            for (int j = 0; j < cell.childCount; j++) {
                Transform sticker = cell.GetChild(j);
                sticker.position = Projection4DTo3D(p.GetSticker(i, j));
            }
        }
    }

    public static bool IsBetweenRangeExcluded(float value, float value1, float value2) {
        return value > Mathf.Min(value1, value2) && value < Mathf.Max(value1, value2);
    }

    public Vector4 Projection4DTo3D(Vector4 point) {
        Vector4 temp = new Vector4(point.x, point.y, point.z, point.w);
        temp = cameraRotation * colorAssignment * temp;
        Vector3 projected = Vector3.zero;
        // handle projection to infinity
        if (temp.w + 1 != 0) {
            projected = new Vector3(temp.x, temp.y, temp.z) / (temp.w + 1);
        }
        else {
            projected = new Vector3(
                Mathf.Sign(temp.x) * Int32.MaxValue,
                Mathf.Sign(temp.y) * Int32.MaxValue,
                Mathf.Sign(temp.z) * Int32.MaxValue);
        }
        return projected;
    }

    public int GetAxis1() {
        return axis1;
    }

    public int GetAxis2() {
        return axis2;
    }

    public bool GetRotateFlag() {
        return _cubeRotating;
    }

    public void setterSelection(SelectSticker selection){
        selectedSticker = selection;
    }

    public SelectSticker GetSelection(){
        return selectedSticker;
    }
    /// <summary>
    /// set the plane based on two axis
    /// </summary>
    /// <param name="a1">the first axis</param>
    /// <param name="a2">the second axis</param>
    public void SetPlane(int a1, int a2) {
        axis1 = a1;
        axis2 = a2;
    }

    // void BaseRotation(GameObject sphere, string input) {
    //     if (input == "y") {
    //         Geometry.RotationMatrix[0, 0] = Mathf.Cos(0.1f);
    //         Geometry.RotationMatrix[2, 0] = -Mathf.Sin(0.1f);
    //         Geometry.RotationMatrix[0, 2] = Mathf.Sin(0.1f);
    //         Geometry.RotationMatrix[2, 2] = Mathf.Cos(0.1f);
    //     }
    //     if (input == "x") {
    //         Geometry.RotationMatrix[1, 1] = Mathf.Cos(0.1f);
    //         Geometry.RotationMatrix[2, 1] = -Mathf.Sin(0.1f);
    //         Geometry.RotationMatrix[1, 2] = Mathf.Sin(0.1f);
    //         Geometry.RotationMatrix[2, 2] = Mathf.Cos(0.1f);
    //     }
    //     if (input == "z") {
    //         Geometry.RotationMatrix[0, 0] = Mathf.Cos(0.1f);
    //         Geometry.RotationMatrix[1, 0] = -Mathf.Sin(0.1f);
    //         Geometry.RotationMatrix[0, 1] = Mathf.Sin(0.1f);
    //         Geometry.RotationMatrix[1, 1] = Mathf.Cos(0.1f);
    //     }
    //     Vector3 sphereCoords = sphere.transform.position;
    //     sphereCoords = Geometry.RotationMatrix * sphereCoords;
    //     sphere.transform.position = sphereCoords;
    // }

    // void BigRotation(GameObject sphere, string input) {
    //     List<string> toBeRotated = new List<string>(6);
    //     toBeRotated = whosGunnaRotate(sphere.name);
    //     foreach (string entry in toBeRotated) {
    //         baseRotation(GameObject.Find(entry),input);
    //     }
    // }
}
