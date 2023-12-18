using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCell : MonoBehaviour {
    [SerializeField]
    private Vector4 coordinates;
    public static Vector4 selectedCoordinates;
    private Renderer rend;
    private Color baseColor;
    private static bool hovered;

    private static GameManager handler;
    
    
    // Start is called before the first frame update
    void Start() {
        SelectCell.handler = GameObject.Find("PuzzleGenerator").GetComponent<GameManager>();
        rend = GetComponent<Renderer>();
        baseColor = rend.material.color;
    }
    
    /// <summary>
    /// A Raycasting Function to temporary hover the user's selection.
    /// Changes the color of hovered sticker. Currently the selection is yellow.
    /// </summary>
    void OnMouseOver() {
        rend.enabled = true;
        rend.material.color = Color.black;
        SelectCell.hovered = true;
    }

    /// <summary>
    /// A Raycasting Function to visually unhover the precedent selection.
    /// </summary>
    void OnMouseExit() {
        if (handler.GetSelection() != this) {
            rend.material.color = baseColor;
            rend.enabled = false;
        }
        SelectCell.hovered = false;
    }

    /// <summary>
    /// A Raycasting, onClick function to permanently hover the user's selection.
    /// </summary>
    void OnMouseDown() {
        if (handler.GetSelection() != null) {
            SelectCell tmp = handler.GetSelectionCell();
            tmp.rend.enabled = false;
            tmp.rend.material.color = tmp.baseColor;
            handler.SetterSelectionCell(tmp);
        }
        rend.enabled = true;
        rend.material.color = Color.yellow;
        handler.SetterSelectionCell(this);
        SelectCell.selectedCoordinates = this.coordinates;
    }

    /// <summary>
    /// Handle the deselection, when clicking away.
    /// </summary>
    void Update() {
        if (!SelectCell.hovered && Input.GetMouseButtonDown(0)) {
            SelectCell tmp = handler.GetSelectionCell();
            if (tmp != null) {
                tmp.rend.material.color = tmp.baseColor;
                handler.SetterSelectionCell(tmp);
            }
        }
    }
    
    /// <summary>
    /// Getter of the original color of the sticker, before hovering.
    /// </summary>
    /// <returns></returns>
    public Color GetBaseColor() {
        return baseColor;
    }

    /// <summary>
    /// A simple setter of the original color of the sticker, without hovering.
    /// </summary>
    /// <param name="col"> A Unity.color to set from. </param>
    public void SetBaseColor(Color col) {
        baseColor = col;
    }

    /// <summary>
    /// Getter of the renderer of the Unity.gameObject.
    /// </summary>
    /// <returns></returns>
    public Renderer GetRend() {
        return rend;
    }

    /// <summary>
    /// Setter of the renderer of the Unity.gameObject.
    /// </summary>
    /// <param name="Rend"> The mesh renderer of the Unity.gameObject. </param>
    public void SetRend(Renderer Rend) {
        rend = Rend;
    }

    /// <summary>
    /// Getter of the 4D coordinates of the sticker.
    /// </summary>
    /// <returns></returns>
    public Vector4 GetCoordinates() {
       return coordinates;
    }

    /// <summary>
    /// Setter of the 4D coordinates of the sticker.
    /// </summary>
    /// <param name="Coordinates"> A Unity.Vector4 to set from. </param>
    public void SetCoordinates(Vector4 Coordinates) {
        coordinates = Coordinates;
    }
}
