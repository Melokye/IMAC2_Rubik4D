using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCell : MonoBehaviour {
    [SerializeField]
    private Coords4D coords4D;
    private Renderer rend;
    private Color baseColor;
    private static bool hovered;
    private static Color hoverColor = new Color(0f, 0f, 0f, 0.125f);

    private static GameManager handler;
    
    
    // Start is called before the first frame update
    void Start() {
        coords4D = this.gameObject.GetComponent<Coords4D>();
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
        rend.material.color = hoverColor;
        SelectCell.hovered = true;
    }

    /// <summary>
    /// A Raycasting Function to visually unhover the precedent selection.
    /// </summary>
    void OnMouseExit() {
        if (handler.GetSelection() != this.coords4D) {
            rend.material.color = baseColor;
            rend.enabled = false;
        }
        SelectCell.hovered = false;
    }

    /// <summary>
    /// A Raycasting, onClick function to permanently hover the user's selection.
    /// </summary>
    void OnMouseDown() {
        if (handler.GetSelection() != null && handler.GetSelection().GetComponent<SelectCell>() != null) {
            SelectCell tmp = handler.GetSelection().gameObject.GetComponent<SelectCell>();
            tmp.rend.enabled = false;
            tmp.rend.material.color = tmp.baseColor;
            handler.SetterSelection(tmp.coords4D);
        }
        rend.enabled = true;
        rend.material.color = baseColor;
        handler.SetterSelection(this.coords4D);
        Coords4D.selectedCoordinates = this.coords4D.GetCoordinates();
    }

    /// <summary>
    /// Handle the deselection, when clicking away.
    /// </summary>
    void Update() {
        if (!SelectCell.hovered && handler.GetSelection() != null) {
            if (handler.GetSelection() != null) {
                SelectCell tmp = handler.GetSelection().gameObject.GetComponent<SelectCell>();
                if (tmp != null) {
                    tmp.rend.material.color = tmp.baseColor;
                }
            }
            else {
                rend.enabled = false;
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
