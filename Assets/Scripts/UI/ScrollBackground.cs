using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollBackground : MonoBehaviour
{
    public float speed = 1f;
    public float offset;

    RectTransform rectTransform;
    Vector2 startPos;
    float newPos;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        startPos = rectTransform.anchoredPosition;
    }

    private void Update()
    {
        newPos = Mathf.Repeat(Time.time * speed, offset);
        rectTransform.anchoredPosition = startPos + Vector2.right * newPos;
    }

}
