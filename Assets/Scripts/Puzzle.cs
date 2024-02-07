using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Rendering; // to use Enumerable

public class Puzzle {
    List<string> _names = new List<string>() {
        "Right", "Left", "Up", "Down", "Back", "Front", "In", "Out" };

    List<string> _materials = new List<string>() {
        "Red", "Orange", "Blue", "Green", "Yellow", "White", "Purple", "Pink" };

    /// \todo _stickers can be optimized?
    List<List<Vector4>> _stickers = new List<List<Vector4>>();
    const int _nbCells = 8;
    private float stickerDistance = 0.1f; /// \todo -> in renderer ?

    /// <summary>
    /// Generate a 4D Rubik
    /// </summary>
    /// <param name="n">Size of the Rubik (by default it's a 2x2x2x2 Rubik)</param>
    public Puzzle(int n = 2) {
        /// \todo n must be added in parameters -> puzzleSize
        for (int i = 0; i < _nbCells; i++) {
            // Define a cell
            Vector4 cell = Vector4.zero;
            int iCell = Mathf.FloorToInt(i * 0.5f);
            cell[iCell] = 1 - (2 * (i % 2)); // 1 for i even, -1 for i odd

            // Create the stickers at the i-th cell
            /// \todo explain more?
            _stickers.Add(new List<Vector4>());
            for (int j = 0; j < Mathf.Pow(n, 3); j++) {
                Vector3 temp = new Vector3(0, 0, 0);
                if (n > 1) {
                    temp.x = Mathf.Lerp(-1f, 1f,
                        (Mathf.FloorToInt(j / Mathf.Pow(n, 2)) % n) / (n - 1f));
                    temp.y = Mathf.Lerp(-1f, 1f,
                        (Mathf.FloorToInt(j / n) % n) / (n - 1f));
                    temp.z = Mathf.Lerp(-1f, 1f,
                        (j % n) / (n - 1f));
                }
                Vector4 sticker = Vector4.zero;
                sticker = Geometry.InsertFloat(temp * stickerDistance, cell[iCell], iCell);
                _stickers[i].Add(sticker);
            }
        }
    }

    /// <summary>
    /// Create coordinates for each sticker
    /// </summary>
    public GameObject RenderStickers(Mesh mesh, float stickerSize, int mode) {
        GameObject puzzle = new GameObject();
        for (int i = 0; i < NbCells(); i++) {
            /// \todo warning : length of _names and _materials may not be the same as the number of points
            GameObject cell = new GameObject();
            cell.name = _names[i];

            // add mesh
            cell.AddComponent<MeshFilter>();
            cell.GetComponent<MeshFilter>().mesh = mesh;

            // add material
            Material cellMat = Resources.Load("_Select", typeof(Material)) as Material;
            cell.AddComponent<MeshRenderer>();
            /// \todo: fix this, not working currently
            /*cell.AddComponent<MeshRenderer>().shader.shadowCastingMode = ShadowCastingMode.Off;
            cell.AddComponent<MeshRenderer>().receiveShadows = false;
            cell.AddComponent<MeshRenderer>().allowOcclusionWhenDynamic = false;*/
            cell.GetComponent<Renderer>().material = cellMat;
            cell.GetComponent<Renderer>().enabled = false;

            // compute the cell's position
            Vector4 cellPosition = Vector4.zero;
            int iCell = Mathf.FloorToInt(i * 0.5f);
            cellPosition[iCell] = 1 - (2 * (i % 2));

            // add the Select Script
            cell.AddComponent<SelectCell>();
            cell.AddComponent<Coords4D>().SetCoordinates(cellPosition);
            cell.AddComponent<MeshCollider>();

            cell.transform.position = Geometry.Projection4DTo3D(GameManager.cameraRotation * cellPosition);
            cell.transform.localScale = 0.6f * (float)Math.Sqrt(
                Vector3.Distance(cell.transform.position, Vector3.zero) + 1) * Vector3.one;

            // place these points in the space
            cell.transform.parent = puzzle.transform;
            for (int j = 0; j < NbStickers(i); j++) {
                GameObject sticker = new GameObject();
                sticker.name = _names[i] + "_" + j;

                // add mesh
                sticker.AddComponent<MeshFilter>().mesh = mesh;

                // add material
                Material stickerMat = Resources.Load(_materials[i], typeof(Material)) as Material;
                sticker.AddComponent<MeshRenderer>();
                sticker.GetComponent<Renderer>().material = stickerMat;

                // add the Select Script
                sticker.AddComponent<SelectSticker>();
                sticker.AddComponent<Coords4D>().SetCoordinates(GetSticker(i, j)
                    + Geometry.InsertFloat(Geometry.ExtractVector3(GetSticker(i, j) * mode * 8f, iCell), 0f, iCell));
                sticker.AddComponent<MeshCollider>().enabled = false;

                // place these points in the space
                sticker.transform.localScale = stickerSize * Vector3.one;
                sticker.transform.parent = cell.transform;
                sticker.transform.position = Geometry.Projection4DTo3D(GameManager.cameraRotation * (GetSticker(i, j)
                    + Geometry.InsertFloat(Geometry.ExtractVector3(GetSticker(i, j) * mode * 8f, iCell), 0f, iCell)));
            }
        }
        return puzzle;
    }

    /// <summary>
    /// /// \todo explain a little bit this function
    /// </summary>
    /// <returns></returns>
    public List<List<bool>> whosGunnaRotate(Coords4D selectedElement = null) {
        // List<List<bool>> toBeRotated = new List<List<bool>>();
        if (selectedElement == null) { /// \todo need optimisation
            List<bool> sticker = Enumerable.Repeat(true, NbStickers(0)).ToList();
            return Enumerable.Repeat(sticker, NbCells()).ToList();
        }

        /// \todo change type of selectedElement?
        int discriminator = 0;
        int signOfDiscriminator = 0;
        for (int i = 0; i < 4; i++) {
            if (Mathf.Abs(selectedElement.GetCoordinates()[i]) == 1) {
                signOfDiscriminator = (int)selectedElement.GetCoordinates()[i];
                discriminator = i;
            }
        }

        List<List<bool>> toBeRotated = new List<List<bool>>();
        for (int i = 0; i < NbCells(); i++) {
            toBeRotated.Add(new List<bool>());
            for (int j = 0; j < NbStickers(i); j++) {
                if (signOfDiscriminator * GetSticker(i, j)[discriminator] > 0) {
                    toBeRotated[i].Add(true);
                }
                else {
                    toBeRotated[i].Add(false);
                }
            }
        }
        return toBeRotated;
    }

    // --- Getter and Setter
    /// \todo must have a version more C# like
    public List<List<Vector4>> GetStickers() {
        return _stickers;
    }

    public int NbCells() {
        return _nbCells;
    }

    public int NbStickers(int index) {
        return _stickers[index].Count;
    }

    public Vector4 GetSticker(int iCell, int iSticker) {
        return _stickers[iCell][iSticker];
    }

    public void setSticker(int iCell, int iSticker, Vector4 value) {
        _stickers[iCell][iSticker] = value;
    }
}
