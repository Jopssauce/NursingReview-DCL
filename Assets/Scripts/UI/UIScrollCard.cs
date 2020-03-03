using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class UIScrollCard : MonoBehaviour, IPointerClickHandler
{
    public Image image;
    public DataCard CardData;
    public UIMainTopic uiMainTopic;

    bool isBack;
    bool isSelected;

    void OnEnable()
    {
        image.sprite = CardData.FrontFace;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        uiMainTopic.ActivateCardFace(CardData);
    }
}