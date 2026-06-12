using UnityEngine;
using UnityEngine.EventSystems;

public class LevelPageDragHandler : MonoBehaviour, IPointerDownHandler
{
    public System.Action<PointerEventData> OnSwipeLeft;
    public System.Action<PointerEventData> OnSwipeRight;

    public float dragThreshold = 80f;

    private bool _isDragging;
    private Vector2 _pointerDownPos;
    private int _pointerId = -1;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (_isDragging) return;

        _isDragging = true;
        _pointerId = eventData.pointerId;
        _pointerDownPos = eventData.position;
    }

    private void Update()
    {
        if (!_isDragging) return;

        Vector2 currentPos;
        bool isUp;

        ReadPointerState(out currentPos, out isUp);

        if (isUp)
        {
            _isDragging = false;
            _pointerId = -1;

            float deltaX = currentPos.x - _pointerDownPos.x;
            if (Mathf.Abs(deltaX) > dragThreshold)
            {
                if (deltaX > 0)
                    OnSwipeRight?.Invoke(null);
                else
                    OnSwipeLeft?.Invoke(null);
            }
        }
    }

    private void ReadPointerState(out Vector2 position, out bool isUp)
    {
        position = Vector2.zero;
        isUp = false;

#if UNITY_EDITOR || UNITY_STANDALONE
        position = Input.mousePosition;
        if (Input.GetMouseButtonUp(0))
        {
            isUp = true;
        }
        else if (!Input.GetMouseButton(0))
        {
            isUp = true;
            position = Input.mousePosition;
        }
#else
        if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.fingerId == _pointerId)
                {
                    position = touch.position;
                    isUp = touch.phase == TouchPhase.Ended
                        || touch.phase == TouchPhase.Canceled;
                    return;
                }
            }
        }
        isUp = true;
#endif
    }

    private void OnDisable()
    {
        ResetDragState();
    }

    private void OnApplicationFocus(bool hasFocus)
    {
        if (!hasFocus) ResetDragState();
    }

    private void ResetDragState()
    {
        _isDragging = false;
        _pointerId = -1;
    }
}
