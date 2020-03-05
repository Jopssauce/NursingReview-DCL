using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using DG.Tweening;

public class UISubTopicMask : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI TextMeshProUGUI;
    public Image image;
    public CanvasGroup CanvasGroup;

    [Range(0f,1f)]
    public float EnterFade = 1;
    [Range(0f, 1f)]
    public float ExitFade = 0;
    [Range(0f, 4f)]
    public float EnterFadeTime = 0.3f;
    [Range(0f, 4f)]
    public float ExitFadeTime = 0.3f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        CanvasGroup.DOFade(EnterFade, EnterFadeTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CanvasGroup.DOFade(ExitFade, ExitFadeTime);
    }
}
