using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CardFace : MonoBehaviour, IPointerClickHandler
{
    public DataCard CardData;
    public Image image;

    public UIMainTopic uiMainTopic;

    bool isBack;
    bool isSelected;

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
