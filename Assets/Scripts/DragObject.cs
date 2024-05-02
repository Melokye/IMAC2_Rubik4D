using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(RectTransform))]
public class DragObject : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler {
    private static GameManager handler;
    private DragManager _manager = null;
    private static bool _hovered = false;

    [SerializeField]
    private RawImage image;

    private Vector2 _centerPoint;
    private Vector2 _worldCenterPoint => transform.TransformPoint(_centerPoint);

    private const float inputBufferThreshold = 0.1f; // Time threshold at which a click becomes a hold
    private float inputBuffer = inputBufferThreshold;
    private bool recentering = false; // Determines whether or not the view is being recentered

    private void Awake() {
        _manager = GetComponentInParent<DragManager>();
        _centerPoint = (transform as RectTransform).rect.center;
    }

    private void Start() {
        handler = GameObject.Find("PuzzleGenerator").GetComponent<GameManager>();
    }

    private void Update() {
        CameraUIRecenteringHandler();
        ZoomHandler();
    }

    /// <summary>
    /// Handles recentering the image in the CameraUI view.
    /// Press Middle click quickly within the window to recenter the view.
    /// </summary>
    private void CameraUIRecenteringHandler() {
        // If Middle click is pressed, halts recentering
        if (Input.GetMouseButtonDown(2)) recentering = false;

        // If Middle click is being held, determine whether or not the hold is long enough
        if (Input.GetMouseButton(2)) inputBuffer += Time.deltaTime;

        // If Middle click is released, reset the inputBuffer
        if (Input.GetMouseButtonUp(2)) {
            // If the cursor is still over the CameraView and input was short, recenter image
            if (_hovered) recentering = inputBuffer < inputBufferThreshold;
            inputBuffer = 0f;
        }
        if (recentering) CenterView();
    }

    /// <summary>
    /// Determines which camera is going to be affected by the scroll wheel, then zoom in/out
    /// </summary>
    private void ZoomHandler() {
        Camera focusedCamera = null;
        if (_hovered) {
            focusedCamera = FindCameraByName("UICamera");
        }
        else {
            focusedCamera = FindCameraByName("MainCamera");
        }
        Vector3 newCameraPosition;
        if (Input.touchCount == 2) {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;
            newCameraPosition = focusedCamera.transform.localPosition - 0.3f * focusedCamera.transform.forward * difference;
        }
        else {
            newCameraPosition = focusedCamera.transform.localPosition - 30f * focusedCamera.transform.forward * Input.GetAxis("Mouse ScrollWheel");
        }
        float newDistance = Vector3.Distance(newCameraPosition, Vector3.zero);
        if (Mathf.Abs(Mathf.Clamp(newDistance, 5f, 200f) - newDistance) < Mathf.Epsilon) {
            focusedCamera.transform.localPosition = newCameraPosition;
        }
    }

    /// <summary>
    /// Center image in CameraUI view
    /// </summary>
    private void CenterView() {
        image.GetComponent<RectTransform>().anchoredPosition = new Vector2(
            Mathf.Lerp(image.GetComponent<RectTransform>().anchoredPosition.x, 0, 5f * Time.deltaTime),
            Mathf.Lerp(image.GetComponent<RectTransform>().anchoredPosition.y, 0, 5f * Time.deltaTime));
        if (Vector2.Distance(image.GetComponent<RectTransform>().anchoredPosition,
            Vector2.zero) < 0.1f) recentering = false;
    }

    /// <summary>
    /// Find a camera object by its name
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    private Camera FindCameraByName(string name) {
        Camera cameraFound = null;
        foreach (Camera camera in handler.cameraArray) {
            if (camera.gameObject.name == name) {
                cameraFound = camera;
            }
        }
        return cameraFound;
    }

    /// <summary>
    /// Find a camera object by its culling layer
    /// </summary>
    /// <param name="layerName"></param>
    /// <returns></returns>
    private Camera FindCameraByCullingLayer(string layerName) {
        Camera cameraFound = null;
        foreach (Camera camera in handler.cameraArray) {
            if (camera.cullingMask == 1 << LayerMask.NameToLayer(layerName)) {
                cameraFound = camera;
            }
        }
        return cameraFound;
    }

    /// <summary>
    /// Triggers upon entering the mouse in the CameraUI view
    /// </summary>
    public void OnMouseEnterUI() {
        _hovered = true;
    }

    /// <summary>
    /// Triggers upon exiting the mouse from the CameraUI view
    /// </summary>
    public void OnMouseExitUI() {
        _hovered = false;
    }

    /// <summary>
    /// Triggers upon dragging starts
    /// </summary>
    /// <param name="eventData"></param>
    public void OnBeginDrag(PointerEventData eventData) {
        _manager.RegisterDraggedObject(this);
    }

    /// <summary>
    /// Runs during dragging of the object.
    /// If hold Right click on CameraUI view, moves it across the screen.
    /// If hold Middle click on CameraUI view, pans the image inside.
    /// </summary>
    /// <param name="eventData"></param>
    public void OnDrag(PointerEventData eventData) {
        // Right click hold
        if (Input.GetMouseButton(1)) {
            transform.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
        }
        // Middle click hold
        if (Input.GetMouseButton(2)) {
            image.GetComponent<RectTransform>().anchoredPosition += eventData.delta;
        }
    }

    /// <summary>
    /// Triggers upon dragging ends
    /// </summary>
    /// <param name="eventData"></param>
    public void OnEndDrag(PointerEventData eventData) {
        _manager.UnregisterDraggedObject(this);
    }
}
