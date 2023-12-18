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

    public void RegisterDraggedObject(DragObject drag) {
        _currentDraggedObject = drag;
        drag.transform.SetParent(_dragLayer);
    }

    public void UnregisterDraggedObject(DragObject drag) {
        drag.transform.SetParent(_defaultLayer);
        _currentDraggedObject = null;
    }

    public bool IsWithinBounds(Vector2 position) {
        return _boundingBox.Contains(position);
    }

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
