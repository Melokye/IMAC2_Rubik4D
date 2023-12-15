using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
class Animation {
    /// <summary>
    /// Determine the destination of each sticker
    /// </summary>
    public static List<List<Vector4>> DefineTargets(Puzzle p, SelectSticker selectedSticker, Geometry.Axis begin, Geometry.Axis end) {
        // TODO may be simplified with List<Vector4>?
        List<List<Vector4>> targets = new List<List<Vector4>>(); 
        Matrix4x4 rotate = Geometry.RotationMatrix(begin, end, 90);

        for (int i = 0; i < p.NbCells(); i++) {
            targets.Add(new List<Vector4>());            
            for (int j = 0; j < p.NbStickers(i); j++) {
                if(p.whosGunnaRotate(selectedSticker)[i][j]){
                    targets[i].Add(rotate * p.GetSticker(i, j));
                }else{
                    targets[i].Add(new Vector4());
                }
            }
        }
        return targets;
    }
}