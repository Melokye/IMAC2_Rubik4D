using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectCell : MonoBehaviour {
    [SerializeField]
    private Coords4D coords4D;
    private Renderer rend;
    private static Color hoverColor = new Color(0.05f, 0.05f, 0.05f, 0.125f);
    private Color selectColor;
    private static Color selectHoverColor = new Color(0.0425f, 0.026875f, 0f, 0.375f);

    private static GameManager handler;

    /// <summary>
    /// Defines the state of the object
    /// </summary>
    public enum State {
        Idle,
        Hovered,
        Selected,
        SelectedHovered
    }

    private State _state;

    // Start is called before the first frame update
    void Start() {
        _state = State.Idle;
        coords4D = this.gameObject.GetComponent<Coords4D>();
        handler = GameObject.Find("PuzzleGenerator").GetComponent<GameManager>();
        rend = GetComponent<Renderer>();
        selectColor = rend.material.color;
        rend.material.color = GetBaseColor();
    }
    
    /// <summary>
    /// A Raycasting Function to temporary hover the user's selection.
    /// Changes the color of hovered sticker. Currently the selection is yellow.
    /// </summary>
    void OnMouseOver() {
        switch (_state) {
            case State.Idle:
                SetState(State.Hovered);
                break;
            case State.Selected:
                SetState(State.SelectedHovered);
                break;
        }
    }

    /// <summary>
    /// A Raycasting Function to visually unhover the precedent selection.
    /// </summary>
    void OnMouseExit() {
        switch (_state) {
            case State.Hovered:
                SetState(State.Idle);
                break;
            case State.SelectedHovered:
                SetState(State.Selected);
                break;
        }
    }

    /// <summary>
    /// A Raycasting, onClick function to permanently hover the user's selection.
    /// </summary>
    void OnMouseDown() {
        switch (_state) {
            case State.Hovered:
                SetState(State.Selected);
                break;
            case State.SelectedHovered:
                SetState(State.Idle);
                break;
        }
    }

    /// <summary>
    /// Sets a new state to the object, and executes one thing if the new state is different
    /// </summary>
    /// <param name="newState"></param>
    public void SetState(State newState) {
        // check if already in the new state
        if (_state == newState)
            return;

        Coords4D currentlySelectedCoords = handler.GetSelection();
        SelectCell currentlySelected = null;
        if (currentlySelectedCoords != null) {
            currentlySelected = currentlySelectedCoords.GetComponent<SelectCell>();
        }

        // calls functions that only fire once on state change
        switch (newState) {
            case State.Idle:
                if (_state == State.SelectedHovered) {
                    handler.SetSelection(null);
                }
                rend.enabled = false;
                rend.material.color = GetBaseColor();
                break;
            case State.Hovered:
                rend.enabled = true;
                rend.material.color = hoverColor;
                break;
            case State.Selected:
                if (_state == State.Hovered) {
                    if (currentlySelected != null) currentlySelected.SetState(State.Idle);
                    handler.SetSelection(coords4D);
                }
                rend.material.color = selectColor;
                break;
            case State.SelectedHovered:
                rend.material.color = selectHoverColor;
                break;
        }

        // set the new _state
        _state = newState;
    }

    /// <summary>
    /// Handle the deselection, when clicking away.
    /// </summary>
    void Update() { }

    /// <summary>
    /// Getter of the original color of the sticker, before hovering.
    /// </summary>
    /// <returns></returns>
    public Color GetBaseColor() {
        return selectColor;
    }

    /// <summary>
    /// A simple setter of the original color of the sticker, without hovering.
    /// </summary>
    /// <param name="col"> A Unity.color to set from. </param>
    public void SetBaseColor(Color col) {
        selectColor = col;
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
