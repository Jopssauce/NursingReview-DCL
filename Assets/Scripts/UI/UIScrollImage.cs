using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScrollImage : MonoBehaviour, IScrollHandler
{
    public float ZoomSpeed = 0.1f;
    public float MaxZoom = 10f;

    Vector3 initialScale;

    private void Awake()
    {
        initialScale = transform.localScale;
    }

    public void OnScroll(PointerEventData eventData)
    {

        Vector3 delta = Vector3.one * (eventData.scrollDelta.y * ZoomSpeed);
        Vector3 desiredScale = transform.localScale + delta;

        desiredScale = ClampedDesiredScale(desiredScale);
        transform.localScale = desiredScale;
    }

    Vector3 ClampedDesiredScale(Vector3 scale)
    {
        scale = Vector3.Max(initialScale, scale);
        scale = Vector3.Min(initialScale * MaxZoom, scale);
        return scale;
    }
}
