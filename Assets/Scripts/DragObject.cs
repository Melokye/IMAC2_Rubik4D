using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    private static GameManager handler;
    private DragManager _manager = null;
    private static bool _hovered = false;

    private Vector2 _centerPoint;
    private Vector2 _worldCenterPoint => transform.TransformPoint(_centerPoint);

    private void Awake() {
        _manager = GetComponentInParent<DragManager>();
        _centerPoint = (transform as RectTransform).rect.center;
    }

    private void Start() {
        handler = GameObject.Find("PuzzleGenerator").GetComponent<GameManager>();
    }

    private void Update() {
        Camera focusedCamera = null;
        if (_hovered) {
            focusedCamera = FindCameraByName("UICamera");
            focusedCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 20f;
        }
        else {
            focusedCamera = FindCameraByName("MainCamera");
            focusedCamera.fieldOfView -= Input.GetAxis("Mouse ScrollWheel") * 20f;
        }
    }

    private Camera FindCameraByName(string name) {
        Camera cameraFound = null;
        foreach (Camera camera in handler.cameraArray) {
            if (camera.gameObject.name == name) {
                cameraFound = camera;
            }
        }
        return cameraFound;
    }

    private Camera FindCameraByCullingLayer(string layerName) {
        Camera cameraFound = null;
        foreach (Camera camera in handler.cameraArray) {
            if (camera.cullingMask == 1 << LayerMask.NameToLayer(layerName)) {
                cameraFound = camera;
            }
        }
        return cameraFound;
    }

    public void OnMouseEnterUI() {
        _hovered = true;
    }

    public void OnMouseExitUI() {
        _hovered = false;
    }

    public void OnBeginDrag(PointerEventData eventData) {
        _manager.RegisterDraggedObject(this);
    }

    public void OnDrag(PointerEventData eventData) {
        if (!Input.GetMouseButton(0))
            transform.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
    }

    public void OnEndDrag(PointerEventData eventData) {
        _manager.UnregisterDraggedObject(this);
    }
}
