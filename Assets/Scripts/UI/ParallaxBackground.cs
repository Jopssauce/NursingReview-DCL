using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ParallaxBackground : MonoBehaviour
{
    public int scrollSpeed = 100;
    public bool Reverse = true;

    RectTransform rectTransform;
    Vector2 newPos;
    Vector2 mousePos;
    Vector2 direction;
    float distance;

    int maxWidth;
    int maxHeight;

    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        maxWidth = (int)rectTransform.sizeDelta.x / 4;
        maxHeight = (int)rectTransform.sizeDelta.y / 4;
    }

    private void Update()
    {
        CaluclatePosition();
    }

    void CaluclatePosition()
    {
        mousePos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        direction = mousePos - new Vector2(0.5f, 0.5f);
        distance = direction.magnitude;
        CalculateScroll();


    }

    void CalculateScroll()
    {
        if(Reverse)newPos = new Vector2(maxWidth, maxHeight) * (-direction) * distance;
        else
        {
            newPos = new Vector2(maxWidth, maxHeight) * (direction) * distance;
        }
        rectTransform.anchoredPosition = Vector2.Lerp(rectTransform.anchoredPosition, newPos, Time.deltaTime * scrollSpeed);
    }
}
