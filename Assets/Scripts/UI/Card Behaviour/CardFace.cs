using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardFace : MonoBehaviour, IPointerClickHandler
{
    public DataCard CardData;
    public Image image;

    bool IsBack;

    void OnEnable()
    {
        image.sprite = CardData.FrontFace;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SwitchFaces();
    }

    void SwitchFaces()
    {
        if (IsBack)
        {
            image.sprite = CardData.FrontFace;
            IsBack = false;
        }
        else
        {
            image.sprite = CardData.BackFace;
            IsBack = true;
        }
    }
}
