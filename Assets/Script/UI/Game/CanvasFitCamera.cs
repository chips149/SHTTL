using UnityEngine;

public class CanvasFitCamera : MonoBehaviour
{
    public float referenceWidth = 1080f;
    public float referenceHeight = 1920f;

    private void Start()
    {
        Camera cam = Camera.main;
        float worldHeight = cam.orthographicSize * 2f;
        float worldWidth = worldHeight * cam.aspect;

        RectTransform rect = GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, worldWidth);
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, worldHeight);
    }
}
