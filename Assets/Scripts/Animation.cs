using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// \todo separate function to distinct GameObject puzzle and Puzzle?

class Animation {
    static float rotationSpeed = 2f; /// \todo attribute?

    /// <summary>
    /// Determine the destination of each sticker
    /// </summary>
    public static List<List<Vector4>> DefineTargets(Puzzle p, Coords4D selectedElement, Geometry.Axis begin, Geometry.Axis end) {
        /// \todo may be simplified with List<Vector4>?
        List<List<Vector4>> targets = new List<List<Vector4>>();
        Matrix4x4 rotate = Geometry.RotationMatrix(begin, end, 90);

        for (int i = 0; i < p.NbCells(); i++) {
            targets.Add(new List<Vector4>());
            for (int j = 0; j < p.NbStickers(i); j++) {
                if (p.whosGunnaRotate(selectedElement)[i][j]) {
                    targets[i].Add(rotate * p.GetSticker(i, j));
                }
                else {
                    targets[i].Add(new Vector4());
                }
            }
        }
        return targets;
    }

    public static List<List<Vector3>> DefineTargets(Puzzle3D p, Coords3D selectedElement, Geometry3D.Axis begin, Geometry3D.Axis end) {
        /// \todo may be simplified with List<Vector4>?
        List<List<Vector3>> targets = new List<List<Vector3>>();
        Matrix4x4 rotate = Geometry3D.RotationMatrix(begin, end, 90);

        for (int i = 0; i < p.NbCells(); i++) {
            targets.Add(new List<Vector3>());
            for (int j = 0; j < p.NbStickers(i); j++) {
                if (p.whosGunnaRotate(selectedElement)[i][j]) {
                    targets[i].Add(rotate * p.GetSticker(i, j));
                }
                else {
                    targets[i].Add(new Vector4());
                }
            }
        }
        return targets;
    }

    /// <summary>
    /// Rotates by 90 degrees with animation
    /// </summary>
    /// <param name="rotationSpeed"> </param>
    public static float RotateOverTime(Puzzle puzzle, GameObject puzzleObject, float totalRotation, List<List<bool>> toBeRotated, Geometry.Axis begin, Geometry.Axis end) {
        /// \todo needs optimization? maybe move rotate outside of the function?
        Matrix4x4 rotate = Geometry.RotationMatrix(begin, end, rotationSpeed);

        rotationSpeed = Mathf.Clamp(rotationSpeed, 0f, 90f - totalRotation);
        totalRotation = Mathf.Clamp(totalRotation + rotationSpeed, 0f, 90f);
        for (int i = 0; i < puzzleObject.transform.childCount; i++) {
            Transform cell = puzzleObject.transform.GetChild(i);
            for (int j = 0; j < cell.childCount; j++) {
                Transform sticker = cell.GetChild(j);
                if (toBeRotated[i][j] == true) {
                    puzzle.setSticker(i, j, rotate * puzzle.GetSticker(i, j)); /// \todo presentation
                    sticker.GetComponent<Coords4D>().SetCoordinates(puzzle.GetSticker(i, j));
                }
            }
        }
        return totalRotation;
    }

    public static float RotateOverTime(Puzzle3D puzzle, GameObject puzzleObject, float totalRotation, List<List<bool>> toBeRotated, Geometry3D.Axis begin, Geometry3D.Axis end) {
        /// \todo needs optimization? maybe move rotate outside of the function?
        Matrix4x4 rotate = Geometry3D.RotationMatrix(begin, end, rotationSpeed);

        rotationSpeed = Mathf.Clamp(rotationSpeed, 0f, 90f - totalRotation);
        totalRotation = Mathf.Clamp(totalRotation + rotationSpeed, 0f, 90f);
        for (int i = 0; i < puzzleObject.transform.childCount; i++) {
            Transform cell = puzzleObject.transform.GetChild(i);
            for (int j = 0; j < cell.childCount; j++) {
                Transform sticker = cell.GetChild(j);
                if (toBeRotated[i][j] == true) {
                    puzzle.setSticker(i, j, rotate * puzzle.GetSticker(i, j)); /// \todo presentation
                    sticker.GetComponent<Coords4D>().SetCoordinates(puzzle.GetSticker(i, j));
                }
            }
        }
        return totalRotation;
    }

    /// <summary>
    /// Snaps each cell sticker to its final position
    /// </summary>
    public static void SnapToTargets(Puzzle puzzle, GameObject puzzleObject, List<List<Vector4>> targets, List<List<bool>> toBeRotated) {
        /// \todo delete puzzleObject?
        for (int i = 0; i < puzzle.NbCells(); i++) {
            Transform cell = puzzleObject.transform.GetChild(i);
            for (int j = 0; j < puzzle.NbStickers(i); j++) {
                Transform sticker = cell.GetChild(j);
                if (toBeRotated[i][j] == true) {
                    puzzle.setSticker(i, j, targets[i][j]);
                    sticker.GetComponent<Coords4D>().SetCoordinates(puzzle.GetSticker(i, j));
                }
            }
        }
    }

    public static void SnapToTargets(Puzzle3D puzzle, GameObject puzzleObject, List<List<Vector3>> targets, List<List<bool>> toBeRotated) {
        /// \todo delete puzzleObject?
        for (int i = 0; i < puzzle.NbCells(); i++) {
            Transform cell = puzzleObject.transform.GetChild(i);
            for (int j = 0; j < puzzle.NbStickers(i); j++) {
                Transform sticker = cell.GetChild(j);
                if (toBeRotated[i][j] == true) {
                    puzzle.setSticker(i, j, targets[i][j]);
                    sticker.GetComponent<Coords3D>().SetCoordinates(puzzle.GetSticker(i, j));
                }
            }
        }
    }

    public static void SetRotationSpeed(float f) {
        Animation.rotationSpeed = f;
    }
}