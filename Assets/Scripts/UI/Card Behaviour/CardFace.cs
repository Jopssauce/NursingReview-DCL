using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardFace : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    public DataCard CardData;
    public Image image;

    public UIMainTopic uiMainTopic;

    bool isBack;
    bool isDragged;

    void OnEnable()
    {
        image.sprite = CardData.FrontFace;
        image.transform.localScale = Vector3.one;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(isDragged == false)SwitchFaces();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragged = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragged = false;
    }

    void SwitchFaces()
    {
        if (isBack)
        {
            image.sprite = CardData.FrontFace;
            isBack = false;
        }
        else
        {
            image.sprite = CardData.BackFace;
            isBack = true;
        }
    }
}
