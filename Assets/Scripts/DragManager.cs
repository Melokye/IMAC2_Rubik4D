using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragManager : MonoBehaviour {
    [SerializeField]
    private RectTransform
        _defaultLayer = null,
        _dragLayer = null;

    private Rect _boundingBox;

    private DragObject _currentDraggedObject = null;
    public DragObject CurrentDraggedObject => _currentDraggedObject;

    private void Awake() {
        SetBoundingBoxRect(_dragLayer);
    }

    /// <summary>
    /// Move dragged object to the dragLayer parent
    /// </summary>
    /// <param name="drag"></param>
    public void RegisterDraggedObject(DragObject drag) {
        _currentDraggedObject = drag;
        drag.transform.SetParent(_dragLayer);
    }

    /// <summary>
    /// Move released dragged object back to the defaultLayer parent
    /// </summary>
    /// <param name="drag"></param>
    public void UnregisterDraggedObject(DragObject drag) {
        drag.transform.SetParent(_defaultLayer);
        _currentDraggedObject = null;
    }

    /// <summary>
    /// Not exactly sure what this one does, but sure.
    /// Currently unused.
    /// </summary>
    /// <param name="position"></param>
    /// <returns></returns>
    public bool IsWithinBounds(Vector2 position) {
        return _boundingBox.Contains(position);
    }

    /// <summary>
    /// Another useless piece of code
    /// </summary>
    /// <param name="rectTransform"></param>
    private void SetBoundingBoxRect(RectTransform rectTransform) {
        var corners = new Vector3[4];
        rectTransform.GetWorldCorners(corners);
        var position = corners[0];

        Vector2 size = new Vector2(
            rectTransform.lossyScale.x * rectTransform.rect.size.x,
            rectTransform.lossyScale.y * rectTransform.rect.size.y);

        _boundingBox = new Rect(position, size);
    }
}
