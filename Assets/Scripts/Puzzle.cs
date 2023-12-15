using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; // to use Enumerable

public class Puzzle {
    List<string> _names = new List<string>() {
        "Right", "Left", "Up", "Down", "Back", "Front", "In", "Out" };

    List<string> _materials = new List<string>() {
        "Red", "Orange", "Blue", "Green", "Yellow", "White", "Purple", "Pink" };

    // TODO _stickers can be optimized?
    List<List<Vector4>> _stickers = new List<List<Vector4>>();
    const int _nbCells = 8;
    private float stickerDistance = 10f; // TODO -> in renderer ?

    /// <summary>
    /// Generate a 4D Rubik
    /// </summary>
    /// <param name="n">Size of the Rubik (by default it's a 2x2x2x2 Rubik)</param>
    public Puzzle(int n = 2) {
        // TODO n must be added in parameters -> puzzleSize
        for (int i = 0; i < _nbCells; i++) {
            // Define a cell
            Vector4 cell = Vector4.zero;
            int iCell = Mathf.FloorToInt(i * 0.5f);
            cell[iCell] = 1 - (2 * (i % 2)); // 1 for i even, -1 for i odd

            // Create the stickers at the i-th cell
            // TODO explain more?
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
                Vector4 sticker = new Vector4(0, 0, 0, 0);
                sticker = Geometry.InsertFloat(temp / stickerDistance, cell[iCell], iCell);
                _stickers[i].Add(sticker);
            }
        }
    }

    /// <summary>
    /// Create coordinates for each sticker
    /// </summary>
    public GameObject RenderStickers(Mesh mesh, float stickerSize) {
        GameObject puzzle = new GameObject();
        for (int i = 0; i < NbCells(); i++) {
            // TODO warning : length of _names and _materials may not be the same as the number of points
            GameObject cell = new GameObject();
            cell.name = _names[i];

            // place these points in the space
            cell.transform.parent = puzzle.transform;
            for (int j = 0; j < NbStickers(i); j++) {
                GameObject sticker = new GameObject();
                sticker.name = _names[i] + "_" + j;

                // add mesh
                sticker.AddComponent<MeshFilter>();
                sticker.GetComponent<MeshFilter>().mesh = mesh;

                // add material
                Material stickerMat = Resources.Load(_materials[i], typeof(Material)) as Material;
                sticker.AddComponent<MeshRenderer>();
                sticker.GetComponent<Renderer>().material = stickerMat;

                // add the Select Scipt
                sticker.AddComponent<SelectSticker>();
                sticker.GetComponent<SelectSticker>().SetCoordinates(GetSticker(i, j));
                sticker.AddComponent<MeshCollider>();

                // place these points in the space
                sticker.transform.localScale = stickerSize * Vector3.one;
                sticker.transform.parent = cell.transform;
                sticker.transform.position = Geometry.Projection4DTo3D(GameManager.cameraRotation * GameManager.colorAssignment * GetSticker(i, j));
            }
        }
        return puzzle;
    }

    /// <summary>
    /// // TODO explain a little bit this function
    /// </summary>
    /// <returns></returns>
    public List<List<bool>> whosGunnaRotate(SelectSticker selectedSticker = null) {
        // List<List<bool>> toBeRotated = new List<List<bool>>();
        if(selectedSticker == null){ // TODO need optimisation
            List<bool> sticker = Enumerable.Repeat(true, NbStickers(0)).ToList();
            return Enumerable.Repeat(sticker, NbCells()).ToList();
        }

        // TODO change type of selectedSticker?
        int discriminator = 0;
        int signOfDiscriminator = 0;
        for(int i = 0 ; i < 4 ; i++){
            if(Mathf.Abs(selectedSticker.GetCoordinates()[i])==1){
                signOfDiscriminator = (int) selectedSticker.GetCoordinates()[i];
                discriminator = i;
            }
        }

        List<List<bool>> toBeRotated = new List<List<bool>>();
        for (int i = 0 ; i < NbCells(); i++){
            toBeRotated.Add(new List<bool>());
            for(int j = 0 ; j < NbStickers(i); j++){
                if(signOfDiscriminator*GetSticker(i,j)[discriminator]>0){
                    toBeRotated[i].Add(true);
                }else{
                    toBeRotated[i].Add(false);
                }
            }
        }
        return toBeRotated;
    }

    // --- Getter and Setter
    // TODO must have a version more C# like
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
