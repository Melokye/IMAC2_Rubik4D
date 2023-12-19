using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSticker : MonoBehaviour {
    [SerializeField]
    private Coords4D coords4D;
    private Renderer rend;
    private Color baseColor;
    private static bool hovered;
    private static Color selectColor = Color.yellow;

    private static GameManager handler;
    
    
    // Start is called before the first frame update
    void Start() {
        coords4D = this.gameObject.GetComponent<Coords4D>();
        SelectSticker.handler = GameObject.Find("PuzzleGenerator").GetComponent<GameManager>();
        rend = GetComponent<Renderer>();
        baseColor = rend.material.color;
    }
    
    /// <summary>
    /// A Raycasting Function to temporary hover the user's selection.
    /// Changes the color of hovered sticker. Currently the selection is yellow.
    /// </summary>
    void OnMouseOver() {
        rend.material.color = selectColor;
        SelectSticker.hovered = true;
    }

    /// <summary>
    /// A Raycasting Function to visually unhover the precedent selection.
    /// </summary>
    void OnMouseExit() {
        if (handler.GetSelection() != this.coords4D) {
            rend.material.color = baseColor;
        }
        SelectSticker.hovered = false;
    }

    /// <summary>
    /// A Raycasting, onClick function to permanently hover the user's selection.
    /// </summary>
    void OnMouseDown() {
        if (handler.GetSelection() != null && handler.GetSelection().GetComponent<SelectSticker>() != null) {
            SelectSticker tmp = handler.GetSelection().gameObject.GetComponent<SelectSticker>();
            tmp.rend.material.color = tmp.baseColor;
            handler.SetterSelection(tmp.coords4D);
        }
        rend.material.color = selectColor;
        handler.SetterSelection(this.coords4D);
        Coords4D.selectedCoordinates = this.coords4D.GetCoordinates();
    }

    /// <summary>
    /// Handle the deselection, when clicking away.
    /// </summary>
    void Update() {
        if (!SelectSticker.hovered && handler.GetSelection() != null) {
            SelectSticker tmp = handler.GetSelection().gameObject.GetComponent<SelectSticker>();
            if (tmp != null) {
                tmp.rend.material.color = selectColor;
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
}
