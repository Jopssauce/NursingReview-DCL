using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

[RequireComponent(typeof(Button))]
public class UIButtonSelectable : UIButton, IPointerEnterHandler, IPointerExitHandler
{
    public DataTopic TopicData;
    public TextMeshProUGUI TextMeshProUGUI;

    public Color Default;
    public Color Highlighted;

    public Sprite SpriteDefault;
    public Sprite SpriteHighlighted;

    UIMainTopic uiMainTopic;
    Image image;

    bool isSelected = false;

    public override void Awake()
    {
        base.Awake();
        uiMainTopic = GetComponentInParent<UIMainTopic>();
    }

    void OnEnable()
    {
        image = GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isSelected == false && TextMeshProUGUI != null) TextMeshProUGUI.color = Highlighted;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (isSelected == false && TextMeshProUGUI != null) TextMeshProUGUI.color = Default;
    }

    public void SelectAction()
    {
        if (TextMeshProUGUI != null) TextMeshProUGUI.color = Highlighted;
        isSelected = true;
        image.sprite = SpriteHighlighted;
        image.fillAmount = 0;
        image.DOFillAmount(1, 0.7f);
    }

    public void DeselectAction()
    {
        if (TextMeshProUGUI != null) TextMeshProUGUI.color = Default;
        isSelected = false;
        image.sprite = SpriteDefault;
        image.fillAmount = 1;
    }
}
