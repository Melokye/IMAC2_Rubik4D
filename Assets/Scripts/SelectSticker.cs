using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectSticker : MonoBehaviour
{
    private Renderer rend;
    private Color baseColor;
    private static bool hovered;

    private static GameManager handler;
    // Start is called before the first frame update
    void Start()
    {
        SelectSticker.handler = GameObject.Find("PuzzleGenerator").GetComponent<GameManager>();
        rend = GetComponent<Renderer>();
        baseColor = rend.material.color;
    }
    void OnMouseOver(){
        rend.material.color = Color.yellow;
        SelectSticker.hovered = true;
    }

    void OnMouseExit(){
        if(handler.GetSelection()!=this){
            rend.material.color = baseColor;
        }
        SelectSticker.hovered = false;
    }

    void OnMouseDown(){
        if(handler.GetSelection() != null){
            SelectSticker tmp = handler.GetSelection();
            tmp.rend.material.color = tmp.baseColor;
            handler.setterSelection(tmp);
        }
        rend.material.color = Color.yellow;
        handler.setterSelection(this);
    }

    void Update(){
        if(!SelectSticker.hovered & 
            Input.GetMouseButtonDown(0))
        {
            SelectSticker tmp = handler.GetSelection();
            tmp.rend.material.color = tmp.baseColor;
            handler.setterSelection(tmp);
            handler.setterSelection(null);
        }
    }
    public Color GetBaseColor(){
        return baseColor;
    }
}
