using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle : Attribute {
        // TODO can be optimized?
    List<List<Vector4>> _stickers = new List<List<Vector4>>();

    
    private int nbCells = 8; // const static
    private float stickerDistance = 10f; // TODO -> in renderer ?

    /// <summary>
    /// Create coordinates for each sticker
    /// </summary>
    public Puzzle(int puzzleSize = 2){
        for (int i = 0; i < nbCells; i++) {
            // Create a cell
            Vector4 point = Vector4.zero;
            int pointIndex = Mathf.FloorToInt(i * 0.5f);
            int altSign = 1 - (2 * (i % 2));
            point[pointIndex] = altSign;

            // Create the stickers inside of the i-eme cell
            _stickers.Add(new List<Vector4>());
            for (int j = 0; j < Mathf.Pow(puzzleSize, 3); j++) {
                Vector3 temp = new Vector3(0, 0, 0);
                if (puzzleSize > 1) {
                    temp.x = Mathf.Lerp(-1f, 1f,
                        (Mathf.FloorToInt(j / Mathf.Pow(puzzleSize, 2)) % puzzleSize) / (puzzleSize - 1f));
                    temp.y = Mathf.Lerp(-1f, 1f,
                        (Mathf.FloorToInt(j / puzzleSize) % puzzleSize) / (puzzleSize - 1f));
                    temp.z = Mathf.Lerp(-1f, 1f,
                        (j % puzzleSize) / (puzzleSize - 1f));
                }
                Vector4 subpoint = new Vector4(0, 0, 0, 0);
                subpoint = InsertFloat(temp / stickerDistance, point[pointIndex], pointIndex);
                _stickers[i].Add(subpoint);
            }
        }
    }

    /// <summary>
    /// Inserts value in Vector3 at pos, making it a Vector4
    /// // TODO add more details about this function
    /// </summary>
    /// <param name="vec"></param>
    /// <param name="value"></param>
    /// <param name="pos"></param>
    /// <returns>Vector4 with value inserted at index pos</returns>
    private Vector4 InsertFloat(Vector3 vec, float value, int pos) {
        pos = Mathf.Clamp(pos, 0, 3);
        Vector4 result = Vector4.zero;
        switch (pos) {
            case 0:
                result = new Vector4(value, vec.x, vec.y, vec.z);
                break;
            case 1:
                result = new Vector4(vec.x, value, vec.y, vec.z);
                break;
            case 2:
                result = new Vector4(vec.x, vec.y, value, vec.z);
                break;
            case 3:
                result = new Vector4(vec.x, vec.y, vec.z, value);
                break;
            default:
                break;
        }
        return result;
    }

    public void UpdateStickers(List<List<Vector4>> stickers){ 
        // TODO into constructor?
        _stickers = stickers;
    }

    public List<List<Vector4>> GetStickers(){
        return _stickers;
    }

    public int NbCells(){
        return _stickers.Count;
    }

    public int NbStickers(int index){
        return _stickers[index].Count;
    }

    public Vector4 GetSticker(int iCell, int iSticker){
        return _stickers[iCell][iSticker];
    }

    public void setSticker(int iCell, int iSticker, Vector4 value){
        _stickers[iCell][iSticker] = value;
    }
}