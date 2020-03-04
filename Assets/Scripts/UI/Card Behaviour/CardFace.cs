using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

public class CardFace : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    public DataCard CardData;
    public Image FrontFace;
    public Image BackFace;
    public RawImage rawImage;

    public UIMainTopic uiMainTopic;

    bool isBack;
    bool isDragged;

    void OnEnable()
    {
        FrontFace.sprite = CardData.FrontFace;
        BackFace.sprite = CardData.BackFace;
        FrontFace.transform.localScale = Vector3.one;
        //FrontFace.transform.position += new Vector3(0, 200, 0);
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
            Sequence sequence = DOTween.Sequence();
            BackFace.gameObject.SetActive(true);
            Tween tween = BackFace.transform.DORotate(new Vector3(0, 0, 0), 0.3f);
            FrontFace.transform.DORotate(new Vector3(0, 180, 0), 0.3f);
            BackFace.transform.localScale = FrontFace.transform.localScale;

            if (tween.position % 2 == 0)
                FrontFace.gameObject.SetActive(false);
            //FrontFace.sprite = CardData.FrontFace;
            isBack = false;
        }
        else
        {
            FrontFace.gameObject.SetActive(true);
            Tween tween = FrontFace.transform.DORotate(new Vector3(0, 0, 0), 0.3f);
            BackFace.transform.DORotate(new Vector3(0, -180, 0), 0.3f);
            FrontFace.transform.localScale = BackFace.transform.localScale;

            if (tween.position % 2 == 0)
                BackFace.gameObject.SetActive(false);
            //FrontFace.sprite = CardData.BackFace;
            isBack = true;
        }
    }
}
