using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puzzle {
    List<List<Vector4>> _stickers = new List<List<Vector4>>();

    void Start(){

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